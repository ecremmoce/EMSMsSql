namespace ShopeeManagement
{
    partial class FormImageTool
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImageTool));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pic = new System.Windows.Forms.PictureBox();
            this.ud_margin = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_cap_end = new System.Windows.Forms.TextBox();
            this.txt_cap_start = new System.Windows.Forms.TextBox();
            this.rd_fit = new System.Windows.Forms.RadioButton();
            this.btn_save_image = new System.Windows.Forms.Button();
            this.txt_image_height = new System.Windows.Forms.TextBox();
            this.txt_image_width = new System.Windows.Forms.TextBox();
            this.chk_view_guide_line = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_y = new System.Windows.Forms.TextBox();
            this.txt_x = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rd_real = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ud_image_size = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.ud_sharpen = new System.Windows.Forms.NumericUpDown();
            this.chk_sharpen = new System.Windows.Forms.CheckBox();
            this.chk_for_big = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chk_auto_cut = new System.Windows.Forms.CheckBox();
            this.txt_cap_height = new System.Windows.Forms.TextBox();
            this.txt_cap_width = new System.Windows.Forms.TextBox();
            this.cmbEdgeDetection = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_image_delete = new System.Windows.Forms.Button();
            this.dgcolor_detail_idx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pic_cap = new System.Windows.Forms.PictureBox();
            this.dgcolor_detail = new System.Windows.Forms.DataGridView();
            this.dgcolor_detail_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcolor_detail_check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgcolor_detail_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcolor_detail_path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_color_detail_top = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.pic_color_image = new System.Windows.Forms.PictureBox();
            this.btn_single_up = new System.Windows.Forms.Button();
            this.btn_single_dn = new System.Windows.Forms.Button();
            this.pic_all = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_margin)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_image_size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_sharpen)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_cap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgcolor_detail)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_color_image)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_all)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.pic);
            this.panel1.Location = new System.Drawing.Point(7, 92);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1006, 945);
            this.panel1.TabIndex = 185;
            // 
            // pic
            // 
            this.pic.BackColor = System.Drawing.Color.White;
            this.pic.Location = new System.Drawing.Point(0, 4);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(998, 900);
            this.pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pic.TabIndex = 0;
            this.pic.TabStop = false;
            this.pic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic_MouseDown);
            this.pic.MouseEnter += new System.EventHandler(this.pic_MouseEnter);
            this.pic.MouseLeave += new System.EventHandler(this.pic_MouseLeave);
            this.pic.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pic_MouseMove);
            // 
            // ud_margin
            // 
            this.ud_margin.Location = new System.Drawing.Point(399, 17);
            this.ud_margin.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ud_margin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ud_margin.Name = "ud_margin";
            this.ud_margin.Size = new System.Drawing.Size(50, 21);
            this.ud_margin.TabIndex = 33;
            this.ud_margin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ud_margin.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_cap_end);
            this.groupBox2.Controls.Add(this.txt_cap_start);
            this.groupBox2.Controls.Add(this.rd_fit);
            this.groupBox2.Controls.Add(this.btn_save_image);
            this.groupBox2.Controls.Add(this.txt_image_height);
            this.groupBox2.Controls.Add(this.txt_image_width);
            this.groupBox2.Controls.Add(this.chk_view_guide_line);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txt_y);
            this.groupBox2.Controls.Add(this.txt_x);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.rd_real);
            this.groupBox2.Location = new System.Drawing.Point(7, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(741, 47);
            this.groupBox2.TabIndex = 178;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "보기방법";
            // 
            // txt_cap_end
            // 
            this.txt_cap_end.BackColor = System.Drawing.Color.White;
            this.txt_cap_end.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_cap_end.Location = new System.Drawing.Point(454, 17);
            this.txt_cap_end.Name = "txt_cap_end";
            this.txt_cap_end.ReadOnly = true;
            this.txt_cap_end.Size = new System.Drawing.Size(35, 21);
            this.txt_cap_end.TabIndex = 18;
            this.txt_cap_end.Text = "끝";
            this.txt_cap_end.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_cap_start
            // 
            this.txt_cap_start.BackColor = System.Drawing.Color.White;
            this.txt_cap_start.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_cap_start.Location = new System.Drawing.Point(417, 17);
            this.txt_cap_start.Name = "txt_cap_start";
            this.txt_cap_start.ReadOnly = true;
            this.txt_cap_start.Size = new System.Drawing.Size(36, 21);
            this.txt_cap_start.TabIndex = 17;
            this.txt_cap_start.Text = "시작";
            this.txt_cap_start.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // rd_fit
            // 
            this.rd_fit.AutoSize = true;
            this.rd_fit.Enabled = false;
            this.rd_fit.Location = new System.Drawing.Point(345, 20);
            this.rd_fit.Name = "rd_fit";
            this.rd_fit.Size = new System.Drawing.Size(75, 16);
            this.rd_fit.TabIndex = 0;
            this.rd_fit.Text = "창에 맞게";
            this.rd_fit.UseVisualStyleBackColor = true;
            // 
            // btn_save_image
            // 
            this.btn_save_image.BackColor = System.Drawing.Color.White;
            this.btn_save_image.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_save_image.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_save_image.Location = new System.Drawing.Point(583, 16);
            this.btn_save_image.Name = "btn_save_image";
            this.btn_save_image.Size = new System.Drawing.Size(152, 23);
            this.btn_save_image.TabIndex = 17;
            this.btn_save_image.Text = "이미지 저장(SPACE)";
            this.btn_save_image.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_save_image.UseVisualStyleBackColor = false;
            this.btn_save_image.Click += new System.EventHandler(this.btn_save_image_Click);
            // 
            // txt_image_height
            // 
            this.txt_image_height.BackColor = System.Drawing.Color.White;
            this.txt_image_height.Location = new System.Drawing.Point(231, 17);
            this.txt_image_height.Name = "txt_image_height";
            this.txt_image_height.ReadOnly = true;
            this.txt_image_height.Size = new System.Drawing.Size(37, 21);
            this.txt_image_height.TabIndex = 14;
            // 
            // txt_image_width
            // 
            this.txt_image_width.BackColor = System.Drawing.Color.White;
            this.txt_image_width.Location = new System.Drawing.Point(164, 17);
            this.txt_image_width.Name = "txt_image_width";
            this.txt_image_width.ReadOnly = true;
            this.txt_image_width.Size = new System.Drawing.Size(37, 21);
            this.txt_image_width.TabIndex = 13;
            // 
            // chk_view_guide_line
            // 
            this.chk_view_guide_line.AutoSize = true;
            this.chk_view_guide_line.Checked = true;
            this.chk_view_guide_line.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_view_guide_line.Location = new System.Drawing.Point(495, 20);
            this.chk_view_guide_line.Name = "chk_view_guide_line";
            this.chk_view_guide_line.Size = new System.Drawing.Size(88, 16);
            this.chk_view_guide_line.TabIndex = 19;
            this.chk_view_guide_line.Text = "가이드 라인";
            this.chk_view_guide_line.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(204, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "세로";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(138, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "가로";
            // 
            // txt_y
            // 
            this.txt_y.BackColor = System.Drawing.Color.White;
            this.txt_y.Location = new System.Drawing.Point(94, 17);
            this.txt_y.Name = "txt_y";
            this.txt_y.ReadOnly = true;
            this.txt_y.Size = new System.Drawing.Size(37, 21);
            this.txt_y.TabIndex = 7;
            // 
            // txt_x
            // 
            this.txt_x.BackColor = System.Drawing.Color.White;
            this.txt_x.Location = new System.Drawing.Point(56, 17);
            this.txt_x.Name = "txt_x";
            this.txt_x.ReadOnly = true;
            this.txt_x.Size = new System.Drawing.Size(37, 21);
            this.txt_x.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "시작좌표";
            // 
            // rd_real
            // 
            this.rd_real.AutoSize = true;
            this.rd_real.Checked = true;
            this.rd_real.Location = new System.Drawing.Point(275, 20);
            this.rd_real.Name = "rd_real";
            this.rd_real.Size = new System.Drawing.Size(71, 16);
            this.rd_real.TabIndex = 1;
            this.rd_real.TabStop = true;
            this.rd_real.Text = "실제크기";
            this.rd_real.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ud_image_size);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.ud_sharpen);
            this.groupBox3.Controls.Add(this.chk_sharpen);
            this.groupBox3.Controls.Add(this.chk_for_big);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.ud_margin);
            this.groupBox3.Controls.Add(this.chk_auto_cut);
            this.groupBox3.Controls.Add(this.txt_cap_height);
            this.groupBox3.Controls.Add(this.txt_cap_width);
            this.groupBox3.Controls.Add(this.cmbEdgeDetection);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.btn_close);
            this.groupBox3.Location = new System.Drawing.Point(1074, 41);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(827, 47);
            this.groupBox3.TabIndex = 179;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "캡춰 이미지";
            // 
            // ud_image_size
            // 
            this.ud_image_size.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ud_image_size.Location = new System.Drawing.Point(552, 16);
            this.ud_image_size.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.ud_image_size.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.ud_image_size.Name = "ud_image_size";
            this.ud_image_size.Size = new System.Drawing.Size(47, 21);
            this.ud_image_size.TabIndex = 38;
            this.ud_image_size.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ud_image_size.Value = new decimal(new int[] {
            1001,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(600, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 39;
            this.label3.Text = "픽셀";
            // 
            // ud_sharpen
            // 
            this.ud_sharpen.DecimalPlaces = 1;
            this.ud_sharpen.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ud_sharpen.Location = new System.Drawing.Point(675, 16);
            this.ud_sharpen.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.ud_sharpen.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ud_sharpen.Name = "ud_sharpen";
            this.ud_sharpen.Size = new System.Drawing.Size(50, 21);
            this.ud_sharpen.TabIndex = 37;
            this.ud_sharpen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ud_sharpen.Value = new decimal(new int[] {
            11,
            0,
            0,
            65536});
            // 
            // chk_sharpen
            // 
            this.chk_sharpen.AutoSize = true;
            this.chk_sharpen.Location = new System.Drawing.Point(632, 19);
            this.chk_sharpen.Name = "chk_sharpen";
            this.chk_sharpen.Size = new System.Drawing.Size(48, 16);
            this.chk_sharpen.TabIndex = 36;
            this.chk_sharpen.Text = "샤픈";
            this.chk_sharpen.UseVisualStyleBackColor = true;
            // 
            // chk_for_big
            // 
            this.chk_for_big.AutoSize = true;
            this.chk_for_big.Checked = true;
            this.chk_for_big.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_for_big.Location = new System.Drawing.Point(484, 19);
            this.chk_for_big.Name = "chk_for_big";
            this.chk_for_big.Size = new System.Drawing.Size(72, 16);
            this.chk_for_big.TabIndex = 35;
            this.chk_for_big.Text = "크기조정";
            this.chk_for_big.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(449, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 34;
            this.label2.Text = "픽셀";
            // 
            // chk_auto_cut
            // 
            this.chk_auto_cut.AutoSize = true;
            this.chk_auto_cut.Checked = true;
            this.chk_auto_cut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_auto_cut.Location = new System.Drawing.Point(355, 20);
            this.chk_auto_cut.Name = "chk_auto_cut";
            this.chk_auto_cut.Size = new System.Drawing.Size(48, 16);
            this.chk_auto_cut.TabIndex = 32;
            this.chk_auto_cut.Text = "여백";
            this.chk_auto_cut.UseVisualStyleBackColor = true;
            // 
            // txt_cap_height
            // 
            this.txt_cap_height.Location = new System.Drawing.Point(94, 17);
            this.txt_cap_height.Name = "txt_cap_height";
            this.txt_cap_height.Size = new System.Drawing.Size(37, 21);
            this.txt_cap_height.TabIndex = 19;
            // 
            // txt_cap_width
            // 
            this.txt_cap_width.Location = new System.Drawing.Point(29, 17);
            this.txt_cap_width.Name = "txt_cap_width";
            this.txt_cap_width.Size = new System.Drawing.Size(37, 21);
            this.txt_cap_width.TabIndex = 18;
            // 
            // cmbEdgeDetection
            // 
            this.cmbEdgeDetection.BackColor = System.Drawing.Color.White;
            this.cmbEdgeDetection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEdgeDetection.Font = new System.Drawing.Font("굴림체", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEdgeDetection.FormattingEnabled = true;
            this.cmbEdgeDetection.Items.AddRange(new object[] {
            "None",
            "Laplacian 3x3",
            "Laplacian 3x3 Grayscale",
            "Laplacian 5x5",
            "Laplacian 5x5 Grayscale",
            "Laplacian of Gaussian",
            "Laplacian 3x3 of Gaussian 3x3",
            "Laplacian 3x3 of Gaussian 5x5 - 1",
            "Laplacian 3x3 of Gaussian 5x5 - 2",
            "Laplacian 5x5 of Gaussian 3x3",
            "Laplacian 5x5 of Gaussian 5x5 - 1",
            "Laplacian 5x5 of Gaussian 5x5 - 2",
            "Sobel 3x3",
            "Sobel 3x3 Grayscale",
            "Prewitt",
            "Prewitt Grayscale",
            "Kirsch",
            "Kirsch Grayscale"});
            this.cmbEdgeDetection.Location = new System.Drawing.Point(132, 18);
            this.cmbEdgeDetection.Name = "cmbEdgeDetection";
            this.cmbEdgeDetection.Size = new System.Drawing.Size(219, 19);
            this.cmbEdgeDetection.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(67, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "세로";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(2, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 20;
            this.label8.Text = "가로";
            // 
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.Color.White;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_close.Location = new System.Drawing.Point(730, 16);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(91, 23);
            this.btn_close.TabIndex = 4;
            this.btn_close.Text = "닫기(Esc)";
            this.btn_close.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_image_delete
            // 
            this.btn_image_delete.BackColor = System.Drawing.Color.White;
            this.btn_image_delete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_image_delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_image_delete.Location = new System.Drawing.Point(1319, 1003);
            this.btn_image_delete.Name = "btn_image_delete";
            this.btn_image_delete.Size = new System.Drawing.Size(53, 25);
            this.btn_image_delete.TabIndex = 183;
            this.btn_image_delete.Text = "삭제";
            this.btn_image_delete.UseVisualStyleBackColor = false;
            this.btn_image_delete.Click += new System.EventHandler(this.btn_image_delete_Click);
            // 
            // dgcolor_detail_idx
            // 
            this.dgcolor_detail_idx.HeaderText = "idx";
            this.dgcolor_detail_idx.Name = "dgcolor_detail_idx";
            this.dgcolor_detail_idx.ReadOnly = true;
            this.dgcolor_detail_idx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgcolor_detail_idx.Visible = false;
            this.dgcolor_detail_idx.Width = 30;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pic_cap);
            this.groupBox4.Location = new System.Drawing.Point(1319, 91);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(582, 596);
            this.groupBox4.TabIndex = 180;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "캡춰 이미지";
            // 
            // pic_cap
            // 
            this.pic_cap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pic_cap.Location = new System.Drawing.Point(3, 17);
            this.pic_cap.Name = "pic_cap";
            this.pic_cap.Size = new System.Drawing.Size(576, 576);
            this.pic_cap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic_cap.TabIndex = 5;
            this.pic_cap.TabStop = false;
            // 
            // dgcolor_detail
            // 
            this.dgcolor_detail.AllowDrop = true;
            this.dgcolor_detail.AllowUserToAddRows = false;
            this.dgcolor_detail.AllowUserToDeleteRows = false;
            this.dgcolor_detail.AllowUserToResizeColumns = false;
            this.dgcolor_detail.AllowUserToResizeRows = false;
            this.dgcolor_detail.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgcolor_detail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgcolor_detail.ColumnHeadersHeight = 18;
            this.dgcolor_detail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgcolor_detail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgcolor_detail_idx,
            this.dgcolor_detail_no,
            this.dgcolor_detail_check,
            this.dgcolor_detail_name,
            this.dgcolor_detail_path});
            this.dgcolor_detail.Location = new System.Drawing.Point(1319, 700);
            this.dgcolor_detail.MultiSelect = false;
            this.dgcolor_detail.Name = "dgcolor_detail";
            this.dgcolor_detail.RowHeadersVisible = false;
            this.dgcolor_detail.RowTemplate.Height = 21;
            this.dgcolor_detail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgcolor_detail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgcolor_detail.Size = new System.Drawing.Size(234, 299);
            this.dgcolor_detail.TabIndex = 182;
            this.dgcolor_detail.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgcolor_detail_CellClick);
            // 
            // dgcolor_detail_no
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgcolor_detail_no.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgcolor_detail_no.HeaderText = "No";
            this.dgcolor_detail_no.Name = "dgcolor_detail_no";
            this.dgcolor_detail_no.ReadOnly = true;
            this.dgcolor_detail_no.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgcolor_detail_no.Width = 35;
            // 
            // dgcolor_detail_check
            // 
            this.dgcolor_detail_check.HeaderText = "V";
            this.dgcolor_detail_check.Name = "dgcolor_detail_check";
            this.dgcolor_detail_check.Width = 25;
            // 
            // dgcolor_detail_name
            // 
            this.dgcolor_detail_name.HeaderText = "편집";
            this.dgcolor_detail_name.Name = "dgcolor_detail_name";
            this.dgcolor_detail_name.ReadOnly = true;
            this.dgcolor_detail_name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgcolor_detail_name.Width = 150;
            // 
            // dgcolor_detail_path
            // 
            this.dgcolor_detail_path.HeaderText = "파일위치";
            this.dgcolor_detail_path.Name = "dgcolor_detail_path";
            this.dgcolor_detail_path.ReadOnly = true;
            this.dgcolor_detail_path.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgcolor_detail_path.Visible = false;
            this.dgcolor_detail_path.Width = 300;
            // 
            // btn_color_detail_top
            // 
            this.btn_color_detail_top.BackColor = System.Drawing.Color.White;
            this.btn_color_detail_top.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Magenta;
            this.btn_color_detail_top.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_color_detail_top.Font = new System.Drawing.Font("굴림체", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_color_detail_top.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_color_detail_top.Location = new System.Drawing.Point(1467, 1003);
            this.btn_color_detail_top.Name = "btn_color_detail_top";
            this.btn_color_detail_top.Size = new System.Drawing.Size(86, 25);
            this.btn_color_detail_top.TabIndex = 188;
            this.btn_color_detail_top.Text = "맨위로 F4";
            this.btn_color_detail_top.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_color_detail_top.UseVisualStyleBackColor = false;
            this.btn_color_detail_top.Click += new System.EventHandler(this.btn_color_detail_top_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.pic_color_image);
            this.groupBox5.Location = new System.Drawing.Point(1578, 692);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(323, 323);
            this.groupBox5.TabIndex = 181;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "캡춰 이미지";
            // 
            // pic_color_image
            // 
            this.pic_color_image.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pic_color_image.Location = new System.Drawing.Point(3, 17);
            this.pic_color_image.Name = "pic_color_image";
            this.pic_color_image.Size = new System.Drawing.Size(317, 303);
            this.pic_color_image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic_color_image.TabIndex = 5;
            this.pic_color_image.TabStop = false;
            // 
            // btn_single_up
            // 
            this.btn_single_up.BackColor = System.Drawing.Color.Linen;
            this.btn_single_up.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Magenta;
            this.btn_single_up.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_single_up.Font = new System.Drawing.Font("굴림체", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_single_up.Image = ((System.Drawing.Image)(resources.GetObject("btn_single_up.Image")));
            this.btn_single_up.Location = new System.Drawing.Point(1380, 1003);
            this.btn_single_up.Name = "btn_single_up";
            this.btn_single_up.Size = new System.Drawing.Size(25, 25);
            this.btn_single_up.TabIndex = 186;
            this.btn_single_up.UseVisualStyleBackColor = false;
            this.btn_single_up.Click += new System.EventHandler(this.btn_single_up_Click);
            // 
            // btn_single_dn
            // 
            this.btn_single_dn.BackColor = System.Drawing.Color.Linen;
            this.btn_single_dn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btn_single_dn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_single_dn.Font = new System.Drawing.Font("굴림체", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_single_dn.Image = ((System.Drawing.Image)(resources.GetObject("btn_single_dn.Image")));
            this.btn_single_dn.Location = new System.Drawing.Point(1411, 1003);
            this.btn_single_dn.Name = "btn_single_dn";
            this.btn_single_dn.Size = new System.Drawing.Size(25, 25);
            this.btn_single_dn.TabIndex = 187;
            this.btn_single_dn.UseVisualStyleBackColor = false;
            this.btn_single_dn.Click += new System.EventHandler(this.btn_single_dn_Click);
            // 
            // pic_all
            // 
            this.pic_all.Location = new System.Drawing.Point(1026, 92);
            this.pic_all.Name = "pic_all";
            this.pic_all.Size = new System.Drawing.Size(282, 945);
            this.pic_all.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic_all.TabIndex = 184;
            this.pic_all.TabStop = false;
            // 
            // FormImageTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1084);
            this.Controls.Add(this.btn_color_detail_top);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_single_up);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btn_image_delete);
            this.Controls.Add(this.btn_single_dn);
            this.Controls.Add(this.pic_all);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.dgcolor_detail);
            this.Controls.Add(this.groupBox5);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImageTool";
            this.Resizable = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.Load += new System.EventHandler(this.FormImageTool_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_margin)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_image_size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_sharpen)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic_cap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgcolor_detail)).EndInit();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic_color_image)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_all)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.PictureBox pic;
        private System.Windows.Forms.NumericUpDown ud_margin;
        private System.Windows.Forms.Button btn_single_up;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_cap_end;
        private System.Windows.Forms.TextBox txt_cap_start;
        private System.Windows.Forms.RadioButton rd_fit;
        private System.Windows.Forms.Button btn_save_image;
        private System.Windows.Forms.TextBox txt_image_height;
        private System.Windows.Forms.TextBox txt_image_width;
        private System.Windows.Forms.CheckBox chk_view_guide_line;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_y;
        private System.Windows.Forms.TextBox txt_x;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rd_real;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown ud_image_size;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown ud_sharpen;
        private System.Windows.Forms.CheckBox chk_sharpen;
        private System.Windows.Forms.CheckBox chk_for_big;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chk_auto_cut;
        private System.Windows.Forms.TextBox txt_cap_height;
        private System.Windows.Forms.TextBox txt_cap_width;
        private System.Windows.Forms.ComboBox cmbEdgeDetection;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_image_delete;
        private System.Windows.Forms.Button btn_single_dn;
        private System.Windows.Forms.PictureBox pic_all;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcolor_detail_idx;
        private System.Windows.Forms.PictureBox pic_cap;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dgcolor_detail;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcolor_detail_no;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgcolor_detail_check;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcolor_detail_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcolor_detail_path;
        private System.Windows.Forms.Button btn_color_detail_top;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.PictureBox pic_color_image;
    }
}