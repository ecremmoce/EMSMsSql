namespace ShopeeManagement
{
    partial class FormUploadExcelPrice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUploadExcelPrice));
            this.CboNetPriceColumn = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.BtnSelectExcelFile = new MetroFramework.Controls.MetroButton();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.cboWeightCol = new MetroFramework.Controls.MetroComboBox();
            this.BtnSaveData = new MetroFramework.Controls.MetroButton();
            this.BtnClose = new MetroFramework.Controls.MetroButton();
            this.CboMarginColumn = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.dgSrcItemList = new System.Windows.Forms.DataGridView();
            this.cboProductIdCol = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.cboOptionIdCol = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.chkUpdateSKU = new System.Windows.Forms.CheckBox();
            this.cboProductSKUCol = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel9 = new MetroFramework.Controls.MetroLabel();
            this.cboOptionSKUCol = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel10 = new MetroFramework.Controls.MetroLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.UdPayoneerFee = new System.Windows.Forms.NumericUpDown();
            this.metroLabel11 = new MetroFramework.Controls.MetroLabel();
            this.UdRetailPriceRate = new System.Windows.Forms.NumericUpDown();
            this.LblRetailPriceRate = new MetroFramework.Controls.MetroLabel();
            this.udPGFee = new System.Windows.Forms.NumericUpDown();
            this.UdShopeeFee = new System.Windows.Forms.NumericUpDown();
            this.metroLabel12 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel13 = new MetroFramework.Controls.MetroLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.UdQty = new System.Windows.Forms.NumericUpDown();
            this.metroLabel14 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel19 = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgSrcItemList)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UdPayoneerFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UdRetailPriceRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPGFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UdShopeeFee)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UdQty)).BeginInit();
            this.SuspendLayout();
            // 
            // CboNetPriceColumn
            // 
            this.CboNetPriceColumn.FormattingEnabled = true;
            this.CboNetPriceColumn.ItemHeight = 23;
            this.CboNetPriceColumn.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "AA",
            "AB",
            "AC",
            "AD",
            "AE",
            "AF",
            "AG",
            "AH",
            "AI",
            "AJ",
            "AK",
            "AL",
            "AM",
            "AN",
            "AO",
            "AP",
            "AQ",
            "AR",
            "AS",
            "AT",
            "AU",
            "AV",
            "AW",
            "AX",
            "AY",
            "AZ",
            "BA",
            "BB",
            "BC",
            "BD",
            "BE",
            "BF",
            "BG",
            "BH",
            "BI",
            "BJ",
            "BK",
            "BL",
            "BM",
            "BN",
            "BO",
            "BP",
            "BQ",
            "BR",
            "BS",
            "BT",
            "BU",
            "BV",
            "BW",
            "BX",
            "BY",
            "BZ"});
            this.CboNetPriceColumn.Location = new System.Drawing.Point(495, 66);
            this.CboNetPriceColumn.Name = "CboNetPriceColumn";
            this.CboNetPriceColumn.Size = new System.Drawing.Size(173, 29);
            this.CboNetPriceColumn.TabIndex = 0;
            this.CboNetPriceColumn.UseSelectable = true;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(403, 70);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(98, 19);
            this.metroLabel1.TabIndex = 1;
            this.metroLabel1.Text = "상품 원가 행 : ";
            // 
            // BtnSelectExcelFile
            // 
            this.BtnSelectExcelFile.Location = new System.Drawing.Point(14, 29);
            this.BtnSelectExcelFile.Name = "BtnSelectExcelFile";
            this.BtnSelectExcelFile.Size = new System.Drawing.Size(94, 63);
            this.BtnSelectExcelFile.TabIndex = 145;
            this.BtnSelectExcelFile.Text = "엑셀파일 선택";
            this.BtnSelectExcelFile.UseCustomBackColor = true;
            this.BtnSelectExcelFile.UseSelectable = true;
            this.BtnSelectExcelFile.Click += new System.EventHandler(this.BtnSelectExcelFile_Click);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(115, 70);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(98, 19);
            this.metroLabel3.TabIndex = 147;
            this.metroLabel3.Text = "상품 무게 행 : ";
            // 
            // cboWeightCol
            // 
            this.cboWeightCol.FormattingEnabled = true;
            this.cboWeightCol.ItemHeight = 23;
            this.cboWeightCol.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "AA",
            "AB",
            "AC",
            "AD",
            "AE",
            "AF",
            "AG",
            "AH",
            "AI",
            "AJ",
            "AK",
            "AL",
            "AM",
            "AN",
            "AO",
            "AP",
            "AQ",
            "AR",
            "AS",
            "AT",
            "AU",
            "AV",
            "AW",
            "AX",
            "AY",
            "AZ",
            "BA",
            "BB",
            "BC",
            "BD",
            "BE",
            "BF",
            "BG",
            "BH",
            "BI",
            "BJ",
            "BK",
            "BL",
            "BM",
            "BN",
            "BO",
            "BP",
            "BQ",
            "BR",
            "BS",
            "BT",
            "BU",
            "BV",
            "BW",
            "BX",
            "BY",
            "BZ"});
            this.cboWeightCol.Location = new System.Drawing.Point(210, 66);
            this.cboWeightCol.Name = "cboWeightCol";
            this.cboWeightCol.Size = new System.Drawing.Size(173, 29);
            this.cboWeightCol.TabIndex = 146;
            this.cboWeightCol.UseSelectable = true;
            // 
            // BtnSaveData
            // 
            this.BtnSaveData.Location = new System.Drawing.Point(1709, 49);
            this.BtnSaveData.Name = "BtnSaveData";
            this.BtnSaveData.Size = new System.Drawing.Size(110, 47);
            this.BtnSaveData.TabIndex = 150;
            this.BtnSaveData.Text = "데이터 저장";
            this.BtnSaveData.UseCustomBackColor = true;
            this.BtnSaveData.UseSelectable = true;
            this.BtnSaveData.Click += new System.EventHandler(this.BtnSaveData_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(1709, 111);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(110, 29);
            this.BtnClose.TabIndex = 151;
            this.BtnClose.Text = "닫기";
            this.BtnClose.UseCustomBackColor = true;
            this.BtnClose.UseSelectable = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // CboMarginColumn
            // 
            this.CboMarginColumn.FormattingEnabled = true;
            this.CboMarginColumn.ItemHeight = 23;
            this.CboMarginColumn.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "AA",
            "AB",
            "AC",
            "AD",
            "AE",
            "AF",
            "AG",
            "AH",
            "AI",
            "AJ",
            "AK",
            "AL",
            "AM",
            "AN",
            "AO",
            "AP",
            "AQ",
            "AR",
            "AS",
            "AT",
            "AU",
            "AV",
            "AW",
            "AX",
            "AY",
            "AZ",
            "BA",
            "BB",
            "BC",
            "BD",
            "BE",
            "BF",
            "BG",
            "BH",
            "BI",
            "BJ",
            "BK",
            "BL",
            "BM",
            "BN",
            "BO",
            "BP",
            "BQ",
            "BR",
            "BS",
            "BT",
            "BU",
            "BV",
            "BW",
            "BX",
            "BY",
            "BZ"});
            this.CboMarginColumn.Location = new System.Drawing.Point(786, 66);
            this.CboMarginColumn.Name = "CboMarginColumn";
            this.CboMarginColumn.Size = new System.Drawing.Size(173, 29);
            this.CboMarginColumn.TabIndex = 153;
            this.CboMarginColumn.UseSelectable = true;
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(690, 70);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(98, 19);
            this.metroLabel4.TabIndex = 154;
            this.metroLabel4.Text = "상품 마진 행 : ";
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(239, 100);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(144, 19);
            this.metroLabel5.TabIndex = 155;
            this.metroLabel5.Text = "단위는 그램(g)입니다.";
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(508, 100);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(160, 19);
            this.metroLabel6.TabIndex = 156;
            this.metroLabel6.Text = "단위는 한국 원화입니다.";
            // 
            // metroLabel7
            // 
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.Location = new System.Drawing.Point(799, 100);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(160, 19);
            this.metroLabel7.TabIndex = 157;
            this.metroLabel7.Text = "단위는 한국 원화입니다.";
            // 
            // dgSrcItemList
            // 
            this.dgSrcItemList.AllowUserToAddRows = false;
            this.dgSrcItemList.AllowUserToDeleteRows = false;
            this.dgSrcItemList.BackgroundColor = System.Drawing.Color.White;
            this.dgSrcItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSrcItemList.Location = new System.Drawing.Point(14, 160);
            this.dgSrcItemList.Name = "dgSrcItemList";
            this.dgSrcItemList.ReadOnly = true;
            this.dgSrcItemList.RowTemplate.Height = 23;
            this.dgSrcItemList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgSrcItemList.Size = new System.Drawing.Size(1805, 766);
            this.dgSrcItemList.TabIndex = 152;
            // 
            // cboProductIdCol
            // 
            this.cboProductIdCol.FormattingEnabled = true;
            this.cboProductIdCol.ItemHeight = 23;
            this.cboProductIdCol.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "AA",
            "AB",
            "AC",
            "AD",
            "AE",
            "AF",
            "AG",
            "AH",
            "AI",
            "AJ",
            "AK",
            "AL",
            "AM",
            "AN",
            "AO",
            "AP",
            "AQ",
            "AR",
            "AS",
            "AT",
            "AU",
            "AV",
            "AW",
            "AX",
            "AY",
            "AZ",
            "BA",
            "BB",
            "BC",
            "BD",
            "BE",
            "BF",
            "BG",
            "BH",
            "BI",
            "BJ",
            "BK",
            "BL",
            "BM",
            "BN",
            "BO",
            "BP",
            "BQ",
            "BR",
            "BS",
            "BT",
            "BU",
            "BV",
            "BW",
            "BX",
            "BY",
            "BZ"});
            this.cboProductIdCol.Location = new System.Drawing.Point(210, 29);
            this.cboProductIdCol.Name = "cboProductIdCol";
            this.cboProductIdCol.Size = new System.Drawing.Size(173, 29);
            this.cboProductIdCol.TabIndex = 158;
            this.cboProductIdCol.UseSelectable = true;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(135, 34);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(78, 19);
            this.metroLabel2.TabIndex = 159;
            this.metroLabel2.Text = "상품ID 행 : ";
            // 
            // cboOptionIdCol
            // 
            this.cboOptionIdCol.FormattingEnabled = true;
            this.cboOptionIdCol.ItemHeight = 23;
            this.cboOptionIdCol.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "AA",
            "AB",
            "AC",
            "AD",
            "AE",
            "AF",
            "AG",
            "AH",
            "AI",
            "AJ",
            "AK",
            "AL",
            "AM",
            "AN",
            "AO",
            "AP",
            "AQ",
            "AR",
            "AS",
            "AT",
            "AU",
            "AV",
            "AW",
            "AX",
            "AY",
            "AZ",
            "BA",
            "BB",
            "BC",
            "BD",
            "BE",
            "BF",
            "BG",
            "BH",
            "BI",
            "BJ",
            "BK",
            "BL",
            "BM",
            "BN",
            "BO",
            "BP",
            "BQ",
            "BR",
            "BS",
            "BT",
            "BU",
            "BV",
            "BW",
            "BX",
            "BY",
            "BZ"});
            this.cboOptionIdCol.Location = new System.Drawing.Point(786, 29);
            this.cboOptionIdCol.Name = "cboOptionIdCol";
            this.cboOptionIdCol.Size = new System.Drawing.Size(173, 29);
            this.cboOptionIdCol.TabIndex = 160;
            this.cboOptionIdCol.UseSelectable = true;
            // 
            // metroLabel8
            // 
            this.metroLabel8.AutoSize = true;
            this.metroLabel8.Location = new System.Drawing.Point(710, 34);
            this.metroLabel8.Name = "metroLabel8";
            this.metroLabel8.Size = new System.Drawing.Size(78, 19);
            this.metroLabel8.TabIndex = 161;
            this.metroLabel8.Text = "옵션ID 행 : ";
            // 
            // chkUpdateSKU
            // 
            this.chkUpdateSKU.AutoSize = true;
            this.chkUpdateSKU.Location = new System.Drawing.Point(1709, 27);
            this.chkUpdateSKU.Name = "chkUpdateSKU";
            this.chkUpdateSKU.Size = new System.Drawing.Size(100, 16);
            this.chkUpdateSKU.TabIndex = 162;
            this.chkUpdateSKU.Text = "엑셀 SKU사용";
            this.chkUpdateSKU.UseVisualStyleBackColor = true;
            // 
            // cboProductSKUCol
            // 
            this.cboProductSKUCol.FormattingEnabled = true;
            this.cboProductSKUCol.ItemHeight = 23;
            this.cboProductSKUCol.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "AA",
            "AB",
            "AC",
            "AD",
            "AE",
            "AF",
            "AG",
            "AH",
            "AI",
            "AJ",
            "AK",
            "AL",
            "AM",
            "AN",
            "AO",
            "AP",
            "AQ",
            "AR",
            "AS",
            "AT",
            "AU",
            "AV",
            "AW",
            "AX",
            "AY",
            "AZ",
            "BA",
            "BB",
            "BC",
            "BD",
            "BE",
            "BF",
            "BG",
            "BH",
            "BI",
            "BJ",
            "BK",
            "BL",
            "BM",
            "BN",
            "BO",
            "BP",
            "BQ",
            "BR",
            "BS",
            "BT",
            "BU",
            "BV",
            "BW",
            "BX",
            "BY",
            "BZ"});
            this.cboProductSKUCol.Location = new System.Drawing.Point(495, 29);
            this.cboProductSKUCol.Name = "cboProductSKUCol";
            this.cboProductSKUCol.Size = new System.Drawing.Size(173, 29);
            this.cboProductSKUCol.TabIndex = 163;
            this.cboProductSKUCol.UseSelectable = true;
            // 
            // metroLabel9
            // 
            this.metroLabel9.AutoSize = true;
            this.metroLabel9.Location = new System.Drawing.Point(412, 34);
            this.metroLabel9.Name = "metroLabel9";
            this.metroLabel9.Size = new System.Drawing.Size(89, 19);
            this.metroLabel9.TabIndex = 164;
            this.metroLabel9.Text = "상품SKU 행 : ";
            // 
            // cboOptionSKUCol
            // 
            this.cboOptionSKUCol.FormattingEnabled = true;
            this.cboOptionSKUCol.ItemHeight = 23;
            this.cboOptionSKUCol.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "AA",
            "AB",
            "AC",
            "AD",
            "AE",
            "AF",
            "AG",
            "AH",
            "AI",
            "AJ",
            "AK",
            "AL",
            "AM",
            "AN",
            "AO",
            "AP",
            "AQ",
            "AR",
            "AS",
            "AT",
            "AU",
            "AV",
            "AW",
            "AX",
            "AY",
            "AZ",
            "BA",
            "BB",
            "BC",
            "BD",
            "BE",
            "BF",
            "BG",
            "BH",
            "BI",
            "BJ",
            "BK",
            "BL",
            "BM",
            "BN",
            "BO",
            "BP",
            "BQ",
            "BR",
            "BS",
            "BT",
            "BU",
            "BV",
            "BW",
            "BX",
            "BY",
            "BZ"});
            this.cboOptionSKUCol.Location = new System.Drawing.Point(1062, 29);
            this.cboOptionSKUCol.Name = "cboOptionSKUCol";
            this.cboOptionSKUCol.Size = new System.Drawing.Size(173, 29);
            this.cboOptionSKUCol.TabIndex = 165;
            this.cboOptionSKUCol.UseSelectable = true;
            // 
            // metroLabel10
            // 
            this.metroLabel10.AutoSize = true;
            this.metroLabel10.Location = new System.Drawing.Point(979, 34);
            this.metroLabel10.Name = "metroLabel10";
            this.metroLabel10.Size = new System.Drawing.Size(89, 19);
            this.metroLabel10.TabIndex = 166;
            this.metroLabel10.Text = "옵션SKU 행 : ";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.UdPayoneerFee);
            this.groupBox3.Controls.Add(this.metroLabel11);
            this.groupBox3.Controls.Add(this.UdRetailPriceRate);
            this.groupBox3.Controls.Add(this.LblRetailPriceRate);
            this.groupBox3.Controls.Add(this.udPGFee);
            this.groupBox3.Controls.Add(this.UdShopeeFee);
            this.groupBox3.Controls.Add(this.metroLabel12);
            this.groupBox3.Controls.Add(this.metroLabel13);
            this.groupBox3.Location = new System.Drawing.Point(1259, 18);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(212, 124);
            this.groupBox3.TabIndex = 167;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "판매가 계산";
            // 
            // UdPayoneerFee
            // 
            this.UdPayoneerFee.DecimalPlaces = 1;
            this.UdPayoneerFee.Enabled = false;
            this.UdPayoneerFee.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.UdPayoneerFee.Location = new System.Drawing.Point(143, 68);
            this.UdPayoneerFee.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.UdPayoneerFee.Name = "UdPayoneerFee";
            this.UdPayoneerFee.Size = new System.Drawing.Size(61, 21);
            this.UdPayoneerFee.TabIndex = 146;
            this.UdPayoneerFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UdPayoneerFee.ValueChanged += new System.EventHandler(this.UdPayoneerFee_ValueChanged);
            // 
            // metroLabel11
            // 
            this.metroLabel11.AutoSize = true;
            this.metroLabel11.Location = new System.Drawing.Point(14, 69);
            this.metroLabel11.Name = "metroLabel11";
            this.metroLabel11.Size = new System.Drawing.Size(128, 19);
            this.metroLabel11.TabIndex = 145;
            this.metroLabel11.Text = "Payoneer 수수료(%)";
            // 
            // UdRetailPriceRate
            // 
            this.UdRetailPriceRate.Location = new System.Drawing.Point(143, 93);
            this.UdRetailPriceRate.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.UdRetailPriceRate.Name = "UdRetailPriceRate";
            this.UdRetailPriceRate.Size = new System.Drawing.Size(61, 21);
            this.UdRetailPriceRate.TabIndex = 142;
            this.UdRetailPriceRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UdRetailPriceRate.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.UdRetailPriceRate.ValueChanged += new System.EventHandler(this.UdRetailPriceRate_ValueChanged);
            // 
            // LblRetailPriceRate
            // 
            this.LblRetailPriceRate.AutoSize = true;
            this.LblRetailPriceRate.Location = new System.Drawing.Point(26, 94);
            this.LblRetailPriceRate.Name = "LblRetailPriceRate";
            this.LblRetailPriceRate.Size = new System.Drawing.Size(116, 19);
            this.LblRetailPriceRate.TabIndex = 141;
            this.LblRetailPriceRate.Text = "소비자가 배율(%)";
            // 
            // udPGFee
            // 
            this.udPGFee.DecimalPlaces = 1;
            this.udPGFee.Location = new System.Drawing.Point(143, 43);
            this.udPGFee.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.udPGFee.Name = "udPGFee";
            this.udPGFee.Size = new System.Drawing.Size(61, 21);
            this.udPGFee.TabIndex = 136;
            this.udPGFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.udPGFee.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.udPGFee.ValueChanged += new System.EventHandler(this.udPGFee_ValueChanged);
            // 
            // UdShopeeFee
            // 
            this.UdShopeeFee.DecimalPlaces = 1;
            this.UdShopeeFee.Enabled = false;
            this.UdShopeeFee.Location = new System.Drawing.Point(143, 18);
            this.UdShopeeFee.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.UdShopeeFee.Name = "UdShopeeFee";
            this.UdShopeeFee.Size = new System.Drawing.Size(61, 21);
            this.UdShopeeFee.TabIndex = 139;
            this.UdShopeeFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UdShopeeFee.ValueChanged += new System.EventHandler(this.UdShopeeFee_ValueChanged);
            // 
            // metroLabel12
            // 
            this.metroLabel12.AutoSize = true;
            this.metroLabel12.Location = new System.Drawing.Point(44, 19);
            this.metroLabel12.Name = "metroLabel12";
            this.metroLabel12.Size = new System.Drawing.Size(98, 19);
            this.metroLabel12.TabIndex = 140;
            this.metroLabel12.Text = "쇼피수수료(%)";
            // 
            // metroLabel13
            // 
            this.metroLabel13.AutoSize = true;
            this.metroLabel13.Location = new System.Drawing.Point(55, 44);
            this.metroLabel13.Name = "metroLabel13";
            this.metroLabel13.Size = new System.Drawing.Size(87, 19);
            this.metroLabel13.TabIndex = 137;
            this.metroLabel13.Text = "PG수수료(%)";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.UdQty);
            this.groupBox4.Controls.Add(this.metroLabel14);
            this.groupBox4.Controls.Add(this.metroLabel19);
            this.groupBox4.Location = new System.Drawing.Point(1477, 18);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(141, 124);
            this.groupBox4.TabIndex = 168;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "빠른 입력";
            // 
            // UdQty
            // 
            this.UdQty.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.UdQty.Location = new System.Drawing.Point(42, 18);
            this.UdQty.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.UdQty.Name = "UdQty";
            this.UdQty.Size = new System.Drawing.Size(68, 21);
            this.UdQty.TabIndex = 126;
            this.UdQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UdQty.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // metroLabel14
            // 
            this.metroLabel14.AutoSize = true;
            this.metroLabel14.Location = new System.Drawing.Point(109, 19);
            this.metroLabel14.Name = "metroLabel14";
            this.metroLabel14.Size = new System.Drawing.Size(23, 19);
            this.metroLabel14.TabIndex = 149;
            this.metroLabel14.Text = "개";
            // 
            // metroLabel19
            // 
            this.metroLabel19.AutoSize = true;
            this.metroLabel19.Location = new System.Drawing.Point(8, 19);
            this.metroLabel19.Name = "metroLabel19";
            this.metroLabel19.Size = new System.Drawing.Size(37, 19);
            this.metroLabel19.TabIndex = 145;
            this.metroLabel19.Text = "수량";
            // 
            // FormUploadExcelPrice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1842, 928);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cboOptionSKUCol);
            this.Controls.Add(this.metroLabel10);
            this.Controls.Add(this.cboProductSKUCol);
            this.Controls.Add(this.metroLabel9);
            this.Controls.Add(this.chkUpdateSKU);
            this.Controls.Add(this.cboOptionIdCol);
            this.Controls.Add(this.metroLabel8);
            this.Controls.Add(this.cboProductIdCol);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel7);
            this.Controls.Add(this.metroLabel6);
            this.Controls.Add(this.metroLabel5);
            this.Controls.Add(this.CboMarginColumn);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.dgSrcItemList);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnSaveData);
            this.Controls.Add(this.CboNetPriceColumn);
            this.Controls.Add(this.cboWeightCol);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.BtnSelectExcelFile);
            this.Controls.Add(this.metroLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormUploadExcelPrice";
            this.Resizable = false;
            this.Load += new System.EventHandler(this.FormUploadExcelPrice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgSrcItemList)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UdPayoneerFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UdRetailPriceRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPGFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UdShopeeFee)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UdQty)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroComboBox CboNetPriceColumn;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroButton BtnSelectExcelFile;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroComboBox cboWeightCol;
        private MetroFramework.Controls.MetroButton BtnSaveData;
        private MetroFramework.Controls.MetroButton BtnClose;
        private MetroFramework.Controls.MetroComboBox CboMarginColumn;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        private System.Windows.Forms.DataGridView dgSrcItemList;
        private MetroFramework.Controls.MetroComboBox cboProductIdCol;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroComboBox cboOptionIdCol;
        private MetroFramework.Controls.MetroLabel metroLabel8;
        private System.Windows.Forms.CheckBox chkUpdateSKU;
        private MetroFramework.Controls.MetroComboBox cboProductSKUCol;
        private MetroFramework.Controls.MetroLabel metroLabel9;
        private MetroFramework.Controls.MetroComboBox cboOptionSKUCol;
        private MetroFramework.Controls.MetroLabel metroLabel10;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown UdPayoneerFee;
        private MetroFramework.Controls.MetroLabel metroLabel11;
        private System.Windows.Forms.NumericUpDown UdRetailPriceRate;
        private MetroFramework.Controls.MetroLabel LblRetailPriceRate;
        private System.Windows.Forms.NumericUpDown udPGFee;
        private System.Windows.Forms.NumericUpDown UdShopeeFee;
        private MetroFramework.Controls.MetroLabel metroLabel12;
        private MetroFramework.Controls.MetroLabel metroLabel13;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown UdQty;
        private MetroFramework.Controls.MetroLabel metroLabel14;
        private MetroFramework.Controls.MetroLabel metroLabel19;
    }
}