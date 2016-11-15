namespace HtmlToPdf {
    partial class Form2 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if ( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.open_btn = new System.Windows.Forms.Button();
            this.htmlpath_text = new System.Windows.Forms.TextBox();
            this.pdfpath_text = new System.Windows.Forms.TextBox();
            this.preview_btn = new System.Windows.Forms.Button();
            this.run_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // open_btn
            // 
            this.open_btn.Location = new System.Drawing.Point(413, 34);
            this.open_btn.Name = "open_btn";
            this.open_btn.Size = new System.Drawing.Size(75, 23);
            this.open_btn.TabIndex = 0;
            this.open_btn.Text = "选择";
            this.open_btn.UseVisualStyleBackColor = true;
            this.open_btn.Click += new System.EventHandler(this.open_btn_Click);
            // 
            // htmlpath_text
            // 
            this.htmlpath_text.Location = new System.Drawing.Point(12, 36);
            this.htmlpath_text.Name = "htmlpath_text";
            this.htmlpath_text.Size = new System.Drawing.Size(395, 21);
            this.htmlpath_text.TabIndex = 1;
            // 
            // pdfpath_text
            // 
            this.pdfpath_text.Location = new System.Drawing.Point(12, 179);
            this.pdfpath_text.Name = "pdfpath_text";
            this.pdfpath_text.Size = new System.Drawing.Size(395, 21);
            this.pdfpath_text.TabIndex = 3;
            // 
            // preview_btn
            // 
            this.preview_btn.Location = new System.Drawing.Point(413, 177);
            this.preview_btn.Name = "preview_btn";
            this.preview_btn.Size = new System.Drawing.Size(75, 23);
            this.preview_btn.TabIndex = 2;
            this.preview_btn.Text = "打开";
            this.preview_btn.UseVisualStyleBackColor = true;
            this.preview_btn.Click += new System.EventHandler(this.preview_btn_Click);
            // 
            // run_btn
            // 
            this.run_btn.Location = new System.Drawing.Point(13, 109);
            this.run_btn.Name = "run_btn";
            this.run_btn.Size = new System.Drawing.Size(75, 23);
            this.run_btn.TabIndex = 4;
            this.run_btn.Text = "转换";
            this.run_btn.UseVisualStyleBackColor = true;
            this.run_btn.Click += new System.EventHandler(this.run_btn_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 378);
            this.Controls.Add(this.run_btn);
            this.Controls.Add(this.pdfpath_text);
            this.Controls.Add(this.preview_btn);
            this.Controls.Add(this.htmlpath_text);
            this.Controls.Add(this.open_btn);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button open_btn;
        private System.Windows.Forms.TextBox htmlpath_text;
        private System.Windows.Forms.TextBox pdfpath_text;
        private System.Windows.Forms.Button preview_btn;
        private System.Windows.Forms.Button run_btn;
    }
}