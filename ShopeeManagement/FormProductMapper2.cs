using MetroFramework.Forms;
using Newtonsoft.Json;
using OfficeOpenXml;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class FormProductMapper2 : MetroForm
    {
        private CancellationTokenSource _cts;
        List<long> lstSrcItemId = new List<long>();
        public Dictionary<long, long> dicSrcItemList = new Dictionary<long, long>();

        List<long> lstTarItemId = new List<long>();
        public Dictionary<long, long> dicTarItemList = new Dictionary<long, long>();


        public FormProductMapper2(string lang)
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager;
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
        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
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
        private void FillHfType()
        {
            dg_hf_category.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<HFType> hfTypeList = context.HFTypes.Where(x => x.UserId == global_var.userId).OrderBy(x => x.HFTypeName).ToList();

                for (int i = 0; i < hfTypeList.Count; i++)
                {
                    dg_hf_category.Rows.Add(hfTypeList[i].HFTypeID, 
                        i +1 , false,
                        hfTypeList[i].HFTypeName);
                }

                if(dg_hf_category.Rows.Count > 0 && dg_hf_category.SelectedRows.Count > 0)
                {
                    int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());
                    FillHfList(HFTypeID);
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

                for (int i = 0; i < hfList.Count; i++)
                {
                    dg_hf_list.Rows.Add(hfList[i].HFListID, i + 1, false, hfList[i].HFName);
                    TxtHFContent.Text = hfList[i].HFContent;
                }

                if(dg_hf_list.Rows.Count > 0)
                {
                    txt_template_name.Text = dg_hf_list.SelectedRows[0].Cells["dg_hf_list_template_name"].Value.ToString();
                }
            }
        }

        private void FillTemplateHeader(string shopeeAccount)
        {
            dg_list_header.Rows.Clear();
            TxtHFContent.Text = "";
            using (AppDbContext context = new AppDbContext())
            {
                var lst = (from template in context.TemplateHeaders
                           join H in context.HFLists
                           on template.HFListID equals H.HFListID
                           where template.ShopeeAccount == shopeeAccount && template.UserId == global_var.userId
                           orderby template.OrderIdx
                           select new
                           {
                               TemplateId = template.Id,
                               TemplateName = H.HFName,
                               HFListId = H.HFListID
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

        private string FullTemplateHeader(string shopeeAccount)
        {
            StringBuilder sbHeader = new StringBuilder();

            using (AppDbContext context = new AppDbContext())
            {
                var lst = (from template in context.TemplateHeaders
                           join H in context.HFLists
                           on template.HFListID equals H.HFListID
                           where template.ShopeeAccount == shopeeAccount && template.UserId == global_var.userId
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

        private string FullTemplateFooter(string shopeeAccount)
        {
            StringBuilder sbFooter = new StringBuilder();

            using (AppDbContext context = new AppDbContext())
            {
                var lst = (from template in context.TemplateFooters
                           join H in context.HFLists
                           on template.HFListID equals H.HFListID
                           where template.ShopeeAccount == shopeeAccount && template.UserId == global_var.userId
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
        private void FillTemplateFooter(string shopeeAccount)
        {
            dg_list_footer.Rows.Clear();
            TxtHFContent.Text = "";
            using (AppDbContext context = new AppDbContext())
            {
                var lst = (from template in context.TemplateFooters
                           join H in context.HFLists
                           on template.HFListID equals H.HFListID
                           where template.ShopeeAccount == shopeeAccount && template.UserId == global_var.userId
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
        private void FormProductMapper_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            set_double_buffer();
            cboPriceOp.SelectedIndex = 0;
            cboRateOp.SelectedIndex = 0;
            dt_start.Value = DateTime.Now.AddDays(-15);
            getShopeeAccount();
            Fill_Currency_Date();
            Fill_from_Currency_Names();
            Fill_to_Currency_Names();
            FillHfType();
            FillHFSeparator();

            Cursor.Current = Cursors.Default;
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

        private void BtnSetSource_Click(object sender, EventArgs e)
        {
            if (dg_site_id.Rows.Count > 0)
            {
                TxtSourceCountry.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();                
                TxtSourceId.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();

                for (int i = 0; i < cbo_currency_to.Items.Count; i++)
                {
                    KeyValuePair<string, decimal> currency_info = (KeyValuePair<string, decimal>)cbo_currency_From.Items[i];
                    if (currency_info.Key.ToString().Contains(TxtSourceCountry.Text))
                    {
                        cbo_currency_From.SelectedIndex = i;
                        //txt_tar_currency_rate.Text = currency_info.Value.ToString();
                        txt_src_currency_rate.Text = currency_info.Value.ToString();
                    }
                }
            }

            if (TxtSourceId.Text.Trim().ToUpper() == TxtTargetId.Text.Trim().ToUpper())
            {
                TxtSourceCountry.Text = "";
                TxtSourceId.Text = "";

                MessageBox.Show("같은 아이디를 설정할 수 없습니다.", "동일 아이디", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void BtnSetTarget_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dg_shopee_discount.Rows.Clear();
            if (dg_site_id.Rows.Count > 0)
            {
                TxtTargetId.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                TxtTargetCountry.Text = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();

                for (int i = 0; i < cbo_currency_to.Items.Count; i++)
                {
                    KeyValuePair<string, decimal> currency_info = (KeyValuePair<string, decimal>)cbo_currency_to.Items[i];                    
                    if (currency_info.Key.ToString().Contains(TxtTargetCountry.Text))
                    {
                        cbo_currency_to.SelectedIndex = i;
                        txt_tar_currency_rate.Text = currency_info.Value.ToString();
                    }
                }
            }

            if (TxtSourceId.Text.Trim().ToUpper() == TxtTargetId.Text.Trim().ToUpper())
            {
                TxtTargetCountry.Text = "";
                TxtTargetId.Text = "";

                MessageBox.Show("같은 아이디를 설정할 수 없습니다.", "동일 아이디", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                FillTemplateHeader(TxtTargetId.Text.Trim());
                FillTemplateFooter(TxtTargetId.Text.Trim());
                get_shopee_logistic(TxtTargetCountry.Text.Trim(),
                    Convert.ToInt32(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString()),
                    Convert.ToInt32(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString()),
                    dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString());
                getShopeeDiscount(0, 100,
                    Convert.ToInt32(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString()),
                    Convert.ToInt32(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString()),
                    dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString());
            }
            //btnGetItemList_Click(null, null);
            Cursor.Current = Cursors.Default;
        }
        private static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
        private string HashString(string StringToHash, string HashKey)
        {
            byte[] key = Encoding.UTF8.GetBytes(HashKey);
            byte[] Text = Encoding.UTF8.GetBytes(StringToHash);

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
        private bool getSrcItemList(long i_partner_id, long i_shop_id, string api_key, int pagination_offset, long update_time_from, long update_time_to)
        {
            bool rtn = false;
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

            dynamic result_item_info = null;
            try
            {
                result_item_info = JsonConvert.DeserializeObject(response_item_info.Content);

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
            catch (Exception ex)
            {

            }

            return rtn;
        }

        private bool getTarItemList(long i_partner_id, long i_shop_id, string api_key, int pagination_offset, long update_time_from, long update_time_to)
        {
            bool rtn = false;
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

            dynamic result_item_info = null;
            try
            {
                result_item_info = JsonConvert.DeserializeObject(response_item_info.Content);

                for (int i = 0; i < result_item_info.items.Count; i++)
                {
                    lstTarItemId.Add(Convert.ToInt64(result_item_info.items[i].item_id));
                    dicTarItemList.Add(Convert.ToInt64(result_item_info.items[i].item_id), Convert.ToInt64(result_item_info.items[i].item_id));
                }
                if ((bool)result_item_info.more)
                {
                    rtn = true;
                    //getItemList(ipagination_offset = ipagination_offset + 100, update_time_from, update_time_to);
                }
            }
            catch (Exception ex)
            {

            }

            return rtn;
        }
        
        private void getSrcItem(long src_partner_id, long src_shop_id, string src_api_key, 
            int ipagination_offset, long update_time_from, long update_time_to)
        {   
            bool rtn = false;
            rtn = getSrcItemList(src_partner_id, src_shop_id, src_api_key, ipagination_offset, update_time_from, update_time_to);

            if (rtn)
            {
                while (rtn)
                {
                    ipagination_offset = ipagination_offset + 100;
                    rtn = getSrcItemList(src_partner_id, src_shop_id, src_api_key, ipagination_offset, update_time_from, update_time_to);
                }
            }
            #region  EDITED For 이전 이미지 병렬 로드 취소
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            #endregion
            Cursor.Current = Cursors.WaitCursor;
            FetchItemData(src_partner_id, src_shop_id, src_api_key, _cts, lstSrcItemId);
        }

        private void getTarItem(long tar_partner_id, long tar_shop_id, string tar_api_key,
            int ipagination_offset, long update_time_from, long update_time_to)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool rtn = false;
            rtn = getTarItemList(tar_partner_id, tar_shop_id, tar_api_key, ipagination_offset, update_time_from, update_time_to);

            if (rtn)
            {
                while (rtn)
                {
                    ipagination_offset = ipagination_offset + 100;
                    rtn = getTarItemList(tar_partner_id, tar_shop_id, tar_api_key, ipagination_offset, update_time_from, update_time_to);
                }
            }
            #region  EDITED For 이전 이미지 병렬 로드 취소
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            #endregion

            Cursor.Current = Cursors.WaitCursor;
            FetchTarItemData(tar_partner_id, tar_shop_id, tar_api_key, _cts, lstTarItemId);
        }
        private void btnGetItemList_Click(object sender, EventArgs e)
        {
            if (TxtTargetId.Text.Trim() == string.Empty)
            {
                MessageBox.Show("복제 대상 아이디를 설정 하세요.", "복제 대상 아이디 설정", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (TxtSourceId.Text.Trim() == string.Empty || TxtTargetId.Text.Trim() == string.Empty)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            dgTarItemList.Rows.Clear();
            btnGetTarItemList.Enabled = false;
            grp_src.Text = "원본 상품 목록";

            dgSrcItemList.Rows.Clear();

            lstSrcItemId.Clear();
            dicSrcItemList.Clear();

            int ipagination_offset = 0;
            long src_partner_id = 0;
            long tar_partner_id = 0;

            long src_shop_id = 0;
            long tar_shop_id = 0;

            string src_api_key = "";
            string tar_api_key = "";

            for (int i = 0; i < dg_site_id.Rows.Count; i++)
            {
                if(dg_site_id.Rows[i].Cells["dg_site_id_id"].Value.ToString() == TxtSourceId.Text)
                {
                    src_partner_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_partner_id"].Value.ToString());
                    src_shop_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_shop_id"].Value.ToString());
                    src_api_key = dg_site_id.Rows[i].Cells["dg_site_id_secret_key"].Value.ToString();
                    break;
                }
            }

            for (int i = 0; i < dg_site_id.Rows.Count; i++)
            {
                if (dg_site_id.Rows[i].Cells["dg_site_id_id"].Value.ToString() == TxtTargetId.Text)
                {
                    tar_partner_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_partner_id"].Value.ToString());
                    tar_shop_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_shop_id"].Value.ToString());
                    tar_api_key = dg_site_id.Rows[i].Cells["dg_site_id_secret_key"].Value.ToString();
                    break;
                }
            }

            long update_time_from = ToUnixTime(dt_start.Value);
            //long update_time_from = ToUnixTime(DateTime.Now.AddDays(-3).AddHours(-9));
            long update_time_to = ToUnixTime(dt_end.Value.AddHours(-9));

            Cursor.Current = Cursors.WaitCursor;
            getSrcItem(src_partner_id, src_shop_id, src_api_key, ipagination_offset, update_time_from, update_time_to);
            //getTarItem(tar_partner_id, tar_shop_id, tar_api_key, ipagination_offset, update_time_from, update_time_to);

            
        }

        public static void UnLockMaxParallel()
        {
            int prevThreads, prevPorts;
            ThreadPool.GetMinThreads(out prevThreads, out prevPorts);
            ThreadPool.SetMinThreads(2000, prevPorts);
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        private void FetchItemData(long i_partner_id, long i_shop_id, string api_key, CancellationTokenSource cts, List<long> lstItem)
        {
            #region EDITED For 이미지 병렬 로드
            UnLockMaxParallel();
            #endregion
            var current_synchronization_context = TaskScheduler.FromCurrentSynchronizationContext();
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Task.Run(() =>
                {
                    Parallel.ForEach(lstItem, new ParallelOptions { MaxDegreeOfParallelism = 20 } , LstRow =>
                    {
                    Cursor.Current = Cursors.WaitCursor;
                    //try
                    //{
                        if (cts.IsCancellationRequested) return;
                        var item_id = LstRow;
                        if (item_id != 0)
                        {
                            dynamic ItemData = getItemDetail(item_id, i_partner_id, i_shop_id, api_key);
                            if (ItemData != null)
                            {
                                var Images = ItemData.item.images;
                                WebClient wc = new WebClient();
                                byte[] bytes = wc.DownloadData(Images[0].ToString());
                                MemoryStream ms = new MemoryStream(bytes);
                                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

                                //배송비를 쪼갠다.
                                var Shipping = ItemData.item.logistics.ToString().Replace("\r\n", "");
                                dynamic dynShipping = JsonConvert.DeserializeObject(Shipping);

                                bool isFreeShipping = true;
                                string estShippingFee = "";
                                        
                                if(dynShipping != null && dynShipping.Count > 0)
                                {
                                    isFreeShipping = (bool)dynShipping[0].is_free;
                                    estShippingFee = dynShipping[0].estimated_shipping_fee;
                                }

                                string SrcShopeeAccount = "";
                                TxtSourceId.Invoke((MethodInvoker)delegate ()
                                {
                                    SrcShopeeAccount = TxtSourceId.Text.Trim();
                                });
                                    //상품 정보를 저장한다.
                                using (AppDbContext context = new AppDbContext())
                                {
                                    long ItemId = ItemData.item.item_id;
                                    ItemInfo result = context.ItemInfoes.SingleOrDefault(
                                    b => b.item_id == ItemId && b.UserId == global_var.userId);

                                    DateTime dtCreateTime = UnixTimeStampToDateTime(Convert.ToInt64(ItemData.item.create_time));
                                    DateTime dtUpdateTime = UnixTimeStampToDateTime(Convert.ToInt64(ItemData.item.update_time));
                                    string[] arImages = ItemData.item.images.ToObject<string[]>();
                                    StringBuilder strImages = new StringBuilder();

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

                                    if (result == null)
                                    {
                                        if (ItemData.item.variations.Count > 0)
                                        {
                                            for (int vari = 0; vari < ItemData.item.variations.Count; vari++)
                                            {
                                                DateTime variCreateTime = UnixTimeStampToDateTime(Convert.ToInt64(ItemData.item.variations[vari].create_time));
                                                DateTime variUpdateTime = UnixTimeStampToDateTime(Convert.ToInt64(ItemData.item.variations[vari].update_time));
                                                var ItemVariation = new ItemVariation
                                                {
                                                    variation_id = ItemData.item.variations[vari].variation_id,
                                                    variation_sku = ItemData.item.variations[vari].variation_sku,
                                                    name = ItemData.item.variations[vari].name,
                                                    price = ItemData.item.variations[vari].price,
                                                    stock = ItemData.item.variations[vari].stock,
                                                    status = ItemData.item.variations[vari].status,
                                                    create_time = variCreateTime,
                                                    update_time = variUpdateTime,
                                                    original_price = ItemData.item.variations[vari].original_price,
                                                    discount_id = ItemData.item.variations[vari].discount_id,
                                                    item_id = ItemData.item.item_id,
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
                                                    b.attribute_id == attributeId);

                                                    if (resultAttr == null)
                                                    {
                                                        var ItemAttribute = new ItemAttribute
                                                        {
                                                            attribute_id = ItemData.item.attributes[attr].attribute_id,
                                                            attribute_name = ItemData.item.attributes[attr].attribute_name,
                                                            is_mandatory = ItemData.item.attributes[attr].is_mandatory,
                                                            attribute_type = ItemData.item.attributes[attr].attribute_type,
                                                            attribute_value = ItemData.item.attributes[attr].attribute_value,
                                                            item_id = ItemData.item.item_id,
                                                            UserId = global_var.userId
                                                        };
                                                        context.ItemAttributes.Add(ItemAttribute);
                                                    }
                                                    else
                                                    {
                                                        resultAttr.attribute_name = ItemData.item.attributes[attr].attribute_name;
                                                        resultAttr.is_mandatory = ItemData.item.attributes[attr].is_mandatory;
                                                        resultAttr.attribute_type = ItemData.item.attributes[attr].attribute_type;
                                                        resultAttr.attribute_value = ItemData.item.attributes[attr].attribute_value;
                                                        resultAttr.item_id = ItemData.item.item_id;
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
                                                long logisticId = ItemData.item.logistics[logi].logistic_id;


                                                using (AppDbContext contextLogi = new AppDbContext())
                                                {
                                                    ItemLogistic resultLogi = context.ItemLogistics.SingleOrDefault(
                                                    b => b.item_id == TempItemId &&
                                                    b.logistic_id == logisticId);

                                                    if (resultLogi == null)
                                                    {
                                                        var ItemLogistic = new ItemLogistic
                                                        {
                                                            enabled = ItemData.item.logistics[logi].enabled,
                                                            estimated_shipping_fee = ItemData.item.logistics[logi].estimated_shipping_fee,
                                                            is_free = ItemData.item.logistics[logi].is_free,
                                                            logistic_id = ItemData.item.logistics[logi].logistic_id,
                                                            logistic_name = ItemData.item.logistics[logi].logistic_name,
                                                            item_id = ItemData.item.item_id,
                                                            UserId = global_var.userId

                                                            //정의는 되어 있으나 필드가 안넘어옴
                                                            //shipping_fee = ItemData.item.logistics[logi].shipping_fee,
                                                            //size_id = ItemData.item.logistics[logi].size_id,
                                                        };
                                                        context.ItemLogistics.Add(ItemLogistic);
                                                    }
                                                    else
                                                    {
                                                        resultLogi.enabled = ItemData.item.logistics[logi].enabled;
                                                        resultLogi.estimated_shipping_fee = ItemData.item.logistics[logi].estimated_shipping_fee;
                                                        resultLogi.is_free = ItemData.item.logistics[logi].is_free;
                                                        resultLogi.logistic_id = ItemData.item.logistics[logi].logistic_id;
                                                        resultLogi.logistic_name = ItemData.item.logistics[logi].logistic_name;
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
                                                    min = ItemData.item.wholesales[sale].min,
                                                    max = ItemData.item.wholesales[sale].max,
                                                    unit_price = ItemData.item.wholesales[sale].unit_price,
                                                    item_id = ItemData.item.item_id,
                                                    UserId = global_var.userId
                                                };
                                                context.ItemWholesales.Add(ItemWholesale);
                                            }
                                        }

                                        var itemInfo = new ItemInfo
                                        {
                                            shopeeAccount = SrcShopeeAccount,
                                            item_id = ItemData.item.item_id,
                                            shopid = ItemData.item.shopid,
                                            item_sku = ItemData.item.item_sku,
                                            status = ItemData.item.status,
                                            name = ItemData.item.name,
                                            description = ItemData.item.description,
                                            currency = ItemData.item.currency,
                                            has_variation = ItemData.item.has_variation,
                                            price = ItemData.item.price,
                                            stock = ItemData.item.stock,
                                            create_time = dtCreateTime,
                                            update_time = dtUpdateTime,
                                            weight = ItemData.item.weight,
                                            category_id = ItemData.item.category_id,
                                            original_price = ItemData.item.original_price,
                                            rating_star = ItemData.item.rating_star,
                                            cmt_count = ItemData.item.cmt_count,
                                            views = ItemData.item.views,
                                            likes = ItemData.item.likes,
                                            package_length = ItemData.item.package_length,
                                            package_width = ItemData.item.package_width,
                                            package_height = ItemData.item.package_height,
                                            days_to_ship = ItemData.item.days_to_ship,
                                            size_chart = ItemData.item.size_chart,
                                            condition = ItemData.item.condition,
                                            discount_id = ItemData.item.discount_id,
                                            is_2tier_item = ItemData.item.is_2tier_item,
                                            images = strImages.ToString(),
                                            UserId = global_var.userId
                                        };

                                        context.ItemInfoes.Add(itemInfo);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        result.item_sku = ItemData.item.item_sku;
                                        result.status = ItemData.item.status;
                                        result.name = ItemData.item.name;
                                        result.description = ItemData.item.description;                                                
                                        result.currency = ItemData.item.currency;
                                        result.has_variation = ItemData.item.has_variation;
                                        result.price = ItemData.item.price;
                                        result.stock = ItemData.item.stock;
                                        result.create_time = dtCreateTime;
                                        result.update_time = dtUpdateTime;
                                        result.weight = ItemData.item.weight;
                                        result.category_id = ItemData.item.category_id;
                                        result.original_price = ItemData.item.original_price;
                                        result.rating_star = ItemData.item.rating_star;
                                        result.cmt_count = ItemData.item.cmt_count;
                                        result.views = ItemData.item.views;
                                        result.likes = ItemData.item.likes;
                                        result.package_length = ItemData.item.package_length;
                                        result.package_width = ItemData.item.package_width;
                                        result.package_height = ItemData.item.package_height;
                                        result.days_to_ship = ItemData.item.days_to_ship;
                                        result.size_chart = ItemData.item.size_chart;
                                        result.condition = ItemData.item.condition;
                                        result.discount_id = ItemData.item.discount_id;
                                        result.is_2tier_item = ItemData.item.is_2tier_item;
                                        result.images = strImages.ToString();
                                        context.SaveChanges();
                                    }
                                }
                                    //쇼피는 옵션이 있으면 모두 2티어로 나온다.
                                    //진짜 2티어이면 옵션명에 콤마로 구분되어 나온다.

                                dgSrcItemList.Invoke((MethodInvoker)delegate ()
                                {
                                    Cursor.Current = Cursors.WaitCursor;
                                    dgSrcItemList.Rows.Add(dgSrcItemList.Rows.Count + 1,
                                        false,
                                        img,
                                        ItemData.item.item_id,
                                        ItemData.item.shopid,
                                        ItemData.item.item_sku,
                                        ItemData.item.status,
                                        ItemData.item.name,
                                        ItemData.item.description,
                                        ItemData.item.images,
                                        ItemData.item.currency,
                                        ItemData.item.has_variation,
                                        string.Format("{0:n0}", ItemData.item.price),
                                        "",
                                        "",
                                        string.Format("{0:n0}", ItemData.item.original_price),
                                        "",
                                        string.Format("{0:n0}", ItemData.item.stock),
                                        "",
                                        ItemData.item.create_time,
                                        ItemData.item.update_time,
                                        ItemData.item.weight * 1000,
                                        "",
                                        isFreeShipping,
                                        estShippingFee,
                                        ItemData.item.category_id,
                                        "",
                                        false,
                                        ItemData.item.variations,
                                        ItemData.item.attributes,
                                        ItemData.item.logistics,
                                        ItemData.item.wholesales,
                                        ItemData.item.rating_star,
                                        string.Format("{0:n0}", ItemData.item.cmt_count),
                                        string.Format("{0:n0}", ItemData.item.sales),
                                        string.Format("{0:n0}", ItemData.item.views),
                                        string.Format("{0:n0}", ItemData.item.likes),
                                        string.Format("{0:n0}", ItemData.item.package_length),
                                        string.Format("{0:n0}", ItemData.item.package_width),
                                        string.Format("{0:n0}", ItemData.item.package_height),
                                        ItemData.item.days_to_ship,
                                        ItemData.item.size_chart,
                                        ItemData.item.condition,
                                        ItemData.item.discount_id,
                                        (bool)ItemData.item.is_2tier_item);

                                    if (ItemData.item.status == "BANNED")
                                    {
                                        dgSrcItemList.Rows[dgSrcItemList.Rows.Count - 1].Cells["dgItemList_status"].Style.BackColor = Color.Orange;
                                    }
                                    Cursor.Current = Cursors.WaitCursor;
                                });

                                Cursor.Current = Cursors.WaitCursor;
                            }
                            else
                            {
                                Cursor.Current = Cursors.WaitCursor;
                            }
                        }
                        else
                        {
                            Cursor.Current = Cursors.WaitCursor;
                        }
                            //}
                            //catch (Exception ex)
                            //{

                            //    Cursor.Current = Cursors.WaitCursor;
                            //}
                    });
                }).ContinueWith(t =>
                {
                    grp_src.Invoke((MethodInvoker)delegate ()
                    {
                        grp_src.Text = "원본 상품 목록 : [ " + string.Format("{0:n0}", dgSrcItemList.Rows.Count) + " ]";
                    });

                    btnGetTarItemList.Invoke((MethodInvoker)delegate ()
                    {
                        btnGetTarItemList.Enabled = true;
                    });

                    dgSrcItemList.Invoke((MethodInvoker)delegate ()
                    {
                        if (dgSrcItemList.Rows.Count > 0)
                        {
                            string SrcCountry = TxtSourceCountry.Text.Trim();
                            string SrcShopeeId = TxtSourceId.Text.Trim();
                            string TarCountry = TxtTargetCountry.Text.Trim();
                            string TarShopeeId = TxtTargetId.Text.Trim();

                            for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                            {
                                //원본 소비자가
                                dgSrcItemList.Rows[i].Cells["dgItemList_price"].Style.BackColor = Color.GreenYellow;

                                //원본 판가
                                dgSrcItemList.Rows[i].Cells["dgItemList_original_price"].Style.BackColor = Color.GreenYellow;

                                //원본 수량
                                dgSrcItemList.Rows[i].Cells["dgItemList_stock"].Style.BackColor = Color.GreenYellow;

                                //원본 무게
                                dgSrcItemList.Rows[i].Cells["dgItemList_weight"].Style.BackColor = Color.GreenYellow;

                                //원본 배송비
                                dgSrcItemList.Rows[i].Cells["dgItemList_shipping_fee"].Style.BackColor = Color.GreenYellow;

                                //원본 카테고리
                                dgSrcItemList.Rows[i].Cells["dgItemList_category_id"].Style.BackColor = Color.GreenYellow;

                                //원본 할인ID
                                dgSrcItemList.Rows[i].Cells["dgItemList_discount_id"].Style.BackColor = Color.GreenYellow;

                                //원본 무료배송
                                dgSrcItemList.Rows[i].Cells["dgItemList_isFreeShipping"].Style.BackColor = Color.GreenYellow;

                                //원본 DTS
                                dgSrcItemList.Rows[i].Cells["dgItemList_days_to_ship"].Style.BackColor = Color.GreenYellow;


                                string srcProductId = dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString();

                                string strVariation = dgSrcItemList.Rows[i].Cells["dgItemList_variations"].Value.ToString().Trim();
                                dynamic dynVariation = JsonConvert.DeserializeObject(strVariation);

                                //상품에 관련된 모든 Variation을 가지고 온다.
                                using (AppDbContext context = new AppDbContext())
                                {
                                    List<ShopeeVariationPrice> priceList = context.ShopeeVariationPrices
                                    .Where(b => b.SrcShopeeCountry == SrcCountry &&
                                            b.SrcShopeeId == SrcShopeeId &&
                                            b.TarShopeeCountry == TarCountry &&
                                            b.TarShopeeId == TarShopeeId &&
                                            b.productId == srcProductId
                                            && b.UserId == global_var.userId)
                                    .OrderBy(x => x.variation_id).ToList();
                                    if (priceList.Count == 0)
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Value = "";
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Value = "";
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_weight"].Value = "";
                                        dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Value = false;

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Style.BackColor = Color.Orange;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Style.BackColor = Color.Orange;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_weight"].Style.BackColor = Color.Orange;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Style.BackColor = Color.Orange;
                                    }
                                    else if (priceList.Count == 1)
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Value = priceList[0].TarNetPrice;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Value = priceList[0].TarRetail_price;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_weight"].Value = priceList[0].product_weight;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Value = true;

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Style.BackColor = Color.GreenYellow;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Style.BackColor = Color.GreenYellow;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_weight"].Style.BackColor = Color.GreenYellow;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Style.BackColor = Color.GreenYellow;
                                    }
                                    else if (priceList.Count > 1)
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Value = priceList[0].TarNetPrice;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Value = priceList[0].TarRetail_price;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_weight"].Value = priceList[0].product_weight;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Value = true;

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Style.BackColor = Color.GreenYellow;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Style.BackColor = Color.GreenYellow;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_weight"].Style.BackColor = Color.GreenYellow;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Style.BackColor = Color.GreenYellow;
                                    }
                                }
                            }
                        }
                    });

                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("원본 상품 정보를 모두 수신 하였습니다.", "상품 정보 수신", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
            catch
            {
                // ignore
            }
        }

        private void FetchTarItemData(long i_partner_id, long i_shop_id, string api_key, CancellationTokenSource cts, List<long> lstTarItem)
        {
            #region EDITED For 이미지 병렬 로드
            UnLockMaxParallel();
            #endregion
            var current_synchronization_context = TaskScheduler.FromCurrentSynchronizationContext();
            Cursor.Current = Cursors.WaitCursor;
            int cnt = 0;
            try
            {
                Task.Run(() =>
                {
                    Parallel.ForEach(lstTarItem,
                        new ParallelOptions { MaxDegreeOfParallelism = 20 }
                        , LstRow =>
                        {
                            try
                            {
                                if (cts.IsCancellationRequested) return;
                                Cursor.Current = Cursors.WaitCursor;
                                var item_id = LstRow;
                                if (item_id != 0)
                                {
                                    Cursor.Current = Cursors.WaitCursor;
                                    dynamic ItemData = getItemDetail(item_id, i_partner_id, i_shop_id, api_key);
                                    if (ItemData != null)
                                    {
                                        var Images = ItemData.item.images;
                                        WebClient wc = new WebClient();
                                        byte[] bytes = wc.DownloadData(Images[0].ToString());
                                        MemoryStream ms = new MemoryStream(bytes);
                                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

                                        dgTarItemList.Invoke((MethodInvoker)delegate ()
                                        {
                                            dgTarItemList.Rows.Add(dgTarItemList.Rows.Count + 1,
                                                false,
                                                img,
                                                ItemData.item.item_id,
                                                ItemData.item.shopid,
                                                ItemData.item.item_sku,
                                                ItemData.item.status,
                                                ItemData.item.name,
                                                ItemData.item.description,
                                                ItemData.item.images,
                                                ItemData.item.currency,
                                                ItemData.item.has_variation,
                                                ItemData.item.price,
                                                ItemData.item.stock,
                                                ItemData.item.create_time,
                                                ItemData.item.update_time,
                                                ItemData.item.weight,
                                                ItemData.item.category_id,
                                                ItemData.item.original_price,
                                                ItemData.item.variations,
                                                ItemData.item.attributes,
                                                ItemData.item.logistics,
                                                ItemData.item.wholesales,
                                                ItemData.item.rating_star,
                                                ItemData.item.cmt_count,
                                                ItemData.item.sales,
                                                ItemData.item.views,
                                                ItemData.item.likes,
                                                ItemData.item.package_length,
                                                ItemData.item.package_width,
                                                ItemData.item.package_height,
                                                ItemData.item.days_to_ship,
                                                ItemData.item.size_chart,
                                                ItemData.item.condition,
                                                ItemData.item.discount_id);

                                            if (ItemData.item.status == "BANNED")
                                            {
                                                dgTarItemList.Rows[dgTarItemList.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                                            }
                                            cnt++;
                                            Cursor.Current = Cursors.WaitCursor;
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Cursor.Current = Cursors.WaitCursor;
                            }
                        });
                }).ContinueWith(t =>
                {
                    grp_tar.Invoke((MethodInvoker)delegate ()
                    {
                        grp_tar.Text = "대상 상품 목록 : [ " + string.Format("{0:n0}", dgTarItemList.Rows.Count) + " ]";
                    });

                    btnGetSrcItemList.Invoke((MethodInvoker)delegate ()
                    {
                        btnGetSrcItemList.Enabled = true;
                    });

                    //상품 목록을 가지고 올때 연결된 상품 정보도 가지고 온다.
                    using (AppDbContext context = new AppDbContext())
                    {
                        List<ProductLink> accountList = context.ProductLinks.Where(
                            a => a.SourceCountry == TxtSourceCountry.Text &&
                            a.TargetCountry == TxtTargetCountry.Text
                            && a.UserId == global_var.userId).OrderBy(x => x.SourceProductId).ToList();
                        
                        for (int i = 0; i < accountList.Count; i++)
                        {
                            for (int j = 0; j < dgSrcItemList.Rows.Count; j++)
                            {
                                long ItemId = Convert.ToInt64(dgSrcItemList.Rows[j].Cells["dgItemList_item_id"].Value.ToString());
                                if(accountList[i].SourceProductId == ItemId)
                                {
                                    dgSrcItemList.Rows[j].DefaultCellStyle.BackColor = Color.GreenYellow;
                                    break;
                                }
                            }
                        }

                        for (int i = 0; i < accountList.Count; i++)
                        {
                            for (int j = 0; j < dgTarItemList.Rows.Count; j++)
                            {
                                long ItemIdTar = Convert.ToInt64(dgTarItemList.Rows[j].Cells["dgTarItemList_item_id"].Value.ToString());
                                if (accountList[i].TargetProductId == ItemIdTar)
                                {
                                    dgTarItemList.Rows[j].DefaultCellStyle.BackColor = Color.GreenYellow;
                                    break;
                                }
                            }
                        }
                    }

                    Cursor.Current = Cursors.Default;

                    MessageBox.Show("대상 상품 정보를 모두 수신 하였습니다.", "상품 정보 수신", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
            catch
            {
                // ignore
            }
        }

        private dynamic getItemDetail(long item_id, long i_partner_id, long i_shop_id, string api_key)
        {
            //------------------------------------상품 목록 획득 ----------------------------------                
            long long_time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));
            string ep_item_info = "https://partner.shopeemobile.com/api/v1/item/get";

            Dictionary<string, object> dic_item_info = new Dictionary<string, object>();
            dic_item_info.Add("item_id", item_id);
            dic_item_info.Add("partner_id", i_partner_id);
            dic_item_info.Add("shopid", i_shop_id);
            dic_item_info.Add("timestamp", long_time_stamp);

            var request_item_info = new RestRequest("", Method.POST);
            request_item_info.Method = Method.POST;
            request_item_info.AddHeader("Accept", "application/json");
            request_item_info.AddJsonBody(new
            {
                item_id = item_id,
                partner_id = i_partner_id,
                shopid = i_shop_id,
                timestamp = long_time_stamp
            });
            request_item_info.AddHeader("authorization",
                HashString(ep_item_info + "|" + JsonConvert.SerializeObject(dic_item_info),
                api_key));

            var client_item_info = new RestClient(ep_item_info);
            IRestResponse response_item_info = client_item_info.Execute(request_item_info);
            dynamic result_item_info = JsonConvert.DeserializeObject(response_item_info.Content);
            if(result_item_info == null)
            {
                var fff = "";
            }
            return result_item_info;
        }

        private void get_shopee_logistic(string country, long partner_id, long shop_id, string api_key)
        {
            dg_shopee_logistics.Rows.Clear();            
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
                        for (int i = 0; i < result.logistics.Count; i++)
                        {
                            if (country.Contains("SG"))
                            {
                                if (result.logistics[i].logistic_name.ToString().Contains("Standard Express - Korea"))
                                {
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
                                    dg_shopee_logistics.Rows.Add(dg_shopee_logistics.Rows.Count + 1, false,
                                    result.logistics[i].logistic_name.ToString(),
                                    result.logistics[i].logistic_id.ToString(),
                                    result.logistics[i].has_cod.ToString(),
                                    result.logistics[i].fee_type.ToString());
                                }
                            }
                            else if (country.Contains("TH"))
                            {
                                if (result.logistics[i].logistic_name.ToString().Contains("Standard Express-Doora"))
                                {
                                    dg_shopee_logistics.Rows.Add(dg_shopee_logistics.Rows.Count + 1, false,
                                    result.logistics[i].logistic_name.ToString(),
                                    result.logistics[i].logistic_id.ToString(),
                                    result.logistics[i].has_cod.ToString(),
                                    result.logistics[i].fee_type.ToString());
                                }
                            }
                            else if (country.Contains("TW"))
                            {
                                if (result.logistics[i].logistic_name.ToString().Contains("YTO"))
                                {
                                    dg_shopee_logistics.Rows.Add(dg_shopee_logistics.Rows.Count + 1, false,
                                    result.logistics[i].logistic_name.ToString(),
                                    result.logistics[i].logistic_id.ToString(),
                                    result.logistics[i].has_cod.ToString(),
                                    result.logistics[i].fee_type.ToString());
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
        private void btnGetTarItemList_Click(object sender, EventArgs e)
        {
            if (TxtSourceId.Text.Trim() == string.Empty || TxtTargetId.Text.Trim() == string.Empty)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            btnGetSrcItemList.Enabled = false;
            grp_tar.Text = "대상 상품 목록";
            dgTarItemList.Rows.Clear();
            lstTarItemId.Clear();
            dicTarItemList.Clear();

            int ipagination_offset = 0;
            long src_partner_id = 0;
            long tar_partner_id = 0;

            long src_shop_id = 0;
            long tar_shop_id = 0;

            string src_api_key = "";
            string tar_api_key = "";

            for (int i = 0; i < dg_site_id.Rows.Count; i++)
            {
                if (dg_site_id.Rows[i].Cells["dg_site_id_id"].Value.ToString() == TxtSourceId.Text)
                {
                    src_partner_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_partner_id"].Value.ToString());
                    src_shop_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_shop_id"].Value.ToString());
                    src_api_key = dg_site_id.Rows[i].Cells["dg_site_id_secret_key"].Value.ToString();
                    break;
                }
            }

            for (int i = 0; i < dg_site_id.Rows.Count; i++)
            {
                if (dg_site_id.Rows[i].Cells["dg_site_id_id"].Value.ToString() == TxtTargetId.Text)
                {
                    tar_partner_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_partner_id"].Value.ToString());
                    tar_shop_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_shop_id"].Value.ToString());
                    tar_api_key = dg_site_id.Rows[i].Cells["dg_site_id_secret_key"].Value.ToString();
                    break;
                }
            }

            long update_time_from = ToUnixTime(dt_start.Value);
            long update_time_to = ToUnixTime(dt_end.Value.AddHours(-9));

            getTarItem(tar_partner_id, tar_shop_id, tar_api_key, ipagination_offset, update_time_from, update_time_to);
        }

        private void btn_link_product_Click(object sender, EventArgs e)
        {
            if(dgSrcItemList.SelectedRows.Count > 0 && dgTarItemList.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    long srcProductId = Convert.ToInt64(dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString());
                    long tarProductId = Convert.ToInt64(dgTarItemList.SelectedRows[0].Cells["dgTarItemList_item_id"].Value.ToString());
                    ProductLink result = context.ProductLinks.SingleOrDefault(
                        b => b.SourceCountry == TxtSourceCountry.Text &&
                        b.TargetCountry == TxtTargetCountry.Text &&
                        b.SourceProductId == srcProductId &&
                        b.TargetProductId == tarProductId);

                    if (result == null)
                    {
                        ProductLink newProductLink = new ProductLink
                        {
                            SourceCountry = TxtSourceCountry.Text,
                            TargetCountry = TxtTargetCountry.Text,
                            SourceProductId = srcProductId,
                            TargetProductId = tarProductId,
                            UserId = global_var.userId
                        };

                        context.ProductLinks.Add(newProductLink);
                        context.SaveChanges();

                        dgSrcItemList.Rows[dgSrcItemList.SelectedRows[0].Index].DefaultCellStyle.BackColor = Color.GreenYellow;
                        dgTarItemList.Rows[dgTarItemList.SelectedRows[0].Index].DefaultCellStyle.BackColor = Color.GreenYellow;

                        dgSrcItemList.Rows.RemoveAt(dgSrcItemList.SelectedRows[0].Index);
                        dgTarItemList.Rows.RemoveAt(dgTarItemList.SelectedRows[0].Index);
                    }
                    else
                    {
                        result.SourceCountry = TxtSourceCountry.Text;
                        result.TargetCountry = TxtTargetCountry.Text;
                        result.SourceProductId = srcProductId;
                        result.TargetProductId = tarProductId;
                        context.SaveChanges();
                    }
                }
            }            
        }

        private void dgSrcItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                if(e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6 || e.ColumnIndex == 7
                    || e.ColumnIndex == 8)
                {
                    string cellValue = dgSrcItemList.SelectedRows[0].Cells[e.ColumnIndex].Value.ToString().Trim();
                    if(cellValue != string.Empty)
                    {
                        Clipboard.SetText(cellValue);
                    }
                }
                using (AppDbContext context = new AppDbContext())
                {
                    long srcProductId = Convert.ToInt64(dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString());

                    ProductLink result = context.ProductLinks.SingleOrDefault(
                        b => b.SourceCountry == TxtSourceCountry.Text &&
                        b.TargetCountry == TxtTargetCountry.Text &&
                        b.SourceProductId == srcProductId);

                    if (result != null)
                    {
                        long tarProductId = result.TargetProductId;
                        for (int i = 0; i < dgTarItemList.Rows.Count; i++)
                        {
                            if (srcProductId == tarProductId)
                            {
                                dgTarItemList.Rows[i].Selected = true;
                                dgTarItemList.FirstDisplayedScrollingRowIndex = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        dgTarItemList.ClearSelection();
                    }
                }
            }     
            else if(e.RowIndex == -1 && e.ColumnIndex == 1)
            {
                if(dgSrcItemList.Rows.Count > 0)
                {
                    bool chk = (bool)dgSrcItemList.Rows[0].Cells["dgItemList_Chk"].Value;
                    for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                    {
                        dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value = !chk;
                    }

                    grp_src.Select();
                }
            }
        }

        private void btn_unlink_product_Click(object sender, EventArgs e)
        {
            if(dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
            {
                DialogResult Result = MessageBox.Show("상품 연결 정보를 삭제 하시겠습니까?", "상품 연결 정보 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        long srcProductId = Convert.ToInt64(dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString());

                        ProductLink result = context.ProductLinks.SingleOrDefault(
                            b => b.SourceCountry == TxtSourceCountry.Text &&
                            b.TargetCountry == TxtTargetCountry.Text &&
                            b.SourceProductId == srcProductId
                            && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            context.ProductLinks.Remove(result);
                            context.SaveChanges();

                            dgSrcItemList.Rows[dgSrcItemList.SelectedRows[0].Index].DefaultCellStyle.BackColor = Color.White;
                            dgTarItemList.Rows[dgTarItemList.SelectedRows[0].Index].DefaultCellStyle.BackColor = Color.White;
                        }
                    }
                }
            }
        }

        private void dgTarItemList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_link_product_Click(null, null);
        }

        private void btn_remove_link_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count > 0)
            {
                for (int i = dgSrcItemList.Rows.Count - 1; i > 0; i--)
                {
                    if (dgSrcItemList.Rows[i].DefaultCellStyle.BackColor == Color.GreenYellow)
                    {
                        dgSrcItemList.Rows.RemoveAt(i);
                    }
                }

                grp_src.Text = "상품 목록 : [ " + string.Format("{0:n0}", dgSrcItemList.Rows.Count) + " ]";

                for (int i = dgTarItemList.Rows.Count - 1; i > 0; i--)
                {
                    if (dgTarItemList.Rows[i].DefaultCellStyle.BackColor == Color.GreenYellow)
                    {
                        dgTarItemList.Rows.RemoveAt(i);
                    }
                }

                grp_tar.Text = "상품 목록 : [ " + string.Format("{0:n0}", dgTarItemList.Rows.Count) + " ]";
            }   
        }

        private void dgTarItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if(e.RowIndex > -1)
            //{
            //    using (AppDbContext context = new AppDbContext())
            //    {
            //        string tarProductId = dgTarItemList.SelectedRows[0].Cells["dgTarItemList_item_id"].Value.ToString();

            //        ProductLink result = context.ProductLinks.SingleOrDefault(
            //            b => b.SourceCountry == TxtSourceCountry.Text &&
            //            b.TargetCountry == TxtTargetCountry.Text &&
            //            b.TargetProductId == tarProductId);

            //        if (result != null)
            //        {
            //            string srcProductId = result.SourceProductId;
            //            for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
            //            {
            //                if (dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString() == srcProductId)
            //                {
            //                    dgSrcItemList.Rows[i].Selected = true;
            //                    dgSrcItemList.FirstDisplayedScrollingRowIndex = i;
            //                    break;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            dgSrcItemList.ClearSelection();
            //        }
            //    }
            //}            
        }

        private void btn_upload_product_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count > 0)
            {
                long src_partner_id = 0;
                long tar_partner_id = 0;

                long src_shop_id = 0;
                long tar_shop_id = 0;

                string src_api_key = "";
                string tar_api_key = "";

                string end_point = "https://partner.shopeemobile.com/api/v1/item/add";
                for (int i = 0; i < dg_site_id.Rows.Count; i++)
                {
                    if (dg_site_id.Rows[i].Cells["dg_site_id_id"].Value.ToString() == TxtTargetId.Text)
                    {
                        tar_partner_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_partner_id"].Value.ToString());
                        tar_shop_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_shop_id"].Value.ToString());
                        tar_api_key = dg_site_id.Rows[i].Cells["dg_site_id_secret_key"].Value.ToString();
                        break;
                    }
                }

                string tar_shopee_account = TxtTargetId.Text.Trim();
                if (tar_shopee_account == string.Empty)
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
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        string tarCategoryId = dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value.ToString();
                        string strCalcPrice = dgSrcItemList.Rows[i].Cells["dgItemList_tar_price"].Value.ToString().Trim().Replace(",", "");

                        if (tarCategoryId == string.Empty)
                        {
                            dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orange;
                        }

                        if (strCalcPrice == string.Empty)
                        {
                            dgSrcItemList.Rows[i].Cells["dgItemList_tar_price"].Style.BackColor = Color.Orange;
                        }

                        if (tarCategoryId != string.Empty && strCalcPrice != string.Empty)
                        {
                            bool shopee_set_preorder = false;
                            string sell_title = dgSrcItemList.Rows[i].Cells["dgItemList_name"].Value.ToString().Trim();
                            string itemSku = dgSrcItemList.Rows[i].Cells["dgItemList_item_sku"].Value.ToString().Trim();
                            int shopee_set_preorder_days = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_days_to_ship"].Value.ToString().Trim());


                            string strHeader = FullTemplateHeader(tar_shopee_account);
                            string strFooter = FullTemplateFooter(tar_shopee_account); ;

                            string description = dgSrcItemList.Rows[i].Cells["dgItemList_description"].Value.ToString().Trim();
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
                            sbNewDesc.Append(strHeader);
                            sbNewDesc.Append(strHeaderSeparator);
                            sbNewDesc.Append(extractDesc);
                            sbNewDesc.Append(strFooterSeparator + "\r\n");
                            sbNewDesc.Append(strFooter);

                            decimal calcPrice = Convert.ToDecimal(dgSrcItemList.Rows[i].Cells["dgItemList_tar_price"].Value.ToString().Trim().Replace(",", ""));
                            string compair_price = dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Value.ToString().Trim().Replace(",", "");
                            string qty = dgSrcItemList.Rows[i].Cells["dgItemList_stock"].Value.ToString().Trim().Replace(",", "");
                            string strSku = dgSrcItemList.Rows[i].Cells["dgItemList_item_sku"].Value.ToString().Trim();
                            int strWeight = Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgItemList_weight"].Value.ToString().Trim().Replace(",", ""));

                            double iWeight = strWeight * 0.001;

                            if (iWeight < 0.1)
                            {
                                iWeight = 0.1;
                            }

                            string strImage = dgSrcItemList.Rows[i].Cells["dgItemList_images"].Value.ToString().Trim();
                            dynamic dynImage = JsonConvert.DeserializeObject(strImage);

                            List<shopee_image> lst_img = new List<shopee_image>();
                            for (int img = 0; img < dynImage.Count; img++)
                            {
                                shopee_image s_image = new shopee_image { url = dynImage[img].ToString().Trim() };
                                lst_img.Add(s_image);
                            }

                            string strVariation = dgSrcItemList.Rows[i].Cells["dgItemList_variations"].Value.ToString().Trim();
                            dynamic dynVariation = JsonConvert.DeserializeObject(strVariation);

                            List<shopee_variations> lst_vari = new List<shopee_variations>();
                            if (dynVariation != null && dynVariation.Count > 0)
                            {
                                //Variation이 있는 경우
                                //진짜 2티어인지 검증한다. 모든 옵션에 콤마가 있으면 2티어임.
                                bool isReal2Tier = true;
                                bool isSamePrice = true;

                                var initPrice = dynVariation[0].price;

                                for (int tier = 0; tier < dynVariation.Count; tier++)
                                {
                                    if (!dynVariation[tier].name.ToString().Contains(","))
                                    {
                                        isReal2Tier = false;
                                        break;
                                    }
                                }

                                for (int tier = 0; tier < dynVariation.Count; tier++)
                                {
                                    if (initPrice != dynVariation[tier].price)
                                    {
                                        isSamePrice = false;
                                        break;
                                    }
                                }

                                //진짜 2티어가 아닌경우와 옵션 가격이 동일한 경우 단순 등록한다.
                                if (!isReal2Tier && isSamePrice)
                                {
                                    for (int vari = 0; vari < dynVariation.Count; vari++)
                                    {
                                        using (shopee_variations vars = new shopee_variations())
                                        {
                                            vars.name = dynVariation[vari].name.ToString();
                                            vars.stock = dynVariation[vari].stock;

                                            //현지 판가로 계산하여야 한다.
                                            vars.price = calcPrice;
                                            vars.variation_sku = dynVariation[vari].variation_sku;
                                            lst_vari.Add(vars);
                                        }
                                    }
                                }
                                else
                                {

                                }
                            }
                            long srcProductId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());

                            List<shopee_attribute> lst_attr = new List<shopee_attribute>();
                            //설정한 속성값을 가지고 온다
                            using (AppDbContext context = new AppDbContext())
                            {
                                List<ProductAttribute> attributeList = context.ProductAttributes
                                .Where(x => x.srcProductId == srcProductId
                                && x.tarShopeeAccount == TxtTargetId.Text.Trim()
                                && x.UserId == global_var.userId)
                                .OrderBy(x => x.tarAttributeId).ToList();

                                if (attributeList != null && attributeList.Count > 0)
                                {
                                    for (int j = 0; j < attributeList.Count; j++)
                                    {
                                        shopee_attribute att = new shopee_attribute
                                        {
                                            attributes_id = attributeList[j].tarAttributeId,
                                            value = attributeList[j].tarAttributeValue
                                        };

                                        lst_attr.Add(att);
                                    }
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

                            var request = new RestRequest("", RestSharp.Method.POST);
                            request.Method = Method.POST;
                            request.AddHeader("Accept", "application/json");

                            long long_time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));

                            string convert_price = string.Format("{0:0.##}", compair_price);
                            int int_val = 0;
                            int int_val_original = 0;

                            bool is_int = int.TryParse(convert_price, out int_val);

                            Dictionary<string, object> dic_json = new Dictionary<string, object>();
                            dic_json.Add("category_id", Convert.ToInt32(tarCategoryId));
                            dic_json.Add("name", sell_title);
                            dic_json.Add("description", description);

                            if (is_int)
                            {
                                dic_json.Add("price", int_val);
                            }
                            else
                            {
                                dic_json.Add("price", Convert.ToDouble(compair_price));
                            }

                            dic_json.Add("stock", Convert.ToInt32(qty));
                            dic_json.Add("item_sku", itemSku);
                            dic_json.Add("images", obj_image);
                            dic_json.Add("variations", obj_vari);
                            dic_json.Add("attributes", obj_attr);
                            dic_json.Add("logistics", obj_logi);
                            dic_json.Add("weight", iWeight);
                            if (shopee_set_preorder == true && shopee_set_preorder_days > 6)
                            {
                                dic_json.Add("days_to_ship", shopee_set_preorder_days);
                            }
                            dic_json.Add("partner_id", tar_partner_id);
                            dic_json.Add("shopid", tar_shop_id);
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
                                tar_api_key));

                            var client = new RestClient(end_point);
                            IRestResponse response = client.Execute(request);
                            var content = response.Content;

                        }
                    }
                }

                Cursor.Current = Cursors.Default;
            }   
        }

        private void btn_validate_category_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                string srcCountry = TxtSourceCountry.Text.Trim();
                string tarCountry = TxtTargetCountry.Text.Trim();

                
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    bool isValidCategory = false;
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
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
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = categoryList[0].TarCategoryId;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                            }
                        }

                        //DB에서 못찾은 경우
                        if(!isValidCategory)
                        {
                            //전체 카테고리DB에서 가지고 온다.
                            using (AppDbContext context = new AppDbContext())
                            {
                                if (TxtSourceCountry.Text == "ID")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories.Where(x => x.cat_id == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (TxtTargetCountry.Text == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;

                                        //새로 저장해 준다.
                                        CustomCategoryData newCustomCategory = new CustomCategoryData
                                        {
                                            SrcShopeeCountry = srcCountry,
                                            TarShopeeCountry = tarCountry,
                                            SrcCategoryId = srcCategoryId,
                                            TarCategoryId = tarCategoryId                                            ,
                                            UserId = global_var.userId
                                        };
                                        context.CustomCategoryDatas.Add(newCustomCategory);
                                        context.SaveChanges();
                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (TxtTargetCountry.Text == "MY")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;

                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;

                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;

                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;

                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orange;
                                    }
                                }
                                else if (TxtSourceCountry.Text == "MY")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories.Where(x => x.cat_my == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (TxtTargetCountry.Text == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "ID")
                                        {
                                            tarCategoryId = categoryList[0].cat_id.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;

                                        //새로 저장해 준다.
                                        CustomCategoryData newCustomCategory = new CustomCategoryData
                                        {
                                            SrcShopeeCountry = srcCountry,
                                            TarShopeeCountry = tarCountry,
                                            SrcCategoryId = srcCategoryId,
                                            TarCategoryId = tarCategoryId,
                                            UserId = global_var.userId
                                        };
                                        context.CustomCategoryDatas.Add(newCustomCategory);
                                        context.SaveChanges();
                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (TxtTargetCountry.Text == "MY")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        if (TxtTargetCountry.Text == "ID")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orange;
                                    }
                                }
                                else if (TxtSourceCountry.Text == "Ph")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories.Where(x => x.cat_ph == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (TxtTargetCountry.Text == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "ID")
                                        {
                                            tarCategoryId = categoryList[0].cat_id.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                        //새로 저장해 준다.
                                        CustomCategoryData newCustomCategory = new CustomCategoryData
                                        {
                                            SrcShopeeCountry = srcCountry,
                                            TarShopeeCountry = tarCountry,
                                            SrcCategoryId = srcCategoryId,
                                            TarCategoryId = tarCategoryId,
                                            UserId = global_var.userId
                                        };
                                        context.CustomCategoryDatas.Add(newCustomCategory);
                                        context.SaveChanges();
                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (TxtTargetCountry.Text == "MY")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        if (TxtTargetCountry.Text == "ID")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orange;
                                    }
                                }
                                else if (TxtSourceCountry.Text == "SG")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories.Where(x => x.cat_sg == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (TxtTargetCountry.Text == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "ID")
                                        {
                                            tarCategoryId = categoryList[0].cat_id.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;

                                        //새로 저장해 준다.
                                        CustomCategoryData newCustomCategory = new CustomCategoryData
                                        {
                                            SrcShopeeCountry = srcCountry,
                                            TarShopeeCountry = tarCountry,
                                            SrcCategoryId = srcCategoryId,
                                            TarCategoryId = tarCategoryId,
                                            UserId = global_var.userId
                                        };
                                        context.CustomCategoryDatas.Add(newCustomCategory);
                                        context.SaveChanges();
                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (TxtTargetCountry.Text == "MY")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        if (TxtTargetCountry.Text == "ID")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orange;
                                    }
                                }
                                else if (TxtSourceCountry.Text == "TW")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories.Where(x => x.cat_tw == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (TxtTargetCountry.Text == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "ID")
                                        {
                                            tarCategoryId = categoryList[0].cat_id.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                        //새로 저장해 준다.
                                        CustomCategoryData newCustomCategory = new CustomCategoryData
                                        {
                                            SrcShopeeCountry = srcCountry,
                                            TarShopeeCountry = tarCountry,
                                            SrcCategoryId = srcCategoryId,
                                            TarCategoryId = tarCategoryId,
                                            UserId = global_var.userId
                                        };
                                        context.CustomCategoryDatas.Add(newCustomCategory);
                                        context.SaveChanges();
                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (TxtTargetCountry.Text == "MY")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        if (TxtTargetCountry.Text == "ID")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orange;
                                    }
                                }
                                else if (TxtSourceCountry.Text == "TH")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories.Where(x => x.cat_th == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (TxtTargetCountry.Text == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "ID")
                                        {
                                            tarCategoryId = categoryList[0].cat_id.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;

                                        //새로 저장해 준다.
                                        CustomCategoryData newCustomCategory = new CustomCategoryData
                                        {
                                            SrcShopeeCountry = srcCountry,
                                            TarShopeeCountry = tarCountry,
                                            SrcCategoryId = srcCategoryId,
                                            TarCategoryId = tarCategoryId,
                                            UserId = global_var.userId
                                        };
                                        context.CustomCategoryDatas.Add(newCustomCategory);
                                        context.SaveChanges();
                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (TxtTargetCountry.Text == "MY")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        if (TxtTargetCountry.Text == "ID")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orange;
                                    }
                                }
                                else if (TxtSourceCountry.Text == "VN")
                                {
                                    List<ShopeeCategory> categoryList = context.ShopeeCategories.Where(x => x.cat_vn == srcCategoryId).ToList();
                                    if (categoryList.Count == 1)
                                    {
                                        if (TxtTargetCountry.Text == "MY")
                                        {
                                            tarCategoryId = categoryList[0].cat_my.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "ID")
                                        {
                                            tarCategoryId = categoryList[0].cat_id.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
                                        {
                                            tarCategoryId = categoryList[0].cat_ph.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
                                        {
                                            tarCategoryId = categoryList[0].cat_sg.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
                                        {
                                            tarCategoryId = categoryList[0].cat_th.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
                                        {
                                            tarCategoryId = categoryList[0].cat_tw.ToString();
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
                                        {
                                            tarCategoryId = categoryList[0].cat_vn.ToString();
                                        }

                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                        //새로 저장해 준다.
                                        CustomCategoryData newCustomCategory = new CustomCategoryData
                                        {
                                            SrcShopeeCountry = srcCountry,
                                            TarShopeeCountry = tarCountry,
                                            SrcCategoryId = srcCategoryId,
                                            TarCategoryId = tarCategoryId,
                                            UserId = global_var.userId
                                        };
                                        context.CustomCategoryDatas.Add(newCustomCategory);
                                        context.SaveChanges();

                                    }
                                    else if (categoryList.Count > 1)
                                    {
                                        //1 이상인 경우 타겟 카테고리 ID가 모두 동일하면 1개로 간주한다.

                                        if (TxtTargetCountry.Text == "MY")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        if (TxtTargetCountry.Text == "ID")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "PH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "SG")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TH")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "TW")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                        else if (TxtTargetCountry.Text == "VN")
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
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Value = tarCategoryId;
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                                                //새로 저장해 준다.
                                                CustomCategoryData newCustomCategory = new CustomCategoryData
                                                {
                                                    SrcShopeeCountry = srcCountry,
                                                    TarShopeeCountry = tarCountry,
                                                    SrcCategoryId = srcCategoryId,
                                                    TarCategoryId = tarCategoryId,
                                                    UserId = global_var.userId
                                                };
                                                context.CustomCategoryDatas.Add(newCustomCategory);
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orchid;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_category"].Style.BackColor = Color.Orange;
                                    }
                                }
                            }
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
                MessageBox.Show("카테고리 검증을 완료 하였습니다.", "카테고리 검증 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }   
        }
        
        public string TempCategoryId = "";
        private void btn_set_category_Click(object sender, EventArgs e)
        {
            if(dgSrcItemList.SelectedRows.Count > 0)
            {
                TempCategoryId = "";
                FormCustomCategory fcs = new FormCustomCategory();
                fcs.fp = this;
                fcs.TxtSrcCountry.Text = TxtSourceCountry.Text.Trim();
                fcs.TxtTarCountry.Text = TxtTargetCountry.Text.Trim();
                fcs.TxtSrcCategoryId.Text = dgSrcItemList.SelectedRows[0].Cells["dgItemList_category_id"].Value.ToString();
                fcs.txtSrcTitle.Text = dgSrcItemList.SelectedRows[0].Cells["dgItemList_name"].Value.ToString();
                fcs.ShowDialog();

                if(TempCategoryId != string.Empty)
                {
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_tar_category"].Value = TempCategoryId;
                    dgSrcItemList.SelectedRows[0].Cells["dgItemList_tar_category"].Style.BackColor = Color.GreenYellow;
                    dgSrcItemList.EndEdit();
                    grp_src.Select();
                }
            }
        }
        private void Fill_to_Currency_Names()
        {
            cls_currency cc = new cls_currency();
            Dictionary<string, decimal> dic_currency = new Dictionary<string, decimal>();
            dic_currency = cc.get_currency();
            if (dic_currency.Count > 0)
            {
                cbo_currency_to.DataSource = new BindingSource(dic_currency, null);
                cbo_currency_to.DisplayMember = "Key";
                cbo_currency_to.ValueMember = "Value";
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
                cbo_currency_to.DataSource = null;
                Fill_to_Currency_Names();
                MessageBox.Show("환율 정보를 업데이트 하였습니다.", "환율정보 업데이트", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void dg_site_id_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dg_site_id_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnApplyPrice_Click(object sender, EventArgs e)
        {
            //환율 기본 계산 방법
            //원본 판가를 가지고 온 후 원화로 계산한다
            //계산된 원화를 대상 환율로 계산한다.
            //우리가 받아야 할 금액이 원화 이므로 둘다 원화 기준으로 하는 것이다.
            //네이버 환율 계산기도 이런 방식임
            //한국 원화 환율 기준이므로 그냥 환율로 곱해준다.


            //원본환율을 가지고 온다.
            KeyValuePair<string, decimal> currencySrc_info = (KeyValuePair<string, decimal>)cbo_currency_From.SelectedItem;
            string strSrcRate = currencySrc_info.Value.ToString();
            decimal rateSrc = 0;
            decimal.TryParse(strSrcRate, out rateSrc);

            //대상환율을 가지고 온다.
            KeyValuePair<string, decimal> currencyTar_info = (KeyValuePair<string, decimal>)cbo_currency_to.SelectedItem;
            string strTarRate = currencyTar_info.Value.ToString();
            decimal rateTar = 0;
            decimal.TryParse(strTarRate, out rateTar);

            for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
            {
                if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                {
                    //1.원본판가의 대상 판가 계산
                    decimal SrcPrice = Convert.ToDecimal(dgSrcItemList.Rows[i].Cells["dgItemList_price"].Value.ToString());
                    //원화로 변경한다.
                    decimal transKRW = 0;
                    //일본엔화, 인도네시아-루피, 베트남동 등은 100으로 나누어 준다.
                    if (currencySrc_info.Key.Contains("JPN") ||
                        currencySrc_info.Key.Contains("IDR") ||
                        currencySrc_info.Key.Contains("VND"))
                    {
                        transKRW = SrcPrice * rateSrc / 100;
                    }
                    else
                    {
                        transKRW = SrcPrice / rateSrc;
                    }

                    decimal TarPrice = 0;
                    //변경한 원화를 대상 환율로 다시 변경
                    if (currencyTar_info.Key.Contains("JPN") ||
                        currencyTar_info.Key.Contains("IDR") ||
                        currencyTar_info.Key.Contains("VND"))
                    {
                        TarPrice = transKRW / rateTar * 100;
                    }
                    else
                    {
                        TarPrice = transKRW / rateTar;
                    }

                    if(RdPrice.Checked)
                    {
                        if(cboPriceOp.Text == "+")
                        {
                            TarPrice = TarPrice + UdPriceValue.Value;
                        }
                        else
                        {
                            TarPrice = TarPrice - UdPriceValue.Value;
                        }
                    }
                    else if (RdRate.Checked)
                    {
                        if (cboRateOp.Text == "+")
                        {
                            TarPrice = TarPrice * (1 + UdPriceValue.Value / 100);
                        }
                        else
                        {
                            TarPrice = TarPrice * (1 - UdPriceValue.Value / 100);
                        }
                    }

                    //소수점이 필요하지 않은 통화
                    if (currencyTar_info.Key.Contains("JPN") ||
                        currencyTar_info.Key.Contains("TWD") ||
                        currencyTar_info.Key.Contains("VND") ||
                        currencyTar_info.Key.Contains("THB") ||
                        currencyTar_info.Key.Contains("PHP"))
                    {
                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_price"].Value = Math.Truncate(TarPrice);
                    }
                    else if (currencyTar_info.Key.Contains("IDR"))
                    {
                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_price"].Value = Math.Truncate(TarPrice);
                    }
                    else if(currencyTar_info.Key.Contains("MYR"))
                    {
                        //소수점 2자리 표시
                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_price"].Value = Math.Round(TarPrice, 2);
                    }
                    else if(currencyTar_info.Key.Contains("SGD"))
                    {
                        //소수점 1자리 표시
                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_price"].Value = Math.Round(TarPrice, 1);
                    }


                    //1.원본소비자가의 대상소비자가 계산
                    decimal SrcRetailPrice = Convert.ToDecimal(dgSrcItemList.Rows[i].Cells["dgItemList_original_price"].Value.ToString());
                    //원화로 변경한다.
                    decimal transRetailKRW = 0;
                    //일본엔화, 인도네시아-루피, 베트남동 등은 100으로 나누어 준다.
                    if (currencySrc_info.Key.Contains("JPN") ||
                        currencySrc_info.Key.Contains("IDR") ||
                        currencySrc_info.Key.Contains("VND"))
                    {
                        transRetailKRW = SrcRetailPrice * rateSrc / 100;
                    }
                    else
                    {
                        transRetailKRW = SrcRetailPrice / rateSrc;
                    }

                    decimal TarRetailPrice = 0;
                    //변경한 원화를 대상 환율로 다시 변경
                    if (currencyTar_info.Key.Contains("JPN") ||
                        currencyTar_info.Key.Contains("IDR") ||
                        currencyTar_info.Key.Contains("VND"))
                    {
                        TarRetailPrice = transRetailKRW / rateTar * 100;
                    }
                    else
                    {
                        TarRetailPrice = transRetailKRW / rateTar;
                    }

                    if (RdPrice.Checked)
                    {
                        if (cboPriceOp.Text == "+")
                        {
                            TarRetailPrice = TarRetailPrice + UdPriceValue.Value;
                        }
                        else
                        {
                            TarRetailPrice = TarRetailPrice - UdPriceValue.Value;
                        }
                    }
                    else if (RdRate.Checked)
                    {
                        if (cboRateOp.Text == "+")
                        {
                            TarRetailPrice = TarRetailPrice * (1 + UdPriceValue.Value / 100);
                        }
                        else
                        {
                            TarRetailPrice = TarRetailPrice * (1 - UdPriceValue.Value / 100);
                        }
                    }


                    //소수점이 필요하지 않은 통화
                    if (currencyTar_info.Key.Contains("JPN") ||
                        currencyTar_info.Key.Contains("TWD") ||
                        currencyTar_info.Key.Contains("VND") ||
                        currencyTar_info.Key.Contains("THB") ||
                        currencyTar_info.Key.Contains("PHP"))
                    {
                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Value = Math.Truncate(TarRetailPrice);
                    }
                    else if (currencyTar_info.Key.Contains("IDR"))
                    {
                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Value = Math.Truncate(TarRetailPrice);
                    }
                    else if (currencyTar_info.Key.Contains("MYR"))
                    {
                        //소수점 2자리 표시
                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Value = Math.Round(TarRetailPrice, 2);
                    }
                    else if (currencyTar_info.Key.Contains("SGD"))
                    {
                        //소수점 1자리 표시
                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Value = Math.Round(TarRetailPrice, 1);
                    }
                }
            }
        }

        private void BtnApplyQty_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
            {
                if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                {
                    dgSrcItemList.Rows[i].Cells["dgItemList_tar_stock"].Value = UdQty.Value;
                    dgSrcItemList.Rows[i].Cells["dgItemList_tar_stock"].Style.BackColor = Color.GreenYellow;
                }
            }
        }

        private void dgSrcItemList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 2)
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
            else if(e.ColumnIndex == 26)
            {
                btn_set_category_Click(null, null);
            }
            else if (e.ColumnIndex == 27)
            {
                BtnSetAttribute_Click(null, null);
            }
            else if (e.ColumnIndex == 45)
            {
                btn_set_variation_Click(null, null);
            }
        }

        private void viewNaverExchange_Click(object sender, EventArgs e)
        {
            string siteUrl = "https://finance.naver.com/marketindex/?tabSel=exchange#tab_section";
            System.Diagnostics.Process.Start("chrome.exe", siteUrl);
        }

        private void BtnCalcSellPrice_Click(object sender, EventArgs e)
        {
            
            //정산가로 부터 SLS의 경우 수수료율 리베이트 등을 계산하여 올린다.
            for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
            {
                if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                {
                    //정산가
                    string TarNetPrice = dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Value.ToString().Trim().Replace(",", "");
                    string strWeight = dgSrcItemList.Rows[i].Cells["dgItemList_weight"].Value.ToString().Trim().Replace(",", "");

                    if(TarNetPrice == string.Empty)
                    {
                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Style.BackColor = Color.Orange;
                    }
                    else
                    {
                        dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Style.BackColor = Color.GreenYellow;
                    }

                    if (strWeight == string.Empty)
                    {
                        dgSrcItemList.Rows[i].Cells["dgItemList_weight"].Style.BackColor = Color.Orange;
                    }
                    else
                    {
                        dgSrcItemList.Rows[i].Cells["dgItemList_weight"].Style.BackColor = Color.GreenYellow;
                    }

                    if (TarNetPrice != string.Empty && strWeight != string.Empty)
                    {
                        decimal d_TarNetPrice = Convert.ToDecimal(dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Value.ToString().Trim());
                        int iWeight = Convert.ToInt32(strWeight);
                        //해당 무게에 따른 배송비를 찾아온다.
                        decimal shippingFee = 0;

                        if(TxtTargetCountry.Text == "SG")
                        {
                            using (AppDbContext context = new AppDbContext())
                            {
                                //일의 자리에서 무조건 올린다.
                                iWeight = (iWeight + 9) / 10 * 10;
                                List<ShippingRateSlsSg> rateList = context.ShippingRateSlsSgs
                                    .Where(a => a.Weight == iWeight)
                                    .OrderByDescending(x => x.Weight).ToList();

                                if(rateList.Count > 0)
                                {
                                    shippingFee = rateList[0].ShippingFeeAvg;
                                }

                                //수수료 계산
                                //(정산가 + 해외배송비 - 1.99) * 2 %
                                var PGFee = (d_TarNetPrice + shippingFee - 1.99m) * (udSellingFee.Value / 100);
                                var SellPrice = d_TarNetPrice + shippingFee + PGFee - 1.99m;
                                var SellRetailPrice = SellPrice * (1 + UdRetailPriceRate.Value / 100);
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_price"].Value = Math.Round(SellPrice, 1);
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Value = Math.Round(SellRetailPrice, 1);

                                //대상수량
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_stock"].Value = dgSrcItemList.Rows[i].Cells["dgItemList_stock"].Value.ToString();

                                if (SellPrice > 0)
                                {
                                    dgSrcItemList.Rows[i].Cells["dgItemList_tar_price"].Style.BackColor = Color.GreenYellow;
                                    dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Style.BackColor = Color.GreenYellow;
                                    dgSrcItemList.Rows[i].Cells["dgItemList_tar_stock"].Style.BackColor = Color.GreenYellow;
                                }
                            }
                        }
                        else if(TxtTargetCountry.Text == "ID")
                        {

                        }
                    }
                }
            }

            MessageBox.Show("판매가 및 소비자가를 계산 완료 하였습니다.","가격 자동 계산",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void dgSrcItemList_SelectionChanged(object sender, EventArgs e)
        {
            Color color = dgSrcItemList.CurrentRow.Cells["dgItemList_tar_price"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_price"].Style.SelectionBackColor = color;
            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_price"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_net_price"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_tar_net_price"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_net_price"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_category"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_tar_category"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_category"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_attribute_valid"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_tar_attribute_valid"].Style.BackColor;

            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_price"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_tar_price"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_price"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_original_price"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_tar_original_price"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_original_price"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_stock"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_tar_stock"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_stock"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_stock"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_stock"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_stock"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_weight"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_weight"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_weight"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_is_validate_variation"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_is_validate_variation"].Style.BackColor;

            dgSrcItemList.CurrentRow.Cells["dgItemList_status"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_status"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_status"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_original_price"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_original_price"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_original_price"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_price"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_price"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_price"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_weight"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_tar_weight"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_tar_weight"].Style.SelectionForeColor = Color.Blue;
            
            dgSrcItemList.CurrentRow.Cells["dgItemList_isFreeShipping"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_isFreeShipping"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_isFreeShipping"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_shipping_fee"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_shipping_fee"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_shipping_fee"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_category_id"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_category_id"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_category_id"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_discount_id"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_discount_id"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_discount_id"].Style.SelectionForeColor = Color.Blue;

            dgSrcItemList.CurrentRow.Cells["dgItemList_days_to_ship"].Style.SelectionBackColor = dgSrcItemList.CurrentRow.Cells["dgItemList_days_to_ship"].Style.BackColor;
            dgSrcItemList.CurrentRow.Cells["dgItemList_days_to_ship"].Style.SelectionForeColor = Color.Blue;

        }

        private void LblRetailPriceRate_MouseHover(object sender, EventArgs e)
        {
            ToolTip_LblRetailPriceRate.IsBalloon = true;
            ToolTip_LblRetailPriceRate.SetToolTip(LblRetailPriceRate, "소비자가 비율은 대상판매가의 추가 비율입니다.");
        }

        private void btn_hf_category_add_Click(object sender, EventArgs e)
        {
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

        private void btn_hf_category_delete_Click(object sender, EventArgs e)
        {
            string shopeeAccount = TxtTargetId.Text.Trim();
            if(shopeeAccount != string.Empty)
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
                            HFType result = context.HFTypes.SingleOrDefault(b => b.HFTypeID == HFTypeID && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                context.HFTypes.Remove(result);
                                context.SaveChanges();
                            }
                        }

                        FillHfType();
                        FillTemplateHeader(shopeeAccount);
                        FillTemplateFooter(shopeeAccount);
                    }
                }
            }
        }

        private void btn_hf_category_edit_Click(object sender, EventArgs e)
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
                               b => b.HFTypeID == HFTypeID && b.UserId == global_var.userId);

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

        private void btn_html_set_Click(object sender, EventArgs e)
        {
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

        private void dg_hf_category_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dg_hf_category.SelectedRows.Count > 0)
            {
                txt_template_name.Text = "";
                txt_hf_category_name.Text = dg_hf_category.SelectedRows[0].Cells["dg_hf_category_name"].Value.ToString();
                int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());
                FillHfList(HFTypeID);
            }
        }

        private void btn_rename_template_name_Click(object sender, EventArgs e)
        {
            string shopeeAccount = TxtTargetId.Text.Trim();

            if(shopeeAccount != string.Empty)
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
                                b => b.HFListID == HFListID && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                result.HFName = txt_template_name.Text.Trim();
                                context.SaveChanges();
                            }

                            FillHfList(HFTypeID);
                            FillTemplateHeader(shopeeAccount);
                        }
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
                        b => b.HFListID == HFListID && b.UserId == global_var.userId);

                    TxtHFContent.Text = result.HFContent;
                }
            }
        }

        private void btn_delete_template_Click(object sender, EventArgs e)
        {
            string shopeeAccount = TxtTargetId.Text.Trim();
            if(shopeeAccount != string.Empty)
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
                            HFList result = context.HFLists.SingleOrDefault(b => b.HFListID == HFListID && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                context.HFLists.Remove(result);
                                context.SaveChanges();
                            }
                        }
                        txt_template_name.Text = "";
                        FillHfList(HFTypeID);
                        FillTemplateHeader(shopeeAccount);
                        FillTemplateFooter(shopeeAccount);

                    }
                }
            }
        }

        private void btn_add_header_Click(object sender, EventArgs e)
        {
            if(TxtTargetId.Text.Trim() == string.Empty)
            {
                MessageBox.Show("복사 대상국가 및 계정을 설정하세요.","복사 대상국가 및 계정 미설정", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            DialogResult Result = MessageBox.Show("헤더에 추가 하시겠습니까?", "헤더추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int HFListID = Convert.ToInt32(dg_hf_list.SelectedRows[0].Cells["dg_hf_list_idx"].Value.ToString());
                    int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());
                    string HfName = dg_hf_list.SelectedRows[0].Cells["dg_hf_list_template_name"].Value.ToString();
                    string shopeeAccount = TxtTargetId.Text.Trim();
                    TemplateHeader newHeader = new TemplateHeader
                    {
                        HFListID = HFListID,
                        ShopeeAccount = shopeeAccount,
                        HFName = HfName,
                        OrderIdx = dg_list_header.Rows.Count + 1,
                        UserId = global_var.userId
                    };

                    context.TemplateHeaders.Add(newHeader);
                    context.SaveChanges();

                    FillTemplateHeader(TxtTargetId.Text.Trim());
                }
            }
        }

        private void TabMain_Header_Footer_Click(object sender, EventArgs e)
        {

        }

        private void btn_add_footer_Click(object sender, EventArgs e)
        {
            if (TxtTargetId.Text.Trim() == string.Empty)
            {
                MessageBox.Show("복사 대상국가 및 계정을 설정하세요.", "복사 대상국가 및 계정 미설정", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string shopeeAccount = TxtTargetId.Text.Trim();

            if (shopeeAccount != string.Empty)
            {
                DialogResult Result = MessageBox.Show("푸터에 추가 하시겠습니까?", "푸터추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        int HFListID = Convert.ToInt32(dg_hf_list.SelectedRows[0].Cells["dg_hf_list_idx"].Value.ToString());
                        int HFTypeID = Convert.ToInt32(dg_hf_category.SelectedRows[0].Cells["dg_hf_category_idx"].Value.ToString());
                        string HfName = dg_hf_list.SelectedRows[0].Cells["dg_hf_list_template_name"].Value.ToString();

                        TemplateFooter newFooter = new TemplateFooter
                        {
                            HFListID = HFListID,
                            ShopeeAccount = shopeeAccount,
                            HFName = HfName,
                            OrderIdx = dg_list_footer.Rows.Count + 1,
                            UserId = global_var.userId
                        };

                        context.TemplateFooters.Add(newFooter);
                        context.SaveChanges();

                        FillTemplateFooter(shopeeAccount);
                    }
                }
            }
        }

        private void BtnSaveHFSeparator_Click(object sender, EventArgs e)
        {
            if(TxtHeaderSeparator.Text.Trim() == TxtFooterSeparator.Text.Trim())
            {
                MessageBox.Show("헤더 구분자와 푸터 구분자가 서로 동일합니다.\r\n글자수를 다르게 하거나 다른 글자를 삽입해 주세요.","구분자 동일", MessageBoxButtons.OK,MessageBoxIcon.Error);
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

                        if(result == null || result.Count == 0)
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
                                    b => b.Id == idx && b.UserId == global_var.userId);

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
                            TemplateHeader result = context.TemplateHeaders.SingleOrDefault(b => b.Id == idx && b.UserId == global_var.userId);

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
                                    b => b.Id == idx && b.UserId == global_var.userId);

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
                            TemplateFooter result = context.TemplateFooters.SingleOrDefault(b => b.Id == idx && b.UserId == global_var.userId);

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
                                    b => b.Id == idx && b.UserId == global_var.userId);

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
                                    b => b.Id == idx && b.UserId == global_var.userId);

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

        private void dg_list_footer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TxtHFContent.Text = "";

            using (AppDbContext context = new AppDbContext())
            {
                int HFListID = Convert.ToInt32(dg_list_footer.SelectedRows[0].Cells["dg_list_footer_HFListId"].Value.ToString());

                if (HFListID != 0)
                {
                    HFList result = context.HFLists.SingleOrDefault(
                        b => b.HFListID == HFListID && b.UserId == global_var.userId);

                    TxtHFContent.Text = result.HFContent;
                }
            }
        }

        private void dg_list_header_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TxtHFContent.Text = "";

            using (AppDbContext context = new AppDbContext())
            {
                int HFListID = Convert.ToInt32(dg_list_header.SelectedRows[0].Cells["dg_list_header_HFListId"].Value.ToString());

                if (HFListID != 0)
                {
                    HFList result = context.HFLists.SingleOrDefault(
                        b => b.HFListID == HFListID && b.UserId == global_var.userId);

                    TxtHFContent.Text = result.HFContent;
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

        private void BtnSearchProduct_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
            {
                if(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString() == TxtSearchProduct.Text.Trim())
                {
                    dgSrcItemList.Rows[i].Selected = true;
                    dgSrcItemList.FirstDisplayedScrollingRowIndex = i;
                    dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value = true;
                }
            }
        }

        private void BtnSetAttribute_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.SelectedRows.Count > 0)
            {
                string src_partner_id = "";
                string tar_partner_id = "";

                string src_shop_id = "";
                string tar_shop_id = "";

                string src_api_key = "";
                string tar_api_key = "";

                string end_point = "https://partner.shopeemobile.com/api/v1/item/add";

                for (int i = 0; i < dg_site_id.Rows.Count; i++)
                {
                    if (dg_site_id.Rows[i].Cells["dg_site_id_id"].Value.ToString() == TxtSourceId.Text)
                    {
                        src_partner_id = dg_site_id.Rows[i].Cells["dg_site_id_partner_id"].Value.ToString();
                        src_shop_id = dg_site_id.Rows[i].Cells["dg_site_id_shop_id"].Value.ToString();
                        src_api_key = dg_site_id.Rows[i].Cells["dg_site_id_secret_key"].Value.ToString();
                        break;
                    }
                }

                for (int i = 0; i < dg_site_id.Rows.Count; i++)
                {
                    if (dg_site_id.Rows[i].Cells["dg_site_id_id"].Value.ToString() == TxtTargetId.Text)
                    {
                        tar_partner_id = dg_site_id.Rows[i].Cells["dg_site_id_partner_id"].Value.ToString();
                        tar_shop_id = dg_site_id.Rows[i].Cells["dg_site_id_shop_id"].Value.ToString();
                        tar_api_key = dg_site_id.Rows[i].Cells["dg_site_id_secret_key"].Value.ToString();
                        break;
                    }
                }

                string srcCategory = dgSrcItemList.SelectedRows[0].Cells["dgItemList_category_id"].Value.ToString();
                string tarCategory = dgSrcItemList.SelectedRows[0].Cells["dgItemList_tar_category"].Value.ToString();
                string strAttribute = dgSrcItemList.SelectedRows[0].Cells["dgItemList_attributes"].Value.ToString().Replace("\r\n", "");
                string srcProductId = dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString();
                dynamic strAttributes = JsonConvert.DeserializeObject(strAttribute);

                if (src_partner_id != string.Empty
                    && src_shop_id != string.Empty
                    && src_api_key != string.Empty
                    && tar_partner_id != string.Empty
                    && tar_shop_id != string.Empty
                    && tar_api_key != string.Empty
                    && srcCategory != string.Empty 
                    && tarCategory != string.Empty)
                {
                    FormSetAttributeClone fcs = new FormSetAttributeClone();
                    fcs.TxtSrcPartnerId.Text = src_partner_id;
                    fcs.TxtSrcShopId.Text = src_shop_id;
                    fcs.TxtSrcSecretKey.Text = src_api_key;
                    fcs.TxtTarPartnerId.Text = tar_partner_id;
                    fcs.TxtTarShopId.Text = tar_shop_id;
                    fcs.TxtTarSecretKey.Text = tar_api_key;
                    fcs.TxtSrcCategoryId.Text = srcCategory;
                    fcs.TxtTarCategoryId.Text = tarCategory;
                    fcs.TxtSrcAttributeStr.Text = strAttribute;
                    fcs.TxtTarShopeeAccount.Text = TxtTargetId.Text.Trim();
                    fcs.TxtSrcProductId.Text = srcProductId;
                    fcs.TxtSrcCountry.Text = TxtSourceCountry.Text.Trim();
                    fcs.TxtTarCountry.Text = TxtTargetCountry.Text.Trim();
                    fcs.Pic.Image = (Image)dgSrcItemList.SelectedRows[0].Cells["dgItemList_Image"].Value;
                    fcs.TxtProductTitle.Text = dgSrcItemList.SelectedRows[0].Cells["dgItemList_name"].Value.ToString();
                    fcs.TxtProductDesc.Text = dgSrcItemList.SelectedRows[0].Cells["dgItemList_description"].Value.ToString();
                    fcs.ShowDialog();
                }
                else
                {
                    MessageBox.Show("원본 카테고리가 설정되지 않았습니다.","원본카테고리 설정",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }

        private void btn_validate_attribute_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        //해당 속성의 mandatory가 설정되어 있으면 검증 완료
                        long srcProductId = Convert.ToInt64(dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString());
                        using (AppDbContext context = new AppDbContext())
                        {
                            List<ProductAttribute> attributeList = context.ProductAttributes
                            .Where(x => x.srcProductId == srcProductId
                            && x.tarShopeeAccount == TxtTargetId.Text.Trim()
                            && x.UserId == global_var.userId)
                            .OrderBy(x => x.tarAttributeId).ToList();

                            if (attributeList != null && attributeList.Count > 0)
                            {
                                bool isValid = true;
                                for (int j = 0; j < attributeList.Count; j++)
                                {
                                    if (attributeList[j].isMandatory == true)
                                    {
                                        if (attributeList[j].tarAttributeValue.Trim() == string.Empty)
                                        {
                                            isValid = false;
                                        }
                                    }
                                }

                                if (isValid)
                                {
                                    dgSrcItemList.Rows[i].Cells["dgItemList_tar_attribute_valid"].Value = true;
                                    dgSrcItemList.Rows[i].Cells["dgItemList_tar_attribute_valid"].Style.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    dgSrcItemList.Rows[i].Cells["dgItemList_tar_attribute_valid"].Value = false;
                                    dgSrcItemList.Rows[i].Cells["dgItemList_tar_attribute_valid"].Style.BackColor = Color.Orange;
                                }
                            }
                            else
                            {
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_attribute_valid"].Value = false;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_attribute_valid"].Style.BackColor = Color.Orange;
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;

                MessageBox.Show("체크한 상품에 대하여 모든 속성값을 검증 하였습니다", "속성데이터 검증", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }   
        }

        private void btn_validate_variation_Click(object sender, EventArgs e)
        {
            //옵션 검증시 DB에 옵션을 생성하여 밀어 넣는다.
            if (dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;

                string SrcCountry = TxtSourceCountry.Text.Trim();
                string TarCountry = TxtTargetCountry.Text.Trim();

                string SrcShopeeId = TxtSourceId.Text.Trim();
                string TarShopeeId = TxtTargetId.Text.Trim();

                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {

                        string productId = dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString();
                        string productSKU = dgSrcItemList.Rows[i].Cells["dgItemList_item_sku"].Value.ToString();
                        string productName = dgSrcItemList.Rows[i].Cells["dgItemList_name"].Value.ToString();
                        string strVariation = dgSrcItemList.Rows[i].Cells["dgItemList_variations"].Value.ToString().Trim();
                        dynamic dynVariation = JsonConvert.DeserializeObject(strVariation);

                        if (dynVariation != null && dynVariation.Count > 0)
                        {
                            //Variation이 있는 경우
                            //진짜 2티어인지 검증한다. 모든 옵션에 콤마가 있으면 2티어임.
                            bool isReal2Tier = true;
                            bool isSamePrice = true;

                            var initPrice = dynVariation[0].price;

                            for (int tier = 0; tier < dynVariation.Count; tier++)
                            {
                                if (!dynVariation[tier].name.ToString().Contains(","))
                                {
                                    isReal2Tier = false;
                                    break;
                                }
                            }

                            for (int tier = 0; tier < dynVariation.Count; tier++)
                            {
                                if (initPrice != dynVariation[tier].price)
                                {
                                    isSamePrice = false;
                                    break;
                                }
                            }

                            //진짜 2티어가 아닌경우와 옵션 가격이 동일한 경우 단순 등록한다.
                            if (!isReal2Tier && isSamePrice)
                            {
                                dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Value = true;
                                dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Style.BackColor = Color.Green;
                            }
                            else
                            {
                                //옵션에 가격이 다를 경우
                                //해당 옵션을 DB에서 찾은다음 판매가격이 있으면 문제가 없는것임

                                bool isValidDB = true;

                                for (int tier = 0; tier < dynVariation.Count; tier++)
                                {
                                    
                                    string variation_id = dynVariation[tier].variation_id;
                                    string variationSKU = dynVariation[tier].variationSku;
                                    string variationName = dynVariation[tier].variationSku;
                                    int productWeight = 0;
                                    using (AppDbContext context = new AppDbContext())
                                    {
                                        ShopeeVariationPrice result = context.ShopeeVariationPrices.SingleOrDefault(
                                                b => b.SrcShopeeCountry == SrcCountry &&
                                                b.SrcShopeeId == SrcShopeeId &&
                                                b.TarShopeeCountry == TarCountry &&
                                                b.TarShopeeId == TarShopeeId &&
                                                b.variation_id == variation_id
                                                && b.UserId == global_var.userId);

                                        if (result != null)
                                        {
                                            if (result.TarNetPrice != 0 && result.TarRetail_price != 0)
                                            {
                                                //정확한 판매가가 있는 경우임                                                
                                                result.productName = dynVariation[tier].name;
                                                result.variation_sku = dynVariation[tier].variation_sku;
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                //DB에 레코드는 있으나 판매가가 없는 경우임
                                                isValidDB = false;
                                                result.productName = dynVariation[tier].name;
                                                result.variation_sku = dynVariation[tier].variation_sku;
                                                context.SaveChanges();
                                            }

                                        }
                                        else
                                        {
                                            isValidDB = false;

                                            ShopeeVariationPrice newVariation = new ShopeeVariationPrice
                                            {
                                                SrcShopeeCountry = SrcCountry,
                                                SrcShopeeId = SrcShopeeId,
                                                TarShopeeCountry = TarCountry,
                                                TarShopeeId = TarShopeeId,
                                                productId = productId,
                                                productSKU = productSKU,
                                                productName = productName,
                                                variation_id = variation_id,
                                                variation_sku = variationSKU,
                                                variation_name = variationName,
                                                UserId = global_var.userId
                                            };
                                            context.ShopeeVariationPrices.Add(newVariation);
                                            context.SaveChanges();
                                        }
                                    }
                                }

                                if(isValidDB)
                                {
                                    dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Value = true;
                                    dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Style.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Value = false;
                                    dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Style.BackColor = Color.Orange;
                                }
                            }
                        }
                        else
                        {
                            dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Value = true;
                            dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Style.BackColor = Color.GreenYellow;
                        }
                    }
                }
                Cursor.Current = Cursors.Default;

                MessageBox.Show("체크한 상품에 대하여 모든 속성값을 검증 하였습니다", "속성데이터 검증", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }   
        }

        private void MenuProductList_View_Product_Page_Click(object sender, EventArgs e)
        {
            if(dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
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

        private void MenuProductList_Link_Product_Click(object sender, EventArgs e)
        {
            btn_link_product_Click(null, null);
        }

        private void MenuProductList_Unlink_Product_Click(object sender, EventArgs e)
        {
            btn_unlink_product_Click(null, null);
        }

        private void MenuProductList_Clear_Linked_Product_Click(object sender, EventArgs e)
        {
            btn_remove_link_Click(null, null);
        }

        private void MenuProductList_Validate_Category_Click(object sender, EventArgs e)
        {
            btn_validate_category_Click(null, null);
        }

        private void MenuProductList_Set_Category_Click(object sender, EventArgs e)
        {
            btn_set_category_Click(null, null);
        }

        private void MenuProductList_Validate_Attribute_Click(object sender, EventArgs e)
        {
            btn_validate_attribute_Click(null, null);
        }

        private void MenuProductList_Set_Attribute_Click(object sender, EventArgs e)
        {
            BtnSetAttribute_Click(null, null);
        }

        private void MenuProductList_Set_Option_Click(object sender, EventArgs e)
        {
            btn_set_variation_Click(null, null);
        }

        private void MenuProductList_Validate_Option_Click(object sender, EventArgs e)
        {
            btn_validate_variation_Click(null, null);
        }

        private void MenuProductList_Upload_Checked_Product_Click(object sender, EventArgs e)
        {
            btn_upload_product_Click(null, null);
        }

        private void BtnUploadSellPrice_Click(object sender, EventArgs e)
        {
            if(TxtSourceCountry.Text.Trim() == string.Empty || TxtSourceId.Text.Trim() == string.Empty)
            {
                MessageBox.Show("쇼피 원본 국가와 ID를 지정하세요.","원본국가 및 ID지정",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (TxtTargetCountry.Text.Trim() == string.Empty || TxtTargetId.Text.Trim() == string.Empty)
            {
                MessageBox.Show("쇼피 대상 국가와 ID를 지정하세요.", "대상국가 및 ID지정", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FormUploadExcelPrice fu = new FormUploadExcelPrice();
            fu.TarShopeeId = TxtTargetId.Text.Trim();
            fu.ShowDialog();
        }

        private void btn_set_variation_Click(object sender, EventArgs e)
        {            
            if(dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
            {
                string src_shop_id = "";
                string productId = "";

                for (int i = 0; i < dg_site_id.Rows.Count; i++)
                {
                    if (dg_site_id.Rows[i].Cells["dg_site_id_id"].Value.ToString() == TxtSourceId.Text)
                    {
                        src_shop_id = dg_site_id.Rows[i].Cells["dg_site_id_shop_id"].Value.ToString();
                        break;
                    }
                }

                string strVar = dgSrcItemList.SelectedRows[0].Cells["dgItemList_variations"].Value.ToString().Trim();
                productId = dgSrcItemList.SelectedRows[0].Cells["dgItemList_item_id"].Value.ToString().Trim();
                if (strVar.Trim() != "[]")
                {
                    FormSetVariationClone fs = new FormSetVariationClone();
                    fs.TxtSrcCountry.Text = TxtSourceCountry.Text.Trim();
                    fs.TxtTarCountry.Text = TxtTargetCountry.Text.Trim();
                    fs.TxtSrcID.Text = TxtSourceId.Text.Trim();
                    fs.TxtTarId.Text = TxtTargetId.Text.Trim();
                    fs.TxtStrVariation.Text = strVar;
                    fs.TxtShopId.Text = src_shop_id;
                    fs.TxtProductId.Text = productId;
                    fs.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Variation이 없는 상품입니다.","Variation 없음",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }                
            }
        }

        private void MenuProductList_Upload_price_list_Click(object sender, EventArgs e)
        {
            BtnUploadSellPrice_Click(null, null);
        }

        private void MenuProductList_Calc_price_Click(object sender, EventArgs e)
        {
            BtnCalcSellPrice_Click(null, null);
        }

        private void btn_validate_price_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count > 0 && dgSrcItemList.SelectedRows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;

                string SrcCountry = TxtSourceCountry.Text.Trim();
                string TarCountry = TxtTargetCountry.Text.Trim();

                string SrcShopeeId = TxtSourceId.Text.Trim();
                string TarShopeeId = TxtTargetId.Text.Trim();

                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgItemList_Chk"].Value.ToString() == "True")
                    {
                        string srcProductId = dgSrcItemList.Rows[i].Cells["dgItemList_item_id"].Value.ToString();

                        string strVariation = dgSrcItemList.Rows[i].Cells["dgItemList_variations"].Value.ToString().Trim();
                        dynamic dynVariation = JsonConvert.DeserializeObject(strVariation);

                        //상품에 관련된 모든 Variation을 가지고 온다.
                        using (AppDbContext context = new AppDbContext())
                        {
                            List<ShopeeVariationPrice> priceList = context.ShopeeVariationPrices
                            .Where(b => b.SrcShopeeCountry == SrcCountry &&
                                    b.SrcShopeeId == SrcShopeeId &&
                                    b.TarShopeeCountry == TarCountry &&
                                    b.TarShopeeId == TarShopeeId &&
                                    b.productId == srcProductId
                                    && b.UserId == global_var.userId)
                            .OrderBy(x => x.variation_id).ToList();
                            if(priceList.Count == 0)
                            {
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Value = "";
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Value = "";
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_weight"].Value = "";
                                dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Value = false;


                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Style.BackColor = Color.Orange;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Style.BackColor = Color.Orange;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_weight"].Style.BackColor = Color.Orange;
                                dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Style.BackColor = Color.Orange;
                            }
                            else if(priceList.Count == 1)
                            {
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Value = priceList[0].TarNetPrice;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Value = priceList[0].TarRetail_price;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_weight"].Value = priceList[0].product_weight;
                                dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Value = true;

                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Style.BackColor = Color.GreenYellow;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Style.BackColor = Color.GreenYellow;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_weight"].Style.BackColor = Color.GreenYellow;
                                dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Style.BackColor = Color.GreenYellow;
                            }
                            else if (priceList.Count > 1)
                            {
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Value = priceList[0].TarNetPrice;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Value = priceList[0].TarRetail_price;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_weight"].Value = priceList[0].product_weight;
                                dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Value = true;

                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_net_price"].Style.BackColor = Color.GreenYellow;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_original_price"].Style.BackColor = Color.GreenYellow;
                                dgSrcItemList.Rows[i].Cells["dgItemList_tar_weight"].Style.BackColor = Color.GreenYellow;
                                dgSrcItemList.Rows[i].Cells["dgItemList_is_validate_variation"].Style.BackColor = Color.GreenYellow;
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;

                MessageBox.Show("체크한 상품에 대하여 모든 속성값을 검증 하였습니다", "속성데이터 검증", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TxtSearchProduct_Click(object sender, EventArgs e)
        {

        }
    }
}
