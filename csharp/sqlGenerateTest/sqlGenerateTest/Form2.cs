using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sqlGenerateTest {
    public partial class Form2 : Form {
        public Form2() {
            InitializeComponent();
        }

        private void open_btn_Click(object sender, EventArgs e) {
            openFile(new Action<string>(path=> {
                this.open_path.Text = path;
            }),new string [ ] { "txt"},true);
        }

        private void save_btn_Click(object sender, EventArgs e) {
            saveFile(new Action<string>(path => {
                this.save_path.Text = path;
            }), new string [ ] { "sql" }, true);
        }

        private void run_btn_Click(object sender, EventArgs e) {
            if (File.Exists(this.open_path.Text)&&Path.GetExtension(this.save_path.Text)==".sql") {
                try {
                    string sql = "";
                    using ( var reader = new StreamReader(this.open_path.Text, Encoding.UTF8) ) {
                        while ( !reader.EndOfStream ) {
                            string line = reader.ReadLine();
                            if ( !string.IsNullOrWhiteSpace(line) ) {
                                if(string.IsNullOrWhiteSpace(this.tabelName.Text) )
                                    sql += @"insert into oa_WorkBlog values(" + line + ")\r\n";
                                else
                                    sql += @"insert into "+(this.tabelName.Text)+" values(" + line + ")\r\n";
                            }
                        }
                    }
                        //实例化一个文件流--->与写入文件相关联
                        FileStream fs = new FileStream(this.save_path.Text, FileMode.Create);
                        //实例化一个StreamWriter-->与fs相关联
                        StreamWriter sw = new StreamWriter(fs);
                        //开始写入
                        sw.Write(sql);
                        //清空缓冲区
                        sw.Flush();
                        //关闭流
                        sw.Close();
                        fs.Close();
                    this.template_text.Text = sql;
                } catch ( Exception ex ) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show("path not exist!!");
            }
        }

        private static void openFile(Action<string> callBack = null, string [ ] fileType = null, bool isNeedAll = false) {
            try {
                OpenFileDialog dlg = new OpenFileDialog();
                if ( fileType == null )
                    dlg.Filter = string.Format("{0} documents (.{0})|*.{0}", "pdf");
                else {
                    foreach ( var item in fileType ) {
                        dlg.Filter += string.Format("{0}{1} documents (.{1})|*.{1}", fileType [0] == item ? "" : "|", item);
                    }
                }
                if ( isNeedAll )
                    dlg.Filter += "| 所有文件(*.*)|*.*";
                if ( dlg.ShowDialog() == DialogResult.OK ) {
                    if ( callBack != null )
                        callBack(dlg.FileName);
                    MessageBox.Show("ok");
                }
            }
            catch ( Exception ex ) {
                MessageBox.Show(ex.Message);
            }
        }
        private static void saveFile(Action<string> callBack = null, string [ ] fileType = null, bool isNeedAll = false) {
            try {
                SaveFileDialog dlg = new SaveFileDialog();

                if ( fileType == null ) {
                    dlg.FileName = "pdfFile";
                    dlg.DefaultExt = ".pdf";
                    dlg.Filter = string.Format("{0} documents (.{0})|*.{0}", "pdf");
                }
                else {
                    dlg.FileName = fileType [0] + "File";
                    dlg.DefaultExt = "." + fileType [0];
                    foreach ( var item in fileType ) {
                        dlg.Filter += string.Format("{0}{1} documents (.{1})|*.{1}", fileType [0] == item ? "" : "|", item);
                    }
                }
                if ( isNeedAll )
                    dlg.Filter += "| 所有文件(*.*)|*.*";

                if ( dlg.ShowDialog() == DialogResult.OK ) {
                    if ( callBack != null )
                        callBack(dlg.FileName);
                    MessageBox.Show("ok");
                }
            }
            catch ( Exception ex ) {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
