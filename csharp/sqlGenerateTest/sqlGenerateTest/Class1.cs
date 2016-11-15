using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using sqlGenerateTest.Contact;
using ZMS.BLL.OA.IApprove;

namespace sqlGenerateTest {
	public class Class1 {

		public class TestAllType {
			public int Int { get; set; }
			public int? IntNullable { get; set; }
			public bool Bool { get; set; }
			public bool? BoolNullable { get; set; }
			public string String { get; set; }
			public DateTime Time{ get; set; }
			public DateTime? TimeNullable{ get; set; }
			public decimal Decimal { get; set; }
			public decimal? DecimalNullable { get; set; }
		}
		public static dynamic getApproveList2(int cid, ApproveParam param) {
			if ( !param.IsValid() ) {
				return null;
			}
			bool hasList = false;
			var sqlparam = new ApproveStatisticBaseParam();
			var app = new oa_ApproveInfo();
			Type t = app.GetType();
			Type form = null;
			switch ( param.Type ) {
				case FormTypeEnum.Leave:
					form = new LeaveForm().GetType();
					break;
				case FormTypeEnum.Reimbursement:
					form = new ReimbursementForm().GetType();
					break;
				case FormTypeEnum.GoodsApply:
					form = new GoodsApplyForm().GetType();

					break;
				case FormTypeEnum.WorkInstructions:
					form = new WorkInstructionsForm().GetType();

					break;
				case FormTypeEnum.BusinessLeave:
					form = new BusinessLeaveForm().GetType();

					break;
				case FormTypeEnum.GoOut:
					form = new GoOutForm().GetType();

					break;
				case FormTypeEnum.Purchase:
					form = new PurchaseForm().GetType();
					PurchaseItem sub7 = new PurchaseItem();
					sqlparam.subListWhere.Add(new WhereItem() { Column = "[List.GoodsNum]", DataType = sub7.GoodsNum.GetType(), IsJson = false, Oprate = OprateEnum.Between, param1 = 0, param2 = 100 });

					break;
				case FormTypeEnum.Payment:
					form = new PaymentForm().GetType();

					break;
				case FormTypeEnum.UseSeal:
					form = new UseSealForm().GetType();

					break;
				case FormTypeEnum.BecomeFull:
					form = new BecomeFullForm().GetType();

					break;
				case FormTypeEnum.LeaveOffice:
					form = new LeaveOfficeForm().GetType();

					break;
				case FormTypeEnum.ExtraWork:
					form = new ExtraWorkForm().GetType();

					break;
				default:
					break;
			}
			foreach ( PropertyInfo pi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) )
				sqlparam.selectList.Add(new SelectItem() { Column = pi.Name, ColumnAsName = pi.Name, DataType = pi.PropertyType, IsJson = false });
			if ( form != null )
				foreach ( PropertyInfo pi in form.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) )
					if ( pi.Name != "Total" && pi.Name != "List" )
						sqlparam.selectList.Add(new SelectItem() { Column = pi.Name, ColumnAsName = "[Property." + pi.Name + "]", DataType = pi.PropertyType, IsJson = true });
					else {
						if ( pi.Name == "Total" ) {
							Type total = pi.PropertyType;
							foreach ( PropertyInfo info in total.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) )
								sqlparam.selectList.Add(new SelectItem() { Column = info.Name, ColumnAsName = "[Total." + info.Name + "]", DataType = info.PropertyType, IsJson = true });
						}
						else if ( pi.Name == "List" ) {
							hasList = true;
							Type list = pi.PropertyType;
							if ( list.IsGenericType ) {
								Type sub = list.GetGenericArguments() [0];
								foreach ( PropertyInfo info in sub.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) )
									sqlparam.subListSelect.Add(new SelectItem() { Column = "[List." + info.Name + "]", ColumnAsName = "[List." + info.Name + "]", DataType = info.PropertyType, IsJson = false });
							}
						}
					}
			sqlparam.whereList.Add(new WhereItem() { Column = "ZMSCompanyID", DataType = app.ZMSCompanyID.GetType(), IsJson = false, Oprate = OprateEnum.等于, param1 = cid });
			sqlparam.whereList.Add(new WhereItem() { Column = "Type", DataType = app.Type.GetType(), IsJson = false, Oprate = OprateEnum.等于, param1 = param.Type });

			if ( param.State > -1 )
				sqlparam.whereList.Add(new WhereItem() { Column = "State", DataType = app.State.GetType(), IsJson = false, Oprate = OprateEnum.等于, param1 = param.State });
			if ( param.DepartID [0] > 0 )
				sqlparam.whereList.Add(new WhereItem() { Column = "DepartmentID", DataType = app.DepartmentID.GetType(), IsJson = false, Oprate = OprateEnum.包含, param1 = param.DepartID });
			if ( param.PositionID [0] > 0 )
				sqlparam.whereList.Add(new WhereItem() { Column = "PositionID", DataType = app.PositionID.GetType(), IsJson = false, Oprate = OprateEnum.包含, param1 = param.PositionID });
			if ( param.Uid [0] > 0 )
				sqlparam.whereList.Add(new WhereItem() { Column = "UserID", DataType = app.UserID.GetType(), IsJson = false, Oprate = OprateEnum.包含, param1 = param.Uid });

			sqlparam.sortList.Add(new SortItem() { ColumnName = "CreateTime", IsAsc = false, DataType = app.CreateTime.GetType(), IsJson = false, Priority = 3 });
			return getApproveStatistic(sqlparam, hasList, param.IsNeedSplitList, ( int ) param.Type);
		}
		private static dynamic getApproveStatistic(ApproveStatisticBaseParam param, bool hasList = false, bool isNeedList = true, int type = 0) {
			try {
				string sql = @" select top({0}) 
										{2} {9}
										from 
										(select 
										{2} {9}
										,row_number() OVER ({6}) AS [row_number] 
										from 
										(select
										{3} {8}
										from 
										{4} as[Extent1] {7}
										where 
										{5}) as [Project1]
										) as [Project1]
										where [row_number]>{0}*{1}
										{6}";
				sql = string.Format(sql,
					param.PageSize,
					param.PageIndex, 
					generateSelect("[Project1].",param.selectList),
					generateSelect("[Extent1].", param.selectList), 
					" oa_ApproveInfo",
				 generateWhere("[Extent1].", param.whereList)+ ( hasList && param.subListWhere.Count() > 0 ? ( param.whereList.Count() > 0 ? " and " : "" ) + ( isNeedList ? generateWhere("sublist.", param.subListWhere) : generateListWhere("sublist.", param.subListWhere) ) : "" ),
					generateOrder("[Project1].", param.sortList), 
					hasList && isNeedList ? " CROSS APPLY dbo.appListSub" + ( int ) type + "([Extent1].JsonContent) as sublist" : "",
					hasList && isNeedList ? "," + generateSelect("sublist.", param.subListSelect) : "",
					hasList && isNeedList ? "," + generateSelect("[Project1].", param.subListSelect) : "");

				//var dataTable = factory.GetApproveContext().SqlQueryForDataTatable(sql);
				//var list = JsonConvert.SerializeObject(dataTable);
				return sql;
			}
			catch ( Exception ex ) { }
			return null;
		}
		private static string generateSelect(string str,List<SelectItem> list,bool isListWhere=false) {
			if ( list != null && list.Count() > 0 ) {
				string select = " ";
				list.ForEach(p => {
					string columnName = p.Column;
					if ( list.IndexOf(p) != 0 )
						select += ",";
					if ( p.IsJson ) {
						if ( str != "[Project1]." ) {
							if ( p.DataType.IsGenericType && p.DataType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) )
								columnName = ColumnRename(p.DataType.GetGenericArguments() [0].Name, str, columnName, isListWhere, true);
							else
								columnName = ColumnRename(p.DataType.Name, str, columnName, isListWhere);
						}
						else {
							columnName = str + p.ColumnAsName;
						}
					}
					else {
						columnName = str + columnName;
					}
					select += columnName + " as " + p.ColumnAsName;
				});
				return select;
			}

			return " * ";
		}
		private static string generateWhere(string str, List<WhereItem> list) {
			if ( list != null && list.Count() > 0 ) {
				string where = " 1=1 ";
				list.ForEach(p => {
					string columnName = p.Column;
					if ( p.IsJson ) {
						if ( p.DataType.IsGenericType && p.DataType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) )
							columnName = ColumnRename(p.DataType.GetGenericArguments() [0].Name, str, columnName, false,true, p.Oprate == OprateEnum.IsNull);
						else
							columnName = ColumnRename(p.DataType.Name, str, columnName);
					}
					else {
						columnName = str + columnName;
					}
					switch ( p.DataType.Name ) {
						case "Boolean":
							switch ( p.Oprate ) {
								case OprateEnum.等于:
									where += string.Format(" and {0}=1");
									break;
								case OprateEnum.IsNull:
									where += string.Format(" and {0} is null");
									break;
								default:
									break;
							}
							break;
						case "Int16":
						case "Int32":
						case "Int64":
							switch ( p.Oprate ) {
								case OprateEnum.等于:
									where += Extensions_StringSql.IntEquals("", Convert.ToInt32(p.param1), columnName);
									break;
								case OprateEnum.大于:
									where += Extensions_StringSql.IntBigger("", Convert.ToInt32(p.param1), columnName);
									break;
								case OprateEnum.小于:
									where += Extensions_StringSql.IntSmaller("", Convert.ToInt32(p.param1), columnName);
									break;
								case OprateEnum.包含:
									where += Extensions_StringSql.IntContains("", p.param1 as int [ ], columnName);
									break;
								case OprateEnum.不等于:
									where += Extensions_StringSql.IntNotEquals("", Convert.ToInt32(p.param1), columnName);
									break;
								case OprateEnum.Between:
									where += Extensions_StringSql.IntEqualsOrBetween("", Convert.ToInt32(p.param1), Convert.ToInt32(p.param2), columnName);
									break;
								case OprateEnum.IsNull:
									where += string.Format(" and {0} is null");
									break;
								default:
									break;
							}
							break;
						case "Decimal":
							switch ( p.Oprate ) {
								case OprateEnum.等于:
									where += Extensions_StringSql.DecimalEquals("", Convert.ToDecimal(p.param1), columnName);
									break;
								case OprateEnum.大于:
									where += Extensions_StringSql.DecimalBigger("", Convert.ToDecimal(p.param1), columnName);
									break;
								case OprateEnum.小于:
									where += Extensions_StringSql.DecimalSmaller("", Convert.ToDecimal(p.param1), columnName);
									break;
								case OprateEnum.包含:
									where += Extensions_StringSql.DecimalContains("", p.param1 as decimal [ ], columnName);
									break;
								case OprateEnum.不等于:
									where += Extensions_StringSql.DecimalNotEquals("", Convert.ToDecimal(p.param1), columnName);
									break;
								case OprateEnum.Between:
									where += Extensions_StringSql.DecimalEqualsOrBetween("", Convert.ToDecimal(p.param1), Convert.ToDecimal(p.param2), columnName);
									break;
								case OprateEnum.IsNull:
									where += string.Format(" and {0} is null");
									break;
								default:
									break;
							}
							break;
						case "String":
							switch ( p.Oprate ) {
								case OprateEnum.等于:
									where += Extensions_StringSql.strEquals("", p.param1 as string, columnName);
									break;
								case OprateEnum.包含:
									where += Extensions_StringSql.strEndLike("", p.param1 as string, columnName);
									break;
								case OprateEnum.IsNull:
									where += string.Format(" and {0} is null");
									break;
								default:
									break;
							}
							break;
						case "DateTime":
							switch ( p.Oprate ) {
								case OprateEnum.等于:
									where += Extensions_StringSql.DtEquals("", Convert.ToDateTime(p.param1), columnName);
									break;
								case OprateEnum.大于:
									where += Extensions_StringSql.DtBigger("", Convert.ToDateTime(p.param1), columnName);
									break;
								case OprateEnum.小于:
									where += Extensions_StringSql.DtSmaller("", Convert.ToDateTime(p.param1), columnName);
									break;
								case OprateEnum.Between:
									where += Extensions_StringSql.DtEqualsOrBetween("", Convert.ToDateTime(p.param1), Convert.ToDateTime(p.param2), columnName);
									break;
								case OprateEnum.IsNull:
									where += string.Format(" and {0} is null");
									break;
								default:
									break;
							}
							break;
						default:
							break;
					}
				});
				return where;
			}
			return " 1=1 ";
		}
		private static string generateListWhere(string str, List<WhereItem> list) {
			string sql = @"(select top 1 1 from (
SELECT 
{0}
from
 dbo.parseJSON([Extent1].JsonContent)
 obj
where
 obj.parent_ID=(select list.Object_ID from dbo.parseJSON([Extent1].JsonContent) list where NAME='List')
) as subList
where {1}
)=1";
			var selectList = new List<SelectItem>();
			list.ForEach(p => {
				selectList.Add(new SelectItem() {  Column=p.Column.Replace("List.","").Replace("[","").Replace("]",""), ColumnAsName= p.Column, DataType=p.DataType, IsJson=true});
			});
			return string.Format(sql, generateSelect("[Extent1].", selectList, true), generateWhere("sublist.", list));
		}
		private static string generateOrder(string str,List<SortItem> list) {
			if ( list != null && list.Count() > 0 ) {
				string select = " order by ";
				list.OrderByDescending(p=>p.Priority).ToList().ForEach(p => {
					string columnName = p.ColumnName;
					if ( list.IndexOf(p) != 0 )
						select += ",";
					if ( p.IsJson ) {
						columnName = ColumnRename(p.DataType.Name, str, columnName);
					}
					else {
						columnName = str + columnName;
					}
					if ( !p.IsAsc ) {
						columnName += " desc";
					}
					select += columnName;
				});
				return select;
			}
			return " order by "+str+"CreateTime desc ";
		}
		private static string ColumnRename(string columnType,string str, string columnName,bool isListWhere=false,bool isNullable=false,bool isNull=false) {
			string sqltype;
			string nullData = @"''";
			switch ( columnType ) {
				case "Boolean":
					sqltype = "bit";
					nullData = "0";
					break;
				case "Int16":
				case "Int32":
				case "Int64":
					sqltype = "int";
					nullData = "0";
					break;
				case "Decimal":
					sqltype = "decimal(18,2)";
					nullData = "0.0";
					break;
				case "DateTime":
					sqltype = "datetime2";
					nullData = @"'2016/1/1'";
					break;
				case "String":
				default:
					sqltype = "nvarchar(1000)";
					break;
			}
			if (isNullable)
				if(isNull)
					return string.Format("(select StringValue from dbo.parseJSON({0}JsonContent) where name='{1}' {2})",  str, columnName,isListWhere? "and parent_ID=obj.Object_ID" : "");
				else
					return string.Format("(select convert({0},isnull(StringValue,{3})) from dbo.parseJSON({1}JsonContent) where name='{2}' {4})", sqltype, str, columnName,nullData, isListWhere ? "and parent_ID=obj.Object_ID" : "");
			else
				return string.Format("(select convert({0},StringValue) from dbo.parseJSON({1}JsonContent) where name='{2}' {3})", sqltype,str, columnName, isListWhere ? "and parent_ID=obj.Object_ID" : "");
		}


	}
}
