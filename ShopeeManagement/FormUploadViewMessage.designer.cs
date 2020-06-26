namespace ShopeeManagement
{
    partial class FormUploadViewMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUploadViewMessage));
            this.TxtMessage = new System.Windows.Forms.RichTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.BtnClose = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // TxtMessage
            // 
            this.TxtMessage.Location = new System.Drawing.Point(23, 82);
            this.TxtMessage.Name = "TxtMessage";
            this.TxtMessage.Size = new System.Drawing.Size(1051, 583);
            this.TxtMessage.TabIndex = 0;
            this.TxtMessage.Text = "";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(37, 60);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(115, 19);
            this.metroLabel1.TabIndex = 1;
            this.metroLabel1.Text = "등록 결과 메시지";
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(967, 38);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(107, 38);
            this.BtnClose.TabIndex = 40;
            this.BtnClose.Text = "닫기";
            this.BtnClose.UseCustomBackColor = true;
            this.BtnClose.UseSelectable = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // FormUploadViewMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 688);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.TxtMessage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormUploadViewMessage";
            this.Load += new System.EventHandler(this.FormUploadViewMessage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroButton BtnClose;
        public System.Windows.Forms.RichTextBox TxtMessage;
    }
}