namespace ShopeeManagement
{
    partial class FormDescription
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDescription));
            this.TxtProductDesc = new System.Windows.Forms.RichTextBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.BtnSetCategory = new MetroFramework.Controls.MetroButton();
            this.BtnClose = new MetroFramework.Controls.MetroButton();
            this.lblCurrentLen = new System.Windows.Forms.Label();
            this.lblMaxLen = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TxtProductDesc
            // 
            this.TxtProductDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtProductDesc.Location = new System.Drawing.Point(23, 50);
            this.TxtProductDesc.Name = "TxtProductDesc";
            this.TxtProductDesc.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.TxtProductDesc.Size = new System.Drawing.Size(1011, 916);
            this.TxtProductDesc.TabIndex = 246;
            this.TxtProductDesc.Text = "";
            this.TxtProductDesc.TextChanged += new System.EventHandler(this.TxtProductDesc_TextChanged);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(23, 28);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(65, 19);
            this.metroLabel2.TabIndex = 245;
            this.metroLabel2.Text = "상품설명";
            // 
            // BtnSetCategory
            // 
            this.BtnSetCategory.Location = new System.Drawing.Point(1067, 47);
            this.BtnSetCategory.Name = "BtnSetCategory";
            this.BtnSetCategory.Size = new System.Drawing.Size(117, 29);
            this.BtnSetCategory.TabIndex = 247;
            this.BtnSetCategory.Text = "저장";
            this.BtnSetCategory.UseCustomBackColor = true;
            this.BtnSetCategory.UseSelectable = true;
            this.BtnSetCategory.Click += new System.EventHandler(this.BtnSetCategory_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(1067, 95);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(117, 29);
            this.BtnClose.TabIndex = 248;
            this.BtnClose.Text = "닫기";
            this.BtnClose.UseCustomBackColor = true;
            this.BtnClose.UseSelectable = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // lblCurrentLen
            // 
            this.lblCurrentLen.Location = new System.Drawing.Point(145, 29);
            this.lblCurrentLen.Name = "lblCurrentLen";
            this.lblCurrentLen.Size = new System.Drawing.Size(100, 21);
            this.lblCurrentLen.TabIndex = 250;
            this.lblCurrentLen.Text = "label1";
            this.lblCurrentLen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCurrentLen.Click += new System.EventHandler(this.LblCurrentLen_Click);
            // 
            // lblMaxLen
            // 
            this.lblMaxLen.AutoSize = true;
            this.lblMaxLen.Location = new System.Drawing.Point(259, 33);
            this.lblMaxLen.Name = "lblMaxLen";
            this.lblMaxLen.Size = new System.Drawing.Size(38, 12);
            this.lblMaxLen.TabIndex = 251;
            this.lblMaxLen.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(246, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 252;
            this.label2.Text = "/";
            // 
            // FormDescription
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 989);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblMaxLen);
            this.Controls.Add(this.lblCurrentLen);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnSetCategory);
            this.Controls.Add(this.TxtProductDesc);
            this.Controls.Add(this.metroLabel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormDescription";
            this.Resizable = false;
            this.Load += new System.EventHandler(this.FormDescription_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox TxtProductDesc;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroButton BtnSetCategory;
        private MetroFramework.Controls.MetroButton BtnClose;
        private System.Windows.Forms.Label lblCurrentLen;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lblMaxLen;
    }
}