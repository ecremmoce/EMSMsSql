using MetroFramework.Forms;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormUploader : MetroForm
    {
        public FormUploader(string Lang)
        {
            InitializeComponent();
            MinimizeBox = false;
            MaximizeBox = false;
            AutoSize = true;
            AutoScroll = true;
            WindowState = FormWindowState.Normal;
        }
        private void set_double_buffer()
        {
            Control[] controls = GetAllControlsUsingRecursive(this);
            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i].GetType() == typeof(DataGridView))
                {
                    ((DataGridView)controls[i]).DoubleBuffered(true);
                }
            }
        }
        static Control[] GetAllControlsUsingRecursive(Control containerControl)
        {
            List<Control> allControls = new List<Control>();
            Queue<Control.ControlCollection> queue = new Queue<Control.ControlCollection>();
            queue.Enqueue(containerControl.Controls);
            while (queue.Count > 0)
            {
                Control.ControlCollection controls = queue.Dequeue();
                if (controls == null || controls.Count == 0) continue;
                foreach (Control control in controls)
                {
                    allControls.Add(control);
                    queue.Enqueue(control.Controls);
                }
            }
            return allControls.ToArray();
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
            var cc = new cls_currency();
            var dic_currency = new Dictionary<string, decimal>();
            dic_currency = cc.get_currency();

            if(dic_currency.Count > 0)
            {
                cbo_currency_From.DataSource = new BindingSource(dic_currency, null);
                cbo_currency_From.DisplayMember = "Key";
                cbo_currency_From.ValueMember = "Value";
            }
        }

        private void getShopeeAccount()
        {
            dg_site_id.Rows.Clear();
            try
            {
                ClsShopee cc = new ClsShopee();
                IList<ShopeeAccount> ShopeeAccounts = cc.GetShopeeAccountList();
                for (int i = 0; i < ShopeeAccounts.Count; i++)
                {
                    dg_site_id.Rows.Add(i + 1, ShopeeAccounts[i].ShopeeId,
                        ShopeeAccounts[i].ShopeeCountry,
                        ShopeeAccounts[i].PartnerId,
                        ShopeeAccounts[i].ShopId,
                        ShopeeAccounts[i].SecretKey);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getShopeeAccount: " + ex.Message);
            }

        }
        private void FillHfType()
        {
            dg_hf_category.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<HFType> hfTypeList = context.HFTypes.Where(x => x.UserId == global_var.userId).OrderBy(x => x.HFTypeName).ToList();

                for (int i = 0; i < hfTypeList.Count; i++)
                {
                    dg_hf_category.Rows.Add(hfTypeList[i].HFTypeID,
                        i + 1, false,
                        hfTypeList[i].HFTypeName);
                }

                if (dg_hf_category.Rows.Count > 0)
                {
                    dg_hf_category.Rows[0].Selected = true;
                    int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());
                    FillHfList(HFTypeID);
                }
            }
        }

        private void FillHeaderSet()
        {
            dg_list_header_set.Rows.Clear();
            dg_upload_header.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<SetHeader> SetHeaderList = context.SetHeaders.Where(x => x.UserId == global_var.userId).OrderBy(x => x.SetHeaderName).ToList();

                for (int i = 0; i < SetHeaderList.Count; i++)
                {
                    dg_list_header_set.Rows.Add(SetHeaderList[i].Id,
                        i + 1, false,
                        SetHeaderList[i].SetHeaderName);

                    dg_upload_header.Rows.Add(SetHeaderList[i].Id,
                        i + 1, false,
                        SetHeaderList[i].SetHeaderName);
                }

                if (dg_list_header_set.Rows.Count > 0)
                {
                    dg_list_header_set.Rows[0].Selected = true;
                    int SetHeaderId = Convert.ToInt32(dg_list_header_set.SelectedRows[0].Cells["dg_headerset_id"].Value.ToString());
                    FillTempleteHeader(SetHeaderId);

                    dg_upload_header.Rows[0].Selected = true;
                }
            }
        }
        private void FillFooterSet()
        {
            dg_list_footer_set.Rows.Clear();
            dg_upload_footer.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<SetFooter> SetFooterList = context.SetFooters.Where(x => x.UserId == global_var.userId).OrderBy(x => x.SetFooterName).ToList();

                for (int i = 0; i < SetFooterList.Count; i++)
                {
                    dg_list_footer_set.Rows.Add(SetFooterList[i].Id,
                        i + 1, false,
                        SetFooterList[i].SetFooterName);
                }

                for (int i = 0; i < SetFooterList.Count; i++)
                {
                    dg_upload_footer.Rows.Add(SetFooterList[i].Id,
                        i + 1, false,
                        SetFooterList[i].SetFooterName);
                }
                

                if (dg_list_footer_set.Rows.Count > 0)
                {
                    dg_list_footer_set.Rows[0].Selected = true;
                    int SetFooterId = Convert.ToInt32(dg_list_footer_set.SelectedRows[0].Cells["dg_footerset_id"].Value.ToString());
                    FillTempleteFooter(SetFooterId);

                    dg_upload_footer.Rows[0].Selected = true;
                }
            }
        }
        private void FillHfList(int HFTypeId)
        {
            dg_hf_list.Rows.Clear();
            TxtHFContent.Text = "";
            using (AppDbContext context = new AppDbContext())
            {
                List<HFList> hfList = context.HFLists
                    .Where(x => x.HFTypeID == HFTypeId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.HFName).ToList();


                string firstData = "";
                if(hfList.Count > 0)
                {
                    for (int i = 0; i < hfList.Count; i++)
                    {
                        if(i == 0)
                        {                            
                            firstData = hfList[i].HFContent;
                        }

                        dg_hf_list.Rows.Add(hfList[i].HFListID, i + 1, false, hfList[i].HFName);
                    }
                }
                

                if (dg_hf_list.Rows.Count > 0 && dg_hf_list.SelectedRows.Count > 0)
                {
                    TxtHFContent.Text = firstData;
                    txt_template_name.Text = dg_hf_list.SelectedRows[0].Cells["dg_hf_list_template_name"].Value.ToString();
                }
            }
        }

        private void FillTempleteHeader(int HeaderSetId)
        {
            dg_list_header.Rows.Clear();
            TxtHFContent.Text = "";
            using (AppDbContext context = new AppDbContext())
            {
                List<TemplateHeader> templeteHeaderList = context.TemplateHeaders
                    .Where(x => x.SetHeaderId == HeaderSetId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.HFName).ToList();


                for (int i = 0; i < templeteHeaderList.Count; i++)
                {
                    dg_list_header.Rows.Add(templeteHeaderList[i].Id, 
                        i + 1, false, templeteHeaderList[i].HFName,
                        templeteHeaderList[i].HFListID);
                }
            }
        }
        private void FillTempleteFooter(int FooterSetId)
        {
            dg_list_footer.Rows.Clear();
            TxtHFContent.Text = "";
            using (AppDbContext context = new AppDbContext())
            {
                List<TemplateFooter> templeteFooterList = context.TemplateFooters
                    .Where(x => x.SetFooterId == FooterSetId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.HFName).ToList();


                for (int i = 0; i < templeteFooterList.Count; i++)
                {
                    dg_list_footer.Rows.Add(templeteFooterList[i].Id,
                        i + 1, false, templeteFooterList[i].HFName,
                        templeteFooterList[i].HFListID);
                }
            }
        }
        private void getLogisticList()
        {
            dg_shopee_logistics.Rows.Clear();
            string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
            using (AppDbContext context = new AppDbContext())
            {
                List<Logistic> logisticList = context.Logistics
                    .Where(x => x.shopeeAccount == shopeeId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.logistic_name).ToList();

                if (logisticList.Count > 0)
                {
                    for (int i = 0; i < logisticList.Count; i++)
                    {
                        dg_shopee_logistics.Rows.Add(i + 1,
                            false,
                            logisticList[i].logistic_name,
                            logisticList[i].logistic_id,
                            logisticList[i].has_cod,
                            logisticList[i].fee_type);
                    }
                }
            }
        }

        private void checkCategoryMapData()
        {
            using (var context = new AppDbContext())
            {
                var category = context.ShopeeCategories.FirstOrDefault();
                if (category == null)
                {   
                    MessageBox.Show("카테고리 맵핑 데이터가 존재하지 않습니다.\r\n상단 메뉴의 카테고리 맵핑에 데이터를 업로드 하여 주세요.",
                        "카테고리 맵핑 데이터 누락",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void FormUploader_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            CboSearchType.SelectedIndex = 0;
            
            getShopeeAccount();
            FillHfType();
            FillHeaderSet();
            FillFooterSet();
            checkCategoryMapData();
            if (dg_site_id.Rows.Count > 0 && dg_site_id.SelectedRows.Count > 0)
            {
                DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(0, 0);
                dg_site_id_CellClick(null, et);
            }

            
            if (dg_hf_category.SelectedRows.Count > 0)
            {
                DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(0, 0);
                dg_hf_category_CellClick(null, et);
            }

            if (dg_list_header_set.Rows.Count > 0 && dg_list_header_set.SelectedRows.Count > 0)
            {
                DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(0, 0);
                dg_list_header_set_CellClick(null, et);
            }

            if (dg_list_footer_set.Rows.Count > 0 && dg_list_footer_set.SelectedRows.Count > 0)
            {
                DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(0, 0);
                dg_list_footer_set_CellClick(null, et);
            }


            Fill_Currency_Date();
            Fill_from_Currency_Names();
            FillHFSeparator();
            setDefaultVar();


            DataGridViewCellEventArgs et2 = new DataGridViewCellEventArgs(0, 0);
            dg_site_id_CellClick(null, et2);
        }
        private void FillHFSeparator()
        {
            TxtHeaderSeparator.Text = "";
            TxtFooterSeparator.Text = "";

            using (AppDbContext context = new AppDbContext())
            {
                List<HeaderSeparator> Hresult = context.HeaderSeparators.Where(x => x.UserId == global_var.userId).ToList();
                if (Hresult != null && Hresult.Count > 0)
                {
                    TxtHeaderSeparator.Text = Hresult[0].HeaderSeparatorString;
                }

                List<FooterSeparator> Fresult = context.FooterSeparators.Where(x => x.UserId == global_var.userId).ToList();
                if (Fresult != null && Fresult.Count > 0)
                {
                    TxtFooterSeparator.Text = Fresult[0].FooterSeparatorString;
                }
            }
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
                    
                    if(result.dts == 0)
                    {
                        UdDTS.Value = 2;
                    }
                    else
                    {
                        UdDTS.Value = result.dts;
                    }
                    
                }
            }
        }
        public delegate void InvokeDelegate();
        private void getProductList(string area, string keyword)
        {
            dgSrcItemList.Rows.Clear();
            if (dg_site_id.SelectedRows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                decimal rateSrc = 0;
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                string currencyDigit = "";
                string pgDigit = "";
                string countryCode = "";

                if (shopeeId.ToUpper().Contains(".ID"))
                {
                    pgDigit = "{0:n1}";
                    currencyDigit = "{0:n0}";
                    countryCode = "IDR";
                }
                else if (shopeeId.ToUpper().Contains(".MY"))
                {
                    pgDigit = "{0:n1}";
                    currencyDigit = "{0:n0}";
                    countryCode = "MYR";
                }
                else if (shopeeId.ToUpper().Contains(".SG"))
                {
                    pgDigit = "{0:n2}";
                    currencyDigit = "{0:n1}";
                    countryCode = "SGD";
                }
                else if (shopeeId.ToUpper().Contains(".TW"))
                {
                    pgDigit = "{0:n1}";
                    currencyDigit = "{0:n0}";
                    countryCode = "TWD";
                }
                else if (shopeeId.ToUpper().Contains(".TH"))
                {
                    pgDigit = "{0:n1}";
                    currencyDigit = "{0:n0}";
                    countryCode = "THB";
                }
                else if (shopeeId.ToUpper().Contains(".PH"))
                {
                    pgDigit = "{0:n1}";
                    currencyDigit = "{0:n0}";
                    countryCode = "PHP";
                }

                //환율을 가지고 온다.
                rateSrc = txt_src_currency_rate.Value;

                using (AppDbContext context = new AppDbContext())
                {
                    List<ItemInfoDraft> productList = null;
                    if (area == "상품ID")
                    {
                        long ItemId = 0;
                        if (keyword != string.Empty)
                        {
                            if (long.TryParse(keyword, out ItemId))
                            {
                                productList = context.ItemInfoDrafts
                                .Where(b => b.tar_shopeeAccount == shopeeId &&
                                b.src_item_id == ItemId
                                && b.UserId == global_var.userId)
                                .OrderBy(x => x.update_time).ToList();
                            }
                        }
                    }
                    else if (area == "상품SKU")
                    {
                        if (keyword != string.Empty)
                        {
                            productList = context.ItemInfoDrafts
                                .Where(b => b.tar_shopeeAccount == shopeeId &&
                                b.item_sku.Contains(keyword)
                                && b.UserId == global_var.userId)
                                .OrderBy(x => x.update_time).ToList();
                        }
                    }
                    else if (area == "상품명")
                    {
                        if (keyword != string.Empty)
                        {
                            productList = context.ItemInfoDrafts
                                .Where(b => b.tar_shopeeAccount == shopeeId &&
                                b.name.Contains(keyword)
                                && b.UserId == global_var.userId)
                                .OrderBy(x => x.update_time).ToList();
                        }
                    }
                    else if (area == "")
                    {
                        productList = context.ItemInfoDrafts
                        .Where(b => b.tar_shopeeAccount == shopeeId
                        && b.UserId == global_var.userId)
                        .OrderBy(x => x.update_time).ToList();
                    }

                    if (productList != null)
                    {
                        progressBar.Value = 0;
                        progressBar.Maximum = productList.Count;
                        for (int i = 0; i < productList.Count; i++)
                        {
                            string strImage = productList[i].images;

                            if (strImage.Length > 0)
                            {
                                var arImages = strImage.Split('^');
                                if (arImages.Length > 0)
                                {
                                    strImage = arImages[0].ToString().Trim();
                                }
                            }
                            

                            System.Drawing.Image img = null;
                            dgSrcItemList.Rows.Add(
                                i + 1,
                                false,
                                img,
                                productList[i].src_shopeeAccount,
                                productList[i].src_item_id,
                                productList[i].src_shopid,
                                productList[i].item_sku,
                                productList[i].status,
                                productList[i].name,
                                productList[i].currency,
                                (bool)productList[i].has_variation,
                                false,
                                string.Format("{0:n0}", productList[i].supply_price),
                                string.Format("{0:n0}", productList[i].margin),
                                0,
                                string.Format(pgDigit, productList[i].pgFee),
                                string.Format("{0:n0}", productList[i].targetSellPriceKRW),
                                string.Format(currencyDigit, productList[i].price),
                                string.Format(currencyDigit, productList[i].original_price),
                                string.Format("{0:n0}", productList[i].stock),
                                productList[i].create_time,
                                productList[i].update_time,
                                string.Format("{0:n2}", productList[i].weight),
                                productList[i].category_id,
                                productList[i].category_id_tar,
                                Math.Truncate(productList[i].rating_star * 100) / 100,
                                string.Format("{0:n0}", productList[i].cmt_count),
                                string.Format("{0:n0}", productList[i].sales),
                                string.Format("{0:n0}", productList[i].views),
                                string.Format("{0:n0}", productList[i].likes),
                                productList[i].package_length,
                                productList[i].package_width,
                                productList[i].package_height,
                                productList[i].days_to_ship,
                                productList[i].size_chart,
                                productList[i].condition,
                                productList[i].discount_id,
                                productList[i].discount_name,
                                productList[i].is_2tier_item,
                                productList[i].images,
                                productList[i].Id,
                                productList[i].description,
                                productList[i].SetHeaderName,
                                productList[i].SetHeaderId,
                                productList[i].SetFooterName,
                                productList[i].SetFooterId,
                                "",
                                "",
                                txt_src_currency_rate.Value.ToString(),
                                lbl_currency_date.Text);

                            if (productList[i].isChanged)
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_item_id"].Style.BackColor = Color.Orange;
                            }

                            if(productList[i].supply_price > 0)
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_supply_price"].Style.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_supply_price"].Style.BackColor = Color.Orange;
                            }

                            if (productList[i].margin > 0)
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_margin"].Style.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_margin"].Style.BackColor = Color.Orange;
                            }

                            if (productList[i].pgFee > 0)
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_pg_fee"].Style.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_pg_fee"].Style.BackColor = Color.Orange;
                            }

                            if (productList[i].targetSellPriceKRW > 0)
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_price_won"].Style.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_price_won"].Style.BackColor = Color.Orange;
                            }

                            if (productList[i].price > 0)
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_price"].Style.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_price"].Style.BackColor = Color.Orange;
                            }

                            if (productList[i].original_price > 0)
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_original_price"].Style.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_original_price"].Style.BackColor = Color.Orange;
                            }

                            if (productList[i].stock > 0)
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_stock"].Style.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_stock"].Style.BackColor = Color.Orange;
                            }

                            if (productList[i].weight > 0)
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_weight"].Style.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_weight"].Style.BackColor = Color.Orange;
                            }

                            if (productList[i].category_id_tar > 0)
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                            }

                            if (productList[i].status != "NORMAL")
                            {
                                dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_status"].Style.BackColor = Color.Orange;
                            }
                        }


                        UnLockMaxParallel();
                        CancellationTokenSource cts = new CancellationTokenSource();

                        Task.Run(() =>
                        {
                            try
                            {
                                Parallel.ForEach(dgSrcItemList.Rows.Cast<DataGridViewRow>(),
                                    new ParallelOptions { MaxDegreeOfParallelism = 200 }
                                    , dgRow =>
                                    {
                                        var retryCount = 0;
                                        Retry:
                                        try
                                        {
                                            if (cts.IsCancellationRequested) return;

                                            var iUrl = dgRow.Cells["dgItemList_images"].Value.ToString() as string;
                                            var ic = dgRow.Cells["dgItemList_Image"] as DataGridViewImageCell;

                                            string[] imgAddress = iUrl.Split('^');
                                            if(imgAddress.Length > 0)
                                            {
                                                var request = WebRequest.Create(imgAddress[0].ToString().Trim());
                                                var response = (HttpWebResponse)request.GetResponse();
                                                var dataStream = response.GetResponseStream();

                                                ic.Value = new Bitmap(dataStream);

                                                progressBar.BeginInvoke(new InvokeDelegate(() => Progress(cts)));
                                            }
                                        }
                                        catch
                                        {
                                            if (++retryCount < 3)
                                            {
                                                Thread.Sleep(retryCount * 1000);
                                                goto Retry;
                                            }

                                        }
                                    });
                            }
                            catch
                            {
                                // ignore
                            }
                        });
                    }
                }

                grp_src.Text = "상품 목록 : [ " + string.Format("{0:n0}", dgSrcItemList.Rows.Count) + " ]";
                dgSrcItemList.ClearSelection();
                Cursor.Current = Cursors.Default;
                //MessageBox.Show("상품 목록을 로드하였습니다.", "상품목록 로드", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public static void UnLockMaxParallel()
        {
            int prevThreads, prevPorts;
            ThreadPool.GetMinThreads(out prevThreads, out prevPorts);
            ThreadPool.SetMinThreads(200, prevPorts);
        }
        private void calcRowPrice(int rowId, string cellName)
        {
            isChanging = true;
            string TarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
            //등록기에서는 등록을 위하여 공급가로부터 시작하여 상품 판매가를 계산한다.
            int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.Rows[rowId].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
            decimal sourcePrice = Convert.ToDecimal(dgSrcItemList.Rows[rowId].Cells["dgItemList_supply_price"].Value.ToString().Replace(",",""));

            double tempWeight = Convert.ToDouble(dgSrcItemList.Rows[rowId].Cells["dgItemList_weight"].Value.ToString().Replace(",", ""));
            int productWeight = (int)(tempWeight * 1000);            

            decimal margin = Convert.ToDecimal(dgSrcItemList.Rows[rowId].Cells["dgItemList_margin"].Value.ToString().Replace(",", ""));

            if (margin > 0)
            {
                dgSrcItemList.Rows[rowId].Cells["dgItemList_margin"].Style.BackColor = Color.GreenYellow;
            }
            else
            {
                dgSrcItemList.Rows[rowId].Cells["dgItemList_margin"].Style.BackColor = Color.Orange;
            }

            if (productWeight > 0)
            {
                dgSrcItemList.Rows[rowId].Cells["dgItemList_weight"].Style.BackColor = Color.GreenYellow;
            }
            else
            {
                dgSrcItemList.Rows[rowId].Cells["dgItemList_weight"].Style.BackColor = Color.Orange;
            }

            string countryCode = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();

            //판매국가의 환율
            decimal rateSrc = txt_src_currency_rate.Value;

            if (sourcePrice > 0 && margin >= 0 && productWeight > 0)
            {
                PriceCalculator pCalc = new PriceCalculator();
                pCalc.CountryCode = TarCountry;
                pCalc.SourcePrice = sourcePrice;
                pCalc.Margin = margin;
                pCalc.Weight = productWeight;
                pCalc.CurrencyRate = txt_src_currency_rate.Value;
                pCalc.ShopeeRate = UdShopeeFee.Value;
                pCalc.PgFeeRate = udPGFee.Value;
                pCalc.RetailPriceRate = UdRetailPriceRate.Value;

                Dictionary<string, decimal> calcResult = new Dictionary<string, decimal>();
                calcResult = pCalc.calculatePrice();

                dgSrcItemList.Rows[rowId].Cells["dgItemList_pg_fee"].Value = string.Format("{0:n2}", calcResult["pgFee"]);
                dgSrcItemList.Rows[rowId].Cells["dgItemList_price"].Value = string.Format("{0:n1}", calcResult["targetSellPrice"]);
                dgSrcItemList.Rows[rowId].Cells["dgItemList_price_won"].Value = string.Format("{0:n0}", calcResult["targetSellPriceKRW"]);
                dgSrcItemList.Rows[rowId].Cells["dgItemList_original_price"].Value = string.Format("{0:n1}", calcResult["targetRetailPrice"]);


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
                }

                if (calcResult["pgFee"] > 0)
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_pg_fee"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_pg_fee"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetSellPrice"] > 0)
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_price"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetSellPriceKRW"] > 0)
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_price_won"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_price_won"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetRetailPrice"] > 0)
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_original_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_original_price"].Style.BackColor = Color.Orange;
                }

                //마지막으로 입력한 값이 숫자니까 콤마 찍어주기
                string strVar = dgSrcItemList.Rows[rowId].Cells[cellName].Value.ToString().Replace(",","");
                decimal Var = 0;
                if (decimal.TryParse(strVar, out Var))
                {
                    if(cellName == "dgItemList_weight")
                    {
                        dgSrcItemList.Rows[rowId].Cells[cellName].Value = string.Format("{0:n2}", Var);
                    }
                    else
                    {
                        dgSrcItemList.Rows[rowId].Cells[cellName].Value = string.Format("{0:n0}", Var);
                    }
                }

                if (sourcePrice > 0)
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_supply_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_supply_price"].Style.BackColor = Color.Orange;
                }

                if (margin > 0)
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_margin"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_margin"].Style.BackColor = Color.Orange;
                }

            }
            isChanging = false;
        }

        private void calcRowPriceFromSellPrice(int rowId, string cellName)
        {
            isChanging = true;
            //반대로 판가를 넣으면 얼마의 마진을 먹는지 계산
            string TarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();            
            int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.Rows[rowId].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
            decimal sourcePrice = Convert.ToDecimal(dgSrcItemList.Rows[rowId].Cells["dgItemList_supply_price"].Value.ToString().Replace(",",""));
            double productWeight = Convert.ToDouble(dgSrcItemList.Rows[rowId].Cells["dgItemList_weight"].Value.ToString().Replace(",", ""));
            decimal targetSellPrice = Convert.ToDecimal(dgSrcItemList.Rows[rowId].Cells["dgItemList_price"].Value.ToString().Replace(",", ""));
            

            if (productWeight > 0)
            {
                dgSrcItemList.Rows[rowId].Cells["dgItemList_weight"].Style.BackColor = Color.GreenYellow;
            }
            else
            {
                dgSrcItemList.Rows[rowId].Cells["dgItemList_weight"].Style.BackColor = Color.Orange;
            }

            string countryCode = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();

            //판매국가의 환율
            decimal rateSrc = txt_src_currency_rate.Value;

            if (sourcePrice > 0 && productWeight > 0 && targetSellPrice > 0)
            {
                PriceCalculator pCalc = new PriceCalculator();
                pCalc.CountryCode = TarCountry;
                pCalc.SourcePrice = sourcePrice;
                pCalc.Margin = 0;
                pCalc.Weight = productWeight;
                pCalc.CurrencyRate = txt_src_currency_rate.Value;
                pCalc.ShopeeRate = UdShopeeFee.Value;
                pCalc.PgFeeRate = udPGFee.Value;
                pCalc.RetailPriceRate = UdRetailPriceRate.Value;
                pCalc.SellPrice = targetSellPrice;

                Dictionary<string, decimal> calcResult = new Dictionary<string, decimal>();
                calcResult = pCalc.calculatePriceFromSellPrice();

                dgSrcItemList.Rows[rowId].Cells["dgItemList_pg_fee"].Value = string.Format("{0:n2}", calcResult["pgFee"]);
                dgSrcItemList.Rows[rowId].Cells["dgItemList_price"].Value = string.Format("{0:n1}", calcResult["targetSellPrice"]);
                dgSrcItemList.Rows[rowId].Cells["dgItemList_price_won"].Value = string.Format("{0:n0}", calcResult["targetSellPriceKRW"]);
                dgSrcItemList.Rows[rowId].Cells["dgItemList_original_price"].Value = string.Format("{0:n1}", calcResult["targetRetailPrice"]);
                dgSrcItemList.Rows[rowId].Cells["dgItemList_margin"].Value = string.Format("{0:n0}", calcResult["targetMarginKRW"]);
                
                //저장해 준다.
                using (AppDbContext context = new AppDbContext())
                {
                    ItemInfoDraft result = context.ItemInfoDrafts.SingleOrDefault(
                            b => b.Id == ItemInfoDraftId
                            && b.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.weight = productWeight;
                        result.supply_price = sourcePrice;
                        result.margin = calcResult["targetMarginKRW"];
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
                }

                if (calcResult["pgFee"] > 0)
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_pg_fee"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_pg_fee"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetSellPrice"] > 0)
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_price"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetSellPriceKRW"] > 0)
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_price_won"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_price_won"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetRetailPrice"] > 0)
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_original_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_original_price"].Style.BackColor = Color.Orange;
                }

                //마지막으로 입력한 값이 숫자니까 콤마 찍어주기
                string strVar = dgSrcItemList.Rows[rowId].Cells[cellName].Value.ToString().Replace(",", "");
                decimal Var = 0;
                if (decimal.TryParse(strVar, out Var))
                {
                    if(TarCountry == "SG")
                    {
                        dgSrcItemList.Rows[rowId].Cells[cellName].Value = string.Format("{0:n1}", Var);
                    }
                    else if(TarCountry == "ID")
                    {
                        dgSrcItemList.Rows[rowId].Cells[cellName].Value = string.Format("{0:n0}", Var);
                    }
                    else if(TarCountry == "MY")
                    {
                        dgSrcItemList.Rows[rowId].Cells[cellName].Value = string.Format("{0:n0}", Var);
                    }
                    else if(TarCountry == "TH")
                    {
                        dgSrcItemList.Rows[rowId].Cells[cellName].Value = string.Format("{0:n0}", Var);
                    }
                    else if (TarCountry == "TW")
                    {
                        dgSrcItemList.Rows[rowId].Cells[cellName].Value = string.Format("{0:n0}", Var);
                    }
                    else if (TarCountry == "VN")
                    {
                        dgSrcItemList.Rows[rowId].Cells[cellName].Value = string.Format("{0:n0}", Var);
                    }
                    else if (TarCountry == "PH")
                    {
                        dgSrcItemList.Rows[rowId].Cells[cellName].Value = string.Format("{0:n0}", Var);
                    }

                }

                if (sourcePrice > 0)
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_supply_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_supply_price"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetMarginKRW"] > 0)
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_margin"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgSrcItemList.Rows[rowId].Cells["dgItemList_margin"].Style.BackColor = Color.Orange;
                }
            }
            isChanging = false;
        }
        public void Progress(CancellationTokenSource cts)
        {
            try
            {
                if (!cts.IsCancellationRequested) progressBar.Value++;
            }
            catch
            {
                // ignored
            }
        }
        private void BtnGetProductList_Click(object sender, EventArgs e)
        {
            getProductList("", "");
        }

        private void dg_site_id_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dgSrcItemList.Rows.Clear();
            Application.DoEvents();
            string countryCode = "";

            if(dg_site_id.Rows.Count > 0)
            {
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                if (shopeeId.ToUpper().Contains(".ID"))
                {
                    countryCode = "IDR";
                }
                else if (shopeeId.ToUpper().Contains(".MY"))
                {
                    countryCode = "MYR";
                }
                else if (shopeeId.ToUpper().Contains(".SG"))
                {
                    countryCode = "SGD";
                }
                else if (shopeeId.ToUpper().Contains(".TW"))
                {
                    countryCode = "TWD";
                }
                else if (shopeeId.ToUpper().Contains(".TH"))
                {
                    countryCode = "THB";
                }
                else if (shopeeId.ToUpper().Contains(".PH"))
                {
                    countryCode = "PHP";
                }
                else if (shopeeId.ToUpper().Contains(".VN"))
                {
                    countryCode = "VND";
                }

                for (int i = 0; i < cbo_currency_From.Items.Count; i++)
                {
                    KeyValuePair<string, decimal> currency_info = (KeyValuePair<string, decimal>)cbo_currency_From.Items[i];
                    if (currency_info.Key.ToString().Contains(countryCode))
                    {
                        cbo_currency_From.SelectedIndex = i;
                        //txt_tar_currency_rate.Text = currency_info.Value.ToString();
                        //txt_src_currency_rate.Text = currency_info.Value.ToString();

                        //DB에서 가지고 온다.
                        using (AppDbContext context = new AppDbContext())
                        {
                            var rate = context.CurrencyRates.FirstOrDefault(x => x.cr_name.Contains(countryCode) && x.UserId == global_var.userId);

                            if (rate != null)
                            {
                                txt_src_currency_rate.Value = rate.cr_exchange;
                            }
                        }

                        break;
                    }
                }


                getPromotionList();
                getLogisticList();
            }
            
            Cursor.Current = Cursors.WaitCursor;
        }
        private void getPromotionList()
        {
            dg_shopee_discount.Rows.Clear();
            string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
            using (AppDbContext context = new AppDbContext())
            {
                List<Promotion> promotionList = context.Promotions
                    .Where(x => x.shopeeAccount == shopeeId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.discount_name).ToList();

                if (promotionList.Count > 0)
                {
                    for (int i = 0; i < promotionList.Count; i++)
                    {
                        dg_shopee_discount.Rows.Add(i + 1,
                            false,
                            promotionList[i].discount_name,
                            promotionList[i].discount_id,
                            promotionList[i].start_time.ToString("yyyy-MM-dd"),
                            promotionList[i].end_time.ToString("yyyy-MM-dd"),
                            promotionList[i].status);
                    }
                }
            }
        }

        private void MainMenu_View_Product_Page_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
            {
                string siteUrl = "";
                string currency = dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString();
                string shop_id = dgSrcItemList.SelectedRows[0].Cells["dgItemList_shopid"].Value.ToString();
                string goods_no = dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString();

                if (currency == "SGD")
                {
                    siteUrl = "https://shopee.sg/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "MYR")
                {
                    siteUrl = "https://shopee.com.my/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "IDR")
                {
                    siteUrl = "https://shopee.co.id/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "THB")
                {
                    siteUrl = "https://shopee.co.th/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "TWD")
                {
                    siteUrl = "https://shopee.tw/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "PHP")
                {
                    siteUrl = "https://shopee.ph/product/" + shop_id + "/" + goods_no;
                }
                else if (currency == "VND")
                {
                    siteUrl = "https://shopee.vn/product/" + shop_id + "/" + goods_no;
                }
                System.Diagnostics.Process.Start("chrome.exe", siteUrl);
            }
        }

        private void MainMenu_DeleteItem_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }
            DialogResult dlg_Result = MessageBox.Show("체크한 상품을 상품등록기에서 삭제 하시겠습니까?", "상품 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                grp_src.Select();
                dgSrcItemList.EndEdit();
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        long srcProductId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                        string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.src_item_id == srcProductId &&
                                b.tar_shopeeAccount == shopeeId
                                && b.UserId == global_var.userId);

                            //원본데이터가 DB에 존재하여야 한다.
                            if (result_src_data != null)
                            {
                                context.ItemInfoDrafts.Remove(result_src_data);
                                context.SaveChanges();
                            }
                        }
                    }
                }
                BtnGetProductList_Click(null, null);
                Cursor.Current = Cursors.Default;
                MessageBox.Show("상품을 모두 삭제하였습니다.","상품 삭제", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void dgSrcItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 1)
            {
                if (dgSrcItemList.Rows.Count > 0)
                {
                    bool chk = (bool)dgSrcItemList.Rows[0].Cells["dgItemList_Chk"].Value;
                    for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                    {
                        dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value = !chk;
                    }

                    grp_src.Select();
                }
            }
            else if (e.RowIndex > -1)
            {
                if(e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6
                    || e.ColumnIndex == 7 || e.ColumnIndex == 8 || e.ColumnIndex == 9)
                {
                    if(dgSrcItemList.SelectedRows.Count > 0)
                    {
                        string temp = dgSrcItemList.SelectedRows[0].Cells[e.ColumnIndex].Value? .ToString() ?? "";
                        if (temp.Trim() != string.Empty)
                        {
                            Clipboard.SetText(temp);
                        }
                    }
                }

                if (e.ColumnIndex == 15 || e.ColumnIndex == 17 || e.ColumnIndex == 18)
                {
                    if(e.ColumnIndex == 15)
                    {
                        metroLabel2.Text = "PG수수료";
                    }

                    if (e.ColumnIndex == 17)
                    {
                        metroLabel2.Text = "판매가";
                    }

                    if (e.ColumnIndex == 18)
                    {
                        metroLabel2.Text = "소비자가";
                    }
                    decimal rateSrc = 0;
                    decimal SrcPrice = 0;

                    if(dgSrcItemList.SelectedRows.Count > 0)
                    {
                        SrcPrice = Convert.ToDecimal(dgSrcItemList.SelectedRows[0].Cells[e.ColumnIndex].Value.ToString().Replace(",", ""));
                        string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                        string currencyDigit = "";
                        string countryCode = "";

                        if (shopeeId.ToUpper().Contains(".ID"))
                        {
                            currencyDigit = "{0:n0}";
                            countryCode = "IDR";
                        }
                        else if (shopeeId.ToUpper().Contains(".MY"))
                        {
                            currencyDigit = "{0:n0}";
                            countryCode = "MYR";
                        }
                        else if (shopeeId.ToUpper().Contains(".SG"))
                        {
                            currencyDigit = "{0:n0}";
                            countryCode = "SGD";
                        }
                        else if (shopeeId.ToUpper().Contains(".TW"))
                        {
                            currencyDigit = "{0:n0}";
                            countryCode = "TWD";
                        }
                        else if (shopeeId.ToUpper().Contains(".TH"))
                        {
                            currencyDigit = "{0:n0}";
                            countryCode = "THB";
                        }
                        else if (shopeeId.ToUpper().Contains(".PH"))
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
            }
        }

        private void btnSaveMargin_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }

            DialogResult dlg_Result = MessageBox.Show("체크한 상품의 마진을 모두 적용하시겠습니까?", "마진 일괄 적용", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        long srcProductId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                        string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.src_item_id == srcProductId &&
                                b.tar_shopeeAccount == shopeeId
                                && b.UserId == global_var.userId);

                            //원본데이터가 DB에 존재하여야 한다.
                            if (result_src_data != null)
                            {
                                result_src_data.margin = UdMargin.Value;
                                context.SaveChanges();
                                dgSrcItemList.Rows[i].Cells["dgItemList_margin"].Value = string.Format("{0:n0}", UdMargin.Value);
                            }
                        }
                    }
                }
                CalcPrice();
                Cursor.Current = Cursors.Default;
                MessageBox.Show("체크한 상품의 마진을 모두 입력하였습니다.", "마진 입력", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
                
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

                if (dg_site_id.Rows.Count > 0 && dg_site_id.SelectedRows.Count > 0)
                {
                    DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(0, dg_site_id.SelectedRows[0].Index);
                    dg_site_id_CellClick(null, et);
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("환율 정보를 업데이트 하였습니다.", "환율정보 업데이트", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void CalcPrice()
        {
            string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
            for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
            {
                if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                {
                    calcRowPrice(i, "dgItemList_supply_price");
                }
            }
        }
        private void BtnCalcSellPrice_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }

            if (dg_site_id.SelectedRows.Count > 0)
            {
                DialogResult dlg_Result = MessageBox.Show("체크한 상품의 판매가를 계산하시겠습니까?", "판가 계산", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg_Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    progressBar.Value = 0;
                    progressBar.Maximum = dgSrcItemList.Rows.Count;
                    CalcPrice();
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("체크한 상품의 가격을 모두 계산하였습니다.", "가격 계산", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }   
        }

        private void BtnApplyQty_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }
            DialogResult dlg_Result = MessageBox.Show("체크한 상품의 수량을 적용하시겠습니까?", "수량 일괄 적용", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                progressBar.Value = 0;
                progressBar.Maximum = dgSrcItemList.Rows.Count;
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        long srcProductId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());                        
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.src_item_id == srcProductId &&
                                b.tar_shopeeAccount == shopeeId
                                && b.UserId == global_var.userId);

                            //원본데이터가 DB에 존재하여야 한다.
                            if (result_src_data != null)
                            {
                                result_src_data.stock = Convert.ToInt32(UdQty.Value);
                                context.SaveChanges();
                                dgSrcItemList.Rows[i].Cells["dgItemList_stock"].Value = string.Format("{0:n0}", UdQty.Value);
                                dgSrcItemList.Rows[i].Cells["dgItemList_stock"].Style.BackColor = Color.GreenYellow;
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("수량을 모두 적용하였습니다.","수량적용",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void viewNaverExchange_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string siteUrl = "https://finance.naver.com/marketindex/?tabSel=exchange#tab_section";
            System.Diagnostics.Process.Start("chrome.exe", siteUrl);
            Cursor.Current = Cursors.Default;
        }

        private void UdFixedShippingFee_ValueChanged(object sender, EventArgs e)
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

        private void udSellingFee_ValueChanged(object sender, EventArgs e)
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
        }

        private void BtnSearchProduct_Click(object sender, EventArgs e)
        {
            if (CboSearchType.Text == "상품ID")
            {
                long ItemId = 0;
                if (TxtSearchProduct.Text.Trim() != string.Empty)
                {
                    if (long.TryParse(TxtSearchProduct.Text.Trim(), out ItemId))
                    {
                        getProductList("상품ID", TxtSearchProduct.Text.Trim());
                    }
                }

                //for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                //{
                //    if (dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString().Contains(TxtSearchProduct.Text.Trim()))
                //    {
                //        dgSrcItemList.Rows[i].Selected = true;
                //        dgSrcItemList.FirstDisplayedScrollingRowIndex = i;
                //        dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value = true;
                //    }
                //}
            }
            else if (CboSearchType.Text == "상품SKU")
            {
                getProductList("상품SKU", TxtSearchProduct.Text.Trim());
            }
            else if (CboSearchType.Text == "상품명")
            {
                getProductList("상품명", TxtSearchProduct.Text.Trim());
            }
        }

        private void TxtSearchProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                BtnSearchProduct_Click(null, null);
            }
        }

        private void MainMenu_check_all_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count > 0)
            {
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value = true;
                }

                grp_src.Select();
            }
        }

        private void MainMenu_uncheck_all_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count > 0)
            {
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value = false;
                }

                grp_src.Select();
            }
        }

        private void MainMenu_calcPrice_Click(object sender, EventArgs e)
        {
            BtnCalcSellPrice_Click(null, null);
        }

        private void MainMenu_Save_margin_Click(object sender, EventArgs e)
        {
            btnSaveMargin_Click(null,null);
        }

        private void MainMenu_save_checked_qty_Click(object sender, EventArgs e)
        {
            BtnApplyQty_Click(null, null);
        }

        private void tab_detail_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            TabPage tp = tab_detail.TabPages[e.Index];

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;  //optional

            // This is the rectangle to draw "over" the tabpage title
            RectangleF headerRect = new RectangleF(e.Bounds.X, e.Bounds.Y + 6, e.Bounds.Width, e.Bounds.Height - 2);

            // This is the default colour to use for the non-selected tabs
            SolidBrush sb = new SolidBrush(Color.Transparent);

            // This changes the colour if we're trying to draw the selected tabpage
            if (tab_detail.SelectedIndex == e.Index)
                sb.Color = Color.FromArgb(204, 244, 255);

            // Colour the header of the current tabpage based on what we did above
            g.FillRectangle(sb, e.Bounds);

            //Remember to redraw the text - I'm always using black for title text
            g.DrawString(tp.Text, tab_detail.Font, new SolidBrush(Color.Black), headerRect, sf);
        }

        private void BtnApplyPromotion_Click(object sender, EventArgs e)
        {
            if(dgSrcItemList.Rows.Count == 0)
            {
                return;
            }
            DialogResult dlg_Result = MessageBox.Show("체크한 상품의 할인 프로모션 코드를 모두 적용하시겠습니까?", "프로모션 일괄 적용", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                if(dg_shopee_discount.Rows.Count > 0 && dg_shopee_discount.SelectedRows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    int discountID = Convert.ToInt32(dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_id"].Value.ToString());
                    string discountName = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_name"].Value.ToString();
                    for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                    {
                        if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                        {
                            int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                            using (AppDbContext context = new AppDbContext())
                            {
                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                    b => b.Id == ItemInfoDraftId
                                    && b.UserId == global_var.userId);

                                //원본데이터가 DB에 존재하여야 한다.
                                if (result_src_data != null)
                                {
                                    result_src_data.discount_id = discountID;
                                    result_src_data.discount_name = discountName;
                                    context.SaveChanges();
                                    dgSrcItemList.Rows[i].Cells["dgItemList_discount_id"].Value = discountID;
                                    dgSrcItemList.Rows[i].Cells["dgItemList_discount_name"].Value = discountName;
                                    dgSrcItemList.Rows[i].Cells["dgItemList_discount_id"].Style.BackColor = Color.GreenYellow;
                                }
                            }
                        }
                    }
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("체크한 상품의 할인ID를 모두 적용하였습니다.", "할인코드 적용", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnRemovePromotion_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }

            DialogResult dlg_Result = MessageBox.Show("체크한 상품의 할인 프로모션 코드를 모두 제거하시겠습니까?", "프로모션 일괄 제거", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                int discountID = Convert.ToInt32(dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_id"].Value.ToString());
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.Id == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                            //원본데이터가 DB에 존재하여야 한다.
                            if (result_src_data != null)
                            {
                                result_src_data.discount_id = 0;
                                result_src_data.discount_name = "";
                                context.SaveChanges();
                                dgSrcItemList.Rows[i].Cells["dgItemList_discount_id"].Value = "";
                                dgSrcItemList.Rows[i].Cells["dgItemList_discount_name"].Value = "";
                                dgSrcItemList.Rows[i].Cells["dgItemList_discount_id"].Style.BackColor = Color.White;
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("체크한 상품의 할인ID를 모두 제거하였습니다.", "할인코드 제거", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dg_hf_category_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dg_hf_category.SelectedRows.Count > 0)
            {
                txt_template_name.Text = "";
                txt_hf_category_name.Text = dg_hf_category.SelectedRows[0].Cells["dg_hf_category_name"].Value.ToString();
                int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());
                FillHfList(HFTypeID);
            }
        }

        private void btn_hf_category_add_Click(object sender, EventArgs e)
        {
            if (txt_hf_category_name.Text.Trim() == string.Empty)
            {
                return;
            }
            DialogResult Result = MessageBox.Show("헤더/푸터 종류 정보를 추가 하시겠습니까?", "헤더/푸터 종류 정보 추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                if (txt_hf_category_name.Text.Trim() != string.Empty)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        HFType newType = new HFType
                        {
                            HFTypeName = txt_hf_category_name.Text.Trim(),
                            UserId = global_var.userId
                        };
                        context.HFTypes.Add(newType);
                        context.SaveChanges();
                        txt_hf_category_name.Text = "";
                    }
                    FillHfType();
                }
            }
        }

        private void btn_hf_category_edit_Click(object sender, EventArgs e)
        {
            if(dg_hf_category.Rows.Count > 0 && dg_hf_category.SelectedRows.Count > 0)
            {
                DialogResult Result = MessageBox.Show("헤더/푸터 종류 정보를 업데이트 하시겠습니까?", "헤더/푸터 종류 정보 업데이트", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());

                        if (HFTypeID != 0)
                        {
                            HFType result = context.HFTypes.SingleOrDefault(
                                   b => b.HFTypeID == HFTypeID
                                   && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                result.HFTypeName = txt_hf_category_name.Text.Trim();
                                context.SaveChanges();
                            }
                        }
                    }

                    FillHfType();

                }
            }
            
        }

        private void btn_hf_category_delete_Click(object sender, EventArgs e)
        {
            if (dg_hf_category.Rows.Count > 0 && dg_hf_category.SelectedRows.Count > 0)
            {
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                if (shopeeId != string.Empty)
                {
                    DialogResult Result = MessageBox.Show("헤더/푸터 종류 정보를 삭제 하시겠습니까?\r\n삭제시에는 포함되어 있는 모든 템플릿이 삭제됩니다.",
                    "헤더/푸터 종류 정보 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (Result == DialogResult.Yes)
                    {
                        int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());

                        if (HFTypeID != 0)
                        {
                            using (AppDbContext context = new AppDbContext())
                            {
                                HFType result = context.HFTypes.SingleOrDefault(b => b.HFTypeID == HFTypeID
                                && b.UserId == global_var.userId);

                                if (result != null)
                                {
                                    context.HFTypes.Remove(result);
                                    context.SaveChanges();
                                }
                            }

                            FillHfType();
                        }
                    }
                }
            }   
        }
        private void FillTemplateHeader(int SetHeaderId)
        {
            dg_list_header.Rows.Clear();
            TxtHFContent.Text = "";
            using (AppDbContext context = new AppDbContext())
            {
                var lst = (from template in context.TemplateHeaders
                           join H in context.HFLists
                           on template.HFListID equals H.HFListID
                           where template.SetHeaderId == SetHeaderId && template.UserId == global_var.userId
                           orderby template.OrderIdx
                           select new
                           {
                               TemplateId = template.Id,
                               TemplateName = H.HFName,
                               HFListId = H.HFListID,                               
                           }).ToList();

                //List<TemplateHeader> headerList = context.TemplateHeaders
                //    .OrderBy(x => x.OrderIdx).ToList();

                for (int i = 0; i < lst.Count; i++)
                {
                    dg_list_header.Rows.Add(lst[i].TemplateId, i + 1, false, lst[i].TemplateName,
                        lst[i].HFListId);
                }
            }
        }
        private void btn_html_set_Click(object sender, EventArgs e)
        {
            if (txt_template_name.Text.Trim() == string.Empty)
            {
                return;
            }
            DialogResult Result = MessageBox.Show("헤더/푸터 종류 정보를 추가 하시겠습니까?", "헤더/푸터 종류 정보 추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                if (txt_template_name.Text.Trim() != string.Empty)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());
                        HFList newType = new HFList
                        {
                            HFName = txt_template_name.Text.Trim(),
                            HFContent = TxtHFContent.Text,
                            HFTypeID = HFTypeID,
                            UserId = global_var.userId
                        };
                        context.HFLists.Add(newType);
                        context.SaveChanges();
                        txt_template_name.Text = "";
                        FillHfList(HFTypeID);
                    }
                }
            }
        }

        private void btn_rename_template_name_Click(object sender, EventArgs e)
        {
            if(dg_hf_list.Rows.Count > 0 && dg_hf_list.SelectedRows.Count > 0)
            {
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();

                if (shopeeId != string.Empty)
                {
                    DialogResult Result = MessageBox.Show("템플릿명 및 템플릿 내용을 업데이트 하시겠습니까?", "템플릿 업데이트", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (Result == DialogResult.Yes)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            int HFListID = Convert.ToInt32(dg_hf_list.SelectedRows[0].Cells["dg_hf_list_idx"].Value.ToString());
                            int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());

                            if (HFListID != 0)
                            {
                                HFList result = context.HFLists.SingleOrDefault(
                                    b => b.HFListID == HFListID
                                    && b.UserId == global_var.userId);

                                if (result != null)
                                {
                                    result.HFName = txt_template_name.Text.Trim();
                                    context.SaveChanges();
                                }

                                FillHfList(HFTypeID);
                                //FillTemplateHeader(shopeeId);
                            }
                        }
                    }
                }
            }

            
        }

        private void btn_delete_template_Click(object sender, EventArgs e)
        {
            if (dg_hf_list.Rows.Count > 0 && dg_hf_list.SelectedRows.Count > 0)
            {
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                if (shopeeId != string.Empty)
                {
                    DialogResult Result = MessageBox.Show("템플릿 정보를 삭제 하시겠습니까?",
                    "템플릿 정보 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (Result == DialogResult.Yes)
                    {
                        int HFListID = Convert.ToInt32(dg_hf_list.SelectedRows[0].Cells["dg_hf_list_idx"].Value.ToString());
                        int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());

                        if (HFListID != 0)
                        {
                            using (AppDbContext context = new AppDbContext())
                            {
                                HFList result = context.HFLists.SingleOrDefault(b => b.HFListID == HFListID
                                && b.UserId == global_var.userId);

                                if (result != null)
                                {
                                    context.HFLists.Remove(result);
                                    context.SaveChanges();
                                }
                            }
                            txt_template_name.Text = "";
                            FillHfList(HFTypeID);
                            //FillTemplateHeader(shopeeId);
                            //FillTemplateFooter(shopeeId);

                        }
                    }
                }
            }   
        }
        private void FillTemplateFooter(int SetFooterId)
        {
            dg_list_footer.Rows.Clear();
            TxtHFContent.Text = "";
            using (AppDbContext context = new AppDbContext())
            {
                var lst = (from template in context.TemplateFooters
                           join H in context.HFLists
                           on template.HFListID equals H.HFListID
                           where template.SetFooterId == SetFooterId && template.UserId == global_var.userId
                           orderby template.OrderIdx
                           select new
                           {
                               TemplateId = template.Id,
                               TemplateName = H.HFName,
                               HFListId = H.HFListID
                           }).ToList();

                for (int i = 0; i < lst.Count; i++)
                {
                    dg_list_footer.Rows.Add(lst[i].TemplateId, i + 1, false, lst[i].TemplateName,
                        lst[i].HFListId);
                }
            }
        }
        private void btn_up_header_Click(object sender, EventArgs e)
        {
            if (dg_list_header.SelectedRows.Count > 0 && dg_list_header.SelectedRows[0].Index > 0)
            {
                //순서를 변경한다.
                int pre_idx = dg_list_header.SelectedRows[0].Index - 1;
                int cur_idx = dg_list_header.SelectedRows[0].Index;

                DataGridViewRow dr_cur = dg_list_header.Rows[dg_list_header.SelectedRows[0].Index];
                dg_list_header.Rows.RemoveAt(dg_list_header.SelectedRows[0].Index);
                dg_list_header.Rows.Insert(pre_idx, dr_cur);
                dg_list_header.Rows[pre_idx].Selected = true;

                for (int i = 0; i < dg_list_header.Rows.Count; i++)
                {
                    dg_list_header.Rows[i].Cells["dg_list_header_no"].Value = i + 1;
                    int idx = Convert.ToInt32(dg_list_header.Rows[i].Cells["dg_list_header_idx"].Value.ToString());
                    if (idx != 0)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            TemplateHeader result = context.TemplateHeaders.SingleOrDefault(
                                    b => b.Id == idx
                                    && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                result.OrderIdx = i;
                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        private void btn_dn_header_Click(object sender, EventArgs e)
        {
            if (dg_list_header.SelectedRows.Count > 0 && dg_list_header.SelectedRows[0].Index < dg_list_header.Rows.Count - 1)
            {
                //순서를 변경한다.

                int cur_idx = dg_list_header.SelectedRows[0].Index;
                int next_idx = dg_list_header.SelectedRows[0].Index + 1;

                DataGridViewRow dr_cur = dg_list_header.Rows[cur_idx];
                DataGridViewRow dr_next = dg_list_header.Rows[next_idx];
                dg_list_header.Rows.RemoveAt(dg_list_header.SelectedRows[0].Index);
                dg_list_header.Rows.Insert(next_idx, dr_cur);
                dg_list_header.Rows[next_idx].Selected = true;

                for (int i = 0; i < dg_list_header.Rows.Count; i++)
                {
                    dg_list_header.Rows[i].Cells["dg_list_header_no"].Value = i + 1;
                    int idx = Convert.ToInt32(dg_list_header.Rows[i].Cells["dg_list_header_idx"].Value.ToString());
                    if (idx != 0)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            TemplateHeader result = context.TemplateHeaders.SingleOrDefault(
                                    b => b.Id == idx
                                    && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                result.OrderIdx = i;
                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        private void btn_delete_added_header_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("선택한 템플릿을 헤더에서 삭제 하시겠습니까?", "템플릿 헤더에서 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                if (dg_list_header.SelectedRows.Count > 0)
                {
                    int idx = Convert.ToInt32(dg_list_header.SelectedRows[0].Cells["dg_list_header_idx"].Value.ToString());
                    if (idx != 0)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            TemplateHeader result = context.TemplateHeaders.SingleOrDefault(b => b.Id == idx
                            && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                context.TemplateHeaders.Remove(result);
                                context.SaveChanges();
                            }
                        }

                        dg_list_header.Rows.RemoveAt(dg_list_header.SelectedRows[0].Index);

                        //지운후에는 번호를 다시 인덱싱
                        for (int i = 0; i < dg_list_header.Rows.Count; i++)
                        {
                            dg_list_header.Rows[i].Cells["dg_list_header_no"].Value = i + 1;
                        }

                        MessageBox.Show("선택한 템플릿을 헤더 영역에서 삭제하였습니다.", "템플릿 헤더에서 삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btn_up_footer_Click(object sender, EventArgs e)
        {
            if (dg_list_footer.SelectedRows.Count > 0 && dg_list_footer.SelectedRows[0].Index > 0)
            {
                //순서를 변경한다.
                int pre_idx = dg_list_footer.SelectedRows[0].Index - 1;
                int cur_idx = dg_list_footer.SelectedRows[0].Index;

                DataGridViewRow dr_cur = dg_list_footer.Rows[dg_list_footer.SelectedRows[0].Index];
                dg_list_footer.Rows.RemoveAt(dg_list_footer.SelectedRows[0].Index);
                dg_list_footer.Rows.Insert(pre_idx, dr_cur);
                dg_list_footer.Rows[pre_idx].Selected = true;

                for (int i = 0; i < dg_list_footer.Rows.Count; i++)
                {
                    dg_list_footer.Rows[i].Cells["dg_list_footer_no"].Value = i + 1;
                    int idx = Convert.ToInt32(dg_list_footer.Rows[i].Cells["dg_list_footer_idx"].Value.ToString());
                    if (idx != 0)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            TemplateFooter result = context.TemplateFooters.SingleOrDefault(
                                    b => b.Id == idx
                                    && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                result.OrderIdx = i;
                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        private void btn_dn_footer_Click(object sender, EventArgs e)
        {
            if (dg_list_footer.SelectedRows.Count > 0 && dg_list_footer.SelectedRows[0].Index < dg_list_footer.Rows.Count - 1)
            {
                //순서를 변경한다.

                int cur_idx = dg_list_footer.SelectedRows[0].Index;
                int next_idx = dg_list_footer.SelectedRows[0].Index + 1;

                DataGridViewRow dr_cur = dg_list_footer.Rows[cur_idx];
                DataGridViewRow dr_next = dg_list_footer.Rows[next_idx];
                dg_list_footer.Rows.RemoveAt(dg_list_footer.SelectedRows[0].Index);
                dg_list_footer.Rows.Insert(next_idx, dr_cur);
                dg_list_footer.Rows[next_idx].Selected = true;

                for (int i = 0; i < dg_list_footer.Rows.Count; i++)
                {
                    dg_list_footer.Rows[i].Cells["dg_list_footer_no"].Value = i + 1;
                    int idx = Convert.ToInt32(dg_list_footer.Rows[i].Cells["dg_list_footer_idx"].Value.ToString());
                    if (idx != 0)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            TemplateFooter result = context.TemplateFooters.SingleOrDefault(
                                    b => b.Id == idx
                                    && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                result.OrderIdx = i;
                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        private void btn_delete_added_footer_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("선택한 템플릿을 푸터에서 삭제 하시겠습니까?", "템플릿 푸터에서 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                if (dg_list_footer.SelectedRows.Count > 0)
                {
                    int idx = Convert.ToInt32(dg_list_footer.SelectedRows[0].Cells["dg_list_footer_idx"].Value.ToString());
                    if (idx != 0)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            TemplateFooter result = context.TemplateFooters.SingleOrDefault(b => b.Id == idx
                            && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                context.TemplateFooters.Remove(result);
                                context.SaveChanges();
                            }
                        }

                        dg_list_footer.Rows.RemoveAt(dg_list_footer.SelectedRows[0].Index);

                        //지운후에는 번호를 다시 인덱싱
                        for (int i = 0; i < dg_list_footer.Rows.Count; i++)
                        {
                            dg_list_footer.Rows[i].Cells["dg_list_footer_no"].Value = i + 1;
                        }

                        MessageBox.Show("선택한 템플릿을 푸터 영역에서 삭제하였습니다.", "템플릿 푸터에서 삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void BtnSaveHeaderSeparator_Click(object sender, EventArgs e)
        {
            if (TxtHeaderSeparator.Text.Trim() == TxtFooterSeparator.Text.Trim())
            {
                MessageBox.Show("헤더 구분자와 푸터 구분자가 서로 동일합니다.\r\n글자수를 다르게 하거나 다른 글자를 삽입해 주세요.", "구분자 동일", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult Result = MessageBox.Show("헤더 구분 문자를 저장 하시겠습니까?", "헤더 구분 문자", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                if (TxtHeaderSeparator.Text.Trim() != string.Empty)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        List<HeaderSeparator> result = context.HeaderSeparators.Where(x => x.UserId == global_var.userId).ToList();

                        if (result == null || result.Count == 0)
                        {
                            HeaderSeparator newSF = new HeaderSeparator
                            {
                                HeaderSeparatorString = TxtHeaderSeparator.Text.Trim(),
                                 UserId = global_var.userId
                            };
                            context.HeaderSeparators.Add(newSF);
                        }
                        else
                        {
                            result[0].HeaderSeparatorString = TxtHeaderSeparator.Text.Trim();
                        }

                        context.SaveChanges();
                    }
                }
            }
        }

        private void BtnSaveFooterSeparator_Click(object sender, EventArgs e)
        {
            if (TxtHeaderSeparator.Text.Trim() == TxtFooterSeparator.Text.Trim())
            {
                MessageBox.Show("헤더 구분자와 푸터 구분자가 서로 동일합니다.\r\n글자수를 다르게 하거나 다른 글자를 삽입해 주세요.", "구분자 동일", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult Result = MessageBox.Show("푸터 구분 문자를 저장 하시겠습니까?", "푸터 구분 문자", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                if (TxtFooterSeparator.Text.Trim() != string.Empty)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        List<FooterSeparator> result = context.FooterSeparators.Where(x => x.UserId == global_var.userId).ToList();

                        if (result == null || result.Count == 0)
                        {
                            FooterSeparator newSF = new FooterSeparator
                            {
                                FooterSeparatorString = TxtFooterSeparator.Text.Trim(),
                                UserId = global_var.userId
                            };
                            context.FooterSeparators.Add(newSF);
                        }
                        else
                        {
                            result[0].FooterSeparatorString = TxtFooterSeparator.Text.Trim();
                        }

                        context.SaveChanges();
                    }
                }
            }
        }

        private void dg_hf_list_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TxtHFContent.Text = "";
            txt_template_name.Text = dg_hf_list.SelectedRows[0].Cells["dg_hf_list_template_name"].Value.ToString();

            using (AppDbContext context = new AppDbContext())
            {
                int HFListID = Convert.ToInt32(dg_hf_list.SelectedRows[0].Cells["dg_hf_list_idx"].Value.ToString());
                int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());

                if (HFListID != 0)
                {
                    HFList result = context.HFLists.SingleOrDefault(
                        b => b.HFListID == HFListID
                        && b.UserId == global_var.userId);

                    TxtHFContent.Text = result.HFContent;
                }
            }
        }

        private void btn_add_header_Click(object sender, EventArgs e)
        {
            if(dg_list_header_set.Rows.Count > 0 && dg_list_header_set.SelectedRows.Count > 0 &&
                dg_hf_list.Rows.Count > 0 && dg_hf_list.SelectedRows.Count > 0)
            {
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();

                if (shopeeId == string.Empty)
                {
                    MessageBox.Show("계정을 설정하세요.", "계정 미설정", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DialogResult Result = MessageBox.Show("헤더에 추가 하시겠습니까?", "헤더추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        int HFListID = Convert.ToInt32(dg_hf_list.SelectedRows[0].Cells["dg_hf_list_idx"].Value.ToString());
                        int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());
                        int HeaderSetId = Convert.ToInt32(dg_list_header_set.SelectedRows[0].Cells["dg_headerset_id"].Value.ToString());
                        string HfName = dg_hf_list.SelectedRows[0].Cells["dg_hf_list_template_name"].Value.ToString();

                        TemplateHeader newHeader = new TemplateHeader
                        {
                            HFListID = HFListID,
                            ShopeeAccount = shopeeId,
                            HFName = HfName,
                            OrderIdx = dg_list_header.Rows.Count + 1,
                            SetHeaderId = HeaderSetId,
                            UserId = global_var.userId
                        };

                        context.TemplateHeaders.Add(newHeader);
                        context.SaveChanges();

                        FillTemplateHeader(HeaderSetId);
                    }
                }
            }
            
        }

        private void btn_add_footer_Click(object sender, EventArgs e)
        {
            if (dg_list_footer_set.Rows.Count > 0 && dg_list_footer_set.SelectedRows.Count > 0 &&
                dg_hf_list.Rows.Count > 0 && dg_hf_list.SelectedRows.Count > 0)
            {
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                if (shopeeId == string.Empty)
                {
                    MessageBox.Show("복사 대상국가 및 계정을 설정하세요.", "복사 대상국가 및 계정 미설정", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (shopeeId != string.Empty)
                {
                    DialogResult Result = MessageBox.Show("푸터에 추가 하시겠습니까?", "푸터추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (Result == DialogResult.Yes)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            int HFListID = Convert.ToInt32(dg_hf_list.SelectedRows[0].Cells["dg_hf_list_idx"].Value.ToString());
                            int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());
                            int FooterSetId = Convert.ToInt32(dg_list_footer_set.SelectedRows[0].Cells["dg_footerset_id"].Value.ToString());

                            string HfName = dg_hf_list.SelectedRows[0].Cells["dg_hf_list_template_name"].Value.ToString();

                            TemplateFooter newFooter = new TemplateFooter
                            {
                                HFListID = HFListID,
                                ShopeeAccount = shopeeId,
                                HFName = HfName,
                                OrderIdx = dg_list_footer.Rows.Count + 1,
                                SetFooterId = FooterSetId,
                                UserId = global_var.userId
                            };

                            context.TemplateFooters.Add(newFooter);
                            context.SaveChanges();

                            FillTemplateFooter(FooterSetId);
                        }
                    }
                }
            }   
        }

        private void BtnInitTextBox_Click(object sender, EventArgs e)
        {
            txt_template_name.Text = "";
            TxtHFContent.Text = "";
            txt_template_name.Select();
        }

        private void BtnInitTextBox2_Click(object sender, EventArgs e)
        {
            txt_hf_category_name.Text = "";
            txt_hf_category_name.Select();
        }

        private void BtnHFPreview_Click(object sender, EventArgs e)
        {
            //선택한 헤더세트와 푸터 세트를 모두 가지고 와야 한다.
            string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
            StringBuilder sb_content = new StringBuilder();

            if (dg_list_header_set.Rows.Count > 0 && dg_list_header_set.SelectedRows.Count > 0)
            {
                int HeaderSetId = Convert.ToInt32(dg_list_header_set.SelectedRows[0].Cells["dg_headerset_id"].Value.ToString());
                using (AppDbContext context = new AppDbContext())
                {
                    var lst = (from template in context.TemplateHeaders
                               join H in context.HFLists
                               on template.HFListID equals H.HFListID
                               where template.SetHeaderId == HeaderSetId && template.UserId == global_var.userId
                               orderby template.OrderIdx
                               select new
                               {
                                   TemplateId = template.Id,
                                   TemplateName = H.HFName,
                                   HFListId = H.HFListID
                               }).ToList();


                    for (int i = 0; i < lst.Count; i++)
                    {
                        int tempId = lst[i].HFListId;
                        HFList result = context.HFLists.SingleOrDefault(
                            b => b.HFListID == tempId
                            && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            sb_content.Append(result.HFContent + "\r\n");
                        }
                    }
                }
            }
                

            sb_content.Append(TxtHeaderSeparator.Text + "\r\n");
            sb_content.Append("본문 상품 시작\r\n");
            sb_content.Append("본문 상품 설명\r\n");
            sb_content.Append("본문 상품 종료\r\n");

            sb_content.Append(TxtFooterSeparator.Text + "\r\n");

            if (dg_list_header_set.Rows.Count > 0 && dg_list_header_set.SelectedRows.Count > 0)
            {
                int FooterSetId = Convert.ToInt32(dg_list_footer_set.SelectedRows[0].Cells["dg_footerset_id"].Value.ToString());
                using (AppDbContext context = new AppDbContext())
                {
                    var lst = (from template in context.TemplateFooters
                               join H in context.HFLists
                               on template.HFListID equals H.HFListID
                               where template.SetFooterId == FooterSetId && template.UserId == global_var.userId
                               orderby template.OrderIdx
                               select new
                               {
                                   TemplateId = template.Id,
                                   TemplateName = H.HFName,
                                   HFListId = H.HFListID
                               }).ToList();

                    for (int i = 0; i < lst.Count; i++)
                    {
                        int tempId = lst[i].HFListId;
                        HFList result = context.HFLists.SingleOrDefault(
                            b => b.HFListID == tempId
                            && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            sb_content.Append(result.HFContent + "\r\n");
                        }
                    }
                }
            }
                

            FormPreviewHF fh = new FormPreviewHF();
            fh.TxtHFContent.Text = sb_content.ToString();
            fh.ShowDialog();
        }

        private void dg_list_header_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TxtHFContent.Text = "";
            
            if(dg_list_header.Rows.Count > 0 && dg_list_header.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int HFListID = Convert.ToInt32(dg_list_header.SelectedRows[0].Cells["dg_list_header_HFListId"].Value.ToString());

                    if (HFListID != 0)
                    {
                        HFList result = context.HFLists.SingleOrDefault(
                            b => b.HFListID == HFListID
                            && b.UserId == global_var.userId);
                        if(result != null && result.HFContent != null)
                        {
                            TxtHFContent.Text = result.HFContent;
                        }
                    }
                }
            }
        }

        private void dg_list_footer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TxtHFContent.Text = "";

            if (dg_list_footer.Rows.Count > 0 && dg_list_footer.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int HFListID = Convert.ToInt32(dg_list_footer.SelectedRows[0].Cells["dg_list_footer_HFListId"].Value.ToString());

                    if (HFListID != 0)
                    {
                        HFList result = context.HFLists.SingleOrDefault(
                            b => b.HFListID == HFListID
                            && b.UserId == global_var.userId);

                        if(result != null && result.HFContent != null)
                        {
                            TxtHFContent.Text = result.HFContent;
                        }
                    }
                }
            }
        }

        private void BtnInitTextBox3_Click(object sender, EventArgs e)
        {
            txt_HeaderSet_Name.Text = "";
        }

        private void BtnInitTextBox4_Click(object sender, EventArgs e)
        {
            txt_FooterSet_Name.Text = "";
        }

        private void btn_HeaderSet_add_Click(object sender, EventArgs e)
        {
            if(txt_HeaderSet_Name.Text.Trim() == string.Empty)
            {
                return;
            }
            DialogResult Result = MessageBox.Show("헤더 세트종류를 추가 하시겠습니까?", "헤더 세트종류 추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                if (txt_HeaderSet_Name.Text.Trim() != string.Empty)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        SetHeader newHeaderSet = new SetHeader
                        {
                            SetHeaderName = txt_HeaderSet_Name.Text.Trim(),
                            UserId = global_var.userId
                        };
                        context.SetHeaders.Add(newHeaderSet);
                        context.SaveChanges();
                        txt_HeaderSet_Name.Text = "";
                    }
                    FillHeaderSet();
                }
            }
        }

        private void btn_HeaderSet_edit_Click(object sender, EventArgs e)
        {
            if (dg_list_header_set.Rows.Count > 0 && dg_list_header_set.SelectedRows.Count > 0)
            {
                DialogResult Result = MessageBox.Show("헤더 세트명을 업데이트 하시겠습니까?", "헤더 세트명 업데이트", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        int HeaderSetId = Convert.ToInt32(dg_list_header_set.SelectedRows[0].Cells["dg_headerset_id"].Value.ToString());

                        if (HeaderSetId != 0)
                        {
                            SetHeader result = context.SetHeaders.SingleOrDefault(
                                   b => b.Id == HeaderSetId
                                   && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                result.SetHeaderName = txt_HeaderSet_Name.Text.Trim();
                                context.SaveChanges();
                            }
                        }
                    }

                    FillHeaderSet();
                }
            }   
        }

        private void btn_HeaderSet_delete_Click(object sender, EventArgs e)
        {
            if (dg_list_header_set.Rows.Count > 0 && dg_list_header_set.SelectedRows.Count > 0 &&
                dg_site_id.Rows.Count > 0 && dg_site_id.SelectedRows.Count > 0)
            {
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                if (shopeeId != string.Empty)
                {
                    DialogResult Result = MessageBox.Show("헤더 세트를 삭제 하시겠습니까?\r\n삭제시에는 포함되어 있는 모든 템플릿이 삭제됩니다.",
                    "헤더 세트 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (Result == DialogResult.Yes)
                    {
                        int HeaderSetId = Convert.ToInt32(dg_list_header_set.SelectedRows[0].Cells["dg_headerset_id"].Value.ToString());

                        if (HeaderSetId != 0)
                        {
                            using (AppDbContext context = new AppDbContext())
                            {
                                SetHeader result = context.SetHeaders.SingleOrDefault(b => b.Id == HeaderSetId
                                && b.UserId == global_var.userId);

                                if (result != null)
                                {
                                    context.SetHeaders.Remove(result);
                                    context.SaveChanges();
                                    txt_HeaderSet_Name.Text = "";
                                }
                            }

                            FillHeaderSet();
                            //FillTemplateHeader(shopeeId);
                            //FillTemplateFooter(shopeeId);
                        }
                    }
                }
            }   
        }

        private void dg_list_header_set_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_HeaderSet_Name.Text = dg_list_header_set.SelectedRows[0].Cells["dg_headerset_name"].Value.ToString();

            if (dg_list_header_set.Rows.Count > 0 && dg_list_header_set.SelectedRows.Count > 0)
            {
                int SetHeaderId = Convert.ToInt32(dg_list_header_set.SelectedRows[0].Cells["dg_headerset_id"].Value.ToString());
                FillTempleteHeader(SetHeaderId);
            }
        }

        private void btn_FooterSet_add_Click(object sender, EventArgs e)
        {
            if (txt_FooterSet_Name.Text.Trim() == string.Empty)
            {
                return;
            }
            DialogResult Result = MessageBox.Show("푸터 세트종류를 추가 하시겠습니까?", "푸터 세트종류 추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                if (txt_FooterSet_Name.Text.Trim() != string.Empty)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        SetFooter newFooterSet = new SetFooter
                        {
                            SetFooterName = txt_FooterSet_Name.Text.Trim(),
                            UserId = global_var.userId
                        };
                        context.SetFooters.Add(newFooterSet);
                        context.SaveChanges();
                        txt_FooterSet_Name.Text = "";
                    }
                    FillFooterSet();
                }
            }
        }

        private void btn_FooterSet_edit_Click(object sender, EventArgs e)
        {
            if (dg_list_footer_set.Rows.Count > 0 && dg_list_footer_set.SelectedRows.Count > 0)
            {
                DialogResult Result = MessageBox.Show("푸터 세트명을 업데이트 하시겠습니까?", "푸터 세트명 업데이트", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        int FooterSetId = Convert.ToInt32(dg_list_footer_set.SelectedRows[0].Cells["dg_footerset_id"].Value.ToString());

                        if (FooterSetId != 0)
                        {
                            SetFooter result = context.SetFooters.SingleOrDefault(
                                   b => b.Id == FooterSetId
                                   && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                result.SetFooterName = txt_FooterSet_Name.Text.Trim();
                                context.SaveChanges();
                            }
                        }
                    }

                    FillFooterSet();
                }
            }
                
        }

        private void btn_FooterSet_delete_Click(object sender, EventArgs e)
        {
            if(dg_list_footer_set.Rows.Count > 0 && dg_list_footer_set.SelectedRows.Count > 0 &&
                dg_site_id.Rows.Count > 0 && dg_site_id.SelectedRows.Count > 0)
            {
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                if (shopeeId != string.Empty)
                {
                    DialogResult Result = MessageBox.Show("푸터 세트를 삭제 하시겠습니까?\r\n삭제시에는 포함되어 있는 모든 템플릿이 삭제됩니다.",
                    "푸터 세트 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (Result == DialogResult.Yes)
                    {
                        int FooterSetId = Convert.ToInt32(dg_list_footer_set.SelectedRows[0].Cells["dg_footerset_id"].Value.ToString());

                        if (FooterSetId != 0)
                        {
                            using (AppDbContext context = new AppDbContext())
                            {
                                SetFooter result = context.SetFooters.SingleOrDefault(b => b.Id == FooterSetId
                                && b.UserId == global_var.userId);

                                if (result != null)
                                {
                                    context.SetFooters.Remove(result);
                                    context.SaveChanges();
                                    txt_FooterSet_Name.Text = "";
                                }
                            }

                            FillFooterSet();
                            //FillTemplateHeader(shopeeId);
                            //FillTemplateFooter(shopeeId);
                        }
                    }
                }
            }
        }

        private void dg_list_footer_set_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_FooterSet_Name.Text = dg_list_footer_set.SelectedRows[0].Cells["dg_footerset_name"].Value.ToString();

            if (dg_list_footer_set.Rows.Count > 0 && dg_list_footer_set.SelectedRows.Count > 0)
            {
                int SetFooterId = Convert.ToInt32(dg_list_footer_set.SelectedRows[0].Cells["dg_footerset_id"].Value.ToString());
                FillTempleteFooter(SetFooterId);
            }
        }

        private void FormUploader_Activated(object sender, EventArgs e)
        {
            

            //BtnAddProduct_Click(null, null);
        }

        private void BtnUploadSellPrice_Click(object sender, EventArgs e)
        {
            if (dg_site_id.Rows.Count == 0 || dg_site_id.SelectedRows.Count == 0)
            {
                MessageBox.Show("ID를 선택하세요.", "ID선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
            string country = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
            FormUploadExcelPrice fu = new FormUploadExcelPrice();
            DateTime dt = new DateTime();
            if(!DateTime.TryParse(lbl_currency_date.Text, out dt))
            {
                dt = DateTime.Now;
            }
            fu.TarCountry = country;
            fu.currencyRate = txt_src_currency_rate.Value;
            fu.currenctDate = dt;
            fu.TarShopeeId = shopeeId;            
            fu.ShowDialog();

            BtnGetProductList_Click(null, null);
        }
        public string TempCategoryId = "";
        public bool isChanged = false;
        private void MainMenu_Set_Category_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.SelectedRows.Count > 0)
            {
                string srcCountry = "";
                string tarCountry = "";
                if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "IDR")
                {
                    srcCountry = "ID";
                }
                if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "SGD")
                {
                    srcCountry = "SG";
                }
                if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "MYR")
                {
                    srcCountry = "MY";
                }
                if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "THB")
                {
                    srcCountry = "TH";
                }
                if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "TWD")
                {
                    srcCountry = "TW";
                }
                if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "PHP")
                {
                    srcCountry = "PH";
                }
                if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "VND")
                {
                    srcCountry = "VN";
                }
                TempCategoryId = "";
                isChanged = false;
                FormSetCategoryAndAttributeDraft fcs = new FormSetCategoryAndAttributeDraft();
                fcs.fp = this;
                fcs.ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                fcs.srcCountry = srcCountry;
                fcs.tarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                fcs.srcCategory = dgSrcItemList.SelectedRows[0].Cells["dgItemList_category_id"].Value.ToString();
                fcs.tarCategory = dgSrcItemList.SelectedRows[0].Cells["dgItemList_tar_category_id"].Value.ToString();

                fcs.partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                fcs.shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                fcs.api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();

                fcs.tarShopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                fcs.srcItemId = Convert.ToInt64(dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString());
                
                fcs.txtSrcTitle.Text = dgSrcItemList.SelectedRows[0].Cells["dgItemList_name"].Value.ToString();
                fcs.ShowDialog();

                if (TempCategoryId != string.Empty)
                {
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_tar_category_id"].Value = TempCategoryId;
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                    dgSrcItemList.EndEdit();
                    grp_src.Select();
                }

                if (isChanged)
                {
                    if(dgSrcItemList.SelectedRows.Count > 0)
                    {
                        dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Style.BackColor = Color.Orange;
                    }
                }
            }
        }

        private void validate_category()
        {
            if (dgSrcItemList.Rows.Count > 0)
            {
                bool validateCategory = true;  
                //카테고리 맵핑 데이터가 있는지 확인
                using (var context = new AppDbContext())
                {
                    var category = context.ShopeeCategories.FirstOrDefault();
                    if (category == null)
                    {
                        validateCategory = false;
                        MessageBox.Show("카테고리 맵핑 데이터가 존재하지 않습니다.\r\n상단 메뉴의 카테고리 맵핑에 데이터를 업로드 하여 주세요.",
                            "카테고리 맵핑 데이터 누락",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if(!validateCategory)
                {
                    return;
                }
                string srcCountry = "";
                string tarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();

                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    bool isValidCategory = false;
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "IDR")
                        {
                            srcCountry = "ID";
                        }
                        if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "SGD")
                        {
                            srcCountry = "SG";
                        }
                        if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "MYR")
                        {
                            srcCountry = "MY";
                        }
                        if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "THB")
                        {
                            srcCountry = "TH";
                        }
                        if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "TWD")
                        {
                            srcCountry = "TW";
                        }
                        if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "PHP")
                        {
                            srcCountry = "PH";
                        }
                        if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "VND")
                        {
                            srcCountry = "VN";
                        }
                        long srcProductId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                        string srcCategoryId = dgSrcItemList.Rows[i].Cells["dgItemList_category_id"].Value.ToString();
                        string tarCategoryId = "";
                        //저장된 DB에서 찾아서 있는 경우

                        using (AppDbContext context = new AppDbContext())
                        {
                            List<CustomCategoryData> categoryList =
                                context.CustomCategoryDatas
                                .Where(
                                    x => x.SrcShopeeCountry == srcCountry &&
                                    x.TarShopeeCountry == tarCountry &&
                                    x.SrcCategoryId == srcCategoryId
                                    && x.UserId == global_var.userId
                                ).ToList();

                            if (categoryList.Count == 1)
                            {
                                isValidCategory = true;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = categoryList[0].TarCategoryId;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;

                                //새로 저장해 준다.
                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.src_item_id == srcProductId &&
                                b.tar_shopeeAccount == shopeeId
                                && b.UserId == global_var.userId);
                                if (result_src_data != null)
                                {
                                    result_src_data.category_id_tar = Convert.ToInt64(categoryList[0].TarCategoryId);
                                    context.SaveChanges();
                                }
                            }
                        }

                        //DB에서 못찾은 경우
                        if (!isValidCategory)
                        {
                            //전체 카테고리DB에서 가지고 온다.
                            using (AppDbContext context = new AppDbContext())
                            {
                                if (srcCountry == "ID")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories
                                        .Where(x => x.cat_id == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (tarCountry == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;

                                        //새로 저장해 준다.
                                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                        b => b.src_item_id == srcProductId &&
                                        b.tar_shopeeAccount == shopeeId
                                        && b.UserId == global_var.userId);
                                        if (result_src_data != null)
                                        {
                                            result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                            context.SaveChanges();
                                        }
                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (tarCountry == "MY")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_my.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_my.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;

                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_ph.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_ph.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;

                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_sg.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_sg.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;

                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_th.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_th.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;

                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_tw.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_tw.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_vn.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_vn.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                    }
                                }
                                else if (srcCountry == "MY")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories.Where(x => x.cat_my == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (tarCountry == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (tarCountry == "ID")
                                        {
                                            tarCategoryId = categoryList[0].cat_id.ToString();
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;

                                        //새로 저장해 준다.
                                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                        b => b.src_item_id == srcProductId &&
                                        b.tar_shopeeAccount == shopeeId
                                        && b.UserId == global_var.userId);
                                        if (result_src_data != null)
                                        {
                                            result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                            context.SaveChanges();
                                        }
                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (tarCountry == "MY")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_my.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_my.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        if (tarCountry == "ID")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_id.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_id.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_ph.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_ph.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_sg.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_sg.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_th.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_th.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_tw.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_tw.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_vn.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_vn.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                    }
                                }
                                else if (srcCountry == "Ph")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories.Where(x => x.cat_ph == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (tarCountry == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (tarCountry == "ID")
                                        {
                                            tarCategoryId = categoryList[0].cat_id.ToString();
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                        //새로 저장해 준다.
                                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                        b => b.src_item_id == srcProductId &&
                                        b.tar_shopeeAccount == shopeeId
                                        && b.UserId == global_var.userId);
                                        if (result_src_data != null)
                                        {
                                            result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                            context.SaveChanges();
                                        }
                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (tarCountry == "MY")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_my.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_my.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        if (tarCountry == "ID")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_id.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_id.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_ph.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_ph.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_sg.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_sg.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_th.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_th.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_tw.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_tw.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_vn.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_vn.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                    }
                                }
                                else if (srcCountry == "SG")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories.Where(x => x.cat_sg == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (tarCountry == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (tarCountry == "ID")
                                        {
                                            tarCategoryId = categoryList[0].cat_id.ToString();
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;

                                        //새로 저장해 준다.
                                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                        b => b.src_item_id == srcProductId &&
                                        b.tar_shopeeAccount == shopeeId
                                        && b.UserId == global_var.userId);
                                        if (result_src_data != null)
                                        {
                                            result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                            context.SaveChanges();
                                        }
                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (tarCountry == "MY")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_my.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_my.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        if (tarCountry == "ID")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_id.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_id.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_ph.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_ph.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_sg.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_sg.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_th.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_th.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_tw.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_tw.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_vn.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_vn.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                    }
                                }
                                else if (srcCountry == "TW")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories.Where(x => x.cat_tw == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (tarCountry == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (tarCountry == "ID")
                                        {
                                            tarCategoryId = categoryList[0].cat_id.ToString();
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                        //새로 저장해 준다.
                                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                        b => b.src_item_id == srcProductId &&
                                        b.tar_shopeeAccount == shopeeId
                                        && b.UserId == global_var.userId);
                                        if (result_src_data != null)
                                        {
                                            result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                            context.SaveChanges();
                                        }
                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (tarCountry == "MY")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_my.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_my.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        if (tarCountry == "ID")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_id.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_id.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_ph.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_ph.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_sg.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_sg.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_th.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_th.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_tw.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_tw.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_vn.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_vn.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                    }
                                }
                                else if (srcCountry == "TH")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories.Where(x => x.cat_th == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (tarCountry == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (tarCountry == "ID")
                                        {
                                            tarCategoryId = categoryList[0].cat_id.ToString();
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;

                                        //새로 저장해 준다.
                                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                        b => b.src_item_id == srcProductId &&
                                        b.tar_shopeeAccount == shopeeId
                                        && b.UserId == global_var.userId);
                                        if (result_src_data != null)
                                        {
                                            result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                            context.SaveChanges();
                                        }
                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (tarCountry == "MY")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_my.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_my.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        if (tarCountry == "ID")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_id.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_id.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_ph.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_ph.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_sg.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_sg.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_th.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_th.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_tw.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_tw.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_vn.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_vn.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                    }
                                }
                                else if (srcCountry == "VN")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories.Where(x => x.cat_vn == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (tarCountry == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (tarCountry == "ID")
                                        {
                                            tarCategoryId = categoryList[0].cat_id.ToString();
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                        //새로 저장해 준다.
                                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                        b => b.src_item_id == srcProductId &&
                                        b.tar_shopeeAccount == shopeeId
                                        && b.UserId == global_var.userId);
                                        if (result_src_data != null)
                                        {
                                            result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                            context.SaveChanges();
                                        }

                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (tarCountry == "MY")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_my.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_my.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        if (tarCountry == "ID")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_id.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_id.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "PH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_ph.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_ph.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "SG")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_sg.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_sg.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TH")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_th.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_th.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "TW")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_tw.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_tw.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                        else if (tarCountry == "VN")
                                        {
                                            string temp_tar_category_id = categoryList[0].cat_vn.ToString();
                                            bool isVal = true;
                                            for (int cat = 1; cat < categoryList.Count; cat++)
                                            {
                                                if (temp_tar_category_id != categoryList[cat].cat_vn.ToString())
                                                {
                                                    isVal = false;
                                                }
                                            }

                                            //모두 같은 것으로 간주
                                            if (isVal)
                                            {
                                                tarCategoryId = temp_tar_category_id;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);
                                                if (result_src_data != null)
                                                {
                                                    result_src_data.category_id_tar = Convert.ToInt64(tarCategoryId);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                    }
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("카테고리 검증을 완료 하였습니다.", "카테고리 검증 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btn_validate_category_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }

            //카테고리 맵핑 데이터가 있는지 검증
            using(AppDbContext context = new AppDbContext())
            {
                var result = context.ShopeeCategories.FirstOrDefault();
                if(result == null)
                {
                    MessageBox.Show("카테고리 맵핑 데이터가 존재하지 않습니다\r\n" +
                        "상단 메뉴의 카테고리 맵핑에서 맵핑 데이터를 업로드 하여주세요.", 
                        "맵핑 데이터 없음",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }


            DialogResult dlg_Result = MessageBox.Show("체크한 상품에 대하여 카테고리를 자동 설정하시겠습니까?\r\n국가별 카테고리 맵핑 데이터베이스를 검색하여 자동 설정합니다.", "카테고리 자동 설정", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                validate_category();
                Cursor.Current = Cursors.Default;
            }
        }

        private void MainMenu_Option_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
            {
                string srcCountry = "";
                string currencyCode = "";
                if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "IDR")
                {
                    srcCountry = "ID";
                }
                else if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "SGD")
                {
                    srcCountry = "SG";
                }
                else if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "MYR")
                {
                    srcCountry = "MY";
                }
                else if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "THB")
                {
                    srcCountry = "TH";
                }
                else if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "TWD")
                {
                    srcCountry = "TW";
                }
                else if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "PHP")
                {
                    srcCountry = "PH";
                }
                else if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString() == "VND")
                {
                    srcCountry = "VN";
                }

                string tarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                long productId = Convert.ToInt64(dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString().Trim());
                long src_shop_id = Convert.ToInt64(dgSrcItemList.SelectedRows[0].Cells["dgItemList_shopid"].Value.ToString().Trim());
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgItemList_ItemInfoDraftId"].Value.ToString().Trim());
                
                int supply_price = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgItemList_supply_price"].Value.ToString().Trim().Replace(",",""));
                int margin = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgItemList_margin"].Value.ToString().Trim().Replace(",", ""));
                int qty = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgItemList_stock"].Value.ToString().Trim().Replace(",", ""));
                decimal weight = Convert.ToDecimal(dgSrcItemList.SelectedRows[0].Cells["dgItemList_weight"].Value.ToString().Trim().Replace(",", ""));

                FormSetVariationDraft fs = new FormSetVariationDraft();
                fs.fp = this;
                fs.ItemInfoDraftId = ItemInfoDraftId;
                fs.tarCountry = tarCountry;
                fs.country = srcCountry;
                fs.shop_id = src_shop_id;
                fs.ItemId = productId;
                fs.tarShopeeId = shopeeId;
                fs.UdSourcePrice.Value = supply_price;
                fs.UdMargin.Value = margin;
                fs.UdQty.Value = qty;
                fs.UdWeight.Value = weight;
                fs.ShowDialog();
            }
        }

        private void validate_option()
        {
            if (dgSrcItemList.Rows.Count > 0)
            {   
                string TarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();

                string TarShopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();

                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        string SrcShopeeId = dgSrcItemList.Rows[i].Cells["dgItemList_src_shopeeId"].Value.ToString();
                        long productId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                        string productSKU = dgSrcItemList.Rows[i].Cells["dgItemList_item_sku"].Value.ToString();
                        string productName = dgSrcItemList.Rows[i].Cells["dgItemList_name"].Value.ToString();
                        int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());

                        using (AppDbContext context = new AppDbContext())
                        {
                            List<ItemVariationDraft> variationList = context.ItemVariationDrafts
                                            .Where(b => b.ItemInfoDraftId == ItemInfoDraftId
                                            && b.UserId == global_var.userId)
                                            .OrderBy(x => x.variation_id).ToList();

                            List<ItemVariationDraft> variationList2 = context.ItemVariationDrafts
                                .Where(b => b.src_item_id == productId
                                && b.UserId == global_var.userId)
                                .OrderBy(x => x.variation_id).ToList();

                            if (variationList != null && variationList.Count > 0)
                            {
                                //Variation이 있는 경우
                                //진짜 2티어인지 검증한다. 모든 옵션에 콤마가 있으면 2티어임.
                                bool isReal2Tier = true;
                                bool isSamePrice = true;

                                var initPrice = variationList[0].price;

                                for (int tier = 0; tier < variationList.Count; tier++)
                                {
                                    if (variationList[tier].name.ToString().Contains(","))
                                    {
                                        isReal2Tier = false;
                                        break;
                                    }
                                }

                                for (int tier = 0; tier < variationList.Count; tier++)
                                {
                                    if (initPrice != variationList[tier].price)
                                    {
                                        isSamePrice = false;
                                        break;
                                    }
                                }

                                bool validatePrice = true;
                                //소비자가와 판가가 있는 경우는 모두 정상이다.
                                for (int tier = 0; tier < variationList.Count; tier++)
                                {
                                    if (variationList[tier].price == 0 || variationList[tier].original_price == 0)
                                    {
                                        validatePrice = false;
                                        break;
                                    }
                                }

                                //일단 가격이 모두 존재하여야 한다.
                                if (validatePrice)
                                {
                                    if (isSamePrice)
                                    {
                                        //dgSrcItemList.Rows[i].Cells["dgItemList_has_variation"].Value = true;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_has_variation"].Style.BackColor = Color.GreenYellow;
                                    }
                                    else
                                    {
                                        //가격이 다른경우
                                        //dgSrcItemList.Rows[i].Cells["dgItemList_has_variation"].Value = true;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_has_variation"].Style.BackColor = Color.Green;
                                    }
                                }
                                else
                                {
                                    //dgSrcItemList.Rows[i].Cells["dgItemList_has_variation"].Value = false;
                                    dgSrcItemList.Rows[i].Cells["dgItemList_has_variation"].Style.BackColor = Color.Orange;
                                }
                            }
                            else
                            {
                                //dgSrcItemList.Rows[i].Cells["dgItemList_has_variation"].Value = true;
                                dgSrcItemList.Rows[i].Cells["dgItemList_has_variation"].Style.BackColor = Color.GreenYellow;
                            }
                        }

                    }
                }
                
            }
        }
        private void btn_validate_option_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            validate_option();
            Cursor.Current = Cursors.Default;
            MessageBox.Show("체크한 상품에 대하여 모든 옵션값을 검증 하였습니다", "옵션데이터 검증", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgSrcItemList_SelectionChanged(object sender, EventArgs e)
        {
            dgSrcItemList.CurrentRow.Cells["dgItemList_has_variation"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_has_variation"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_has_variation"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_category_id"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_tar_category_id"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_category_id"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_stock"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_stock"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_stock"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_weight"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_weight"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_weight"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_item_id"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_item_id"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_item_id"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_supply_price"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_supply_price"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_supply_price"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_margin"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_margin"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_margin"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_pg_fee"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_pg_fee"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_pg_fee"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_price_won"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_price_won"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_price_won"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_price"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_price"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_price"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_original_price"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_original_price"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_original_price"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_stock"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_stock"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_stock"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_weight"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_weight"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_weight"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_attribute"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_attribute"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_attribute"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_Result"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_Result"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_Result"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_has_variation"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_has_variation"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_has_variation"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_attribute"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_attribute"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_attribute"].Style.SelectionForeColor = Color.Blue;


        }
        private void validate_weight()
        {
            if(dgSrcItemList.Rows.Count > 0)
            {
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        string weight = dgSrcItemList.Rows[i].Cells["dgItemList_weight"].Value.ToString().Replace(",", "");
                        if (weight != string.Empty && weight != "0")
                        {
                            dgSrcItemList.Rows[i].Cells["dgItemList_weight"].Style.BackColor = Color.GreenYellow;
                        }
                        else
                        {
                            dgSrcItemList.Rows[i].Cells["dgItemList_weight"].Style.BackColor = Color.Orange;
                        }
                    }
                }
            }
        }
        private void btn_validate_weight_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            validate_weight();
            Cursor.Current = Cursors.Default;
            MessageBox.Show("무게를 모두 검증하였습니다.", "무게 검증", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MainMenu_validate_category_Click(object sender, EventArgs e)
        {
            btn_validate_category_Click(null, null);
        }

        private void MainMenu_validate_option_Click(object sender, EventArgs e)
        {
            btn_validate_option_Click(null, null);
        }

        private void MainMenu_validate_weight_Click(object sender, EventArgs e)
        {
            btn_validate_weight_Click(null, null);
        }

        private void MainMenu_Validate_all_Click(object sender, EventArgs e)
        {
            if(dgSrcItemList.Rows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                validate_category();
                validate_option();
                validate_weight();
                validate_attribute();
                Cursor.Current = Cursors.Default;
                MessageBox.Show("체크한 상품에 대하여 모든 값을 검증 하였습니다", "데이터 일괄 검증", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UdDTS_ValueChanged(object sender, EventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                ConfigVar result = context.ConfigVars.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result == null)
                {
                    ConfigVar newVar = new ConfigVar
                    {
                        dts = Convert.ToInt32(UdDTS.Value),
                        UserId = global_var.userId
                    };
                    context.ConfigVars.Add(newVar);
                    context.SaveChanges();
                }
                else
                {
                    result.dts = Convert.ToInt32(UdDTS.Value);
                    context.SaveChanges();
                }
            }
        }

        private void BtnApplyDTS_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }
            DialogResult dlg_Result = MessageBox.Show("체크한 상품의 Days To Ship을 적용하시겠습니까?", "DTS 일괄 적용", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                progressBar.Value = 0;
                progressBar.Maximum = dgSrcItemList.Rows.Count;
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        long srcProductId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.src_item_id == srcProductId &&
                                b.tar_shopeeAccount == shopeeId
                                && b.UserId == global_var.userId);

                            //원본데이터가 DB에 존재하여야 한다.
                            if (result_src_data != null)
                            {
                                result_src_data.days_to_ship = Convert.ToInt32(UdDTS.Value);
                                context.SaveChanges();
                                dgSrcItemList.Rows[i].Cells["dgItemList_days_to_ship"].Value = string.Format("{0:n0}", UdDTS.Value);
                                dgSrcItemList.Rows[i].Cells["dgItemList_days_to_ship"].Style.BackColor = Color.GreenYellow;
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("DTS를 모두 적용하였습니다.", "DTS적용", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        bool isChanging;
        private void dgSrcItemList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dg_site_id.Rows.Count > 0)
            {
                int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.Rows[e.RowIndex].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                //상품명이 변경되면 저장해 준다.
                if (e.ColumnIndex == 8 && e.RowIndex > -1)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                            b => b.Id == ItemInfoDraftId
                            && b.UserId == global_var.userId);

                        //원본데이터가 DB에 존재하여야 한다.
                        if (result_src_data != null)
                        {
                            result_src_data.name = dgSrcItemList.Rows[e.RowIndex].Cells["dgItemList_name"].Value.ToString().Trim();
                            context.SaveChanges();
                            dgSrcItemList.Rows[e.RowIndex].Cells["dgItemList_name"].Style.BackColor = Color.SkyBlue;
                        }
                    }
                }
                else if (e.ColumnIndex == 6 && e.RowIndex > -1)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                            b => b.Id == ItemInfoDraftId
                            && b.UserId == global_var.userId);

                        //원본데이터가 DB에 존재하여야 한다.
                        if (result_src_data != null)
                        {
                            if(dgSrcItemList.Rows[e.RowIndex].Cells["dgItemList_item_sku"].Value == null)
                            {
                                result_src_data.item_sku = "";
                            }
                            else
                            {
                                result_src_data.item_sku = dgSrcItemList.Rows[e.RowIndex].Cells["dgItemList_item_sku"].Value.ToString().Trim();
                            }
                            
                            context.SaveChanges();
                            dgSrcItemList.Rows[e.RowIndex].Cells["dgItemList_item_sku"].Style.BackColor = Color.SkyBlue;
                        }
                    }
                }
                else if(e.RowIndex > -1 && e.ColumnIndex == 19)
                {
                    //수량 변경
                    //저장해 준다.
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemInfoDraft result = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.Id == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                        int stock = Convert.ToInt32(dgSrcItemList.Rows[e.RowIndex].Cells["dgItemList_stock"].Value.ToString().Trim().Replace(",",""));
                        if (result != null)
                        {
                            result.stock = stock;
                            context.SaveChanges();

                            if(stock > 0)
                            {
                                dgSrcItemList.Rows[e.RowIndex].Cells["dgItemList_stock"].Style.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                dgSrcItemList.Rows[e.RowIndex].Cells["dgItemList_stock"].Style.BackColor = Color.Orange;
                            }
                        }
                    }
                }
                else if((e.ColumnIndex == 12 || e.ColumnIndex == 13 || e.ColumnIndex == 22) && e.RowIndex > -1) // 12: 상품원가(원), 13: 마진(원), 22: 무게(Kg)
                {
                    if(!isChanging)
                    {
                        calcRowPrice(e.RowIndex, dgSrcItemList.Columns[e.ColumnIndex].Name);
                    }
                }
                else if ((e.ColumnIndex == 17) && e.RowIndex > -1)
                {
                    if(!isChanging)
                    {
                        calcRowPriceFromSellPrice(e.RowIndex, dgSrcItemList.Columns[e.ColumnIndex].Name);
                    }
                }
            }
        }

        private void dgSrcItemList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1 && e.ColumnIndex == 2)
            {
                MainMenu_View_Product_Page_Click(null, null);
            }
            else if(e.RowIndex > -1 && e.ColumnIndex == 10)
            {
                MainMenu_Option_Click(null, null);
            }
            else if (e.RowIndex > -1 && e.ColumnIndex == 11)
            {
                MainMenu_Set_Category_Click(null, null);
                validate_row_attribute();
            }
            else if (e.RowIndex > -1 && e.ColumnIndex == 24)
            {
                MainMenu_Set_Category_Click(null, null);
                validate_row_attribute();
            }
            else if (e.RowIndex > -1 && e.ColumnIndex == 37)
            {
                if(dg_shopee_discount.Rows.Count > 0 && dg_shopee_discount.SelectedRows.Count > 0)
                {
                    //할인명 세팅
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_discount_id"].Value = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_id"].Value.ToString();
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_discount_name"].Value = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_name"].Value.ToString();
                    //저장한다.
                    using (AppDbContext context = new AppDbContext())
                    {
                        int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.Id == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                        if (result_src_data != null)
                        {
                            result_src_data.discount_id = Convert.ToInt32(dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_id"].Value.ToString());
                            result_src_data.discount_name = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_name"].Value.ToString();
                            context.SaveChanges();
                        }
                    }
                }
            }
            else if (e.RowIndex > -1 && e.ColumnIndex == 42)
            {
                //헤더세트
                if (dg_upload_header.Rows.Count > 0 && dg_upload_header.SelectedRows.Count > 0)
                {   
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_header_set"].Value = dg_upload_header.SelectedRows[0].Cells["dg_upload_header_name"].Value.ToString();
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_header_set_id"].Value = dg_upload_header.SelectedRows[0].Cells["dg_upload_header_idx"].Value.ToString();
                    //저장한다.
                    using (AppDbContext context = new AppDbContext())
                    {
                        int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.Id == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                        if (result_src_data != null)
                        {
                            result_src_data.SetHeaderId = Convert.ToInt32(dg_upload_header.SelectedRows[0].Cells["dg_upload_header_idx"].Value.ToString());
                            result_src_data.SetHeaderName = dg_upload_header.SelectedRows[0].Cells["dg_upload_header_name"].Value.ToString();
                            context.SaveChanges();
                        }
                    }
                }
            }

            else if (e.RowIndex > -1 && e.ColumnIndex == 44)
            {
                //푸터세트
                if (dg_upload_footer.Rows.Count > 0 && dg_upload_footer.SelectedRows.Count > 0)
                {
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_footer_set"].Value = dg_upload_footer.SelectedRows[0].Cells["dg_upload_footer_name"].Value.ToString();
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_footer_set_id"].Value = dg_upload_footer.SelectedRows[0].Cells["dg_upload_footer_idx"].Value.ToString();
                    //저장한다.
                    using (AppDbContext context = new AppDbContext())
                    {
                        int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.Id == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                        if (result_src_data != null)
                        {
                            result_src_data.SetFooterId = Convert.ToInt32(dg_upload_footer.SelectedRows[0].Cells["dg_upload_footer_idx"].Value.ToString());
                            result_src_data.SetFooterName = dg_upload_footer.SelectedRows[0].Cells["dg_upload_footer_name"].Value.ToString();
                            context.SaveChanges();
                        }
                    }
                }
            }
            else if (e.RowIndex > -1 && e.ColumnIndex == 46)
            {
                //등록 성공 실패
                if (dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
                {
                    if(dgSrcItemList.SelectedRows[0].Cells["dgItemList_Result"].Value != null)
                    {
                        if (dgSrcItemList.SelectedRows[0].Cells["dgItemList_Result"].Value.ToString() == "실패")
                        {
                            FormUploadViewMessage fv = new FormUploadViewMessage();
                            fv.TxtMessage.Text = dgSrcItemList.SelectedRows[0].Cells["dgItemList_Result_message"].Value.ToString();
                            fv.ShowDialog();
                        }
                        else if(dgSrcItemList.SelectedRows[0].Cells["dgItemList_Result"].Value.ToString() == "성공")
                        {
                            if (dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
                            {
                                string siteUrl = "";

                                long shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                                string tarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();


                                if (tarCountry == "SG")
                                {
                                    siteUrl = "https://shopee.sg/product/" + shop_id + "/" + dgSrcItemList.SelectedRows[0].Cells["dgItemList_Result_message"].Value.ToString();
                                }
                                else if (tarCountry == "MY")
                                {
                                    siteUrl = "https://shopee.com.my/product/" + shop_id + "/" + dgSrcItemList.SelectedRows[0].Cells["dgItemList_Result_message"].Value.ToString();
                                }
                                else if (tarCountry == "ID")
                                {
                                    siteUrl = "https://shopee.co.id/product/" + shop_id + "/" + dgSrcItemList.SelectedRows[0].Cells["dgItemList_Result_message"].Value.ToString();
                                }
                                else if (tarCountry == "TH")
                                {
                                    siteUrl = "https://shopee.co.th/product/" + shop_id + "/" + dgSrcItemList.SelectedRows[0].Cells["dgItemList_Result_message"].Value.ToString();
                                }
                                else if (tarCountry == "TW")
                                {
                                    siteUrl = "https://shopee.tw/product/" + shop_id + "/" + dgSrcItemList.SelectedRows[0].Cells["dgItemList_Result_message"].Value.ToString();
                                }
                                else if (tarCountry == "PH")
                                {
                                    siteUrl = "https://shopee.ph/product/" + shop_id + "/" + dgSrcItemList.SelectedRows[0].Cells["dgItemList_Result_message"].Value.ToString();
                                }
                                else if (tarCountry == "VN")
                                {
                                    siteUrl = "https://shopee.vn/product/" + shop_id + "/" + dgSrcItemList.SelectedRows[0].Cells["dgItemList_Result_message"].Value.ToString();
                                }
                                System.Diagnostics.Process.Start("chrome.exe", siteUrl);
                            }
                        }
                    }
                }
            }
        }
        private static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
        private string HashString(string StringToHash, string HashKey)
        {
            System.Text.UTF8Encoding myEncoder = new UTF8Encoding();
            byte[] key = myEncoder.GetBytes(HashKey);
            byte[] Text = myEncoder.GetBytes(StringToHash);
            HMACSHA256 myHMACSHA256 = new HMACSHA256(key);
            byte[] HashCode = myHMACSHA256.ComputeHash(Text);
            string hash = ByteToString(HashCode);
            return hash.ToLower();
        }
        private int getAttributeCountByCategory(long categoryID, long partner_id, long shop_id, string api_key)
        {
            long long_time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));
            string end_point_attribute = "https://partner.shopeemobile.com/api/v1/item/attributes/get";
            Dictionary<string, object> dic_attribute = new Dictionary<string, object>();
            dic_attribute.Add("category_id", categoryID);
            dic_attribute.Add("partner_id", Convert.ToInt32(partner_id));
            dic_attribute.Add("shopid", Convert.ToInt32(shop_id));
            dic_attribute.Add("timestamp", long_time_stamp);

            var client_attribute = new RestClient(end_point_attribute);
            string body_attribute = JsonConvert.SerializeObject(dic_attribute);
            string auth_attribute = end_point_attribute + "|" + body_attribute;
            string authorization_attribute = HashString(auth_attribute, api_key);
            var request_attribute = new RestRequest("", RestSharp.Method.POST);
            request_attribute.Method = Method.POST;
            request_attribute.AddHeader("Accept", "application/json");
            request_attribute.AddJsonBody(new
            {
                category_id = Convert.ToInt32(categoryID),
                partner_id = Convert.ToInt32(partner_id),
                shopid = Convert.ToInt32(shop_id),
                timestamp = long_time_stamp
            });
            request_attribute.AddHeader("authorization", authorization_attribute);

            IRestResponse response_attribute = client_attribute.Execute(request_attribute);
            var content_attribute = response_attribute.Content;
            dynamic result_attribute = JsonConvert.DeserializeObject(content_attribute);

            int count = 0;
            if (result_attribute.attributes != null)
            {
                for (int i = 0; i < result_attribute.attributes.Count; i++)
                {
                    if((bool)result_attribute.attributes[i].is_mandatory)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
        private void btn_validate_attribute_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }

            DialogResult dlg_Result = MessageBox.Show("체크한 상품의 속성값을 일괄 검증 하시겠습니까?", "상품 속성 검증", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                validate_attribute();
                MessageBox.Show("속성검증을 완료 하였습니다.","속성 검증 완료",MessageBoxButtons.OK,MessageBoxIcon.Information);
                Cursor.Current = Cursors.Default;
            }
                
        }

        private void validate_attribute()
        {
            string tarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
            long partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
            long shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
            string api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
            string tarShopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();

            //속성을 검증하기 위해서는 각 카테고리별로 대상의 필수속성의 개수를 기록하고 
            //이 개수와 저장된 개수를 비교하여 검증한다.
            Dictionary<int, int> dicCategory = new Dictionary<int, int>();
            for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
            {
                int tarCategoryId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value.ToString());
                if (tarCategoryId != 0)
                {
                    try
                    {
                        //우선DB에서 데이터가 있는지 확인하고 없으면 가지고 온다.

                        dicCategory.Add(tarCategoryId, 0);
                    }
                    catch
                    {

                    }
                }
            }

            //속성개수를 가지고 온다.
            for (int i = 0; i < dicCategory.Count; i++)
            {
                //먼저 DB를 뒤진다
                using (AppDbContext context = new AppDbContext())
                {
                    int tarCategoryId = dicCategory.Keys.ToList()[i];
                    CategoryVariationMandatoryCount result = context.CategoryVariationMandatoryCounts.FirstOrDefault(
                        b => b.tarCountry == tarCountry &&
                        b.tarCategoryId == tarCategoryId
                        && b.UserId == global_var.userId);

                    if (result == null)
                    {
                        int cnt = getAttributeCountByCategory(tarCategoryId, partner_id, shop_id, api_key);
                        dicCategory[dicCategory.Keys.ToList()[i]] = cnt;

                        var newData = new CategoryVariationMandatoryCount
                        {
                            tarCountry = tarCountry,
                            tarCategoryId = tarCategoryId,
                            mandatoryCount = cnt,
                            UserId = global_var.userId
                        };
                        context.CategoryVariationMandatoryCounts.Add(newData);
                        context.SaveChanges();
                    }
                    else
                    {
                        dicCategory[dicCategory.Keys.ToList()[i]] = result.mandatoryCount;
                    }
                }
            }

            for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
            {
                long srcItemId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                int tarCategoryId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value.ToString());
                int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                if (tarCategoryId != 0)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        List<ItemAttributeDraftTar> attributeList = context.ItemAttributeDraftTars
                                        .Where(b => b.src_item_id == srcItemId &&
                                        b.ItemInfoDraftId == ItemInfoDraftId
                                        && b.UserId == global_var.userId)
                                        .OrderBy(x => x.is_mandatory).ToList();

                        //필수항목의 개수를 구한다.
                        //데이터가 아예 없을수도 있다.
                        int cntDb = 0;
                        if (attributeList != null)
                        {
                            for (int cnt = 0; cnt < attributeList.Count; cnt++)
                            {
                                if (attributeList[cnt].is_mandatory)
                                {
                                    cntDb++;
                                }
                            }
                        }

                        if (dicCategory[tarCategoryId] == cntDb)
                        {
                            dgSrcItemList.Rows[i].Cells["dgItemList_attribute"].Value = true;
                            dgSrcItemList.Rows[i].Cells["dgItemList_attribute"].Style.BackColor = Color.GreenYellow;
                        }
                        else
                        {
                            dgSrcItemList.Rows[i].Cells["dgItemList_attribute"].Value = false;
                            dgSrcItemList.Rows[i].Cells["dgItemList_attribute"].Style.BackColor = Color.Orange;
                        }
                    }
                }
                else
                {
                    dgSrcItemList.Rows[i].Cells["dgItemList_attribute"].Value = false;
                    dgSrcItemList.Rows[i].Cells["dgItemList_attribute"].Style.BackColor = Color.Orange;
                }
            }
        }

        private void validate_row_attribute()
        {
            if(dg_site_id.Rows.Count > 0 &&
                dg_site_id.SelectedRows.Count > 0 &&
                dgSrcItemList.Rows.Count > 0 &&
                dgSrcItemList.SelectedRows.Count > 0)
            {
                string tarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                long partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                long shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                string api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
                string tarShopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                long ItemId = Convert.ToInt64(dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString());

                //속성을 검증하기 위해서는 각 카테고리별로 대상의 필수속성의 개수를 기록하고 
                //이 개수와 저장된 개수를 비교하여 검증한다.
                var dicCategory = new Dictionary<long, int>();

                for (int i = 0; i < dgSrcItemList.SelectedRows.Count; i++)
                {
                    long tarCategoryId = 0;
                    //화면에 보이는 값이 아닌 DB에서 값을 가지고 온다.
                    using (var context = new AppDbContext())
                    {
                        var selectedItem = context.ItemInfoDrafts.FirstOrDefault(
                            x => x.tar_shopeeAccount == tarShopeeId 
                            && x.src_item_id == ItemId 
                            && x.UserId == global_var.userId);

                        if(selectedItem != null)
                        {
                            tarCategoryId = selectedItem.category_id_tar;
                        }
                    }
                        
                    if (tarCategoryId != 0)
                    {
                        try
                        {
                            //우선DB에서 데이터가 있는지 확인하고 없으면 가지고 온다.

                            dicCategory.Add(tarCategoryId, 0);
                        }
                        catch
                        {

                        }
                    }
                }

                //속성개수를 가지고 온다.
                for (int i = 0; i < dicCategory.Count; i++)
                {
                    //먼저 DB를 뒤진다
                    using (var context = new AppDbContext())
                    {
                        long tarCategoryId = dicCategory.Keys.ToList()[i];
                        CategoryVariationMandatoryCount result = context.CategoryVariationMandatoryCounts.FirstOrDefault(
                            b => b.tarCountry == tarCountry 
                            && b.tarCategoryId == tarCategoryId
                            && b.UserId == global_var.userId);

                        if (result == null)
                        {
                            int cnt = getAttributeCountByCategory(tarCategoryId, partner_id, shop_id, api_key);
                            dicCategory[dicCategory.Keys.ToList()[i]] = cnt;

                            CategoryVariationMandatoryCount newData = new CategoryVariationMandatoryCount
                            {
                                tarCountry = tarCountry,
                                tarCategoryId = tarCategoryId,
                                mandatoryCount = cnt,
                                UserId = global_var.userId
                            };
                            context.CategoryVariationMandatoryCounts.Add(newData);
                            context.SaveChanges();
                        }
                        else
                        {
                            dicCategory[dicCategory.Keys.ToList()[i]] = result.mandatoryCount;
                        }
                    }
                }

                for (int i = 0; i < dgSrcItemList.SelectedRows.Count; i++)
                {
                    long srcItemId = Convert.ToInt64(dgSrcItemList.SelectedRows[i].Cells["dgItemList_item_id"].Value.ToString());
                    int tarCategoryId = Convert.ToInt32(dgSrcItemList.SelectedRows[i].Cells["dgItemList_tar_category_id"].Value.ToString());
                    int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.SelectedRows[i].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                    if (tarCategoryId != 0)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            List<ItemAttributeDraftTar> attributeList = context.ItemAttributeDraftTars
                                            .Where(b => b.src_item_id == srcItemId 
                                            && b.ItemInfoDraftId == ItemInfoDraftId
                                            && b.UserId == global_var.userId)
                                            .OrderBy(x => x.is_mandatory).ToList();

                            //필수항목의 개수를 구한다.
                            //데이터가 아예 없을수도 있다.
                            int cntDb = 0;
                            if (attributeList != null)
                            {
                                for (int cnt = 0; cnt < attributeList.Count; cnt++)
                                {
                                    if (attributeList[cnt].is_mandatory)
                                    {
                                        cntDb++;
                                    }
                                }
                            }

                            if (dicCategory.Count > 0)
                            {
                                if (dicCategory[tarCategoryId] == cntDb)
                                {
                                    dgSrcItemList.SelectedRows[i].Cells["dgItemList_attribute"].Value = true;
                                    dgSrcItemList.SelectedRows[i].Cells["dgItemList_attribute"].Style.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    dgSrcItemList.SelectedRows[i].Cells["dgItemList_attribute"].Value = false;
                                    dgSrcItemList.SelectedRows[i].Cells["dgItemList_attribute"].Style.BackColor = Color.Orange;
                                }
                            }
                        }
                    }
                    else
                    {
                        dgSrcItemList.SelectedRows[i].Cells["dgItemList_attribute"].Value = false;
                        dgSrcItemList.SelectedRows[i].Cells["dgItemList_attribute"].Style.BackColor = Color.Orange;
                    }
                }
            }
        }
        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        private void MainMenu_validate_attribute_Click(object sender, EventArgs e)
        {
            btn_validate_attribute_Click(null, null);
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            if(dg_site_id.Rows.Count > 0)
            {
                string tarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                FromAddProduct fa = new FromAddProduct();
                //fa.tarCountry = tarCountry;
                fa.strHeaderSeparator = TxtHeaderSeparator.Text;
                fa.strFooterSeparator = TxtFooterSeparator.Text;
                fa.ShowDialog();
            }
        }

        private void BtnUploadProduct_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }

            if (dgSrcItemList.Rows.Count > 0)
            {
                DialogResult dlg_Result = MessageBox.Show("설정하신 판매가격으로 체크상품을 업데이트 하시겠습니까?", "쇼피 상품 등록", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dlg_Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    long partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                    long shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                    string api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
                    string end_point = "https://partner.shopeemobile.com/api/v1/item/add";
                    string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                    string tarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();

                    if (shopeeId == string.Empty)
                    {
                        return;
                    }

                    int logisticCode = 0;
                    if (dg_shopee_logistics.SelectedRows.Count > 0)
                    {
                        logisticCode = Convert.ToInt32(dg_shopee_logistics.SelectedRows[0].Cells["dg_shopee_logistics_logistics_id"].Value.ToString());
                    }
                    else
                    {
                        MessageBox.Show("등록 대상의 배송 코드가 선택되지 않았습니다.", "등록 배송 코드 선택",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    
                    Cursor.Current = Cursors.WaitCursor;
                    using (AppDbContext context = new AppDbContext())
                    {
                        for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                        {
                            if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                            {
                                string srcCountry = "";
                                if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "IDR")
                                {
                                    srcCountry = "ID";
                                }
                                else if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "SGD")
                                {
                                    srcCountry = "SG";
                                }
                                else if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "MYR")
                                {
                                    srcCountry = "MY";
                                }
                                else if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "THB")
                                {
                                    srcCountry = "TH";
                                }
                                else if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "TWD")
                                {
                                    srcCountry = "TW";
                                }
                                else if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "PHP")
                                {
                                    srcCountry = "PH";
                                }
                                else if (dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString() == "VND")
                                {
                                    srcCountry = "VN";
                                }

                                string strSku = dgSrcItemList.Rows[i].Cells["dgItemList_item_sku"].Value? .ToString().Trim() ?? "";

                                if (strSku == string.Empty)
                                {
                                    MessageBox.Show("상품SKU가 없습니다. 상품SKU는 반드시 입력하셔야 합니다.",
                                        "상품SKU없음",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                                }
                                else
                                {
                                    int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                                    string tarCategoryId = dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value.ToString();
                                    string strCalcPrice = dgSrcItemList.Rows[i].Cells["dgItemList_original_price"].Value.ToString().Trim();
                                    string srcAccount = dgSrcItemList.Rows[i].Cells["dgItemList_src_shopeeId"].Value.ToString().Trim();

                                    if (tarCategoryId == string.Empty)
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Style.BackColor = Color.Orange;
                                    }

                                    if (strCalcPrice == string.Empty)
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_original_price"].Style.BackColor = Color.Orange;
                                    }

                                    if (tarCategoryId != string.Empty && strCalcPrice != string.Empty)
                                    {
                                        long srcProductId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString().Trim());
                                        bool shopee_set_preorder = false;
                                        string sell_title = dgSrcItemList.Rows[i].Cells["dgItemList_name"].Value.ToString().Trim();
                                        string itemSku = dgSrcItemList.Rows[i].Cells["dgItemList_item_sku"].Value.ToString().Trim();
                                        int shopee_set_preorder_days = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_days_to_ship"].Value.ToString().Trim());


                                        string strHeader = "";
                                        int headerSetId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_header_set_id"].Value?.ToString().Trim());
                                        string headerSetName = dgSrcItemList.Rows[i].Cells["dgItemList_header_set"].Value?.ToString().Trim();
                                        if (headerSetId != 0)
                                        {
                                            strHeader = FillHeaderSet(Convert.ToInt32(headerSetId));
                                        }


                                        string strFooter = "";
                                        int footerSetId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_footer_set_id"].Value?.ToString().Trim());
                                        string footerSetName = dgSrcItemList.Rows[i].Cells["dgItemList_footer_set"].Value?.ToString().Trim();
                                        if (footerSetId != 0)
                                        {
                                            strFooter = FillFooterSet(Convert.ToInt32(footerSetId));
                                        }
                                        // 상품 설명을 수정했을 수 있으므로 DB에서 가지고 온다
                                        var ItemBaseInfo = context.ItemInfoDrafts.SingleOrDefault(
                                            x => x.Id == ItemInfoDraftId
                                            && x.UserId == global_var.userId);

                                        string description = ItemBaseInfo.description;
                                        //상품 설명에 구분자가 있는지 검사하여 헤더/푸터를 구분하여 정리한다.

                                        string extractDesc = "";
                                        string strHeaderSeparator = TxtHeaderSeparator.Text.Trim();
                                        string strFooterSeparator = TxtFooterSeparator.Text.Trim();

                                        int idxEndOfHeader = 0;
                                        int idxEndOfFooter = 0;
                                        if (description.Contains(strHeaderSeparator))
                                        {
                                            idxEndOfHeader = description.IndexOf(strHeaderSeparator) + strHeaderSeparator.Length;
                                        }

                                        if (description.Contains(strFooterSeparator))
                                        {
                                            idxEndOfFooter = description.LastIndexOf(strFooterSeparator);
                                        }

                                        //헤더 푸터를 제외한 본문만 추출한다.

                                        if (idxEndOfHeader == 0 && idxEndOfFooter == 0)
                                        {
                                            //헤더 푸터 모두가 없는 경우
                                            extractDesc = description;
                                        }
                                        else if (idxEndOfHeader != 0 && idxEndOfFooter == 0)
                                        {
                                            //헤더만 있는 경우
                                            extractDesc = description.Substring(idxEndOfHeader);
                                        }
                                        else if (idxEndOfHeader == 0 && idxEndOfFooter != 0)
                                        {
                                            //푸터만 있는 경우
                                            extractDesc = description.Substring(0, idxEndOfFooter);
                                        }
                                        else if (idxEndOfHeader > idxEndOfFooter)
                                        {
                                            //잘못 인식한 경우
                                            extractDesc = description;
                                        }

                                        StringBuilder sbNewDesc = new StringBuilder();

                                        if (headerSetId != 0)
                                        {
                                            sbNewDesc.Append(strHeader);
                                            sbNewDesc.Append(strHeaderSeparator + "\r\n");
                                        }

                                        sbNewDesc.Append(extractDesc);

                                        if (footerSetId != 0)
                                        {
                                            sbNewDesc.Append(strFooterSeparator + "\r\n");
                                            sbNewDesc.Append(strFooter);
                                        }


                                        decimal calcPrice = Convert.ToDecimal(dgSrcItemList.Rows[i].Cells["dgItemList_price"].Value.ToString().Trim().Replace(",", ""));
                                        string compair_price = dgSrcItemList.Rows[i].Cells["dgItemList_original_price"].Value.ToString().Trim().Replace(",", "");
                                        string qty = dgSrcItemList.Rows[i].Cells["dgItemList_stock"].Value.ToString().Trim().Replace(",", "");
                                        int iQty = Convert.ToInt32(qty);
                                        double iWeight = Convert.ToDouble(dgSrcItemList.Rows[i].Cells["dgItemList_weight"].Value.ToString().Trim().Replace(",", ""));
                                        decimal pgFee = 0;

                                        if (dgSrcItemList.Rows[i].Cells["dgItemList_pg_fee"].Value != null && 
                                            dgSrcItemList.Rows[i].Cells["dgItemList_pg_fee"].Value.ToString().Trim() != string.Empty)
                                        {
                                            pgFee = Convert.ToDecimal(dgSrcItemList.Rows[i].Cells["dgItemList_pg_fee"].Value.ToString().Trim().Replace(",", ""));
                                        }

                                        string strImage = dgSrcItemList.Rows[i].Cells["dgItemList_images"].Value.ToString().Trim();

                                        List<shopee_image> lst_img = new List<shopee_image>();
                                        if (strImage.Length > 0)
                                        {
                                            var arImages = strImage.Split('^');
                                            if (arImages.Length > 0)
                                            {
                                                for (int img = 0; img < arImages.Length; img++)
                                                {
                                                    shopee_image s_image = new shopee_image { url = arImages[img].ToString().Trim() };
                                                    lst_img.Add(s_image);
                                                }
                                            }
                                        }

                                        List<shopee_variations> lst_vari = new List<shopee_variations>();
                                        List<ItemVariationDraft> variationList = context.ItemVariationDrafts
                                                            .Where(b => b.src_item_id == srcProductId &&
                                                            b.tar_shopeeAccount == shopeeId
                                                            && b.UserId == global_var.userId)
                                                            .OrderBy(x => x.variation_id).ToList();
                                        Dictionary<string, decimal> dicVariationPromotionPrice = new Dictionary<string, decimal>();
                                        if (variationList != null && variationList.Count > 0)
                                        {
                                            //Variation이 있는 경우
                                            //진짜 2티어인지 검증한다. 모든 옵션에 콤마가 있으면 2티어임.
                                            bool isReal2Tier = true;
                                            bool isSamePrice = true;

                                            for (int vari = 0; vari < variationList.Count; vari++)
                                            {
                                                using (shopee_variations vars = new shopee_variations())
                                                {
                                                    vars.name = variationList[vari].name.ToString();
                                                    vars.stock = variationList[vari].stock;

                                                    //현지 판가로 계산하여야 한다.
                                                    //정수형일 경우 정수로 넣어주어야 한다.
                                                    string variTemp = "";
                                                    int iVariTemp = 0;

                                                    //소수점이 0이면 없이 넘어온다.
                                                    variTemp = string.Format("{0:0.##}", variationList[vari].original_price);
                                                    bool is_int_vari = int.TryParse(variTemp, out iVariTemp);

                                                    if (int.TryParse(variTemp, out iVariTemp))
                                                    {
                                                        vars.price = iVariTemp;
                                                    }
                                                    else
                                                    {
                                                        vars.price = variationList[vari].original_price;
                                                    }

                                                    vars.variation_sku = variationList[vari].variation_sku;
                                                    if (!dicVariationPromotionPrice.ContainsKey(variationList[vari].name.ToString()))
                                                    {
                                                        dicVariationPromotionPrice.Add(variationList[vari].name.ToString(), variationList[vari].price);
                                                        lst_vari.Add(vars);
                                                    }
                                                }
                                            }
                                        }

                                        var lst_attr = new List<shopee_attribute>();
                                        //설정한 속성값을 가지고 온다
                                        List<ItemAttributeDraftTar> attributeList = context.ItemAttributeDraftTars
                                            .Where(x => x.src_item_id == srcProductId
                                            && x.ItemInfoDraftId == ItemInfoDraftId
                                            && x.UserId == global_var.userId)
                                            .OrderBy(x => x.attribute_id).ToList();

                                        if (attributeList != null && attributeList.Count > 0)
                                        {
                                            for (int j = 0; j < attributeList.Count; j++)
                                            {
                                                var att = new shopee_attribute
                                                {
                                                    attributes_id = attributeList[j].attribute_id
                                                };
                                                var restClient = new RestClient($"https://shopeecategory.azurewebsites.net/api/CategoryDictionary/FindOriginalText?CountryCode={tarCountry}&TranslationText={attributeList[j].attribute_value}");
                                                var restRequest = new RestRequest
                                                {
                                                    Method = Method.GET
                                                };
                                                IRestResponse transResponse = restClient.Execute(restRequest);

                                                if (transResponse.StatusCode == HttpStatusCode.OK)
                                                {
                                                    dynamic ContentString = JsonConvert.DeserializeObject(transResponse.Content);
                                                    att.value = ContentString.OriginalText;
                                                }
                                                else
                                                {
                                                    att.value = attributeList[j].attribute_value;
                                                }

                                                lst_attr.Add(att);
                                            }
                                        }

                                        List<shopee_logi> lst_logi = new List<shopee_logi>();
                                        shopee_logi sh = new shopee_logi { logistic_id = logisticCode, enabled = true, shipping_fee = 0 };
                                        lst_logi.Add(sh);

                                        object[] obj_image = lst_img.ToArray();
                                        object[] obj_attr = lst_attr.ToArray();
                                        object[] obj_logi = lst_logi.ToArray();
                                        object[] obj_vari = lst_vari.ToArray();

                                        int dts = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_days_to_ship"].Value.ToString());

                                        var request = new RestRequest("", Method.POST);
                                        request.Method = Method.POST;
                                        request.AddHeader("Accept", "application/json");

                                        long long_time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));

                                        double decimal_compair_price = 0;
                                        string convert_price = "";

                                        if (double.TryParse(compair_price, out decimal_compair_price))
                                        {
                                            convert_price = string.Format("{0:0.##}", decimal_compair_price);
                                        }

                                        int int_val = 0;
                                        int int_val_original = 0;

                                        bool is_int = int.TryParse(convert_price, out int_val);

                                        var dic_json = new Dictionary<string, object>();
                                        dic_json.Add("category_id", Convert.ToInt32(tarCategoryId));
                                        dic_json.Add("name", sell_title);
                                        dic_json.Add("description", sbNewDesc.ToString());

                                        if (is_int)
                                        {
                                            dic_json.Add("price", int_val);
                                        }
                                        else
                                        {
                                            dic_json.Add("price", Convert.ToDouble(compair_price));
                                        }

                                        dic_json.Add("stock", iQty);
                                        dic_json.Add("item_sku", itemSku);
                                        dic_json.Add("images", obj_image);
                                        dic_json.Add("variations", obj_vari);
                                        dic_json.Add("attributes", obj_attr);
                                        dic_json.Add("logistics", obj_logi);
                                        dic_json.Add("weight", iWeight);
                                        if (dts > 6)
                                        {
                                            dic_json.Add("days_to_ship", dts);
                                        }
                                        else
                                        {
                                            dic_json.Add("days_to_ship", 2);
                                        }
                                        dic_json.Add("partner_id", partner_id);
                                        dic_json.Add("shopid", shop_id);
                                        dic_json.Add("timestamp", long_time_stamp);

                                        if (shopee_set_preorder != true && dts > 6)
                                        {
                                            request.AddJsonBody(JsonConvert.SerializeObject(dic_json));
                                        }
                                        else
                                        {
                                            request.AddJsonBody(JsonConvert.SerializeObject(dic_json));
                                        }

                                        var dddde = JsonConvert.SerializeObject(dic_json);
                                        request.AddHeader("authorization",
                                            HashString(end_point + "|" + JsonConvert.SerializeObject(dic_json),
                                            api_key));

                                        var client = new RestClient(end_point);
                                        IRestResponse response = client.Execute(request);
                                        var content = response.Content;

                                        try
                                        {
                                            //등록에 성공하였을 경우
                                            //상품 정보를 상품관리쪽으로 옮긴다.
                                            dynamic result = JsonConvert.DeserializeObject(content);

                                            if (result.msg.ToString().Contains("Add item success"))
                                            {
                                                StringBuilder strResultImage = new StringBuilder();
                                                for (int resultImg = 0; resultImg < result.item.images.Count; resultImg++)
                                                {
                                                    strResultImage.Append(result.item.images[resultImg].Value.ToString() + "^");
                                                }

                                                if (strResultImage.Length > 0)
                                                {
                                                    strResultImage.Remove(strResultImage.Length - 1, 1);
                                                }

                                                for (int logi = 0; logi < result.item.logistics.Count; logi++)
                                                {
                                                    ItemLogistic newLogistic = new ItemLogistic
                                                    {
                                                        enabled = result.item.logistics[logi].enabled,
                                                        estimated_shipping_fee = result.item.logistics[logi].estimated_shipping_fee ?? 0,
                                                        is_free = result.item.logistics[logi].is_free,
                                                        logistic_id = result.item.logistics[logi].logistic_id,
                                                        logistic_name = result.item.logistics[logi].logistic_name,
                                                        item_id = result.item_id,

                                                        //정의는 되어 있으나 필드가 안넘어옴
                                                        shipping_fee = result.item.logistics[logi].shipping_fee ?? 0,
                                                        size_id = result.item.logistics[logi].size_id ?? 0,
                                                        UserId = global_var.userId
                                                    };
                                                    context.ItemLogistics.Add(newLogistic);
                                                }

                                                for (int resultAtt = 0; resultAtt < result.item.attributes.Count; resultAtt++)
                                                {
                                                    ItemAttribute newAttribute = new ItemAttribute
                                                    {
                                                        attribute_id = result.item.attributes[resultAtt].attribute_id,
                                                        attribute_name = result.item.attributes[resultAtt].attribute_name,
                                                        is_mandatory = result.item.attributes[resultAtt].is_mandatory,
                                                        attribute_type = result.item.attributes[resultAtt].attribute_type,
                                                        attribute_value = result.item.attributes[resultAtt].attribute_value,
                                                        item_id = result.item_id,
                                                        UserId = global_var.userId
                                                    };
                                                    context.ItemAttributes.Add(newAttribute);
                                                }

                                                float fpackage_length = 0;
                                                float fpackage_width = 0;
                                                float fpackage_height = 0;

                                                if (result.item.package_length != null)
                                                {
                                                    fpackage_length = result.item.package_length;
                                                }

                                                if (result.item.package_width != null)
                                                {
                                                    fpackage_width = result.item.package_width;
                                                }

                                                if (result.item.package_length != null)
                                                {
                                                    fpackage_height = result.item.package_height;
                                                }



                                                decimal supplyPrice = 0;
                                                decimal margin = 0;
                                                int targetSellPriceKRW = 0;
                                                decimal currencyRate = 0;

                                                if (!string.IsNullOrEmpty(dgSrcItemList.Rows[i].Cells["dgItemList_supply_price"].Value.ToString().Trim()))
                                                {
                                                    decimal.TryParse(
                                                        dgSrcItemList.Rows[i].Cells["dgItemList_supply_price"].Value.ToString().Trim().Replace(",", ""), out supplyPrice);
                                                }

                                                if (!string.IsNullOrEmpty(dgSrcItemList.Rows[i].Cells["dgItemList_margin"].Value.ToString().Trim()))
                                                {
                                                    decimal.TryParse(
                                                        dgSrcItemList.Rows[i].Cells["dgItemList_margin"].Value.ToString().Trim().Replace(",", ""), out margin);
                                                }

                                                if (!string.IsNullOrEmpty(dgSrcItemList.Rows[i].Cells["dgItemList_price_won"].Value.ToString().Trim()))
                                                {
                                                    int.TryParse(
                                                        dgSrcItemList.Rows[i].Cells["dgItemList_price_won"].Value.ToString().Trim().Replace(",", ""), out targetSellPriceKRW);
                                                }

                                                if (!string.IsNullOrEmpty(dgSrcItemList.Rows[i].Cells["dgItemList_Currency_Rate"].Value.ToString().Trim()))
                                                {
                                                    decimal.TryParse(
                                                        dgSrcItemList.Rows[i].Cells["dgItemList_Currency_Rate"].Value.ToString().Trim().Replace(",", ""), out currencyRate);
                                                }

                                                //할인ID가 있는 경우에는 할인에 추가해 주어야 한다.
                                                var discountId = dgSrcItemList.Rows[i].Cells["dgItemList_discount_id"].Value.ToString().Trim();
                                                long ldiscountId = 0;
                                                if (discountId != string.Empty)
                                                {
                                                    ldiscountId = Convert.ToInt64(discountId);
                                                }

                                                string discountName = dgSrcItemList.Rows[i].Cells["dgItemList_discount_name"].Value.ToString().Trim();
                                                DateTime dtCurrencyDate = Convert.ToDateTime(dgSrcItemList.Rows[i].Cells["dgItemList_Currency_Date"].Value.ToString().Trim());

                                                DateTime dtCreateTime = UnixTimeStampToDateTime(Convert.ToInt64(result.item.create_time));
                                                DateTime dtUpdateTime = UnixTimeStampToDateTime(Convert.ToInt64(result.item.update_time));
                                                ItemInfo newItemInfo = new ItemInfo
                                                {
                                                    item_id = result.item_id,
                                                    shopeeAccount = shopeeId,
                                                    item_sku = result.item.item_sku,
                                                    status = result.item.status,
                                                    name = result.item.name,
                                                    description = result.item.description,
                                                    currency = result.item.currency,
                                                    has_variation = result.item.has_variation,
                                                    price = calcPrice,
                                                    pgFee = pgFee,
                                                    shopeeFee = UdShopeeFee.Value,
                                                    virtualBankFee = 0,
                                                    stock = iQty,
                                                    create_time = dtCreateTime,
                                                    update_time = dtUpdateTime,
                                                    weight = iWeight,
                                                    category_id = result.item.category_id,
                                                    original_price = result.item.original_price,
                                                    rating_star = result.item.rating_star,
                                                    cmt_count = result.item.cmt_count,
                                                    sales = result.item.sales,
                                                    views = result.item.views,
                                                    likes = result.item.likes,
                                                    package_length = fpackage_length,
                                                    package_width = fpackage_width,
                                                    package_height = fpackage_height,
                                                    days_to_ship = result.item.days_to_ship,
                                                    size_chart = result.item.size_chart,
                                                    discount_id = ldiscountId,
                                                    discount_name = discountName,
                                                    SetHeaderId = headerSetId,
                                                    SetHeaderName = headerSetName,
                                                    SetFooterId = footerSetId,
                                                    SetFooterName = footerSetName,
                                                    is_2tier_item = result.item.is_2tier_item,
                                                    condition = result.item.condition,
                                                    images = strResultImage.ToString(),
                                                    isChanged = false,
                                                    supply_price = supplyPrice,
                                                    margin = margin,
                                                    targetSellPriceKRW = targetSellPriceKRW,
                                                    currencyRate = currencyRate,
                                                    currencyDate = dtCurrencyDate,
                                                    savedDate = dtCreateTime,
                                                    UserId = global_var.userId
                                                };


                                                context.ItemInfoes.Add(newItemInfo);
                                                context.SaveChanges();


                                                if (discountId != string.Empty)
                                                {
                                                    dynamic variation = result.item.variations;
                                                    long item_id = result.item_id;
                                                    AddDiscountItems(ldiscountId, item_id, calcPrice, variation, dicVariationPromotionPrice,
                                                        partner_id, shop_id, api_key);
                                                }

                                                //등록을 성공한 상품은 등록기에서 삭제한다.
                                                ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                                b => b.src_item_id == srcProductId &&
                                                b.tar_shopeeAccount == shopeeId
                                                && b.UserId == global_var.userId);

                                                if (result_src_data != null)
                                                {
                                                    context.ItemInfoDrafts.Remove(result_src_data);
                                                    context.SaveChanges();
                                                    dgSrcItemList.Rows[i].Cells["dgItemList_Result"].Value = "성공";
                                                    dgSrcItemList.Rows[i].Cells["dgItemList_Result_message"].Value = result.item_id;
                                                    dgSrcItemList.Rows[i].Cells["dgItemList_Result"].Style.BackColor = Color.GreenYellow;
                                                    Application.DoEvents();
                                                }

                                                //맵핑하여 준다.
                                                ProductLink resultMapping = context.ProductLinks.SingleOrDefault(
                                                    b => b.SourceCountry == srcCountry 
                                                    && b.TargetCountry == tarCountry 
                                                    && b.SourceProductId == srcProductId
                                                    && b.UserId == global_var.userId);

                                                //만약 있는 경우라면 옮기는 것이므로 기존꺼는 흰색으로 새로 연결한 놈은 녹색으로
                                                if (result == null)
                                                {
                                                    ProductLink newProductLink = new ProductLink
                                                    {
                                                        SourceCountry = srcCountry,
                                                        TargetCountry = tarCountry,
                                                        SourceAccount = srcAccount,
                                                        SourceProductId = srcProductId,
                                                        TargetProductId = result.item_id,
                                                        TargetAccount = shopeeId,
                                                        UserId = global_var.userId
                                                    };

                                                    context.ProductLinks.Add(newProductLink);
                                                    context.SaveChanges();
                                                }

                                                MessageBox.Show("체크한 상품을 모두 등록 하였습니다. \r\n등록된 상품은 등록기에서 상품 관리로 이관 됩니다.", "등록완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else if (result.msg.ToString().Contains("Contains invalid attribute value"))
                                            {
                                                // 실패상황
                                                dgSrcItemList.Rows[i].Cells["dgItemList_Result"].Value = "실패";
                                                dgSrcItemList.Rows[i].Cells["dgItemList_Result"].Style.BackColor = Color.Orange;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_Result_message"].Value = result.msg;
                                                Application.DoEvents();
                                                MessageBox.Show($"체크한 상품 등록이 실패했습니다.\r\n쇼피 등록 실패메세지: {result.msg}\r\n셀러센터에 등록된 매칭되는 브랜드가 존재하지 않습니다.\r\nNo Brand로 다시 올려주세요.", "등록실패", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else
                                            {
                                                //실패상황
                                                dgSrcItemList.Rows[i].Cells["dgItemList_Result"].Value = "실패";
                                                dgSrcItemList.Rows[i].Cells["dgItemList_Result"].Style.BackColor = Color.Orange;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_Result_message"].Value = result.msg;
                                                Application.DoEvents();
                                                MessageBox.Show($"체크한 상품 등록이 실패했습니다.\r\n쇼피 등록 실패메세지: {result.msg}", "등록실패", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            //실패상황
                                            dgSrcItemList.Rows[i].Cells["dgItemList_Result"].Value = "실패";
                                            dgSrcItemList.Rows[i].Cells["dgItemList_Result"].Style.BackColor = Color.Orange;
                                            dgSrcItemList.Rows[i].Cells["dgItemList_Result_message"].Value = content;
                                            Application.DoEvents();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    Cursor.Current = Cursors.Default;
                }   
            }
        }

        public void AddDiscountItems(long discount_id, long item_id, decimal promotion_price,
            dynamic lstVariation, Dictionary<string, decimal> dicVariationPromotionPrice,
            long partner_id, long shop_id, string api_key)
        {

            //소수점 자리수에 따라 표시한다.
            int IntPromotionPrice = 0;
            bool isIntPromotionPrice = int.TryParse(string.Format("{0:#.##}", promotion_price), out IntPromotionPrice);
            


            //할인 이벤트에 설정한다.
            List<ShopeeDiscountItem> lstDiscountItems = new List<ShopeeDiscountItem>();
            using (ShopeeDiscountItem DiscountItem = new ShopeeDiscountItem())
            {
                DiscountItem.item_id = item_id;
                if (isIntPromotionPrice)
                {
                    DiscountItem.item_promotion_price = IntPromotionPrice;
                }
                else
                {
                    DiscountItem.item_promotion_price = promotion_price;
                }

                DiscountItem.purchase_limit = 1000;
                List<ShopeeDiscountVariation> lstDiscountVariations = new List<ShopeeDiscountVariation>();
                if (lstVariation != null)
                {
                    for (int i = 0; i < lstVariation.Count; i++)
                    {
                        using (ShopeeDiscountVariation discountVariation = new ShopeeDiscountVariation())
                        {
                            discountVariation.variation_id = lstVariation[i].variation_id;

                            decimal tempPromoPrice = dicVariationPromotionPrice[lstVariation[i].name.ToString()];
                            //소수점 자리수에 따라 표시한다.
                            int IntVariationPromotionPrice = 0;
                            bool isIntVariationPromotionPrice = int.TryParse(string.Format("{0:#.##}", tempPromoPrice), out IntVariationPromotionPrice);

                            if (isIntVariationPromotionPrice)
                            {
                                discountVariation.variation_promotion_price = IntVariationPromotionPrice;
                            }
                            else
                            {
                                discountVariation.variation_promotion_price = tempPromoPrice;
                            }

                            lstDiscountVariations.Add(discountVariation);
                        }
                    }
                }

                object[] objDiscountVariations = lstDiscountVariations.ToArray();

                DiscountItem.variations = objDiscountVariations;
                lstDiscountItems.Add(DiscountItem);
            }


            object[] objDiscountItems = lstDiscountItems.ToArray();

            long time_stamp_discount = ToUnixTime(DateTime.Now.AddHours(-9));
            string end_point_discount = "https://partner.shopeemobile.com/api/v1/discount/items/add";

            Dictionary<string, object> dic_discount = new Dictionary<string, object>();
            dic_discount.Add("discount_id", discount_id);
            dic_discount.Add("items", objDiscountItems);
            dic_discount.Add("partner_id", partner_id);
            dic_discount.Add("shopid", shop_id);
            dic_discount.Add("timestamp", time_stamp_discount);

            var client_discount = new RestClient(end_point_discount);
            var request_discount = new RestRequest("", Method.POST);
            request_discount.Method = Method.POST;
            request_discount.AddHeader("Accept", "application/json");
            request_discount.AddJsonBody(new
            {
                discount_id = discount_id,
                items = objDiscountItems,
                partner_id = partner_id,
                shopid = shop_id,
                timestamp = time_stamp_discount
            });

            request_discount.AddHeader("authorization",
                        HashString(end_point_discount + "|" + JsonConvert.SerializeObject(dic_discount),
                        api_key));
            IRestResponse response_discount = client_discount.Execute(request_discount);
            var content_discount = response_discount.Content;
            dynamic result_discount = null;
            if (!content_discount.Contains("504 Gateway Time-out") && !content_discount.Contains("502 Bad Gateway"))
            {
                try
                {
                    result_discount = JsonConvert.DeserializeObject(content_discount);
                }
                catch
                {

                }
            }
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private string FillHeaderSet(int headerSetId)
        {
            StringBuilder sbHeader = new StringBuilder();

            using (AppDbContext context = new AppDbContext())
            {
                var lst = (from template in context.TemplateHeaders
                           join H in context.HFLists
                           on template.HFListID equals H.HFListID
                           where template.SetHeaderId == headerSetId && template.UserId == global_var.userId
                           orderby template.OrderIdx
                           select new
                           {
                               HContent = H.HFContent
                           }).ToList();

                //List<TemplateHeader> headerList = context.TemplateHeaders
                //    .OrderBy(x => x.OrderIdx).ToList();

                for (int i = 0; i < lst.Count; i++)
                {
                    sbHeader.Append(lst[i].HContent.ToString() + "\r\n");
                }
            }

            return sbHeader.ToString();
        }

        private string FillFooterSet(int footerSetId)
        {
            StringBuilder sbFooter = new StringBuilder();

            using (AppDbContext context = new AppDbContext())
            {
                var lst = (from template in context.TemplateFooters
                           join H in context.HFLists
                           on template.HFListID equals H.HFListID
                           where template.SetFooterId == footerSetId && template.UserId == global_var.userId
                           orderby template.OrderIdx
                           select new
                           {
                               HContent = H.HFContent
                           }).ToList();

                //List<TemplateHeader> headerList = context.TemplateHeaders
                //    .OrderBy(x => x.OrderIdx).ToList();

                for (int i = 0; i < lst.Count; i++)
                {
                    sbFooter.Append(lst[i].HContent.ToString() + "\r\n");
                }
            }

            return sbFooter.ToString();
        }
        private void MenuLogistic_getList_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("쇼피 할인이벤트 목록을 동기화 하시겠습니까?", "할인 이벤트 동기화", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                dg_shopee_logistics.Rows.Clear();
                long partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                long shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                string api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
                string country = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                long long_time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));
                string end_point = "https://partner.shopeemobile.com/api/v1/logistics/channel/get";
                var client = new RestSharp.RestClient(end_point);

                Dictionary<string, object> dic_json = new Dictionary<string, object>();
                dic_json.Add("partner_id", partner_id);
                dic_json.Add("shopid", shop_id);
                dic_json.Add("timestamp", long_time_stamp);

                string body = JsonConvert.SerializeObject(dic_json);
                string auth_str = end_point + "|" + body;
                string authorization = HashString(auth_str, api_key);
                var request = new RestSharp.RestRequest(string.Empty, RestSharp.Method.POST);
                request.Method = Method.POST;
                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(new
                {
                    partner_id = Convert.ToInt32(partner_id),
                    shopid = Convert.ToInt32(shop_id),
                    timestamp = long_time_stamp
                });
                request.AddHeader("authorization", authorization);

                IRestResponse response = client.Execute(request);
                var content = response.Content;
                if (!string.IsNullOrEmpty(content))
                {
                    try
                    {
                        dynamic result = JsonConvert.DeserializeObject(content);
                        if (result.logistics != null)
                        {
                            //기존 데이터를 지우고 새로 저장한다.
                            using (AppDbContext context = new AppDbContext())
                            {
                                List<Logistic> logisticList = context.Logistics
                                .Where(x => x.shopeeAccount == shopeeId
                                && x.UserId == global_var.userId)
                                .OrderBy(x => x.logistic_id).ToList();

                                if(logisticList.Count > 0)
                                {
                                    context.Logistics.RemoveRange(logisticList);
                                    context.SaveChanges();
                                }
                            
                                for (int i = 0; i < result.logistics.Count; i++)
                                {
                                    if (country.Contains("SG"))
                                    {
                                        if (result.logistics[i].logistic_name.ToString().Contains("Standard Express - Korea"))
                                        {
                                            Logistic newLogic = new Logistic
                                            {
                                                shopeeAccount = shopeeId,
                                                logistic_id = result.logistics[i].logistic_id,
                                                logistic_name = result.logistics[i].logistic_name,
                                                fee_type = result.logistics[i].fee_type,
                                                has_cod = result.logistics[i].has_cod,
                                                UserId = global_var.userId
                                            };
                                            context.Logistics.Add(newLogic);
                                            context.SaveChanges();

                                            dg_shopee_logistics.Rows.Add(dg_shopee_logistics.Rows.Count + 1, false,
                                            result.logistics[i].logistic_name.ToString(),
                                            result.logistics[i].logistic_id.ToString(),
                                            result.logistics[i].has_cod.ToString(),
                                            result.logistics[i].fee_type.ToString());
                                        }
                                    }
                                    else if (country.Contains("ID") || country.Contains("MY"))
                                    {
                                        if (result.logistics[i].logistic_name.ToString().Contains("Standard Express - Korea") ||
                                        result.logistics[i].logistic_name.ToString().Contains("YS") ||
                                        result.logistics[i].logistic_name.ToString().Contains("EFS"))
                                        {
                                            Logistic newLogic = new Logistic
                                            {
                                                shopeeAccount = shopeeId,
                                                logistic_id = result.logistics[i].logistic_id,
                                                logistic_name = result.logistics[i].logistic_name,
                                                fee_type = result.logistics[i].fee_type,
                                                has_cod = result.logistics[i].has_cod,
                                                UserId = global_var.userId
                                            };
                                            context.Logistics.Add(newLogic);
                                            context.SaveChanges();

                                            dg_shopee_logistics.Rows.Add(dg_shopee_logistics.Rows.Count + 1, false,
                                            result.logistics[i].logistic_name,
                                            result.logistics[i].logistic_id,
                                            result.logistics[i].has_cod,
                                            result.logistics[i].fee_type);
                                        }
                                    }
                                    else if (country.Contains("TH"))
                                    {
                                        if (result.logistics[i].logistic_name.ToString().Contains("Standard Express-Doora"))
                                        {
                                            Logistic newLogic = new Logistic
                                            {
                                                shopeeAccount = shopeeId,
                                                logistic_id = result.logistics[i].logistic_id,
                                                logistic_name = result.logistics[i].logistic_name,
                                                fee_type = result.logistics[i].fee_type,
                                                has_cod = result.logistics[i].has_cod,
                                                UserId = global_var.userId
                                            };
                                            context.Logistics.Add(newLogic);
                                            context.SaveChanges();

                                            dg_shopee_logistics.Rows.Add(dg_shopee_logistics.Rows.Count + 1, false,
                                            result.logistics[i].logistic_name,
                                            result.logistics[i].logistic_id,
                                            result.logistics[i].has_cod,
                                            result.logistics[i].fee_type);
                                        }
                                    }
                                    else if (country.Contains("TW"))
                                    {
                                        if (result.logistics[i].logistic_name.ToString().Contains("YTO"))
                                        {
                                            Logistic newLogic = new Logistic
                                            {
                                                shopeeAccount = shopeeId,
                                                logistic_id = result.logistics[i].logistic_id,
                                                logistic_name = result.logistics[i].logistic_name,
                                                fee_type = result.logistics[i].fee_type,
                                                has_cod = result.logistics[i].has_cod,
                                                UserId = global_var.userId
                                            };
                                            context.Logistics.Add(newLogic);
                                            context.SaveChanges();

                                            dg_shopee_logistics.Rows.Add(dg_shopee_logistics.Rows.Count + 1, false,
                                            result.logistics[i].logistic_name,
                                            result.logistics[i].logistic_id,
                                            result.logistics[i].has_cod,
                                            result.logistics[i].fee_type);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
        private void PromotionMenu_getList_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("쇼피 할인이벤트 목록을 동기화 하시겠습니까?", "할인 이벤트 동기화", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                int pagination_offset = 0;
                int pagination_entries_per_page = 100;

                long partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                long shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                string api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();

                //계정별 데이터는 모두 삭제한다.
                using (AppDbContext context = new AppDbContext())
                {
                    List<Promotion> promotionList = context.Promotions
                    .Where(x => x.shopeeAccount == shopeeId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.discount_name).ToList();

                    context.Promotions.RemoveRange(promotionList);
                    context.SaveChanges();
                }

                ClsShopee shopee = new ClsShopee();
                var result = shopee.GetDiscountsList("ONGOING", pagination_offset, pagination_entries_per_page, partner_id, shop_id, api_key);
                if (result != null && result.msg == null)
                {
                    for (int i = 0; i < result.discount.Count; i++)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            Promotion newData = new Promotion
                            {
                                shopeeAccount = shopeeId,
                                discount_id = result.discount[i].discount_id,
                                discount_name = result.discount[i].discount_name,
                                start_time = ConvertFromUnixTimestamp(Convert.ToDouble(result.discount[i].start_time)),
                                end_time = ConvertFromUnixTimestamp(Convert.ToDouble(result.discount[i].end_time)),
                                status = "진행중",
                                UserId = global_var.userId
                            };
                            context.Promotions.Add(newData);
                            context.SaveChanges();
                        }
                    }
                }

                var result2 = shopee.GetDiscountsList("UPCOMING", pagination_offset, pagination_entries_per_page, partner_id, shop_id, api_key);
                if (result2 != null && result2.msg == null)
                {
                    for (int i = 0; i < result2.discount.Count; i++)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            Promotion newData = new Promotion
                            {
                                shopeeAccount = shopeeId,
                                discount_id = result2.discount[i].discount_id,
                                discount_name = result2.discount[i].discount_name,
                                start_time = ConvertFromUnixTimestamp(Convert.ToDouble(result2.discount[i].start_time)),
                                end_time = ConvertFromUnixTimestamp(Convert.ToDouble(result2.discount[i].end_time)),
                                status = "예정",
                                UserId = global_var.userId
                            };
                            context.Promotions.Add(newData);
                            context.SaveChanges();
                        }
                    }
                }

                dg_shopee_discount.Rows.Clear();
                using (AppDbContext context = new AppDbContext())
                {
                    List<Promotion> promotionList = context.Promotions
                        .Where(x => x.shopeeAccount == shopeeId
                        && x.UserId == global_var.userId)
                        .OrderBy(x => x.discount_name).ToList();

                    if (promotionList.Count > 0)
                    {
                        for (int i = 0; i < promotionList.Count; i++)
                        {
                            dg_shopee_discount.Rows.Add(i + 1,
                                false,
                                promotionList[i].discount_name,
                                promotionList[i].discount_id,
                                promotionList[i].start_time.ToString("yyyy-MM-dd"),
                                promotionList[i].end_time.ToString("yyyy-MM-dd"),
                                promotionList[i].status);
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
                MessageBox.Show("할인 이벤트 데이터를 동기화 하였습니다.", "할인 이벤트 동기화", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MainMenu_Set_Deacription_Click(object sender, EventArgs e)
        {

        }

        private void MainMenu_Set_Description_Click(object sender, EventArgs e)
        {
            if(dgSrcItemList.SelectedRows.Count > 0)
            {
                isChanged = false;
                FormDescriptionDraft fd = new FormDescriptionDraft();
                fd.fp = this;
                fd.ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                fd.ShowDialog();
                if (isChanged)
                {
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Style.BackColor = Color.Orange;
                }
            }
        }

        private void MainMenu_Set_discount_Click(object sender, EventArgs e)
        {
            BtnApplyPromotion_Click(null, null);
        }

        private void MainMenu_Remove_discount_Click(object sender, EventArgs e)
        {
            BtnRemovePromotion_Click(null, null);
        }

        private void BtnApplyHeaderSet_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }
            DialogResult dlg_Result = MessageBox.Show("체크한 상품의 헤더세트를 모두 적용하시겠습니까?", "헤더세트 일괄 적용", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                int headerID = Convert.ToInt32(dg_upload_header.SelectedRows[0].Cells["dg_upload_header_idx"].Value.ToString());
                string headerName = dg_upload_header.SelectedRows[0].Cells["dg_upload_header_name"].Value.ToString();
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                        string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.Id == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                            //원본데이터가 DB에 존재하여야 한다.
                            if (result_src_data != null)
                            {
                                result_src_data.SetHeaderId = headerID;
                                result_src_data.SetHeaderName = headerName;
                                context.SaveChanges();
                                dgSrcItemList.Rows[i].Cells["dgItemList_header_set"].Value = headerName;
                                dgSrcItemList.Rows[i].Cells["dgItemList_header_set_id"].Value = headerID;
                                dgSrcItemList.Rows[i].Cells["dgItemList_header_set"].Style.BackColor = Color.GreenYellow;
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("체크한 상품의 헤더세트를 모두 적용하였습니다.", "헤더세트 적용", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnRemoveHeaderSet_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }
            DialogResult dlg_Result = MessageBox.Show("체크한 상품의 헤더세트를 모두 제거하시겠습니까?", "헤더세트 일괄 제거", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.Id == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                            //원본데이터가 DB에 존재하여야 한다.
                            if (result_src_data != null)
                            {
                                result_src_data.SetHeaderId = 0;
                                result_src_data.SetHeaderName = "";
                                context.SaveChanges();
                                dgSrcItemList.Rows[i].Cells["dgItemList_header_set"].Value = "";
                                dgSrcItemList.Rows[i].Cells["dgItemList_discount_id"].Style.BackColor = Color.White;
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("체크한 상품의 헤더세트를 모두 제거하였습니다.", "헤더세트 제거", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnApplyFooterSet_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }

            DialogResult dlg_Result = MessageBox.Show("체크한 상품의 푸터세트를 모두 적용하시겠습니까?", "푸터세트 일괄 적용", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                int footerID = Convert.ToInt32(dg_upload_footer.SelectedRows[0].Cells["dg_upload_footer_idx"].Value.ToString());
                string footerName = dg_upload_footer.SelectedRows[0].Cells["dg_upload_footer_name"].Value.ToString();
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                        string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.Id == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                            //원본데이터가 DB에 존재하여야 한다.
                            if (result_src_data != null)
                            {
                                result_src_data.SetFooterId = footerID;
                                result_src_data.SetFooterName = footerName;
                                context.SaveChanges();
                                dgSrcItemList.Rows[i].Cells["dgItemList_footer_set"].Value = footerName;
                                dgSrcItemList.Rows[i].Cells["dgItemList_footer_set_id"].Value = footerID;
                                dgSrcItemList.Rows[i].Cells["dgItemList_footer_set"].Style.BackColor = Color.GreenYellow;
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("체크한 상품의 푸터세트를 모두 적용하였습니다.", "헤더세트 적용", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnRemoveFooterSet_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }
            DialogResult dlg_Result = MessageBox.Show("체크한 상품의 푸터세트를 모두 제거하시겠습니까?", "푸터세트 일괄 제거", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.Id == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                            //원본데이터가 DB에 존재하여야 한다.
                            if (result_src_data != null)
                            {
                                result_src_data.SetFooterId = 0;
                                result_src_data.SetFooterName = "";
                                context.SaveChanges();
                                dgSrcItemList.Rows[i].Cells["dgItemList_footer_set"].Value = "";
                                dgSrcItemList.Rows[i].Cells["dgItemList_discount_id"].Style.BackColor = Color.White;
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("체크한 상품의 푸터세트를 모두 제거하였습니다.", "헤더세트 제거", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MainMenu_upload_checked_Click(object sender, EventArgs e)
        {

        }

        private void dgSrcItemList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 12 || e.ColumnIndex == 13 || e.ColumnIndex == 19 
                || e.ColumnIndex == 24 || e.ColumnIndex == 33) // 1 should be your column index
            {
                int i;

                if (!int.TryParse(Convert.ToString(e.FormattedValue).Replace(",",""), out i))
                {
                    e.Cancel = true;
                }
                else
                {
                    // the input is numeric 
                }
            }
            else if(e.ColumnIndex == 17 || e.ColumnIndex == 18)
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
            else if(e.ColumnIndex == 22)
            {
                //무게 컬럼
                float f;

                if (!float.TryParse(Convert.ToString(e.FormattedValue).Replace(",", ""), out f))
                {
                    e.Cancel = true;
                }
                else
                {
                    // the input is numeric 
                }
            }
        }

        private void dg_shopee_discount_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                if (dg_shopee_discount.Rows.Count > 0 && dg_shopee_discount.SelectedRows.Count > 0
                    && dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
                {
                    //할인명 세팅
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_discount_id"].Value = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_id"].Value.ToString();
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_discount_name"].Value = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_name"].Value.ToString();
                    //저장한다.
                    using (AppDbContext context = new AppDbContext())
                    {
                        int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.Id == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                        if (result_src_data != null)
                        {
                            result_src_data.discount_id = Convert.ToInt32(dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_id"].Value.ToString());
                            result_src_data.discount_name = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_name"].Value.ToString();
                            context.SaveChanges();
                            //다음행으로 밀어준다.
                            if (dgSrcItemList.SelectedRows[0].Index < dgSrcItemList.Rows.Count - 1)
                            {
                                dgSrcItemList.Rows[dgSrcItemList.SelectedRows[0].Index + 1].Selected = true;
                                dgSrcItemList.FirstDisplayedScrollingRowIndex = dgSrcItemList.SelectedRows[0].Index;
                            }
                        }
                    }
                }
            }
        }

        private void dg_upload_header_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                //헤더세트
                if (dg_upload_header.Rows.Count > 0 && dg_upload_header.SelectedRows.Count > 0
                    && dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
                {
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_header_set"].Value = dg_upload_header.SelectedRows[0].Cells["dg_upload_header_name"].Value.ToString();
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_header_set_id"].Value = dg_upload_header.SelectedRows[0].Cells["dg_upload_header_idx"].Value.ToString();
                    //저장한다.
                    using (AppDbContext context = new AppDbContext())
                    {
                        int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.Id == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                        if (result_src_data != null)
                        {
                            result_src_data.SetHeaderId = Convert.ToInt32(dg_upload_header.SelectedRows[0].Cells["dg_upload_header_idx"].Value.ToString());
                            result_src_data.SetHeaderName = dg_upload_header.SelectedRows[0].Cells["dg_upload_header_name"].Value.ToString();
                            context.SaveChanges();

                            //다음행으로 밀어준다.
                            if(dgSrcItemList.SelectedRows[0].Index < dgSrcItemList.Rows.Count - 1)
                            {
                                dgSrcItemList.Rows[dgSrcItemList.SelectedRows[0].Index + 1].Selected = true;
                                dgSrcItemList.FirstDisplayedScrollingRowIndex = dgSrcItemList.SelectedRows[0].Index;
                            }
                        }
                    }
                }
            }
        }

        private void dg_upload_footer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                //푸터세트
                if (dg_upload_footer.Rows.Count > 0 && dg_upload_footer.SelectedRows.Count > 0
                    && dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
                {
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_footer_set"].Value = dg_upload_footer.SelectedRows[0].Cells["dg_upload_footer_name"].Value.ToString();
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_footer_set_id"].Value = dg_upload_footer.SelectedRows[0].Cells["dg_upload_footer_idx"].Value.ToString();
                    //저장한다.
                    using (AppDbContext context = new AppDbContext())
                    {
                        int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                        ItemInfoDraft result_src_data = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.Id == ItemInfoDraftId
                                && b.UserId == global_var.userId);

                        if (result_src_data != null)
                        {
                            result_src_data.SetFooterId = Convert.ToInt32(dg_upload_footer.SelectedRows[0].Cells["dg_upload_footer_idx"].Value.ToString());
                            result_src_data.SetFooterName = dg_upload_footer.SelectedRows[0].Cells["dg_upload_footer_name"].Value.ToString();
                            context.SaveChanges();
                            //다음행으로 밀어준다.
                            if (dgSrcItemList.SelectedRows[0].Index < dgSrcItemList.Rows.Count - 1)
                            {
                                dgSrcItemList.Rows[dgSrcItemList.SelectedRows[0].Index + 1].Selected = true;
                                dgSrcItemList.FirstDisplayedScrollingRowIndex = dgSrcItemList.SelectedRows[0].Index;
                            }
                        }
                    }
                }
            }
        }

        private void btn_attribute_auto_fill_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count == 0)
            {
                return;
            }
            DialogResult dlg_Result = MessageBox.Show("체크한 상품의 속성값을 원본의 값으로 모두 자동 적용하시겠습니까?\r\n적용전 카테고리 값을 확인하여 주세요.", "속성값 일괄 자동 적용", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                string tarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                long partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                long shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                string api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
                string tarShopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();

                //속성을 검증하기 위해서는 각 카테고리별로 대상의 필수속성의 개수를 기록하고 
                //이 개수와 저장된 개수를 비교하여 검증한다.
                Dictionary<int, int> dicCategory = new Dictionary<int, int>();
                using (AppDbContext context = new AppDbContext())
                {
                    for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                    {
                        string srcCurrency = dgSrcItemList.Rows[i].Cells["dgItemList_currency"].Value.ToString();
                        int tarCategoryId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_tar_category_id"].Value.ToString());
                        int ItemInfoDraftId = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_ItemInfoDraftId"].Value.ToString());
                        long srcProductId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                        string srcShopeeId = dgSrcItemList.Rows[i].Cells["dgItemList_src_shopeeId"].Value.ToString();

                        if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                        {
                            if (ItemInfoDraftId != 0)
                            {
                                //원본 상품에 설정되어 있는 값
                                //이 값을 최대한 활용 해야 한다.
                                List<ItemAttributeDraft> attributeSrcList = context.ItemAttributeDrafts
                                            .Where(b => b.ItemInfoDraftId == ItemInfoDraftId &&
                                            b.tar_shopeeAccount == tarShopeeId
                                            && b.UserId == global_var.userId)
                                            .OrderByDescending(x => x.is_mandatory).ToList();

                                Dictionary<string, string> dicSrcData = new Dictionary<string, string>();
                                for (int dic = 0; dic < attributeSrcList.Count; dic++)
                                {
                                    if (attributeSrcList[dic].attribute_value.Trim() != string.Empty)
                                    {
                                        dicSrcData.Add(attributeSrcList[dic].attribute_name.ToUpper(), attributeSrcList[dic].attribute_value);
                                    }
                                }

                                //원본사전의 값이 있는 경우가 있을때만 실행한다.
                                if (dicSrcData.Count > 0)
                                {
                                    //타겟값에 저장되어 있는 데이터를 가지고 온다.
                                    List<ItemAttributeDraftTar> attributeTarList = context.ItemAttributeDraftTars
                                                .Where(b => b.ItemInfoDraftId == ItemInfoDraftId
                                                && b.UserId == global_var.userId)
                                                .OrderByDescending(x => x.is_mandatory).ToList();

                                    //타겟의 속성중에 필수 속성을 개수를 센 후에 이 개수 만큼 다 채우면 완료이다.
                                    //필수 속성값의 개수를 구한다.
                                    int manCount = 0;
                                    int matchCount = 0;
                                    for (int manCnt = 0; manCnt < attributeTarList.Count; manCnt++)
                                    {
                                        if(attributeTarList[manCnt].is_mandatory)
                                        {
                                            manCount++;
                                        }
                                    }
                                    //타겟 카테고리의 속성목록을 모두 가지고 온다.
                                    List<AllAttributeList> savedAttrList = context.AllAttributeLists
                                    .Where(b => b.category_id == tarCategoryId &&
                                        b.country == tarCountry)
                                        .OrderByDescending(x => x.is_mandatory).ToList();

                                    //원본 속성값을 최대한 살려보자는 취지 이므로 없으면 아무것도 안하면 된다.
                                    for (int srcDic = 0; srcDic < dicSrcData.Count; srcDic++)
                                    {
                                        //타겟 속성값이 존재해야만 수행한다.

                                        if (savedAttrList.Count > 0)
                                        {
                                            //타겟 속성값에 뭔가 설정되어 있으면 설정하면 안된다. 원본 유지
                                            AttributeNameMap mapData = null;
                                            string mapAttrName = "";
                                            //원본이 인도네시아인 경우
                                            string mapSrcName = dicSrcData.Keys.ToList()[srcDic];
                                            if (srcCurrency == "IDR")
                                            {
                                                mapData = context.AttributeNameMaps.SingleOrDefault
                                                (x => x.ID == mapSrcName);
                                            }
                                            else if (srcCurrency == "SGD")
                                            {
                                                mapData = context.AttributeNameMaps.SingleOrDefault
                                                (b => b.SG == mapSrcName);
                                            }
                                            else if (srcCurrency == "MYR")
                                            {
                                                mapData = context.AttributeNameMaps.SingleOrDefault
                                                (b => b.MY == mapSrcName);
                                            }
                                            else if (srcCurrency == "THB")
                                            {
                                                mapData = context.AttributeNameMaps.SingleOrDefault
                                                (b => b.TH == mapSrcName);
                                            }
                                            else if (srcCurrency == "TWD")
                                            {
                                                mapData = context.AttributeNameMaps.SingleOrDefault
                                                (b => b.TW == mapSrcName);
                                            }
                                            else if (srcCurrency == "VND")
                                            {
                                                mapData = context.AttributeNameMaps.SingleOrDefault
                                                (b => b.VN == mapSrcName);
                                            }
                                            else if (srcCurrency == "PHP")
                                            {
                                                mapData = context.AttributeNameMaps.SingleOrDefault
                                                (b => b.PH == mapSrcName);
                                            }

                                            //맵데이터가 있다는 것은 동일한 것을 찾을 수 있다는 이야기
                                            if (mapData != null)
                                            {
                                                //자료가 있으면 일단 찾아본다.
                                                //무조건 1개만 있어야 함
                                                if (tarCountry == "ID")
                                                {
                                                    mapAttrName = mapData.ID.ToUpper();
                                                }
                                                else if (tarCountry == "MY")
                                                {
                                                    mapAttrName = mapData.MY.ToUpper();
                                                }
                                                else if (tarCountry == "SG")
                                                {
                                                    mapAttrName = mapData.SG.ToUpper();
                                                }
                                                else if (tarCountry == "TH")
                                                {
                                                    mapAttrName = mapData.TH.ToUpper();
                                                }
                                                else if (tarCountry == "TW")
                                                {
                                                    mapAttrName = mapData.TW.ToUpper();
                                                }
                                                else if (tarCountry == "VN")
                                                {
                                                    mapAttrName = mapData.VN.ToUpper();
                                                }
                                                else if (tarCountry == "PH")
                                                {
                                                    mapAttrName = mapData.PH.ToUpper();
                                                }


                                                for (int iDB = 0; iDB < savedAttrList.Count; iDB++)
                                                {
                                                    if (savedAttrList[iDB].attribute_name.ToUpper() == mapAttrName)
                                                    {
                                                        bool isExistData = false;
                                                        //이때 저장된 값이 있으면 패스하고 없는 경우에만 저장한다.
                                                        for (int tarAttr = 0; tarAttr < attributeTarList.Count; tarAttr++)
                                                        {
                                                            if (attributeTarList[tarAttr].attribute_id == savedAttrList[iDB].attribute_id)
                                                            {
                                                                //두개의 속성번호가 일치하는 경우 값이 있으면 저장하면 안된다.
                                                                if (attributeTarList[tarAttr].attribute_value.Trim() != string.Empty)
                                                                {
                                                                    isExistData = true;
                                                                    matchCount++;
                                                                    break;
                                                                }
                                                            }
                                                        }

                                                        if (!isExistData)
                                                        {
                                                            //저장한다.
                                                            //타겟의 이름이 일치하는 원본의 값을 입력한다.
                                                            ItemAttributeDraftTar newDraftData = new ItemAttributeDraftTar
                                                            {
                                                                attribute_id = savedAttrList[iDB].attribute_id,
                                                                attribute_name = savedAttrList[iDB].attribute_name,
                                                                attribute_type = savedAttrList[iDB].attribute_type,
                                                                attribute_value = dicSrcData.Values.ToList()[srcDic],
                                                                is_mandatory = savedAttrList[iDB].is_mandatory,
                                                                ItemInfoDraftId = ItemInfoDraftId,
                                                                src_item_id = srcProductId,
                                                                //src_shopeeAccount = srcShopeeId,
                                                                tar_shopeeAccount = tarShopeeId,
                                                                UserId = global_var.userId
                                                            };

                                                            context.ItemAttributeDraftTars.Add(newDraftData);
                                                            context.SaveChanges();
                                                            matchCount++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //속성값을 안넣어도 되는 케이스
                                            dgSrcItemList.Rows[i].Cells["dgItemList_attribute"].Value = true;
                                            dgSrcItemList.Rows[i].Cells["dgItemList_attribute"].Style.BackColor = Color.GreenYellow;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("체크한 모든 상품에 대하여 속성값이 없는 상품 데이터를 모두 자동맵핑 하였습니다.", "자동맵핑 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
                
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Menu_Logistic_Sync_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("쇼피 배송코드 목록을 동기화 하시겠습니까?", "배송코드 동기화", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                dg_shopee_logistics.Rows.Clear();
                long partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                long shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                string api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
                string country = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                long long_time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));
                string end_point = "https://partner.shopeemobile.com/api/v1/logistics/channel/get";
                var client = new RestSharp.RestClient(end_point);

                Dictionary<string, object> dic_json = new Dictionary<string, object>();
                dic_json.Add("partner_id", partner_id);
                dic_json.Add("shopid", shop_id);
                dic_json.Add("timestamp", long_time_stamp);

                string body = JsonConvert.SerializeObject(dic_json);
                string auth_str = end_point + "|" + body;
                string authorization = HashString(auth_str, api_key);
                var request = new RestSharp.RestRequest(string.Empty, RestSharp.Method.POST);
                request.Method = Method.POST;
                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(JsonConvert.SerializeObject(dic_json));
                request.AddHeader("authorization", authorization);

                IRestResponse response = client.Execute(request);
                var content = response.Content;
                if (!string.IsNullOrEmpty(content))
                {
                    try
                    {
                        dynamic result = JsonConvert.DeserializeObject(content);
                        if (result.logistics != null)
                        {
                            //기존 데이터를 지우고 새로 저장한다.
                            using (AppDbContext context = new AppDbContext())
                            {
                                List<Logistic> logisticList = context.Logistics
                                .Where(x => x.shopeeAccount == shopeeId
                                && x.UserId == global_var.userId)
                                .OrderBy(x => x.logistic_id).ToList();

                                if (logisticList.Count > 0)
                                {
                                    context.Logistics.RemoveRange(logisticList);
                                    context.SaveChanges();
                                }

                                for (int i = 0; i < result.logistics.Count; i++)
                                {
                                    if (country.Contains("SG") || country.Contains("PH"))
                                    {
                                        if (result.logistics[i].logistic_name.ToString().Contains("Standard Express - Korea"))
                                        {
                                            Logistic newLogic = new Logistic
                                            {
                                                shopeeAccount = shopeeId,
                                                logistic_id = result.logistics[i].logistic_id,
                                                logistic_name = result.logistics[i].logistic_name,
                                                fee_type = result.logistics[i].fee_type,
                                                has_cod = result.logistics[i].has_cod,
                                                UserId = global_var.userId
                                            };
                                            context.Logistics.Add(newLogic);
                                            context.SaveChanges();

                                            dg_shopee_logistics.Rows.Add(dg_shopee_logistics.Rows.Count + 1, false,
                                            result.logistics[i].logistic_name.ToString(),
                                            result.logistics[i].logistic_id.ToString(),
                                            result.logistics[i].has_cod.ToString(),
                                            result.logistics[i].fee_type.ToString());
                                        }
                                    }
                                    else if (country.Contains("ID") || country.Contains("MY"))
                                    {
                                        string isEnabled = result.logistics[i].enabled.ToString();
                                        if (isEnabled.Contains("True"))
                                        {
                                            Logistic newLogic = new Logistic
                                            {
                                                shopeeAccount = shopeeId,
                                                logistic_id = result.logistics[i].logistic_id,
                                                logistic_name = result.logistics[i].logistic_name,
                                                fee_type = result.logistics[i].fee_type,
                                                has_cod = result.logistics[i].has_cod,
                                                UserId = global_var.userId
                                            };
                                            context.Logistics.Add(newLogic);
                                            context.SaveChanges();

                                            dg_shopee_logistics.Rows.Add(dg_shopee_logistics.Rows.Count + 1, false,
                                            result.logistics[i].logistic_name,
                                            result.logistics[i].logistic_id,
                                            result.logistics[i].has_cod,
                                            result.logistics[i].fee_type);
                                        }
                                    }
                                    else if (country.Contains("TH"))
                                    {
                                        if (result.logistics[i].logistic_name.ToString().Contains("Standard Express-Doora"))
                                        {
                                            Logistic newLogic = new Logistic
                                            {
                                                shopeeAccount = shopeeId,
                                                logistic_id = result.logistics[i].logistic_id,
                                                logistic_name = result.logistics[i].logistic_name,
                                                fee_type = result.logistics[i].fee_type,
                                                has_cod = result.logistics[i].has_cod,
                                                UserId = global_var.userId
                                            };
                                            context.Logistics.Add(newLogic);
                                            context.SaveChanges();

                                            dg_shopee_logistics.Rows.Add(dg_shopee_logistics.Rows.Count + 1, false,
                                            result.logistics[i].logistic_name,
                                            result.logistics[i].logistic_id,
                                            result.logistics[i].has_cod,
                                            result.logistics[i].fee_type);
                                        }
                                    }
                                    else if (country.Contains("VN"))
                                    {
                                        if (result.logistics[i].logistic_name.ToString().Contains("Standard Express - Doora"))
                                        {
                                            Logistic newLogic = new Logistic
                                            {
                                                shopeeAccount = shopeeId,
                                                logistic_id = result.logistics[i].logistic_id,
                                                logistic_name = result.logistics[i].logistic_name,
                                                fee_type = result.logistics[i].fee_type,
                                                has_cod = result.logistics[i].has_cod,
                                                UserId = global_var.userId
                                            };
                                            context.Logistics.Add(newLogic);
                                            context.SaveChanges();

                                            dg_shopee_logistics.Rows.Add(dg_shopee_logistics.Rows.Count + 1, false,
                                                result.logistics[i].logistic_name,
                                                result.logistics[i].logistic_id,
                                                result.logistics[i].has_cod,
                                                result.logistics[i].fee_type);
                                        }
                                    }
                                    else if (country.Contains("TW"))
                                    {
                                        if (result.logistics[i].logistic_name.ToString().Contains("YTO"))
                                        {
                                            Logistic newLogic = new Logistic
                                            {
                                                shopeeAccount = shopeeId,
                                                logistic_id = result.logistics[i].logistic_id,
                                                logistic_name = result.logistics[i].logistic_name,
                                                fee_type = result.logistics[i].fee_type,
                                                has_cod = result.logistics[i].has_cod,
                                                UserId = global_var.userId
                                            };
                                            context.Logistics.Add(newLogic);
                                            context.SaveChanges();

                                            dg_shopee_logistics.Rows.Add(dg_shopee_logistics.Rows.Count + 1, false,
                                            result.logistics[i].logistic_name,
                                            result.logistics[i].logistic_id,
                                            result.logistics[i].has_cod,
                                            result.logistics[i].fee_type);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void PromotionMenu_Sync_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("쇼피 할인이벤트 목록을 동기화 하시겠습니까?", "할인 이벤트 동기화", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                int pagination_offset = 0;
                int pagination_entries_per_page = 100;

                long partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                long shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                string api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();

                //계정별 데이터는 모두 삭제한다.
                using (AppDbContext context = new AppDbContext())
                {
                    List<Promotion> promotionList = context.Promotions
                    .Where(x => x.shopeeAccount == shopeeId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.discount_name).ToList();

                    context.Promotions.RemoveRange(promotionList);
                    context.SaveChanges();
                }

                ClsShopee shopee = new ClsShopee();
                var result = shopee.GetDiscountsList("ONGOING", pagination_offset, pagination_entries_per_page, partner_id, shop_id, api_key);
                if (result != null && result.msg == null)
                {
                    for (int i = 0; i < result.discount.Count; i++)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            Promotion newData = new Promotion
                            {
                                shopeeAccount = shopeeId,
                                discount_id = result.discount[i].discount_id,
                                discount_name = result.discount[i].discount_name,
                                start_time = ConvertFromUnixTimestamp(Convert.ToDouble(result.discount[i].start_time)),
                                end_time = ConvertFromUnixTimestamp(Convert.ToDouble(result.discount[i].end_time)),
                                status = "진행중",
                                UserId = global_var.userId
                            };
                            context.Promotions.Add(newData);
                            context.SaveChanges();
                        }
                    }
                }

                var result2 = shopee.GetDiscountsList("UPCOMING", pagination_offset, pagination_entries_per_page, partner_id, shop_id, api_key);
                if (result2 != null && result2.msg == null)
                {
                    for (int i = 0; i < result2.discount.Count; i++)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            Promotion newData = new Promotion
                            {
                                shopeeAccount = shopeeId,
                                discount_id = result2.discount[i].discount_id,
                                discount_name = result2.discount[i].discount_name,
                                start_time = ConvertFromUnixTimestamp(Convert.ToDouble(result2.discount[i].start_time)),
                                end_time = ConvertFromUnixTimestamp(Convert.ToDouble(result2.discount[i].end_time)),
                                status = "예정",
                                UserId = global_var.userId
                            };
                            context.Promotions.Add(newData);
                            context.SaveChanges();
                        }
                    }
                }

                dg_shopee_discount.Rows.Clear();
                using (AppDbContext context = new AppDbContext())
                {
                    List<Promotion> promotionList = context.Promotions
                        .Where(x => x.shopeeAccount == shopeeId
                        && x.UserId == global_var.userId)
                        .OrderBy(x => x.discount_name).ToList();

                    if (promotionList.Count > 0)
                    {
                        for (int i = 0; i < promotionList.Count; i++)
                        {
                            dg_shopee_discount.Rows.Add(i + 1,
                                false,
                                promotionList[i].discount_name,
                                promotionList[i].discount_id,
                                promotionList[i].start_time.ToString("yyyy-MM-dd"),
                                promotionList[i].end_time.ToString("yyyy-MM-dd"),
                                promotionList[i].status);
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
                MessageBox.Show("할인 이벤트 데이터를 동기화 하였습니다.", "할인 이벤트 동기화", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgSrcItemList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgSrcItemList.ClearSelection();
        }

        private void PromotionMenu_Create_Click(object sender, EventArgs e)
        {
            FormAddDiscountEvent form = new FormAddDiscountEvent();
            form.i_partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
            form.i_shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
            form.api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
            form.ShowDialog();

            PromotionMenu_Sync_Click(null, null);
        }

        private void PromotionMenu_Delete_Click(object sender, EventArgs e)
        {
            if(dg_shopee_discount.Rows.Count > 0 && dg_shopee_discount.SelectedRows.Count > 0)
            {
                FormDeleteDiscountEvent form = new FormDeleteDiscountEvent();
                form.discount_id = Convert.ToInt64(dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_id"].Value.ToString());
                form.i_partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                form.i_shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                form.api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
                form.ShowDialog();

                PromotionMenu_Sync_Click(null, null);
            }
        }

        private void Txt_src_currency_rate_ValueChanged(object sender, EventArgs e)
        {
            using(AppDbContext context = new AppDbContext())
            {
                if (cbo_currency_From.Text.Contains("PHP"))
                {
                    var res = context.CurrencyRates.FirstOrDefault(x => x.cr_code.Contains("PHP") && x.UserId == global_var.userId);

                    if(res != null)
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

        private void BtnSaveCurrencyRate_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("환율을 저장 하시겠습니까?", "환율 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            string Code = string.Empty;

            if (dlg_Result == DialogResult.Yes)
            {
                if (cbo_currency_From.Text.Contains("PHP"))
                {
                    Code = "PHP";
                }
                else if (cbo_currency_From.Text.Contains("IDR"))
                {
                    Code = "IDR";
                }
                else if (cbo_currency_From.Text.Contains("SGD"))
                {
                    Code = "SGD";
                }
                else if (cbo_currency_From.Text.Contains("MYR"))
                {
                    Code = "MYR";
                }
                else if (cbo_currency_From.Text.Contains("THB"))
                {
                    Code = "THB";
                }
                else if (cbo_currency_From.Text.Contains("TWD"))
                {
                    Code = "TWD";
                }
                else if (cbo_currency_From.Text.Contains("VND"))
                {
                    Code = "VND";
                }

                using (var context = new AppDbContext())
                {
                    var res = context.CurrencyRates.FirstOrDefault(x => x.cr_code == Code && x.UserId == global_var.userId);

                    if (res != null)
                    {
                        res.cr_exchange = txt_src_currency_rate.Value;
                        context.SaveChanges();
                    }

                    Fill_from_Currency_Names();
                }

                cbo_currency_From.SelectedIndex = dg_site_id.Rows.IndexOf(dg_site_id.SelectedRows[0]);
            }
        }
    }
}
