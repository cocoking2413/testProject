using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqlGenerateTest {
	public static partial class Extensions_StringSql {

		#region ------------String类型的扩展-----------

		/// <summary>
		/// return  " and UserId='Admin'"
		/// </summary>
		/// <param name="str"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.strEquals(txtUserId.text,"UserID")
		/// </example>
		public static string strEquals(this string strSql, string strValue, string ColName) {
			if ( !string.IsNullOrEmpty(strValue) )
				return string.Format(strSql + "  and {0}='{1}'  ", ColName, strValue);
			else
				return strSql;
		}

		/// <summary>
		/// return "  and VBELN > '107100000'  "
		/// </summary>
		/// <param name="str"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.strBigger(txtVBELB.text,"VBELN");
		/// </example>
		public static string strBigger(this string strSql, string strValue, string ColName) {
			if ( !string.IsNullOrEmpty(strValue) )
				return string.Format(strSql + " and {0}>'{1}' ", ColName, strValue);
			else
				return strSql;
		}

		/// <summary>
		/// return "  and VBELN < '107100000'  "
		/// </summary>
		/// <param name="str"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.strSmaler(txtVBELB.text,"VBELN");
		/// </example>
		public static string strSmaller(this string strSql, string strValue, string ColName) {
			if ( !string.IsNullOrEmpty(strValue) )
				return string.Format(strSql + " and {0}<'{1}' ", ColName, strValue);
			else
				return strSql;
		}

		/// <summary>
		/// return "  and UserName <like '%Tom%'  "
		/// </summary>
		/// <param name="str"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.strLike(txtUserName.text,"Tom");
		/// </example>
		public static string strLike(this string strSql, string strValue, string ColName) {
			if ( !string.IsNullOrEmpty(strValue) )
				return string.Format(strSql + " and {0} like '%{1}%' ", ColName, strValue);
			else
				return strSql;
		}

		/// <summary>
		/// return "  and UserName <like 'Tom%'  "
		/// </summary>
		/// <param name="str"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.strStartLike(txtUserName.text,"UserName");
		/// </example>
		public static string strStartLike(this string strSql, string strValue, string ColName) {
			if ( !string.IsNullOrEmpty(strValue) )
				return string.Format(strSql + " and {0} like '{1}%' ", ColName, strValue);
			else
				return strSql;
		}

		/// <summary>
		/// return "  and UserName <like '%Tom'  "
		/// </summary>
		/// <param name="str"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.strEndLike(txtUserName.text,"UserName");
		/// </example>
		public static string strEndLike(this string strSql, string strValue, string ColName) {
			if ( !string.IsNullOrEmpty(strValue) )
				return string.Format(strSql + " and {0} like '%{1}' ", ColName, strSql);
			else
				return strValue;
		}

		/// <summary>
		/// Return " VBELN>'107111110' and VBELN <'107111113'"
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="strStart"></param>
		/// <param name="strEnd"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <remarks>
		/// 针对字符串区间的比较，如果起始都没有值的话，则不加限定条件，如果都有值的话则取中间，
		/// 如果其中一个有值的话去相等
		/// </remarks>
		/// <example>
		/// strSql.strEqualsOrBetween("107111110","107111113",VBELN)
		/// </example>
		public static string strEqualsOrBetween(this string strSql, string strStart, string strEnd, string ColName) {
			if ( string.IsNullOrEmpty(strStart) && string.IsNullOrEmpty(strEnd) )
				return strSql;
			else if ( !string.IsNullOrEmpty(strStart) && !string.IsNullOrEmpty(strEnd) ) {
				return strSql.strBigger(strStart, ColName).strSmaller(strEnd, ColName);
			}
			else if ( string.IsNullOrEmpty(strStart) && !string.IsNullOrEmpty(strEnd) )
				return strSql.strEquals(strEnd, ColName);
			else
				return strSql.strEquals(strStart, ColName);
		}

		#endregion

		#region --------------Int类型的扩展-------------

		/// <summary>
		/// Return"  and Age=18  "
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="IntValue"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.IntEquals(IntAge,"Age")
		/// </example>
		public static string IntEquals(this string strSql, int IntValue, string ColName) {
			return string.Format(strSql + " and {0}={1} ", ColName, IntValue);
		}


		/// <summary>
		/// return " and Age!=18"
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="IntValue"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.IntNotEquals(IntAge,"Age")
		/// </example>
		public static string IntNotEquals(this string strSql, int IntValue, string ColName) {
				return string.Format(strSql + " and {0}!={1} ", ColName, IntValue);
		}
		/// <summary>
		/// return " and Age in (1,2,3)"
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="IntValue"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		public static string IntContains(this string strSql, int [ ] IntValue, string ColName) {
			return string.Format(strSql + " and {0} in (" + string.Join(",", IntValue) + ") ", ColName, IntValue);
		}

		/// <summary>
		///Return"  and Age=18  "
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="IntValue"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.IntBigger(IntAge,"Age")
		/// </example>
		public static string IntBigger(this string strSql, int IntValue, string ColName) {
			return string.Format(strSql + " and {0}>{1} ", ColName, IntValue);
		}

		/// <summary>
		///Return"  and Age=18  "
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="IntValue"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.IntSmaller(IntAge,"Age")
		/// </example>
		public static string IntSmaller(this string strSql, int IntValue, string ColName) {
			return string.Format(strSql + " and  {0}<{1} ", ColName,IntValue);
		}


		/// <summary>
		/// Return " Age>18 and Age <20"
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="IntStart"></param>
		/// <param name="IntEnd"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <remarks>
		/// 设定的默认比较值为零
		/// </remarks>
		public static string IntEqualsOrBetween(this string strSql, int IntStart, int IntEnd, string ColName) {
			return strSql.IntBigger(IntStart, ColName)
						 .IntSmaller(IntEnd, ColName);
		}

		#endregion

		#region --------------Decimal类型的扩展-------------


		/// <summary>
		///Return"  and Age=18.00  "
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="DecimalValue"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.DecimalEquals(DecimalAge,"Age")
		/// </example>
		public static string DecimalEquals(this string strSql, decimal DecimalValue, string ColName) {
			return string.Format(strSql + " and {0}={1} ", ColName, DecimalValue);
		}


		/// <summary>
		/// return " and Age!=18.00"
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="DecimalValue"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.DecimalNotEquals(DecimalAge,"Age")
		/// </example>
		public static string DecimalNotEquals(this string strSql, decimal DecimalValue, string ColName) {
				return string.Format(strSql + " and {0}!={1} ", ColName, DecimalValue);
		}
		/// <summary>
		/// return " and Age in (1.00,2.00,3.00)"
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="DecimalValue"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		public static string DecimalContains(this string strSql, decimal [ ] DecimalValue, string ColName) {
			return string.Format(strSql + " and {0} in (" + string.Join(",", DecimalValue) + ") ", ColName, DecimalValue);
		}


		/// <summary>
		///Return"  and Age=18.00  "
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="DecimalValue"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.DecimalBigger(DecimalAge,"Age")
		/// </example>
		public static string DecimalBigger(this string strSql, decimal DecimalValue, string ColName) {
			return string.Format(strSql + " and {0}>{1} ", ColName, DecimalValue);
		}

		/// <summary>
		///  return " and Age<18.00"
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="DecimalValue"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.DecimalSmaller(DecimalAge,"Age")
		/// </example>
		public static string DecimalSmaller(this string strSql, decimal DecimalValue, string ColName) {
				return string.Format(strSql + " and  {0}<{1} ", ColName, DecimalValue);
		}


		/// <summary>
		/// Return " Age>18.00 and Age <20.00"
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="DecimalStart"></param>
		/// <param name="DecimalEnd"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <remarks>
		/// 针对整形区间的比较，如果起始都没有值的话，则不加限定条件，如果都有值的话则取中间，
		/// </remarks>
		/// <example>
		/// strSql.DecimalEqualsOrBetween(18.00,20.00,Age)
		/// </example>
		public static string DecimalEqualsOrBetween(this string strSql, decimal DecimalStart, decimal DecimalEnd,  string ColName) {
				return strSql.DecimalBigger(DecimalStart, ColName)
							 .DecimalSmaller(DecimalEnd, ColName);
		}

		#endregion

		#region --------------Datetime类型的扩展-------------

		/// <summary>
		/// Return " and CreateDate='2012-9-10 12:47:44' "
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="DtValue"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.DtEquals("2012-9-10 12:47:44",CreateTime);
		/// </example>
		public static string DtEquals(this string strSql, DateTime DtValue, string ColName) {
			if ( !string.IsNullOrEmpty(DtValue.ToString()) )
				return string.Format(strSql + " and {0}='{1}' ", ColName, DtValue);
			else
				return strSql;
		}

		/// <summary>
		/// Return " and CreateDate>'2012-9-10 12:47:44' "
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="DtValue"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.DtBigger("2012-9-10 12:47:44",CreateTime);
		/// </example>
		public static string DtBigger(this string strSql, DateTime DtValue, string ColName) {
			if ( !string.IsNullOrEmpty(DtValue.ToString()) )
				return string.Format(strSql + " and {0}>'{1}' ", ColName, DtValue);
			else
				return strSql;
		}

		/// <summary>
		/// Return " and CreateDate<'2012-9-10 12:47:44' "
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="DtValue"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <example>
		/// strSql.DtSmaller("2012-9-10 12:47:44",CreateTime);
		/// </example>
		public static string DtSmaller(this string strSql, DateTime DtValue, string ColName) {
			if ( !string.IsNullOrEmpty(DtValue.ToString()) )
				return string.Format(strSql + " and {0}<'{1}' ", ColName, DtValue);
			else
				return strSql;
		}

		/// <summary>
		/// Return " CreateDate>'2012-9-10 13:00:38' and CreateDate <'2012-9-10 13:00:42'"
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="strStart"></param>
		/// <param name="strEnd"></param>
		/// <param name="ColName"></param>
		/// <returns></returns>
		/// <remarks>
		/// 针对时间区间的比较，如果起始都没有值的话，则不加限定条件，如果都有值的话则取中间，
		/// 如果其中一个有值的话去相等
		/// </remarks>
		/// <example>
		/// strSql.DtEqualsOrBetween("2012-9-10 13:00:38","2012-9-10 13:00:42",VBELN)
		/// </example>
		public static string DtEqualsOrBetween(this string strSql, DateTime DtStart, DateTime DtEnd, string ColName) {
			if ( !string.IsNullOrEmpty(DtStart.ToString()) && !string.IsNullOrEmpty(DtEnd.ToString()) )
				return strSql.DtBigger(DtStart, ColName)
							 .DtSmaller(DtEnd, ColName);
			else if ( string.IsNullOrEmpty(DtStart.ToString()) && string.IsNullOrEmpty(DtEnd.ToString()) )
				return strSql;
			else if ( string.IsNullOrEmpty(DtStart.ToString()) && !string.IsNullOrEmpty(DtEnd.ToString()) )
				return strSql.DtEquals(DtEnd, ColName);
			else
				return strSql.DtEquals(DtStart, ColName);
		}

		#endregion

	}

}
