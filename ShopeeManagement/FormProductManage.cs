using MetroFramework.Forms;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormProductManage : MetroForm
    {
        List<long> lstSrcItemId = new List<long>();
        private CancellationTokenSource _cts;
        public Dictionary<long, long> dicSrcItemList = new Dictionary<long, long>();

        public Dictionary<string, long> dicExpireAccount = new Dictionary<string, long>();
        public FormProductManage(string Lang)
        {
            InitializeComponent();
            MinimizeBox = false;
            MaximizeBox = false;
            AutoSize = true;
            AutoScroll = true;
            WindowState = FormWindowState.Normal;
            toolTipDownLoadData.SetToolTip(btnGetSrcItemList, "상품정보를 셀러센터로부터 현재 컴퓨터로 내려받습니다.\r\n기존 내려받은 상품정보는 업데이트 하지 않습니다. \r\n새로운 정보로 내려 받기를 원할 경우 해당 상품 정보를 삭제한 후 내려받아주세요");
            //toolTipAccountRefresh.SetToolTip(btnGetAccount, "쇼피 계정 정보를 새로 가져옵니다.");
            toolTipUpdate.SetToolTip(BtnUpdateProduct, "체크한 상품의 정보를 셀러센터로 전송하여 업데이트 합니다.");
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

            if(dg_site_id.Rows.Count > 0 && dg_site_id.SelectedRows.Count > 0)
            {
                string countryCode = "";
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
                        txt_src_currency_rate.Text = currency_info.Value.ToString();
                        break;
                    }
                }
            }
            
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
                else
                {
                    MessageBox.Show("환율 데이터가 없습니니다.\r\n환율 갱신 버튼을 클릭하여 주세요.","환율 갱신",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
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
                }
            }
        }
        private void FormProductManage_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            CboSearchType.SelectedIndex = 0;
            Fill_Currency_Date();
            Fill_from_Currency_Names();
            dt_start.Value = DateTime.Now.AddYears(-1);
            getShopeeAccount();

            if(dg_site_id.Rows.Count > 0 && dg_site_id.SelectedRows.Count > 0)
            {
                DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(0, 0);
                dg_site_id_CellClick(null, et);
            }

            setDefaultVar();
        }
        private void getShopeeDiscount(int pagination_offset, int pagination_entries_per_page,
            int partner_id, int shop_id, string api_key)
        {
            dg_shopee_discount.Rows.Clear();
            ClsShopee shopee = new ClsShopee();
            var result = shopee.GetDiscountsList("ONGOING", pagination_offset, pagination_entries_per_page, partner_id, shop_id, api_key);
            if (result != null && result.msg == null)
            {
                for (int i = 0; i < result.discount.Count; i++)
                {
                    dg_shopee_discount.Rows.Add(i + 1, false,
                        result.discount[i].discount_name.ToString(),
                        result.discount[i].discount_id.ToString(),
                        ConvertFromUnixTimestamp(Convert.ToDouble(result.discount[i].start_time)),
                        ConvertFromUnixTimestamp(Convert.ToDouble(result.discount[i].end_time)),
                        "진행중");
                }
            }

            var result2 = shopee.GetDiscountsList("UPCOMING", pagination_offset, pagination_entries_per_page, partner_id, shop_id, api_key);
            if (result2 != null && result.msg == null)
            {
                for (int i = 0; i < result2.discount.Count; i++)
                {
                    dg_shopee_discount.Rows.Add(dg_shopee_discount.RowCount + 1, false,
                        result2.discount[i].discount_name.ToString(),
                        result2.discount[i].discount_id.ToString(),
                        ConvertFromUnixTimestamp(Convert.ToDouble(result2.discount[i].start_time)),
                        ConvertFromUnixTimestamp(Convert.ToDouble(result2.discount[i].end_time)),
                        "예정");
                }
            }
        }

        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
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

                if(ShopeeAccounts.Count > 0)
                {
                    DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(0, 0);
                    dg_site_id_CellClick(null, et);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getShopeeAccount: " + ex.Message);
            }

        }
        private void getProductList(string area, string keyword)
        {
            DataGridView dv = null;
            if(tabMain.SelectedIndex == 0)
            {
                dv = dgItemList;
            }
            else
            {
                dv = dgItemList2;
            }


            dv.Rows.Clear();

            if (dg_site_id.SelectedRows.Count > 0)
            {
                keyword = keyword.ToUpper();
                Cursor.Current = Cursors.WaitCursor;
                decimal rateSrc = 0;
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
                    currencyDigit = "{0:n2}";
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
                else if (shopeeId.ToUpper().Contains(".VN"))
                {
                    currencyDigit = "{0:n0}";
                    countryCode = "VND";
                }




                //환율을 가지고 온다.
                rateSrc = txt_src_currency_rate.Value;
                

                using (AppDbContext context = new AppDbContext())
                {
                    List<ItemInfo> productList = null;
                    if (area == "상품ID")
                    {
                        long ItemId = 0;
                        if (keyword != string.Empty)
                        {
                            if (long.TryParse(keyword, out ItemId))
                            {
                                productList = context.ItemInfoes
                                .Where(b => b.shopeeAccount == shopeeId &&
                                b.item_id.ToString().Contains(ItemId.ToString())
                                && b.UserId == global_var.userId)
                                .OrderBy(x => x.update_time).ToList();
                            }
                        }
                    }
                    else if (area == "상품SKU")
                    {                     
                        if (keyword != string.Empty)
                        {
                            productList = context.ItemInfoes
                                .Where(b => b.shopeeAccount == shopeeId &&
                                b.item_sku.ToUpper().Contains(keyword)
                                && b.UserId == global_var.userId)
                                .OrderBy(x => x.update_time).ToList();
                        }
                    }
                    else if (area == "상품명")
                    {                     
                        if (keyword != string.Empty)
                        {
                            productList = context.ItemInfoes
                                .Where(b => b.shopeeAccount == shopeeId &&
                                b.name.ToUpper().Contains(keyword)
                                && b.UserId == global_var.userId)
                                .OrderBy(x => x.update_time).ToList();
                        }
                    }
                    else if(area == "")
                    {
                        productList = context.ItemInfoes
                        .Where(b => b.shopeeAccount == shopeeId
                        && b.UserId == global_var.userId)
                        .OrderByDescending(x => x.update_time).ToList();
                    }
                    
                    if(productList != null)
                    {
                        progressBar.Value = 0;
                        progressBar.Maximum = productList.Count;

                        tabMain.TabPages[0].Text = "상품 목록 : [ " + string.Format("{0:n0}", productList.Count) + " ]";
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

                            decimal SrcPrice = productList[i].price;
                            System.Drawing.Image img = null;
                            dv.Rows.Add(
                                i + 1,
                                false,
                                img,
                                productList[i].item_id,
                                productList[i].shopid,
                                productList[i].item_sku,
                                productList[i].status,
                                productList[i].name,
                                productList[i].currency,
                                (bool)productList[i].has_variation,
                                string.Format("{0:n0}", productList[i].supply_price),
                                string.Format("{0:n0}", productList[i].margin),
                                "",
                                string.Format(currencyDigit, productList[i].pgFee),
                                string.Format("{0:n0}", productList[i].targetSellPriceKRW),
                                string.Format(currencyDigit, productList[i].price),
                                productList[i].original_price,
                                //string.Format(currencyDigit, productList[i].original_price),
                                string.Format("{0:n0}", productList[i].stock),
                                productList[i].create_time,
                                productList[i].update_time,
                                productList[i].savedDate,
                                productList[i].weight,
                                productList[i].category_id,
                                Math.Truncate(productList[i].rating_star * 100) / 100,
                                productList[i].cmt_count,
                                productList[i].sales,
                                productList[i].views,
                                productList[i].likes,
                                productList[i].package_length,
                                productList[i].package_width,
                                productList[i].package_height,
                                productList[i].days_to_ship,
                                productList[i].size_chart,
                                productList[i].condition,
                                productList[i].discount_id,
                                productList[i].discount_name,
                                productList[i].is_2tier_item,
                                strImage);

                            if(productList[i].isChanged)
                            {
                                dv.Rows[dv.Rows.Count - 1].Cells[dv.Name + "_item_id"].Style.BackColor = Color.SkyBlue;
                            }

                            if (productList[i].status != "NORMAL")
                            {
                                dv.Rows[dv.Rows.Count - 1].Cells[dv.Name + "_status"].Style.BackColor = Color.Orange;
                            }                            
                        }

                        CancellationTokenSource cts = new CancellationTokenSource();

                        Task.Run(() =>
                        {
                            try
                            {
                                Parallel.ForEach(dv.Rows.Cast<DataGridViewRow>(),
                                    new ParallelOptions { MaxDegreeOfParallelism = 200 }
                                    , dgRow =>
                                    {
                                        var retryCount = 0;
                                        Retry:
                                        try
                                        {
                                            if (cts.IsCancellationRequested) return;

                                            var iUrl = dgRow.Cells[dv.Name + "_images"].Value.ToString() as string;
                                            var ic = dgRow.Cells[dv.Name + "_Image"] as DataGridViewImageCell;

                                            var request = WebRequest.Create(iUrl);
                                            var response = (HttpWebResponse)request.GetResponse();
                                            var dataStream = response.GetResponseStream();

                                            ic.Value = new Bitmap(dataStream);

                                            progressBar.BeginInvoke(new InvokeDelegate(() => Progress(cts)));
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

                groupBox3.Text = "상품 목록 : [ " + string.Format("{0:n0}", dv.Rows.Count) + " ]";
                dv.ClearSelection();
                Cursor.Current = Cursors.Default;
            }
        }
        
        private void BtnProductList_Click(object sender, EventArgs e)
        {   
            
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
        public delegate void InvokeDelegate();
        private void btnGetSrcItemList_Click(object sender, EventArgs e)
        {
            if (dg_site_id.Rows.Count == 0 && dg_site_id.SelectedRows.Count == 0)
            {
                MessageBox.Show("아이디를 선택 하세요.", "아이디 선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dlg_Result = MessageBox.Show("쇼피셀러센터로 부터 상품 데이터를 저장하시겠습니까?\r\n이미 내려받은 자료는 업데이트 하지 않습니다. 업데이트가 필요 할 경우 삭제 후 내려받아 주세요.", "상품 데이터 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Application.UseWaitCursor = true;

                PromotionMenu_getList_Click(null, null);

                dgItemList.Rows.Clear();

                lstSrcItemId.Clear();
                dicSrcItemList.Clear();

                progressBar.Value = 0;
                groupBox3.Text = "상품 목록";

                Application.DoEvents();
                int ipagination_offset = 0;
                long src_partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                long src_shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                string src_api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();

                
                Dictionary<long, long> dicDate = new Dictionary<long, long>();
                Dictionary<DateTime, DateTime> dicDate2 = new Dictionary<DateTime, DateTime>();

                //Dictionary<DateTime, DateTime> dicDate2 = new Dictionary<DateTime, DateTime>();
                //유닉스 시간은 9시간 차이가 난다.
                DateTime startDate = new DateTime(dt_start.Value.Year, dt_start.Value.Month, dt_start.Value.Day, 0, 0, 0);
                //startDate = startDate.AddHours(+9);
                DateTime endDate = new DateTime(dt_end.Value.Year, dt_end.Value.Month, dt_end.Value.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                //endDate = endDate.AddHours(+9);

                //DateTime startDate = dt_start.Value;
                //startDate = startDate.AddHours(-9);

                //DateTime endDate = dt_end.Value;
                TimeSpan TS = endDate - startDate;
                
                if (TS.Days > 14)
                {
                    while (startDate < endDate)
                    {
                        DateTime tempDate = endDate.AddDays(-14);
                        dicDate.Add(ToUnixTime(tempDate), ToUnixTime(endDate));
                        dicDate2.Add(tempDate, endDate.AddDays(-14));
                        endDate = endDate.AddDays(-14);
                    }
                }
                else
                {
                    dicDate.Add(ToUnixTime(startDate), ToUnixTime(endDate));
                }

                Dictionary<long, string> dicDiscount = new Dictionary<long, string>();
                for (int i = 0; i < dg_shopee_discount.Rows.Count; i++)
                {
                    dicDiscount.Add(Convert.ToInt64(dg_shopee_discount.Rows[i].Cells["dg_shopee_discount_discount_id"].Value.ToString()),
                        dg_shopee_discount.Rows[i].Cells["dg_shopee_discount_discount_name"].Value.ToString());
                }
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                getSrcItem(src_partner_id, src_shop_id, src_api_key, dicDate, shopeeId, dicDiscount);
            }   
        }

        private static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        private void getSrcItem(long src_partner_id, long src_shop_id, string src_api_key,
            Dictionary<long, long> dicDate, string shopeeId, Dictionary<long, string> dicDiscount)
        {
            foreach (KeyValuePair<long, long> items in dicDate)
            {
                int ipagination_offset = 0;
                bool rtn = false;
                rtn = getSrcItemList(src_partner_id, src_shop_id, src_api_key, ipagination_offset, items.Key, items.Value, shopeeId);

                if (rtn)
                {
                    while (rtn)
                    {
                        ipagination_offset = ipagination_offset + 100;
                        rtn = getSrcItemList(src_partner_id, src_shop_id, src_api_key, ipagination_offset, items.Key, items.Value, shopeeId);
                    }
                }
                #region  EDITED For 이전 이미지 병렬 로드 취소
                _cts?.Cancel();
                _cts = new CancellationTokenSource();
                #endregion
            }

            progressBar.Maximum = dicSrcItemList.Count;
            var aaa = lstSrcItemId.Count;
            var bbb = dicSrcItemList.Count;
            FetchItemData(src_partner_id, src_shop_id, src_api_key, _cts, dicSrcItemList.Keys.ToList(), shopeeId, dicDiscount);
        }

        public static void UnLockMaxParallel()
        {
            int prevThreads, prevPorts;
            ThreadPool.GetMinThreads(out prevThreads, out prevPorts);
            ThreadPool.SetMinThreads(2000, prevPorts);
        }

        

        private void FetchItemData(long i_partner_id, long i_shop_id, string api_key, CancellationTokenSource cts, 
            List<long> lstItem, string SrcShopeeAccount, Dictionary<long, string>dicDiscount)
        {
            #region EDITED For 이미지 병렬 로드
            UnLockMaxParallel();
            #endregion
            var current_synchronization_context = TaskScheduler.FromCurrentSynchronizationContext();

            int cur_idx = 0;
            Application.UseWaitCursor = true;

            ShopeeApi shopeeApi = new ShopeeApi();
            try
            {
                Task.Run(() =>
                {
                    Parallel.ForEach(lstItem, new ParallelOptions { MaxDegreeOfParallelism = 50 }, LstRow =>
                    {   
                        if (cts.IsCancellationRequested) return;
                        var item_id = LstRow;
                        if (item_id != 0)
                        {
                            using (AppDbContext context = new AppDbContext())
                            {
                                ItemInfo result = context.ItemInfoes.SingleOrDefault(
                                b => b.item_id == item_id && b.shopeeAccount == SrcShopeeAccount
                                && b.UserId == global_var.userId);

                                if(result == null)
                                {
                                    dynamic ItemData = shopeeApi.GetItemDetail(item_id, i_partner_id, i_shop_id, api_key);

                                    if (ItemData != null)
                                    {
                                        //상품 정보를 저장한다.
                                        DateTime dtCreateTime = UnixTimeStampToDateTime(Convert.ToInt64(ItemData.item.create_time));
                                        DateTime dtUpdateTime = UnixTimeStampToDateTime(Convert.ToInt64(ItemData.item.update_time));
                                        string[] arImages = ItemData.item.images.ToObject<string[]>();
                                        StringBuilder strImages = new StringBuilder();

                                        long discountId = 0;

                                        //이미지가 존재할 경우
                                        if (arImages.Length > 0)
                                        {
                                            for (int ii = 0; ii < arImages.Length; ii++)
                                            {
                                                strImages.Append(arImages[ii].ToString() + "^");
                                            }
                                            if (strImages.Length > 0)
                                            {
                                                strImages.Remove(strImages.Length - 1, 1);
                                            }
                                        }

                                        if (ItemData.item.variations.Count > 0)
                                        {
                                            for (int vari = 0; vari < ItemData.item.variations.Count; vari++)
                                            {
                                                DateTime variCreateTime = UnixTimeStampToDateTime(Convert.ToInt64(ItemData.item.variations[vari].create_time));
                                                DateTime variUpdateTime = UnixTimeStampToDateTime(Convert.ToInt64(ItemData.item.variations[vari].update_time));

                                                //옵션에 할인이 걸려 있으면 상품에는 할인이 안걸린다. 지랄같다
                                                discountId = ItemData.item.variations[vari].discount_id ?? 0;

                                                var ItemVariation = new ItemVariation
                                                {
                                                    variation_id = ItemData.item.variations[vari].variation_id ?? 0,
                                                    variation_sku = ItemData.item.variations[vari].variation_sku ?? "",
                                                    name = ItemData.item.variations[vari].name ?? "",
                                                    price = ItemData.item.variations[vari].price ?? 0,
                                                    stock = ItemData.item.variations[vari].stock ?? 0,
                                                    status = ItemData.item.variations[vari].status ?? "",
                                                    create_time = variCreateTime,
                                                    update_time = variUpdateTime,
                                                    //create_time = DateTime.Now,
                                                    //update_time = DateTime.Now,
                                                    original_price = ItemData.item.variations[vari].original_price ?? 0,
                                                    discount_id = ItemData.item.variations[vari].discount_id ?? 0,
                                                    item_id = ItemData.item.item_id ?? 0,
                                                    
                                                    margin = 0,
                                                    supplyPrice = 0,

                                                    //주석풀것
                                                    pgFee = 0,
                                                    currencyDate = DateTime.Now,
                                                    currencyRate = 0,
                                                    targetRetailPriceKRW = 0,
                                                    targetSellPriceKRW = 0,
                                                    UserId = global_var.userId

                                                };
                                                context.ItemVariations.Add(ItemVariation);
                                            }
                                        }

                                        if (ItemData.item.attributes.Count > 0)
                                        {
                                            for (int attr = 0; attr < ItemData.item.attributes.Count; attr++)
                                            {
                                                long TempItemId = ItemData.item.item_id;
                                                long attributeId = ItemData.item.attributes[attr].attribute_id;
                                                using (AppDbContext contextAttr = new AppDbContext())
                                                {
                                                    ItemAttribute resultAttr = contextAttr.ItemAttributes.SingleOrDefault(
                                                    b => b.item_id == TempItemId &&
                                                    b.attribute_id == attributeId &&
                                                    b.UserId == global_var.userId);

                                                    if (resultAttr == null)
                                                    {
                                                        var ItemAttribute = new ItemAttribute
                                                        {
                                                            attribute_id = ItemData.item.attributes[attr].attribute_id ?? 0,
                                                            attribute_name = ItemData.item.attributes[attr].attribute_name ?? "",
                                                            is_mandatory = ItemData.item.attributes[attr].is_mandatory ?? false,
                                                            attribute_type = ItemData.item.attributes[attr].attribute_type ?? "",
                                                            attribute_value = ItemData.item.attributes[attr].attribute_value ?? "",
                                                            item_id = ItemData.item.item_id ?? 0,
                                                            UserId = global_var.userId
                                                        };
                                                        context.ItemAttributes.Add(ItemAttribute);
                                                    }
                                                    else
                                                    {
                                                        resultAttr.attribute_name = ItemData.item.attributes[attr].attribute_name ?? "";
                                                        resultAttr.is_mandatory = ItemData.item.attributes[attr].is_mandatory ?? false;
                                                        resultAttr.attribute_type = ItemData.item.attributes[attr].attribute_type ?? "";
                                                        resultAttr.attribute_value = ItemData.item.attributes[attr].attribute_value ?? "";
                                                        resultAttr.item_id = ItemData.item.item_id ?? 0;
                                                        contextAttr.SaveChanges();
                                                    }
                                                }
                                            }
                                        }

                                        //배송비 세팅관련 데이터 
                                        //Sold Out되면 배송 데이터가 없어진다.
                                        if (ItemData.item.logistics.Count > 0)
                                        {
                                            for (int logi = 0; logi < ItemData.item.logistics.Count; logi++)
                                            {
                                                long TempItemId = ItemData.item.item_id;
                                                long logisticId = ItemData.item.logistics[logi].logistic_id ?? 0;


                                                using (AppDbContext contextLogi = new AppDbContext())
                                                {
                                                    ItemLogistic resultLogi = context.ItemLogistics.SingleOrDefault(
                                                    b => b.item_id == TempItemId &&
                                                    b.logistic_id == logisticId &&
                                                    b.UserId == global_var.userId);

                                                    if (resultLogi == null)
                                                    {   
                                                        var ItemLogistic = new ItemLogistic
                                                        {
                                                            enabled = ItemData.item.logistics[logi].enabled ?? false,
                                                            estimated_shipping_fee = ItemData.item.logistics[logi].estimated_shipping_fee ?? 0,
                                                            is_free = ItemData.item.logistics[logi].is_free ?? false,
                                                            logistic_id = ItemData.item.logistics[logi].logistic_id ?? 0,
                                                            logistic_name = ItemData.item.logistics[logi].logistic_name ?? "",
                                                            item_id = ItemData.item.item_id ?? 0,
                                                            UserId = global_var.userId

                                                            //정의는 되어 있으나 필드가 안넘어옴
                                                            //shipping_fee = ItemData.item.logistics[logi].shipping_fee,
                                                            //size_id = ItemData.item.logistics[logi].size_id,
                                                        };
                                                        context.ItemLogistics.Add(ItemLogistic);
                                                    }
                                                    else
                                                    {
                                                        resultLogi.enabled = ItemData.item.logistics[logi].enabled ?? false;
                                                        resultLogi.estimated_shipping_fee = ItemData.item.logistics[logi].estimated_shipping_fee ?? 0;
                                                        resultLogi.is_free = ItemData.item.logistics[logi].is_free ?? false;
                                                        resultLogi.logistic_id = ItemData.item.logistics[logi].logistic_id ?? 0;
                                                        resultLogi.logistic_name = ItemData.item.logistics[logi].logistic_name ?? "";
                                                        resultLogi.UserId = global_var.userId;
                                                        contextLogi.SaveChanges();
                                                    }
                                                }
                                            }
                                        }

                                        if (ItemData.item.wholesales.Count > 0)
                                        {
                                            for (int sale = 0; sale < ItemData.item.wholesales.Count; sale++)
                                            {
                                                var ItemWholesale = new ItemWholesale
                                                {
                                                    min = ItemData.item.wholesales[sale].min ?? 0,
                                                    max = ItemData.item.wholesales[sale].max ?? 0,
                                                    unit_price = ItemData.item.wholesales[sale].unit_price ?? 0,
                                                    item_id = ItemData.item.item_id ?? 0,
                                                    UserId = global_var.userId
                                                };
                                                context.ItemWholesales.Add(ItemWholesale);
                                            }
                                        }

                                        long lastDicountId = 0;

                                        if(ItemData.item.discount_id != null && ItemData.item.discount_id != 0)
                                        {
                                            lastDicountId = ItemData.item.discount_id;
                                        }                                            
                                        else if(discountId != 0)
                                        {
                                            lastDicountId = discountId;
                                        }

                                        string disCountName = "";
                                        if(dicDiscount.ContainsKey(lastDicountId))
                                        {
                                            disCountName = dicDiscount[lastDicountId];
                                        }
                                        var itemInfo = new ItemInfo
                                        {
                                            shopeeAccount = SrcShopeeAccount,
                                            item_id = ItemData.item.item_id ?? 0,
                                            shopid = ItemData.item.shopid ?? 0,
                                            item_sku = ItemData.item.item_sku ?? "",
                                            status = ItemData.item.status ?? "",
                                            name = ItemData.item.name ?? "",
                                            description = ItemData.item.description ?? "",
                                            currency = ItemData.item.currency ?? "",
                                            has_variation = ItemData.item.has_variation ?? false,
                                            price = ItemData.item.price ?? 0,
                                            stock = ItemData.item.stock ?? 0,
                                            create_time = dtCreateTime,
                                            update_time = dtUpdateTime,
                                            weight = ItemData.item.weight ?? 0,
                                            category_id = ItemData.item.category_id ?? 0,
                                            original_price = ItemData.item.original_price ?? 0,
                                            rating_star = ItemData.item.rating_star ?? 0,
                                            cmt_count = ItemData.item.cmt_count ?? 0,
                                            views = ItemData.item.views ?? 0,
                                            likes = ItemData.item.likes ?? 0,
                                            sales = ItemData.item.sales ?? 0,
                                            package_length = ItemData.item.package_length ?? 0,
                                            package_width = ItemData.item.package_width ?? 0,
                                            package_height = ItemData.item.package_height ?? 0,
                                            days_to_ship = ItemData.item.days_to_ship ?? 2,
                                            size_chart = ItemData.item.size_chart ?? "",
                                            condition = ItemData.item.condition ?? "",
                                            discount_id = lastDicountId,
                                            discount_name = disCountName,
                                            is_2tier_item = ItemData.item.is_2tier_item ?? false,
                                            images = strImages.ToString(),
                                            UserId = global_var.userId,
                                            currencyDate = DateTime.Now,
                                            currencyRate = 0,
                                            savedDate = DateTime.Now,
                                        };

                                        
                                        try
                                        {
                                            context.ItemInfoes.Add(itemInfo);
                                            context.SaveChanges();
                                        }
                                        catch (DbEntityValidationException ex)
                                        {
                                            foreach (var eve in ex.EntityValidationErrors)
                                            {
                                                MessageBox.Show(string.Format("Entity of type \"{0}\" in the state \"{1}\" has the following validation errors:",
                                                    eve.Entry.Entity.GetType().Name, eve.Entry.State));
                                                foreach (var ve in eve.ValidationErrors)
                                                {
                                                    MessageBox.Show(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                        ve.PropertyName, ve.ErrorMessage));
                                                }

                                                foreach (var ve in eve.ValidationErrors)
                                                {
                                                    MessageBox.Show(string.Format("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                                        ve.PropertyName,
                                                        eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                                        ve.ErrorMessage));
                                                }
                                            }
                                        }
                                        catch (DbUpdateException ess)
                                        {
                                            var sb = new StringBuilder();
                                            sb.AppendLine($"DbUpdateException error details - {ess?.InnerException?.InnerException?.Message}");

                                            foreach (var eve in ess.Entries)
                                            {
                                                sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                                            }

                                            MessageBox.Show(sb.ToString());
                                        }

                                        catch (Exception ex)
                                        {
                                            string aa = ex.InnerException.Message;
                                            //중복키 에러 발생함
                                        }
                                    }
                                }
                            }
                        }
                        
                        progressBar.Invoke((MethodInvoker)delegate ()
                        {
                            //Cursor.Current = Cursors.WaitCursor;
                            cur_idx++;
                            progressBar.Value = cur_idx;
                        });
                        //}
                    });
                }).ContinueWith(t =>
                {
                    Application.UseWaitCursor = false;
                    MessageBox.Show("원본 상품 정보 수신 및 동기화를 완료하였습니다.", "상품 정보 수신 및 동기화", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
            catch(Exception ex)
            {
                cur_idx++;
                progressBar.Invoke((MethodInvoker)delegate ()
                {
                    Application.UseWaitCursor = true;
                    progressBar.Value = cur_idx;
                });
            }
        }

        private object lockObject = new object();
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        private bool getSrcItemList(long i_partner_id, long i_shop_id, string api_key, int pagination_offset, long update_time_from, long update_time_to, string shopeeId)
        {
            bool rtn = false;

            if(dicExpireAccount.ContainsKey(shopeeId))
            {
                return false;
            }
            //------------------------------------상품 목록 획득 ----------------------------------                
            long long_time_stamp_tier_info = ToUnixTime(DateTime.Now.AddHours(-9));
            string ep_item_info = "https://partner.shopeemobile.com/api/v1/items/get";

            //pagination_offset = 500;
            int pagination_entries_per_page = 100;
            Dictionary<string, object> dic_item_info = new Dictionary<string, object>();
            dic_item_info.Add("pagination_offset", pagination_offset);
            dic_item_info.Add("pagination_entries_per_page", pagination_entries_per_page);
            dic_item_info.Add("update_time_from", update_time_from);
            dic_item_info.Add("update_time_to", update_time_to);
            dic_item_info.Add("partner_id", i_partner_id);
            dic_item_info.Add("shopid", i_shop_id);
            dic_item_info.Add("timestamp", long_time_stamp_tier_info);

            var request_item_info = new RestRequest("", Method.POST);
            request_item_info.Method = Method.POST;
            request_item_info.AddHeader("Accept", "application/json");
            request_item_info.AddJsonBody(new
            {
                pagination_offset = pagination_offset,
                pagination_entries_per_page = pagination_entries_per_page,
                update_time_from = update_time_from,
                update_time_to = update_time_to,
                partner_id = i_partner_id,
                shopid = i_shop_id,
                timestamp = long_time_stamp_tier_info
            });
            request_item_info.AddHeader("authorization",
                HashString(ep_item_info + "|" + JsonConvert.SerializeObject(dic_item_info),
                api_key));

            var client_item_info = new RestClient(ep_item_info);
            IRestResponse response_item_info = client_item_info.Execute(request_item_info);
            if(response_item_info.StatusCode == HttpStatusCode.OK)
            {
                dynamic result_item_info = null;
                try
                {
                    result_item_info = JsonConvert.DeserializeObject(response_item_info.Content);
                    
                    if (result_item_info.msg == null)
                    {
                        if (result_item_info.items != null)
                        {
                            int cnt = result_item_info.items.Count;
                            for (int i = 0; i < result_item_info.items.Count; i++)
                            {

                                lstSrcItemId.Add(Convert.ToInt64(result_item_info.items[i].item_id));
                                dicSrcItemList.Add(Convert.ToInt64(result_item_info.items[i].item_id), Convert.ToInt64(result_item_info.items[i].item_id));
                            }

                            if ((bool)result_item_info.more)
                            {
                                rtn = true;
                                //getItemList(ipagination_offset = ipagination_offset + 100, update_time_from, update_time_to);
                            }
                        }
                    }
                    else
                    {
                        if (result_item_info.msg.ToString().Contains("partner and shop has no linked"))
                        {
                            dicExpireAccount.Add(shopeeId, i_shop_id);
                            MessageBox.Show(shopeeId + " 계정의 API인증 기간이 만료 되었습니다. 재인증이 필요합니다.", "계정 인증 기간 만료", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var kk = "";
                }
            }
            else
            {
                var kk = "";
            }
            

            return rtn;
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
        public static string ByteToString(byte[] buff)
        {
            string sbinary = BitConverter.ToString(buff).Replace("-", "");
            return (sbinary);
        }

        private void dgSrcItemList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                string siteUrl = "";
                string currency = dgItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString();
                string shop_id = dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString();
                string goods_no = dgItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString();

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
            else if(e.ColumnIndex == 22)
            {
                btn_set_category_Click(null, null);
            }
            else if (e.ColumnIndex == 9)
            {
                btn_set_variation_Click(null, null);
            }
        }

        private void dg_site_id_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //다른창에서 환율을 바꾸었을 수 있으므로 DB에서 가지고 온다.
            Cursor.Current = Cursors.WaitCursor;
            dgItemList.Rows.Clear();
            Application.DoEvents();
            string countryCode = "";
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
                    txt_src_currency_rate.Text = currency_info.Value.ToString();

                    //DB에서 가지고 온다.
                    using(AppDbContext context = new AppDbContext())
                    {
                        var rate = context.CurrencyRates.FirstOrDefault(x => x.cr_name.Contains(countryCode) && x.UserId == global_var.userId);

                        if(rate != null)
                        {
                            txt_src_currency_rate.Value = rate.cr_exchange;
                        }
                    }
                    break;
                }
            }


            getPromotionList();

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
        private void MainMenu_description_Click(object sender, EventArgs e)
        {
            if(dgItemList.Rows.Count > 0 && dgItemList.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    long ItemId = Convert.ToInt64(dgItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString());
                    ItemInfo result = context.ItemInfoes.SingleOrDefault(
                    b => b.item_id == ItemId && b.UserId == global_var.userId);

                    string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                    string countryCode = "";
                    int maxLen = 0;
                    if (shopeeId.ToUpper().Contains(".ID"))
                    {
                        countryCode = "ID";
                        maxLen = 3000;
                    }
                    else if (shopeeId.ToUpper().Contains(".MY"))
                    {
                        countryCode = "MY";
                        maxLen = 3000;
                    }
                    else if (shopeeId.ToUpper().Contains(".SG"))
                    {
                        countryCode = "SG";
                        maxLen = 3000;
                    }
                    else if (shopeeId.ToUpper().Contains(".TW"))
                    {
                        countryCode = "TW";
                        maxLen = 3000;
                    }
                    else if (shopeeId.ToUpper().Contains(".TH"))
                    {
                        countryCode = "TH";
                        maxLen = 5000;
                    }
                    else if (shopeeId.ToUpper().Contains(".PH"))
                    {
                        countryCode = "PH";
                        maxLen = 3000;
                    }
                    else if (shopeeId.ToUpper().Contains(".VN"))
                    {
                        countryCode = "VN";
                        maxLen = 3000;
                    }


                    if (result != null)
                    {
                        isChanged = false;
                        FormDescription fd = new FormDescription();
                        fd.fp = this;
                        fd.ItemId = ItemId;
                        fd.maxLen = maxLen;
                        fd.lblMaxLen.Text = string.Format("{0:n0}", maxLen) + "자";
                        fd.TxtProductDesc.Text = result.description.ToString();
                        fd.ShowDialog();

                        if (isChanged)
                        {
                            dgItemList.SelectedRows[0].Cells["dgItemList_item_id"].Style.BackColor = Color.Orange;
                        }
                    }
                }   
            }
            else
            {
                MessageBox.Show("선택된 상품이 없습니다. 목록에서 상품을 선택해 주세요.","상품선택", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void BtnGetProductList_Click(object sender, EventArgs e)
        {
            getProductList("", "");
        }

        private void BtnUpdateCurrencyRate_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("네이버 매매기준율 기준으로 환율을 업데이트 하시겠습니까?", "환율 업데이트", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
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

                MessageBox.Show("환율 정보를 업데이트 하였습니다.", "환율정보 업데이트", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSearchProduct_Click(object sender, EventArgs e)
        {
            if(CboSearchType.Text == "상품ID")
            {
                long ItemId = 0;
                if(TxtSearchProduct.Text.Trim() != string.Empty)
                {
                    if(long.TryParse(TxtSearchProduct.Text.Trim(), out ItemId))
                    {
                        getProductList("상품ID", TxtSearchProduct.Text.Trim());
                    }
                    else
                    {
                        MessageBox.Show("숫자만 입력해 주세요.","입력오류",MessageBoxButtons.OK,MessageBoxIcon.Error);
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
            else if(CboSearchType.Text == "상품SKU")
            {
                getProductList("상품SKU", TxtSearchProduct.Text.Trim());
            }
            else if (CboSearchType.Text == "상품명")
            {
                getProductList("상품명", TxtSearchProduct.Text.Trim());
            }


            TxtSearchProduct.Select();
            TxtSearchProduct.SelectAll();
        }

        private void TxtSearchProduct_Click(object sender, EventArgs e)
        {

            
        }

        private void BtnSaveSupplyPrice_Click(object sender, EventArgs e)
        {
            if(dgItemList.Rows.Count > 0)
            {
                DialogResult dlg_Result = MessageBox.Show("체크한 상품의 공급가를 저장 하시겠습니까?", "공급가 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg_Result == DialogResult.Yes)
                {
                    for (int i = 0; i < dgItemList.Rows.Count; i++)
                    {
                        if (dgItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                        {
                            long itemId = Convert.ToInt64(dgItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                            string strSupplyPrice = dgItemList.Rows[i].Cells["dgItemList_supply_price"].Value.ToString().Trim();
                            long supplyPrice = 0;

                            if (Int64.TryParse(strSupplyPrice, out supplyPrice))
                            {
                                using (AppDbContext context = new AppDbContext())
                                {
                                    ItemInfo result = context.ItemInfoes.SingleOrDefault(
                                            b => b.item_id == itemId && b.UserId == global_var.userId);

                                    if (result != null)
                                    {
                                        result.supply_price = supplyPrice;
                                        context.SaveChanges();

                                        dgItemList.Rows[i].Cells["dgItemList_item_id"].Style.BackColor = Color.Orange;
                                    }
                                }
                            }
                        }
                    }

                    MessageBox.Show("체크한 상품의 모든 공급가를 저장하였습니다.", "공급가 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            
        }

        private void dgSrcItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 1)
            {
                if (dgItemList.Rows.Count > 0)
                {
                    bool chk = (bool)dgItemList.Rows[0].Cells["dgItemList_Chk"].Value;
                    for (int i = 0; i < dgItemList.Rows.Count; i++)
                    {
                        dgItemList.Rows[i].Cells["dgItemList_Chk"].Value = !chk;
                    }

                    groupBox3.Select();
                }
            }
            else if(e.RowIndex > -1)
            {
                if(e.ColumnIndex== 1)
                {
                    bool chk = (bool)dgItemList.SelectedRows[0].Cells["dgItemList_Chk"].Value;
                    dgItemList.SelectedRows[0].Cells["dgItemList_Chk"].Value = !chk;
                    groupBox3.Select();
                }
                else if(e.ColumnIndex == 13 || e.ColumnIndex == 15 || e.ColumnIndex == 16)
                {
                    if(dgItemList.SelectedRows[0].Cells[e.ColumnIndex].Value != null && dgItemList.SelectedRows[0].Cells[e.ColumnIndex].Value.ToString() != string.Empty)
                    {
                        decimal rateSrc = 0;
                        decimal SrcPrice = 0;
                        SrcPrice = Convert.ToDecimal(dgItemList.SelectedRows[0].Cells[e.ColumnIndex].Value.ToString().Replace(",", ""));
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

            if(e.RowIndex > -1)
            {
                string colName = dgItemList.Columns[e.ColumnIndex].Name.ToString();
                if (dgItemList.SelectedRows[0].Cells[e.ColumnIndex].Value != null &&
                    colName != "dgItemList_name")
                {
                    string val = dgItemList.SelectedRows[0].Cells[e.ColumnIndex].Value.ToString().Trim();
                    if (val != string.Empty)
                    {
                        Clipboard.SetText(val);
                    }
                }                
            }
        }

        private void TxtSearchProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                BtnSearchProduct_Click(null, null);
                TxtSearchProduct.Select();
                TxtSearchProduct.SelectAll();
            }
        }
        public string TempCategoryId = "";
        public bool isChanged = false;
        private void btn_set_category_Click(object sender, EventArgs e)
        {
            if (dgItemList.Rows.Count > 0 && dgItemList.SelectedRows.Count > 0)
            {
                TempCategoryId = "";
                isChanged = false;
                FormSetCategoryAndAttribute fcs = new FormSetCategoryAndAttribute();
                fcs.fp = this;
                fcs.partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                fcs.shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                fcs.api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
                fcs.TxtItemId.Text = dgItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString();
                fcs.itemId = Convert.ToInt64(dgItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString());
                fcs.TxtSrcCountry.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                fcs.country = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                fcs.categoryId = Convert.ToInt64(dgItemList.SelectedRows[0].Cells["dgItemList_category_id"].Value.ToString());
                fcs.txtSrcTitle.Text = dgItemList.SelectedRows[0].Cells["dgItemList_name"].Value.ToString();
                fcs.ShowDialog();

                if (TempCategoryId != string.Empty)
                {
                    dgItemList.SelectedRows[0].Cells["dgItemList_category_id"].Value = TempCategoryId;
                    dgItemList.SelectedRows[0].Cells["dgItemList_category_id"].Style.BackColor = Color.GreenYellow;
                    dgItemList.EndEdit();
                    groupBox3.Select();
                }

                if(isChanged)
                {
                    dgItemList.SelectedRows[0].Cells["dgItemList_item_id"].Style.BackColor = Color.Orange;
                }
            }
            else
            {
                MessageBox.Show("선택된 상품이 없습니다. 목록에서 상품을 선택해 주세요.", "상품선택", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void MainMenu_Save_Supply_Price_checked_Click(object sender, EventArgs e)
        {
            BtnSaveSupplyPrice_Click(null, null);
        }

        private void MainMenu_Set_Category_Click(object sender, EventArgs e)
        {
            btn_set_category_Click(null, null);
        }

        private void btn_set_attribute_Click(object sender, EventArgs e)
        {
            if (dgItemList.SelectedRows.Count > 0)
            {
                string src_partner_id = "";
                string src_shop_id = "";
                string src_api_key = "";

                string end_point = "https://partner.shopeemobile.com/api/v1/item/add";

                src_partner_id = dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString();
                src_shop_id = dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString();
                src_api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();

                string srcCategory = dgItemList.SelectedRows[0].Cells["dgItemList_category_id"].Value.ToString();
                string srcProductId = dgItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString();

                if (src_partner_id != string.Empty
                    && src_shop_id != string.Empty
                    && src_api_key != string.Empty
                    && srcCategory != string.Empty)
                {
                    FormSetAttribute fcs = new FormSetAttribute();
                    fcs.TxtSrcPartnerId.Text = src_partner_id;
                    fcs.TxtSrcShopId.Text = src_shop_id;
                    fcs.TxtSrcSecretKey.Text = src_api_key;
                    fcs.TxtSrcCategoryId.Text = srcCategory;
                    fcs.TxtSrcProductId.Text = srcProductId;                    
                    fcs.TxtSrcCountry.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                    fcs.Pic.Image = (Image)dgItemList.SelectedRows[0].Cells["dgItemList_Image"].Value;
                    fcs.TxtProductTitle.Text = dgItemList.SelectedRows[0].Cells["dgItemList_name"].Value.ToString();
                    fcs.ShowDialog();
                }
                else
                {
                    MessageBox.Show("원본 카테고리가 설정되지 않았습니다.", "원본카테고리 설정", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dg_site_id_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            BtnGetProductList_Click(null, null);
        }

        private void MainMenu_View_Product_Page_Click(object sender, EventArgs e)
        {
            if (dgItemList.Rows.Count > 0 && dgItemList.SelectedRows.Count > 0)
            {
                string siteUrl = "";
                string currency = dgItemList.SelectedRows[0].Cells["dgItemList_currency"].Value.ToString();
                string shop_id = dgItemList.SelectedRows[0].Cells["dgItemList_shopid"].Value.ToString();
                string goods_no = dgItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString();

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

        private void BtnViewDescription_Click(object sender, EventArgs e)
        {
            MainMenu_description_Click(null, null);
        }

        private void viewNaverExchange_Click(object sender, EventArgs e)
        {
            string siteUrl = "https://finance.naver.com/marketindex/?tabSel=exchange#tab_section";
            System.Diagnostics.Process.Start("chrome.exe", siteUrl);
        }

        private void MainMenu_check_all_Click(object sender, EventArgs e)
        {
            if (dgItemList.Rows.Count > 0)
            {   
                for (int i = 0; i < dgItemList.Rows.Count; i++)
                {
                    dgItemList.Rows[i].Cells["dgItemList_Chk"].Value = true;
                }

                groupBox3.Select();
            }
        }

        private void MainMenu_uncheck_all_Click(object sender, EventArgs e)
        {
            if (dgItemList.Rows.Count > 0)
            {
                for (int i = 0; i < dgItemList.Rows.Count; i++)
                {
                    dgItemList.Rows[i].Cells["dgItemList_Chk"].Value = false;
                }

                groupBox3.Select();
            }
        }

        private void PromotionMenu_getList_Click(object sender, EventArgs e)
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
                .Where(x => x.shopeeAccount == shopeeId && x.UserId == global_var.userId)
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
                    .Where(x => x.shopeeAccount == shopeeId && x.UserId == global_var.userId)
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
        }

        private void dgSrcItemList_SelectionChanged(object sender, EventArgs e)
        {
            dgItemList.CurrentRow.Cells["dgItemList_item_id"].Style.SelectionBackColor = dgItemList.CurrentRow.Cells["dgItemList_item_id"].Style.BackColor;
            dgItemList.CurrentRow.Cells["dgItemList_item_id"].Style.SelectionForeColor = Color.GreenYellow;            
        }

        private void btn_set_variation_Click(object sender, EventArgs e)
        {
            if (dgItemList.Rows.Count > 0 && dgItemList.SelectedRows.Count > 0)
            {
                if(dg_site_id.Rows.Count > 0 && dg_site_id.SelectedRows.Count > 0)
                {
                    long src_shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                    long productId = Convert.ToInt64(dgItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString().Trim());

                    TempCategoryId = "";
                    isChanged = false;

                    FormSetVariation fs = new FormSetVariation();
                    //fs.fp = this;
                    fs.ItemId = productId;
                    fs.partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                    fs.shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                    fs.api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
                    fs.country = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                    fs.udRetailPrice.Value = Convert.ToDecimal(dgItemList.SelectedRows[0].Cells["dgItemList_original_price"].Value.ToString());
                    fs.udSellPrice.Value = Convert.ToDecimal(dgItemList.SelectedRows[0].Cells["dgItemList_price"].Value.ToString());
                    fs.udSrcWeight.Value = Convert.ToDecimal(dgItemList.SelectedRows[0].Cells["dgItemList_weight"].Value.ToString());
                    fs.UdSourcePrice.Value = Convert.ToInt32(dgItemList.SelectedRows[0].Cells["dgItemList_supply_price"].Value.ToString().Replace(",",""));
                    fs.UdMargin.Value = Convert.ToInt32(dgItemList.SelectedRows[0].Cells["dgItemList_margin"].Value.ToString().Replace(",", ""));
                    fs.UdQty.Value = Convert.ToInt32(dgItemList.SelectedRows[0].Cells["dgItemList_stock"].Value.ToString().Replace(",", ""));
                    fs.ShowDialog();

                    if (isChanged)
                    {
                        setDefaultVar();
                        dgItemList.SelectedRows[0].Cells["dgItemList_item_id"].Style.BackColor = Color.Orange;
                    }
                }
            }
            else
            {
                MessageBox.Show("선택된 상품이 없습니다. 목록에서 상품을 선택해 주세요.", "상품선택", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void MainMenu_Option_Click(object sender, EventArgs e)
        {
            btn_set_variation_Click(null, null);
        }
        

        private void MainMenu_Delete_Click(object sender, EventArgs e)
        {
            if (dgItemList.Rows.Count == 0)
            {
                return;
            }
            DialogResult dlg_Result = MessageBox.Show("체크한 상품을 삭제 하시겠습니까?", "상품 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                groupBox3.Select();
                dgItemList.EndEdit();
                for (int i = 0; i < dgItemList.Rows.Count; i++)
                {
                    if (dgItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        long srcProductId = Convert.ToInt64(dgItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                        string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemInfo result_src_data = context.ItemInfoes.SingleOrDefault(
                                b => b.item_id == srcProductId &&
                                b.shopeeAccount == shopeeId && b.UserId == global_var.userId);

                            //원본데이터가 DB에 존재하여야 한다.
                            if (result_src_data != null)
                            {
                                context.ItemInfoes.Remove(result_src_data);
                                context.SaveChanges();
                            }

                            //맵핑 정보도 삭제하여 준다.
                            //참조할 놈이 없어졌으므로 양쪽다 지워준다.
                            List<ProductLink> linkList = context.ProductLinks
                                .Where(b => b.SourceProductId == srcProductId ||
                                b.TargetProductId == srcProductId 
                                && b.UserId == global_var.userId).ToList();

                            if(linkList.Count > 0)
                            {
                                context.ProductLinks.RemoveRange(linkList);
                                context.SaveChanges();
                            }
                        }
                    }
                }
                BtnGetProductList_Click(null, null);
                Cursor.Current = Cursors.Default;
                MessageBox.Show("상품을 모두 삭제하였습니다.", "상품 삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dt_start_ValueChanged(object sender, EventArgs e)
        {
            //dt_end.Value = dt_start.Value.AddDays(14);
        }

        private void dt_end_ValueChanged(object sender, EventArgs e)
        {
            //TimeSpan TS = dt_end.Value - dt_start.Value;
            //if (TS.Days > 14)
            //{
            //    MessageBox.Show("최대 14일까지 가능합니다.","날짜 설정",MessageBoxButtons.OK,MessageBoxIcon.Error);
            //    dt_end.Value = dt_start.Value.AddDays(14);
            //}
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void BtnGetAccount_Click(object sender, EventArgs e)
        {
            getShopeeAccount();
        }

        private void DgSrcItemList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
            {
            if (dg_site_id.Rows.Count > 0)
            {
                long ItemInfoItemId = Convert.ToInt64(dgItemList.Rows[e.RowIndex].Cells["dgItemList_item_id"].Value.ToString());
                //상품명이 변경되면 저장해 준다.
                if (e.ColumnIndex == 7 && e.RowIndex > -1)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemInfo result_src_data = context.ItemInfoes.SingleOrDefault(
                            b => b.item_id == ItemInfoItemId && b.UserId == global_var.userId);

                        //원본데이터가 DB에 존재하여야 한다.
                        if (result_src_data != null)
                        {
                            result_src_data.name = dgItemList.Rows[e.RowIndex].Cells["dgItemList_name"].Value.ToString().Trim();
                            result_src_data.isChanged = true;
                            context.SaveChanges();
                            dgItemList.Rows[e.RowIndex].Cells["dgItemList_name"].Style.BackColor = Color.SkyBlue;
                            dgItemList.Rows[e.RowIndex].Cells["dgItemList_item_id"].Style.BackColor = Color.SkyBlue;
                        }
                    }
                }
                else if (e.ColumnIndex == 5 && e.RowIndex > -1)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemInfo result_src_data = context.ItemInfoes.SingleOrDefault(
                            b => b.item_id == ItemInfoItemId && b.UserId == global_var.userId);

                        //원본데이터가 DB에 존재하여야 한다.
                        if (result_src_data != null)
                        {
                            string parentSku = "";
                            if(dgItemList.Rows[e.RowIndex].Cells["dgItemList_item_sku"].Value != null)
                            {
                                parentSku = dgItemList.Rows[e.RowIndex].Cells["dgItemList_item_sku"].Value?.ToString().Trim();
                            }
                            else
                            {
                                parentSku = "";
                            }
                            result_src_data.item_sku = parentSku;
                            result_src_data.isChanged = true;
                            context.SaveChanges();
                            dgItemList.Rows[e.RowIndex].Cells["dgItemList_item_sku"].Style.BackColor = Color.SkyBlue;
                            dgItemList.Rows[e.RowIndex].Cells["dgItemList_item_id"].Style.BackColor = Color.SkyBlue;
                        }
                    }
                }
                else if (e.ColumnIndex == 10 && e.RowIndex > -1)
                {
                    if (!isChanging)
                    {
                        calcRowPrice(e.RowIndex, dgItemList.Columns[e.ColumnIndex].Name);
                    }

                    //공급가
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemInfo result_src_data = context.ItemInfoes.SingleOrDefault(
                            b => b.item_id == ItemInfoItemId && b.UserId == global_var.userId);

                        //원본데이터가 DB에 존재하여야 한다.
                        if (result_src_data != null)
                        {
                            result_src_data.supply_price = Convert.ToDecimal(dgItemList.Rows[e.RowIndex].Cells["dgItemList_supply_price"].Value.ToString().Trim().Replace(",",""));
                            result_src_data.isChanged = true;
                            context.SaveChanges();
                            dgItemList.Rows[e.RowIndex].Cells["dgItemList_supply_price"].Style.BackColor = Color.SkyBlue;
                            dgItemList.Rows[e.RowIndex].Cells["dgItemList_item_id"].Style.BackColor = Color.SkyBlue;
                        }
                    }
                }
                else if (e.ColumnIndex == 11 && e.RowIndex > -1)
                {
                    //마진

                    if (!isChanging)
                    {
                        calcRowPrice(e.RowIndex, dgItemList.Columns[e.ColumnIndex].Name);
                    }

                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemInfo result_src_data = context.ItemInfoes.SingleOrDefault(
                            b => b.item_id == ItemInfoItemId && b.UserId == global_var.userId);

                        //원본데이터가 DB에 존재하여야 한다.
                        if (result_src_data != null)
                        {
                            result_src_data.margin = Convert.ToDecimal(dgItemList.Rows[e.RowIndex].Cells["dgItemList_margin"].Value.ToString().Trim().Replace(",", ""));
                            result_src_data.isChanged = true;
                            context.SaveChanges();
                            dgItemList.Rows[e.RowIndex].Cells["dgItemList_margin"].Style.BackColor = Color.SkyBlue;
                            dgItemList.Rows[e.RowIndex].Cells["dgItemList_item_id"].Style.BackColor = Color.SkyBlue;
                        }
                    }
                }
                else if (e.RowIndex > -1 && e.ColumnIndex == 15)
                {
                    //판매가격 변경
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemInfo result = context.ItemInfoes.SingleOrDefault(
                                b => b.item_id == ItemInfoItemId && b.UserId == global_var.userId);

                        decimal price = Convert.ToDecimal(dgItemList.Rows[e.RowIndex].Cells["dgItemList_price"].Value.ToString().Trim().Replace(",", ""));
                        if (result != null)
                        {
                            result.price = price;
                            result.isChanged = true;
                            context.SaveChanges();

                            dgItemList.Rows[e.RowIndex].Cells["dgItemList_item_id"].Style.BackColor = Color.SkyBlue;
                            dgItemList.Rows[e.RowIndex].Cells["dgItemList_price"].Style.BackColor = Color.SkyBlue;
                        }
                    }
                }
                else if (e.RowIndex > -1 && e.ColumnIndex == 16)
                {
                    //소비자가격 변경
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemInfo result = context.ItemInfoes.SingleOrDefault(
                                b => b.item_id == ItemInfoItemId && b.UserId == global_var.userId);

                        decimal retailPrice = Convert.ToDecimal(dgItemList.Rows[e.RowIndex].Cells["dgItemList_original_price"].Value.ToString().Trim().Replace(",", ""));
                        if (result != null)
                        {
                            result.original_price = retailPrice;
                            result.isChanged = true;
                            context.SaveChanges();

                            dgItemList.Rows[e.RowIndex].Cells["dgItemList_item_id"].Style.BackColor = Color.SkyBlue;
                            dgItemList.Rows[e.RowIndex].Cells["dgItemList_original_price"].Style.BackColor = Color.SkyBlue;
                            dgItemList.Rows[e.RowIndex].Cells["dgItemList_original_price"].Value = string.Format("{0:n2}", retailPrice);
                        }
                    }
                }
                else if (e.RowIndex > -1 && e.ColumnIndex == 17)
                {
                    //재고수량 변경
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemInfo result = context.ItemInfoes.SingleOrDefault(
                                b => b.item_id == ItemInfoItemId && b.UserId == global_var.userId);

                        int stock = Convert.ToInt32(dgItemList.Rows[e.RowIndex].Cells["dgItemList_stock"].Value.ToString().Trim().Replace(",", ""));
                        if (result != null)
                        {
                            result.stock = stock;
                            result.isChanged = true;
                            context.SaveChanges();

                            if (stock > 0)
                            {
                                dgItemList.Rows[e.RowIndex].Cells["dgItemList_stock"].Style.BackColor = Color.GreenYellow;
                                dgItemList.Rows[e.RowIndex].Cells["dgItemList_item_id"].Style.BackColor = Color.SkyBlue;
                            }
                            else
                            {
                                dgItemList.Rows[e.RowIndex].Cells["dgItemList_stock"].Style.BackColor = Color.Orange;
                            }
                        }
                    }
                }
                else if (e.RowIndex > -1 && e.ColumnIndex == 21)
                {
                    //무게 변경
                    if (!isChanging)
                    {
                        double weight = Convert.ToDouble(dgItemList.Rows[e.RowIndex].Cells["dgItemList_weight"].Value.ToString().Trim().Replace(",", ""));
                        calcRowPrice(e.RowIndex, dgItemList.Columns[e.ColumnIndex].Name);

                        using (AppDbContext context = new AppDbContext())
                        {
                            ItemInfo result = context.ItemInfoes.SingleOrDefault(
                                    b => b.item_id == ItemInfoItemId && b.UserId == global_var.userId);


                            if (weight > 0)
                            {
                                if (result != null)
                                {
                                    result.weight = weight;
                                    result.isChanged = true;
                                    context.SaveChanges();

                                    if (weight > 0)
                                    {
                                        dgItemList.Rows[e.RowIndex].Cells["dgItemList_weight"].Style.BackColor = Color.GreenYellow;
                                        dgItemList.Rows[e.RowIndex].Cells["dgItemList_item_id"].Style.BackColor = Color.SkyBlue;
                                    }
                                    else
                                    {
                                        dgItemList.Rows[e.RowIndex].Cells["dgItemList_weight"].Style.BackColor = Color.Orange;
                                    }
                                }
                            }
                        }
                    }
                    
                    
                }
                else if (e.RowIndex > -1 && e.ColumnIndex == 31)
                {
                    //DTS수정
                    //재고수량 변경
                    //DTS는 가끔 정책에 따라 변경된다
                    //2019-10-26현재 국가별로 다른점은 없으며, 기본 2일 설정시 5-10일
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemInfo result = context.ItemInfoes.SingleOrDefault(
                                b => b.item_id == ItemInfoItemId && b.UserId == global_var.userId);

                        int dts = 0;
                        if (int.TryParse(dgItemList.Rows[e.RowIndex].Cells["dgItemList_days_to_ship"].Value.ToString().Trim().Replace(",", ""), out dts))
                        {
                            if (result != null)
                            {
                                if (dts == 2 || (dts >= 5 && dts <= 10))
                                {
                                    result.days_to_ship = dts;
                                    result.isChanged = true;
                                    context.SaveChanges();

                                    if (dts > 0)
                                    {
                                        dgItemList.Rows[e.RowIndex].Cells["dgItemList_days_to_ship"].Style.BackColor = Color.GreenYellow;
                                        dgItemList.Rows[e.RowIndex].Cells["dgItemList_item_id"].Style.BackColor = Color.SkyBlue;
                                    }
                                    else
                                    {
                                        dgItemList.Rows[e.RowIndex].Cells["dgItemList_days_to_ship"].Style.BackColor = Color.Orange;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Pre-Oder 기간 설정은 기본 2일이며 설정시는 5일~10일까지 입니다.", "Pre-Order 설정 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }   
            }
        }

        private void PromotionMenu_SetDiscount_Click(object sender, EventArgs e)
        {
            groupBox3.Select();
            if (dg_shopee_discount.Rows.Count > 0 && dg_shopee_discount.SelectedRows.Count > 0)
            {
                long discountId = Convert.ToInt64(dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_id"].Value.ToString());
                string discountName = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_name"].Value.ToString();

                using(AppDbContext context =  new AppDbContext())
                {
                    for (int i = 0; i < dgItemList.Rows.Count; i++)
                    {
                        if ((bool)dgItemList.Rows[i].Cells["dgItemList_Chk"].Value)
                        {
                            //전제조건은 옵션이 있는 경우는 상품의 메인가격은 무시한다.
                            //옵션이 없는 경우는 판가는 무조건 할인가보다 작아야 한다.
                            //할인이벤트를 걸려면 판가가 소비자가보다 무조건 작아야 한다.
                            bool validate = true;
                            long itemId = Convert.ToInt64(dgItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());

                            var varList = context.ItemVariations.Where(x => x.item_id == itemId 
                            && x.UserId == global_var.userId).ToList();

                            if(varList.Count > 0)
                            {
                                for (int j = 0; j < varList.Count; j++)
                                {
                                    if (varList[j].original_price <= varList[j].price)
                                    {
                                        dgItemList.Rows[i].Cells["dgItemList_discount_name"].Value = "옵션의 판매가가 소비자가보다 작아야 함";
                                        dgItemList.Rows[i].Cells["dgItemList_discount_name"].Style.BackColor = Color.Orange;
                                        validate = false;                                        
                                    }
                                }
                            }
                            else
                            {
                                decimal itemPrice = Convert.ToDecimal(dgItemList.Rows[i].Cells["dgItemList_price"].Value.ToString());
                                decimal itemRetailPrice = Convert.ToDecimal(dgItemList.Rows[i].Cells["dgItemList_original_price"].Value.ToString());

                                if (itemRetailPrice <= itemPrice)
                                {
                                    dgItemList.Rows[i].Cells["dgItemList_discount_name"].Value = "상품의 판매가격이 소비자가격보다 작아야 함";
                                    dgItemList.Rows[i].Cells["dgItemList_discount_name"].Style.BackColor = Color.Orange;
                                    validate = false;
                                }
                            }

                            if(validate)
                            {
                                var result = context.ItemInfoes.FirstOrDefault(x => x.item_id == itemId && x.UserId == global_var.userId);

                                if (result != null)
                                {
                                    dgItemList.Rows[i].Cells["dgItemList_discount_id"].Value = discountId;
                                    dgItemList.Rows[i].Cells["dgItemList_discount_name"].Value = discountName;

                                    result.discount_id = discountId;
                                    result.discount_name = discountName;
                                    context.SaveChanges();
                                }
                            }
                            
                        }
                    }
                }
            }
        }

        private void PromotionMenu_UnCheckDiscount_Click(object sender, EventArgs e)
        {
            if (dg_shopee_discount.Rows.Count > 0 && dg_shopee_discount.SelectedRows.Count > 0)
            {
                groupBox3.Select();
                long discountId = 0;
                string discountName = "";

                using (AppDbContext context = new AppDbContext())
                {
                    for (int i = 0; i < dgItemList.Rows.Count; i++)
                    {
                        if ((bool)dgItemList.Rows[i].Cells["dgItemList_Chk"].Value)
                        {
                            long itemId = Convert.ToInt64(dgItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                            var result = context.ItemInfoes.FirstOrDefault(x => x.item_id == itemId && x.UserId == global_var.userId);

                            if (result != null)
                            {
                                dgItemList.Rows[i].Cells["dgItemList_discount_id"].Value = discountId;
                                dgItemList.Rows[i].Cells["dgItemList_discount_name"].Value = discountName;

                                result.discount_id = discountId;
                                result.discount_name = discountName;
                                context.SaveChanges();

                                dgItemList.Rows[i].Cells["dgItemList_discount_name"].Style.BackColor = Color.White;
                            }

                            var resultVariation = context.ItemVariations.Where(x => x.item_id == itemId && x.UserId == global_var.userId).ToList();
                            if(resultVariation.Count > 0)
                            {
                                for (int j = 0; j < resultVariation.Count; j++)
                                {
                                    resultVariation[j].discount_id = 0;
                                    context.Entry(resultVariation[j]).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BtnUpdateProduct_Click(object sender, EventArgs e)
        {
            if(dg_site_id.Rows.Count == 0 || dg_site_id.SelectedRows.Count == 0 || dgItemList.Rows.Count == 0 || dgItemList.SelectedRows.Count == 0)
            {
                return;
            }

            DialogResult dlg_Result = MessageBox.Show("체크한 상품의 정보를 수정 하시겠습니까?", "상품 데이터 수정", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                groupBox3.Select();
                Cursor.Current = Cursors.WaitCursor;
                
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                long partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                long shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                string secertKey = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();

                ShopeeApi shopeeApi = new ShopeeApi();
                for (int i = 0; i < dgItemList.Rows.Count; i++)
                {
                    dgItemList.Rows[i].Cells["dgItemList_Result"].Value = "";
                    dgItemList.Rows[i].Cells["dgItemList_Result"].Style.BackColor = Color.White;
                    if ((bool)dgItemList.Rows[i].Cells["dgItemList_Chk"].Value)
                    {
                        long itemId = Convert.ToInt64(dgItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString().Trim());
                        long discount_id = 0;
                        decimal discountItemPrice = Convert.ToDecimal(dgItemList.Rows[i].Cells["dgItemList_price"].Value.ToString().Trim());
                        if (dgItemList.Rows[i].Cells["dgItemList_discount_id"].Value != null && 
                            dgItemList.Rows[i].Cells["dgItemList_discount_id"].Value.ToString().Trim() != string.Empty &&
                            dgItemList.Rows[i].Cells["dgItemList_discount_name"].Value != null &&
                            dgItemList.Rows[i].Cells["dgItemList_discount_name"].Value.ToString().Trim() != string.Empty)
                        {
                            discount_id = Convert.ToInt64(dgItemList.Rows[i].Cells["dgItemList_discount_id"].Value.ToString().Trim());
                        }
                             
                        using (AppDbContext context = new AppDbContext())
                        {
                            var itemData = context.ItemInfoes.FirstOrDefault(x => x.item_id == itemId && x.UserId == global_var.userId);
                            if(itemData != null)
                            {
                                var itemVariation = context.ItemVariations.Where(x => x.item_id == itemId && x.UserId == global_var.userId).ToList();
                                var itemAttribute = context.ItemAttributes.Where(x => x.item_id == itemId && x.UserId == global_var.userId).ToList();

                                bool validateData = true;
                                DateTime saveDate = DateTime.Now;
                                long time_stamp = ToUnixTime(saveDate.AddHours(-9));

                                if (itemData.name.Trim() == string.Empty)
                                {
                                    validateData = false;
                                }

                                if (validateData)
                                {
                                    //카테고리에 따른 속성목록 작성
                                    List<shopee_attribute> ListAttribute = new List<shopee_attribute>();
                                    if(itemAttribute.Count > 0)
                                    {
                                        for (int attr = 0; attr < itemAttribute.Count; attr++)
                                        {
                                            shopee_attribute att = new shopee_attribute
                                            {
                                                attributes_id = itemAttribute[attr].attribute_id,
                                                value = itemAttribute[attr].attribute_value
                                            };

                                            ListAttribute.Add(att);
                                        }
                                    }

                                    List<shopee_variations> ListVariation = new List<shopee_variations>();
                                    List<Item2TierVariation> List2TierVariation = new List<Item2TierVariation>();

                                    Item2TierVariationInit item2TierInit = new Item2TierVariationInit();
                                    List<tier_variation> lstTier_variation = new List<tier_variation>();
                                    List<variation> lst2TierVariation = new List<variation>();
                                    List<ItemDiscountPrice> lstDiscountPrice = new List<ItemDiscountPrice>();

                                    List<string> lstOptions = new List<string>();
                                    List<string> lstImages = new List<string>();

                                    if (itemVariation.Count > 0)
                                    {
                                        for (int vari = 0; vari < itemVariation.Count; vari++)
                                        {
                                            shopee_variations itemVari = new shopee_variations
                                            {   
                                                name = itemVariation[vari].name,
                                                stock = itemVariation[vari].stock,
                                                variation_sku = itemVariation[vari].variation_sku,
                                                price = itemVariation[vari].original_price,
                                                UserId = global_var.userId
                                            };
                                            ListVariation.Add(itemVari);

                                            lstOptions.Add(itemVariation[vari].name);
                                            lstImages.Add("");

                                            variation vv = new variation
                                            {
                                                tier_index = new int[] { vari },
                                                stock = itemVariation[vari].stock,
                                                price = itemVariation[vari].original_price,                                                
                                                variation_sku = itemVariation[vari].variation_sku
                                            };
                                            lst2TierVariation.Add(vv);

                                            ItemDiscountPrice discPrice = new ItemDiscountPrice
                                            {
                                                discountPrice = itemVariation[vari].price,
                                                variationName = itemVariation[vari].name,
                                                UserId = global_var.userId
                                            };

                                            lstDiscountPrice.Add(discPrice);
                                        }

                                        //무조건 2티어 옵션으로 만들기 위해
                                        tier_variation tv = new tier_variation
                                        {
                                            name = "Variation",
                                            options = lstOptions.ToArray(),
                                            //images_url = lstImages.ToArray()
                                        };

                                        lstTier_variation.Add(tv);


                                        item2TierInit = new Item2TierVariationInit
                                        {
                                            item_id = itemData.item_id,
                                            tier_variation = lstTier_variation,
                                            variation = lst2TierVariation
                                        };
                                    }

                                    //상품 기본 데이터 업데이트
                                    //상품명, 카테고리ID, 상품설명, sku, 속성, 
                                    bool rtn = updateItemData(itemData, ListAttribute, partner_id, shop_id, time_stamp, secertKey, 
                                        ListVariation, lst2TierVariation, lstOptions, lstImages, discount_id, lstDiscountPrice, 
                                        discountItemPrice, i, item2TierInit);

                                    //단순 모델일 경우만 수량 업데이트가 가능하다.
                                    //hasVariation = false 인 경우만

                                    if (!itemData.has_variation)
                                    {
                                        //int stock = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_stock"].Value.ToString().Trim().Replace(",",""));
                                        //shopeeApi.UpdateStock(itemData.item_id, stock, partner_id, shop_id, time_stamp, secertKey);
                                    }
                                    else
                                    {
                                        //옵션이 있는 경우 업데이트
                                        //각 옵션을 가지고 와야한다.

                                        //shopeeApi.UpdateVariationStockBatch(itemVariation, partner_id, shop_id, time_stamp, secertKey);
                                    }

                                    if(rtn)
                                    {
                                        dgItemList.Rows[i].Cells["dgItemList_Result"].Style.BackColor = Color.SkyBlue;
                                    }
                                    else
                                    {
                                        dgItemList.Rows[i].Cells["dgItemList_Result"].Style.BackColor = Color.Orange;
                                    }
                                }
                            }                            
                        }   
                    }
                }

                MessageBox.Show("모든 상품 정보를 업데이트 하였습니다","업데이트 완료",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        
        
        

        private bool updateItemData(ItemInfo itemData, List<shopee_attribute> ListItemAttribute, 
            long partner_id, long shop_id, long time_stamp, string secretKey, List<shopee_variations> ListItemVariation,            
            List<variation> lst2TierVariation,
            List<string> lstOption, List<string> lstImages, long discount_id, List<ItemDiscountPrice> lstDiscountPrice, decimal discountItemPrice,
            int rowId, Item2TierVariationInit item2TierInit)
        {            
            //상품의 기본 정보를 업데이트 한 후에 옵션 정보를 업데이트 한다.
            bool rtn = false;
            ShopeeApi shopeeApi = new ShopeeApi();

            dynamic ItemUpdatedData = shopeeApi.GetItemDetail(itemData.item_id, partner_id, shop_id, secretKey);
            dynamic updateResult = shopeeApi.UpdateItem(itemData, ListItemAttribute, partner_id, shop_id, secretKey);

            //상품 정보를 가지고오는 API
            //문제 발생 2019-11-05일 정보 업데이트 이후 다시 상세 정보를 호출 하였을 경우 옵션 정보를 가지고 오지 못함.
            //일정 시간이 지나야 동기화됨 1티어 옵션만 그럼
            //업데이트 결과는 옵션이 있는데, 상세정보에는 옵션이 없고 리스트가 없음.
            

            //결론은 1티어 옵션을 2티어로 변경하여 관리 함.
            if (updateResult.msg != null)
            {   
                dgItemList.Rows[rowId].Cells["dgItemList_Result"].Style.BackColor = Color.Orange;

                if(updateResult.msg.ToString().Contains("Must contains all mandatory attribute"))
                {
                    dgItemList.Rows[rowId].Cells["dgItemList_Result"].Value = "모든 필수 속성의 값이 입력되지 않았습니다.";
                    dgItemList.Rows[rowId].Cells["dgItemList_Result"].Style.BackColor = Color.Orange;
                    rtn = false;
                    Application.DoEvents();
                }
                else if(updateResult.msg.ToString().Contains("Update item success"))
                {
                    dgItemList.Rows[rowId].Cells["dgItemList_Result"].Style.BackColor = Color.GreenYellow;
                    dgItemList.Rows[rowId].Cells["dgItemList_Result"].Value = "업데이트 성공";
                    rtn = true;
                    Application.DoEvents();
                }


                //업데이트가 성공해야 진행한다.
                if(rtn)
                {
                    bool isReal2Tier = false;
                    if ((bool)updateResult.item.is_2tier_item)
                    {
                        isReal2Tier = true;
                    }

                    //2단인지 아닌지를 판별하여 1단이면 2단으로 바꾼다.
                    //옵션이 있는 경우만 2단으로 변경한다.

                    if(!isReal2Tier)
                    {
                        //1티어 이면서 할인이 걸려 있을 경우
                        if (ItemUpdatedData.item.discount_id > 0)
                        {
                            shopeeApi.DeleteDiscountItem((long)updateResult.item.discount_id,
                                    (long)updateResult.item.item_id,
                                    0,
                                    partner_id,
                                    shop_id,
                                    secretKey);
                        }

                        //GET 한 정보로 옵션 할인을 삭제하는 루틴
                        for (int i = 0; i < ItemUpdatedData.item.variations.Count; i++)
                        {
                            if (ItemUpdatedData.item.variations[i].discount_id != 0)
                            {
                                bool resultDeleteDiscountItem = false;
                                int retryCount = 0;
                                while (!resultDeleteDiscountItem && retryCount < 5)
                                {
                                    resultDeleteDiscountItem = shopeeApi.DeleteDiscountItem((long)ItemUpdatedData.item.variations[i].discount_id,
                                    (long)ItemUpdatedData.item.item_id,
                                    (long)ItemUpdatedData.item.variations[i].variation_id,
                                    partner_id,
                                    shop_id,
                                    secretKey);
                                    retryCount++;
                                }
                            }
                        }


                        //등록해야 할 옵션이 생겼을 수도 있다 옵션이 있는 경우만 2티어로 변경한다.
                        if (lstOption.Count > 0)
                        {    
                            //2티어로 변경한다.
                            bool rrr = shopeeApi.InitTierVariation(item2TierInit, partner_id, shop_id, secretKey);
                            dynamic result_tier_info = shopeeApi.GetTierVariations(itemData.item_id, partner_id, shop_id, secretKey);
                            isReal2Tier = true;
                        }
                    }

                    if (isReal2Tier)
                    {
                        //2티어 옵션인 경우            
                        //상품의 2티어 정보를 가지고 온다.
                        dynamic result_tier_info = shopeeApi.GetTierVariations(itemData.item_id, partner_id, shop_id, secretKey);

                        //만약 옵션할인이 걸려 있으면 할인 제거
                        //GET 한 정보로 옵션 할인을 삭제하는 루틴
                        for (int i = 0; i < ItemUpdatedData.item.variations.Count; i++)
                        {
                            if (ItemUpdatedData.item.variations[i].discount_id != 0)
                            {
                                bool resultDeleteDiscountItem = false;
                                int retryCount = 0;
                                while (!resultDeleteDiscountItem && retryCount < 5)
                                {
                                    resultDeleteDiscountItem = shopeeApi.DeleteDiscountItem((long)ItemUpdatedData.item.variations[i].discount_id,
                                    (long)ItemUpdatedData.item.item_id,
                                    (long)ItemUpdatedData.item.variations[i].variation_id,
                                    partner_id,
                                    shop_id,
                                    secretKey);
                                    retryCount++;
                                }
                            }
                        }

                        //상품 할인 제거
                        //discount_id가 0이면 해제 인데 기존에 걸려 있으면 해제
                        if (ItemUpdatedData.item.discount_id > 0)
                        {
                            shopeeApi.DeleteDiscountItem((long)updateResult.item.discount_id,
                                    (long)updateResult.item.item_id,
                                    0,
                                    partner_id,
                                    shop_id,
                                    secretKey);
                        }

                        //옵션을 모두 삭제한다.
                        //shopeeApi.DeleteVariationAll(result_tier_info, itemData.item_id, partner_id, shop_id, secretKey);
                        //shopeeApi.DeleteVariation(itemData.item_id, (long)updateResult.item.variations[0].variation_id, partner_id, shop_id, secretKey);
                        //shopeeApi.UpdateVariationPrice(itemData.item_id, firstVariationId, Convert.ToDecimal(lst2TierVariation[0].price.ToString()), partner_id, shop_id, secretKey);
                        //가격과 수량은 각각 변경해야 한다. api가 분리되어 있음

                        //모든 옵션의 재고수량을 변경
                        //shopeeApi.AddVariations(itemData.item_id, ListItemVariation, partner_id, shop_id, time_stamp, secretKey);

                        //옵션이 존재할 때만 시행한다.
                        if(lst2TierVariation.Count > 0)
                        {
                            //옵션의 리스트를 업데이트 한다.
                            shopeeApi.UpdateTierVariationList(itemData.item_id, partner_id, shop_id, secretKey, lstOption, lstImages);

                            //새로운 티어만 추가한다. 
                            shopeeApi.AddTierVariation(itemData.item_id, lst2TierVariation, partner_id, shop_id, secretKey);

                            dynamic ItemUpdatedData3 = shopeeApi.GetItemDetail(itemData.item_id, partner_id, shop_id, secretKey);
                            dynamic result_tier_info5 = shopeeApi.GetTierVariations(itemData.item_id, partner_id, shop_id, secretKey);
                            bool result2TierVariation = false;

                            dynamic result_tier_info2 = null;


                            Dictionary<string, long> dicVarPair = new Dictionary<string, long>();
                            int retryCount = 0;
                            while (!result2TierVariation)
                            {
                                if (retryCount > 20)
                                {
                                    break;
                                }
                                //상품의 2티어 정보를 가지고 온다.
                                //응답은 빠른데 variation_id가 없는 경우가 있다 이때 다시 호출해야 한다.
                                result_tier_info2 = shopeeApi.GetTierVariations(itemData.item_id, partner_id, shop_id, secretKey);

                                //리턴받은 데이터의 개수를 확인해서 다시 받아올지를 검증한다.
                                if (result_tier_info2.tier_variation != null)
                                {
                                    int listOptionCnt = 0;
                                    int listVariationCnt = 0;

                                    for (int i = 0; i < result_tier_info2.tier_variation.Count; i++)
                                    {
                                        listOptionCnt = listOptionCnt + result_tier_info2.tier_variation[i].options.Count;
                                    }

                                    listVariationCnt = result_tier_info2.variations.Count;

                                    if (listOptionCnt == listVariationCnt)
                                    {
                                        result2TierVariation = true;
                                    }
                                    else
                                    {
                                        //Thread.Sleep(2000);
                                        retryCount++;
                                    }
                                }
                                else
                                {
                                    //Thread.Sleep(2000);
                                    retryCount++;
                                }
                            }


                            if(result_tier_info2.tier_variation != null)
                            {
                                for (int i = 0; i < result_tier_info2.tier_variation.Count; i++)
                                {
                                    for (int j = 0; j < result_tier_info2.tier_variation[i].options.Count; j++)
                                    {
                                        dicVarPair.Add(result_tier_info2.tier_variation[i].options[j].ToString(), 0);
                                    }
                                }

                                for (int i = 0; i < result_tier_info2.variations.Count; i++)
                                {
                                    int idx = Convert.ToInt32(result_tier_info2.variations[i].tier_index[0]);
                                    for (int j = 0; j < dicVarPair.Count; j++)
                                    {
                                        if (idx == j)
                                        {
                                            string keyName = dicVarPair.Keys.ToList()[j];
                                            dicVarPair[keyName] = Convert.ToInt64(result_tier_info2.variations[i].variation_id);
                                        }
                                    }
                                }
                            }

                            


                            if (discount_id > 0)
                            {
                                //할인을 걸어달라고 했을 경우
                                //상품 옵션이 있으면 옵션에 걸리고 옵션이 없으면 상품에 걸리도록 되어 있다.
                                //충격!!!!! 순서가 뒤집혀서 온다
                                //인덱스를 점검해야 함
                                List<PromotionItem> lstPromotionItem = new List<PromotionItem>();
                                List<PromotionVariations> lstPV = new List<PromotionVariations>();
                                for (int j = 0; j < lstDiscountPrice.Count; j++)
                                {
                                    if (dicVarPair.ContainsKey(lstDiscountPrice[j].variationName.ToString().Trim()))
                                    {
                                        PromotionVariations PV = new PromotionVariations
                                        {
                                            variation_id = dicVarPair[lstDiscountPrice[j].variationName.ToString().Trim()],
                                            variation_promotion_price = lstDiscountPrice[j].discountPrice
                                        };
                                        lstPV.Add(PV);
                                    }
                                }

                                PromotionItem PI = new PromotionItem
                                {
                                    item_id = itemData.item_id,
                                    variations = lstPV,
                                    //옵션이 없는 경우 이 금액이 할인가가 된다.
                                    item_promotion_price = discountItemPrice,
                                    purchase_limit = (int)UdDiscountQty.Value
                                };

                                lstPromotionItem.Add(PI);

                                bool resultAddDiscountItem = false;

                                if (lstPromotionItem.Count > 0)
                                {
                                    retryCount = 0;
                                    while (!resultAddDiscountItem)
                                    {
                                        resultAddDiscountItem = shopeeApi.AddDiscountItem(discount_id, lstPromotionItem, partner_id, shop_id, secretKey);
                                        if (!resultAddDiscountItem)
                                        {
                                            Thread.Sleep(5000);
                                            retryCount++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //일반 옵션인 경우 중 옵션이 없는 경우이다.
                        //옵션이 있었다면 2티어로 변경해서 위에서 처리한다.
                        //여기서는 상품가격, 수량, 상품의 할인설정만 처리한다.

                        //    //옵션이 없는 경우는 상품 수량을 업데이트 한다.
                        shopeeApi.UpdateStock(itemData.item_id, itemData.stock, partner_id, shop_id, secretKey);


                        //    //옵션이 없는 경우 상품의 가격을 업데이트 한다.
                        shopeeApi.UpdatePrice(itemData.item_id, itemData.original_price, partner_id, shop_id, secretKey);



                        //할인이 걸려 있으면 삭제가 불가하다.
                        //할인에서  제거해 준다.
                        //할인은 2가지가 존재 상품자체 할인, 옵션 할인

                        //옵션할인 제거
                        //즉시 제거되지 않고 딜레이가 발생한다.
                        //업데이트 결과에 옵션 정보가 따라 오는데 그 정보는 정확하지 않다.
                        //그러나 할인 아이디는 업데이트 결과에서 가지고 와야 정확하다

                        
                        if (discount_id > 0 && ItemUpdatedData.item.discount_id != 0)
                        {
                            bool resultDeleteDiscountItem = false;
                            int retryCount = 0;
                            while (!resultDeleteDiscountItem && retryCount < 5)
                            {
                                resultDeleteDiscountItem = shopeeApi.DeleteDiscountItem((long)ItemUpdatedData.item.discount_id,
                                (long)ItemUpdatedData.item.item_id,
                                0,
                                partner_id,
                                shop_id,
                                secretKey);
                                retryCount++;
                            }
                        }

                        //할인 설정이 있으면 할인에 옵션을 모두 걸어준다.
                        //옵션은 할인설정 전에 소비자가로 등록 하였고 그 등록한 옵션의 variation_id가 생성 되었으므로 그걸로 등록한다.
                        //등록한 옵션에서 초기 등록해야 할 옵션의 이름과 비교하여 같은놈의 가격을 가지고 와서 등록한다.



                        //옵션은 없고 상품을 할인 걸어 달라고 했을 때
                        if (discount_id > 0)
                        {
                            List<PromotionItem> lstPromotionItem = new List<PromotionItem>();
                            PromotionItem PI = new PromotionItem
                            {
                                item_id = itemData.item_id,
                                item_promotion_price = discountItemPrice,
                                purchase_limit = (int)UdDiscountQty.Value
                            };

                            lstPromotionItem.Add(PI);

                            bool resultAddDiscountItem = false;
                            int retryCount = 0;
                            while (!resultAddDiscountItem && retryCount < 5)
                            {
                                resultAddDiscountItem = shopeeApi.AddDiscountItem(discount_id, lstPromotionItem, partner_id, shop_id, secretKey);
                                if (!resultAddDiscountItem)
                                {
                                    Thread.Sleep(5000);
                                    retryCount++;
                                }
                            }
                        }
                    }
                

                    rtn = true;
                }
            }

            return rtn;
        }


        bool isChanging;
        private void calcRowPrice(int rowId, string cellName)
        {
            isChanging = true;
            string TarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
            //등록기에서는 등록을 위하여 공급가로부터 시작하여 상품 판매가를 계산한다.
            long ItemId = Convert.ToInt64(dgItemList.Rows[rowId].Cells["dgItemList_item_id"].Value.ToString());
            decimal sourcePrice = Convert.ToDecimal(dgItemList.Rows[rowId].Cells["dgItemList_supply_price"].Value.ToString().Replace(",", ""));

            //무게를 그램으로 환산한다. 왜냐하면 요율표는 모두 그램으로 되어 있다.
            double tempWeight = Convert.ToDouble(dgItemList.Rows[rowId].Cells["dgItemList_weight"].Value.ToString().Replace(",", ""));
            int productWeight = (int)(tempWeight * 1000);

            decimal margin = Convert.ToDecimal(dgItemList.Rows[rowId].Cells["dgItemList_margin"].Value.ToString().Replace(",", ""));

            if (margin > 0)
            {
                dgItemList.Rows[rowId].Cells["dgItemList_margin"].Style.BackColor = Color.GreenYellow;
            }
            else
            {
                dgItemList.Rows[rowId].Cells["dgItemList_margin"].Style.BackColor = Color.Orange;
            }

            if (productWeight > 0)
            {
                dgItemList.Rows[rowId].Cells["dgItemList_weight"].Style.BackColor = Color.GreenYellow;
            }
            else
            {
                dgItemList.Rows[rowId].Cells["dgItemList_weight"].Style.BackColor = Color.Orange;
            }

            string countryCode = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();

            //판매국가의 환율
            decimal rateSrc = txt_src_currency_rate.Value;
            if (sourcePrice > 0 && margin > 0 && productWeight > 0)
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

                dgItemList.Rows[rowId].Cells["dgItemList_pg_fee"].Value = string.Format("{0:n2}", calcResult["pgFee"]);
                dgItemList.Rows[rowId].Cells["dgItemList_price"].Value = string.Format("{0:n2}", calcResult["targetSellPrice"]);
                dgItemList.Rows[rowId].Cells["dgItemList_price_won"].Value = string.Format("{0:n0}", calcResult["targetSellPriceKRW"]);
                dgItemList.Rows[rowId].Cells["dgItemList_original_price"].Value = string.Format("{0:n2}", calcResult["targetRetailPrice"]);


                //저장해 준다.
                using (AppDbContext context = new AppDbContext())
                {
                    ItemInfo result = context.ItemInfoes.SingleOrDefault(
                            b => b.item_id == ItemId && b.UserId == global_var.userId);

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
                    dgItemList.Rows[rowId].Cells["dgItemList_pg_fee"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgItemList.Rows[rowId].Cells["dgItemList_pg_fee"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetSellPrice"] > 0)
                {
                    dgItemList.Rows[rowId].Cells["dgItemList_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgItemList.Rows[rowId].Cells["dgItemList_price"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetSellPriceKRW"] > 0)
                {
                    dgItemList.Rows[rowId].Cells["dgItemList_price_won"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgItemList.Rows[rowId].Cells["dgItemList_price_won"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetRetailPrice"] > 0)
                {
                    dgItemList.Rows[rowId].Cells["dgItemList_original_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgItemList.Rows[rowId].Cells["dgItemList_original_price"].Style.BackColor = Color.Orange;
                }

                //마지막으로 입력한 값이 숫자니까 콤마 찍어주기
                //string strVar = dgItemList.Rows[rowId].Cells[cellName].Value.ToString().Replace(",", "");
                //decimal Var = 0;
                //if (decimal.TryParse(strVar, out Var))
                //{
                //    if(cellName == "dgItemList_weight")
                //    {
                //        dgItemList.Rows[rowId].Cells[cellName].Value = string.Format("{0:n1}", Var);
                //    }
                //    else
                //    {
                //        dgItemList.Rows[rowId].Cells[cellName].Value = string.Format("{0:n0}", Var);
                //    }
                //}

                if (sourcePrice > 0)
                {
                    dgItemList.Rows[rowId].Cells["dgItemList_supply_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgItemList.Rows[rowId].Cells["dgItemList_supply_price"].Style.BackColor = Color.Orange;
                }

                if (margin > 0)
                {
                    dgItemList.Rows[rowId].Cells["dgItemList_margin"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    dgItemList.Rows[rowId].Cells["dgItemList_margin"].Style.BackColor = Color.Orange;
                }

            }
            isChanging = false;
        }


        private void DgSrcItemList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

        }

        private void DgSrcItemList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                //상품명
                //어느국가인지 확인한 다음 길이를 제한한다.
                string currency = dgItemList.Rows[e.RowIndex].Cells["dgItemList_currency"].Value.ToString();
                int maxLen = 0;
                if (currency == "SGD")
                {
                    maxLen = 80;
                }
                else if (currency == "IDR")
                {
                    maxLen = 100;
                }
                else if (currency == "MYR")
                {
                    maxLen = 80;
                }
                else if (currency == "THB")
                {
                    maxLen = 120;
                }
                else if (currency == "TWD")
                {
                    maxLen = 60;
                }
                else if (currency == "PHP")
                {
                    maxLen = 80;
                }
                else if (currency == "VND")
                {
                    maxLen = 80;
                }

                string str = e.FormattedValue.ToString();
                int curLen = str.Length;
                if (curLen > maxLen)
                {
                    MessageBox.Show("최대 " + maxLen.ToString() + "자까지 입력 가능합니다.\r\n현재 " + curLen + "자 입니다", "최대입력 초과", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }

            }
            else if (e.ColumnIndex == 10)
            {
                //상품 공급가 수정일 때
                decimal input = 0;
                bool isOk = decimal.TryParse(e.FormattedValue.ToString().Replace(",", ""), out input);

                if (isOk)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else if (e.ColumnIndex == 11)
            {
                //상품 마진 수정일 때
                decimal input = 0;
                bool isOk = decimal.TryParse(e.FormattedValue.ToString().Replace(",", ""), out input);

                if (isOk)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else if (e.ColumnIndex == 15)
            {
                //상품 판매가 수정일 때
                decimal input = 0;
                bool isOk = decimal.TryParse(e.FormattedValue.ToString().Replace(",", ""), out input);

                if (isOk)
                {
                    //소비자가를 가지고 와서 비교한다. 소비자가보다 크면 안된다.
                    decimal retailPrice = Convert.ToDecimal(dgItemList.Rows[e.RowIndex].Cells["dgItemList_original_price"].Value.ToString().Replace(",", ""));
                    if (input > retailPrice)
                    {
                        MessageBox.Show("판매가격이 소비자가보다 클 수 없습니다.", "가격 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                }

            }
            else if (e.ColumnIndex == 16)
            {
                //상품 소비자가 수정일 때
                decimal input = 0;
                bool isOk = decimal.TryParse(e.FormattedValue.ToString().Replace(",", ""), out input);

                if (isOk)
                {
                    if(dgItemList.Rows[e.RowIndex].Cells["dgItemList_price"].Value != null && dgItemList.Rows[e.RowIndex].Cells["dgItemList_price"].Value.ToString() != string.Empty)
                    {
                        //소비자가를 가지고 와서 비교한다. 소비자가보다 크면 안된다.
                        decimal price = Convert.ToDecimal(dgItemList.Rows[e.RowIndex].Cells["dgItemList_price"].Value.ToString().Replace(",", ""));
                        decimal orgRetailPrice = Convert.ToDecimal(dgItemList.Rows[e.RowIndex].Cells["dgItemList_original_price"].Value.ToString().Replace(",", ""));
                        //이전에 있던 가격의 10%를 구한다.
                        var limitPrice = decimal.Multiply(orgRetailPrice, 0.1m);
                        if (input < price)
                        {
                            MessageBox.Show("소비자가격이 판매가격보다 작을 수 없습니다.", "가격 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Cancel = true;
                        }
                        else if (input > orgRetailPrice + limitPrice)
                        {
                            DialogResult dlg_Result = MessageBox.Show("입력한 가격이 기존 가격보다 10% 이상입니다. 기존 가격보다 10% 이상 인상할 경우 패널티가 부과될 수 있습니다. 가격을 적용하시겠습니까?", "가격 인상 경고", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dlg_Result == DialogResult.Yes)
                            {
                                e.Cancel = false;
                            }
                            else
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                }

            }
            else if (e.ColumnIndex == 17)
            {
                //수량
                int a = 0;
                var val = e.FormattedValue.ToString().Replace(",", "");

                if (!int.TryParse(val, out a))
                {
                    //문자인 경우
                    e.Cancel = true;
                }
            }
            else if (e.ColumnIndex == 21)
            {
                //무게인 경우
                double a = 0;
                var val = (string)e.FormattedValue;
                if (!double.TryParse(val, out a))
                {
                    e.Cancel = true;

                }
            }
            else if (e.ColumnIndex == 31)
            {
                //DTS
                int a = 0;
                var val = (string)e.FormattedValue;

                if (!int.TryParse(val, out a))
                {
                    //문자인 경우
                    e.Cancel = true;
                }
                else
                {
                    if (a == 2 || (a >= 5 && a <= 10))
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        MessageBox.Show("Pre-Oder 기간 설정은 기본 2일이며 설정시는 5일~10일까지 입니다.", "Pre-Order 설정 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
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

                    Fill_from_Currency_Names();
                }
            }   
        }

        private void Txt_src_currency_rate_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                BtnSaveCurrencyRate_Click(null, null);
            }
        }

        private void BtnGetDiscountList_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("쇼피 할인이벤트 목록을 동기화 하시겠습니까?", "할인 이벤트 동기화", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                PromotionMenu_getList_Click(null, null);

                MessageBox.Show("할인 이벤트 데이터를 동기화 하였습니다.", "할인 이벤트 동기화", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddDiscountEvent_Click(object sender, EventArgs e)
        {
            PromotionMenu_SetDiscount_Click(null, null);
        }

        private void BtnDeleteDiscountEvent_Click(object sender, EventArgs e)
        {
            PromotionMenu_UnCheckDiscount_Click(null, null);
        }

        private void UdDiscountQty_ValueChanged(object sender, EventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                ConfigVar result = context.ConfigVars.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result == null)
                {
                    ConfigVar newVar = new ConfigVar
                    {
                        discountQty = (int)UdDiscountQty.Value
                    };
                    context.ConfigVars.Add(newVar);
                    context.SaveChanges();
                }
                else
                {
                    result.discountQty = (int)UdDiscountQty.Value;
                    context.SaveChanges();
                }
            }
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            getShopeeAccount();
        }

        private void MainMenu_Update_Click(object sender, EventArgs e)
        {
            BtnUpdateProduct_Click(null, null);
        }

        private void MetroButton1_Click(object sender, EventArgs e)
        {
            ShopeeApi shopeeApi = new ShopeeApi();
            string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
            long partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
            long shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
            string secretKey = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
            long itemId = Convert.ToInt64(dgItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString());

            bool hasVar = false;
            //while(!hasVar)
            //{
                dynamic ItemUpdatedData = shopeeApi.GetItemDetail(itemId, partner_id, shop_id, secretKey);
            //    if(ItemUpdatedData.item.variations.Count > 0)
            //    {
            //        hasVar = true;
            //    }
            //    else
            //    {
            //        Thread.Sleep(1000);
            //    }
            //}

            //MessageBox.Show("상태변경");
            //shopeeApi.GetTierVariations(itemId, partner_id, shop_id, secretKey);


            //상품의 2티어 정보를 가지고 온다.
            dynamic result_tier_info2 = shopeeApi.GetTierVariations(itemId, partner_id, shop_id, secretKey);
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

        private void BtnCalcSellPrice_Click(object sender, EventArgs e)
        {
            if (dgItemList.Rows.Count > 0)
            {
                for (int i = 0; i < dgItemList.Rows.Count; i++)
                {
                    if (dgItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        calcRowPrice(dgItemList.Rows[i].Index, "");
                    }
                }
            }
        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }
    }
}
