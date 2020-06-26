using MetroFramework.Forms;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormUploadExcelPrice : MetroForm
    {
        public string SrcCountry;
        public string TarCountry;
        public string SrcShopeeId;
        public string TarShopeeId;
        public decimal currencyRate;
        public DateTime currenctDate;
        public int ItemInfoDraftId;
        public FormUploadExcelPrice()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool rtn = false;
            if (!base.ProcessCmdKey(ref msg, keyData))
            {
                if (keyData.Equals(Keys.Escape))
                {
                    this.Close();
                    this.Dispose();
                }
            }
            else
            {
                rtn = false;
            }
            return rtn;
        }
        private void set_double_buffer()
        {
            System.Windows.Forms.Control[] controls = GetAllControlsUsingRecursive(this);
            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i].GetType() == typeof(DataGridView))
                {
                    ((DataGridView)controls[i]).DoubleBuffered(true);
                }
            }
        }

        static System.Windows.Forms.Control[] GetAllControlsUsingRecursive(System.Windows.Forms.Control containerControl)
        {
            List<System.Windows.Forms.Control> allControls = new List<System.Windows.Forms.Control>();
            Queue<System.Windows.Forms.Control.ControlCollection> queue = new Queue<System.Windows.Forms.Control.ControlCollection>();
            queue.Enqueue(containerControl.Controls);
            while (queue.Count > 0)
            {
                System.Windows.Forms.Control.ControlCollection controls = queue.Dequeue();
                if (controls == null || controls.Count == 0) continue;
                foreach (System.Windows.Forms.Control control in controls)
                {
                    allControls.Add(control);
                    queue.Enqueue(control.Controls);
                }
            }
            return allControls.ToArray();
        }
        
        private void FormUploadExcelPrice_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            cboProductIdCol.SelectedIndex = 0;
            cboProductSKUCol.SelectedIndex = 0;
            cboOptionIdCol.SelectedIndex = 0;
            cboOptionSKUCol.SelectedIndex = 0;
            cboWeightCol.SelectedIndex = 0;
            CboNetPriceColumn.SelectedIndex = 0;
            CboMarginColumn.SelectedIndex = 0;

            setDefaultVar();
        }
        private void setDefaultVar()
        {
            using (AppDbContext context = new AppDbContext())
            {
                ConfigVar result = context.ConfigVars.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result != null)
                {
                    UdShopeeFee.Value = result.shopee_fee;
                    udPGFee.Value = result.pg_fee;
                    UdPayoneerFee.Value = result.payoneer_fee;
                    UdRetailPriceRate.Value = result.retail_price_rate;
                    UdQty.Value = result.qty;
                }
            }
        }
        static int TextToNumber(string text)
        {
            int sum = 0;
            foreach (char c in text)
            {
                sum = sum * 26 + c - 'A' + 1;
            }
            return sum;
        }

        public DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                OfficeOpenXml.ExcelWorksheet ws = package.Workbook.Worksheets.First();                
                DataTable tbl = new DataTable();
                for (int i = 1; i < ws.Dimension.End.Column + 1; i++)
                {
                    if(ws.Cells[1, i].Value == null)
                    {
                        tbl.Columns.Add("컬럼" + i.ToString());
                    }
                    else
                    {
                        tbl.Columns.Add(ws.Cells[1, i].Value.ToString());
                    }
                    
                }

                for (int i = 2; i <= ws.Dimension.End.Row; i++)
                {
                    DataRow workRow;
                    workRow = tbl.NewRow();
                    for (int j = 0; j < ws.Dimension.End.Column; j++)
                    {
                        if(ws.Cells[i, j + 1].Value == null)
                        {
                            workRow[j] = "";
                        }
                        else
                        {
                            workRow[j] = ws.Cells[i, j + 1].Value.ToString();
                        }
                        
                    }
                    tbl.Rows.Add(workRow);
                }
                
                return tbl;
            }
        }
        private void BtnSelectExcelFile_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dgSrcItemList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgSrcItemList.DataSource = null;
            
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = "xlsx";
            openFileDlg.Filter = "엑셀 파일 (*.xlsx)|*.xlsx";
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                string file_name = openFileDlg.FileName;
                string ext = Path.GetExtension(file_name);

                if (ext.Contains("xlsx"))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    dgSrcItemList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    int productIdCol = TextToNumber(cboProductIdCol.Text);
                    int optionIdCol = TextToNumber(cboOptionIdCol.Text);
                    int productSKUCol = TextToNumber(cboProductSKUCol.Text);
                    int optionSKUCol = TextToNumber(cboOptionSKUCol.Text);
                    int weightCol = TextToNumber(cboWeightCol.Text);
                    int netPriceCol = TextToNumber(CboNetPriceColumn.Text);
                    int marginCol = TextToNumber(CboMarginColumn.Text);
                    var package = new ExcelPackage(new FileInfo(file_name));

                    DataTable dt_excel = GetDataTableFromExcel(file_name, true);

                    dgSrcItemList.DataSource = dt_excel;
                    //OfficeOpenXml.ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

                    var columns = new Dictionary<string, int>();

                    for (int i = 0; i < dt_excel.Columns.Count; i++)
                    {
                        columns[dt_excel.Columns[i].ToString()] = i;
                    }


                    cboProductIdCol.DataSource = new BindingSource(columns, null);
                    cboProductIdCol.DisplayMember = "key";
                    cboProductIdCol.ValueMember = "value";

                    cboOptionIdCol.DataSource = new BindingSource(columns, null);
                    cboOptionIdCol.DisplayMember = "key";
                    cboOptionIdCol.ValueMember = "value";

                    cboProductSKUCol.DataSource = new BindingSource(columns, null);
                    cboProductSKUCol.DisplayMember = "key";
                    cboProductSKUCol.ValueMember = "value";

                    cboOptionSKUCol.DataSource = new BindingSource(columns, null);
                    cboOptionSKUCol.DisplayMember = "key";
                    cboOptionSKUCol.ValueMember = "value";

                    cboWeightCol.DataSource = new BindingSource(columns, null);
                    cboWeightCol.DisplayMember = "key";
                    cboWeightCol.ValueMember = "value";

                    CboNetPriceColumn.DataSource = new BindingSource(columns, null);
                    CboNetPriceColumn.DisplayMember = "key";
                    CboNetPriceColumn.ValueMember = "value";

                    CboMarginColumn.DataSource = new BindingSource(columns, null);
                    CboMarginColumn.DisplayMember = "key";
                    CboMarginColumn.ValueMember = "value";


                    //DataTable dTable = new DataTable("price_data");
                    //DataColumn dc_product_id = new DataColumn("productID", typeof(string));
                    //DataColumn dc_product_sku = new DataColumn("productSKU", typeof(string));
                    //DataColumn dc_product_name = new DataColumn("productName", typeof(string));

                    //DataColumn dc_variation_id = new DataColumn("variationID", typeof(string));
                    //DataColumn dc_variation_sku = new DataColumn("variationSKU", typeof(string));
                    //DataColumn dc_variation_name = new DataColumn("variationName", typeof(string));

                    //DataColumn dc_product_wight = new DataColumn("productWeight", typeof(int));                    
                    //DataColumn dc_NetPrice = new DataColumn("NetPrice", typeof(decimal));
                    //DataColumn dc_RetailPrice = new DataColumn("RetailPrice", typeof(decimal));

                    //dTable.Columns.Add(dc_product_id);
                    //dTable.Columns.Add(dc_product_sku);
                    //dTable.Columns.Add(dc_product_name);

                    //dTable.Columns.Add(dc_variation_id);
                    //dTable.Columns.Add(dc_variation_sku);
                    //dTable.Columns.Add(dc_variation_name);

                    //dTable.Columns.Add(dc_product_wight);
                    //dTable.Columns.Add(dc_NetPrice);
                    //dTable.Columns.Add(dc_RetailPrice);

                    //DataRow dr;
                    //for (int i = workSheet.Dimension.Start.Row + 2; i <= workSheet.Dimension.End.Row; i++)
                    //{
                    //    dr = dTable.NewRow();
                    //    if (workSheet.Cells[i, 1].Value != null)
                    //    {
                    //        dr["productID"] = workSheet.Cells[i, 1].Value;
                    //    }

                    //    if (workSheet.Cells[i, 2].Value != null)
                    //    {
                    //        dr["productSKU"] = workSheet.Cells[i, 2].Value;
                    //    }

                    //    if (workSheet.Cells[i, 3].Value != null)
                    //    {
                    //        dr["productName"] = workSheet.Cells[i, 3].Value;
                    //    }

                    //    if (workSheet.Cells[i, 4].Value != null)
                    //    {
                    //        dr["variationID"] = workSheet.Cells[i, 4].Value;
                    //    }

                    //    if (workSheet.Cells[i, 5].Value != null)
                    //    {
                    //        dr["variationSKU"] = workSheet.Cells[i, 5].Value;
                    //    }

                    //    if (workSheet.Cells[i, 6].Value != null)
                    //    {
                    //        dr["variationName"] = workSheet.Cells[i, 6].Value;
                    //    }

                    //    if (workSheet.Cells[i, weightCol].Value != null)
                    //    {
                    //        dr["productWeight"] = workSheet.Cells[i, weightCol].Value;
                    //    }

                    //    if (workSheet.Cells[i, netPriceCol].Value != null)
                    //    {
                    //        dr["NetPrice"] = workSheet.Cells[i, netPriceCol].Value;
                    //    }

                    //    if (workSheet.Cells[i, retailCol].Value != null)
                    //    {
                    //        dr["RetailPrice"] = workSheet.Cells[i, retailCol].Value;
                    //    }

                    //    dTable.Rows.Add(dr);
                    //}

                    //for (int i = 0; i < dTable.Rows.Count; i++)
                    //{
                    //    string product_id = dTable.Rows[i]["productID"].ToString();
                    //    //만약에 Variation_id가 있는 경우는 DB에 넣어 주어야 하며 무게가 없을 경우 스킵한다.

                    //    string variation_id = dTable.Rows[i]["VariationID"].ToString().Trim();
                    //    int productWeight = 0;
                    //    if (dTable.Rows[i]["productWeight"] != null && dTable.Rows[i]["productWeight"].ToString() != string.Empty)
                    //    {
                    //        productWeight = (int)dTable.Rows[i]["productWeight"];
                    //    }

                    //    if (variation_id != string.Empty && productWeight > 0)
                    //    {
                    //        using (AppDbContext context = new AppDbContext())
                    //        {
                    //            ShopeeVariationPrice result = context.ShopeeVariationPrices.SingleOrDefault(
                    //                    b => b.SrcShopeeCountry == SrcCountry &&
                    //                    b.SrcShopeeId == SrcShopeeId &&
                    //                    b.TarShopeeCountry == TarCountry &&
                    //                    b.TarShopeeId == TarShopeeId &&
                    //                    b.variation_id == variation_id);

                    //            if (result != null)
                    //            {
                    //                result.product_weight = Convert.ToInt32(dTable.Rows[i][weightCol].ToString());
                    //                result.TarNetPrice = Convert.ToDecimal(dTable.Rows[i][netPriceCol].ToString());
                    //                result.TarRetail_price = Convert.ToDecimal(dTable.Rows[i][retailCol].ToString());
                    //                context.SaveChanges();
                    //            }
                    //            else
                    //            {
                    //                ShopeeVariationPrice newVariationPrice = new ShopeeVariationPrice
                    //                {
                    //                    SrcShopeeCountry = SrcCountry,
                    //                    SrcShopeeId = SrcShopeeId,
                    //                    TarShopeeCountry = TarCountry,
                    //                    TarShopeeId = TarShopeeId,
                    //                    productId = dTable.Rows[i]["productID"].ToString(),
                    //                    productSKU = dTable.Rows[i]["productSKU"].ToString(),
                    //                    productName = dTable.Rows[i]["productName"].ToString(),
                    //                    variation_id = dTable.Rows[i]["variationID"].ToString(),
                    //                    variation_sku = dTable.Rows[i]["variationSKU"].ToString(),                                        
                    //                    variation_name = dTable.Rows[i]["variationName"].ToString(),
                    //                    TarRetail_price = Convert.ToDecimal(dTable.Rows[i][retailCol].ToString()),
                    //                    TarNetPrice = Convert.ToDecimal(dTable.Rows[i][netPriceCol].ToString())
                    //                };
                    //            }
                    //        }
                    //    }
                    //}
                }

                dgSrcItemList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                Cursor.Current = Cursors.Default;
                MessageBox.Show("데이터를 모두 입력하였습니다.", "데이터 입력 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSaveData_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("상품 가격 정보를 저장 하시겠습니까?\r\n상품 무게가 있는 자료만 저장됩니다.", "상품 가격 정보 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;

                int productIdCol = (int)cboProductIdCol.SelectedValue;
                int optionIdCol = (int)cboOptionIdCol.SelectedValue;
                int productSKUCol = (int)cboProductSKUCol.SelectedValue;
                int optionSKUCol = (int)cboOptionSKUCol.SelectedValue;
                int productWeightCol = (int)cboWeightCol.SelectedValue;
                int netPriceCol = (int)CboNetPriceColumn.SelectedValue;
                int marginCol = (int)CboMarginColumn.SelectedValue;

                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {   
                    if(dgSrcItemList.Rows[i].Cells[productIdCol].Value.ToString().Trim() != string.Empty)
                    {
                        long productID = Convert.ToInt64(dgSrcItemList.Rows[i].Cells[productIdCol].Value.ToString().Trim());

                        long variationID = 0;
                        if (dgSrcItemList.Rows[i].Cells[optionIdCol].Value.ToString().Trim() != string.Empty)
                        {
                            long.TryParse(dgSrcItemList.Rows[i].Cells[optionIdCol].Value.ToString().Trim(), out variationID);
                        }


                        int productWeight = 0;
                        decimal netPrice = 0;
                        decimal margin = 0;

                        if (dgSrcItemList.Rows[i].Cells[productWeightCol] != null
                            && dgSrcItemList.Rows[i].Cells[productWeightCol].Value.ToString().Trim() != string.Empty)
                        {
                            int.TryParse(dgSrcItemList.Rows[i].Cells[productWeightCol].Value.ToString(), out productWeight);
                        }

                        if (dgSrcItemList.Rows[i].Cells[netPriceCol] != null
                            && dgSrcItemList.Rows[i].Cells[netPriceCol].Value.ToString().Trim() != string.Empty)
                        {
                            decimal.TryParse(dgSrcItemList.Rows[i].Cells[netPriceCol].Value.ToString(), out netPrice);
                        }

                        if (dgSrcItemList.Rows[i].Cells[marginCol] != null
                            && dgSrcItemList.Rows[i].Cells[marginCol].Value.ToString().Trim() != string.Empty)
                        {
                            decimal.TryParse(dgSrcItemList.Rows[i].Cells[marginCol].Value.ToString(), out margin);
                        }

                        string productSKU = "";
                        if (dgSrcItemList.Rows[i].Cells[productSKUCol] != null
                            && dgSrcItemList.Rows[i].Cells[productSKUCol].Value.ToString().Trim() != string.Empty)
                        {
                            productSKU = dgSrcItemList.Rows[i].Cells[productSKUCol].Value.ToString().Trim();
                        }

                        string optionSKU = "";
                        if (dgSrcItemList.Rows[i].Cells[optionSKUCol] != null
                            && dgSrcItemList.Rows[i].Cells[optionSKUCol].Value.ToString().Trim() != string.Empty)
                        {
                            optionSKU = dgSrcItemList.Rows[i].Cells[optionSKUCol].Value.ToString().Trim();
                        }

                        //데이터를 넣을때 판가를 계산하여 넣도록 한다.

                        PriceCalculator pCalc = new PriceCalculator();
                        pCalc.CountryCode = TarCountry;
                        pCalc.SourcePrice = netPrice;
                        pCalc.Margin = margin;
                        pCalc.Weight = productWeight;
                        pCalc.CurrencyRate = currencyRate;
                        pCalc.ShopeeRate = UdShopeeFee.Value;
                        pCalc.PayoneerRate = UdPayoneerFee.Value;
                        pCalc.PgFeeRate = udPGFee.Value;
                        pCalc.RetailPriceRate = UdRetailPriceRate.Value;

                        Dictionary<string, decimal> calcResult = new Dictionary<string, decimal>();
                        calcResult = pCalc.calculatePrice();

                        if (variationID == 0)
                        {
                            //부모상품인 경우
                            using (AppDbContext context = new AppDbContext())
                            {
                                ItemInfoDraft result = context.ItemInfoDrafts.SingleOrDefault(
                                        b => b.src_item_id == productID &&
                                        b.tar_shopeeAccount == TarShopeeId
                                        && b.UserId == global_var.userId);

                                if (result != null)
                                {
                                    result.weight = productWeight;
                                    result.supply_price = netPrice;
                                    result.margin = margin;
                                    result.pgFee = calcResult["pgFee"];
                                    result.price = calcResult["targetSellPrice"];
                                    result.original_price = calcResult["targetRetailPrice"];
                                    result.targetSellPriceKRW = (int)calcResult["targetSellPriceKRW"];
                                    result.currencyRate = calcResult["currencyRate"];
                                    result.currencyDate = currenctDate;
                                    result.stock = (int)UdQty.Value;
                                    context.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            //부모와 옵션의 데이터를 모두 업데이트
                            using (AppDbContext context = new AppDbContext())
                            {
                                ItemInfoDraft resultParent = context.ItemInfoDrafts.SingleOrDefault(
                                        b => b.src_item_id == productID &&
                                        b.tar_shopeeAccount == TarShopeeId
                                        && b.UserId == global_var.userId);

                                if (resultParent != null)
                                {
                                    if (chkUpdateSKU.Checked)
                                    {
                                        resultParent.item_sku = productSKU;
                                    }

                                    resultParent.weight = productWeight;
                                    resultParent.supply_price = netPrice;
                                    resultParent.margin = margin;
                                    resultParent.pgFee = calcResult["pgFee"];
                                    resultParent.price = calcResult["targetSellPrice"];
                                    resultParent.original_price = calcResult["targetRetailPrice"];
                                    resultParent.targetSellPriceKRW = (int)calcResult["targetSellPriceKRW"];
                                    resultParent.currencyRate = calcResult["currencyRate"];
                                    resultParent.currencyDate = currenctDate;
                                    resultParent.stock = (int)UdQty.Value;
                                    context.SaveChanges();
                                }

                                ItemVariationDraft result = context.ItemVariationDrafts.SingleOrDefault(
                                        b => b.src_item_id == productID &&
                                        b.variation_id == variationID &&
                                        b.tar_shopeeAccount == TarShopeeId
                                        && b.UserId == global_var.userId);

                                if (result != null)
                                {
                                    if (chkUpdateSKU.Checked)
                                    {
                                        result.variation_sku = optionSKU;
                                    }
                                    result.weight = productWeight;
                                    result.supply_price = netPrice;
                                    result.margin = margin;
                                    result.pgFee = calcResult["pgFee"];
                                    result.price = calcResult["targetSellPrice"];
                                    result.original_price = calcResult["targetRetailPrice"];
                                    result.targetSellPriceKRW = (int)calcResult["targetSellPriceKRW"];
                                    result.currencyRate = calcResult["currencyRate"];
                                    result.currencyDate = currenctDate;
                                    result.stock = (int)UdQty.Value;
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("데이터를 저장하였습니다.","데이터 저장 완료",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UdShopeeFee_ValueChanged(object sender, EventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                ConfigVar result = context.ConfigVars.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result == null)
                {
                    ConfigVar newVar = new ConfigVar
                    {
                        shopee_fee = UdShopeeFee.Value,
                        UserId = global_var.userId
                    };
                    context.ConfigVars.Add(newVar);
                    context.SaveChanges();
                }
                else
                {
                    result.shopee_fee = UdShopeeFee.Value;
                    context.SaveChanges();
                }
            }
        }

        private void udPGFee_ValueChanged(object sender, EventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                ConfigVar result = context.ConfigVars.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result == null)
                {
                    ConfigVar newVar = new ConfigVar
                    {
                        pg_fee = udPGFee.Value,
                        UserId = global_var.userId
                    };
                    context.ConfigVars.Add(newVar);
                    context.SaveChanges();
                }
                else
                {
                    result.pg_fee = udPGFee.Value;
                    context.SaveChanges();
                }
            }
        }

        private void UdPayoneerFee_ValueChanged(object sender, EventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                ConfigVar result = context.ConfigVars.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result == null)
                {
                    ConfigVar newVar = new ConfigVar
                    {
                        payoneer_fee = UdPayoneerFee.Value,
                        UserId = global_var.userId
                    };
                    context.ConfigVars.Add(newVar);
                    context.SaveChanges();
                }
                else
                {
                    result.payoneer_fee = UdPayoneerFee.Value;
                    context.SaveChanges();
                }
            }
        }

        private void UdRetailPriceRate_ValueChanged(object sender, EventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                ConfigVar result = context.ConfigVars.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result == null)
                {
                    ConfigVar newVar = new ConfigVar
                    {
                        retail_price_rate = UdRetailPriceRate.Value,
                        UserId = global_var.userId
                    };
                    context.ConfigVars.Add(newVar);
                    context.SaveChanges();
                }
                else
                {
                    result.retail_price_rate = UdRetailPriceRate.Value;
                    context.SaveChanges();
                }
            }
        }
    }
}
