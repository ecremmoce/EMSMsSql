using MetroFramework.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormSetVariationDraft : MetroForm
    {
        public FormSetVariationDraft()
        {
            InitializeComponent();
        }
        public long ItemId;
        public long partner_id;
        public long shop_id;
        public string api_key;
        public string country;
        public string tarCountry;
        public string tarShopeeId;
        public string srcShopeeId;
        public int ItemInfoDraftId;
        
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
        private void Fill_Currency_Date()
        {
            using (AppDbContext context = new AppDbContext())
            {
                CurrencyRate currencyList = context.CurrencyRates.FirstOrDefault(x => x.UserId == global_var.userId);
                if(currencyList != null)
                {
                    lbl_currency_date.Text = currencyList.cr_save_date.ToString();
                }
            }
        }
        private void Fill_from_Currency_Names()
        {
            cls_currency cc = new cls_currency();
            Dictionary<string, decimal> dic_currency = new Dictionary<string, decimal>();
            dic_currency = cc.get_currency();
            if(dic_currency.Count > 0)
            {
                cbo_currency_From.DataSource = new BindingSource(dic_currency, null);
                cbo_currency_From.DisplayMember = "Key";
                cbo_currency_From.ValueMember = "Value";
            }
            
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
        private void setDefaultVar()
        {
            using (AppDbContext context = new AppDbContext())
            {
                ConfigVar result = context.ConfigVars.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result != null)
                {
                    UdShopeeFee.Value = result.shopee_fee;
                    udPGFee.Value = result.pg_fee;
                    UdRetailPriceRate.Value = result.retail_price_rate;
                }
            }
        }
        public FormUploader fp;
        private void FormSetVariation_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            Cursor.Current = Cursors.WaitCursor;
            setDefaultVar();
            Fill_Currency_Date();
            Fill_from_Currency_Names();
            string countryCurrencyCode = "";
            if (tarCountry == "ID")
            {
                countryCurrencyCode = "IDR";
            }
            else if(tarCountry == "SG")
            {
                countryCurrencyCode = "SGD";
            }
            else if (tarCountry == "MY")
            {
                countryCurrencyCode = "MYR";
            }
            else if (tarCountry == "TH")
            {
                countryCurrencyCode = "THB";
            }
            else if (tarCountry == "TW")
            {
                countryCurrencyCode = "TWD";
            }
            else if (tarCountry == "PH")
            {
                countryCurrencyCode = "PHP";
            }
            else if (tarCountry == "VN")
            {
                countryCurrencyCode = "VND";
            }
            

            for (int i = 0; i < cbo_currency_From.Items.Count; i++)
            {
                KeyValuePair<string, decimal> currency_info = (KeyValuePair<string, decimal>)cbo_currency_From.Items[i];
                if (currency_info.Key.ToString().Contains(countryCurrencyCode))
                {
                    cbo_currency_From.SelectedIndex = i;
                    //txt_tar_currency_rate.Text = currency_info.Value.ToString();
                    txt_src_currency_rate.Text = currency_info.Value.ToString();
                    break;
                }
            }
            
            getVariation();

            Cursor.Current = Cursors.Default;
        }
        private void getVariation()
        {
            using (AppDbContext context = new AppDbContext())
            {   
                List<ItemVariationDraft> variationList = context.ItemVariationDrafts
                                .Where(b => b.ItemInfoDraftId == ItemInfoDraftId
                                && b.UserId == global_var.userId)
                                .OrderBy(x => x.variation_id).ToList();

                string currencyDigit = "";
                string pgDigit = "";
                if (variationList.Count > 0)
                {
                    if (tarCountry == "ID")
                    {
                        pgDigit = "{0:n1}";
                        currencyDigit = "{0:n0}";
                    }
                    else if (tarCountry == "MY")
                    {
                        pgDigit = "{0:n1}";
                        currencyDigit = "{0:n0}";
                    }
                    else if (tarCountry == "SG")
                    {
                        pgDigit = "{0:n2}";
                        currencyDigit = "{0:n1}";
                    }
                    else if (tarCountry == "TW")
                    {
                        pgDigit = "{0:n1}";
                        currencyDigit = "{0:n0}";
                    }
                    else if (tarCountry == "TH")
                    {
                        pgDigit = "{0:n1}";
                        currencyDigit = "{0:n0}";                     
                    }
                    else if (tarCountry == "PH")
                    {
                        pgDigit = "{0:n1}";
                        currencyDigit = "{0:n0}";
                    }


                    for (int i = 0; i < variationList.Count; i++)
                    {
                        DgSrcVariation.Rows.Add(i + 1,
                            false,
                            variationList[i].status,
                            variationList[i].variation_id,
                            variationList[i].variation_sku,
                            variationList[i].name,
                            string.Format("{0:n0}", variationList[i].supply_price),
                            string.Format("{0:n0}", variationList[i].margin),
                            string.Format(pgDigit, variationList[i].pgFee),
                            string.Format("{0:n0}", variationList[i].targetSellPriceKRW),
                            string.Format(currencyDigit, variationList[i].price),
                            string.Format(currencyDigit, variationList[i].original_price),
                            string.Format("{0:n0}", variationList[i].stock),
                            string.Format("{0:n2}", variationList[i].weight),
                            variationList[i].create_time,
                            variationList[i].update_time,
                            variationList[i].discount_id);

                        //입력한 뒤에 상품원가, 마진, 무게가 있는 경우에는 자동계산하여 값을 넣어준다.
                        if(variationList[i].supply_price > 0 &&
                            variationList[i].margin > 0 &&
                            variationList[i].weight > 0)
                        {
                            //판가가 계산이 안된 경우
                            if(variationList[i].targetSellPriceKRW == 0)
                            {
                                calcRowPrice(DgSrcVariation.Rows.Count - 1, "DgSrcVariation_supply_price");
                            }
                            else
                            {
                                if(variationList[i].supply_price > 0)
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_supply_price"].Style.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_supply_price"].Style.BackColor = Color.Orange;
                                }

                                if (variationList[i].margin > 0)
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_margin"].Style.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_margin"].Style.BackColor = Color.Orange;
                                }

                                if (variationList[i].pgFee > 0)
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_pg_fee"].Style.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_pg_fee"].Style.BackColor = Color.Orange;
                                }

                                if (variationList[i].targetSellPriceKRW > 0)
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_price_won"].Style.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_price_won"].Style.BackColor = Color.Orange;
                                }

                                if (variationList[i].price > 0)
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_src_price"].Style.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_src_price"].Style.BackColor = Color.Orange;
                                }

                                if (variationList[i].original_price > 0)
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_src_original_price"].Style.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_src_original_price"].Style.BackColor = Color.Orange;
                                }

                                if (variationList[i].stock > 0)
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_stock"].Style.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_stock"].Style.BackColor = Color.Orange;
                                }

                                if (variationList[i].weight > 0)
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_weight"].Style.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    DgSrcVariation.Rows[DgSrcVariation.Rows.Count - 1].Cells["DgSrcVariation_weight"].Style.BackColor = Color.Orange;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btn_View_Product_Click(object sender, EventArgs e)
        {
            string siteUrl = "";

            if (country == "SG")
            {
                siteUrl = "https://shopee.sg/product/" + shop_id + "/" + ItemId;
            }
            else if (country == "MY")
            {
                siteUrl = "https://shopee.com.my/product/" + shop_id + "/" + ItemId;
            }
            else if (country == "ID")
            {
                siteUrl = "https://shopee.co.id/product/" + shop_id + "/" + ItemId;
            }
            else if (country == "TH")
            {
                siteUrl = "https://shopee.co.th/product/" + shop_id + "/" + ItemId;
            }
            else if (country == "TW")
            {
                siteUrl = "https://shopee.tw/product/" + shop_id + "/" + ItemId;
            }
            else if (country == "PH")
            {
                siteUrl = "https://shopee.ph/product/" + shop_id + "/" + ItemId;
            }
            else if (country == "VN")
            {
                siteUrl = "https://shopee.vn/product/" + shop_id + "/" + ItemId;
            }
            System.Diagnostics.Process.Start("chrome.exe", siteUrl);
        }

        private void btn_save_data_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("옵션값을 저장 하시겠습니까?", "속성값 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
                {
                    long variationId = Convert.ToInt64(DgSrcVariation.Rows[i].Cells["DgSrcVariation_variation_id"].Value.ToString());
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemVariationDraft result = context.ItemVariationDrafts.SingleOrDefault(
                                b => b.src_item_id == ItemId &&
                                b.variation_id == variationId &&
                                b.tar_shopeeAccount == tarShopeeId
                                && b.UserId == global_var.userId);
                        
                        if (result != null)
                        {
                            if(DgSrcVariation.Rows[i].Cells["DgSrcVariation_supply_price"].Value != null && DgSrcVariation.Rows[i].Cells["DgSrcVariation_supply_price"].Value.ToString() != string.Empty)
                            {
                                decimal temp = 0;
                                if(decimal.TryParse(DgSrcVariation.Rows[i].Cells["DgSrcVariation_supply_price"].Value.ToString().Trim().Replace(",", ""), out temp))
                                {
                                    result.supply_price = temp;
                                }
                                
                            }

                            if(DgSrcVariation.Rows[i].Cells["DgSrcVariation_margin"].Value != null && DgSrcVariation.Rows[i].Cells["DgSrcVariation_margin"].Value.ToString() != string.Empty)
                            {
                                decimal temp = 0;
                                if (decimal.TryParse(DgSrcVariation.Rows[i].Cells["DgSrcVariation_margin"].Value.ToString().Trim().Replace(",", ""), out temp))
                                {
                                    result.margin = temp;
                                }                                
                            }

                            if (DgSrcVariation.Rows[i].Cells["DgSrcVariation_weight"].Value != null && DgSrcVariation.Rows[i].Cells["DgSrcVariation_weight"].Value.ToString() != string.Empty)
                            {
                                int temp = 0;
                                if (int.TryParse(DgSrcVariation.Rows[i].Cells["DgSrcVariation_weight"].Value.ToString().Trim().Replace(",", ""), out temp))
                                {
                                    result.weight = temp;
                                }
                            }

                            
                            result.variation_sku = DgSrcVariation.Rows[i].Cells["DgSrcVariation_variation_sku"].Value.ToString().Trim();
                            result.name = DgSrcVariation.Rows[i].Cells["DgSrcVariation_Name"].Value.ToString().Trim();

                            if (DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_price"].Value != null && DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_price"].Value.ToString() != string.Empty)
                            {
                                decimal temp = 0;
                                if (decimal.TryParse(DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_price"].Value.ToString().Trim().Replace(",", ""), out temp))
                                {
                                    result.price = temp;
                                }
                            }

                            if (DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_original_price"].Value != null && DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_original_price"].Value.ToString() != string.Empty)
                            {
                                decimal temp = 0;
                                if (decimal.TryParse(DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_original_price"].Value.ToString().Trim().Replace(",", ""), out temp))
                                {
                                    result.original_price = temp;
                                }
                            }

                            if (DgSrcVariation.Rows[i].Cells["DgSrcVariation_stock"].Value != null && DgSrcVariation.Rows[i].Cells["DgSrcVariation_stock"].Value.ToString() != string.Empty)
                            {
                                int temp = 0;
                                if (int.TryParse(DgSrcVariation.Rows[i].Cells["DgSrcVariation_stock"].Value.ToString().Trim().Replace(",", ""), out temp))
                                {
                                    result.stock = temp;
                                }
                            }

                            context.SaveChanges();
                        }
                    }
                }

                using (AppDbContext context = new AppDbContext())
                {
                    ItemInfoDraft result = context.ItemInfoDrafts.SingleOrDefault(
                            b => b.src_item_id == ItemId &&
                            b.tar_shopeeAccount == tarShopeeId
                            && b.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.isChanged = true;
                        context.SaveChanges();
                        fp.isChanged = true;
                    }
                }
                MessageBox.Show("옵션값을 업데이트 하였습니다.", "옵션값 업데이트", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public bool isChanged;
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        

        private void calcRowPrice(int rowId, string cellName)
        {
            string currencyDigit = "";
            string pgDigit = "";

            if (country == "ID")
            {
                pgDigit = "{0:n2}";
                currencyDigit = "{0:n0}";
            }
            else if (country == "MY")
            {
                pgDigit = "{0:n2}";
                currencyDigit = "{0:n0}";
            }
            else if (country == "SG")
            {
                pgDigit = "{0:n2}";
                currencyDigit = "{0:n1}";
            }
            else if (country == "TW")
            {
                pgDigit = "{0:n2}";
                currencyDigit = "{0:n0}";
            }
            else if (country == "TH")
            {
                pgDigit = "{0:n2}";
                currencyDigit = "{0:n0}";
            }
            else if (country == "PH")
            {
                pgDigit = "{0:n2}";
                currencyDigit = "{0:n0}";
            }

            decimal sourcePrice = Convert.ToDecimal(DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_supply_price"].Value.ToString().Replace(",", ""));
            double tempWeight = Convert.ToDouble(DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Value.ToString().Replace(",", ""));
            int productWeight = (int)(tempWeight * 1000);

            long variationId = Convert.ToInt64(DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_variation_id"].Value.ToString().Replace(",", ""));
            
            decimal margin = Convert.ToDecimal(DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Value.ToString().Replace(",", ""));

            if (margin >= 0)
            {
                DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Style.BackColor = Color.GreenYellow;
            }
            else
            {
                DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Style.BackColor = Color.Orange;
            }

            if (productWeight > 0)
            {
                DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Style.BackColor = Color.GreenYellow;
            }
            else
            {
                DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Style.BackColor = Color.Orange;
            }

            string countryCode = tarCountry;

            //판매국가의 환율
            decimal rateSrc = txt_src_currency_rate.Value;
            if (sourcePrice > 0 && margin >= 0 && productWeight > 0)
            {
                PriceCalculator pCalc = new PriceCalculator();
                pCalc.CountryCode = tarCountry;
                pCalc.SourcePrice = sourcePrice;
                pCalc.Margin = margin;
                pCalc.Weight = productWeight;
                pCalc.CurrencyRate = txt_src_currency_rate.Value;
                pCalc.ShopeeRate = UdShopeeFee.Value;
                pCalc.PgFeeRate = udPGFee.Value;
                pCalc.RetailPriceRate = UdRetailPriceRate.Value;

                Dictionary<string, decimal> calcResult = new Dictionary<string, decimal>();
                calcResult = pCalc.calculatePrice();

                DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_pg_fee"].Value = string.Format(pgDigit, calcResult["pgFee"]);
                DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_src_price"].Value = string.Format(currencyDigit, calcResult["targetSellPrice"]);
                DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_price_won"].Value = string.Format("{0:n0}", calcResult["targetSellPriceKRW"]);
                DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_src_original_price"].Value = string.Format(currencyDigit, calcResult["targetRetailPrice"]);


                //저장해 준다.
                using (AppDbContext context = new AppDbContext())
                {
                    ItemInfoDraft result = context.ItemInfoDrafts.SingleOrDefault(
                            b => b.Id == ItemInfoDraftId
                            && b.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.weight = tempWeight;
                        result.supply_price = sourcePrice;
                        result.margin = margin;
                        result.pgFee = calcResult["pgFee"];
                        result.price = calcResult["targetSellPrice"];
                        result.original_price = calcResult["targetRetailPrice"];
                        result.targetSellPriceKRW = (int)calcResult["targetSellPriceKRW"];
                        result.currencyRate = calcResult["currencyRate"];

                        DateTime dt = new DateTime();
                        if (!DateTime.TryParse(lbl_currency_date.Text, out dt))
                        {
                            dt = DateTime.Now;
                        }

                        result.currencyDate = dt;
                        context.SaveChanges();
                    }


                    ItemVariationDraft variation = context.ItemVariationDrafts.FirstOrDefault(
                        b => b.ItemInfoDraftId == ItemInfoDraftId &&
                                b.variation_id == variationId
                                && b.UserId == global_var.userId);
                    if(variation != null)
                    {
                        variation.weight = tempWeight;
                        variation.supply_price = sourcePrice;
                        variation.margin = margin;
                        variation.pgFee = calcResult["pgFee"];
                        variation.price = calcResult["targetSellPrice"];
                        variation.original_price = calcResult["targetRetailPrice"];
                        variation.targetSellPriceKRW = (int)calcResult["targetSellPriceKRW"];
                        variation.currencyRate = calcResult["currencyRate"];
                        DateTime dt = new DateTime();
                        if (!DateTime.TryParse(lbl_currency_date.Text, out dt))
                        {
                            dt = DateTime.Now;
                        }

                        result.currencyDate = dt;
                        context.SaveChanges();
                    }
                                
                }

                if (calcResult["pgFee"] > 0)
                {
                    DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_pg_fee"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_pg_fee"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetSellPrice"] > 0)
                {
                    DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_src_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_src_price"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetSellPriceKRW"] > 0)
                {
                    DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_price_won"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_price_won"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetRetailPrice"] > 0)
                {
                    DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_src_original_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_src_original_price"].Style.BackColor = Color.Orange;
                }

                //마지막으로 입력한 값이 숫자니까 콤마 찍어주기
                string strVar = DgSrcVariation.Rows[rowId].Cells[cellName].Value.ToString().Replace(",", "");
                decimal Var = 0;
                
                if (decimal.TryParse(strVar, out Var))
                {
                    if (cellName == "DgSrcVariation_weight")
                    {
                        DgSrcVariation.Rows[rowId].Cells[cellName].Value = string.Format("{0:n2}", Var);
                    }
                    else
                    {
                        DgSrcVariation.Rows[rowId].Cells[cellName].Value = string.Format("{0:n0}", Var);
                    }
                    
                }

                if (sourcePrice > 0)
                {
                    DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_supply_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_supply_price"].Style.BackColor = Color.Orange;
                }

                if (margin > 0)
                {
                    DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Style.BackColor = Color.Orange;
                }

            }
            
        }
        private void viewNaverExchange_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string siteUrl = "https://finance.naver.com/marketindex/?tabSel=exchange#tab_section";
            System.Diagnostics.Process.Start("chrome.exe", siteUrl);
            Cursor.Current = Cursors.Default;
        }

        private void BtnUpdateCurrencyRate_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("네이버 매매기준율 기준으로 환율을 업데이트 하시겠습니까?", "환율 업데이트", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                CookieAwareWebClient web_exchange = new CookieAwareWebClient();
                string purl = "https://finance.naver.com/marketindex/exchangeList.nhn";
                web_exchange.Encoding = Encoding.UTF8;
                //Stream docStream = cclient.OpenRead(purl);
                byte[] docBytes = web_exchange.DownloadData(purl);
                string encodeType = web_exchange.ResponseHeaders["Content-Type"];
                string charsetKey = "charset";
                int pos = encodeType.IndexOf(charsetKey);
                Encoding currentEncoding = Encoding.Default;
                if (pos != -1)
                {
                    pos = encodeType.IndexOf("=", pos + charsetKey.Length);
                    if (pos != -1)
                    {
                        string charset = encodeType.Substring(pos + 1);
                        currentEncoding = Encoding.GetEncoding(charset);
                    }
                }
                string doc2 = currentEncoding.GetString(docBytes);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(doc2);
                string update_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                HtmlAgilityPack.HtmlNodeCollection nd_cr_title = doc.DocumentNode.SelectNodes("//td[@class='tit']/a");
                HtmlAgilityPack.HtmlNodeCollection nd_cr_rate = doc.DocumentNode.SelectNodes("//td[@class='sale']");

                ArrayList ar_currency_data = new ArrayList();
                for (int i = 0; i < nd_cr_title.Count; i++)
                {
                    string cr_name = nd_cr_title[i].InnerText.Trim();
                    cr_name = cr_name.Replace(" (100엔)", string.Empty);
                    cr_name = cr_name.Replace(" 100", string.Empty);
                    cr_name = cr_name.Replace("남아프리카 공화국 ZAR", "남아프리카공화국 ZAR");

                    int code_start = cr_name.IndexOf(' ') + 1;
                    string cr_code = cr_name.Substring(code_start);

                    string cr_exchange = nd_cr_rate[i].InnerText.Trim().Replace(",", string.Empty);
                    string[] row_data = { cr_code, cr_name, cr_exchange, update_date };
                    ar_currency_data.Add(row_data);
                }

                cls_currency cc = new cls_currency();
                cc.update_currency(ar_currency_data);
                lbl_currency_date.Text = update_date + " 기준";

                Fill_Currency_Date();
                Fill_from_Currency_Names();


                for (int i = 0; i < cbo_currency_From.Items.Count; i++)
                {
                    KeyValuePair<string, decimal> currency_info = (KeyValuePair<string, decimal>)cbo_currency_From.Items[i];
                    if (currency_info.Key.ToString().Contains(tarCountry))
                    {
                        cbo_currency_From.SelectedIndex = i;
                        //txt_tar_currency_rate.Text = currency_info.Value.ToString();
                        txt_src_currency_rate.Text = currency_info.Value.ToString();
                        break;
                    }
                }

                Cursor.Current = Cursors.Default;
                MessageBox.Show("환율 정보를 업데이트 하였습니다.", "환율정보 업데이트", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DgSrcVariation_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //BtnCalcSellPrice_Click(null, null);
        }

        private void BtnApplyQty_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
            {
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_stock"].Value = string.Format("{0:n0}", UdQty.Value);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveMargin_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
            {
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_margin"].Value = string.Format("{0:n0}", UdMargin.Value);
            }
            Cursor.Current = Cursors.Default;
        }

        private void DgSrcVariation_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (DgSrcVariation.Rows.Count > 0)
            {
                long variationId = Convert.ToInt64(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_variation_id"].Value.ToString());
                if (e.ColumnIndex == 4)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemVariationDraft result = context.ItemVariationDrafts.SingleOrDefault(
                                b => b.variation_id == variationId &&
                                b.ItemInfoDraftId == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            string sku = DgSrcVariation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim();
                            result.variation_sku = sku;
                            context.SaveChanges();
                        }
                    }
                }
                else if(e.ColumnIndex == 5)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemVariationDraft result = context.ItemVariationDrafts.SingleOrDefault(
                                b => b.variation_id == variationId &&
                                b.ItemInfoDraftId == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            string name = DgSrcVariation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim();
                            result.name = name;
                            context.SaveChanges();
                        }
                    }
                }
                else if ((e.ColumnIndex == 6 || e.ColumnIndex == 7 || e.ColumnIndex == 13 || e.ColumnIndex == 12) && e.RowIndex > -1)
                {
                    calcRowPrice(e.RowIndex, DgSrcVariation.Columns[e.ColumnIndex].Name);

                    if (e.ColumnIndex == 13)
                    {
                        //수량을 입력했을 때 수량만 저장한다.
                        //저장해 준다.
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemVariationDraft result = context.ItemVariationDrafts.SingleOrDefault(
                                b => b.variation_id == variationId &&
                                     b.ItemInfoDraftId == ItemInfoDraftId
                                     && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                double weight = Convert.ToDouble(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_weight"].Value.ToString().Replace(",", ""));
                                result.weight = weight;
                                context.SaveChanges();

                                if (weight > 0)
                                {
                                    DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_weight"].Style.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_weight"].Style.BackColor = Color.Orange;
                                }
                            }
                        }
                    }
                }
                else if(e.ColumnIndex == 10 && e.RowIndex > -1)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemVariationDraft result = context.ItemVariationDrafts.SingleOrDefault(
                                b => b.variation_id == variationId &&
                                b.ItemInfoDraftId == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            string price = DgSrcVariation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim();
                            result.price = Convert.ToDecimal(price);
                            context.SaveChanges();
                        }
                    }
                }
                else if (e.ColumnIndex == 11 && e.RowIndex > -1)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemVariationDraft result = context.ItemVariationDrafts.SingleOrDefault(
                                b => b.variation_id == variationId &&
                                b.ItemInfoDraftId == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            string original_price = DgSrcVariation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim();
                            result.original_price = Convert.ToDecimal(original_price);
                            context.SaveChanges();
                        }
                    }
                }
                else if ((e.ColumnIndex == 12) && e.RowIndex > -1)
                {
                    //수량을 입력했을 때 수량만 저장한다.
                    //저장해 준다.
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemVariationDraft result = context.ItemVariationDrafts.SingleOrDefault(
                                b => b.variation_id == variationId &&
                                b.ItemInfoDraftId == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            int qty = Convert.ToInt32(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_stock"].Value.ToString().Replace(",", ""));
                            result.stock = qty;
                            context.SaveChanges();

                            if(qty > 0)
                            {
                                DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_stock"].Style.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_stock"].Style.BackColor = Color.Orange;
                            }
                        }
                    }
                    //calcRowPriceFromSellPrice(e.RowIndex, dgSrcItemList.Columns[e.ColumnIndex].Name);
                }
            }
        }

        
        private void DgSrcVariation_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 6 || e.ColumnIndex == 7 || e.ColumnIndex == 12
                || e.ColumnIndex == 13) // 1 should be your column index
            {
                //6: 상품원가
                //7: 마진(원)
                //8: PG수수료
                //9: 판매가(원)
                //10:판매가
                //11:소비자가
                //12:수량
                //13:무게(g)
                double i;

                if (!double.TryParse(Convert.ToString(e.FormattedValue).Replace(",", ""), out i))
                {
                    e.Cancel = true;
                }
                else
                {
                    // the input is numeric 
                }
            }
            else if (e.ColumnIndex == 10 || e.ColumnIndex == 11)
            {
                decimal i;

                if (!decimal.TryParse(Convert.ToString(e.FormattedValue).Replace(",", ""), out i))
                {
                    e.Cancel = true;
                }
                else
                {
                    // the input is numeric 
                }
            }
        }

        private void DgSrcVariation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 8 || e.ColumnIndex == 10 || e.ColumnIndex == 11)
            {
                if (e.ColumnIndex == 8)
                {
                    metroLabel2.Text = "PG수수료";
                }

                if (e.ColumnIndex == 10)
                {
                    metroLabel2.Text = "판매가";
                }

                if (e.ColumnIndex == 11)
                {
                    metroLabel2.Text = "소비자가";
                }
                decimal rateSrc = 0;
                decimal SrcPrice = 0;
                SrcPrice = Convert.ToDecimal(DgSrcVariation.SelectedRows[0].Cells[e.ColumnIndex].Value.ToString().Replace(",", ""));
                string currencyDigit = "";
                string countryCode = "";

                if (tarCountry == "ID")
                {
                    currencyDigit = "{0:n0}";
                    countryCode = "IDR";
                }
                else if (tarCountry == "MY")
                {
                    currencyDigit = "{0:n0}";
                    countryCode = "MYR";
                }
                else if (tarCountry == "SG")
                {
                    currencyDigit = "{0:n0}";
                    countryCode = "SGD";
                }
                else if (tarCountry == "TW")
                {
                    currencyDigit = "{0:n0}";
                    countryCode = "TWD";
                }
                else if (tarCountry == "TH")
                {
                    currencyDigit = "{0:n0}";
                    countryCode = "THB";
                }
                else if (tarCountry == "PH")
                {
                    currencyDigit = "{0:n0}";
                    countryCode = "PHP";
                }

                decimal transKRW = 0;

                rateSrc = txt_src_currency_rate.Value;
                if (countryCode.Contains("JPN") ||
                        countryCode.Contains("IDR") ||
                        countryCode.Contains("VND"))
                {
                    transKRW = SrcPrice * rateSrc / 100;

                    LBLWon.Text = string.Format(currencyDigit, transKRW);
                }
                else
                {
                    transKRW = SrcPrice * rateSrc;
                    LBLWon.Text = string.Format(currencyDigit, transKRW);
                }
            }
        }

        private void BtnApplySrcPrice_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
            {
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_supply_price"].Value = string.Format("{0:n0}", UdSourcePrice.Value);
            }
            Cursor.Current = Cursors.Default;
        }

        private void BtnApplyWeight_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
            {
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_weight"].Value = string.Format("{0:n2}", UdWeight.Value);
            }
            Cursor.Current = Cursors.Default;
        }

        private void BtnAddOption_Click(object sender, EventArgs e)
        {
            string option = TxtVariation.Text.Trim();
            string optionSKU = TxtVariationSKU.Text.Trim();
            long variationID = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            if(option == string.Empty && optionSKU == string.Empty)
            {
                return;
            }
            DateTime dt = new DateTime();
            if (!DateTime.TryParse(lbl_currency_date.Text, out dt))
            {
                dt = DateTime.Now;
            }

            using (AppDbContext context = new AppDbContext())
            {
                ItemVariationDraft varDr = new ItemVariationDraft()
                {
                    ItemInfoDraftId = ItemInfoDraftId,
                    variation_id = variationID,
                    variation_sku = optionSKU,
                    name = option,
                    supply_price = 0,
                    margin = 0,
                    pgFee = 0,
                    price = 0,
                    original_price = 0,
                    targetRetailPriceKRW = 0,
                    targetSellPriceKRW = 0,
                    stock = (int)UdQty.Value,
                    create_time = DateTime.Now,
                    update_time = DateTime.Now,
                    currencyDate = dt,
                    currencyRate =txt_src_currency_rate.Value,
                    isChanged = false,
                    weight = (int)UdWeight.Value,
                    status = "MODEL_NORMAL",                    
                    src_item_id = ItemId,
                    discount_id = 0,
                    src_shopeeAccount = srcShopeeId,
                    tar_shopeeAccount = tarShopeeId,
                    UserId = global_var.userId
                };

                context.ItemVariationDrafts.Add(varDr);
                context.SaveChanges();
            }
            if (option != string.Empty && optionSKU != string.Empty)
            {

                DgSrcVariation.Rows.Add(DgSrcVariation.Rows.Count + 1,
                            false,
                            "MODEL_NORMAL",
                            variationID,
                            optionSKU,
                            option,
                            "0",
                            "0",
                            "0",
                            "0",
                            "0",
                            "0",
                            string.Format("{0:n0}", UdQty.Value),
                            string.Format("{0:n0}", UdWeight.Value),
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            "0");

                //입력한 뒤에 상품원가, 마진, 무게가 있는 경우에는 자동계산하여 값을 넣어준다.

                TxtVariation.Text = "";
                TxtVariationSKU.Text = "";

                TxtVariationSKU.Select();
            }
        }

        private void DgSrcVariation_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            long variationId = Convert.ToInt64(DgSrcVariation.Rows[e.Row.Index].Cells["DgSrcVariation_variation_id"].Value.ToString());
            using (AppDbContext context = new AppDbContext())
            {
                var variation = context.ItemVariationDrafts.FirstOrDefault
                    (x => x.ItemInfoDraftId == ItemInfoDraftId &&
                    x.variation_id == variationId
                    && x.UserId == global_var.userId);

                if (variation != null)
                {
                    context.ItemVariationDrafts.Remove(variation);
                    context.SaveChanges();
                }
            }
        }

        private void BtnDeleteOption_Click(object sender, EventArgs e)
        {
            if(DgSrcVariation.SelectedRows.Count > 0)
            {
                long variationId = Convert.ToInt64(DgSrcVariation.SelectedRows[0].Cells["DgSrcVariation_variation_id"].Value.ToString());
                using (AppDbContext context = new AppDbContext())
                {
                    var variation = context.ItemVariationDrafts.FirstOrDefault
                        (x => x.ItemInfoDraftId == ItemInfoDraftId &&
                        x.variation_id == variationId
                        && x.UserId == global_var.userId);

                    if (variation != null)
                    {
                        context.ItemVariationDrafts.Remove(variation);
                        context.SaveChanges();

                        DgSrcVariation.Rows.RemoveAt(DgSrcVariation.SelectedRows[0].Index);
                    }
                }
            }
        }

        private void BtnReCalc_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
            {
                calcRowPrice(i, DgSrcVariation.Columns[1].Name);
            }
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
