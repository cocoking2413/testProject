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
using NReco.PdfGenerator;

namespace HtmlToPdf {
    public partial class Form2 : Form {
        public Form2() {
            InitializeComponent();
        }
        private void run_btn_Click(object sender, EventArgs e) {
            if ( File.Exists(this.htmlpath_text.Text) ) {
                saveFile(new Action<string>(filePath => {
                    this.pdfpath_text.Text = filePath;
                    var conv = new HtmlToPdfConverter();
                    var pdf = conv.GeneratePdfFromFile(this.htmlpath_text.Text, "<h1 class='center'>test html</h1>");
                    File.WriteAllBytes(filePath,pdf);
                    //using ( var fs = new FileStream(filePath, FileMode.Create) ) {
                    //    MemoryStream ms = new MemoryStream(pdf);
                    //    ms.WriteTo(fs);
                    //    ms.Close();
                    //}
                }));
            }
            else {
                open_btn_Click(sender,e);
                run_btn_Click(sender,e);
            }
        }
        private void open_btn_Click(object sender, EventArgs e) {
            openFile(new Action<string>(filePath=> {
                this.htmlpath_text.Text = filePath;
            }),new string [ ] { "html","htm"});
        }

        private void preview_btn_Click(object sender, EventArgs e) {
            if ( File.Exists(this.pdfpath_text.Text) ) {
                if ( Path.GetExtension(this.pdfpath_text.Text) == ".pdf" ) {
                    System.Diagnostics.Process.Start(this.pdfpath_text.Text);
                    return;
                }
                MessageBox.Show("不支持当前文件类型", Path.GetExtension(this.pdfpath_text.Text));
                return;
            }
            MessageBox.Show("文件不存在");
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

    }
}
