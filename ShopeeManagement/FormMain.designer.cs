namespace ShopeeManagement
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.Tile_New_Product = new MetroFramework.Controls.MetroTile();
            this.Tile_current_display = new MetroFramework.Controls.MetroTile();
            this.Tile_Uploader = new MetroFramework.Controls.MetroTile();
            this.Tile_Manage_Product = new MetroFramework.Controls.MetroTile();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.metroLink1 = new MetroFramework.Controls.MetroLink();
            this.txt_loginId = new MetroFramework.Controls.MetroLink();
            this.Label_version = new MetroFramework.Controls.MetroLink();
            this.Tile_exit = new MetroFramework.Controls.MetroTile();
            this.Tile_Style = new MetroFramework.Controls.MetroTile();
            this.Tile_Manual = new MetroFramework.Controls.MetroTile();
            this.Tile_Shipping_Store = new MetroFramework.Controls.MetroTile();
            this.Tile_Config = new MetroFramework.Controls.MetroTile();
            this.Tile_Update = new MetroFramework.Controls.MetroTile();
            this.Tile_mapping_category = new MetroFramework.Controls.MetroTile();
            this.Tile_Product_Upload = new MetroFramework.Controls.MetroTile();
            this.Tile_Set_Shipping_Fee = new MetroFramework.Controls.MetroTile();
            this.expire_timer = new System.Windows.Forms.Timer(this.components);
            this.update_timer = new System.Windows.Forms.Timer(this.components);
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroPanel1.SuspendLayout();
            this.metroPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            // 
            // metroPanel1
            // 
            this.metroPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.metroPanel1.Controls.Add(this.Tile_New_Product);
            this.metroPanel1.Controls.Add(this.Tile_current_display);
            this.metroPanel1.Controls.Add(this.Tile_Uploader);
            this.metroPanel1.Controls.Add(this.Tile_Manage_Product);
            this.metroPanel1.Controls.Add(this.metroPanel2);
            this.metroPanel1.Controls.Add(this.Tile_exit);
            this.metroPanel1.Controls.Add(this.Tile_Style);
            this.metroPanel1.Controls.Add(this.Tile_Manual);
            this.metroPanel1.Controls.Add(this.Tile_Shipping_Store);
            this.metroPanel1.Controls.Add(this.Tile_Config);
            this.metroPanel1.Controls.Add(this.Tile_Update);
            this.metroPanel1.Controls.Add(this.Tile_mapping_category);
            this.metroPanel1.Controls.Add(this.Tile_Product_Upload);
            this.metroPanel1.Controls.Add(this.Tile_Set_Shipping_Fee);
            this.metroPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(0, 30);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(1925, 78);
            this.metroPanel1.TabIndex = 13;
            this.metroPanel1.UseCustomBackColor = true;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // Tile_New_Product
            // 
            this.Tile_New_Product.ActiveControl = null;
            this.Tile_New_Product.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(115)))), ((int)(((byte)(196)))));
            this.Tile_New_Product.Location = new System.Drawing.Point(387, 10);
            this.Tile_New_Product.Name = "Tile_New_Product";
            this.Tile_New_Product.Size = new System.Drawing.Size(119, 60);
            this.Tile_New_Product.TabIndex = 68;
            this.Tile_New_Product.Text = "신상품 등록";
            this.Tile_New_Product.TileImage = global::ShopeeManagement.Properties.Resources.logout_24;
            this.Tile_New_Product.UseCustomBackColor = true;
            this.Tile_New_Product.UseSelectable = true;
            this.Tile_New_Product.UseTileImage = true;
            this.Tile_New_Product.Click += new System.EventHandler(this.Tile_New_Product_Click);
            // 
            // Tile_current_display
            // 
            this.Tile_current_display.ActiveControl = null;
            this.Tile_current_display.Location = new System.Drawing.Point(1535, 10);
            this.Tile_current_display.Name = "Tile_current_display";
            this.Tile_current_display.Size = new System.Drawing.Size(163, 60);
            this.Tile_current_display.TabIndex = 67;
            this.Tile_current_display.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Tile_current_display.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.Tile_current_display.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.Tile_current_display.UseSelectable = true;
            // 
            // Tile_Uploader
            // 
            this.Tile_Uploader.ActiveControl = null;
            this.Tile_Uploader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(109)))), ((int)(((byte)(0)))));
            this.Tile_Uploader.Location = new System.Drawing.Point(261, 10);
            this.Tile_Uploader.Name = "Tile_Uploader";
            this.Tile_Uploader.Size = new System.Drawing.Size(119, 60);
            this.Tile_Uploader.TabIndex = 66;
            this.Tile_Uploader.Text = "복사 등록기";
            this.Tile_Uploader.TileImage = global::ShopeeManagement.Properties.Resources.logout_24;
            this.Tile_Uploader.UseCustomBackColor = true;
            this.Tile_Uploader.UseSelectable = true;
            this.Tile_Uploader.UseTileImage = true;
            this.Tile_Uploader.Click += new System.EventHandler(this.Tile_Uploader_Click);
            // 
            // Tile_Manage_Product
            // 
            this.Tile_Manage_Product.ActiveControl = null;
            this.Tile_Manage_Product.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(75)))), ((int)(((byte)(120)))));
            this.Tile_Manage_Product.Location = new System.Drawing.Point(9, 10);
            this.Tile_Manage_Product.Name = "Tile_Manage_Product";
            this.Tile_Manage_Product.Size = new System.Drawing.Size(119, 60);
            this.Tile_Manage_Product.TabIndex = 65;
            this.Tile_Manage_Product.Text = "상품 정보 관리";
            this.Tile_Manage_Product.TileImage = global::ShopeeManagement.Properties.Resources.logout_24;
            this.Tile_Manage_Product.UseCustomBackColor = true;
            this.Tile_Manage_Product.UseSelectable = true;
            this.Tile_Manage_Product.UseTileImage = true;
            this.Tile_Manage_Product.Click += new System.EventHandler(this.Tile_Manage_Product_Click_1);
            // 
            // metroPanel2
            // 
            this.metroPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.metroPanel2.Controls.Add(this.pictureBox1);
            this.metroPanel2.Controls.Add(this.metroLink1);
            this.metroPanel2.Controls.Add(this.txt_loginId);
            this.metroPanel2.Controls.Add(this.Label_version);
            this.metroPanel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(1706, 0);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(219, 78);
            this.metroPanel2.TabIndex = 64;
            this.metroPanel2.UseCustomBackColor = true;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(15, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(47, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // metroLink1
            // 
            this.metroLink1.Location = new System.Drawing.Point(61, 32);
            this.metroLink1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.metroLink1.Name = "metroLink1";
            this.metroLink1.Size = new System.Drawing.Size(111, 21);
            this.metroLink1.TabIndex = 62;
            this.metroLink1.Text = "Normal Link";
            this.metroLink1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.metroLink1.UseCustomBackColor = true;
            this.metroLink1.UseSelectable = true;
            // 
            // txt_loginId
            // 
            this.txt_loginId.FontWeight = MetroFramework.MetroLinkWeight.Light;
            this.txt_loginId.Location = new System.Drawing.Point(61, 6);
            this.txt_loginId.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txt_loginId.Name = "txt_loginId";
            this.txt_loginId.Size = new System.Drawing.Size(111, 21);
            this.txt_loginId.TabIndex = 63;
            this.txt_loginId.Text = "Normal Link";
            this.txt_loginId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txt_loginId.UseCustomBackColor = true;
            this.txt_loginId.UseSelectable = true;
            // 
            // Label_version
            // 
            this.Label_version.FontSize = MetroFramework.MetroLinkSize.Medium;
            this.Label_version.FontWeight = MetroFramework.MetroLinkWeight.Light;
            this.Label_version.ForeColor = System.Drawing.Color.Tomato;
            this.Label_version.Location = new System.Drawing.Point(9, 44);
            this.Label_version.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Label_version.Name = "Label_version";
            this.Label_version.Size = new System.Drawing.Size(205, 33);
            this.Label_version.TabIndex = 13;
            this.Label_version.Text = "Shopee Management Solution";
            this.Label_version.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Label_version.UseCustomBackColor = true;
            this.Label_version.UseSelectable = true;
            // 
            // Tile_exit
            // 
            this.Tile_exit.ActiveControl = null;
            this.Tile_exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(75)))), ((int)(((byte)(120)))));
            this.Tile_exit.Location = new System.Drawing.Point(1395, 10);
            this.Tile_exit.Name = "Tile_exit";
            this.Tile_exit.Size = new System.Drawing.Size(119, 60);
            this.Tile_exit.TabIndex = 11;
            this.Tile_exit.Text = "로그아웃";
            this.Tile_exit.TileImage = global::ShopeeManagement.Properties.Resources.logout_24;
            this.Tile_exit.UseCustomBackColor = true;
            this.Tile_exit.UseSelectable = true;
            this.Tile_exit.UseTileImage = true;
            this.Tile_exit.Click += new System.EventHandler(this.Tile_exit_Click);
            // 
            // Tile_Style
            // 
            this.Tile_Style.ActiveControl = null;
            this.Tile_Style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(136)))));
            this.Tile_Style.Location = new System.Drawing.Point(1269, 10);
            this.Tile_Style.Name = "Tile_Style";
            this.Tile_Style.Size = new System.Drawing.Size(119, 60);
            this.Tile_Style.TabIndex = 10;
            this.Tile_Style.Text = "스타일 변경";
            this.Tile_Style.TileImage = global::ShopeeManagement.Properties.Resources.website_design_24;
            this.Tile_Style.UseCustomBackColor = true;
            this.Tile_Style.UseSelectable = true;
            this.Tile_Style.UseTileImage = true;
            this.Tile_Style.Click += new System.EventHandler(this.Tile_Style_Click);
            // 
            // Tile_Manual
            // 
            this.Tile_Manual.ActiveControl = null;
            this.Tile_Manual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            this.Tile_Manual.Location = new System.Drawing.Point(1143, 10);
            this.Tile_Manual.Name = "Tile_Manual";
            this.Tile_Manual.Size = new System.Drawing.Size(119, 60);
            this.Tile_Manual.TabIndex = 8;
            this.Tile_Manual.Text = "매뉴얼";
            this.Tile_Manual.TileImage = global::ShopeeManagement.Properties.Resources.book_24;
            this.Tile_Manual.UseCustomBackColor = true;
            this.Tile_Manual.UseSelectable = true;
            this.Tile_Manual.UseTileImage = true;
            this.Tile_Manual.Click += new System.EventHandler(this.Tile_Manual_Click);
            // 
            // Tile_Shipping_Store
            // 
            this.Tile_Shipping_Store.ActiveControl = null;
            this.Tile_Shipping_Store.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(112)))), ((int)(((byte)(56)))));
            this.Tile_Shipping_Store.Location = new System.Drawing.Point(1017, 10);
            this.Tile_Shipping_Store.Name = "Tile_Shipping_Store";
            this.Tile_Shipping_Store.Size = new System.Drawing.Size(119, 60);
            this.Tile_Shipping_Store.TabIndex = 7;
            this.Tile_Shipping_Store.Text = "About Us";
            this.Tile_Shipping_Store.TileImage = global::ShopeeManagement.Properties.Resources.shop_24;
            this.Tile_Shipping_Store.UseCustomBackColor = true;
            this.Tile_Shipping_Store.UseSelectable = true;
            this.Tile_Shipping_Store.UseTileImage = true;
            this.Tile_Shipping_Store.Click += new System.EventHandler(this.Tile_Shipping_Store_Click);
            // 
            // Tile_Config
            // 
            this.Tile_Config.ActiveControl = null;
            this.Tile_Config.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Tile_Config.Location = new System.Drawing.Point(513, 10);
            this.Tile_Config.Margin = new System.Windows.Forms.Padding(10);
            this.Tile_Config.Name = "Tile_Config";
            this.Tile_Config.PaintTileCount = false;
            this.Tile_Config.Size = new System.Drawing.Size(119, 60);
            this.Tile_Config.TabIndex = 0;
            this.Tile_Config.Text = "환경 설정";
            this.Tile_Config.TileImage = global::ShopeeManagement.Properties.Resources.data_configuration_24;
            this.Tile_Config.UseCustomBackColor = true;
            this.Tile_Config.UseSelectable = true;
            this.Tile_Config.UseTileImage = true;
            this.Tile_Config.Click += new System.EventHandler(this.Tile_Config_Click);
            // 
            // Tile_Update
            // 
            this.Tile_Update.ActiveControl = null;
            this.Tile_Update.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(109)))), ((int)(((byte)(0)))));
            this.Tile_Update.Location = new System.Drawing.Point(891, 10);
            this.Tile_Update.Name = "Tile_Update";
            this.Tile_Update.Size = new System.Drawing.Size(119, 60);
            this.Tile_Update.TabIndex = 4;
            this.Tile_Update.Text = "업데이트 확인";
            this.Tile_Update.TileImage = global::ShopeeManagement.Properties.Resources.available_updates_24;
            this.Tile_Update.UseCustomBackColor = true;
            this.Tile_Update.UseSelectable = true;
            this.Tile_Update.UseTileImage = true;
            this.Tile_Update.Click += new System.EventHandler(this.Tile_Update_Click);
            // 
            // Tile_mapping_category
            // 
            this.Tile_mapping_category.ActiveControl = null;
            this.Tile_mapping_category.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(115)))), ((int)(((byte)(196)))));
            this.Tile_mapping_category.Location = new System.Drawing.Point(639, 10);
            this.Tile_mapping_category.Name = "Tile_mapping_category";
            this.Tile_mapping_category.Size = new System.Drawing.Size(119, 60);
            this.Tile_mapping_category.TabIndex = 3;
            this.Tile_mapping_category.Text = "카테고리 연결";
            this.Tile_mapping_category.TileImage = global::ShopeeManagement.Properties.Resources.categorize_24;
            this.Tile_mapping_category.UseCustomBackColor = true;
            this.Tile_mapping_category.UseSelectable = true;
            this.Tile_mapping_category.UseTileImage = true;
            this.Tile_mapping_category.Click += new System.EventHandler(this.Tile_mapping_category_Click);
            // 
            // Tile_Product_Upload
            // 
            this.Tile_Product_Upload.ActiveControl = null;
            this.Tile_Product_Upload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(136)))));
            this.Tile_Product_Upload.ForeColor = System.Drawing.Color.White;
            this.Tile_Product_Upload.Location = new System.Drawing.Point(135, 10);
            this.Tile_Product_Upload.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.Tile_Product_Upload.Name = "Tile_Product_Upload";
            this.Tile_Product_Upload.Size = new System.Drawing.Size(119, 60);
            this.Tile_Product_Upload.TabIndex = 2;
            this.Tile_Product_Upload.Text = "상품 연결";
            this.Tile_Product_Upload.TileImage = global::ShopeeManagement.Properties.Resources.activity_feed_24;
            this.Tile_Product_Upload.UseCustomBackColor = true;
            this.Tile_Product_Upload.UseCustomForeColor = true;
            this.Tile_Product_Upload.UseSelectable = true;
            this.Tile_Product_Upload.UseStyleColors = true;
            this.Tile_Product_Upload.UseTileImage = true;
            this.Tile_Product_Upload.Click += new System.EventHandler(this.Tile_Manage_Product_Click);
            // 
            // Tile_Set_Shipping_Fee
            // 
            this.Tile_Set_Shipping_Fee.ActiveControl = null;
            this.Tile_Set_Shipping_Fee.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Tile_Set_Shipping_Fee.Location = new System.Drawing.Point(765, 10);
            this.Tile_Set_Shipping_Fee.Name = "Tile_Set_Shipping_Fee";
            this.Tile_Set_Shipping_Fee.Size = new System.Drawing.Size(119, 60);
            this.Tile_Set_Shipping_Fee.TabIndex = 6;
            this.Tile_Set_Shipping_Fee.Text = "배송비 설정";
            this.Tile_Set_Shipping_Fee.TileImage = global::ShopeeManagement.Properties.Resources.free_shipping_24;
            this.Tile_Set_Shipping_Fee.UseCustomBackColor = true;
            this.Tile_Set_Shipping_Fee.UseSelectable = true;
            this.Tile_Set_Shipping_Fee.UseTileImage = true;
            this.Tile_Set_Shipping_Fee.Click += new System.EventHandler(this.Tile_Set_Shipping_Fee_Click);
            // 
            // expire_timer
            // 
            this.expire_timer.Interval = 3600000;
            // 
            // update_timer
            // 
            this.update_timer.Interval = 3600000;
            // 
            // metroStyleManager
            // 
            this.metroStyleManager.Owner = this;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1925, 1041);
            this.Controls.Add(this.metroPanel1);
            this.DisplayHeader = false;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.IsMdiContainer = true;
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.Text = "Shopee Management Solution";
            this.TransparencyKey = System.Drawing.Color.Empty;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private MetroFramework.Controls.MetroLink metroLink1;
        public MetroFramework.Controls.MetroLink txt_loginId;
        private MetroFramework.Controls.MetroLink Label_version;
        private MetroFramework.Controls.MetroTile Tile_exit;
        private MetroFramework.Controls.MetroTile Tile_Style;
        private MetroFramework.Controls.MetroTile Tile_Manual;
        private MetroFramework.Controls.MetroTile Tile_Shipping_Store;
        private MetroFramework.Controls.MetroTile Tile_Set_Shipping_Fee;
        private MetroFramework.Controls.MetroTile Tile_Config;
        private MetroFramework.Controls.MetroTile Tile_Update;
        private MetroFramework.Controls.MetroTile Tile_mapping_category;
        private MetroFramework.Controls.MetroTile Tile_Product_Upload;
        private System.Windows.Forms.Timer expire_timer;
        private System.Windows.Forms.Timer update_timer;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        private MetroFramework.Controls.MetroTile Tile_Manage_Product;
        private MetroFramework.Controls.MetroTile Tile_Uploader;
        private MetroFramework.Controls.MetroTile Tile_current_display;
        private MetroFramework.Controls.MetroTile Tile_New_Product;
    }
}