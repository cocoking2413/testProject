using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using HeyRed.MarkdownSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NReco.PdfGenerator;

namespace MD {
	public partial class Main : Form {
		//当前文件全名称
		public string curFileInfo { get; set; }
		//是否编辑
		public bool isChange { get; set; }

		public Main() {
			InitializeComponent();
			Init();
		}
		//初始化
		private void Init() {
			this.KeyDown += Main_KeyDown;
			edit_text.KeyDown += Edit_text_KeyDown;
			edit_text.MouseClick += Edit_text_MouseClick;
			this.isChange = false;
		}
		//快捷键
		private void Main_KeyDown(object sender, KeyEventArgs e) {
			if ( e.KeyCode == Keys.S && e.Modifiers == Keys.Control ) {//保存文件Ctrl+S
				save_Btn_click(sender, e);
			}
		}

		#region 操作
		//保存按钮
		private void save_Btn_click(object sender, EventArgs e) {
			if ( string.IsNullOrWhiteSpace(this.edit_text.Text) ) return;
			if ( string.IsNullOrWhiteSpace(this.curFileInfo) ) {
				SaveFileDialog save = new SaveFileDialog();
				save.FileName = "mdFile.md";
				save.DefaultExt = ".md";
				//默认目录
				save.InitialDirectory = System.Windows.Forms.Application.StartupPath.Replace(@"bin\Debug", "") + @"\public\workfile\";
				//设置文件保存类型
				save.Filter = "md文件|*.md|md文件|*.markdown|md文件|*.mdown|md文件|*.mkd|所有文件|*.*";
				//如果用户没有输入扩展名，自动追加后缀
				save.AddExtension = true;
				//保存对话框是否记忆上次打开的目录  
				save.RestoreDirectory = true;
				//设置标题
				save.Title = "保存";
				if ( save.ShowDialog() == DialogResult.OK ) {
					this.curFileInfo = save.FileName;
					saveFile();
					turnHtml(new Action<string>((str) => {
						this.html_text.Text = str;
						this.webBrowser1.DocumentText = str;
					}));
					this.tabPage1.Text = this.curFileInfo.Substring(this.curFileInfo.LastIndexOf(@"\") + 1, this.curFileInfo.Length - this.curFileInfo.LastIndexOf(@"\") - 1);
					this.isChange = false;
				}
			}
			else {
				saveFile();
				turnHtml(new Action<string>((str) => {
					this.html_text.Text = str;
					this.webBrowser1.DocumentText = str;
				}));
				this.tabPage1.Text = this.curFileInfo.Substring(this.curFileInfo.LastIndexOf(@"\") + 1, this.curFileInfo.Length - this.curFileInfo.LastIndexOf(@"\") - 1);
				this.isChange = false;
			}
		}
		//保存编辑文件
		private void saveFile() {
			try {
				//实例化一个文件流--->与写入文件相关联
				FileStream fs = new FileStream(this.curFileInfo, FileMode.Create);
				//实例化一个StreamWriter-->与fs相关联
				StreamWriter sw = new StreamWriter(fs);
				//开始写入
				sw.Write(this.edit_text.Text);
				//清空缓冲区
				sw.Flush();
				//关闭流
				sw.Close();
				fs.Close();
			}
			catch ( Exception ex ) {
				MessageBox.Show(ex.Message, "save error:");
			}
		}
		//解析markdown=>html
		private void turnHtml(Action<string> callBack) {
			try {
				Markdown md = new Markdown();
				callBack(md.Transform(this.edit_text.Text));
			}
			catch ( Exception ex ) {
				MessageBox.Show(ex.Message, "turn error:");
			}
		}
		//导出pdf文件
		private void savePDF_Btn_click(object sender, EventArgs e) {
			if ( string.IsNullOrWhiteSpace(this.edit_text.Text) ) return;
				SaveFileDialog save = new SaveFileDialog();
				save.FileName = "mdFile.pdf";
			save.DefaultExt = ".pdf";
			//默认目录
			save.InitialDirectory = System.Windows.Forms.Application.StartupPath.Replace(@"bin\Debug", "") + @"\public\workfile\";
				//设置文件保存类型
				save.Filter = "pdf文件|*.pdf|所有文件|*.*";
				//如果用户没有输入扩展名，自动追加后缀
				save.AddExtension = true;
				//保存对话框是否记忆上次打开的目录  
				save.RestoreDirectory = true;
				//设置标题
				save.Title = "导出";
				if ( save.ShowDialog() == DialogResult.OK ) {
				turnPDF(save.FileName,new Action<Exception>(error => {
                    if ( error != null ) {

                    }
					}));
				}
		
		}
		private void turnPDF(string path,Action<Exception> callBack) {
			string htmlPath=System.Windows.Forms.Application.StartupPath.Replace(@"bin\Debug", "") + @"\public\workfile\temphtml.html";
			try {

				#region 保存html临时文件
				/************************
				 * 1. 模板嵌套
				 * 2. 资源文件拷贝
				 * ************************/
				//保存文件
				try {
					//实例化一个文件流--->与写入文件相关联
					FileStream fs = new FileStream(htmlPath,FileMode.OpenOrCreate);
					//实例化一个StreamWriter-->与fs相关联
					StreamWriter sw = new StreamWriter(fs);
					//开始写入
					Markdown md = new Markdown();
					sw.Write(md.Transform(this.edit_text.Text));
					//清空缓冲区
					sw.Flush();
					//关闭流
					sw.Close();
					fs.Close();
				}
				catch ( Exception ex ) {
					MessageBox.Show(ex.Message, "save html error:");
                    callBack(ex);
                }
                #endregion

                #region html=>pdf
                try {
                    var conv = new HtmlToPdfConverter();
                    var pdf = conv.GeneratePdfFromFile(htmlPath, "");

                    #region 保存pdf
                    try {
                        File.WriteAllBytes(path, pdf);
                    }
                    catch ( Exception ex ) {
                        MessageBox.Show(ex.Message, "save pdf error:");
                        callBack(ex);
                    }
                    #endregion

                }
                catch ( Exception ex ) {
                    MessageBox.Show(ex.Message, "turn pdf error:");
                    callBack(ex);
                }
                #endregion

            }
			catch ( Exception ex ) {
				MessageBox.Show(ex.Message, "turn error:");
                callBack(ex);
            }
        }
		/// <summary>  
		/// 将Html文字 输出到PDF档里  
		/// </summary>  
		/// <param name="htmlText"></param>  
		/// <returns></returns>  
		public byte [ ] ConvertHtmlTextToPDF(string htmlText) {
			if ( string.IsNullOrEmpty(htmlText) ) {
				return null;
			}
			//避免当htmlText无任何html tag标签的纯文字时，转PDF时会挂掉，所以一律加上<p>标签  
			htmlText = "<p>" + htmlText + "</p>";

			MemoryStream outputStream = new MemoryStream();//要把PDF写到哪个串流  
			byte [ ] data = Encoding.UTF8.GetBytes(htmlText);//字串转成byte[]  
			MemoryStream msInput = new MemoryStream(data);
			Document doc = new Document();//要写PDF的文件，建构子没填的话预设直式A4  
			PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);
			//指定文件预设开档时的缩放为100%  

			PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
			//开启Document文件   
			doc.Open();

			//使用XMLWorkerHelper把Html parse到PDF档里  
			// XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new UnicodeFontFactory());  
			//XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8);

			//将pdfDest设定的资料写到PDF档  
			PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
			writer.SetOpenAction(action);
			doc.Close();
			msInput.Close();
			outputStream.Close();
			//回传PDF档案   
			return outputStream.ToArray();

		}

		//设置字体类  
		public class UnicodeFontFactory : FontFactoryImp {
			private static readonly string arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
	  "arialuni.ttf");//arial unicode MS是完整的unicode字型。  
			private static readonly string 标楷体Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
			"KAIU.TTF");//标楷体  


			public override iTextSharp.text.Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color, bool cached) {
				BaseFont bfChiness = BaseFont.CreateFont(@"C:\Windows\Fonts\SIMSUN.TTC,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
				//可用Arial或标楷体，自己选一个  
				BaseFont baseFont = BaseFont.CreateFont(标楷体Path, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
				return new iTextSharp.text.Font(bfChiness, size, style, color);
			}
		}


		#endregion

		#region 效果
		//编辑器鼠标事件
		private void Edit_text_MouseClick(object sender, MouseEventArgs e) {
			RichTextBox rich = ( RichTextBox ) sender;
			Control [ ] c = rich.Controls.Find("mylb", false);
			if ( c.Length > 0 )
				( ( ListBox ) c [0] ).Dispose();
		}

		public static List<string> AllClass() {
			List<string> list = new List<string>();
			list.Add("function");
			list.Add("return");
			list.Add("class");
			list.Add("new");
			list.Add("extends");
			list.Add("var");
			return list;
		}
		public static string GetLastWord(string str, int i) {
			string x = str;
			Regex reg = new Regex(@"\s+[a-z]+\s*", RegexOptions.RightToLeft);
			x = reg.Match(x).Value;
			Regex reg2 = new Regex(@"\s");
			x = reg2.Replace(x, "");
			return x;
		}
		//编辑器键盘事件
		private void Edit_text_KeyDown(object sender, KeyEventArgs e) {
			RichTextBox rich = ( RichTextBox ) sender;
			string s = GetLastWord(rich.Text, rich.SelectionStart);
			if ( AllClass().IndexOf(s) > -1 ) {
				MySelect(rich, rich.SelectionStart, s, Color.CadetBlue, true);
			}
			var likeList = AllClass().Where(p => p.Contains(s));
			if ( likeList.Count() > 0 ) {
				Control [ ] c = rich.Controls.Find("mylb", false);
				if ( c.Length > 0 )
					( ( ListBox ) c [0] ).Dispose();
				ListBox lb = new ListBox();
				lb.Name = "mylb";
				lb.Items.AddRange(likeList.ToArray());
				lb.Show();
				lb.TabIndex = 100;
				lb.Location = rich.GetPositionFromCharIndex(rich.SelectionStart);
				lb.Left += 10;
				rich.Controls.Add(lb);
			}
		}
		//设定颜色
		public static void MySelect(System.Windows.Forms.RichTextBox tb, int i, string s, Color c, bool font) {
			if ( i >= s.Length )
				tb.Select(i - s.Length, s.Length);
			tb.SelectionColor = c;
			//是否改变字体
			if ( font )
				tb.SelectionFont = new System.Drawing.Font("宋体", 12, ( FontStyle.Bold ));
			else
				tb.SelectionFont = new System.Drawing.Font("宋体", 12, ( FontStyle.Regular ));
			//以下是把光标放到原来位置，并把光标后输入的文字重置
			tb.Select(i, 0);
			tb.SelectionFont = new System.Drawing.Font("宋体", 12, ( FontStyle.Regular ));
			tb.SelectionColor = Color.Black;
		}
		//切换tab
		private void tabControl1_TabIndexChanged(object sender, EventArgs e) {
			var filePath = System.Windows.Forms.Application.StartupPath.Replace(@"bin\Debug", "") + @"\public\temp\temp1.html";

			switch ( tabControl1.SelectedIndex ) {
				case 0:
					break;
				case 1:
					//try {
					//	using ( var reader = new StreamReader(filePath, Encoding.UTF8) ) {
					//		this.webBrowser1.DocumentText = reader.ReadToEnd();
					//	}
					//}
					//catch ( Exception ex ) {
					//	this.webBrowser1.DocumentText = filePath;
					//}
					break;
				case 2:
					//try {
					//	using ( var reader = new StreamReader(filePath, Encoding.UTF8) ) {
					//		this.html_text.Text = reader.ReadToEnd();
					//	}
					//}
					//catch ( Exception ex ) {
					//	this.html_text.Text = filePath;
					//}
					break;
				default:
					break;
			}
		}
		//编辑器内容改变事件
		private void edit_text_TextChanged(object sender, EventArgs e) {
			if ( !this.isChange )
				this.tabPage1.Text += "*";
			this.isChange = true;
		}
		#endregion

		private void 打开ToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenFileDialog open = new OpenFileDialog();
			//默认目录
			open.InitialDirectory = System.Windows.Forms.Application.StartupPath.Replace(@"bin\Debug", "") + @"\public\workfile\";
			//设置文件保存类型
			open.Filter = "md文件|*.md|md文件|*.markdown|md文件|*.mdown|md文件|*.mkd|所有文件|*.*";
			//保存对话框是否记忆上次打开的目录  
			open.RestoreDirectory = true;
			//设置标题
			open.Title = "打开";
			if ( open.ShowDialog() == DialogResult.OK ) {
				var filePath = open.FileName;
				this.curFileInfo = filePath;
				try {
					using ( var reader = new StreamReader(filePath, Encoding.UTF8) ) {
						this.edit_text.Text = reader.ReadToEnd();
					}
				}
				catch ( Exception ex ) {
					MessageBox.Show(ex.Message);
				}
			}
		}

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void hTMLToolStripMenuItem_Click(object sender, EventArgs e) {
            
        }

        private void pDFToolStripMenuItem_Click(object sender, EventArgs e) {
            savePDF_Btn_click(sender,e);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e) {
            save_Btn_click(sender, e);
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e) {
            save_Btn_click(sender, e);
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
