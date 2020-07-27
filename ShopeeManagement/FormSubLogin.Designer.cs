namespace ShopeeManagement
{
    partial class FormSubLogin
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
            this.LblLoginID = new System.Windows.Forms.Label();
            this.LblLoginPW = new System.Windows.Forms.Label();
            this.TxtLoginID = new System.Windows.Forms.TextBox();
            this.TxtLoginPW = new System.Windows.Forms.TextBox();
            this.BtnLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LblLoginID
            // 
            this.LblLoginID.AutoSize = true;
            this.LblLoginID.Font = new System.Drawing.Font("굴림", 12F);
            this.LblLoginID.Location = new System.Drawing.Point(12, 22);
            this.LblLoginID.Name = "LblLoginID";
            this.LblLoginID.Size = new System.Drawing.Size(26, 16);
            this.LblLoginID.TabIndex = 0;
            this.LblLoginID.Text = "ID:";
            // 
            // LblLoginPW
            // 
            this.LblLoginPW.AutoSize = true;
            this.LblLoginPW.Font = new System.Drawing.Font("굴림", 12F);
            this.LblLoginPW.Location = new System.Drawing.Point(12, 57);
            this.LblLoginPW.Name = "LblLoginPW";
            this.LblLoginPW.Size = new System.Drawing.Size(37, 16);
            this.LblLoginPW.TabIndex = 1;
            this.LblLoginPW.Text = "PW:";
            // 
            // TxtLoginID
            // 
            this.TxtLoginID.Font = new System.Drawing.Font("굴림", 12F);
            this.TxtLoginID.Location = new System.Drawing.Point(80, 19);
            this.TxtLoginID.Name = "TxtLoginID";
            this.TxtLoginID.Size = new System.Drawing.Size(255, 26);
            this.TxtLoginID.TabIndex = 2;
            // 
            // TxtLoginPW
            // 
            this.TxtLoginPW.Font = new System.Drawing.Font("굴림", 12F);
            this.TxtLoginPW.Location = new System.Drawing.Point(80, 54);
            this.TxtLoginPW.Name = "TxtLoginPW";
            this.TxtLoginPW.PasswordChar = '*';
            this.TxtLoginPW.Size = new System.Drawing.Size(255, 26);
            this.TxtLoginPW.TabIndex = 3;
            this.TxtLoginPW.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtLoginPW_KeyDown);
            // 
            // BtnLogin
            // 
            this.BtnLogin.Font = new System.Drawing.Font("굴림", 12F);
            this.BtnLogin.Location = new System.Drawing.Point(249, 86);
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.Size = new System.Drawing.Size(86, 34);
            this.BtnLogin.TabIndex = 4;
            this.BtnLogin.Text = "로그인";
            this.BtnLogin.UseVisualStyleBackColor = true;
            this.BtnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // FormSubLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 132);
            this.Controls.Add(this.BtnLogin);
            this.Controls.Add(this.TxtLoginPW);
            this.Controls.Add(this.TxtLoginID);
            this.Controls.Add(this.LblLoginPW);
            this.Controls.Add(this.LblLoginID);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSubLogin";
            this.Text = "로그인";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblLoginID;
        private System.Windows.Forms.Label LblLoginPW;
        private System.Windows.Forms.TextBox TxtLoginID;
        private System.Windows.Forms.TextBox TxtLoginPW;
        private System.Windows.Forms.Button BtnLogin;
    }
}