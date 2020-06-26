namespace ShopeeManagement
{
    partial class FormDownloadData
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
            this.btnDownload = new DevExpress.XtraEditors.SimpleButton();
            this.progressBarControl1 = new DevExpress.XtraEditors.ProgressBarControl();
            this.txtTemplateName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtUpdateVersion = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtCurVersion = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTemplateName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpdateVersion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurVersion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDownload
            // 
            this.btnDownload.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.btnDownload.Appearance.Options.UseFont = true;
            this.btnDownload.Location = new System.Drawing.Point(294, 139);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 0;
            this.btnDownload.Text = "다운로드";
            this.btnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
            // 
            // progressBarControl1
            // 
            this.progressBarControl1.Location = new System.Drawing.Point(17, 106);
            this.progressBarControl1.Name = "progressBarControl1";
            this.progressBarControl1.Size = new System.Drawing.Size(355, 18);
            this.progressBarControl1.TabIndex = 1;
            // 
            // txtTemplateName
            // 
            this.txtTemplateName.Location = new System.Drawing.Point(153, 75);
            this.txtTemplateName.Name = "txtTemplateName";
            this.txtTemplateName.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTemplateName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtTemplateName.Size = new System.Drawing.Size(219, 20);
            this.txtTemplateName.TabIndex = 2;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(74, 76);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(70, 18);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "템플릿 종류";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(17, 48);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(127, 18);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "템플릿 업데이트 버전";
            // 
            // txtUpdateVersion
            // 
            this.txtUpdateVersion.Location = new System.Drawing.Point(153, 47);
            this.txtUpdateVersion.Name = "txtUpdateVersion";
            this.txtUpdateVersion.Properties.Appearance.Options.UseTextOptions = true;
            this.txtUpdateVersion.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtUpdateVersion.Size = new System.Drawing.Size(219, 20);
            this.txtUpdateVersion.TabIndex = 4;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(43, 19);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(101, 18);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "템플릿 현재 버전";
            // 
            // txtCurVersion
            // 
            this.txtCurVersion.Location = new System.Drawing.Point(153, 18);
            this.txtCurVersion.Name = "txtCurVersion";
            this.txtCurVersion.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCurVersion.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCurVersion.Size = new System.Drawing.Size(219, 20);
            this.txtCurVersion.TabIndex = 6;
            // 
            // FormDownloadData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 181);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.txtCurVersion);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtUpdateVersion);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtTemplateName);
            this.Controls.Add(this.progressBarControl1);
            this.Controls.Add(this.btnDownload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "FormDownloadData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "템플릿 데이터 다운로드";
            this.Load += new System.EventHandler(this.FormDownloadData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTemplateName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpdateVersion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurVersion.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnDownload;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl1;
        private DevExpress.XtraEditors.TextEdit txtTemplateName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtUpdateVersion;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtCurVersion;
    }
}