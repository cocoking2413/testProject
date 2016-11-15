namespace HtmlToPdf {
	partial class Form1 {
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent() {
			this.turn_btn = new System.Windows.Forms.Button();
			this.urlOrPath_text = new System.Windows.Forms.TextBox();
			this.pdfpath_text = new System.Windows.Forms.TextBox();
			this.open_btn = new System.Windows.Forms.Button();
			this.openpdf_btn = new System.Windows.Forms.Button();
			this.explore_btn = new System.Windows.Forms.Button();
			this.test_btn = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.readPdf_btn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// turn_btn
			// 
			this.turn_btn.Location = new System.Drawing.Point(13, 140);
			this.turn_btn.Name = "turn_btn";
			this.turn_btn.Size = new System.Drawing.Size(75, 23);
			this.turn_btn.TabIndex = 0;
			this.turn_btn.Text = "转换";
			this.turn_btn.UseVisualStyleBackColor = true;
			this.turn_btn.Click += new System.EventHandler(this.turn_btn_Click);
			// 
			// urlOrPath_text
			// 
			this.urlOrPath_text.Location = new System.Drawing.Point(13, 37);
			this.urlOrPath_text.Name = "urlOrPath_text";
			this.urlOrPath_text.Size = new System.Drawing.Size(441, 21);
			this.urlOrPath_text.TabIndex = 1;
			// 
			// pdfpath_text
			// 
			this.pdfpath_text.Location = new System.Drawing.Point(13, 190);
			this.pdfpath_text.Name = "pdfpath_text";
			this.pdfpath_text.Size = new System.Drawing.Size(441, 21);
			this.pdfpath_text.TabIndex = 2;
			// 
			// open_btn
			// 
			this.open_btn.Location = new System.Drawing.Point(13, 248);
			this.open_btn.Name = "open_btn";
			this.open_btn.Size = new System.Drawing.Size(101, 23);
			this.open_btn.TabIndex = 3;
			this.open_btn.Text = "打开所在目录";
			this.open_btn.UseVisualStyleBackColor = true;
			this.open_btn.Click += new System.EventHandler(this.open_btn_Click);
			// 
			// openpdf_btn
			// 
			this.openpdf_btn.Location = new System.Drawing.Point(139, 248);
			this.openpdf_btn.Name = "openpdf_btn";
			this.openpdf_btn.Size = new System.Drawing.Size(75, 23);
			this.openpdf_btn.TabIndex = 4;
			this.openpdf_btn.Text = "打开";
			this.openpdf_btn.UseVisualStyleBackColor = true;
			this.openpdf_btn.Click += new System.EventHandler(this.openpdf_btn_Click);
			// 
			// explore_btn
			// 
			this.explore_btn.Location = new System.Drawing.Point(13, 92);
			this.explore_btn.Name = "explore_btn";
			this.explore_btn.Size = new System.Drawing.Size(75, 23);
			this.explore_btn.TabIndex = 5;
			this.explore_btn.Text = "浏览";
			this.explore_btn.UseVisualStyleBackColor = true;
			this.explore_btn.Click += new System.EventHandler(this.explore_btn_Click);
			// 
			// test_btn
			// 
			this.test_btn.Location = new System.Drawing.Point(379, 92);
			this.test_btn.Name = "test_btn";
			this.test_btn.Size = new System.Drawing.Size(75, 23);
			this.test_btn.TabIndex = 6;
			this.test_btn.Text = "test";
			this.test_btn.UseVisualStyleBackColor = true;
			this.test_btn.Click += new System.EventHandler(this.test_btn_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(113, 92);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(245, 20);
			this.comboBox1.TabIndex = 7;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// readPdf_btn
			// 
			this.readPdf_btn.Location = new System.Drawing.Point(242, 248);
			this.readPdf_btn.Name = "readPdf_btn";
			this.readPdf_btn.Size = new System.Drawing.Size(75, 23);
			this.readPdf_btn.TabIndex = 8;
			this.readPdf_btn.Text = "修改pdf";
			this.readPdf_btn.UseVisualStyleBackColor = true;
			this.readPdf_btn.Click += new System.EventHandler(this.readPdf_btn_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(471, 337);
			this.Controls.Add(this.readPdf_btn);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.test_btn);
			this.Controls.Add(this.explore_btn);
			this.Controls.Add(this.openpdf_btn);
			this.Controls.Add(this.open_btn);
			this.Controls.Add(this.pdfpath_text);
			this.Controls.Add(this.urlOrPath_text);
			this.Controls.Add(this.turn_btn);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button turn_btn;
		private System.Windows.Forms.TextBox urlOrPath_text;
		private System.Windows.Forms.TextBox pdfpath_text;
		private System.Windows.Forms.Button open_btn;
		private System.Windows.Forms.Button openpdf_btn;
		private System.Windows.Forms.Button explore_btn;
		private System.Windows.Forms.Button test_btn;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button readPdf_btn;
	}
}

