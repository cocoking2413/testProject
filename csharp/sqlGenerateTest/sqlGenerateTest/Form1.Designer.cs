namespace sqlGenerateTest {
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
			this.result_text = new System.Windows.Forms.RichTextBox();
			this.run_btn = new System.Windows.Forms.Button();
			this.sql_btn = new System.Windows.Forms.Button();
			this.test_btn = new System.Windows.Forms.Button();
			this.json_text = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// result_text
			// 
			this.result_text.Location = new System.Drawing.Point(449, 42);
			this.result_text.Name = "result_text";
			this.result_text.Size = new System.Drawing.Size(588, 444);
			this.result_text.TabIndex = 0;
			this.result_text.Text = "";
			// 
			// run_btn
			// 
			this.run_btn.Location = new System.Drawing.Point(354, 42);
			this.run_btn.Name = "run_btn";
			this.run_btn.Size = new System.Drawing.Size(75, 23);
			this.run_btn.TabIndex = 1;
			this.run_btn.Text = "生成";
			this.run_btn.UseVisualStyleBackColor = true;
			this.run_btn.Click += new System.EventHandler(this.run_btn_Click);
			// 
			// sql_btn
			// 
			this.sql_btn.Location = new System.Drawing.Point(354, 96);
			this.sql_btn.Name = "sql_btn";
			this.sql_btn.Size = new System.Drawing.Size(75, 23);
			this.sql_btn.TabIndex = 2;
			this.sql_btn.Text = "查询";
			this.sql_btn.UseVisualStyleBackColor = true;
			this.sql_btn.Click += new System.EventHandler(this.sql_btn_Click);
			// 
			// test_btn
			// 
			this.test_btn.Location = new System.Drawing.Point(354, 151);
			this.test_btn.Name = "test_btn";
			this.test_btn.Size = new System.Drawing.Size(75, 23);
			this.test_btn.TabIndex = 3;
			this.test_btn.Text = "test";
			this.test_btn.UseVisualStyleBackColor = true;
			this.test_btn.Click += new System.EventHandler(this.test_btn_Click);
			// 
			// json_text
			// 
			this.json_text.Location = new System.Drawing.Point(12, 42);
			this.json_text.Name = "json_text";
			this.json_text.Size = new System.Drawing.Size(322, 444);
			this.json_text.TabIndex = 4;
			this.json_text.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1049, 498);
			this.Controls.Add(this.json_text);
			this.Controls.Add(this.test_btn);
			this.Controls.Add(this.sql_btn);
			this.Controls.Add(this.run_btn);
			this.Controls.Add(this.result_text);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox result_text;
		private System.Windows.Forms.Button run_btn;
		private System.Windows.Forms.Button sql_btn;
		private System.Windows.Forms.Button test_btn;
		private System.Windows.Forms.RichTextBox json_text;
	}
}

