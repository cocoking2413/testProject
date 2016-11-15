using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HtmlToPdf {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
			this.urlOrPath_text.Text = @"C:\Users\Administrator\Desktop\temp1.html";
			this.comboBox1.Items.AddRange(new string [ ] { "CreatePDF", "CreatePdfSetInfo", "CreateNewPdfPage", "ImageDirect", "CreateTable", "CreateTableFormList", "CreateLink", "CreateBookMark", "ExportPdf", "CreateImgText", "CreateList", "CreateImg" });
			this.comboBox1.SelectedIndex = 0;
		}

		private void turn_btn_Click(object sender, EventArgs e) {
			if ( File.Exists(this.urlOrPath_text.Text) ) {
				if ( Path.GetExtension(this.urlOrPath_text.Text) == ".html" ) {
					MessageBox.Show("打开成功");
					return;
				}
				MessageBox.Show("不支持当前文件类型");
				return;
			}
			MessageBox.Show("文件不存在");
		}

		private void openpdf_btn_Click(object sender, EventArgs e) {
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

		private void open_btn_Click(object sender, EventArgs e) {
			try {
				System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
				psi.Arguments = "/e,/select," + this.pdfpath_text.Text;
				System.Diagnostics.Process.Start(psi);
			}
			catch ( Exception ex ) { }
		}

		private void explore_btn_Click(object sender, EventArgs e) {
			try {
				System.Diagnostics.Process.Start(this.urlOrPath_text.Text);
			}
			catch ( Exception ex ) {
				MessageBox.Show(ex.Message);
			}

		}

		private void test_btn_Click(object sender, EventArgs e) {
			comboBox1_SelectedIndexChanged(sender, e);
		}
		/// <summary>
		/// 我得第一个Pdf程序
		/// </summary>
		/// <param name="msg"></param>
		public void CreatePDF(string msg) {
			saveFile(new Action<string>((fileName) => {
				this.pdfpath_text.Text = fileName;
				using ( var fs = new FileStream(fileName, FileMode.Create) ) {
					Document document = new Document();
					string path = fileName;
					this.pdfpath_text.Text = path;
					PdfWriter.GetInstance(document, fs);
					
					//设置中文是字体，否则，中文存不了
					BaseFont bfHei = BaseFont.CreateFont(@"C:\Windows\Fonts\simfang.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
					iTextSharp.text.Font font = new iTextSharp.text.Font(bfHei, 16);
					try {
						document.Open();
						document.Add(new Paragraph(msg, font));//msg你需要在PDF中显示的文字
					}
					catch ( Exception ex ) {
						throw ex;
					}
					finally {
						document.Close();
					}
				}
			}));
		}
		/// <summary>
		/// 设置页面大小、作者、标题等相关信息设置
		/// </summary>
		private void CreatePdfSetInfo(string str) {
			saveFile(new Action<string>((fileName) => {
				this.pdfpath_text.Text = fileName;
				using ( var fs = new FileStream(fileName, FileMode.Create) ) {

					//设置页面大小
					iTextSharp.text.Rectangle pageSize = new iTextSharp.text.Rectangle(960f, 1900f);
					pageSize.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
					//设置边界
					Document document = new Document(pageSize, 36f, 72f, 108f, 180f);
					PdfWriter.GetInstance(document, fs);
					//设置中文是字体，否则，中文存不了
					BaseFont bfHei = BaseFont.CreateFont(@"C:\Windows\Fonts\simfang.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
					iTextSharp.text.Font font = new iTextSharp.text.Font(bfHei, 16);
					try {
						// 添加文档信息
						document.AddTitle("PDFInfo");
						document.AddSubject("Demo of PDFInfo");
						document.AddKeywords("Info, PDF, Demo");
						document.AddCreator("coco king");
						document.AddAuthor("coco king");
						document.AddProducer();
						document.AddCreationDate();

						document.AddHeader("generate by cocoking", "https://github.com/cocoking2413/");
						document.Open();

						// 添加文档内容
						document.Add(new Paragraph(str, font));
					}
					catch ( Exception ex ) {
						throw ex;
					}
					finally {
						document.Close();
					}
				}
			}));
		}
		/// <summary>
		/// 多页面
		/// </summary>
		/// <param name="str"></param>
		private void CreateNewPdfPage(string str) {
			saveFile(new Action<string>((fileName) => {
				this.pdfpath_text.Text = fileName;
				using ( var fs = new FileStream(fileName, FileMode.Create) ) {

					Document document = new Document(PageSize.NOTE);
					PdfWriter writer = PdfWriter.GetInstance(document, fs);
					//设置中文是字体，否则，中文存不了
					BaseFont bfHei = BaseFont.CreateFont(@"C:\Windows\Fonts\simfang.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
					iTextSharp.text.Font font = new iTextSharp.text.Font(bfHei, 16);
					try {
						document.Open();
						// 第一页
						document.Add(new iTextSharp.text.Paragraph(str, font));
						// 添加新页面
						document.NewPage();
						// 第二页
						// 添加第二页内容
						document.Add(new iTextSharp.text.Paragraph(str, font));
						// 添加新页面
						document.NewPage();
						// 第三页
						// 添加新内容
						document.Add(new iTextSharp.text.Paragraph(str, font));
						// 重新开始页面计数
						document.ResetPageCount();
						// 新建一页
						document.NewPage();
						// 第四页
						// 添加第四页内容
						document.Add(new iTextSharp.text.Paragraph(str, font));
					}
					catch ( Exception ex ) {
						throw ex;
					}
					finally {
						document.Close();
					}
				}
			}));
		}
		/// <summary>
		/// 生成图片pdf页（pdf中插入图片）
		/// </summary>
		private void ImageDirect(string imagePath) {
			saveFile(new Action<string>((fileName) => {
				this.pdfpath_text.Text = fileName;
				using ( var fs = new FileStream(fileName, FileMode.Create) ) {

					iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
					//设置页面大小
					iTextSharp.text.Rectangle pageSize = new iTextSharp.text.Rectangle(img.ScaledWidth + 36f * 2, img.ScaledHeight + 72f * 2);
					pageSize.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
					img.SetAbsolutePosition(( pageSize.Width - img.ScaledWidth ) / 2, ( pageSize.Height - img.ScaledHeight ) / 2);
					//img.ScalePercent(24f);
					//img.SetDpi(300,300);
					img.SetDpi(1000, 1000);

					Document document = new Document(pageSize, 36f, 36f, 72f, 72f);
					PdfWriter writer = PdfWriter.GetInstance(document, fs);
					try {
						document.Open();
						document.Add(img);
					}
					catch ( Exception ex ) {
						throw ex;
					}
					finally {
						document.Close();
					}
				}
			}));
		}
		/// <summary>
		/// 创建表格
		/// </summary>
		private void CreateTable() {
			saveFile(new Action<string>((fileName) => {
				this.pdfpath_text.Text = fileName;
				using ( var fs = new FileStream(fileName, FileMode.Create) ) {
					Document document = new Document();
					PdfWriter writer = PdfWriter.GetInstance(document, fs);

					PdfPTable table = new PdfPTable(3);
					PdfPCell cell;
					cell = new PdfPCell(new Phrase("Cell with colspan 3"));
					cell.Colspan = 3;
					table.AddCell(cell);
					cell = new PdfPCell(new Phrase("Cell with rowspan 2"));
					cell.Rowspan = 2;
					table.AddCell(cell);
					table.AddCell("row 1; cell 1");
					table.AddCell("row 1; cell 2");
					table.AddCell("row 2; cell 1");
					table.AddCell("row 2; cell 2");
					try {
						document.Open();
						document.Add(table);
					}
					catch ( Exception ex ) {
						throw ex;
					}
					finally {
						document.Close();
					}
				}
			}));
		}
		/// <summary>
		/// list生成表格
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="arrayColWidth"></param>
		private void CreateTable<T>(List<T> list, float [ ] arrayColWidth = null) where T : new() {
			saveFile(new Action<string>((fileName) => {
				this.pdfpath_text.Text = fileName;
				using ( var fs = new FileStream(fileName, FileMode.Create) ) {
					Document document = new Document(PageSize.A4.Rotate());//如果希望使用横向页面，你只须使用rotate()函数
					PdfWriter writer = PdfWriter.GetInstance(document, fs);
					T t = new T();
					var types = t.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
					PdfPTable table;
					if ( arrayColWidth == null )
						table = new PdfPTable(types.Length + 2);
					else
						table = new PdfPTable(arrayColWidth);
					table.AddCell("");
					PdfPCell cell;

					foreach ( var info in types ) {
						cell = new PdfPCell(new Phrase(info.Name));
						cell.BackgroundColor = new BaseColor(Color.LightBlue);
						cell.HorizontalAlignment = 1;
						cell.VerticalAlignment = 1;
						table.AddCell(cell);
					}
					list.ForEach(p => {
						cell = new PdfPCell(new Phrase(( list.IndexOf(p) + 1 ).ToString()));
						cell.BackgroundColor = new BaseColor(Color.LightGray);
						cell.HorizontalAlignment = 1;
						cell.VerticalAlignment = 1;
						table.AddCell(cell);
						foreach ( var info in types ) {
							if ( types [2] == info ) {
								cell = new PdfPCell(new Phrase(info.GetValue(p).ToString()));
								cell.BackgroundColor = new BaseColor(Color.LimeGreen);
								cell.HorizontalAlignment = 1;
								cell.VerticalAlignment = 1;
								table.AddCell(cell);
							}
							else
								table.AddCell(info.GetValue(p).ToString());
						}
					});
					try {
						document.Open();
						document.Add(table);
					}
					catch ( Exception ex ) {
						throw ex;
					}
					finally {
						document.Close();
					}
				}
			}));
		}
		/// <summary>
		/// 链接+锚点
		/// </summary>
		private void CreateLink() {
			saveFile(new Action<string>((fileName) => {
				this.pdfpath_text.Text = fileName;
				using ( var fs = new FileStream(fileName, FileMode.Create) ) {

					//设置页面大小
					//设置边界
					Document document = new Document(PageSize.A4, 36f, 72f, 108f, 180f);
					PdfWriter.GetInstance(document, fs);
					//设置中文是字体，否则，中文存不了
					BaseFont bfHei = BaseFont.CreateFont(@"C:\Windows\Fonts\simfang.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
					iTextSharp.text.Font font = new iTextSharp.text.Font(bfHei, 16);

					try {
						// 添加文档信息
						document.AddTitle("PDFInfo");
						document.AddSubject("Demo of PDFInfo");
						document.AddKeywords("Info, PDF, Demo");
						document.AddCreator("coco king");
						document.AddAuthor("coco king");
						document.AddProducer();
						document.AddCreationDate();

						document.AddHeader("generate by cocoking", "https://github.com/cocoking2413/");

						document.Open();
						// 添加文档内容
						Anchor link;
						link = new Anchor("github.com/cocoking2413/", FontFactory.GetFont(FontFactory.HELVETICA, 14, ( int ) FontStyle.Underline, new BaseColor(Color.Blue)));
						link.Reference = "https://github.com/cocoking2413/";
						link.Name = "cocoking";
						document.Add(link);

						document.Add(new Paragraph("\n"));

						link = new Anchor("click 改是为啥地方", FontFactory.GetFont(FontFactory.HELVETICA, 14, ( int ) FontStyle.Bold, new BaseColor(Color.GreenYellow)));
						link.Reference = "#target";
						document.Add(link);

						document.NewPage();
						link = new Anchor("target");
						link.Name = "target";
						document.Add(link);
					}
					catch ( Exception ex ) {
						throw ex;
					}
					finally {
						document.Close();
					}
				}
			}));
		}
		/// <summary>
		/// 导出表格示例+页眉页脚
		/// </summary>
		private void ExportPdf() {
			saveFile(new Action<string>(fileName => {
				this.pdfpath_text.Text = fileName;
				PDFReportTest.ExportPDF(fileName);
			}));
		}
		/// <summary>
		/// 创建书签或结构树
		/// </summary>
		private void CreateBookMark() {
			saveFile(new Action<string>(fileName => {
				this.pdfpath_text.Text = fileName;
				using ( var fs = new FileStream(fileName, FileMode.Create) ) {
					Document doc = new Document();
					var writer = PdfWriter.GetInstance(doc, fs);


					Chapter chapter1 = new Chapter(new Paragraph("This is Chapter 1"), 1);

					Section section1 = chapter1.AddSection(20f, "Section 1.1", 2);

					Section section2 = chapter1.AddSection(20f, "Section 1.2", 2);

					Section subsection1 = section2.AddSection(20f, "Subsection 1.2.1", 3);

					Section subsection2 = section2.AddSection(20f, "Subsection 1.2.2", 3);

					Section subsubsection = subsection2.AddSection(20f, "Sub Subsection 1.2.2.1", 4);

					Chapter chapter2 = new Chapter(new Paragraph("This is Chapter 2"), 1);

					Section section3 = chapter2.AddSection("Section 2.1", 2);

					Section subsection3 = section3.AddSection("Subsection 2.1.1", 3);

					Section section4 = chapter2.AddSection("Section 2.2", 2);

					chapter1.BookmarkTitle = "Changed Title";

					chapter1.BookmarkOpen = true;

					chapter2.BookmarkOpen = false;

					try {
						doc.Open();
						doc.Add(chapter1);
						doc.Add(chapter2);
					}
					catch ( Exception ex ) {
						throw ex;
					}
					finally {
						doc.Close();
					}
				}
			}));
		}
		/// <summary>
		/// 图文混合
		/// </summary>
		private void CreateImgText() {
			openFile(new Action<string>(imgPath => {
				saveFile(new Action<string>(filePath => {
					this.pdfpath_text.Text = filePath;
					using ( var fs = new FileStream(filePath, FileMode.Create) ) {
						Document doc = new Document();
						PdfWriter writer = PdfWriter.GetInstance(doc, fs);

						iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imgPath);

						Paragraph paragraph = new Paragraph(@"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse blandit blandit turpis. Nam in lectus ut dolor consectetuer bibendum. Morbi neque ipsum, laoreet id; dignissim et, viverra id, mauris. Nulla mauris elit, consectetuer sit amet, accumsan eget, congue ac, libero. Vivamus suscipit. Nunc dignissim consectetuer lectus. Fusce elit nisi; commodo non, facilisis quis, hendrerit eu, dolor? Suspendisse eleifend nisi ut magna. Phasellus id lectus! Vivamus laoreet enim et dolor. Integer arcu mauris, ultricies vel, porta quis, venenatis at, libero. Donec nibh est, adipiscing et, ullamcorper vitae, placerat at, diam. Integer ac turpis vel ligula rutrum auctor! Morbi egestas erat sit amet diam. Ut ut ipsum? Aliquam non sem. Nulla risus eros, mollis quis, blandit ut; luctus eget, urna. Vestibulum vestibulum dapibus erat. Proin egestas leo a metus?");

						paragraph.Alignment = Element.ALIGN_JUSTIFIED;

						img.ScaleToFit(250f, 250f);

						img.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_RIGHT;

						img.IndentationLeft = 9f;

						img.SpacingAfter = 9f;

						img.BorderWidthTop = 36f;

						img.BorderColorTop = new BaseColor(Color.White);


						try {
							doc.Open();
							doc.Add(img);
							doc.Add(paragraph);
						}
						catch ( Exception ex ) {
							throw ex;
						}
						finally {
							doc.Close();
						}
					}
				}));
			}), new string [ ] { "png", "jpg", "gif", "bmp", "tif", "wmf" });
		}
		/// <summary>
		/// 列表 ul/ol
		/// </summary>
		private void CreateList() {
			saveFile(new Action<string>(filePath=> {
				this.pdfpath_text.Text = filePath;
				using (var fs =new FileStream(filePath, FileMode.Create)) {
					Document doc = new Document(PageSize.A4);
					PdfWriter writer = PdfWriter.GetInstance(doc,fs);
					try {
						doc.Open();
						Paragraph paragraph;
						string [ ] lists = new string [ ] { "List", "RomanList", "GreekList", "ZapfDingbatsList ", "ZapfDingbatsNumberList" };
						for ( int i = 0; i < lists.Length; i++ ) {
							switch ( i ) {
								case 1:
									generateList(doc, out paragraph, lists [i], new Func<RomanList>(() => {
										return new RomanList(true,20);
									}),new Func<RomanList, RomanList>(list=> {
										var romanlist = new GreekList(true, 20);
										romanlist.Add("One");

										romanlist.Add("Two");

										romanlist.Add("Three");

										romanlist.Add("Four");

										romanlist.Add("Five");

										list.Add(romanlist);
										return list;
									}));
									break;
								case 2:
									generateList(doc, out paragraph, lists [i], new Func<GreekList>(() => {
										return new GreekList(true,20);
									}));
									break;
								case 3:
									generateList(doc, out paragraph, lists [i], new Func<ZapfDingbatsList>(() => {
										return new ZapfDingbatsList(49,15);
									}));
									break;
								case 4:
									generateList(doc, out paragraph, lists [i], new Func<ZapfDingbatsNumberList>(() => {
										return new ZapfDingbatsNumberList(49,15);
									}));
									break;
								case 0:
								default:
									generateList(doc, out paragraph, lists [i], new Func<List>(() => {
										return new List(List.ORDERED,20f);
									}), new Func<List, List>(list => {
										var romanlist = new RomanList(true,20);
										romanlist.Add("One");

										romanlist.Add("Two");

										romanlist.Add("Three");

										romanlist.Add("Four");

										romanlist.Add("Five");

										list.Add(romanlist);
 										return list;
									}));
									break;
							}
						}
					}
					catch ( Exception ex ) {
						throw ex;
					} finally {
						doc.Close();
					}
				}
			}));
		}
		private void CreateImg() {
			saveFile(new Action<string>(fileName => {
				this.pdfpath_text.Text = fileName;
				using ( var fs = new FileStream(fileName, FileMode.Create) ) {
					Document doc = new Document(PageSize.A4.Rotate());
					PdfWriter writer = PdfWriter.GetInstance(doc, fs);

					try {
						doc.Open();
						var cb = writer.DirectContent;

						//线
						cb.MoveTo(10, 10);
						cb.LineTo(doc.PageSize.Width - 10, doc.PageSize.Height - 10);
						cb.SetCMYKColorStroke(0, 255, 0, 5);
						cb.Stroke();
						cb.MoveTo(doc.PageSize.Width - 10, 10);
						cb.LineTo(10, doc.PageSize.Height - 10);
						cb.SetRGBColorStroke(155, 4, 5);
						cb.Stroke();

						//三角
						cb.MoveTo(doc.PageSize.Width / 2, doc.PageSize.Height / 2 - 50);
						cb.LineTo(doc.PageSize.Width / 2 - 200, 50);
						cb.LineTo(doc.PageSize.Width / 2 + 200, 50);
						cb.ClosePath();
						cb.SetRGBColorFill(0, 0, 0);
						cb.SetRGBColorStroke(255, 0, 0);
						cb.FillStroke();

						//矩形
						cb.Rectangle(10, doc.PageSize.Height / 2 - 200 / 2, 200, 200);
						cb.SetRGBColorFill(6, 100, 200);
						cb.SetRGBColorStroke(200, 4, 100);
						cb.FillStroke();
						//圆角矩形
						cb.RoundRectangle(35, doc.PageSize.Height / 2 - 100 / 2, 150, 100, 200 / 10);
						cb.SetRGBColorFill(100, 255, 100);
						cb.SetRGBColorStroke(255, 255, 255);
						cb.FillStroke();

						//椭圆
						cb.Ellipse(doc.PageSize.Width / 2 - 400 / 2, doc.PageSize.Height - 10 - 200, doc.PageSize.Width / 2 + 400 / 2, doc.PageSize.Height - 10);
						cb.SetRGBColorFill(100, 255, 100);
						cb.SetRGBColorStroke(0, 4, 100);
						cb.FillStroke();
						//圆
						cb.Circle(doc.PageSize.Width / 2, doc.PageSize.Height - 10 - 100, 100);
						cb.SetRGBColorFill(0, 0, 100);
						cb.SetRGBColorStroke(255, 255, 255);
						cb.FillStroke();

						//曲线
						cb.Rectangle(doc.PageSize.Width - 10 - 180, doc.PageSize.Height / 2 - 300 / 2, 180, 300);
						cb.SetRGBColorFill(255, 255, 255);
						cb.SetRGBColorStroke(200, 200, 200);
						cb.Stroke();
						cb.MoveTo(doc.PageSize.Width - 10 - 180 / 2, doc.PageSize.Height / 2 + 300 / 2);
						cb.CurveTo(doc.PageSize.Width - 10 - 360, doc.PageSize.Height / 2 + 90, doc.PageSize.Width - 10 + 180, doc.PageSize.Height / 2 - 90, doc.PageSize.Width - 10 - 180 / 2, doc.PageSize.Height / 2 - 300 / 2);
						cb.SetRGBColorStroke(0, 100, 255);
						cb.Stroke();

						cb.MoveTo(doc.PageSize.Width - 10 - 180 / 2, doc.PageSize.Height / 2 + 300 / 2);
						cb.Arc(doc.PageSize.Width - 10 - 180, doc.PageSize.Height / 2 - 300 / 2, doc.PageSize.Width - 10, doc.PageSize.Height / 2 + 300 / 2, 180, -90);
						cb.SetLineDash(2f, 6f);
						cb.SetRGBColorStroke(0, 255, 100);
						cb.Stroke();

						cb.MoveTo(doc.PageSize.Width - 10, doc.PageSize.Height / 2 + 300 / 2);
						cb.Arc(doc.PageSize.Width - 10 - 180, doc.PageSize.Height / 2 - 300 / 2, doc.PageSize.Width - 10+180, doc.PageSize.Height / 2 + 300 / 2, 180, -90);
						cb.SetLineDash(2f, 6f);
						cb.SetRGBColorStroke(20, 255, 20);
						cb.Stroke();

						cb.SetLineDash(1);
						generateArrow(cb, doc.PageSize.Width-10-180/Math.Sqrt(2), doc.PageSize.Height / 2+150/Math.Sqrt(2), 5, 10, new Func<double [ , ], double [ , ]>(arr => {
							arr = arr.rotate(135);
							return arr;
						}), 2);
						cb.SetRGBColorFill(0, 0, 255);
						cb.SetRGBColorStroke(0, 0, 255);
						cb.FillStroke();

                        generateArrow(cb, doc.PageSize.Width - 10 - 180 / Math.Sqrt(2), doc.PageSize.Height / 2 + 150 / Math.Sqrt(2), 5, 10, new Func<double [ , ], double [ , ]>(arr => {
                            arr = arr.rotate(135);
                            return arr;
                        }), 2);
                        cb.SetRGBColorFill(0, 0, 255);
                        cb.SetRGBColorStroke(0, 0, 255);
                        cb.FillStroke();

                        //箭头
                        //上
                        cb.MoveTo(doc.PageSize.Width / 2, doc.PageSize.Height / 2);
						cb.LineTo(doc.PageSize.Width / 2, doc.PageSize.Height / 2 + 100);
						cb.SetRGBColorStroke(0, 0, 255);
						cb.SetLineDash(2, 6);
						cb.Stroke();

						cb.SetLineDash(1);

						generateArrow(cb, doc.PageSize.Width / 2, doc.PageSize.Height / 2, 15, 30, new Func<double [ , ], double [ , ]>(arr => {
							arr = arr.scale(0.5, 0.7);
							arr = arr.rotate(180);
							arr = arr.translate(0, 100);
							return arr;
						}), 2);
						cb.SetRGBColorFill(0, 0, 255);
						cb.SetRGBColorStroke(0, 0, 255);
						cb.FillStroke();

						//右
						cb.MoveTo(doc.PageSize.Width / 2, doc.PageSize.Height / 2);
						cb.LineTo(doc.PageSize.Width / 2 + 100, doc.PageSize.Height / 2);
						cb.SetRGBColorStroke(0, 0, 255);
						cb.SetLineDash(2, 6);
						cb.Stroke();

						cb.SetLineDash(1);

						generateArrow(cb, doc.PageSize.Width / 2, doc.PageSize.Height / 2, 15, 30, new Func<double [ , ], double [ , ]>(arr => {
							arr = arr.scale(0.5, 0.7);
							arr = arr.rotate(90);
							arr = arr.translate(100, 0);
							return arr;
						}), 2);
						cb.SetRGBColorFill(0, 0, 255);
						cb.SetRGBColorStroke(0, 0, 255);
						cb.FillStroke();

						//下
						cb.MoveTo(doc.PageSize.Width / 2, doc.PageSize.Height / 2);
						cb.LineTo(doc.PageSize.Width / 2, doc.PageSize.Height / 2 - 100);
						cb.SetRGBColorStroke(0, 0, 255);
						cb.SetLineDash(2, 6);
						cb.Stroke();

						cb.SetLineDash(1);

						generateArrow(cb, doc.PageSize.Width / 2, doc.PageSize.Height / 2, 15, 30, new Func<double [ , ], double [ , ]>(arr => {
							arr = arr.scale(0.5, 0.7);
							arr = arr.translate(0, -100);
							return arr;
						}), 2);
						cb.SetRGBColorFill(0, 0, 255);
						cb.SetRGBColorStroke(0, 0, 255);
						cb.FillStroke();

						//左
						cb.MoveTo(doc.PageSize.Width / 2, doc.PageSize.Height / 2);
						cb.LineTo(doc.PageSize.Width / 2 - 100, doc.PageSize.Height / 2);
						cb.SetRGBColorStroke(0, 0, 255);
						cb.SetLineDash(2, 6);
						cb.Stroke();

						cb.SetLineDash(1);

						generateArrow(cb, doc.PageSize.Width / 2, doc.PageSize.Height / 2, 15, 30, new Func<double [ , ], double [ , ]>(arr => {
							arr = arr.scale(0.5, 0.7);
							arr = arr.rotate(-90);
							arr = arr.translate(-100, 0);
							return arr;
						}), 2);
						cb.SetRGBColorFill(0, 0, 255);
						cb.SetRGBColorStroke(0, 0, 255);
						cb.FillStroke();

						//四角箭头
						generateArrow(cb, doc.PageSize.Width / 2, doc.PageSize.Height / 2, 5, 10, new Func<double [ , ], double [ , ]>(arr => {
							arr = arr.rotate(45);
							return arr;
						}), 2);
						generateArrow(cb, doc.PageSize.Width / 2, doc.PageSize.Height / 2, 5, 10, new Func<double [ , ], double [ , ]>(arr => {
							arr = arr.rotate(135);
							return arr;
						}), 2);
						generateArrow(cb, doc.PageSize.Width / 2, doc.PageSize.Height / 2, 5, 10, new Func<double [ , ], double [ , ]>(arr => {
							arr = arr.rotate(225);
							return arr;
						}), 2);
						generateArrow(cb, doc.PageSize.Width / 2, doc.PageSize.Height / 2, 5, 10, new Func<double [ , ], double [ , ]>(arr => {
							arr = arr.rotate(-45);
							return arr;
						}), 2);
						cb.SetRGBColorFill(255, 10, 100);
						cb.SetRGBColorStroke(255, 10, 100);
						cb.FillStroke();


					}
					catch ( Exception ex ) {
						throw ex;
					}
					finally {
						doc.Close();
					}
				}
			}));
		}
		/// <summary>
		/// 生成箭头
		/// </summary>
		/// <param name="cb"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="r">逆时针旋转角度</param>
		private static void generateArrow(PdfContentByte cb,double x,double y,int w,int h,Func<double[,],double[,]> turnFunc,int type=1) {
			switch ( type ) {
				case 0:
					double [ ,] arr0 = new double [ ,] { { 0,0}, { w,h}, {0,-0.2*h }, {-w,h} };
					arr0= turnFunc(arr0);
					cb.MoveTo(x + arr0 [0, 0], y + arr0 [0, 1]);
					cb.LineTo(x + arr0[1,0], y + arr0[1,1]);
					cb.CurveTo(x+arr0[2,0], y+arr0[2,1], x+arr0[3,0], y +arr0[3,1]);
					break;
				case 1:
					double [ , ] arr1 = new double [ , ] { { 0, 0 }, { w, h }, { 0, h * 0.432 }, { -w, h } };
					arr1 = turnFunc(arr1);
					cb.MoveTo(x + arr1 [0, 0], y + arr1 [0, 1]);
					cb.LineTo(x + arr1 [1, 0], y + arr1 [1, 1]);
					cb.LineTo(x+arr1[2,0], y + arr1[2,1]);
					cb.LineTo(x+arr1[3,0], y +arr1[3,1]);
					break;
				case 2:
					double [ , ] arr2 = new double [ , ] { { 0, 0 }, { w, h }, { w * 0.25, h * 0.5 }, {0,0.38*h }, {-0.25*w,0.5*h }, { -w, h } };
					arr2 = turnFunc(arr2);
					cb.MoveTo(x+arr2[0,0], y+arr2[0,1]);
					cb.LineTo(x + arr2 [1, 0], y + arr2 [1, 1]);
					cb.LineTo(x+arr2[2,0],y+arr2[2,1]);
					cb.CurveTo(x+arr2[3,0],y+arr2[3,1],x+arr2[4,0], y + arr2[4,1]);
					cb.LineTo(x+arr2[5,0], y +arr2[5,1]);
					break;
				default:
					break;
			}
			cb.ClosePath();
		}
        /// <summary>
        /// shengchengquxianjiantou  zhuyaoshijiantouhudujisuan
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="type"></param>
        /// <param name="arcPoints"></param>
        /// <param name="arrows"></param>
        /// <param name="arrowFunc">canshuyicishi jiantoudianzuobiao hudu</param>
        private static void generatArcArrow(PdfContentByte cb,int type,double[,] arcPoints,double[,] arrows,Action<double,double,int> arrowFunc) {
            switch ( type ) {
                case 1://bl
                    if ( arcPoints.Length / 2 > 2 ) {
                        cb.MoveTo(arcPoints [0, 0], arcPoints [0, 1]);
                        if ( arcPoints.Length / 2 == 3 )
                            cb.CurveTo(arcPoints [1, 0], arcPoints [1, 1], arcPoints [2, 0], arcPoints [2, 1]);
                        else if ( arcPoints.Length / 2 == 4 )
                            cb.CurveTo(arcPoints [1, 0], arcPoints [1, 1], arcPoints [2, 0], arcPoints [2, 1], arcPoints [3, 0], arcPoints [3, 1]);
                    }
                    break;
                case 0://arc
                    if (arcPoints.Length/2==4) {
                        cb.MoveTo(arcPoints[0,0],arcPoints[0,1]);
                        cb.Arc(arcPoints [1, 0], arcPoints [1, 1], arcPoints [2, 0], arcPoints [2, 1], arcPoints [3, 0], arcPoints [3, 1]);
                    }
                    break;
                default:
                    break;
            }
        }


		private static void generateList<T>(Document doc, out Paragraph paragraph, string text, Func<T> func, Func<T, T> action = null) where T : List {
			T list = func();

			list.IndentationLeft = 30f;


			list.Add(new ListItem("One"));

			list.Add("Two");

			list.Add("Three");

			list.Add("Four");

			list.Add("Five");

			if ( action != null ) {
				list = action(list);
			}

			paragraph = new Paragraph();

			paragraph.Add(text);

			doc.Add(paragraph);

			doc.Add(list);
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
			switch ( this.comboBox1.SelectedIndex ) {
				case 0:
					Button btn = sender as Button;
					if ( btn != null )
						CreatePDF("dfsdfsdfffffffffffffffffffffff汉字玩儿玩儿");
					break;
				case 1:
					CreatePdfSetInfo(readFileByUTF8(this.urlOrPath_text.Text));
					break;
				case 2:
					CreateNewPdfPage(readFileByUTF8(this.urlOrPath_text.Text));
					break;
				case 3:
					//ImageDirect(@"C:\Users\Administrator\Desktop\js\layer\skin\default\icon.png");
					ImageDirect(@"http://dl.eteams.cn/site/44bc9b1d-9d59-4b96-b022-5e432cc803f3");
					break;
				case 4:
					CreateTable();
					break;
				case 5:
					var list = new List<Class1>();
					for ( int i = 0; i < 100; i++ ) {
						list.Add(new Class1() { Name = "name" + i, Number = i, Score = Math.Floor(new Random().Next(1000) * Math.Sqrt(( i + 10 ) * Math.PI * Math.E)) / 100, IsActive = i % 3 == 0, CreateTime = DateTime.Now.AddDays(-i) });
					}
					CreateTable<Class1>(list, new float [ ] { 0.1f, 0.15f, 0.15f, 0.35f, 0.15f, 0.15f });
					break;
				case 6:
					CreateLink();
					break;
				case 7:
					CreateBookMark();
					break;
				case 8:
					ExportPdf();
					break;
				case 9:
					CreateImgText();
					break;
				case 10:
					CreateList();
					break;
				case 11:
					CreateImg();
					break;
				default:
					break;
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
		private static string readFileByUTF8(string path) {
			try {
				OpenFileDialog open = new OpenFileDialog();
				//默认目录
				open.InitialDirectory = System.Windows.Forms.Application.StartupPath.Replace(@"bin\Debug", "") + @"\public\workfile\";
				//设置文件保存类型
				open.Filter = "text|*.txt|pdf|*.pdf|md文件|*.md|html文件|*.html|doc文件|*.doc|docx文件|*.docx|所有文件|*.*";
				//保存对话框是否记忆上次打开的目录  
				open.RestoreDirectory = true;
				//设置标题
				open.Title = "打开";
				if ( open.ShowDialog() == DialogResult.OK ) {
					using ( var reader = new StreamReader(open.FileName, Encoding.UTF8) ) {
						return reader.ReadToEnd();
					}
				}
			}
			catch ( Exception ex ) {
				MessageBox.Show(ex.Message);
			}
			return "";
		}

		private void readPdf_btn_Click(object sender, EventArgs e) {
			ReadPdf();
		}

		private void ReadPdf() {
			if ( File.Exists(this.pdfpath_text.Text) && Path.GetExtension(this.pdfpath_text.Text) == ".pdf" ) {
				saveFile(new Action<string>((savePath) => {
					readPdf(this.pdfpath_text.Text, savePath);
					this.pdfpath_text.Text = savePath;
				}));
			}
			else {
				openFile(new Action<string>((fileName) => {
					this.pdfpath_text.Text = fileName;
					saveFile(new Action<string>((savePath) => {
						readPdf(fileName, savePath);
						this.pdfpath_text.Text = savePath;
					}));
				}));
			}
		}

		private static void readPdf(string fileName, string savePath) {
			using ( var fs = new FileStream(savePath, FileMode.Create) ) {
				// 创建一个PdfReader对象
				PdfReader reader = new PdfReader(fileName);
				// 获得文档页数
				int n = reader.NumberOfPages;
				// 获得第一页的大小
				iTextSharp.text.Rectangle psize = reader.GetPageSize(1);
				float width = psize.Width;
				float height = psize.Height;
				// 创建一个文档变量
				Document document = new Document(psize, 50, 50, 50, 50);
				// 创建该文档
				PdfWriter writer = PdfWriter.GetInstance(document, fs);
				// 打开文档
				document.Open();
				// 添加内容
				PdfContentByte cb = writer.DirectContent;
				int i = 0;
				int p = 0;
				Console.WriteLine("一共有 " + n + " 页.");
				while ( i < n ) {
					document.NewPage();
					p++;
					i++;
					PdfImportedPage page1 = writer.GetImportedPage(reader, i);
					cb.AddTemplate(page1, .5f, 0, 0, .5f, 0, height / 2);
					Console.WriteLine("处理第 " + i + " 页");
					if ( i < n ) {
						i++;
						PdfImportedPage page2 = writer.GetImportedPage(reader, i);
						cb.AddTemplate(page2, .5f, 0, 0, .5f, width / 2, height / 2);
						Console.WriteLine("处理第 " + i + " 页");
					}
					if ( i < n ) {
						i++;
						PdfImportedPage page3 = writer.GetImportedPage(reader, i);
						cb.AddTemplate(page3, .5f, 0, 0, .5f, 0, 0);
						Console.WriteLine("处理第 " + i + " 页");
					}
					if ( i < n ) {
						i++;
						PdfImportedPage page4 = writer.GetImportedPage(reader, i);
						cb.AddTemplate(page4, .5f, 0, 0, .5f, width / 2, 0);
						Console.WriteLine("处理第 " + i + " 页");
					}
					cb.SetRGBColorStroke(255, 0, 0);
					cb.MoveTo(0, height / 2);
					cb.LineTo(width, height / 2);
					cb.Stroke();
					cb.MoveTo(width / 2, height);
					cb.LineTo(width / 2, 0);
					cb.Stroke();
					BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
					cb.BeginText();
					cb.SetFontAndSize(bf, 14);
					cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "page " + p + " of " + ( ( n / 4 ) + ( n % 4 > 0 ? 1 : 0 ) ), width / 2, 40, 0);
					cb.EndText();
				}
				// 关闭文档
				document.Close();
			}
		}
	}
}
