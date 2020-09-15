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
    public partial class FormSetVariation : MetroForm
    {
        public long ItemId;
        public long partner_id;
        public long shop_id;
        public string api_key;
        public string country;
        public string tarCountry;
        public string tarShopeeId;
        public string srcShopeeId;

        public FormSetVariation()
        {
            InitializeComponent();
        }
        private void Fill_Currency_Date()
        {
            using (AppDbContext context = new AppDbContext())
            {
                CurrencyRate currencyList = context.CurrencyRates.FirstOrDefault(x => x.UserId == global_var.userId);
                if (currencyList != null)
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
            if (dic_currency.Count > 0)
            {
                cbo_currency_From.DataSource = new BindingSource(dic_currency, null);
                cbo_currency_From.DisplayMember = "Key";
                cbo_currency_From.ValueMember = "Value";
            }

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
                    UdMargin.Value = result.margin;
                    UdQty.Value = result.qty;

                    decimal weight = (decimal)result.weight;
                }
            }
        }
        private void FormSetVariation_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            Cursor.Current = Cursors.WaitCursor;
            Fill_Currency_Date();
            Fill_from_Currency_Names();
            string countryCurrencyCode = "";
            if (country == "ID")
            {
                countryCurrencyCode = "IDR";
            }
            else if (country == "SG")
            {
                countryCurrencyCode = "SGD";
            }
            else if (country == "MY")
            {
                countryCurrencyCode = "MYR";
            }
            else if (country == "TH")
            {
                countryCurrencyCode = "THB";
            }
            else if (country == "TW")
            {
                countryCurrencyCode = "TWD";
            }
            else if (country == "PH")
            {
                countryCurrencyCode = "PHP";
            }
            else if (country == "VN")
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
            setDefaultVar();
            getVariation();
            Cursor.Current = Cursors.Default;
        }
        private void getVariation()
        {
            using (AppDbContext context = new AppDbContext())
            {
                List<ItemVariation> variationList = context.ItemVariations
                                .Where(b => b.item_id == ItemId
                                && b.UserId == global_var.userId)
                                .OrderBy(x => x.variation_id).ToList();

                string currencyDigit = "";
                string pgDigit = "";
                if (variationList.Count > 0)
                {
                    if (country == "ID")
                    {
                        pgDigit = "{0:n0}";
                        currencyDigit = "{0:n0}";
                    }
                    else if (country == "MY")
                    {
                        pgDigit = "{0:n0}";
                        currencyDigit = "{0:n0}";
                    }
                    else if (country == "SG")
                    {
                        pgDigit = "{0:n2}";
                        currencyDigit = "{0:n1}";
                    }
                    else if (country == "TW")
                    {
                        pgDigit = "{0:n0}";
                        currencyDigit = "{0:n0}";
                    }
                    else if (country == "TH")
                    {
                        pgDigit = "{0:n0}";
                        currencyDigit = "{0:n0}";
                    }
                    else if (country == "PH")
                    {
                        pgDigit = "{0:n0}";
                        currencyDigit = "{0:n0}";
                    }
                    else if (country == "VN")
                    {
                        pgDigit = "{0:n0}";
                        currencyDigit = "{0:n0}";
                    }


                    for (int i = 0; i < variationList.Count; i++)
                    {

                        //주석풀것
                        DgSrcVariation.Rows.Add(i + 1,
                            false,
                            variationList[i].status,
                            variationList[i].variation_id,
                            variationList[i].variation_sku,
                            variationList[i].name,
                            string.Format("{0:n0}", variationList[i].supplyPrice),
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




                        //DgSrcVariation.Rows.Add(i + 1,
                        //    false,
                        //    variationList[i].status,
                        //    variationList[i].variation_id,
                        //    variationList[i].variation_sku,
                        //    variationList[i].name,
                        //    string.Format("{0:n0}", variationList[i].supplyPrice),
                        //    string.Format("{0:n0}", variationList[i].margin),
                        //    string.Format(pgDigit, 0),
                        //    string.Format("{0:n0}", 0),
                        //    string.Format(currencyDigit, variationList[i].price),
                        //    string.Format(currencyDigit, variationList[i].original_price),
                        //    string.Format("{0:n0}", variationList[i].stock),
                        //    string.Format("{0:n2}", variationList[i].weight),
                        //    variationList[i].create_time,
                        //    variationList[i].update_time,
                        //    variationList[i].discount_id);
                    }
                }
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

        private void BtnClose_Click(object sender, EventArgs e)
        {
            bool validate = true;
            //닫기전에 확인하여 준다.
            for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
            {
                if(DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_price"].Value != null 
                    && DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_price"].Value.ToString() != string.Empty
                    && DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_original_price"].Value != null
                    && DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_original_price"].Value.ToString() != string.Empty)
                {
                    decimal price = Convert.ToDecimal(DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_price"].Value.ToString().Replace(",", ""));
                    decimal retailPrice = Convert.ToDecimal(DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_original_price"].Value.ToString().Replace(",", ""));

                    if (price >= retailPrice)
                    {
                        MessageBox.Show("판매가는 소비자가보다 크거나 같을 수 없습니다.\r\n가격을 수정하여 주세요.", "가격오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DgSrcVariation.Rows[i].Selected = true;
                        validate = false;
                        break;
                    }
                }
            }
            
            if(validate)
            {
                this.Close();
            }
        }

        private void Btn_View_Product_Click(object sender, EventArgs e)
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

        private void ViewNaverExchange_Click(object sender, EventArgs e)
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
                    if (currency_info.Key.ToString().Contains(country))
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

        private void BtnApplySrcPrice_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
            {
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_supply_price"].Value = string.Format("{0:n0}", UdSourcePrice.Value);
            }
            Cursor.Current = Cursors.Default;
        }

        private void BtnSaveMargin_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
            {
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_margin"].Value = string.Format("{0:n0}", UdMargin.Value);
            }
            Cursor.Current = Cursors.Default;
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
        private void DgSrcVariation_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (DgSrcVariation.Rows.Count > 0)
            {
                long variationId = Convert.ToInt64(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_variation_id"].Value.ToString());
                if (e.ColumnIndex == 12)
                {
                    //수량
                    int stock = Convert.ToInt32(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_stock"].Value.ToString().Replace(",", ""));
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemVariation result = context.ItemVariations.SingleOrDefault(
                                b => b.variation_id == variationId &&
                                b.item_id == ItemId
                                && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            result.stock = stock;
                            context.SaveChanges();
                        }
                    }
                }
                else if (e.ColumnIndex == 13)
                {
                    if(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_weight"].Value != null
                        && DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_weight"].Value.ToString() != string.Empty)
                    {
                        //무게
                        double weight = Convert.ToDouble(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_weight"].Value.ToString().Replace(",", ""));
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemVariation result = context.ItemVariations.SingleOrDefault(
                                    b => b.variation_id == variationId &&
                                    b.item_id == ItemId
                                    && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                result.weight = weight;
                                context.SaveChanges();
                            }
                        }
                    }
                }
                else if (e.ColumnIndex == 7)
                {
                    if(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_margin"].Value != null 
                        && DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_margin"].Value.ToString() != string.Empty)
                    {
                        //마진
                        int margin = Convert.ToInt32(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_margin"].Value.ToString().Replace(",", ""));
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemVariation result = context.ItemVariations.SingleOrDefault(
                                    b => b.variation_id == variationId &&
                                    b.item_id == ItemId
                                    && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                result.margin = margin;
                                context.SaveChanges();
                            }
                        }
                    }
                }
                else if (e.ColumnIndex == 6)
                {
                    if(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_supply_price"].Value != null &&
                        DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_supply_price"].Value.ToString() != string.Empty)
                    {
                        //원가
                        int supplyPrice = Convert.ToInt32(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_supply_price"].Value.ToString().Replace(",", ""));
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemVariation result = context.ItemVariations.SingleOrDefault(
                                    b => b.variation_id == variationId &&
                                    b.item_id == ItemId
                                    && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                result.supplyPrice = supplyPrice;
                                context.SaveChanges();
                            }
                        }
                    }
                    
                }
                else if (e.ColumnIndex == 4)
                {
                    //sku
                    string sku = DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_variation_sku"].Value.ToString().Trim();
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemVariation result = context.ItemVariations.SingleOrDefault(
                                b => b.variation_id == variationId &&
                                b.item_id == ItemId
                                && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            result.variation_sku = sku;
                            context.SaveChanges();
                        }
                    }
                }
                else if (e.ColumnIndex == 5)
                {
                    //옵션명
                    string name = DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_Name"].Value.ToString().Trim();
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemVariation result = context.ItemVariations.SingleOrDefault(
                                b => b.variation_id == variationId &&
                                b.item_id == ItemId
                                && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            result.name = name;
                            context.SaveChanges();
                        }
                    }
                }
                else if (e.ColumnIndex == 10)
                {
                    //판매가
                     decimal price =  Convert.ToDecimal(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_src_price"].Value.ToString().Trim().Replace(",",""));
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemVariation result = context.ItemVariations.SingleOrDefault(
                                b => b.variation_id == variationId &&
                                b.item_id == ItemId
                                && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            result.price = price;
                            context.SaveChanges();
                        }
                    }
                }
                else if (e.ColumnIndex == 11)
                {
                    //원가
                    decimal original_price = Convert.ToDecimal(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_src_original_price"].Value.ToString().Trim().Replace(",", ""));                   

                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemVariation result = context.ItemVariations.SingleOrDefault(
                                b => b.variation_id == variationId &&
                                b.item_id == ItemId
                                && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            result.original_price = original_price;
                            context.SaveChanges();
                        }
                    }
                }

                if (e.ColumnIndex == 6 || e.ColumnIndex == 7 || e.ColumnIndex == 10 || e.ColumnIndex == 13)
                {
                    calcRowPrice(e.RowIndex, DgSrcVariation.Columns[e.ColumnIndex].Name);
                }
            }
        }

        private void calcRowPrice(int rowId, string cellName)
        {
            string currencyDigit = "";
            string pgDigit = "";

            if (country == "ID")
            {
                pgDigit = "{0:n1}";
                currencyDigit = "{0:n0}";
            }
            else if (country == "MY")
            {
                pgDigit = "{0:n1}";
                currencyDigit = "{0:n0}";
            }
            else if (country == "SG")
            {
                pgDigit = "{0:n2}";
                currencyDigit = "{0:n1}";
            }
            else if (country == "TW")
            {
                pgDigit = "{0:n1}";
                currencyDigit = "{0:n0}";
            }
            else if (country == "TH")
            {
                pgDigit = "{0:n1}";
                currencyDigit = "{0:n0}";
            }
            else if (country == "PH")
            {
                pgDigit = "{0:n1}";
                currencyDigit = "{0:n0}";
            }
            else if (country == "VN")
            {
                pgDigit = "{0:n1}";
                currencyDigit = "{0:n0}";
            }

            if(DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_supply_price"].Value != null &&
                DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_supply_price"].Value.ToString() != string.Empty &&
                DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Value != null &&
                DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Value.ToString() != string.Empty &&
                DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Value != null &&
                DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Value.ToString() != string.Empty)
            {
                int sourcePrice = Convert.ToInt32(DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_supply_price"].Value.ToString().Replace(",", ""));
                double tempWeight = Convert.ToDouble(DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Value.ToString().Replace(",", ""));
                int productWeight = (int)(tempWeight * 1000);
                //double productWeight = Convert.ToDouble(DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Value.ToString().Replace(",", ""));
                long variationId = Convert.ToInt64(DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_variation_id"].Value.ToString().Replace(",", ""));

                int margin = Convert.ToInt32(DgSrcVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Value.ToString().Replace(",", ""));

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

                string countryCode = country;

                //판매국가의 환율
                decimal rateSrc = txt_src_currency_rate.Value;
                if (sourcePrice > 0 && margin >= 0 && productWeight > 0)
                {
                    PriceCalculator pCalc = new PriceCalculator();
                    pCalc.CountryCode = country;
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
                        ItemInfo result = context.ItemInfoes.SingleOrDefault(
                                b => b.item_id == ItemId
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


                        ItemVariation variation = context.ItemVariations.FirstOrDefault(
                            b => b.item_id == ItemId &&
                                    b.variation_id == variationId && b.UserId == global_var.userId);
                        if (variation != null)
                        {
                            variation.weight = tempWeight;
                            variation.supplyPrice = sourcePrice;
                            variation.margin = margin;                            
                            variation.price = calcResult["targetSellPrice"];
                            variation.original_price = calcResult["targetRetailPrice"];

                            //주석풀것
                            variation.pgFee = calcResult["pgFee"];
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

                    if(cellName != string.Empty)
                    {
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
            }
        }
        
        private void DgSrcVariation_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 10)
            {
                //판매가
                decimal price = 0;

                bool isOk = decimal.TryParse(e.FormattedValue.ToString().Replace(",", ""), out price);

                if(isOk)
                {
                    decimal retailPrice = Convert.ToDecimal(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_src_original_price"].Value.ToString());
                    if(retailPrice > 0)
                    {
                        if (price >= retailPrice)
                        {
                            MessageBox.Show("판매가격은 소비자가격 보다 크거가 같을 수 없습니다.", "가격 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Cancel = true;
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else if (e.ColumnIndex == 11)
            {
                //상품 소비자가 수정일 때
                decimal input = 0;
                bool isOk = decimal.TryParse(e.FormattedValue.ToString().Replace(",", ""), out input);

                if (isOk)
                {
                    //소비자가를 가지고 와서 비교한다. 소비자가보다 크면 안된다.
                    decimal price = Convert.ToDecimal(DgSrcVariation.Rows[e.RowIndex].Cells["DgSrcVariation_src_price"].Value.ToString().Replace(",", ""));
                    if(input > 0)
                    {
                        if (input < price)
                        {
                            MessageBox.Show("소비자가격이 판매가격보다 작을 수 없습니다.", "가격 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Cancel = true;
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                }

            }
            else if (e.ColumnIndex == 12)
            {
                //수량
                int a = 0;
                var val = (string)e.FormattedValue;

                if (!int.TryParse(val, out a))
                {
                    //문자인 경우
                    e.Cancel = true;
                }
            }
        }

        private void UdQty_ValueChanged(object sender, EventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                ConfigVar result = context.ConfigVars.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result == null)
                {
                    ConfigVar newVar = new ConfigVar
                    {
                        qty = UdQty.Value,
                        UserId = global_var.userId
                    };
                    context.ConfigVars.Add(newVar);
                    context.SaveChanges();
                }
                else
                {
                    result.qty = UdQty.Value;
                    context.SaveChanges();
                }
            }

            BtnApplyQty_Click(null, null);
        }

        private void UdMargin_ValueChanged(object sender, EventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                ConfigVar result = context.ConfigVars.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result == null)
                {
                    ConfigVar newVar = new ConfigVar
                    {
                        margin = UdMargin.Value,
                        UserId = global_var.userId
                    };
                    context.ConfigVars.Add(newVar);
                    context.SaveChanges();
                }
                else
                {
                    result.margin = UdMargin.Value;
                    context.SaveChanges();
                }
            }
        }

        private void UdWeight_ValueChanged(object sender, EventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                ConfigVar result = context.ConfigVars.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result == null)
                {
                    ConfigVar newVar = new ConfigVar
                    {
                        weight = (double)udSrcWeight.Value,
                        UserId = global_var.userId
                    };
                    context.ConfigVars.Add(newVar);
                    context.SaveChanges();
                }
                else
                {
                    result.weight = (double)udSrcWeight.Value;
                    context.SaveChanges();
                }
            }
            
        }

        private void BtnSaveCurrencyRate_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("환율을 저장 하시겠습니까?", "환율 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    if (cbo_currency_From.Text.Contains("PHP"))
                    {
                        var res = context.CurrencyRates.FirstOrDefault(x => x.cr_code.Contains("PHP") && x.UserId == global_var.userId);

                        if (res != null)
                        {
                            res.cr_exchange = txt_src_currency_rate.Value;
                            context.SaveChanges();
                        }
                    }
                    else if (cbo_currency_From.Text.Contains("IDR"))
                    {
                        var res = context.CurrencyRates.FirstOrDefault(x => x.cr_code.Contains("IDR") && x.UserId == global_var.userId);

                        if (res != null)
                        {
                            res.cr_exchange = txt_src_currency_rate.Value;
                            context.SaveChanges();
                        }
                    }
                    else if (cbo_currency_From.Text.Contains("SGD"))
                    {
                        var res = context.CurrencyRates.FirstOrDefault(x => x.cr_code.Contains("SGD") && x.UserId == global_var.userId);

                        if (res != null)
                        {
                            res.cr_exchange = txt_src_currency_rate.Value;
                            context.SaveChanges();
                        }
                    }
                    else if (cbo_currency_From.Text.Contains("MYR"))
                    {
                        var res = context.CurrencyRates.FirstOrDefault(x => x.cr_code.Contains("MYR") && x.UserId == global_var.userId);

                        if (res != null)
                        {
                            res.cr_exchange = txt_src_currency_rate.Value;
                            context.SaveChanges();
                        }
                    }
                    else if (cbo_currency_From.Text.Contains("THB"))
                    {
                        var res = context.CurrencyRates.FirstOrDefault(x => x.cr_code.Contains("THB") && x.UserId == global_var.userId);

                        if (res != null)
                        {
                            res.cr_exchange = txt_src_currency_rate.Value;
                            context.SaveChanges();
                        }
                    }
                    else if (cbo_currency_From.Text.Contains("TWD"))
                    {
                        var res = context.CurrencyRates.FirstOrDefault(x => x.cr_code.Contains("TWD") && x.UserId == global_var.userId);

                        if (res != null)
                        {
                            res.cr_exchange = txt_src_currency_rate.Value;
                            context.SaveChanges();
                        }
                    }
                }
            }
        }

        private void BtnAddOption_Click(object sender, EventArgs e)
        {
            string option = TxtVariation.Text.Trim();
            string optionSKU = TxtVariationSKU.Text.Trim();
            long variationID = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            if (option == string.Empty && optionSKU == string.Empty)
            {
                return;
            }

            //동일한 옵션명으로 등록 못하게 한다.
            bool isDuplicate = false;

            for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
            {
                if(option == DgSrcVariation.Rows[i].Cells["DgSrcVariation_Name"].Value.ToString().Trim())
                {
                    isDuplicate = true;
                    MessageBox.Show("동일한 옵션이 존재합니다","옵션 중복", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    TxtVariation.Select();
                    TxtVariation.SelectAll();
                    break;
                }
            }

            if(isDuplicate)
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
                ItemVariation varDr = new ItemVariation()
                {
                    variation_id = variationID,
                    variation_sku = optionSKU,
                    name = option,
                    price = 0,
                    stock = (int)UdQty.Value,
                    status = "MODEL_NORMAL",
                    create_time = DateTime.Now,
                    update_time = DateTime.Now,
                    original_price = 0,
                    discount_id = 0,
                    item_id  = ItemId,
                    weight = (int)udSrcWeight.Value,
                    supplyPrice = Convert.ToInt32(UdSourcePrice.Value),
                    margin = Convert.ToInt32(UdMargin.Value),
                    currencyDate = DateTime.Now,
                    currencyRate = txt_src_currency_rate.Value,
                    UserId = global_var.userId
                };

                context.ItemVariations.Add(varDr);
                context.SaveChanges();

                //만약에 옵션이 존재한다면 옵션이 있다고 플래그를 세팅한다.
                var itemInfo = context.ItemInfoes.FirstOrDefault(x => x.item_id == ItemId && x.UserId == global_var.userId);
                if (itemInfo != null)
                {
                    if (DgSrcVariation.Rows.Count > 0)
                    {
                        itemInfo.has_variation = true;
                    }
                    else
                    {
                        itemInfo.has_variation = false;
                    }

                    context.SaveChanges();
                }
                

            }
            if (option != string.Empty && optionSKU != string.Empty)
            {

                DgSrcVariation.Rows.Add(DgSrcVariation.Rows.Count + 1,
                            false,
                            "MODEL_NORMAL",
                            variationID,
                            optionSKU,
                            option,
                            string.Format("{0:n0}", UdSourcePrice.Value),
                            string.Format("{0:n0}", UdMargin.Value),
                            "0",
                            "0",
                            "0",
                            "0",
                            string.Format("{0:n0}", UdQty.Value),
                            string.Format("{0:n2}", udSrcWeight.Value),
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            "0");

                //입력한 뒤에 상품원가, 마진, 무게가 있는 경우에는 자동계산하여 값을 넣어준다.

                TxtVariation.Text = "";
                TxtVariationSKU.Text = "";
                calcRowPrice(DgSrcVariation.Rows.Count - 1 , "");
                TxtVariationSKU.Select();
            }
        }

        private void BtnDeleteOption_Click(object sender, EventArgs e)
        {
            if (DgSrcVariation.SelectedRows.Count > 0)
            {
                long variationId = Convert.ToInt64(DgSrcVariation.SelectedRows[0].Cells["DgSrcVariation_variation_id"].Value.ToString());
                using (AppDbContext context = new AppDbContext())
                {
                    var variation = context.ItemVariations.FirstOrDefault
                        (x => x.item_id == ItemId &&
                        x.variation_id == variationId && x.UserId == global_var.userId);

                    if (variation != null)
                    {
                        context.ItemVariations.Remove(variation);
                        context.SaveChanges();

                        DgSrcVariation.Rows.RemoveAt(DgSrcVariation.SelectedRows[0].Index);
                    }

                    //만약에 옵션이 존재한다면 옵션이 있다고 플래그를 세팅한다.
                    var itemInfo = context.ItemInfoes.FirstOrDefault(x => x.item_id == ItemId && x.UserId == global_var.userId);
                    if (itemInfo != null)
                    {
                        if (DgSrcVariation.Rows.Count > 0)
                        {
                            itemInfo.has_variation = true;
                        }
                        else
                        {
                            itemInfo.has_variation = false;
                        }

                        context.SaveChanges();
                    }
                }
            }
        }

        private void BtnDeleteAllOption_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("모든 옵션을 삭제하시겠습니까?", "옵션 전체 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                long variationId = Convert.ToInt64(DgSrcVariation.SelectedRows[0].Cells["DgSrcVariation_variation_id"].Value.ToString());
                using (AppDbContext context = new AppDbContext())
                {
                    var variation = context.ItemVariations.Where
                        (x => x.item_id == ItemId && x.UserId == global_var.userId).ToList();

                    if (variation != null)
                    {
                        context.ItemVariations.RemoveRange(variation);
                        context.SaveChanges();

                        DgSrcVariation.Rows.Clear();
                    }

                    //만약에 옵션이 존재한다면 옵션이 있다고 플래그를 세팅한다.
                    var itemInfo = context.ItemInfoes.FirstOrDefault(x => x.item_id == ItemId && x.UserId == global_var.userId);
                    if (itemInfo != null)
                    {
                        if (DgSrcVariation.Rows.Count > 0)
                        {
                            itemInfo.has_variation = true;
                        }
                        else
                        {
                            itemInfo.has_variation = false;
                        }

                        context.SaveChanges();
                    }
                }
            }   
        }

        private void TxtVariationSKU_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                TxtVariation.Select();
                TxtVariation.SelectAll();
            }
        }

        private void TxtVariation_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                BtnAddOption_Click(null, null);
            }
        }

        private void BtnApplyPrice_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
            {
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_supply_price"].Value = "0";
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_margin"].Value = "0";
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_pg_fee"].Value = "0";
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_price_won"].Value = "0";
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_original_price"].Value = string.Format("{0:n2}", udRetailPrice.Value);

                long variationID = Convert.ToInt64(DgSrcVariation.Rows[i].Cells["DgSrcVariation_variation_id"].Value.ToString());

                using (AppDbContext context = new AppDbContext())
                {
                    var val = context.ItemVariations.FirstOrDefault(b => b.item_id == ItemId
                                && b.variation_id == variationID
                                && b.UserId == global_var.userId);

                    if(val != null)
                    {
                        val.supplyPrice = 0;
                        val.pgFee = 0;                        
                        val.targetRetailPriceKRW = 0;
                        val.targetSellPriceKRW = 0;                        
                        val.margin = 0;
                        val.original_price = udRetailPrice.Value;

                        context.SaveChanges();
                    }
                }
            }
        }

        private void BtnApplySrcWeight_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
            {
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_weight"].Value = udSrcWeight.Value;
            }


            for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
            {
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_weight"].Value = string.Format("{0:n2}", udSrcWeight.Value);

                long variationID = Convert.ToInt64(DgSrcVariation.Rows[i].Cells["DgSrcVariation_variation_id"].Value.ToString());

                using (AppDbContext context = new AppDbContext())
                {
                    var val = context.ItemVariations.FirstOrDefault(b => b.item_id == ItemId
                                && b.variation_id == variationID
                                && b.UserId == global_var.userId);

                    if (val != null)
                    {
                        val.weight = Convert.ToDouble(udSrcWeight.Value);                        
                        context.SaveChanges();
                    }
                }
            }
        }

        private void BtnApplySellPrice_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
            {
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_supply_price"].Value = "0";
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_margin"].Value = "0";
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_pg_fee"].Value = "0";
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_price_won"].Value = "0";
                DgSrcVariation.Rows[i].Cells["DgSrcVariation_src_price"].Value = string.Format("{0:n2}", udSellPrice.Value);

                long variationID = Convert.ToInt64(DgSrcVariation.Rows[i].Cells["DgSrcVariation_variation_id"].Value.ToString());

                using (AppDbContext context = new AppDbContext())
                {
                    var val = context.ItemVariations.FirstOrDefault(b => b.item_id == ItemId
                                && b.variation_id == variationID
                                && b.UserId == global_var.userId);

                    if (val != null)
                    {
                        val.supplyPrice = 0;
                        val.pgFee = 0;                        
                        val.targetRetailPriceKRW = 0;
                        val.targetSellPriceKRW = 0;
                        val.price = udSellPrice.Value;
                        val.margin = 0;                       
                        context.SaveChanges();
                    }
                }
            }
        }

        private void UdShopeeFee_ValueChanged(object sender, EventArgs e)
        {
            if(DgSrcVariation.Rows.Count > 0)
            {
                for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
                {
                    calcRowPrice(i, "");
                }
            }
        }

        private void udPGFee_ValueChanged(object sender, EventArgs e)
        {
            if (DgSrcVariation.Rows.Count > 0)
            {
                for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
                {
                    calcRowPrice(i, "");
                }
            }
        }

        private void UdPayoneerFee_ValueChanged(object sender, EventArgs e)
        {
            if (DgSrcVariation.Rows.Count > 0)
            {
                for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
                {
                    calcRowPrice(i, "");
                }
            }
        }

        private void UdRetailPriceRate_ValueChanged(object sender, EventArgs e)
        {
            if (DgSrcVariation.Rows.Count > 0)
            {
                for (int i = 0; i < DgSrcVariation.Rows.Count; i++)
                {
                    calcRowPrice(i, "");
                }
            }
        }
    }
}
