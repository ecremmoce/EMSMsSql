namespace ShopeeManagement
{
    partial class FormCategoryMapper
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCategoryMapper));
            this.btnLoadCategory = new MetroFramework.Controls.MetroButton();
            this.gridCategory = new MetroFramework.Controls.MetroGrid();
            this.gridCategory_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_id_l1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_id_l2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_id_l3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_my_l1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_my_l2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_my_l3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_ph_l1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_ph_l2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_ph_l3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_sg_l1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_sg_l2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_sg_l3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_th_l1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_th_l2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_th_l3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_tw_l1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_tw_l2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_tw_l3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_vn_l1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_vn_l2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_vn_l3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_my = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_ph = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_sg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_th = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_tw = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCategory_vn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnUploadCategory = new MetroFramework.Controls.MetroButton();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.btn_save_category = new MetroFramework.Controls.MetroButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnDeleteMapData = new MetroFramework.Controls.MetroButton();
            this.btnGetCategoryData = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadCategory
            // 
            this.btnLoadCategory.Location = new System.Drawing.Point(13, 15);
            this.btnLoadCategory.Name = "btnLoadCategory";
            this.btnLoadCategory.Size = new System.Drawing.Size(124, 23);
            this.btnLoadCategory.TabIndex = 0;
            this.btnLoadCategory.Text = "카테고리 로드";
            this.btnLoadCategory.UseCustomBackColor = true;
            this.btnLoadCategory.UseSelectable = true;
            this.btnLoadCategory.Click += new System.EventHandler(this.btnLoadCategory_Click);
            // 
            // gridCategory
            // 
            this.gridCategory.AllowUserToAddRows = false;
            this.gridCategory.AllowUserToDeleteRows = false;
            this.gridCategory.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridCategory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridCategory.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridCategory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridCategory.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridCategory.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridCategory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridCategory.ColumnHeadersHeight = 25;
            this.gridCategory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridCategory_no,
            this.gridCategory_id_l1,
            this.gridCategory_id_l2,
            this.gridCategory_id_l3,
            this.gridCategory_my_l1,
            this.gridCategory_my_l2,
            this.gridCategory_my_l3,
            this.gridCategory_ph_l1,
            this.gridCategory_ph_l2,
            this.gridCategory_ph_l3,
            this.gridCategory_sg_l1,
            this.gridCategory_sg_l2,
            this.gridCategory_sg_l3,
            this.gridCategory_th_l1,
            this.gridCategory_th_l2,
            this.gridCategory_th_l3,
            this.gridCategory_tw_l1,
            this.gridCategory_tw_l2,
            this.gridCategory_tw_l3,
            this.gridCategory_vn_l1,
            this.gridCategory_vn_l2,
            this.gridCategory_vn_l3,
            this.gridCategory_id,
            this.gridCategory_my,
            this.gridCategory_ph,
            this.gridCategory_sg,
            this.gridCategory_th,
            this.gridCategory_tw,
            this.gridCategory_vn});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridCategory.DefaultCellStyle = dataGridViewCellStyle11;
            this.gridCategory.EnableHeadersVisualStyles = false;
            this.gridCategory.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridCategory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridCategory.Location = new System.Drawing.Point(13, 49);
            this.gridCategory.Name = "gridCategory";
            this.gridCategory.ReadOnly = true;
            this.gridCategory.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridCategory.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.gridCategory.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridCategory.RowTemplate.Height = 23;
            this.gridCategory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCategory.Size = new System.Drawing.Size(1892, 864);
            this.gridCategory.TabIndex = 1;
            // 
            // gridCategory_no
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridCategory_no.DefaultCellStyle = dataGridViewCellStyle3;
            this.gridCategory_no.HeaderText = "No";
            this.gridCategory_no.Name = "gridCategory_no";
            this.gridCategory_no.ReadOnly = true;
            this.gridCategory_no.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gridCategory_id_l1
            // 
            this.gridCategory_id_l1.HeaderText = "ID L1";
            this.gridCategory_id_l1.Name = "gridCategory_id_l1";
            this.gridCategory_id_l1.ReadOnly = true;
            this.gridCategory_id_l1.Width = 200;
            // 
            // gridCategory_id_l2
            // 
            this.gridCategory_id_l2.HeaderText = "ID L2";
            this.gridCategory_id_l2.Name = "gridCategory_id_l2";
            this.gridCategory_id_l2.ReadOnly = true;
            this.gridCategory_id_l2.Width = 200;
            // 
            // gridCategory_id_l3
            // 
            this.gridCategory_id_l3.HeaderText = "ID L3";
            this.gridCategory_id_l3.Name = "gridCategory_id_l3";
            this.gridCategory_id_l3.ReadOnly = true;
            this.gridCategory_id_l3.Width = 200;
            // 
            // gridCategory_my_l1
            // 
            this.gridCategory_my_l1.HeaderText = "MY L1";
            this.gridCategory_my_l1.Name = "gridCategory_my_l1";
            this.gridCategory_my_l1.ReadOnly = true;
            this.gridCategory_my_l1.Width = 200;
            // 
            // gridCategory_my_l2
            // 
            this.gridCategory_my_l2.HeaderText = "MY L2";
            this.gridCategory_my_l2.Name = "gridCategory_my_l2";
            this.gridCategory_my_l2.ReadOnly = true;
            this.gridCategory_my_l2.Width = 200;
            // 
            // gridCategory_my_l3
            // 
            this.gridCategory_my_l3.HeaderText = "MY L3";
            this.gridCategory_my_l3.Name = "gridCategory_my_l3";
            this.gridCategory_my_l3.ReadOnly = true;
            this.gridCategory_my_l3.Width = 200;
            // 
            // gridCategory_ph_l1
            // 
            this.gridCategory_ph_l1.HeaderText = "PH L1";
            this.gridCategory_ph_l1.Name = "gridCategory_ph_l1";
            this.gridCategory_ph_l1.ReadOnly = true;
            this.gridCategory_ph_l1.Width = 200;
            // 
            // gridCategory_ph_l2
            // 
            this.gridCategory_ph_l2.HeaderText = "PH L2";
            this.gridCategory_ph_l2.Name = "gridCategory_ph_l2";
            this.gridCategory_ph_l2.ReadOnly = true;
            this.gridCategory_ph_l2.Width = 200;
            // 
            // gridCategory_ph_l3
            // 
            this.gridCategory_ph_l3.HeaderText = "PH L3";
            this.gridCategory_ph_l3.Name = "gridCategory_ph_l3";
            this.gridCategory_ph_l3.ReadOnly = true;
            this.gridCategory_ph_l3.Width = 200;
            // 
            // gridCategory_sg_l1
            // 
            this.gridCategory_sg_l1.HeaderText = "SG L1";
            this.gridCategory_sg_l1.Name = "gridCategory_sg_l1";
            this.gridCategory_sg_l1.ReadOnly = true;
            this.gridCategory_sg_l1.Width = 200;
            // 
            // gridCategory_sg_l2
            // 
            this.gridCategory_sg_l2.HeaderText = "SG L2";
            this.gridCategory_sg_l2.Name = "gridCategory_sg_l2";
            this.gridCategory_sg_l2.ReadOnly = true;
            this.gridCategory_sg_l2.Width = 200;
            // 
            // gridCategory_sg_l3
            // 
            this.gridCategory_sg_l3.HeaderText = "SG L3";
            this.gridCategory_sg_l3.Name = "gridCategory_sg_l3";
            this.gridCategory_sg_l3.ReadOnly = true;
            this.gridCategory_sg_l3.Width = 200;
            // 
            // gridCategory_th_l1
            // 
            this.gridCategory_th_l1.HeaderText = "TH L1";
            this.gridCategory_th_l1.Name = "gridCategory_th_l1";
            this.gridCategory_th_l1.ReadOnly = true;
            this.gridCategory_th_l1.Width = 200;
            // 
            // gridCategory_th_l2
            // 
            this.gridCategory_th_l2.HeaderText = "TH L2";
            this.gridCategory_th_l2.Name = "gridCategory_th_l2";
            this.gridCategory_th_l2.ReadOnly = true;
            this.gridCategory_th_l2.Width = 200;
            // 
            // gridCategory_th_l3
            // 
            this.gridCategory_th_l3.HeaderText = "TH L3";
            this.gridCategory_th_l3.Name = "gridCategory_th_l3";
            this.gridCategory_th_l3.ReadOnly = true;
            this.gridCategory_th_l3.Width = 200;
            // 
            // gridCategory_tw_l1
            // 
            this.gridCategory_tw_l1.HeaderText = "TW L1";
            this.gridCategory_tw_l1.Name = "gridCategory_tw_l1";
            this.gridCategory_tw_l1.ReadOnly = true;
            this.gridCategory_tw_l1.Width = 200;
            // 
            // gridCategory_tw_l2
            // 
            this.gridCategory_tw_l2.HeaderText = "TW L2";
            this.gridCategory_tw_l2.Name = "gridCategory_tw_l2";
            this.gridCategory_tw_l2.ReadOnly = true;
            this.gridCategory_tw_l2.Width = 200;
            // 
            // gridCategory_tw_l3
            // 
            this.gridCategory_tw_l3.HeaderText = "TW L3";
            this.gridCategory_tw_l3.Name = "gridCategory_tw_l3";
            this.gridCategory_tw_l3.ReadOnly = true;
            this.gridCategory_tw_l3.Width = 200;
            // 
            // gridCategory_vn_l1
            // 
            this.gridCategory_vn_l1.HeaderText = "VN L1";
            this.gridCategory_vn_l1.Name = "gridCategory_vn_l1";
            this.gridCategory_vn_l1.ReadOnly = true;
            this.gridCategory_vn_l1.Width = 200;
            // 
            // gridCategory_vn_l2
            // 
            this.gridCategory_vn_l2.HeaderText = "VN L2";
            this.gridCategory_vn_l2.Name = "gridCategory_vn_l2";
            this.gridCategory_vn_l2.ReadOnly = true;
            this.gridCategory_vn_l2.Width = 200;
            // 
            // gridCategory_vn_l3
            // 
            this.gridCategory_vn_l3.HeaderText = "VN L3";
            this.gridCategory_vn_l3.Name = "gridCategory_vn_l3";
            this.gridCategory_vn_l3.ReadOnly = true;
            this.gridCategory_vn_l3.Width = 200;
            // 
            // gridCategory_id
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridCategory_id.DefaultCellStyle = dataGridViewCellStyle4;
            this.gridCategory_id.HeaderText = "ID";
            this.gridCategory_id.Name = "gridCategory_id";
            this.gridCategory_id.ReadOnly = true;
            this.gridCategory_id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gridCategory_my
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridCategory_my.DefaultCellStyle = dataGridViewCellStyle5;
            this.gridCategory_my.HeaderText = "MY";
            this.gridCategory_my.Name = "gridCategory_my";
            this.gridCategory_my.ReadOnly = true;
            this.gridCategory_my.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gridCategory_ph
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridCategory_ph.DefaultCellStyle = dataGridViewCellStyle6;
            this.gridCategory_ph.HeaderText = "PH";
            this.gridCategory_ph.Name = "gridCategory_ph";
            this.gridCategory_ph.ReadOnly = true;
            this.gridCategory_ph.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gridCategory_sg
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridCategory_sg.DefaultCellStyle = dataGridViewCellStyle7;
            this.gridCategory_sg.HeaderText = "SG";
            this.gridCategory_sg.Name = "gridCategory_sg";
            this.gridCategory_sg.ReadOnly = true;
            this.gridCategory_sg.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gridCategory_th
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridCategory_th.DefaultCellStyle = dataGridViewCellStyle8;
            this.gridCategory_th.HeaderText = "TH";
            this.gridCategory_th.Name = "gridCategory_th";
            this.gridCategory_th.ReadOnly = true;
            this.gridCategory_th.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gridCategory_tw
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridCategory_tw.DefaultCellStyle = dataGridViewCellStyle9;
            this.gridCategory_tw.HeaderText = "TW";
            this.gridCategory_tw.Name = "gridCategory_tw";
            this.gridCategory_tw.ReadOnly = true;
            this.gridCategory_tw.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gridCategory_vn
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridCategory_vn.DefaultCellStyle = dataGridViewCellStyle10;
            this.gridCategory_vn.HeaderText = "VN";
            this.gridCategory_vn.Name = "gridCategory_vn";
            this.gridCategory_vn.ReadOnly = true;
            this.gridCategory_vn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btnUploadCategory
            // 
            this.btnUploadCategory.Location = new System.Drawing.Point(143, 15);
            this.btnUploadCategory.Name = "btnUploadCategory";
            this.btnUploadCategory.Size = new System.Drawing.Size(224, 23);
            this.btnUploadCategory.TabIndex = 2;
            this.btnUploadCategory.Text = "카테고리 엑셀 파일 업로드";
            this.btnUploadCategory.UseCustomBackColor = true;
            this.btnUploadCategory.UseSelectable = true;
            this.btnUploadCategory.Click += new System.EventHandler(this.btnUploadCategory_Click);
            // 
            // metroStyleManager
            // 
            this.metroStyleManager.Owner = null;
            // 
            // btn_save_category
            // 
            this.btn_save_category.Location = new System.Drawing.Point(555, 15);
            this.btn_save_category.Name = "btn_save_category";
            this.btn_save_category.Size = new System.Drawing.Size(87, 23);
            this.btn_save_category.TabIndex = 3;
            this.btn_save_category.Text = "저장";
            this.btn_save_category.UseCustomBackColor = true;
            this.btn_save_category.UseSelectable = true;
            this.btn_save_category.Click += new System.EventHandler(this.btn_save_category_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(741, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(560, 21);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "전체 카테고리가 연결되어 있는 카테고리 맵핑 파일을 다운받아 업로드 하세요.";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnDeleteMapData
            // 
            this.btnDeleteMapData.Location = new System.Drawing.Point(645, 15);
            this.btnDeleteMapData.Name = "btnDeleteMapData";
            this.btnDeleteMapData.Size = new System.Drawing.Size(87, 23);
            this.btnDeleteMapData.TabIndex = 5;
            this.btnDeleteMapData.Text = "자료 삭제";
            this.btnDeleteMapData.UseCustomBackColor = true;
            this.btnDeleteMapData.UseSelectable = true;
            this.btnDeleteMapData.Click += new System.EventHandler(this.BtnDeleteMapData_Click);
            // 
            // btnGetCategoryData
            // 
            this.btnGetCategoryData.Location = new System.Drawing.Point(373, 15);
            this.btnGetCategoryData.Name = "btnGetCategoryData";
            this.btnGetCategoryData.Size = new System.Drawing.Size(156, 23);
            this.btnGetCategoryData.TabIndex = 6;
            this.btnGetCategoryData.Text = "카테고리 데이터 자동 수신";
            this.btnGetCategoryData.UseCustomBackColor = true;
            this.btnGetCategoryData.UseSelectable = true;
            this.btnGetCategoryData.Click += new System.EventHandler(this.BtnGetCategoryData_Click);
            // 
            // FormCategoryMapper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(2063, 1003);
            this.Controls.Add(this.btnGetCategoryData);
            this.Controls.Add(this.btnDeleteMapData);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_save_category);
            this.Controls.Add(this.btnUploadCategory);
            this.Controls.Add(this.gridCategory);
            this.Controls.Add(this.btnLoadCategory);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCategoryMapper";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormCategoryMapper_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroButton btnLoadCategory;
        private MetroFramework.Controls.MetroGrid gridCategory;
        private MetroFramework.Controls.MetroButton btnUploadCategory;
        public MetroFramework.Components.MetroStyleManager metroStyleManager;
        private MetroFramework.Controls.MetroButton btn_save_category;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_id_l1;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_id_l2;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_id_l3;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_my_l1;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_my_l2;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_my_l3;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_ph_l1;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_ph_l2;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_ph_l3;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_sg_l1;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_sg_l2;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_sg_l3;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_th_l1;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_th_l2;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_th_l3;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_tw_l1;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_tw_l2;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_tw_l3;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_vn_l1;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_vn_l2;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_vn_l3;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_my;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_ph;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_sg;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_th;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_tw;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridCategory_vn;
        private System.Windows.Forms.TextBox textBox1;
        private MetroFramework.Controls.MetroButton btnDeleteMapData;
        private MetroFramework.Controls.MetroButton btnGetCategoryData;
    }
}