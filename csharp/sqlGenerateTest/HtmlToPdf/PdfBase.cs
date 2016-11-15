using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HtmlToPdf {
	public class PdfBase : PdfPageEventHelper {

		#region 属性  
		private String _fontFilePathForHeaderFooter = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "SIMHEI.TTF");
		/// <summary>  
		/// 页眉/页脚所用的字体  
		/// </summary>  
		public String FontFilePathForHeaderFooter {
			get {
				return _fontFilePathForHeaderFooter;
			}

			set {
				_fontFilePathForHeaderFooter = value;
			}
		}

		private String _fontFilePathForBody = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "SIMSUN.TTC,1");
		/// <summary>  
		/// 正文内容所用的字体  
		/// </summary>  
		public String FontFilePathForBody {
			get { return _fontFilePathForBody; }
			set { _fontFilePathForBody = value; }
		}


		private PdfPTable _header;
		/// <summary>  
		/// 页眉  
		/// </summary>  
		public PdfPTable Header {
			get { return _header; }
			private set { _header = value; }
		}

		private PdfPTable _footer;
		/// <summary>  
		/// 页脚  
		/// </summary>  
		public PdfPTable Footer {
			get { return _footer; }
			private set { _footer = value; }
		}


		private BaseFont _baseFontForHeaderFooter;
		/// <summary>  
		/// 页眉页脚所用的字体  
		/// </summary>  
		public BaseFont BaseFontForHeaderFooter {
			get { return _baseFontForHeaderFooter; }
			set { _baseFontForHeaderFooter = value; }
		}

		private BaseFont _baseFontForBody;
		/// <summary>  
		/// 正文所用的字体  
		/// </summary>  
		public BaseFont BaseFontForBody {
			get { return _baseFontForBody; }
			set { _baseFontForBody = value; }
		}

		private Document _document;
		/// <summary>  
		/// PDF的Document  
		/// </summary>  
		public Document Document {
			get { return _document; }
			private set { _document = value; }
		}

		#endregion

		#region GenerateHeader  
		/// <summary>  
		/// 生成页眉  
		/// </summary>  
		/// <param name="writer"></param>  
		/// <returns></returns>  
		public virtual PdfPTable GenerateHeader(iTextSharp.text.pdf.PdfWriter writer) {
			return null;
		}
		#endregion

		#region GenerateFooter  
		/// <summary>  
		/// 生成页脚  
		/// </summary>  
		/// <param name="writer"></param>  
		/// <returns></returns>  
		public virtual PdfPTable GenerateFooter(iTextSharp.text.pdf.PdfWriter writer) {
			return null;
		}
		#endregion
		public override void OnOpenDocument(PdfWriter writer, Document document) {
			try {
				BaseFontForHeaderFooter = BaseFont.CreateFont(FontFilePathForHeaderFooter, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
				BaseFontForBody = BaseFont.CreateFont(FontFilePathForBody, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
				Document = document;
			}
			catch ( DocumentException de ) {

			}
			catch ( System.IO.IOException ioe ) {

			}
		}
		public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document) {
			base.OnEndPage(writer, document);

			//输出页眉  
			Header = GenerateHeader(writer);
			Header.TotalWidth = document.PageSize.Width - 20f;
			///调用PdfTable的WriteSelectedRows方法。该方法以第一个参数作为开始行写入。  
			///第二个参数-1表示没有结束行，并且包含所写的所有行。  
			///第三个参数和第四个参数是开始写入的坐标x和y.  
			Header.WriteSelectedRows(0, -1, 10, document.PageSize.Height - 20, writer.DirectContent);

			//输出页脚  
			Footer = GenerateFooter(writer);
			Footer.TotalWidth = document.PageSize.Width - 20f;
			Footer.WriteSelectedRows(0, -1, 10, document.PageSize.GetBottom(50), writer.DirectContent);
		}
	}

	/// <summary>  
	/// PDF报表  
	/// </summary>  
	public class PDFReportTest : PdfBase {
		#region 属性  
		private int _pageRowCount = 12;
		/// <summary>  
		/// 每页的数据行数  
		/// </summary>  
		public int PageRowCount {
			get { return _pageRowCount; }
			set { _pageRowCount = value; }
		}
		#endregion

		/// <summary>  
		/// 模拟数据。实际时可能需要从数据库或网络中获取.  
		/// </summary>  
		private DataTable _dtSimulateData = null;

		private DataTable GenerateSimulateData() {
			DataTable dtSimulateData = new DataTable();
			dtSimulateData.Columns.Add("Order", typeof(int));//序号  
			dtSimulateData.Columns.Add("No");//料号  
			dtSimulateData.Columns.Add("Name");//名称  
			dtSimulateData.Columns.Add("Type");//类型  
			dtSimulateData.Columns.Add("GetTime");//领取时间  
			dtSimulateData.Columns.Add("BackTime");//归还时间  
			dtSimulateData.Columns.Add("Remark");//备注  
			DataRow row = null;
			String [ ] types = new string [ ] { "文具", "辅料", "生活用品", "测试用具", "实体机" };
			DateTime getDate = DateTime.Now;
			DateTime backDate = DateTime.Now;
			for ( int i = 0; i < 100; i++ ) {
				row = dtSimulateData.NewRow();
				row ["Order"] = i + 1;
				row ["No"] = ( 10000 + i + 1 ).ToString();
				row ["Name"] = Guid.NewGuid().ToString().Substring(0, 5);//造出随机名称  
				row ["Type"] = types [( i * 3 ) % 5];
				row ["GetTime"] = getDate.AddDays(i).ToString("yyyy-MM-dd");
				row ["BackTime"] = ( ( i + i ) % 3 == 0 ) ? "" : ( backDate.AddDays(i + ( i * 3 ) % 8).ToString("yyyy-MM-dd") );//造出没有归还时间的数据  
				row ["Remark"] = "XXXXXXX";
				dtSimulateData.Rows.Add(row);
			}
			return dtSimulateData;
		}

		#region GenerateHeader  
		/// <summary>  
		/// 生成页眉  
		/// </summary>  
		/// <param name="fontFilePath"></param>  
		/// <param name="pageNumber"></param>  
		/// <returns></returns>  
		public override PdfPTable GenerateHeader(iTextSharp.text.pdf.PdfWriter writer) {
			BaseFont baseFont = BaseFontForHeaderFooter;
			iTextSharp.text.Font font_logo = new iTextSharp.text.Font(baseFont, 30, iTextSharp.text.Font.BOLD);
			iTextSharp.text.Font font_header1 = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);
			iTextSharp.text.Font font_header2 = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);
			iTextSharp.text.Font font_headerContent = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL);

			float [ ] widths = new float [ ] { 100, 300, 50, 90, 15, 20, 15 };

			PdfPTable header = new PdfPTable(widths);
			PdfPCell cell = new PdfPCell();
			cell.BorderWidthBottom = 2;
			cell.BorderWidthLeft = 2;
			cell.BorderWidthTop = 2;
			cell.BorderWidthRight = 2;
			cell.FixedHeight = 35;
			cell.Phrase = new Phrase("Coco King", font_logo);
			cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
			cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
			cell.PaddingTop = -1;
			header.AddCell(cell);

			cell = GenerateOnlyBottomBorderCell(2, iTextSharp.text.Element.ALIGN_LEFT);
			cell.Phrase = new Paragraph("PDF报表示例", font_header1);
			header.AddCell(cell);


			cell = GenerateOnlyBottomBorderCell(2, iTextSharp.text.Element.ALIGN_RIGHT);
			cell.Phrase = new Paragraph("日期:", font_header2);
			header.AddCell(cell);

			cell = GenerateOnlyBottomBorderCell(2, iTextSharp.text.Element.ALIGN_LEFT);
			cell.Phrase = new Paragraph(DateTime.Now.ToShortDateString(), font_headerContent);
			header.AddCell(cell);

			cell = GenerateOnlyBottomBorderCell(2, iTextSharp.text.Element.ALIGN_RIGHT);
			cell.Phrase = new Paragraph("第", font_header2);
			header.AddCell(cell);

			cell = GenerateOnlyBottomBorderCell(2, iTextSharp.text.Element.ALIGN_CENTER);
			cell.Phrase = new Paragraph(writer.PageNumber.ToString(), font_headerContent);
			header.AddCell(cell);

			cell = GenerateOnlyBottomBorderCell(2, iTextSharp.text.Element.ALIGN_RIGHT);
			cell.Phrase = new Paragraph("页", font_header2);
			header.AddCell(cell);
			return header;
		}

		#region GenerateOnlyBottomBorderCell  
		/// <summary>  
		/// 生成只有底边的cell  
		/// </summary>  
		/// <param name="bottomBorder"></param>  
		/// <param name="horizontalAlignment">水平对齐方式<see cref="iTextSharp.text.Element"/></param>  
		/// <returns></returns>  
		private PdfPCell GenerateOnlyBottomBorderCell(int bottomBorder,
															int horizontalAlignment) {
			PdfPCell cell = GenerateOnlyBottomBorderCell(bottomBorder, horizontalAlignment, iTextSharp.text.Element.ALIGN_BOTTOM);
			cell.PaddingBottom = 5;
			return cell;
		}

		/// <summary>  
		/// 生成只有底边的cell  
		/// </summary>  
		/// <param name="bottomBorder"></param>  
		/// <param name="horizontalAlignment">水平对齐方式<see cref="iTextSharp.text.Element"/></param>  
		/// <param name="verticalAlignment">垂直对齐方式<see cref="iTextSharp.text.Element"/</param>  
		/// <returns></returns>  
		private PdfPCell GenerateOnlyBottomBorderCell(int bottomBorder,
															int horizontalAlignment,
															int verticalAlignment) {
			PdfPCell cell = GenerateOnlyBottomBorderCell(bottomBorder);
			cell.HorizontalAlignment = horizontalAlignment;
			cell.VerticalAlignment = verticalAlignment; ;
			return cell;
		}

		/// <summary>  
		/// 生成只有底边的cell  
		/// </summary>  
		/// <param name="bottomBorder"></param>  
		/// <returns></returns>  
		private PdfPCell GenerateOnlyBottomBorderCell(int bottomBorder) {
			PdfPCell cell = new PdfPCell();
			cell.BorderWidthBottom = 2;
			cell.BorderWidthLeft = 0;
			cell.BorderWidthTop = 0;
			cell.BorderWidthRight = 0;
			return cell;
		}
		#endregion

		#endregion

		#region GenerateFooter  
		public override PdfPTable GenerateFooter(iTextSharp.text.pdf.PdfWriter writer) {
			BaseFont baseFont = BaseFontForHeaderFooter;
			iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL);

			PdfPTable footer = new PdfPTable(3);
			AddFooterCell(footer, "审阅:", font);
			AddFooterCell(footer, "审批:", font);
			AddFooterCell(footer, "制表:coco", font);
			return footer;
		}

		private void AddFooterCell(PdfPTable foot, String text, iTextSharp.text.Font font, int horizontalAlignment= iTextSharp.text.Element.ALIGN_CENTER) {
			PdfPCell cell = new PdfPCell();
			cell.BorderWidthTop = 2;
			cell.BorderWidthRight = 0;
			cell.BorderWidthBottom = 0;
			cell.BorderWidthLeft = 0;
			cell.Phrase = new Phrase(text, font);
			cell.HorizontalAlignment = horizontalAlignment;
			foot.AddCell(cell);
		}
		#endregion

		#region ExportPDF  
		/// <summary>  
		/// 导出PDF  
		/// </summary>  
		/// <param name="path">导出路径</param>  
		public static void ExportPDF(String path) {
			using (var fs = new FileStream(path, FileMode.Create) ) {
				PDFReportTest pdfReport = new PDFReportTest();
				Document document = new Document(PageSize.A4.Rotate(), -90, -90, 60, 10);//此处设置的偏移量是为了加大页面的可用范围，可以使用默认.  
				PdfWriter pdfWriter = PdfWriter.GetInstance(document,fs);
				pdfWriter.PageEvent = pdfReport;//此处一定要赋值，才能触发页眉和页脚的处理  
				document.Open();
				pdfReport.AddBody(document);
				document.Close();
			}
		}

		/// <summary>  
		/// 导出PDF  
		/// </summary>  
		public static byte [ ] ExportPDF() {
			using ( var ms = new MemoryStream() ) {
				PDFReportTest pdfReport = new PDFReportTest();
				Document document = new Document(PageSize.A4.Rotate(), -90, -90, 60, 10);//此处设置的偏移量是为了加大页面的可用范围，可以使用默认.  
				PdfWriter pdfWriter = PdfWriter.GetInstance(document, ms);
				pdfWriter.PageEvent = pdfReport;//此处一定要赋值，才能触发页眉和页脚的处理  
				document.Open();
				pdfReport.AddBody(document);
				document.Close();
				byte [ ] buff = ms.ToArray();
				return buff;
			}
		}
		#endregion

		#region AddBody  
		private void AddBody(Document document) {
			_dtSimulateData = GenerateSimulateData();
			int count = ( _dtSimulateData.Rows.Count + ( PageRowCount - 1 ) ) / PageRowCount;
			for ( int i = 0; i < count; i++ ) {
				AddBodySinglePage(i + 1);
				document.NewPage();
			}
		}

		private void AddBodySinglePage(int pageNumber) {
			BaseFont baseFont = BaseFontForBody;
			iTextSharp.text.Font font_columnHeader = new iTextSharp.text.Font(baseFont, 11f, iTextSharp.text.Font.BOLD);
			iTextSharp.text.Font font_contentNormal = new iTextSharp.text.Font(baseFont, 9.5f, iTextSharp.text.Font.NORMAL);
			iTextSharp.text.Font font_contentSmall = new iTextSharp.text.Font(baseFont, 8f, iTextSharp.text.Font.NORMAL);

			int columnCount = 6;//此处是为了当列数很多时，便于循环生成所要的列宽比例  
			float [ ] widths = new float [columnCount];
			for ( int i = 0; i < columnCount; i++ ) {
				widths [i] = 1;
			}
			//需要加宽的再进行额外处理  
			widths [3] = 2;//时间  
			widths [4] = 3;//备注  

			PdfPTable bodyTable = new PdfPTable(widths);
			bodyTable.SpacingBefore = 10;//与头部的距离  

			AddBodyHeader(bodyTable, font_columnHeader);
			AddBodyContent(bodyTable, font_contentNormal, font_contentSmall, pageNumber);

			Document.Add(bodyTable);
		}

		#region AddBodyHeader  
		/// <summary>  
		/// 添加Body的列标题  
		/// </summary>  
		/// <param name="document"></param>  
		/// <param name="font_columnHeader"></param>  
		private void AddBodyHeader(PdfPTable bodyTable, iTextSharp.text.Font font_columnHeader) {
			//采用Rowspan和Colspan来控制单元格的合并与拆分，类似于HTML的Table.  
			AddColumnHeaderCell(bodyTable, "料号", font_columnHeader, 1, 2);
			AddColumnHeaderCell(bodyTable, "名称", font_columnHeader, 1, 2);
			AddColumnHeaderCell(bodyTable, "种类", font_columnHeader, 1, 2);
			AddColumnHeaderCell(bodyTable, "时间", font_columnHeader, 2, 1);//表头下还有两列表头  
			AddColumnHeaderCell(bodyTable, "备注", font_columnHeader, 1, 2, true, true);

			//时间  
			AddColumnHeaderCell(bodyTable, "领取", font_columnHeader);
			AddColumnHeaderCell(bodyTable, "归还", font_columnHeader, true, true);
		}
		#endregion

		#region AddBodyContent Function  
		/// <summary>  
		/// 添加报表正文  
		/// </summary>  
		/// <param name="bodyTable"></param>  
		/// <param name="fontNormal">普通字体</param>  
		/// <param name="fontSmall">小字体，当内容显示不下，需要使用小字体来显示时，可以使用.</param>  
		/// <param name="pageNumber">页码。便于从数据库中按页拉取数据时使用.</param>  
		public void AddBodyContent(PdfPTable bodyTable,
								   iTextSharp.text.Font fontNormal,
								   iTextSharp.text.Font fontSmall,
								   int pageNumber) {
			String filterExpression = String.Format("Order>{0} AND Order<={1}",
													( pageNumber - 1 ) * PageRowCount,
													 pageNumber * PageRowCount);
			DataRow [ ] rows = _dtSimulateData.Select(filterExpression);
			foreach ( var row in rows ) {
				AddBodyContentRow(bodyTable, fontNormal, fontSmall, row);
			}
		}

		private void AddBodyContentRow(PdfPTable bodyTable, iTextSharp.text.Font fontNormal, iTextSharp.text.Font fontSmall, DataRow row) {
			AddBodyContentCell(bodyTable, String.Format("{0}", row ["No"]), fontNormal);//料号  
			AddBodyContentCell(bodyTable, String.Format("{0}", row ["Name"]), fontNormal);//名称  
			AddBodyContentCell(bodyTable, String.Format("{0}", row ["Type"]), fontNormal);//种类  
			AddBodyContentCell(bodyTable, String.Format("{0}", row ["GetTime"]), fontSmall, 1);//时间-领取  
			String backTime = String.Format("{0}", row ["BackTime"]);
			AddBodyContentCell(bodyTable, backTime, fontSmall);//时间-归还  
			AddBodyContentCell(bodyTable, String.Format("{0}", row ["Remark"]), fontSmall, 2, true);//备注    

			AddBodyContentCell(bodyTable, ( String.IsNullOrWhiteSpace(backTime) ) ? "消耗品不需归还" : "", fontSmall, 1);//时间-领取(下方说明)  
		}

		private static void AddBodyContentCell(PdfPTable bodyTable,
											   String text,
											   iTextSharp.text.Font font,
											   int rowspan = 2,
											   bool needRightBorder = false) {
			PdfPCell cell = new PdfPCell();
			float defaultBorder = 0.5f;
			cell.BorderWidthLeft = defaultBorder;
			cell.BorderWidthTop = 0;
			cell.BorderWidthRight = needRightBorder ? defaultBorder : 0;
			cell.BorderWidthBottom = defaultBorder;
			cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
			cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_BASELINE;
			cell.Rowspan = rowspan;
			cell.PaddingBottom = 4;
			cell.Phrase = new Phrase(text, font);
			cell.FixedHeight = 18f;
			bodyTable.AddCell(cell);
		}
		#endregion

		#region AddColumnHeaderCell Function  
		/// <summary>  
		/// 添加列标题单元格  
		/// </summary>  
		/// <param name="table">表格行的单元格列表</param>  
		/// <param name="header">标题</param>  
		/// <param name="font">字段</param>  
		/// <param name="colspan">列空间</param>  
		/// <param name="rowspan">行空间</param>  
		/// <param name="needLeftBorder">是否需要左边框</param>  
		/// <param name="needRightBorder">是否需要右边框</param>  
		public void AddColumnHeaderCell(PdfPTable table,
												String header,
												iTextSharp.text.Font font,
												int colspan,
												int rowspan,
												bool needLeftBorder = true,
												bool needRightBorder = false) {
			PdfPCell cell = GenerateColumnHeaderCell(header, font, needLeftBorder, needRightBorder);
			if ( colspan > 1 ) {
				cell.Colspan = colspan;
			}

			if ( rowspan > 1 ) {
				cell.Rowspan = rowspan;
			}

			table.AddCell(cell);
		}

		/// <summary>  
		/// 添加列标题单元格  
		/// </summary>  
		/// <param name="table">表格</param>  
		/// <param name="header">标题</param>  
		/// <param name="font">字段</param>  
		/// <param name="needLeftBorder">是否需要左边框</param>  
		/// <param name="needRightBorder">是否需要右边框</param>  
		public void AddColumnHeaderCell(PdfPTable table,
												String header,
												iTextSharp.text.Font font,
												bool needLeftBorder = true,
												bool needRightBorder = false) {
			PdfPCell cell = GenerateColumnHeaderCell(header, font, needLeftBorder, needRightBorder);
			table.AddCell(cell);
		}
		#endregion

		#region GenerateColumnHeaderCell  
		/// <summary>  
		/// 生成列标题单元格  
		/// </summary>  
		/// <param name="header">标题</param>  
		/// <param name="font">字段</param>  
		/// <param name="needLeftBorder">是否需要左边框</param>  
		/// <param name="needRightBorder">是否需要右边框</param>  
		/// <returns></returns>  
		private PdfPCell GenerateColumnHeaderCell(String header,
														iTextSharp.text.Font font,
														bool needLeftBorder = true,
														bool needRightBorder = false) {
			PdfPCell cell = new PdfPCell();
			float border = 0.5f;
			cell.BorderWidthBottom = border;
			if ( needLeftBorder ) {
				cell.BorderWidthLeft = border;
			}
			else {
				cell.BorderWidthLeft = 0;
			}

			cell.BorderWidthTop = border;
			if ( needRightBorder ) {
				cell.BorderWidthRight = border;
			}
			else {
				cell.BorderWidthRight = 0;
			}

			cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
			cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_BASELINE;
			cell.PaddingBottom = 4;
			cell.Phrase = new Phrase(header, font);
			return cell;
		}
		#endregion

		#endregion
	}
}
