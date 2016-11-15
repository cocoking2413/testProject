using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using sqlGenerateTest.Contact;
using Newtonsoft.Json;
using static sqlGenerateTest.Class1;
using System.Reflection;
using ZMS.BLL.OA.IApprove;

namespace sqlGenerateTest {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
			this.json_text.Text = JsonConvert.SerializeObject(new ApproveParam() { Type = FormTypeEnum.Purchase, State = -1, DepartID = new int [ ] { 0 }, PositionID = new int [ ] { 0 }, Uid = new int [ ] { 0 }, IsNeedSplitList = false }, Formatting.Indented);
		}

		private void run_btn_Click(object sender, EventArgs e) {
			try {
				var param=JsonConvert.DeserializeObject<ApproveParam>(this.json_text.Text);
				result_text.Text = Class1.getApproveList2(1,param);
			}
			catch ( Exception ex ) {
				MessageBox.Show(ex.Message,"序列化失败");
			}
		}

		private void sql_btn_Click(object sender, EventArgs e) {
			using (DBContext db=new DBContext()) {
				//result_text.Text = JsonConvert.SerializeObject(db.Infos.Where(p=>p.Type==2).ToList(), Formatting.Indented);
				var param = JsonConvert.DeserializeObject<ApproveParam>(this.json_text.Text);
				result_text.Text = JsonConvert.SerializeObject(db.SqlQueryForDataTatable(Class1.getApproveList2(1, param)), Formatting.Indented);
			}
		}

		private void test_btn_Click(object sender, EventArgs e) {
			Type test = new TestAllType().GetType();
			foreach ( PropertyInfo pi in test.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) )
				result_text.Text += swichMethod(pi.PropertyType)+"\n";
		}

		private static string swichMethod(Type type) {
			string columnType = type.Name;
			string sqltype;
			switch ( columnType ) {
				case "Boolean":
					sqltype = "bit";
					break;
				case "Int16":
				case "Int32":
				case "Int64":
					sqltype = "int";
					break;
				case "Decimal":
					sqltype = "decimal(18,2)";
					break;
				case "DateTime":
					sqltype = "datetime2";
					break;
				case "String":
					sqltype = "nvarchar(1000)";
					break;
				case "Nullable`1":
					sqltype = swichMethod(type.GetGenericArguments()[0]);
					break;
				default:
					sqltype = columnType;
					break;
			}
			return sqltype;
		}
	}
}
