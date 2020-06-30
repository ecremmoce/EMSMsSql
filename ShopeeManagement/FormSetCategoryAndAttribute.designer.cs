namespace ShopeeManagement
{
    partial class FormSetCategoryAndAttribute
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetCategoryAndAttribute));
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.txtSrcTitle = new MetroFramework.Controls.MetroTextBox();
            this.TxtSrcCategoryId = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.BtnSearch = new MetroFramework.Controls.MetroButton();
            this.TxtSearchText = new MetroFramework.Controls.MetroTextBox();
            this.BtnSetCategory = new MetroFramework.Controls.MetroButton();
            this.dgSrcItemList = new System.Windows.Forms.DataGridView();
            this.dgItemList_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgSrcItemList_Src_cat1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgSrcItemList_Src_cat2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgSrcItemList_Src_cat3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgSrcItemList_Src_cat3_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TxtSrcCountry = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.DgSrcAttribute = new System.Windows.Forms.DataGridView();
            this.TxtItemId = new MetroFramework.Controls.MetroTextBox();
            this.BtnSaveAttribute = new MetroFramework.Controls.MetroButton();
            this.DgTarAttribute = new System.Windows.Forms.DataGridView();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.BtnClose = new MetroFramework.Controls.MetroButton();
            this.Menu_Attr1 = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.Menu_Attr1_copy_cell = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdSrcLocal = new System.Windows.Forms.RadioButton();
            this.rdSrcKor = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdTarLocal = new System.Windows.Forms.RadioButton();
            this.rdTarKor = new System.Windows.Forms.RadioButton();
            this.toolTipCommon = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgSrcItemList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgSrcAttribute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgTarAttribute)).BeginInit();
            this.Menu_Attr1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(19, 21);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(83, 19);
            this.metroLabel4.TabIndex = 15;
            this.metroLabel4.Text = "원본 상품명";
            // 
            // txtSrcTitle
            // 
            // 
            // 
            // 
            this.txtSrcTitle.CustomButton.Image = null;
            this.txtSrcTitle.CustomButton.Location = new System.Drawing.Point(648, 1);
            this.txtSrcTitle.CustomButton.Name = "";
            this.txtSrcTitle.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.txtSrcTitle.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtSrcTitle.CustomButton.TabIndex = 1;
            this.txtSrcTitle.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtSrcTitle.CustomButton.UseSelectable = true;
            this.txtSrcTitle.CustomButton.Visible = false;
            this.txtSrcTitle.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtSrcTitle.Lines = new string[0];
            this.txtSrcTitle.Location = new System.Drawing.Point(103, 16);
            this.txtSrcTitle.MaxLength = 32767;
            this.txtSrcTitle.Name = "txtSrcTitle";
            this.txtSrcTitle.PasswordChar = '\0';
            this.txtSrcTitle.ReadOnly = true;
            this.txtSrcTitle.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSrcTitle.SelectedText = "";
            this.txtSrcTitle.SelectionLength = 0;
            this.txtSrcTitle.SelectionStart = 0;
            this.txtSrcTitle.ShortcutsEnabled = true;
            this.txtSrcTitle.Size = new System.Drawing.Size(676, 29);
            this.txtSrcTitle.TabIndex = 14;
            this.toolTipCommon.SetToolTip(this.txtSrcTitle, "상품명입니다.");
            this.txtSrcTitle.UseSelectable = true;
            this.txtSrcTitle.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtSrcTitle.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // TxtSrcCategoryId
            // 
            // 
            // 
            // 
            this.TxtSrcCategoryId.CustomButton.Image = null;
            this.TxtSrcCategoryId.CustomButton.Location = new System.Drawing.Point(72, 1);
            this.TxtSrcCategoryId.CustomButton.Name = "";
            this.TxtSrcCategoryId.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.TxtSrcCategoryId.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.TxtSrcCategoryId.CustomButton.TabIndex = 1;
            this.TxtSrcCategoryId.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.TxtSrcCategoryId.CustomButton.UseSelectable = true;
            this.TxtSrcCategoryId.CustomButton.Visible = false;
            this.TxtSrcCategoryId.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.TxtSrcCategoryId.Lines = new string[0];
            this.TxtSrcCategoryId.Location = new System.Drawing.Point(290, 56);
            this.TxtSrcCategoryId.MaxLength = 32767;
            this.TxtSrcCategoryId.Name = "TxtSrcCategoryId";
            this.TxtSrcCategoryId.PasswordChar = '\0';
            this.TxtSrcCategoryId.ReadOnly = true;
            this.TxtSrcCategoryId.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TxtSrcCategoryId.SelectedText = "";
            this.TxtSrcCategoryId.SelectionLength = 0;
            this.TxtSrcCategoryId.SelectionStart = 0;
            this.TxtSrcCategoryId.ShortcutsEnabled = true;
            this.TxtSrcCategoryId.Size = new System.Drawing.Size(100, 29);
            this.TxtSrcCategoryId.TabIndex = 16;
            this.TxtSrcCategoryId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTipCommon.SetToolTip(this.TxtSrcCategoryId, "기존 설정된 카테고리 번호입니다.");
            this.TxtSrcCategoryId.UseSelectable = true;
            this.TxtSrcCategoryId.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.TxtSrcCategoryId.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(187, 60);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(97, 19);
            this.metroLabel3.TabIndex = 17;
            this.metroLabel3.Text = "카테고리 번호";
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(725, 56);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(54, 29);
            this.BtnSearch.TabIndex = 21;
            this.BtnSearch.Text = "검색";
            this.toolTipCommon.SetToolTip(this.BtnSearch, "카테고리명, 카테고리번호를 검색합니다.");
            this.BtnSearch.UseCustomBackColor = true;
            this.BtnSearch.UseSelectable = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // TxtSearchText
            // 
            // 
            // 
            // 
            this.TxtSearchText.CustomButton.Image = null;
            this.TxtSearchText.CustomButton.Location = new System.Drawing.Point(152, 1);
            this.TxtSearchText.CustomButton.Name = "";
            this.TxtSearchText.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.TxtSearchText.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.TxtSearchText.CustomButton.TabIndex = 1;
            this.TxtSearchText.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.TxtSearchText.CustomButton.UseSelectable = true;
            this.TxtSearchText.CustomButton.Visible = false;
            this.TxtSearchText.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.TxtSearchText.Lines = new string[0];
            this.TxtSearchText.Location = new System.Drawing.Point(539, 56);
            this.TxtSearchText.MaxLength = 32767;
            this.TxtSearchText.Name = "TxtSearchText";
            this.TxtSearchText.PasswordChar = '\0';
            this.TxtSearchText.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TxtSearchText.SelectedText = "";
            this.TxtSearchText.SelectionLength = 0;
            this.TxtSearchText.SelectionStart = 0;
            this.TxtSearchText.ShortcutsEnabled = true;
            this.TxtSearchText.Size = new System.Drawing.Size(180, 29);
            this.TxtSearchText.TabIndex = 20;
            this.TxtSearchText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtSearchText.UseSelectable = true;
            this.TxtSearchText.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.TxtSearchText.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.TxtSearchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtSearchText_KeyDown);
            // 
            // BtnSetCategory
            // 
            this.BtnSetCategory.Location = new System.Drawing.Point(807, 56);
            this.BtnSetCategory.Name = "BtnSetCategory";
            this.BtnSetCategory.Size = new System.Drawing.Size(117, 29);
            this.BtnSetCategory.TabIndex = 18;
            this.BtnSetCategory.Text = "카테고리 저장";
            this.toolTipCommon.SetToolTip(this.BtnSetCategory, "아래에 선택된 카테고리를 상품의 카테고리로 저장합니다.");
            this.BtnSetCategory.UseCustomBackColor = true;
            this.BtnSetCategory.UseSelectable = true;
            this.BtnSetCategory.Click += new System.EventHandler(this.BtnSetCategory_Click);
            // 
            // dgSrcItemList
            // 
            this.dgSrcItemList.AllowUserToAddRows = false;
            this.dgSrcItemList.AllowUserToDeleteRows = false;
            this.dgSrcItemList.AllowUserToResizeColumns = false;
            this.dgSrcItemList.AllowUserToResizeRows = false;
            this.dgSrcItemList.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgSrcItemList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgSrcItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgSrcItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgItemList_No,
            this.dgSrcItemList_Src_cat1,
            this.dgSrcItemList_Src_cat2,
            this.dgSrcItemList_Src_cat3,
            this.dgSrcItemList_Src_cat3_id});
            this.dgSrcItemList.Location = new System.Drawing.Point(20, 109);
            this.dgSrcItemList.MultiSelect = false;
            this.dgSrcItemList.Name = "dgSrcItemList";
            this.dgSrcItemList.ReadOnly = true;
            this.dgSrcItemList.RowHeadersVisible = false;
            this.dgSrcItemList.RowTemplate.Height = 25;
            this.dgSrcItemList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgSrcItemList.Size = new System.Drawing.Size(904, 675);
            this.dgSrcItemList.TabIndex = 22;
            this.dgSrcItemList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgSrcItemList_CellClick);
            // 
            // dgItemList_No
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgItemList_No.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgItemList_No.HeaderText = "No";
            this.dgItemList_No.Name = "dgItemList_No";
            this.dgItemList_No.ReadOnly = true;
            this.dgItemList_No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgItemList_No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgItemList_No.Width = 40;
            // 
            // dgSrcItemList_Src_cat1
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgSrcItemList_Src_cat1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgSrcItemList_Src_cat1.HeaderText = "카테고리1";
            this.dgSrcItemList_Src_cat1.Name = "dgSrcItemList_Src_cat1";
            this.dgSrcItemList_Src_cat1.ReadOnly = true;
            this.dgSrcItemList_Src_cat1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgSrcItemList_Src_cat1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgSrcItemList_Src_cat1.Width = 180;
            // 
            // dgSrcItemList_Src_cat2
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgSrcItemList_Src_cat2.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgSrcItemList_Src_cat2.HeaderText = "카테고리2";
            this.dgSrcItemList_Src_cat2.Name = "dgSrcItemList_Src_cat2";
            this.dgSrcItemList_Src_cat2.ReadOnly = true;
            this.dgSrcItemList_Src_cat2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgSrcItemList_Src_cat2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgSrcItemList_Src_cat2.Width = 280;
            // 
            // dgSrcItemList_Src_cat3
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgSrcItemList_Src_cat3.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgSrcItemList_Src_cat3.HeaderText = "카테고리3";
            this.dgSrcItemList_Src_cat3.Name = "dgSrcItemList_Src_cat3";
            this.dgSrcItemList_Src_cat3.ReadOnly = true;
            this.dgSrcItemList_Src_cat3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgSrcItemList_Src_cat3.Width = 280;
            // 
            // dgSrcItemList_Src_cat3_id
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgSrcItemList_Src_cat3_id.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgSrcItemList_Src_cat3_id.HeaderText = "카테고리3 번호";
            this.dgSrcItemList_Src_cat3_id.Name = "dgSrcItemList_Src_cat3_id";
            this.dgSrcItemList_Src_cat3_id.ReadOnly = true;
            this.dgSrcItemList_Src_cat3_id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TxtSrcCountry
            // 
            // 
            // 
            // 
            this.TxtSrcCountry.CustomButton.Image = null;
            this.TxtSrcCountry.CustomButton.Location = new System.Drawing.Point(47, 1);
            this.TxtSrcCountry.CustomButton.Name = "";
            this.TxtSrcCountry.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.TxtSrcCountry.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.TxtSrcCountry.CustomButton.TabIndex = 1;
            this.TxtSrcCountry.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.TxtSrcCountry.CustomButton.UseSelectable = true;
            this.TxtSrcCountry.CustomButton.Visible = false;
            this.TxtSrcCountry.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.TxtSrcCountry.Lines = new string[0];
            this.TxtSrcCountry.Location = new System.Drawing.Point(103, 56);
            this.TxtSrcCountry.MaxLength = 32767;
            this.TxtSrcCountry.Name = "TxtSrcCountry";
            this.TxtSrcCountry.PasswordChar = '\0';
            this.TxtSrcCountry.ReadOnly = true;
            this.TxtSrcCountry.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TxtSrcCountry.SelectedText = "";
            this.TxtSrcCountry.SelectionLength = 0;
            this.TxtSrcCountry.SelectionStart = 0;
            this.TxtSrcCountry.ShortcutsEnabled = true;
            this.TxtSrcCountry.Size = new System.Drawing.Size(75, 29);
            this.TxtSrcCountry.TabIndex = 24;
            this.TxtSrcCountry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTipCommon.SetToolTip(this.TxtSrcCountry, "상품이 등록된 국가입니다.");
            this.TxtSrcCountry.UseSelectable = true;
            this.TxtSrcCountry.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.TxtSrcCountry.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(65, 60);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(37, 19);
            this.metroLabel1.TabIndex = 23;
            this.metroLabel1.Text = "국가";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(936, 87);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(115, 19);
            this.metroLabel2.TabIndex = 273;
            this.metroLabel2.Text = "저장된 속성 목록";
            // 
            // DgSrcAttribute
            // 
            this.DgSrcAttribute.AllowUserToAddRows = false;
            this.DgSrcAttribute.AllowUserToDeleteRows = false;
            this.DgSrcAttribute.AllowUserToResizeColumns = false;
            this.DgSrcAttribute.AllowUserToResizeRows = false;
            this.DgSrcAttribute.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgSrcAttribute.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.DgSrcAttribute.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgSrcAttribute.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgSrcAttribute.Location = new System.Drawing.Point(936, 109);
            this.DgSrcAttribute.MultiSelect = false;
            this.DgSrcAttribute.Name = "DgSrcAttribute";
            this.DgSrcAttribute.RowHeadersVisible = false;
            this.DgSrcAttribute.RowTemplate.Height = 30;
            this.DgSrcAttribute.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgSrcAttribute.Size = new System.Drawing.Size(905, 240);
            this.DgSrcAttribute.TabIndex = 272;
            this.DgSrcAttribute.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgSrcAttribute_CellClick);
            this.DgSrcAttribute.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DgSrcAttribute_MouseUp);
            // 
            // TxtItemId
            // 
            // 
            // 
            // 
            this.TxtItemId.CustomButton.Image = null;
            this.TxtItemId.CustomButton.Location = new System.Drawing.Point(72, 1);
            this.TxtItemId.CustomButton.Name = "";
            this.TxtItemId.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.TxtItemId.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.TxtItemId.CustomButton.TabIndex = 1;
            this.TxtItemId.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.TxtItemId.CustomButton.UseSelectable = true;
            this.TxtItemId.CustomButton.Visible = false;
            this.TxtItemId.Lines = new string[0];
            this.TxtItemId.Location = new System.Drawing.Point(807, 16);
            this.TxtItemId.MaxLength = 32767;
            this.TxtItemId.Name = "TxtItemId";
            this.TxtItemId.PasswordChar = '\0';
            this.TxtItemId.ReadOnly = true;
            this.TxtItemId.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TxtItemId.SelectedText = "";
            this.TxtItemId.SelectionLength = 0;
            this.TxtItemId.SelectionStart = 0;
            this.TxtItemId.ShortcutsEnabled = true;
            this.TxtItemId.Size = new System.Drawing.Size(100, 29);
            this.TxtItemId.TabIndex = 274;
            this.TxtItemId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtItemId.UseSelectable = true;
            this.TxtItemId.Visible = false;
            this.TxtItemId.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.TxtItemId.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // BtnSaveAttribute
            // 
            this.BtnSaveAttribute.Location = new System.Drawing.Point(1724, 423);
            this.BtnSaveAttribute.Name = "BtnSaveAttribute";
            this.BtnSaveAttribute.Size = new System.Drawing.Size(117, 25);
            this.BtnSaveAttribute.TabIndex = 275;
            this.BtnSaveAttribute.Text = "속성값 신규 저장";
            this.BtnSaveAttribute.UseCustomBackColor = true;
            this.BtnSaveAttribute.UseSelectable = true;
            this.BtnSaveAttribute.Click += new System.EventHandler(this.BtnSaveAttribute_Click);
            // 
            // DgTarAttribute
            // 
            this.DgTarAttribute.AllowUserToAddRows = false;
            this.DgTarAttribute.AllowUserToDeleteRows = false;
            this.DgTarAttribute.AllowUserToResizeColumns = false;
            this.DgTarAttribute.AllowUserToResizeRows = false;
            this.DgTarAttribute.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgTarAttribute.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.DgTarAttribute.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgTarAttribute.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgTarAttribute.Location = new System.Drawing.Point(936, 463);
            this.DgTarAttribute.MultiSelect = false;
            this.DgTarAttribute.Name = "DgTarAttribute";
            this.DgTarAttribute.RowHeadersVisible = false;
            this.DgTarAttribute.RowTemplate.Height = 30;
            this.DgTarAttribute.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgTarAttribute.Size = new System.Drawing.Size(905, 240);
            this.DgTarAttribute.TabIndex = 276;
            this.DgTarAttribute.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.DgTarAttribute_EditingControlShowing);
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(936, 441);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(189, 19);
            this.metroLabel5.TabIndex = 277;
            this.metroLabel5.Text = "선택된 카테고리의 속성 목록";
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(436, 60);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(97, 19);
            this.metroLabel6.TabIndex = 280;
            this.metroLabel6.Text = "카테고리 검색";
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(1724, 70);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(117, 25);
            this.metroButton1.TabIndex = 281;
            this.metroButton1.Text = "속성값 저장";
            this.metroButton1.UseCustomBackColor = true;
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(1597, 11);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(99, 25);
            this.BtnClose.TabIndex = 282;
            this.BtnClose.Text = "닫기";
            this.BtnClose.UseCustomBackColor = true;
            this.BtnClose.UseSelectable = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // Menu_Attr1
            // 
            this.Menu_Attr1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Attr1_copy_cell});
            this.Menu_Attr1.Name = "Menu_Attr1";
            this.Menu_Attr1.Size = new System.Drawing.Size(191, 26);
            // 
            // Menu_Attr1_copy_cell
            // 
            this.Menu_Attr1_copy_cell.Image = global::ShopeeManagement.Properties.Resources.copy_16;
            this.Menu_Attr1_copy_cell.Name = "Menu_Attr1_copy_cell";
            this.Menu_Attr1_copy_cell.Size = new System.Drawing.Size(190, 22);
            this.Menu_Attr1_copy_cell.Text = "셀값 클립보드로 복사";
            this.Menu_Attr1_copy_cell.Click += new System.EventHandler(this.Menu_Attr1_copy_cell_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdSrcLocal);
            this.groupBox1.Controls.Add(this.rdSrcKor);
            this.groupBox1.Location = new System.Drawing.Point(1495, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(204, 49);
            this.groupBox1.TabIndex = 285;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "표시 언어";
            // 
            // rdSrcLocal
            // 
            this.rdSrcLocal.AutoSize = true;
            this.rdSrcLocal.Location = new System.Drawing.Point(121, 22);
            this.rdSrcLocal.Name = "rdSrcLocal";
            this.rdSrcLocal.Size = new System.Drawing.Size(59, 16);
            this.rdSrcLocal.TabIndex = 1;
            this.rdSrcLocal.Text = "현지어";
            this.rdSrcLocal.UseVisualStyleBackColor = true;
            this.rdSrcLocal.Click += new System.EventHandler(this.RdSrcLocal_Click);
            // 
            // rdSrcKor
            // 
            this.rdSrcKor.AutoSize = true;
            this.rdSrcKor.Checked = true;
            this.rdSrcKor.Location = new System.Drawing.Point(18, 22);
            this.rdSrcKor.Name = "rdSrcKor";
            this.rdSrcKor.Size = new System.Drawing.Size(59, 16);
            this.rdSrcKor.TabIndex = 0;
            this.rdSrcKor.TabStop = true;
            this.rdSrcKor.Text = "한국어";
            this.rdSrcKor.UseVisualStyleBackColor = true;
            this.rdSrcKor.Click += new System.EventHandler(this.RdSrcKor_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdTarLocal);
            this.groupBox2.Controls.Add(this.rdTarKor);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(1495, 408);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(204, 49);
            this.groupBox2.TabIndex = 286;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "표시언어";
            // 
            // rdTarLocal
            // 
            this.rdTarLocal.AutoSize = true;
            this.rdTarLocal.Checked = true;
            this.rdTarLocal.Location = new System.Drawing.Point(121, 22);
            this.rdTarLocal.Name = "rdTarLocal";
            this.rdTarLocal.Size = new System.Drawing.Size(59, 16);
            this.rdTarLocal.TabIndex = 1;
            this.rdTarLocal.TabStop = true;
            this.rdTarLocal.Text = "현지어";
            this.rdTarLocal.UseVisualStyleBackColor = true;
            this.rdTarLocal.Click += new System.EventHandler(this.RdTarLocal_Click);
            // 
            // rdTarKor
            // 
            this.rdTarKor.AutoSize = true;
            this.rdTarKor.Location = new System.Drawing.Point(18, 22);
            this.rdTarKor.Name = "rdTarKor";
            this.rdTarKor.Size = new System.Drawing.Size(59, 16);
            this.rdTarKor.TabIndex = 0;
            this.rdTarKor.Text = "한국어";
            this.rdTarKor.UseVisualStyleBackColor = true;
            this.rdTarKor.Click += new System.EventHandler(this.RdTarKor_Click);
            // 
            // toolTipCommon
            // 
            this.toolTipCommon.AutomaticDelay = 25000;
            this.toolTipCommon.AutoPopDelay = 25000;
            this.toolTipCommon.InitialDelay = 10;
            this.toolTipCommon.IsBalloon = true;
            this.toolTipCommon.ReshowDelay = 10;
            this.toolTipCommon.ShowAlways = true;
            this.toolTipCommon.UseAnimation = false;
            this.toolTipCommon.UseFading = false;
            // 
            // FormSetCategoryAndAttribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2065, 1080);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.metroLabel6);
            this.Controls.Add(this.metroLabel5);
            this.Controls.Add(this.DgTarAttribute);
            this.Controls.Add(this.BtnSaveAttribute);
            this.Controls.Add(this.TxtItemId);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.DgSrcAttribute);
            this.Controls.Add(this.TxtSrcCountry);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.dgSrcItemList);
            this.Controls.Add(this.BtnSearch);
            this.Controls.Add(this.TxtSearchText);
            this.Controls.Add(this.BtnSetCategory);
            this.Controls.Add(this.TxtSrcCategoryId);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.txtSrcTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormSetCategoryAndAttribute";
            this.Resizable = false;
            this.Activated += new System.EventHandler(this.FormSetCategoryAndAttribute_Activated);
            this.Load += new System.EventHandler(this.FormSetCategory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgSrcItemList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgSrcAttribute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgTarAttribute)).EndInit();
            this.Menu_Attr1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel4;
        public MetroFramework.Controls.MetroTextBox txtSrcTitle;
        public MetroFramework.Controls.MetroTextBox TxtSrcCategoryId;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroButton BtnSearch;
        public MetroFramework.Controls.MetroTextBox TxtSearchText;
        private MetroFramework.Controls.MetroButton BtnSetCategory;
        private System.Windows.Forms.DataGridView dgSrcItemList;
        public MetroFramework.Controls.MetroTextBox TxtSrcCountry;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private System.Windows.Forms.DataGridView DgSrcAttribute;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgItemList_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgSrcItemList_Src_cat1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgSrcItemList_Src_cat2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgSrcItemList_Src_cat3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgSrcItemList_Src_cat3_id;
        public MetroFramework.Controls.MetroTextBox TxtItemId;
        private MetroFramework.Controls.MetroButton BtnSaveAttribute;
        private System.Windows.Forms.DataGridView DgTarAttribute;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroButton BtnClose;
        private MetroFramework.Controls.MetroContextMenu Menu_Attr1;
        private System.Windows.Forms.ToolStripMenuItem Menu_Attr1_copy_cell;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdSrcLocal;
        private System.Windows.Forms.RadioButton rdSrcKor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdTarLocal;
        private System.Windows.Forms.RadioButton rdTarKor;
        private System.Windows.Forms.ToolTip toolTipCommon;
    }
}