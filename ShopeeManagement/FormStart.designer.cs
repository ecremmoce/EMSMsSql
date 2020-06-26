namespace ShopeeManagement
{ 
    partial class FormStart
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStart));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdChn = new MetroFramework.Controls.MetroRadioButton();
            this.rdEng = new MetroFramework.Controls.MetroRadioButton();
            this.rdKor = new MetroFramework.Controls.MetroRadioButton();
            this.btn_login = new MetroFramework.Controls.MetroTile();
            this.BtnManual = new MetroFramework.Controls.MetroTile();
            this.BtnShippingStore = new MetroFramework.Controls.MetroTile();
            this.BtnCheckUpdate = new MetroFramework.Controls.MetroTile();
            this.Label_version = new MetroFramework.Controls.MetroLink();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdChn);
            this.groupBox2.Controls.Add(this.rdEng);
            this.groupBox2.Controls.Add(this.rdKor);
            this.groupBox2.Location = new System.Drawing.Point(78, 78);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(238, 48);
            this.groupBox2.TabIndex = 66;
            this.groupBox2.TabStop = false;
            // 
            // rdChn
            // 
            this.rdChn.AutoSize = true;
            this.rdChn.Location = new System.Drawing.Point(159, 20);
            this.rdChn.Name = "rdChn";
            this.rdChn.Size = new System.Drawing.Size(65, 15);
            this.rdChn.TabIndex = 2;
            this.rdChn.Text = "Chinese";
            this.rdChn.UseSelectable = true;
            // 
            // rdEng
            // 
            this.rdEng.AutoSize = true;
            this.rdEng.Location = new System.Drawing.Point(83, 20);
            this.rdEng.Name = "rdEng";
            this.rdEng.Size = new System.Drawing.Size(61, 15);
            this.rdEng.TabIndex = 1;
            this.rdEng.Text = "English";
            this.rdEng.UseSelectable = true;
            // 
            // rdKor
            // 
            this.rdKor.AutoSize = true;
            this.rdKor.Checked = true;
            this.rdKor.Location = new System.Drawing.Point(9, 20);
            this.rdKor.Name = "rdKor";
            this.rdKor.Size = new System.Drawing.Size(59, 15);
            this.rdKor.TabIndex = 0;
            this.rdKor.TabStop = true;
            this.rdKor.Text = "한국어";
            this.rdKor.UseSelectable = true;
            // 
            // btn_login
            // 
            this.btn_login.ActiveControl = null;
            this.btn_login.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(109)))), ((int)(((byte)(0)))));
            this.btn_login.Location = new System.Drawing.Point(199, 139);
            this.btn_login.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(116, 74);
            this.btn_login.TabIndex = 65;
            this.btn_login.Text = "LOGIN";
            this.btn_login.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_login.UseCustomBackColor = true;
            this.btn_login.UseSelectable = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // BtnManual
            // 
            this.BtnManual.ActiveControl = null;
            this.BtnManual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(112)))), ((int)(((byte)(56)))));
            this.BtnManual.Location = new System.Drawing.Point(199, 219);
            this.BtnManual.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnManual.Name = "BtnManual";
            this.BtnManual.Size = new System.Drawing.Size(116, 74);
            this.BtnManual.TabIndex = 64;
            this.BtnManual.Text = "MANUAL";
            this.BtnManual.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnManual.UseCustomBackColor = true;
            this.BtnManual.UseSelectable = true;
            this.BtnManual.Click += new System.EventHandler(this.BtnManual_Click);
            // 
            // BtnShippingStore
            // 
            this.BtnShippingStore.ActiveControl = null;
            this.BtnShippingStore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(115)))), ((int)(((byte)(196)))));
            this.BtnShippingStore.Location = new System.Drawing.Point(75, 219);
            this.BtnShippingStore.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnShippingStore.Name = "BtnShippingStore";
            this.BtnShippingStore.Size = new System.Drawing.Size(116, 74);
            this.BtnShippingStore.TabIndex = 63;
            this.BtnShippingStore.Text = "STORE";
            this.BtnShippingStore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnShippingStore.UseCustomBackColor = true;
            this.BtnShippingStore.UseSelectable = true;
            this.BtnShippingStore.Click += new System.EventHandler(this.BtnShippingStore_Click);
            // 
            // BtnCheckUpdate
            // 
            this.BtnCheckUpdate.ActiveControl = null;
            this.BtnCheckUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(136)))));
            this.BtnCheckUpdate.Location = new System.Drawing.Point(75, 139);
            this.BtnCheckUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnCheckUpdate.Name = "BtnCheckUpdate";
            this.BtnCheckUpdate.Size = new System.Drawing.Size(116, 74);
            this.BtnCheckUpdate.TabIndex = 62;
            this.BtnCheckUpdate.Text = "UPDATE";
            this.BtnCheckUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnCheckUpdate.UseCustomBackColor = true;
            this.BtnCheckUpdate.UseSelectable = true;
            this.BtnCheckUpdate.Click += new System.EventHandler(this.BtnCheckUpdate_Click);
            // 
            // Label_version
            // 
            this.Label_version.FontWeight = MetroFramework.MetroLinkWeight.Light;
            this.Label_version.Location = new System.Drawing.Point(142, 319);
            this.Label_version.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Label_version.Name = "Label_version";
            this.Label_version.Size = new System.Drawing.Size(111, 21);
            this.Label_version.TabIndex = 68;
            this.Label_version.Text = "Normal Link";
            this.Label_version.UseSelectable = true;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel1.Location = new System.Drawing.Point(53, 45);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(288, 25);
            this.metroLabel1.TabIndex = 67;
            this.metroLabel1.Text = "ECREMMOCE Management Solution";
            // 
            // metroStyleManager
            // 
            this.metroStyleManager.Owner = this;
            // 
            // FormStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(395, 368);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.BtnManual);
            this.Controls.Add(this.BtnShippingStore);
            this.Controls.Add(this.BtnCheckUpdate);
            this.Controls.Add(this.Label_version);
            this.Controls.Add(this.metroLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormStart";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.TransparencyKey = System.Drawing.Color.Empty;
            this.Activated += new System.EventHandler(this.FormStart_Activated);
            this.Load += new System.EventHandler(this.FormStart_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private MetroFramework.Controls.MetroRadioButton rdChn;
        private MetroFramework.Controls.MetroRadioButton rdEng;
        private MetroFramework.Controls.MetroRadioButton rdKor;
        private MetroFramework.Controls.MetroTile btn_login;
        private MetroFramework.Controls.MetroTile BtnManual;
        private MetroFramework.Controls.MetroTile BtnShippingStore;
        private MetroFramework.Controls.MetroTile BtnCheckUpdate;
        private MetroFramework.Controls.MetroLink Label_version;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
    }
}

