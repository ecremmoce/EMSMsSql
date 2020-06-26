namespace ShopeeManagement
{
    partial class FormDescriptionDraft
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDescriptionDraft));
            this.BtnClose = new MetroFramework.Controls.MetroButton();
            this.BtnSetCategory = new MetroFramework.Controls.MetroButton();
            this.TxtProductDesc = new System.Windows.Forms.RichTextBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(1067, 92);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(117, 29);
            this.BtnClose.TabIndex = 252;
            this.BtnClose.Text = "닫기";
            this.BtnClose.UseCustomBackColor = true;
            this.BtnClose.UseSelectable = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnSetCategory
            // 
            this.BtnSetCategory.Location = new System.Drawing.Point(1067, 44);
            this.BtnSetCategory.Name = "BtnSetCategory";
            this.BtnSetCategory.Size = new System.Drawing.Size(117, 29);
            this.BtnSetCategory.TabIndex = 251;
            this.BtnSetCategory.Text = "저장";
            this.BtnSetCategory.UseCustomBackColor = true;
            this.BtnSetCategory.UseSelectable = true;
            this.BtnSetCategory.Click += new System.EventHandler(this.BtnSetCategory_Click);
            // 
            // TxtProductDesc
            // 
            this.TxtProductDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtProductDesc.Location = new System.Drawing.Point(23, 47);
            this.TxtProductDesc.Name = "TxtProductDesc";
            this.TxtProductDesc.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.TxtProductDesc.Size = new System.Drawing.Size(1011, 916);
            this.TxtProductDesc.TabIndex = 250;
            this.TxtProductDesc.Text = "";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(23, 25);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(65, 19);
            this.metroLabel2.TabIndex = 249;
            this.metroLabel2.Text = "상품설명";
            // 
            // FormDescriptionDraft
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 989);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnSetCategory);
            this.Controls.Add(this.TxtProductDesc);
            this.Controls.Add(this.metroLabel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDescriptionDraft";
            this.Load += new System.EventHandler(this.FormDescriptionDraft_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroButton BtnClose;
        private MetroFramework.Controls.MetroButton BtnSetCategory;
        public System.Windows.Forms.RichTextBox TxtProductDesc;
        private MetroFramework.Controls.MetroLabel metroLabel2;
    }
}