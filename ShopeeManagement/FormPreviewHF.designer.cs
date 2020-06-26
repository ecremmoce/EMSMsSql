namespace ShopeeManagement
{
    partial class FormPreviewHF
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPreviewHF));
            this.TxtHFContent = new System.Windows.Forms.RichTextBox();
            this.BtnClose = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // TxtHFContent
            // 
            this.TxtHFContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtHFContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtHFContent.Location = new System.Drawing.Point(23, 32);
            this.TxtHFContent.Name = "TxtHFContent";
            this.TxtHFContent.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.TxtHFContent.Size = new System.Drawing.Size(1141, 858);
            this.TxtHFContent.TabIndex = 277;
            this.TxtHFContent.Text = "";
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(1179, 33);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(61, 23);
            this.BtnClose.TabIndex = 278;
            this.BtnClose.Text = "닫기";
            this.BtnClose.UseCustomBackColor = true;
            this.BtnClose.UseSelectable = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // FormPreviewHF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1254, 919);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.TxtHFContent);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPreviewHF";
            this.Resizable = false;
            this.Load += new System.EventHandler(this.FormPreviewHF_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroButton BtnClose;
        public System.Windows.Forms.RichTextBox TxtHFContent;
    }
}