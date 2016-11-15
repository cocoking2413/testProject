namespace sqlGenerateTest {
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
            this.open_path = new System.Windows.Forms.TextBox();
            this.save_path = new System.Windows.Forms.TextBox();
            this.run_btn = new System.Windows.Forms.Button();
            this.save_btn = new System.Windows.Forms.Button();
            this.template_text = new System.Windows.Forms.RichTextBox();
            this.tabelName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // open_btn
            // 
            this.open_btn.Location = new System.Drawing.Point(520, 26);
            this.open_btn.Name = "open_btn";
            this.open_btn.Size = new System.Drawing.Size(75, 23);
            this.open_btn.TabIndex = 0;
            this.open_btn.Text = "open";
            this.open_btn.UseVisualStyleBackColor = true;
            this.open_btn.Click += new System.EventHandler(this.open_btn_Click);
            // 
            // open_path
            // 
            this.open_path.Location = new System.Drawing.Point(30, 26);
            this.open_path.Name = "open_path";
            this.open_path.Size = new System.Drawing.Size(484, 21);
            this.open_path.TabIndex = 1;
            // 
            // save_path
            // 
            this.save_path.Location = new System.Drawing.Point(31, 62);
            this.save_path.Name = "save_path";
            this.save_path.Size = new System.Drawing.Size(482, 21);
            this.save_path.TabIndex = 3;
            // 
            // run_btn
            // 
            this.run_btn.Location = new System.Drawing.Point(520, 102);
            this.run_btn.Name = "run_btn";
            this.run_btn.Size = new System.Drawing.Size(75, 23);
            this.run_btn.TabIndex = 2;
            this.run_btn.Text = "run";
            this.run_btn.UseVisualStyleBackColor = true;
            this.run_btn.Click += new System.EventHandler(this.run_btn_Click);
            // 
            // save_btn
            // 
            this.save_btn.Location = new System.Drawing.Point(520, 62);
            this.save_btn.Name = "save_btn";
            this.save_btn.Size = new System.Drawing.Size(75, 23);
            this.save_btn.TabIndex = 4;
            this.save_btn.Text = "save";
            this.save_btn.UseVisualStyleBackColor = true;
            this.save_btn.Click += new System.EventHandler(this.save_btn_Click);
            // 
            // template_text
            // 
            this.template_text.Location = new System.Drawing.Point(32, 134);
            this.template_text.Name = "template_text";
            this.template_text.Size = new System.Drawing.Size(482, 147);
            this.template_text.TabIndex = 5;
            this.template_text.Text = "";
            // 
            // tabelName
            // 
            this.tabelName.Location = new System.Drawing.Point(133, 102);
            this.tabelName.Name = "tabelName";
            this.tabelName.Size = new System.Drawing.Size(380, 21);
            this.tabelName.TabIndex = 6;
            this.tabelName.Text = "oa_WorkBlog";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "tabel Name :";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 293);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabelName);
            this.Controls.Add(this.template_text);
            this.Controls.Add(this.save_btn);
            this.Controls.Add(this.save_path);
            this.Controls.Add(this.run_btn);
            this.Controls.Add(this.open_path);
            this.Controls.Add(this.open_btn);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button open_btn;
        private System.Windows.Forms.TextBox open_path;
        private System.Windows.Forms.TextBox save_path;
        private System.Windows.Forms.Button run_btn;
        private System.Windows.Forms.Button save_btn;
        private System.Windows.Forms.RichTextBox template_text;
        private System.Windows.Forms.TextBox tabelName;
        private System.Windows.Forms.Label label1;
    }
}