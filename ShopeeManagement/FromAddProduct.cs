using MetroFramework.Forms;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Drawing.Imaging;
using RestSharp;
using OfficeOpenXml;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using AngleSharp.Html.Parser;
using ArtboxModel.Extensions;
using ArtboxModel.Models;
using HtmlAgilityPack;
using ShopeeManagement.CrawlingModel;
using System.Dynamic;
using ShopeeManagement.DTOs.Req;
using ShopeeManagement.DTOs.Res;
using ShopeeManagement.APIs;

namespace ShopeeManagement
{
    public partial class FromAddProduct : MetroForm
    {
        public FromAddProduct()
        {
            InitializeComponent();
            MinimizeBox = false;
            MaximizeBox = false;
            AutoSize = true;
            AutoScroll = true;
            WindowState = FormWindowState.Normal;
        }
        private Guid pGuid;
        private string ImagePath;
        private string ImagePathThumb;
        private string ImagePathDetail;
        private string ImagePathSlice;
        private string TemplateImagePath;
        
        int minDesc = 0;
        int maxDesc = 0;

        private Dictionary<string, decimal> dicCurrencyRate;



        public string strHeaderSeparator;
        public string strFooterSeparator;

        private void FillHeaderSet()
        {
            dg_list_header_set.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<SetHeader> SetHeaderList = context.SetHeaders.Where(x => x.UserId == global_var.userId).OrderBy(x => x.SetHeaderName).ToList();

                for (int i = 0; i < SetHeaderList.Count; i++)
                {
                    dg_list_header_set.Rows.Add(SetHeaderList[i].Id,
                        i + 1, false,
                        SetHeaderList[i].SetHeaderName);
                }
            }
        }
        private void FillFooterSet()
        {
            dg_list_footer_set.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<SetFooter> SetFooterList = context.SetFooters.Where(x => x.UserId == global_var.userId).OrderBy(x => x.SetFooterName).ToList();

                for (int i = 0; i < SetFooterList.Count; i++)
                {
                    dg_list_footer_set.Rows.Add(SetFooterList[i].Id,
                        i + 1, false,
                        SetFooterList[i].SetFooterName);
                }
            }
        }
        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    bool rtn = false;
        //    if (!base.ProcessCmdKey(ref msg, keyData))
        //    {
        //        if (keyData.Equals(Keys.Escape))
        //        {
        //            this.Close();
        //            this.Dispose();
        //        }
        //    }
        //    else
        //    {
        //        rtn = false;
        //    }
        //    return rtn;
        //}

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
            dgCurrencyRate.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<CurrencyRate> currencyList = context.CurrencyRates.Where(x => x.UserId == global_var.userId).OrderBy(x => x.cr_name).ToList();

                if(currencyList.Count == 0)
                {
                    MessageBox.Show("환율정보가 없습니다. 환율 업데이트가 필요합니다.","환율정보 없음",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                {
                    for (int i = 0; i < currencyList.Count; i++)
                    {
                        if(currencyList[i].cr_code.Contains("SGD") ||
                            currencyList[i].cr_code.Contains("IDR") ||
                            currencyList[i].cr_code.Contains("VND") ||
                            currencyList[i].cr_code.Contains("MYR") ||                            
                            currencyList[i].cr_code.Contains("THB") ||
                                currencyList[i].cr_code.Contains("TWD"))
                        dgCurrencyRate.Rows.Add(currencyList[i].cr_name,
                            currencyList[i].cr_code,
                            currencyList[i].cr_exchange);
                    }
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
                    
                    //udPGFee.Value = result.pg_fee;
                    //UdPayoneerFee.Value = result.payoneer_fee;
                    //UdRetailPriceRate.Value = result.retail_price_rate;
                    //UdMargin.Value = result.margin;
                    //UdQty.Value = result.qty;

                    if (result.shopee_fee == 0)
                    {
                        UdShopeeFee.Value = result.shopee_fee;
                    }
                    else
                    {
                        UdShopeeFee.Value = result.shopee_fee;
                    }

                    if (result.pg_fee == 0)
                    {
                        udPGFee.Value = 2;
                    }
                    else
                    {
                        udPGFee.Value = result.pg_fee;
                    }

                    if (result.qty == 0)
                    {
                        UdQty.Value = 100;
                    }
                    else
                    {
                        UdQty.Value = result.qty;
                    }

                    if (result.weight == 0)
                    {
                        UdWeight.Value = 0.5m;
                    }
                    else
                    {
                        UdWeight.Value = (decimal)result.weight;
                    }
                    
                    if (result.dts < 7)
                    {
                        Ud_pre_order_value.Value = 7;
                    }
                    else
                    {
                        Ud_pre_order_value.Value = result.dts;
                    }

                    if (result.retail_price_rate == 0)
                    {
                        UdRetailPriceRate.Value = 150;
                    }
                    else
                    {
                        UdRetailPriceRate.Value = result.retail_price_rate;
                    }

                    if (result.source_price == 0)
                    {
                        UdSourcePrice.Value = 15000m;
                    }
                    else
                    {
                        UdSourcePrice.Value = result.source_price;
                    }

                    if (result.margin == 0)
                    {
                        UdMargin.Value = 15000m;
                    }
                    else
                    {
                        UdMargin.Value = result.margin;
                    }

                }
                else
                {
                    UdShopeeFee.Value = 0;
                    udPGFee.Value = 2;
                    UdRetailPriceRate.Value = 150;
                    UdMargin.Value = 5000;
                    UdQty.Value = 100;
                }
            }
        }
        private void getShopeeAccount()
        {
            dg_site_id.Rows.Clear();
            int maxTitleLength = 0;
            try
            {
                ClsShopee cc = new ClsShopee();
                IList<ShopeeAccount> ShopeeAccounts = cc.GetShopeeAccountList();
                for (int i = 0; i < ShopeeAccounts.Count; i++)
                {
                    if (ShopeeAccounts[i].ShopeeCountry == "ID")
                    {
                        maxTitleLength = 100;
                        minDesc = 20;
                        maxDesc = 3000;
                    }
                    else if (ShopeeAccounts[i].ShopeeCountry == "SG")
                    {
                        maxTitleLength = 80;
                        minDesc = 20;
                        maxDesc = 3000;
                    }
                    else if (ShopeeAccounts[i].ShopeeCountry == "MY")
                    {
                        maxTitleLength = 80;
                        minDesc = 20;
                        maxDesc = 3000;
                    }
                    else if (ShopeeAccounts[i].ShopeeCountry == "TH")
                    {
                        maxTitleLength = 120;
                        minDesc = 25;
                        maxDesc = 5000;
                    }
                    else if (ShopeeAccounts[i].ShopeeCountry == "TW")
                    {
                        maxTitleLength = 60;
                        minDesc = 3;
                        maxDesc = 3000;
                    }
                    else if (ShopeeAccounts[i].ShopeeCountry == "PH")
                    {
                        minDesc = 20;
                        maxDesc = 3000;
                    }
                    else if (ShopeeAccounts[i].ShopeeCountry == "VN")
                    {
                        minDesc = 100;
                        maxDesc = 3000;
                    }
                    dg_site_id.Rows.Add(i + 1, false, ShopeeAccounts[i].ShopeeId,
                        ShopeeAccounts[i].ShopeeCountry,
                        ShopeeAccounts[i].PartnerId,
                        ShopeeAccounts[i].ShopId,
                        ShopeeAccounts[i].SecretKey,
                        minDesc,
                        maxDesc, 
                        maxTitleLength);

                    dg_site_id_category.Rows.Add(i + 1, false, ShopeeAccounts[i].ShopeeId,
                        ShopeeAccounts[i].ShopeeCountry,
                        ShopeeAccounts[i].PartnerId,
                        ShopeeAccounts[i].ShopId,
                        ShopeeAccounts[i].SecretKey,
                        minDesc,
                        maxDesc,
                        maxTitleLength);

                    dg_site_id_reg.Rows.Add(i + 1, false, ShopeeAccounts[i].ShopeeId,
                        ShopeeAccounts[i].ShopeeCountry,
                        ShopeeAccounts[i].PartnerId,
                        ShopeeAccounts[i].ShopId,
                        ShopeeAccounts[i].SecretKey,
                        minDesc,
                        maxDesc,
                        maxTitleLength);

                    dg_site_id_desc.Rows.Add(i + 1, false, ShopeeAccounts[i].ShopeeId,
                        ShopeeAccounts[i].ShopeeCountry,
                        ShopeeAccounts[i].PartnerId,
                        ShopeeAccounts[i].ShopId,
                        ShopeeAccounts[i].SecretKey,
                        minDesc,
                        maxDesc,
                        maxTitleLength);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getShopeeAccount: " + ex.Message);
            }

        }
        private void getPromotionList(string shopeeId)
        {
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
        }
        private void FromAddProduct_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            cboLang.SelectedIndex = 0;
            cboLang2.SelectedIndex = 0;
            TxtCategorySearchText.ImeMode = ImeMode.Alpha;
            TxtTitleTypeSearch.ImeMode = ImeMode.Alpha;
            TxtTitleType.ImeMode = ImeMode.Alpha;

            TxtBrandSearch.ImeMode = ImeMode.Alpha;
            TxtBrand.ImeMode = ImeMode.Alpha;

            TxtModelSearch.ImeMode = ImeMode.Alpha;
            TxtModel.ImeMode = ImeMode.Alpha;

            TxtGroupSearch.ImeMode = ImeMode.Alpha;
            TxtGroup.ImeMode = ImeMode.Alpha;

            TxtFeatureSearch.ImeMode = ImeMode.Alpha;
            TxtFeature.ImeMode = ImeMode.Alpha;

            TxtOptionSearch.ImeMode = ImeMode.Alpha;
            TxtOption.ImeMode = ImeMode.Alpha;

            TxtRelatSearch.ImeMode = ImeMode.Alpha;
            TxtRelat.ImeMode = ImeMode.Alpha;

            TxtRelatSearch.ImeMode = ImeMode.Alpha;
            TxtRelat.ImeMode = ImeMode.Alpha;


            TxtCategorySearchText.ImeMode = ImeMode.Alpha;

            TabMain.SelectedIndex = 0;
            getShopeeAccount();
            setDefaultVar();
            getLogisticList();
            Fill_Currency_Date();
            Fill_from_Currency_Names();

            lblDesc.Text = "상품 설명 [ 최소 : " + minDesc + "자 / 최대 : " + string.Format("{0:n0}", maxDesc) + "자 ]";
            lblDescPreview.Text = "상품 설명 미리보기 [ 최소 : " + minDesc + "자 / 최대 : " + string.Format("{0:n0}", maxDesc) + "자 ]";
            

            //DgPrice.Rows.Add();

            InitDgAttribute();
            getFavKeywordList();
            getFavKeywordOtherList();


            //GetCategory(); // GetCategoryAPI로 사용
            CboProductCondition.SelectedIndex = 0;
            pGuid = Guid.NewGuid();
            ImagePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\ShopeeManagement\ItemImages\{pGuid}\";
            ImagePathThumb = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\ShopeeManagement\ItemImages\{pGuid}\Thumb\";
            ImagePathSlice = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\ShopeeManagement\ItemImages\{pGuid}\Slice\";
            TemplateImagePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\ShopeeManagement\ItemImages\TemplateImage\";
            FillHeaderSet();
            FillFooterSet();
            
            getTitleTypeList();
            if (dgTitle_type.Rows.Count > 0)
            {
                TxtTitleType.Text = dgTitle_type.Rows[0].Cells["dgTitle_type_Name"].Value.ToString();
                int TypeId = Convert.ToInt32(dgTitle_type.Rows[0].Cells["dgTitle_type_id"].Value.ToString());

                if (dgBrand.Rows.Count > 0)
                {
                    int BrandId = Convert.ToInt32(dgBrand.Rows[0].Cells["dgBrand_id"].Value.ToString());
                    getListModel(BrandId);
                }


                getListBrand(TypeId);
                getListGroup(TypeId);
                getListFeature(TypeId);
                getListOption(TypeId);
                getListRelat(TypeId);
            }

            //템플릿 이미지 로드
            LoadTemplateImage();

            //할인 이벤트 목록
            if (DgPrice.Rows.Count > 0 && DgPrice.SelectedRows.Count > 0)
            {
                string shopeeId = DgPrice.SelectedRows[0].Cells["DgPrice_shopee_id"].Value.ToString();
                getPromotionList(shopeeId);
            }
        }
        private void getLogisticList()
        {
            dg_shopee_logistics.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<Logistic> logisticList = context.Logistics.Where(x => x.UserId == global_var.userId)
                    .OrderBy(x => x.logistic_name).ToList();

                if (logisticList.Count > 0)
                {
                    for (int i = 0; i < logisticList.Count; i++)
                    {
                        dg_shopee_logistics.Rows.Add(i + 1,
                            false,
                            "",
                            logisticList[i].shopeeAccount,
                            logisticList[i].logistic_name,
                            logisticList[i].logistic_id,
                            logisticList[i].has_cod,
                            logisticList[i].fee_type);
                    }

                    dg_shopee_logistics.Rows[0].Selected = true;
                }
            }
        }
        private void LoadTemplateImage()
        {
            if (Directory.Exists(TemplateImagePath))
            {
                int i = 1;
                WebClient wc = new WebClient();
                DirectoryInfo di = new DirectoryInfo(TemplateImagePath);
                foreach (var item in di.GetFiles())
                {
                    var content = wc.DownloadData(item.FullName);
                    using (var stream = new MemoryStream(content))
                    {
                        Image im = Image.FromStream(stream);
                        DGImageTempletelList.Rows.Add(i++, false, im, item.Name, im.Width, im.Height, item.FullName);
                    }
                }
            }

            lblTemplate.Text = "템플릿 이미지 [ " + DGImageTempletelList.Rows.Count + " ]";
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

        private void getTitleTypeList()
        {
            dgTitle_type.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleType> lstList = context.TitleTypes.Where(x => x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleTypeName).ToList();

                for (int i = 0; i < lstList.Count; i++)
                {
                    dgTitle_type.Rows.Add(lstList[i].Id, i + 1, false, lstList[i].TitleTypeName);
                }
            }
        }

        private void getTitleTypeList(string typeName)
        {
            dgTitle_type.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleType> lstList = context.TitleTypes
                    .Where(x => x.TitleTypeName.ToUpper().Contains(typeName)
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleTypeName).ToList();

                for (int i = 0; i < lstList.Count; i++)
                {
                    dgTitle_type.Rows.Add(lstList[i].Id, i + 1, false, lstList[i].TitleTypeName);
                }
            }
        }
        private void getListBrand(int TitleTypeId)
        {
            dgBrand.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleBrand> lstBrand = context.TitleBrands
                    .Where(x => x.TitleTypeId == TitleTypeId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleBrandName).ToList();

                for (int i = 0; i < lstBrand.Count; i++)
                {
                    dgBrand.Rows.Add(lstBrand[i].Id, i + 1, false, lstBrand[i].TitleBrandName);
                }
            }
        }
        private void getListBrand(int TitleTypeId, string BrandName)
        {
            dgBrand.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleBrand> lstBrand = context.TitleBrands
                    .Where(x => x.TitleTypeId == TitleTypeId &&
                    x.TitleBrandName.ToUpper().Contains(BrandName)
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleBrandName).ToList();

                for (int i = 0; i < lstBrand.Count; i++)
                {
                    dgBrand.Rows.Add(lstBrand[i].Id, i + 1, false, lstBrand[i].TitleBrandName);
                }
            }
        }

        private void getListModel(int TitleBrandId)
        {
            dgModel.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleModel> lstModel = context.TitleModels
                    .Where(x => x.TitleBrandId == TitleBrandId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleModelName).ToList();

                for (int i = 0; i < lstModel.Count; i++)
                {
                    dgModel.Rows.Add(lstModel[i].Id, i + 1, false, lstModel[i].TitleModelName);
                }
            }
        }

        private void getListGroup(int TitleTypeId)
        {
            dgGroup.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleGroup> lstGroup = context.TitleGroups
                    .Where(x => x.TitleTypeId == TitleTypeId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleGroupName).ToList();

                for (int i = 0; i < lstGroup.Count; i++)
                {
                    dgGroup.Rows.Add(lstGroup[i].Id, i + 1, false, lstGroup[i].TitleGroupName);
                }
            }
        }

        private void getListFeature(int TitleTypeId)
        {
            dgFeature.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleFeature> lstFeature = context.TitleFeatures
                    .Where(x => x.TitleTypeId == TitleTypeId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleFeatureName).ToList();

                for (int i = 0; i < lstFeature.Count; i++)
                {
                    dgFeature.Rows.Add(lstFeature[i].Id, i + 1, false, lstFeature[i].TitleFeatureName);
                }
            }
        }
        private void getListOption(int TitleTypeId)
        {
            dgOption.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleOption> lstOption = context.TitleOptions
                    .Where(x => x.TitleTypeId == TitleTypeId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleOptionName).ToList();

                for (int i = 0; i < lstOption.Count; i++)
                {
                    dgOption.Rows.Add(lstOption[i].Id, i + 1, false, lstOption[i].TitleOptionName);
                }
            }
        }
        private void getListRelat(int TitleTypeId)
        {
            dgRelat.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleRelat> lstRelat = context.TitleRelats
                    .Where(x => x.TitleTypeId == TitleTypeId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleRelatName).ToList();

                for (int i = 0; i < lstRelat.Count; i++)
                {
                    dgRelat.Rows.Add(lstRelat[i].Id, i + 1, false, lstRelat[i].TitleRelatName);
                }
            }
        }
        private void getSubList(int TitleTypeId)
        {

        }
        private void InitDgAttribute()
        {
            DataGridViewComboBoxColumn cboTarOption = new DataGridViewComboBoxColumn();
            cboTarOption.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            cboTarOption.HeaderText = "옵션";
            cboTarOption.Name = "DgTarAttribute_option";

            DataGridViewCheckBoxColumn chkTarMandatory = new DataGridViewCheckBoxColumn();
            chkTarMandatory.HeaderText = "필수";
            chkTarMandatory.Name = "DgTarAttribute_is_mandatory";


            //DataGridViewComboBoxColumn cboSaveOption = new DataGridViewComboBoxColumn();
            //cboTarOption.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            //cboTarOption.HeaderText = "옵션";
            //cboTarOption.Name = "DgSaveAttribute_option";

            //DataGridViewCheckBoxColumn chkSaveMandatory = new DataGridViewCheckBoxColumn();
            //chkTarMandatory.HeaderText = "필수";
            //chkTarMandatory.Name = "DgSaveAttribute_is_mandatory";

            DgAttribute.Columns.Add("DgTarAttribute_no", "No");
            DgAttribute.Columns.Add("DgAttribute_attribute_name", "속성명");
            DgAttribute.Columns.Add("DgAttribute_input_type", "입력타입");
            DgAttribute.Columns.Add("DgAttribute_attribute_id", "속성ID");
            DgAttribute.Columns.Add("DgAttribute_attribute_type", "속성타입");
            DgAttribute.Columns.Add(chkTarMandatory);
            DgAttribute.Columns.Add(cboTarOption);
            DgAttribute.Columns.Add("DgAttribute_is_complete", "완료여부");
            DgAttribute.Columns.Add("DgAttribute_attribute_value", "설정된 값");

            //DgSaveAttribute.Columns.Add("DgTarAttribute_no", "No");
            //DgSaveAttribute.Columns.Add("DgAttribute_attribute_name", "속성명");
            //DgSaveAttribute.Columns.Add("DgAttribute_input_type", "입력타입");
            //DgSaveAttribute.Columns.Add("DgAttribute_attribute_id", "속성ID");
            //DgSaveAttribute.Columns.Add("DgAttribute_attribute_type", "속성타입");
            //DgSaveAttribute.Columns.Add(chkSaveMandatory);
            //DgSaveAttribute.Columns.Add(cboSaveOption);
            //DgSaveAttribute.Columns.Add("DgAttribute_is_complete", "완료여부");
            //DgSaveAttribute.Columns.Add("DgAttribute_attribute_value", "설정된 값");

            DgAttribute.Columns[0].Width = 28;
            DgAttribute.Columns[1].Width = 250;
            DgAttribute.Columns[2].Width = 87;
            DgAttribute.Columns[3].Width = 70;
            DgAttribute.Columns[4].Width = 104;
            DgAttribute.Columns[5].Width = 36;
            DgAttribute.Columns[6].Width = 250;
            DgAttribute.Columns[7].Width = 80;
            DgAttribute.Columns[8].Width = 250;

            //DgSaveAttribute.Columns[0].Width = 28;
            //DgSaveAttribute.Columns[1].Width = 250;
            //DgSaveAttribute.Columns[2].Width = 87;
            //DgSaveAttribute.Columns[3].Width = 70;
            //DgSaveAttribute.Columns[4].Width = 104;
            //DgSaveAttribute.Columns[5].Width = 36;
            //DgSaveAttribute.Columns[6].Width = 250;
            //DgSaveAttribute.Columns[7].Width = 80;
            //DgSaveAttribute.Columns[8].Width = 250;

            DgAttribute.Columns[0].ReadOnly = true;
            DgAttribute.Columns[1].ReadOnly = true;
            DgAttribute.Columns[2].ReadOnly = true;
            DgAttribute.Columns[3].ReadOnly = true;
            DgAttribute.Columns[4].ReadOnly = true;
            DgAttribute.Columns[5].ReadOnly = true;
            DgAttribute.Columns[6].ReadOnly = false;
            DgAttribute.Columns[7].ReadOnly = true;
            DgAttribute.Columns[8].ReadOnly = false;

            //DgSaveAttribute.Columns[0].ReadOnly = true;
            //DgSaveAttribute.Columns[1].ReadOnly = true;
            //DgSaveAttribute.Columns[2].ReadOnly = true;
            //DgSaveAttribute.Columns[3].ReadOnly = true;
            //DgSaveAttribute.Columns[4].ReadOnly = true;
            //DgSaveAttribute.Columns[5].ReadOnly = true;
            //DgSaveAttribute.Columns[6].ReadOnly = false;
            //DgSaveAttribute.Columns[7].ReadOnly = true;
            //DgSaveAttribute.Columns[8].ReadOnly = false;


            DgAttribute.Columns[2].Visible = false;
            DgAttribute.Columns[4].Visible = false;
            DgAttribute.Columns[7].Visible = false;


            //DgSaveAttribute.Columns[2].Visible = false;
            //DgSaveAttribute.Columns[4].Visible = false;
            //DgSaveAttribute.Columns[7].Visible = false;

            DgAttribute.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgAttribute.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DgAttribute.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgAttribute.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgAttribute.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgAttribute.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgAttribute.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DgAttribute.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgAttribute.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;



            //DgSaveAttribute.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //DgSaveAttribute.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //DgSaveAttribute.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //DgSaveAttribute.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //DgSaveAttribute.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //DgSaveAttribute.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //DgSaveAttribute.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //DgSaveAttribute.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //DgSaveAttribute.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void dgCategoryList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string strCategoryId = dgCategoryList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();
            string tarCountry = dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_country"].Value.ToString();
            dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_set_category_id"].Value = dgCategoryList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();
            //GetTarAttributeData(Convert.ToInt32(strCategoryId), true, tarCountry);
            GetTarAttributeDataAPI(Convert.ToInt32(strCategoryId), true, tarCountry);
            Cursor.Current = Cursors.Default;
        }

        private void GetTarAttributeDataAPI(int categoryID, bool isTranslated, string tarCountry)
        {
            DgAttribute.Rows.Clear();

            string endPoint = $"https://shopeecategory.azurewebsites.net/api/ShopeeAttribute?CategoryId={categoryID}&CountryCode={tarCountry}";
            var request = new RestRequest("", Method.GET);
            request.Method = Method.GET;
            var client = new RestClient(endPoint);
            IRestResponse response = client.Execute(request);
            List<APIShopeeCategory> lstShopeeCategory = new List<APIShopeeCategory>();
            var result = JsonConvert.DeserializeObject<dynamic>(response.Content);

            for (int i = 0; i < result.Count; i++)
            {
                var dicCombo = new Dictionary<string, string>();
                var NewComboCell = new DataGridViewComboBoxCell();
                NewComboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;

                if (result[i].InputType.ToString() == "COMBO_BOX" || result[i].InputType.ToString() == "DROP_DOWN")
                {
                    var options = JsonConvert.DeserializeObject<dynamic>(result[i].Options.Value.ToString());

                    for (int j = 0; j < options.Count; j++)
                    {
                        dicCombo.Add(options[j].ToString(), options[j].ToString());
                    }

                    if (dicCombo.Count > 0)
                    {
                        NewComboCell.DataSource = new BindingSource(dicCombo, null);
                        NewComboCell.DisplayMember = "Key";
                        NewComboCell.ValueMember = "Value";
                    }
                }
                else if (result[i].InputType.ToString() == "TEXT_FILED")
                {
                }
                else
                {
                }

                DgAttribute.Rows.Add(i + 1, // DgTarAttrivute_no, No
                    result[i].AttributeName.ToString(), // DgAttribute_attribute_name, 속성명
                    result[i].InputType.ToString(), // DgAttribute_input_type, 입력타입
                    result[i].AttributeId.ToString(), // DgAttribute_attribute_id, 속성ID
                    result[i].AttributeType.ToString(), // DgAttribute_attribute_type, 속성타입
                    (bool)result[i].isMandatory, // chkTarMandatory
                    null, // choTarOption
                    false, // DgAttribute_is_complete, 완료여부
                           //result[i].AttributeName.ToString()); // DgAttribute_attribute_value
                    string.Empty); // DgAttribute_attribute_value
                DgAttribute.Rows[DgAttribute.Rows.Count - 1].Cells[6] = NewComboCell;
            }

            //수정 필요
            if (dg_productTitle.Rows.Count > 0 && dg_productTitle.SelectedRows.Count > 0)
            {
                string brandName = dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString().Trim();

                var pattern = @"\[(.*?)\]";
                var matches = Regex.Matches(brandName, pattern);

                foreach (Match m in matches)
                {
                    brandName = m.Groups[1].ToString();
                }

                if (brandName != string.Empty)
                {
                    for (int i = 0; i < DgAttribute.Rows.Count; i++)
                    {
                        if (DgAttribute.Rows[i].Cells["DgAttribute_attribute_name"].Value.ToString() == "브랜드" ||
                            DgAttribute.Rows[i].Cells["DgAttribute_attribute_name"].Value.ToString() == "상표" ||
                            DgAttribute.Rows[i].Cells["DgAttribute_attribute_name"].Value.ToString() == "Brand" ||
                            DgAttribute.Rows[i].Cells["DgAttribute_attribute_name"].Value.ToString() == "Merek")
                        {
                            DgAttribute.Rows[i].Cells["DgAttribute_attribute_value"].Value = brandName;
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
            //string hash = BitConverter.ToString(HashCode).Replace("-", "");
            string hash1 = Convert.ToBase64String(HashCode);
            string hash2 = BitConverter.ToString(HashCode);
            string hash3 = BitConverter.ToString(HashCode).Replace("-", "").ToLower();
            string hash4 = Convert.ToBase64String(HashCode).TrimEnd("=".ToCharArray());
            string hash5 = ByteToString(HashCode);
            hash1 = hash1.ToLower();

            return hash5.ToLower();
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
        private void GetTarAttributeData(int categoryID, bool isTranslated, string tarCountry)
        {
            DgAttribute.Rows.Clear();

            using (AppDbContext context = new AppDbContext())
            {
                List<AllAttributeList> savedAttrList = context.AllAttributeLists
                    .Where(b => b.category_id == categoryID &&
                                b.country == tarCountry)
                                .OrderByDescending(x => x.is_mandatory).ToList();

                if (savedAttrList != null && savedAttrList.Count > 0)
                {
                    for (int i = 0; i < savedAttrList.Count; i++)
                    {
                        DataGridViewComboBoxCell NewComboCell = new DataGridViewComboBoxCell();
                        NewComboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;

                        List<string> lstOptionsKor = new List<string>();
                        List<string> lstOptionsSrc = new List<string>();

                        lstOptionsKor = savedAttrList[i].options_kor.ToString().Split('^').ToList();
                        lstOptionsSrc = savedAttrList[i].options.ToString().Split('^').ToList();


                        Dictionary<string, string> dicCombo = new Dictionary<string, string>();

                        if (isTranslated)
                        {
                            for (int j = 0; j < lstOptionsSrc.Count; j++)
                            {
                                try
                                {
                                    dicCombo.Add(lstOptionsKor[j], lstOptionsSrc[j]);
                                }
                                catch
                                {

                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < lstOptionsSrc.Count; j++)
                            {
                                try
                                {
                                    dicCombo.Add(lstOptionsSrc[j], lstOptionsSrc[j]);
                                }
                                catch
                                {

                                }
                            }
                        }


                        if (dicCombo.Count > 0)
                        {
                            NewComboCell.DataSource = new BindingSource(dicCombo, null);
                            NewComboCell.DisplayMember = "Key";
                            NewComboCell.ValueMember = "Value";
                        }

                        string attributeName = "";
                        if (isTranslated)
                        {
                            attributeName = savedAttrList[i].attribute_name_kor.ToString();
                        }
                        else
                        {
                            attributeName = savedAttrList[i].attribute_name.ToString();
                        }

                        DgAttribute.Rows.Add(i + 1,
                            attributeName,
                            savedAttrList[i].input_type.ToString(),
                            savedAttrList[i].attribute_id.ToString(),
                            savedAttrList[i].attribute_type.ToString(),
                            (bool)savedAttrList[i].is_mandatory,
                            null,
                            false,
                            "",
                            savedAttrList[i].attribute_name.ToString());

                        DgAttribute.Rows[DgAttribute.Rows.Count - 1].Cells[6] = NewComboCell;
                    }

                    DgAttribute.Sort(DgAttribute.Columns[5], ListSortDirection.Descending);
                    dg_site_id_category.EndEdit();
                }
                else
                {
                    MessageBox.Show("속성 데이터가 없습니다.\r\n속성데이터 관리에서 해당 국가의 속성 데이터를 업로드 해 주세요.","속성 데이터 누락",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    BtnManageAttributeData_Click(null, null);
                }

                //수정 필요
                if(dg_productTitle.Rows.Count > 0 && dg_productTitle.SelectedRows.Count > 0)
                {

                    string brandName = dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString().Trim();

                    var pattern = @"\[(.*?)\]";
                    var matches = Regex.Matches(brandName, pattern);

                    foreach (Match m in matches)
                    {
                        brandName = m.Groups[1].ToString();
                    }

                    if(brandName != string.Empty)
                    {
                        for (int i = 0; i < DgAttribute.Rows.Count; i++)
                        {
                            if (DgAttribute.Rows[i].Cells["DgAttribute_attribute_name"].Value.ToString() == "브랜드" ||
                                    DgAttribute.Rows[i].Cells["DgAttribute_attribute_name"].Value.ToString() == "상표" ||
                                    DgAttribute.Rows[i].Cells["DgAttribute_attribute_name"].Value.ToString() == "Brand")
                            {
                                DgAttribute.Rows[i].Cells["DgAttribute_attribute_value"].Value = brandName;
                            }
                        }
                    }
                }
            }
        }

        private void DgAttribute_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView control = sender as DataGridView;

            if (control.CurrentCell.ColumnIndex == 6 && e.Control is ComboBox)
            {
                //control.CurrentRow.Selected = true;
                ComboBox comboBox = e.Control as ComboBox;
                comboBox.SelectedIndexChanged -= LastColumnComboSelectionChanged;
                comboBox.SelectedIndexChanged += LastColumnComboSelectionChanged;
            }
        }

        private void LastColumnComboSelectionChanged(object sender, EventArgs e)
        {
            var currentcell = DgAttribute.CurrentCellAddress;
            var sendingCB = sender as DataGridViewComboBoxEditingControl;
            if (sendingCB.SelectedItem != null)
            {
                var typeName = sendingCB.SelectedItem.GetType().Name.ToString();
                if (typeName.Contains("KeyValuePair"))
                {
                    KeyValuePair<string, string> cboInfo = (KeyValuePair<string, string>)sendingCB.SelectedItem;
                    DgAttribute.Rows[currentcell.Y].Cells["DgAttribute_attribute_value"].Value = cboInfo.Value.ToString();
                }
                else
                {
                    DgAttribute.Rows[currentcell.Y].Cells["DgAttribute_attribute_value"].Value = sendingCB.SelectedItem.ToString();
                }
            }
        }

        private void BtnAttributeTranslate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (dgCategoryList.SelectedRows.Count > 0)
            {
                string strCategoryId = dgCategoryList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();
                string tarCountry = dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_country"].Value.ToString();
                GetTarAttributeData(Convert.ToInt32(strCategoryId), true, tarCountry);
            }



            //실제 구글 번역에서 바로 번역
            //string tar_lang = string.Empty;
            //if (tarCountry == "ID")
            //{
            //    tar_lang = "id";
            //}
            //else if (tarCountry == "SG")
            //{
            //    tar_lang = "en";
            //}
            //else if (tarCountry == "MY")
            //{
            //    tar_lang = "ms";
            //}
            //else if (tarCountry == "TH")
            //{
            //    tar_lang = "th";
            //}
            //else if (tarCountry == "TW")
            //{
            //    tar_lang = "zh-TW";
            //}
            //else if (tarCountry == "PH")
            //{
            //    tar_lang = "en";
            //}
            //else if (tarCountry == "VN")
            //{
            //    tar_lang = "vi";
            //}
            //string translate_lang = "ko";
            //for (int i = 0; i < DgAttribute.Rows.Count; i++)
            //{
            //    string translated = string.Empty;
            //    translated = translate(
            //        DgAttribute.Rows[i].Cells["DgAttribute_attribute_name"].Value.ToString(),
            //        tar_lang,
            //        translate_lang).Trim();

            //    DgAttribute.Rows[i].Cells["DgAttribute_attribute_name"].Value = translated;

            //    if (!translated.Contains("브랜드") && !translated.Contains("상표"))
            //    {
            //        string translated2 = "";
            //        DataGridViewComboBoxCell lstCombo = (DataGridViewComboBoxCell)DgAttribute.Rows[i].Cells[6];
            //        DataGridViewComboBoxCell NewComboCell = new DataGridViewComboBoxCell();

            //        Dictionary<string, string> dicCombo = new Dictionary<string, string>();

            //        for (int j = 0; j < lstCombo.Items.Count; j++)
            //        {
            //            translated2 = translate(
            //            lstCombo.Items[j].ToString(),
            //            tar_lang,
            //            translate_lang).Trim();
            //            try
            //            {
            //                dicCombo.Add(translated2, lstCombo.Items[j].ToString());
            //            }
            //            catch
            //            {

            //            }
            //        }

            //        if (dicCombo.Count > 0)
            //        {
            //            NewComboCell.DataSource = new BindingSource(dicCombo, null);
            //            NewComboCell.DisplayMember = "Key";
            //            NewComboCell.ValueMember = "Value";

            //            DgAttribute.Rows[i].Cells[6] = NewComboCell;
            //        }
            //    }
            //}


            Cursor.Current = Cursors.Default;

            //MessageBox.Show("번역을 완료 하였습니다.", "번역 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public string translate(string src_str, string src_lang, string tar_lang)
        {
            string rtn = string.Empty;

            if (src_str != string.Empty)
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("X-HTTP-Method-Override", "GET");
                    var data = new NameValueCollection()
                {
                    { "key", "AIzaSyBElCVs2sX9FzSF8iZvbzzh6gvcZht95Lg" },
                    { "source", src_lang },
                    { "target", tar_lang},
                    { "q", src_str }
                };
                    string GoogleTranslateApiUrl = "https://www.googleapis.com/language/translate/v2";
                    try
                    {
                        var responseBytes = webClient.UploadValues(GoogleTranslateApiUrl, "POST", data);
                        var json = Encoding.UTF8.GetString(responseBytes);
                        var result = JsonConvert.DeserializeObject<dynamic>(json);
                        var translation = result.data.translations[0].translatedText.Value;
                        rtn = translation;
                    }
                    catch (Exception ex)
                    {
                        rtn = "번역 실패";
                    }
                }
            }
            return rtn;
        }

        private void BtnViewAttributeOriginal_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (dgCategoryList.SelectedRows.Count > 0)
            {
                string strCategoryId = dgCategoryList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();
                string tarCountry = dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_country"].Value.ToString();
                GetTarAttributeData(Convert.ToInt32(strCategoryId), false, tarCountry);
            }

            Cursor.Current = Cursors.Default;
        }

        private void BtnSelectImageFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = "jpg";
            openFileDlg.Filter = "Image Files(*.JPG;*.PNG)|*.JPG;*.PNG|All files (*.*)|*.*";
            openFileDlg.Multiselect = true;
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                //DGSelectedList.Rows.Clear();
                if (!Directory.Exists(ImagePathThumb))
                {
                    Directory.CreateDirectory(ImagePathThumb);
                }
                int startIdx = DGSelectedList.Rows.Count;
                WebClient wc = new WebClient();
                for (int i = 0; i < openFileDlg.FileNames.Length; i++)
                {
                    string fileName = Path.GetFileName(openFileDlg.FileNames[i]);
                    string fileExt = Path.GetExtension(openFileDlg.FileNames[i]);
                    string destPath = ImagePathThumb + @"\" + $"thumb_{startIdx + 1:000}" + fileExt;

                    File.Copy(openFileDlg.FileNames[i], destPath, true);
                    var content = wc.DownloadData(destPath);
                    using (var stream = new MemoryStream(content))
                    {
                        Image im = Image.FromStream(stream);
                        DGSelectedList.Rows.Add(startIdx + 1, false, im, $"thumb_{startIdx + 1:000}.jpg", im.Width, im.Height,
                            destPath);
                    }
                    startIdx++;
                }

                lblThumb.Text = "기본 썸네일 [ " + DGSelectedList.Rows.Count + " ]";
            }
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        private void BtnImageClear_Click(object sender, EventArgs e)
        {
            if (DGSelectedList.Rows.Count == 0)
            {
                return;
            }
            DialogResult dlg_Result = MessageBox.Show("체크한 기본 썸네일 이미지를 삭제 하시겠습니까?", "썸네일 이미지 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                List<string> FileList = new List<string>();
                for (int i = DGSelectedList.Rows.Count - 1; i >= 0; i--)
                {
                    if ((bool)DGSelectedList.Rows[i].Cells["DGSelectedList_Chk"].Value)
                    {
                        FileList.Add(DGSelectedList.Rows[i].Cells["DGSelectedList_FileName"].Value.ToString());
                        DGSelectedList.Rows.RemoveAt(i);
                    }
                }

                reIndexNo(DGSelectedList);
                //파일을 지운다.
                for (int i = 0; i < FileList.Count; i++)
                {
                    if (File.Exists(ImagePath + @"\" + FileList[i].ToString()))
                    {
                        File.Delete(ImagePath + @"\" + FileList[i].ToString());
                    }
                }
            }

            lblThumb.Text = "기본 썸네일 [ " + DGSelectedList.Rows.Count + " ]";
        }

        private void BtnCategorySearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string tarCountry = dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_country"].Value.ToString();
            string keyWord = TxtCategorySearchText.Text.Trim().ToUpper();

            string endPoint = $"https://shopeecategory.azurewebsites.net/api/ShopeeCategory?CountryCode={tarCountry}";
            var request = new RestRequest("", Method.GET);
            request.Method = Method.GET;
            var client = new RestClient(endPoint);
            IRestResponse response = client.Execute(request);
            List<APIShopeeCategory> lstShopeeCategory = new List<APIShopeeCategory>();
            var result = JsonConvert.DeserializeObject<List<APIShopeeCategory>>(response.Content);

            List<APIShopeeCategory> query = (from shopeeCategory in result
                                            where (shopeeCategory.Category1Name ?? string.Empty).ToUpper().Contains(keyWord)
                                            || (shopeeCategory.Category2Name ?? string.Empty).ToUpper().Contains(keyWord)
                                            || (shopeeCategory.Category3Name ?? string.Empty).ToUpper().Contains(keyWord)
                                            select shopeeCategory).ToList();

            dgCategoryList.Rows.Clear();

            for (int i = 0; i < query.Count(); i++)
            {
                dgCategoryList.Rows.Add(i + 1,
                    query[i].Category1Name,
                    query[i].Category2Name,
                    query[i].Category3Name,
                    query[i].LastCategoryId);
            }

            dgCategoryList.ClearSelection();

            Cursor.Current = Cursors.Default;
        }

        private void TxtCategorySearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnCategorySearch_Click(null, null);
            }
        }

        private void BtnTitleTypeAdd_Click(object sender, EventArgs e)
        {
            if (TxtTitleType.Text.Trim() != string.Empty)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    var result = context.TitleTypes
                        .SingleOrDefault(x => x.TitleTypeName.ToUpper() == TxtTitleType.Text.Trim().ToUpper() && x.UserId == global_var.userId);

                    if (result == null)
                    {
                        TitleType newData = new TitleType
                        {
                            TitleTypeName = TxtTitleType.Text.Trim(),
                            UserId = global_var.userId
                        };
                        context.TitleTypes.Add(newData);
                        context.SaveChanges();
                    }
                    TxtTitleType.Text = "";
                    getTitleTypeList();
                    if (dgTitle_type.Rows.Count > 0)
                    {
                        int TypeId = Convert.ToInt32(dgTitle_type.Rows[0].Cells["dgTitle_type_id"].Value.ToString());
                        getListBrand(TypeId);

                        if (dgBrand.Rows.Count > 0)
                        {
                            int BrandId = Convert.ToInt32(dgBrand.Rows[0].Cells["dgBrand_id"].Value.ToString());
                            getListModel(BrandId);
                        }

                        getListGroup(TypeId);
                        getListFeature(TypeId);
                        getListOption(TypeId);
                        getListRelat(TypeId);
                    }
                }
            }
        }

        private void BtnBrandAdd_Click(object sender, EventArgs e)
        {
            if (TxtBrand.Text.Trim() != string.Empty && dgTitle_type.Rows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    var result = context.TitleBrands
                        .SingleOrDefault(x => x.TitleTypeId == TypeId && x.TitleBrandName.ToUpper() == TxtBrand.Text.Trim().ToUpper() && x.UserId == global_var.userId);

                    if (result == null)
                    {
                        TitleBrand newData = new TitleBrand
                        {
                            TitleBrandName = TxtBrand.Text.Trim(),
                            TitleTypeId = TypeId,
                            UserId = global_var.userId
                        };
                        context.TitleBrands.Add(newData);
                        context.SaveChanges();
                    }
                    TxtBrand.Text = "";
                    getListBrand(TypeId);
                }
            }
            else
            {
                MessageBox.Show("선택한 상품명 템플릿 종류가 없습니다.", "상품명 템플릿 종류 없음", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgTitle_type_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TxtTitleType.Text = dgTitle_type.SelectedRows[0].Cells["dgTitle_type_Name"].Value.ToString();
            int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
            getListBrand(TypeId);
            dgModel.Rows.Clear();
            if (dgBrand.Rows.Count > 0)
            {
                int BrandId = Convert.ToInt32(dgBrand.SelectedRows[0].Cells["dgBrand_id"].Value.ToString());
                getListModel(BrandId);
            }


            getListGroup(TypeId);
            getListFeature(TypeId);
            getListOption(TypeId);
            getListRelat(TypeId);
        }

        private void BtnModelAdd_Click(object sender, EventArgs e)
        {
            if (dgBrand.Rows.Count == 0 || dgBrand.SelectedRows.Count == 0)
            {
                return;
            }

            if (TxtModel.Text.Trim() != string.Empty && dgTitle_type.Rows.Count > 0)
            {
                int BrandId = Convert.ToInt32(dgBrand.SelectedRows[0].Cells["dgBrand_id"].Value.ToString());

                using (AppDbContext context = new AppDbContext())
                {
                    var result = context.TitleModels
                        .SingleOrDefault(x => x.TitleModelName.ToUpper() == TxtModel.Text.Trim().ToUpper() &&
                        x.TitleBrandId == BrandId && x.UserId == global_var.userId);

                    if (result == null)
                    {
                        TitleModel newData = new TitleModel
                        {
                            TitleModelName = TxtModel.Text.Trim(),
                            TitleBrandId = BrandId,
                            UserId = global_var.userId
                        };
                        context.TitleModels.Add(newData);
                        context.SaveChanges();
                    }
                    TxtModel.Text = "";
                    getListModel(BrandId);
                }
            }
        }

        private void BtnGroupAdd_Click(object sender, EventArgs e)
        {
            if (TxtGroup.Text.Trim() != string.Empty && dgTitle_type.Rows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    var result = context.TitleGroups
                        .SingleOrDefault(x => x.TitleTypeId == TypeId && x.TitleGroupName.ToUpper() == TxtGroup.Text.Trim().ToUpper() && x.UserId == global_var.userId);

                    if (result == null)
                    {
                        TitleGroup newData = new TitleGroup
                        {
                            TitleGroupName = TxtGroup.Text.Trim(),
                            TitleTypeId = TypeId,
                            UserId = global_var.userId
                        };
                        context.TitleGroups.Add(newData);
                        context.SaveChanges();
                    }
                    TxtGroup.Text = "";
                    getListGroup(TypeId);
                }
            }
        }

        private void BtnFeatureAdd_Click(object sender, EventArgs e)
        {
            if (TxtFeature.Text.Trim() != string.Empty && dgTitle_type.Rows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    var result = context.TitleFeatures
                        .SingleOrDefault(x => x.TitleTypeId == TypeId && x.TitleFeatureName.ToUpper() == TxtFeature.Text.Trim().ToUpper() && x.UserId == global_var.userId);

                    if (result == null)
                    {
                        TitleFeature newData = new TitleFeature
                        {
                            TitleFeatureName = TxtFeature.Text.Trim(),
                            TitleTypeId = TypeId,
                            UserId = global_var.userId
                        };
                        context.TitleFeatures.Add(newData);
                        context.SaveChanges();
                    }
                    TxtFeature.Text = "";
                    getListFeature(TypeId);
                }
            }
        }

        private void BtnOptionAdd_Click(object sender, EventArgs e)
        {
            if (TxtOption.Text.Trim() != string.Empty && dgTitle_type.Rows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    var result = context.TitleOptions
                        .SingleOrDefault(x => x.TitleTypeId == TypeId && x.TitleOptionName.ToUpper() == TxtOption.Text.Trim().ToUpper() && x.UserId == global_var.userId);

                    if (result == null)
                    {
                        TitleOption newData = new TitleOption
                        {
                            TitleOptionName = TxtOption.Text.Trim(),
                            TitleTypeId = TypeId,
                            UserId = global_var.userId
                        };
                        context.TitleOptions.Add(newData);
                        context.SaveChanges();
                    }
                    TxtOption.Text = "";
                    getListOption(TypeId);
                }
            }
        }

        private void BtnRelatAdd_Click(object sender, EventArgs e)
        {
            if (TxtRelat.Text.Trim() != string.Empty && dgTitle_type.Rows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    var result = context.TitleRelats
                        .SingleOrDefault(x => x.TitleTypeId == TypeId && x.TitleRelatName.ToUpper() == TxtRelat.Text.Trim().ToUpper() && x.UserId == global_var.userId);

                    if (result == null)
                    {
                        TitleRelat newData = new TitleRelat
                        {
                            TitleRelatName = TxtRelat.Text.Trim(),
                            TitleTypeId = TypeId,
                            UserId = global_var.userId
                        };
                        context.TitleRelats.Add(newData);
                        context.SaveChanges();
                    }
                    TxtRelat.Text = "";
                    getListRelat(TypeId);
                }
            }
        }

        private void BtnBrandDel_Click(object sender, EventArgs e)
        {
            if (dgBrand.Rows.Count > 0 && dgBrand.SelectedRows.Count > 0)
            {
                DialogResult dlg_Result = MessageBox.Show("체크한 브랜드목록을 삭제하시겠습니까?", "브랜드 목록 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg_Result == DialogResult.Yes)
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    for (int i = 0; i < dgBrand.Rows.Count; i++)
                    {
                        if (dgBrand.Rows[i].Cells["dgBrand_Chk"].Value.ToString() == "True")
                        {
                            using (AppDbContext context = new AppDbContext())
                            {

                                int BrandId = Convert.ToInt32(dgBrand.Rows[i].Cells["dgBrand_id"].Value.ToString());
                                var result = context.TitleBrands
                                    .SingleOrDefault(x => x.Id == BrandId
                                    && x.UserId == global_var.userId);

                                if (result != null)
                                {
                                    context.TitleBrands.Remove(result);
                                    context.SaveChanges();
                                }
                            }
                        }

                    }
                    dgModel.Rows.Clear();
                    getListBrand(TypeId);
                    TxtBrand.Text = "";
                }
            }
        }

        private void BtnModelDel_Click(object sender, EventArgs e)
        {
            if (dgBrand.Rows.Count == 0 || dgBrand.SelectedRows.Count == 0)
            {
                return;
            }


            if (dgModel.Rows.Count > 0 && dgModel.SelectedRows.Count > 0)
            {
                DialogResult dlg_Result = MessageBox.Show("체크한 모델목록을 삭제하시겠습니까?", "모델 목록 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg_Result == DialogResult.Yes)
                {
                    int BrandId = Convert.ToInt32(dgBrand.SelectedRows[0].Cells["dgBrand_id"].Value.ToString());
                    for (int i = 0; i < dgModel.Rows.Count; i++)
                    {
                        if (dgModel.Rows[i].Cells["dgModel_Chk"].Value.ToString() == "True")
                        {
                            using (AppDbContext context = new AppDbContext())
                            {

                                int ModelId = Convert.ToInt32(dgModel.Rows[i].Cells["dgModel_id"].Value.ToString());
                                var result = context.TitleModels
                                    .SingleOrDefault(x => x.Id == ModelId
                                    && x.UserId == global_var.userId);

                                if (result != null)
                                {
                                    context.TitleModels.Remove(result);
                                    context.SaveChanges();
                                }
                            }
                        }

                    }

                    getListModel(BrandId);
                    TxtModel.Text = "";
                }
            }
        }

        private void BtnGroupDel_Click(object sender, EventArgs e)
        {
            if (dgGroup.Rows.Count > 0 && dgGroup.SelectedRows.Count > 0)
            {
                DialogResult dlg_Result = MessageBox.Show("체크한 상품군 목록을 삭제하시겠습니까?", "상품군 목록 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg_Result == DialogResult.Yes)
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    for (int i = 0; i < dgGroup.Rows.Count; i++)
                    {
                        if (dgGroup.Rows[i].Cells["dgGroup_Chk"].Value.ToString() == "True")
                        {
                            using (AppDbContext context = new AppDbContext())
                            {

                                int GroupId = Convert.ToInt32(dgGroup.Rows[i].Cells["dgGroup_id"].Value.ToString());
                                var result = context.TitleGroups
                                    .SingleOrDefault(x => x.Id == GroupId
                                    && x.UserId == global_var.userId);

                                if (result != null)
                                {
                                    context.TitleGroups.Remove(result);
                                    context.SaveChanges();
                                }
                            }
                        }

                    }

                    getListGroup(TypeId);
                    TxtGroup.Text = "";
                }
            }
        }

        private void BtnFeatureDel_Click(object sender, EventArgs e)
        {
            if (dgFeature.Rows.Count > 0 && dgFeature.SelectedRows.Count > 0)
            {
                DialogResult dlg_Result = MessageBox.Show("체크한 상품군 목록을 삭제하시겠습니까?", "상품군 목록 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg_Result == DialogResult.Yes)
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    for (int i = 0; i < dgFeature.Rows.Count; i++)
                    {
                        if (dgFeature.Rows[i].Cells["dgFeature_Chk"].Value.ToString() == "True")
                        {
                            using (AppDbContext context = new AppDbContext())
                            {

                                int FeatureId = Convert.ToInt32(dgFeature.Rows[i].Cells["dgFeature_id"].Value.ToString());
                                var result = context.TitleFeatures
                                    .SingleOrDefault(x => x.Id == FeatureId
                                    && x.UserId == global_var.userId);

                                if (result != null)
                                {
                                    context.TitleFeatures.Remove(result);
                                    context.SaveChanges();
                                }
                            }
                        }

                    }

                    getListFeature(TypeId);
                    TxtFeature.Text = "";
                }
            }
        }

        private void BtnOptionDel_Click(object sender, EventArgs e)
        {
            if (dgOption.Rows.Count > 0 && dgOption.SelectedRows.Count > 0)
            {
                DialogResult dlg_Result = MessageBox.Show("체크한 상품특징 목록을 삭제하시겠습니까?", "상품특징 목록 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg_Result == DialogResult.Yes)
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    for (int i = 0; i < dgOption.Rows.Count; i++)
                    {
                        if (dgOption.Rows[i].Cells["dgOption_Chk"].Value.ToString() == "True")
                        {
                            using (AppDbContext context = new AppDbContext())
                            {

                                int OptionId = Convert.ToInt32(dgOption.Rows[i].Cells["dgOption_id"].Value.ToString());
                                var result = context.TitleOptions
                                    .SingleOrDefault(x => x.Id == OptionId
                                    && x.UserId == global_var.userId);

                                if (result != null)
                                {
                                    context.TitleOptions.Remove(result);
                                    context.SaveChanges();
                                }
                            }
                        }

                    }

                    getListOption(TypeId);
                    TxtOption.Text = "";
                }
            }
        }

        private void BtnRelatDel_Click(object sender, EventArgs e)
        {
            if (dgRelat.Rows.Count > 0 && dgRelat.SelectedRows.Count > 0)
            {
                DialogResult dlg_Result = MessageBox.Show("체크한 연관검색어 목록을 삭제하시겠습니까?", "연관검색어 목록 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg_Result == DialogResult.Yes)
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    for (int i = 0; i < dgRelat.Rows.Count; i++)
                    {
                        if (dgRelat.Rows[i].Cells["dgRelat_Chk"].Value.ToString() == "True")
                        {
                            using (AppDbContext context = new AppDbContext())
                            {

                                int RelatId = Convert.ToInt32(dgRelat.Rows[i].Cells["dgRelat_id"].Value.ToString());
                                var result = context.TitleRelats
                                    .SingleOrDefault(x => x.Id == RelatId
                                    && x.UserId == global_var.userId);

                                if (result != null)
                                {
                                    context.TitleRelats.Remove(result);
                                    context.SaveChanges();
                                }
                            }
                        }

                    }

                    getListRelat(TypeId);
                    TxtRelat.Text = "";
                }
            }
        }

        private void BtnTitleTypeDel_Click(object sender, EventArgs e)
        {
            if (dgTitle_type.Rows.Count > 0 && dgTitle_type.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    var result = context.TitleTypes
                        .SingleOrDefault(x => x.Id == TypeId
                        && x.UserId == global_var.userId);

                    if (result != null)
                    {
                        context.TitleTypes.Remove(result);
                        context.SaveChanges();
                    }

                    TxtTitleType.Text = "";
                    getTitleTypeList();
                    if (dgTitle_type.Rows.Count > 0)
                    {
                        int SelectedTypeId = Convert.ToInt32(dgTitle_type.Rows[0].Cells["dgTitle_type_id"].Value.ToString());
                        getSubList(SelectedTypeId);
                    }
                }
            }
        }

        private void BtnBrandUpdate_Click(object sender, EventArgs e)
        {
            if (dgBrand.Rows.Count > 0 && dgBrand.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    int BrandId = Convert.ToInt32(dgBrand.SelectedRows[0].Cells["dgBrand_id"].Value.ToString());
                    var result = context.TitleBrands
                        .SingleOrDefault(x => x.Id == BrandId && x.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.TitleBrandName = TxtBrand.Text.Trim();
                        context.SaveChanges();
                    }

                    getListBrand(TypeId);
                }
            }
        }

        private void BtnModelUpdate_Click(object sender, EventArgs e)
        {
            if (dgBrand.Rows.Count == 0 || dgBrand.SelectedRows.Count == 0)
            {
                return;
            }

            if (dgModel.Rows.Count > 0 && dgModel.SelectedRows.Count > 0)
            {
                int BrandId = Convert.ToInt32(dgBrand.SelectedRows[0].Cells["dgBrand_id"].Value.ToString());

                using (AppDbContext context = new AppDbContext())
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    int ModelId = Convert.ToInt32(dgModel.SelectedRows[0].Cells["dgModel_id"].Value.ToString());
                    var result = context.TitleModels
                        .SingleOrDefault(x => x.Id == ModelId && x.TitleBrandId == BrandId && x.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.TitleModelName = TxtModel.Text.Trim();
                        context.SaveChanges();
                    }

                    getListModel(BrandId);
                }
            }
        }

        private void BtnGroupUpdate_Click(object sender, EventArgs e)
        {
            if (dgGroup.Rows.Count > 0 && dgGroup.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    int GroupId = Convert.ToInt32(dgGroup.SelectedRows[0].Cells["dgGroup_id"].Value.ToString());
                    var result = context.TitleGroups
                        .SingleOrDefault(x => x.Id == GroupId && x.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.TitleGroupName = TxtGroup.Text.Trim();
                        context.SaveChanges();
                    }

                    getListGroup(TypeId);
                }
            }
        }

        private void BtnFeatureUpdate_Click(object sender, EventArgs e)
        {
            if (dgFeature.Rows.Count > 0 && dgFeature.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    int FeatureId = Convert.ToInt32(dgFeature.SelectedRows[0].Cells["dgFeature_id"].Value.ToString());
                    var result = context.TitleFeatures
                        .SingleOrDefault(x => x.Id == FeatureId && x.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.TitleFeatureName = TxtFeature.Text.Trim();
                        context.SaveChanges();
                    }

                    getListFeature(TypeId);
                }
            }
        }

        private void BtnOptionUpdate_Click(object sender, EventArgs e)
        {
            if (dgOption.Rows.Count > 0 && dgOption.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    int OptionId = Convert.ToInt32(dgOption.SelectedRows[0].Cells["dgOption_id"].Value.ToString());
                    var result = context.TitleOptions
                        .SingleOrDefault(x => x.Id == OptionId && x.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.TitleOptionName = TxtOption.Text.Trim();
                        context.SaveChanges();
                    }

                    getListOption(TypeId);
                }
            }
        }

        private void BtnRelatUpdate_Click(object sender, EventArgs e)
        {
            if (dgRelat.Rows.Count > 0 && dgRelat.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    int RelatId = Convert.ToInt32(dgRelat.SelectedRows[0].Cells["dgRelat_id"].Value.ToString());
                    var result = context.TitleRelats
                        .SingleOrDefault(x => x.Id == RelatId && x.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.TitleRelatName = TxtRelat.Text.Trim();
                        context.SaveChanges();
                    }

                    getListRelat(TypeId);
                }
            }
        }

        private void dgBrand_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgBrand.Rows.Count == 0 || dgBrand.SelectedRows.Count == 0)
            {
                return;
            }

            if (e.RowIndex == -1 && e.ColumnIndex == 2)
            {
                bool val = (bool)dgBrand.Rows[0].Cells[2].Value;
                for (int i = 0; i < dgBrand.Rows.Count; i++)
                {
                    dgBrand.Rows[i].Cells[2].Value = !val;
                }
            }

            TxtBrand.Text = dgBrand.SelectedRows[0].Cells["dgBrand_Name"].Value.ToString();
            int BrandId = Convert.ToInt32(dgBrand.SelectedRows[0].Cells["dgBrand_id"].Value.ToString());
            getListModel(BrandId);
        }

        private void dgModel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TxtModel.Text = dgModel.SelectedRows[0].Cells["dgModel_Name"].Value.ToString();

            if (e.RowIndex == -1 && e.ColumnIndex == 2)
            {
                bool val = (bool)dgModel.Rows[0].Cells[2].Value;
                for (int i = 0; i < dgModel.Rows.Count; i++)
                {
                    dgModel.Rows[i].Cells[2].Value = !val;
                }
            }
        }

        private void dgGroup_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TxtGroup.Text = dgGroup.SelectedRows[0].Cells["dgGroup_Name"].Value.ToString();

            if (e.RowIndex == -1 && e.ColumnIndex == 2)
            {
                bool val = (bool)dgGroup.Rows[0].Cells[2].Value;
                for (int i = 0; i < dgGroup.Rows.Count; i++)
                {
                    dgGroup.Rows[i].Cells[2].Value = !val;
                }
            }
        }

        private void dgFeature_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TxtFeature.Text = dgFeature.SelectedRows[0].Cells["dgFeature_Name"].Value.ToString();
            if (e.RowIndex == -1 && e.ColumnIndex == 2)
            {
                bool val = (bool)dgFeature.Rows[0].Cells[2].Value;
                for (int i = 0; i < dgFeature.Rows.Count; i++)
                {
                    dgFeature.Rows[i].Cells[2].Value = !val;
                }
            }
        }

        private void dgOption_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TxtOption.Text = dgOption.SelectedRows[0].Cells["dgOption_Name"].Value.ToString();
            if (e.RowIndex == -1 && e.ColumnIndex == 2)
            {
                bool val = (bool)dgOption.Rows[0].Cells[2].Value;
                for (int i = 0; i < dgOption.Rows.Count; i++)
                {
                    dgOption.Rows[i].Cells[2].Value = !val;
                }
            }
        }

        private void dgRelat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TxtRelat.Text = dgRelat.SelectedRows[0].Cells["dgRelat_Name"].Value.ToString();
            if (e.RowIndex == -1 && e.ColumnIndex == 2)
            {
                bool val = (bool)dgRelat.Rows[0].Cells[2].Value;
                for (int i = 0; i < dgRelat.Rows.Count; i++)
                {
                    dgRelat.Rows[i].Cells[2].Value = !val;
                }
            }
        }

        private void TabMain_Name_Click(object sender, EventArgs e)
        {

        }

        private void dgBrand_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgBrand.Rows.Count > 0 && dgBrand.SelectedRows.Count > 0 &&
                dg_productTitle.Rows.Count > 0 &&
                dg_productTitle.SelectedRows.Count > 0 &&
                dg_productTitle.SelectedRows[0].Cells["dg_productTitle_min_max"].Value != null)
            {
                int maxTitleLength = Convert.ToInt32(dg_productTitle.SelectedRows[0].Cells["dg_productTitle_min_max"].Value.ToString());
                
                if(chkCopy.Checked)
                {
                    for (int i = 0; i < dg_productTitle.Rows.Count; i++)
                    {
                        if(dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString().Trim().Length > 0)
                        {
                            dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value =
                            dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString()
                            + " [" + dgBrand.SelectedRows[0].Cells["dgBrand_Name"].Value.ToString().Trim() + "]";
                        }
                        else
                        {
                            dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value =
                            "[" + dgBrand.SelectedRows[0].Cells["dgBrand_Name"].Value.ToString().Trim() + "]";
                        }
                        
                    }
                }
                else
                {
                    if (dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString().Trim().Length > 0)
                    {
                        dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value =
                            dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString()
                            + " [" + dgBrand.SelectedRows[0].Cells["dgBrand_Name"].Value.ToString().Trim() + "]";
                    }
                    else
                    {
                        dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value =
                            "[" + dgBrand.SelectedRows[0].Cells["dgBrand_Name"].Value.ToString().Trim() + "]";
                    }   
                }
            }
        }

        private void BtnOpenGoogleTranslate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string siteUrl = "https://translate.google.com";
            System.Diagnostics.Process.Start("chrome.exe", siteUrl);
            Cursor.Current = Cursors.Default;
        }

        private void dgModel_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgModel.Rows.Count > 0 && dgModel.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dg_productTitle.Rows.Count; i++)
                {

                }
                    

                if(chkCopy.Checked)
                {
                    for (int i = 0; i < dg_productTitle.Rows.Count; i++)
                    {
                        string brandName = dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString();

                        var pattern = @"\[(.*?)\]";
                        var matches = Regex.Matches(brandName, pattern);

                        foreach (Match m in matches)
                        {
                            brandName = m.Groups[1].ToString();
                        }

                        if (brandName.Trim() == string.Empty)
                        {

                            if (dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString().Trim().Length > 0)
                            {
                                dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value =
                                dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString()
                                + " [" + dgBrand.SelectedRows[0].Cells["dgBrand_Name"].Value.ToString().Trim() + "]";

                                dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value =
                                    dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString()
                                    + " " + dgModel.SelectedRows[0].Cells["dgModel_Name"].Value.ToString().Trim();
                            }
                            else
                            {
                                dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value =
                                "[" + dgBrand.SelectedRows[0].Cells["dgBrand_Name"].Value.ToString().Trim() + "]";

                                dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value =
                                    dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString()
                                    + " " + dgModel.SelectedRows[0].Cells["dgModel_Name"].Value.ToString().Trim();
                            }
                        }
                        else
                        {
                            if (dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString().Trim().Length > 0)
                            {
                                dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value =
                                    dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString()
                                    + " " + dgModel.SelectedRows[0].Cells["dgModel_Name"].Value.ToString().Trim();
                            }
                            else
                            {
                                dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value =
                                    dgModel.SelectedRows[0].Cells["dgModel_Name"].Value.ToString().Trim();
                            }
                        }
                    }
                }
                else
                {
                    string brandName = dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString();

                    var pattern = @"\[(.*?)\]";
                    var matches = Regex.Matches(brandName, pattern);

                    foreach (Match m in matches)
                    {
                        brandName = m.Groups[1].ToString();
                    }

                    if (brandName.Trim() == string.Empty)
                    {
                        if (dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString().Trim().Length > 0)
                        {
                            dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value =
                                dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString()
                                + " [" + dgBrand.SelectedRows[0].Cells["dgBrand_Name"].Value.ToString().Trim() + "]";

                            dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value =
                                    dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString()
                                    + " " + dgModel.SelectedRows[0].Cells["dgModel_Name"].Value.ToString().Trim();
                        }
                        else
                        {
                            dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value =
                                "[" + dgBrand.SelectedRows[0].Cells["dgBrand_Name"].Value.ToString().Trim() + "]";

                            dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value =
                                    dgModel.SelectedRows[0].Cells["dgModel_Name"].Value.ToString().Trim();
                        }
                    }
                    else
                    {
                        if (dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString().Trim().Length > 0)
                        {
                            dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value =
                                    dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString()
                                    + " " + dgModel.SelectedRows[0].Cells["dgModel_Name"].Value.ToString().Trim();
                        }
                        else
                        {
                            dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value =
                                    dgModel.SelectedRows[0].Cells["dgModel_Name"].Value.ToString().Trim();
                        }
                    }   
                }
            }
        }

        private void dgGroup_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgGroup.Rows.Count > 0 && dgGroup.SelectedRows.Count > 0)
            {
                int maxTitleLength = Convert.ToInt32(dg_productTitle.SelectedRows[0].Cells["dg_productTitle_min_max"].Value.ToString());

                if (chkCopy.Checked)
                {
                    for (int i = 0; i < dg_productTitle.Rows.Count; i++)
                    {
                        dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value =
                            dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString()
                            + " " + dgGroup.SelectedRows[0].Cells["dgGroup_Name"].Value.ToString().Trim();
                    }
                }
                else
                {
                    dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value =
                            dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString()
                            + " " + dgGroup.SelectedRows[0].Cells["dgGroup_Name"].Value.ToString().Trim();
                }
            }
        }

        private void dgFeature_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgFeature.Rows.Count > 0 && dgFeature.SelectedRows.Count > 0)
            {
                int maxTitleLength = Convert.ToInt32(dg_productTitle.SelectedRows[0].Cells["dg_productTitle_min_max"].Value.ToString());

                if (chkCopy.Checked)
                {
                    for (int i = 0; i < dg_productTitle.Rows.Count; i++)
                    {
                        dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value =
                            dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString()
                            + " " + dgFeature.SelectedRows[0].Cells["dgFeature_Name"].Value.ToString().Trim();
                    }
                }
                else
                {
                    dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value =
                            dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString()
                            + " " + dgFeature.SelectedRows[0].Cells["dgFeature_Name"].Value.ToString().Trim();
                }
            }
        }

        private void dgOption_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgOption.Rows.Count > 0 && dgOption.SelectedRows.Count > 0)
            {
                int maxTitleLength = Convert.ToInt32(dg_productTitle.SelectedRows[0].Cells["dg_productTitle_min_max"].Value.ToString());

                if (chkCopy.Checked)
                {
                    for (int i = 0; i < dg_productTitle.Rows.Count; i++)
                    {
                        dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value =
                            dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString()
                            + " " + dgOption.SelectedRows[0].Cells["dgOption_Name"].Value.ToString().Trim();
                    }
                }
                else
                {
                    dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value =
                            dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString()
                            + " " + dgOption.SelectedRows[0].Cells["dgOption_Name"].Value.ToString().Trim();
                }
            }
        }

        private void dgRelat_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgRelat.Rows.Count > 0 && dgRelat.SelectedRows.Count > 0)
            {
                int maxTitleLength = Convert.ToInt32(dg_productTitle.SelectedRows[0].Cells["dg_productTitle_min_max"].Value.ToString());

                if (chkCopy.Checked)
                {
                    for (int i = 0; i < dg_productTitle.Rows.Count; i++)
                    {
                        dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value =
                            dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString()
                            + " " + dgRelat.SelectedRows[0].Cells["dgRelat_Name"].Value.ToString().Trim();
                    }
                }
                else
                {
                    dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value =
                            dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString()
                            + " " + dgRelat.SelectedRows[0].Cells["dgRelat_Name"].Value.ToString().Trim();
                }
            }
        }

        private void BtnClearTitle_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dg_productTitle.Rows.Count; i++)
            {
                dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value = "";
            }
        }

        private void BtnClearTitleKor_Click(object sender, EventArgs e)
        {
            TxtProductNameKor.Text = "";
        }

        private void BtnTitleTypeUpdate_Click(object sender, EventArgs e)
        {
            if (dgTitle_type.Rows.Count > 0 && dgTitle_type.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    var result = context.TitleTypes
                        .SingleOrDefault(x => x.Id == TypeId && x.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.TitleTypeName = TxtTitleType.Text.Trim();
                        context.SaveChanges();
                    }

                    getTitleTypeList();
                    if (dgTitle_type.Rows.Count > 0)
                    {
                        int SelectedTypeId = Convert.ToInt32(dgTitle_type.Rows[0].Cells["dgTitle_type_id"].Value.ToString());
                        getSubList(SelectedTypeId);
                    }
                }
            }
        }

        private void BtnTranslate_Click(object sender, EventArgs e)
        {
            if(dg_productTitle.Rows.Count == 0)
            {
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            string tar_lang = "";
            if (cboLang.Text == "인도네시아어")
            {
                tar_lang = "id";
            }
            else if (cboLang.Text == "영어")
            {
                tar_lang = "en";
            }
            else if (cboLang.Text == "말레이시아어")
            {
                tar_lang = "ms";
            }
            else if (cboLang.Text == "태국어")
            {
                tar_lang = "th";
            }
            else if (cboLang.Text == "중국어 번체")
            {
                tar_lang = "zh-TW";
            }
            else if (cboLang.Text == "베트남어")
            {
                tar_lang = "vi";
            }
            string translated = translate(TxtProductNameKor.Text.Trim(), "ko", tar_lang);
            int maxTitleLength = Convert.ToInt32(dg_productTitle.SelectedRows[0].Cells["dg_productTitle_min_max"].Value.ToString());
            if (chkCopy.Checked)
            {
                //무조건 추가 모두로 작동
                for (int i = 0; i < dg_productTitle.Rows.Count; i++)
                {
                    
                    if (translated.Length > maxTitleLength)
                    {
                        dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value = translated.Substring(0, maxTitleLength);
                    }
                    else
                    {
                        if(dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString().Trim().Length > 0)
                        {
                            dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value = translated;
                            //dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString() + " " + translated;
                        }
                        else
                        {
                            dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value = translated;
                        }

                        
                    }
                }
            }
            else
            {
                if (dg_productTitle.Rows.Count > 0 && dg_productTitle.SelectedRows.Count > 0)
                {
                    if (translated.Length > maxTitleLength)
                    {
                        //TxtProductName.Text = translated.Substring(0, maxTitleLength);
                        dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value = translated.Substring(0, maxTitleLength);
                    }
                    else
                    {
                        if (dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString().Trim().Length > 0)
                        {
                            dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value = translated;
                            //dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value.ToString() + " " + translated;
                        }
                        else
                        {
                            dg_productTitle.SelectedRows[0].Cells["dg_productTitle_title"].Value = translated;
                        }
                    }
                }
            }

            checkLength();
            Cursor.Current = Cursors.Default;
        }

        private void BtnOpenGoogleTranslate2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string title = TxtProductNameKor.Text.Trim();
            string siteUrl = "https://translate.google.com/#view=home&op=translate&sl=ko&tl=en&text=" + HttpUtility.UrlEncode(title);
            System.Diagnostics.Process.Start("chrome.exe", siteUrl);
            Cursor.Current = Cursors.Default;
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (dg_list_header_set.Rows.Count == 0)
            {
                chkUseHeader.Checked = false;
                return;
            }

            if (chkUseHeader.Checked)
            {
                dg_list_header_set.Enabled = true;
            }
            else
            {
                dg_list_header_set.Enabled = false;
            }

            refreshPreView();
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            if (dg_list_footer_set.Rows.Count == 0)
            {
                chkUseFooter.Checked = false;
                return;
            }

            if (chkUseFooter.Checked)
            {
                dg_list_footer_set.Enabled = true;
            }
            else
            {
                dg_list_footer_set.Enabled = false;
            }

            refreshPreView();
        }

        private void TxtProductDesc_TextChanged(object sender, EventArgs e)
        {
            lblDescLen.Text = TxtProductDesc.Text.Length + "자";
            refreshPreView();
        }


        private void refreshPreView()
        {
            StringBuilder sb_preview = new StringBuilder();
            if (chkUseHeader.Checked)
            {
                string strHeader = "";
                if (dg_list_header_set.Rows.Count > 0 && dg_list_header_set.SelectedRows.Count > 0)
                {
                    string headerSetId = dg_list_header_set.SelectedRows[0].Cells["dg_headerset_id"].Value?.ToString().Trim();
                    if (headerSetId != "0")
                    {
                        strHeader = FillHeaderSet(Convert.ToInt32(headerSetId));
                        sb_preview.Append(strHeader + "\r\n" + strHeaderSeparator + "\r\n");

                    }
                }
            }

            sb_preview.Append(TxtProductDesc.Text);

            if (chkUseFooter.Checked)
            {
                string strFooter = "";
                if (dg_list_footer_set.Rows.Count > 0 && dg_list_footer_set.SelectedRows.Count > 0)
                {
                    string footerSetId = dg_list_footer_set.SelectedRows[0].Cells["dg_footerset_id"].Value?.ToString().Trim();
                    if (footerSetId != "0")
                    {
                        strFooter = FillFooterSet(Convert.ToInt32(footerSetId));
                        sb_preview.Append("\r\n" + strFooterSeparator + "\r\n" + strFooter);
                    }
                }
            }

            txtPreview.Text = sb_preview.ToString();

            lblDescPrevLen.Text = string.Format("{0:n0}", txtPreview.Text.Length) + "자";
        }
        private string FillFooterSet(int footerSetId)
        {
            StringBuilder sbFooter = new StringBuilder();

            using (AppDbContext context = new AppDbContext())
            {
                var lst = (from template in context.TemplateFooters
                           join H in context.HFLists
                           on template.HFListID equals H.HFListID
                           where template.SetFooterId == footerSetId
                           && template.UserId == global_var.userId
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

        private string FillHeaderSet(int headerSetId)
        {
            StringBuilder sbHeader = new StringBuilder();

            using (AppDbContext context = new AppDbContext())
            {
                var lst = (from template in context.TemplateHeaders
                           join H in context.HFLists
                           on template.HFListID equals H.HFListID
                           where template.SetHeaderId == headerSetId
                           && template.UserId == global_var.userId
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

        private void dg_list_header_set_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            refreshPreView();
        }

        private void dg_list_footer_set_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            refreshPreView();
        }

        private void BtnTitleTypeSearch_Click(object sender, EventArgs e)
        {
            dgBrand.Rows.Clear();
            dgModel.Rows.Clear();
            dgGroup.Rows.Clear();
            dgFeature.Rows.Clear();
            dgOption.Rows.Clear();
            dgRelat.Rows.Clear();

            if (TxtTitleTypeSearch.Text.Trim() != string.Empty)
            {


                getTitleTypeList(TxtTitleTypeSearch.Text.Trim());
            }
            else
            {
                getTitleTypeList();
            }

            if (dgTitle_type.Rows.Count > 0)
            {
                TxtTitleType.Text = dgTitle_type.Rows[0].Cells["dgTitle_type_Name"].Value.ToString();
                int TypeId = Convert.ToInt32(dgTitle_type.Rows[0].Cells["dgTitle_type_id"].Value.ToString());
                getSubList(TypeId);
            }
        }

        private void TxtTitleTypeSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnTitleTypeSearch_Click(null, null);
            }
        }

        private void BtnBrandSearch_Click(object sender, EventArgs e)
        {
            if (dgTitle_type.Rows.Count > 0 && dgTitle_type.SelectedRows.Count > 0)
            {
                if (TxtBrandSearch.Text.Trim() != string.Empty)
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    getListBrand(TypeId, TxtBrandSearch.Text.Trim());
                }
                else
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    getListBrand(TypeId);
                }
            }
        }

        private void TxtBrandSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnBrandSearch_Click(null, null);
            }
        }

        private void BtnModelSearch_Click(object sender, EventArgs e)
        {
            if (dgBrand.Rows.Count == 0 || dgBrand.SelectedRows.Count == 0)
            {
                return;
            }

            if (dgTitle_type.Rows.Count > 0 && dgTitle_type.SelectedRows.Count > 0)
            {
                int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                int BrandId = Convert.ToInt32(dgBrand.SelectedRows[0].Cells["dgBrand_id"].Value.ToString());

                if (TxtModelSearch.Text.Trim() != string.Empty)
                {

                    getListModel(TypeId, BrandId, TxtModelSearch.Text.Trim());
                }
                else
                {
                    getListModel(BrandId);
                }
            }
        }
        private void getListModel(int TitleTypeId, int BrandId, string ModelName)
        {
            dgModel.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleModel> lstModel = context.TitleModels
                    .Where(x => x.TitleModelName.ToUpper().Contains(ModelName)
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleModelName).ToList();

                for (int i = 0; i < lstModel.Count; i++)
                {
                    dgModel.Rows.Add(lstModel[i].Id, i + 1, false, lstModel[i].TitleModelName);
                }
            }
        }

        private void getListGroup(int TitleTypeId, string GroupName)
        {
            dgGroup.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleGroup> lstGroup = context.TitleGroups
                    .Where(x => x.TitleTypeId == TitleTypeId &&
                    x.TitleGroupName.ToUpper().Contains(GroupName)
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleGroupName).ToList();

                for (int i = 0; i < lstGroup.Count; i++)
                {
                    dgGroup.Rows.Add(lstGroup[i].Id, i + 1, false, lstGroup[i].TitleGroupName);
                }
            }
        }

        private void getListFeature(int TitleTypeId, string FeatureName)
        {
            dgFeature.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleFeature> lstFeature = context.TitleFeatures
                    .Where(x => x.TitleTypeId == TitleTypeId &&
                    x.TitleFeatureName.ToUpper().Contains(FeatureName)
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleFeatureName).ToList();

                for (int i = 0; i < lstFeature.Count; i++)
                {
                    dgFeature.Rows.Add(lstFeature[i].Id, i + 1, false, lstFeature[i].TitleFeatureName);
                }
            }
        }
        private void getListOption(int TitleTypeId, string OptionName)
        {
            dgOption.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleOption> lstOption = context.TitleOptions
                    .Where(x => x.TitleTypeId == TitleTypeId &&
                    x.TitleOptionName.ToUpper().Contains(OptionName)
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleOptionName).ToList();

                for (int i = 0; i < lstOption.Count; i++)
                {
                    dgOption.Rows.Add(lstOption[i].Id, i + 1, false, lstOption[i].TitleOptionName);
                }
            }
        }

        private void getListRelat(int TitleTypeId, string RelatName)
        {
            dgRelat.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<TitleRelat> lstRelat = context.TitleRelats
                    .Where(x => x.TitleTypeId == TitleTypeId &&
                    x.TitleRelatName.ToUpper().Contains(RelatName)
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.TitleRelatName).ToList();

                for (int i = 0; i < lstRelat.Count; i++)
                {
                    dgRelat.Rows.Add(lstRelat[i].Id, i + 1, false, lstRelat[i].TitleRelatName);
                }
            }
        }

        private void BtnGroupSearch_Click(object sender, EventArgs e)
        {
            if (dgTitle_type.Rows.Count > 0 && dgTitle_type.SelectedRows.Count > 0)
            {
                if (TxtGroupSearch.Text.Trim() != string.Empty)
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    getListGroup(TypeId, TxtGroupSearch.Text.Trim());
                }
                else
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    getListGroup(TypeId);
                }
            }
        }

        private void BtnFeatureSearch_Click(object sender, EventArgs e)
        {
            if (dgTitle_type.Rows.Count > 0 && dgTitle_type.SelectedRows.Count > 0)
            {
                if (TxtFeatureSearch.Text.Trim() != string.Empty)
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    getListFeature(TypeId, TxtFeatureSearch.Text.Trim());
                }
                else
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    getListFeature(TypeId);
                }
            }
        }

        private void BtnOptionSearch_Click(object sender, EventArgs e)
        {
            if (dgTitle_type.Rows.Count > 0 && dgTitle_type.SelectedRows.Count > 0)
            {
                if (TxtOptionSearch.Text.Trim() != string.Empty)
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    getListOption(TypeId, TxtOptionSearch.Text.Trim());
                }
                else
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    getListOption(TypeId);
                }
            }
        }

        private void BtnRelatSearch_Click(object sender, EventArgs e)
        {
            if (dgTitle_type.Rows.Count > 0 && dgTitle_type.SelectedRows.Count > 0)
            {
                if (TxtRelatSearch.Text.Trim() != string.Empty)
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    getListRelat(TypeId, TxtRelatSearch.Text.Trim());
                }
                else
                {
                    int TypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    getListRelat(TypeId);
                }
            }
        }

        private void TxtModelSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnModelSearch_Click(null, null);
            }
        }

        private void TxtGroupSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnGroupSearch_Click(null, null);
            }
        }

        private void TxtFeatureSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnFeatureSearch_Click(null, null);
            }
        }

        private void TxtOptionSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnOptionSearch_Click(null, null);
            }
        }

        private void TxtRelatSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnRelatSearch_Click(null, null);
            }
        }

        private void PicGet_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void FromAddProduct_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }
        private void ScrapAmoremall(string ProductUrl)
        {
            if (ProductUrl.Contains("amorepacificmall.com/kr/ko/product/detail?"))
            {
                Cursor.Current = Cursors.WaitCursor;
                DGSelectedList.Rows.Clear();
                DGDetailList.Rows.Clear();
                DGImageSlicedList.Rows.Clear();
                DgVariation.Rows.Clear();

                string Html = GetHtml(ProductUrl);
                var myDoc = new HtmlAgilityPack.HtmlDocument();
                myDoc.LoadHtml(Html);

                //상품 제목
                var metaObj = myDoc.DocumentNode.SelectSingleNode("//div[@class='product_name']");
                if (metaObj != null)
                {   
                    TxtProductNameKor.Text = metaObj.InnerText.Trim();
                }

                //상품 가격
                var price = myDoc.DocumentNode.SelectSingleNode("//div[@class='product_name']");
                if(price != null)
                {

                }

                var thumbList = myDoc.DocumentNode.SelectNodes("//div[@class='product_visual prd_img_wrap']/div/div/ul//img");
                if(thumbList != null)
                {

                    int imgIdx = 0;
                    //메인이미지 1장

                    if (!Directory.Exists(ImagePath))
                    {
                        Directory.CreateDirectory(ImagePath);
                        Directory.CreateDirectory(ImagePathThumb);
                    }
                    else
                    {
                        //디렉토리를 비운다.
                        DirectoryInfo di = new DirectoryInfo(ImagePath);
                        di.Delete(true);

                        Directory.CreateDirectory(ImagePath);
                        Directory.CreateDirectory(ImagePathThumb);
                    }
                    //썸네일 이미지
                    if (thumbList.Count > 0)
                    {
                        for (int i = 0; i < thumbList.Count; i++)
                        {
                            if(thumbList[i].Attributes.Count == 4)
                            {
                                imgIdx++;
                                if(!thumbList[i].Attributes["src"].Value.ToString().Contains("img_loading_prd.png"))
                                {
                                    var request_Thumb = WebRequest.Create(thumbList[i].Attributes["src"].Value.ToString());
                                    var response_Thumb = (HttpWebResponse)request_Thumb.GetResponse();
                                    var dataStream_Thumb = response_Thumb.GetResponseStream();
                                    Bitmap bm_Thumb = new Bitmap(dataStream_Thumb);

                                    DGSelectedList.Rows.Add(DGSelectedList.Rows.Count + 1, false, bm_Thumb,
                                        $"thumb_{imgIdx:000}.jpg",
                                    string.Format("{0:n0}", bm_Thumb.Width),
                                    string.Format("{0:n0}", bm_Thumb.Height),
                                    ImagePathThumb + $"thumb_{imgIdx:000}.jpg");

                                    using (var b = new Bitmap(bm_Thumb.Width, bm_Thumb.Height))
                                    {

                                        b.SetResolution(bm_Thumb.HorizontalResolution, bm_Thumb.VerticalResolution);

                                        using (var g = Graphics.FromImage(b))
                                        {
                                            g.Clear(Color.White);
                                            g.DrawImageUnscaled(bm_Thumb, 0, 0);
                                            b.Save(ImagePathThumb + $@"thumb_{imgIdx:000}.jpg", ImageFormat.Jpeg);
                                        }
                                    }
                                }   
                            }
                        }
                    }
                }

                var listDetail = myDoc.DocumentNode.SelectNodes("//div[@class='detail_img']//img");
                for (int i = 0; i < listDetail.Count; i++)
                {
                    var request_Detail = WebRequest.Create(listDetail[i].Attributes["src"].Value.ToString().Trim());
                    var response_Detail = (HttpWebResponse)request_Detail.GetResponse();
                    var dataStream_Detail = response_Detail.GetResponseStream();
                    Bitmap bm_Detail = new Bitmap(dataStream_Detail);

                    DGDetailList.Rows.Add(DGDetailList.Rows.Count + 1, false, bm_Detail,
                    Path.GetFileName(HttpUtility.UrlDecode(listDetail[i].Attributes["src"].Value.ToString().Trim())),
                    string.Format("{0:n0}", bm_Detail.Width),
                    string.Format("{0:n0}", bm_Detail.Height));
                }

                var basePrice = myDoc.DocumentNode.SelectSingleNode("//span[@class='price']/span");
                for (int prc = 0; prc < DgPrice.Rows.Count; prc++)
                {
                    DgPrice.Rows[prc].Cells["DgPrice_source_price"].Value = basePrice.InnerText.Trim();
                    DgPrice.Rows[prc].Cells["DgPrice_margin"].Value = string.Format("{0:n0}", UdMargin.Value);
                    DgPrice.Rows[prc].Cells["DgPrice_qty"].Value = string.Format("{0:n0}", UdQty.Value);
                }
                

                int startIdx = Html.IndexOf("model = ") + 8;
                //int endIdx = Html.IndexOf("model.isLimitDc");
                int endIdx = Html.IndexOf("/* 구매하기후 에러처리");
                string temp = Html.Substring(startIdx, endIdx - startIdx).Trim();
                int lastSemiColonIdx = temp.LastIndexOf(';');
                string temp2 = temp.Substring(0, lastSemiColonIdx);
                dynamic jOption = JsonConvert.DeserializeObject(temp2);
                var onlyOption = jOption.products;

                if(onlyOption.Count > 0)
                {
                    Rd_None_Variation1.Checked = true;
                    DgVariation.Enabled = true;
                }
                else
                {
                    Rd_None_Variation1.Checked = false;
                    DgVariation.Enabled = false;
                    DgVariation.Rows.Clear();
                }
                for (int j = 0; j < DgPrice.Rows.Count; j++)
                {
                    string country = DgPrice.Rows[j].Cells["DgPrice_tar_country"].Value.ToString();
                    string shopeeid = DgPrice.Rows[j].Cells["DgPrice_shopee_id"].Value.ToString();

                    for (int i = 0; i < onlyOption.Count; i++)
                    {
                        var priceObj = Convert.ToInt32(onlyOption[i].availablePrices[0].finalOnlinePrice.Value.ToString());

                        DgVariation.Rows.Add(DgVariation.Rows.Count + 1,
                            country,
                            shopeeid, 
                            false,
                            "상태",
                            "",
                            "",
                            jOption.products[i].prodName.Value.ToString(), // 옵션값(수집명)
                            "",
                            string.Format("{0:n0}", priceObj), // 상품원가(원)
                            string.Format("{0:n0}", UdMargin.Value),
                            "", //무게는 생각하면서 넣자 //string.Format("{0:n0}", UdWeight.Value),
                            "",
                            "",
                            "",
                            "",
                            string.Format("{0:n0}", UdQty.Value),
                            "",
                            "",
                            "");
                    }
                    
                }

                //if (optionName != string.Empty)
                //{
                //    DgVariation.Rows.Add(i + 1, false,
                //    "상태",
                //    "",
                //    "",
                //    optionName,
                //    "",
                //    string.Format("{0:n0}", option2Ds[i].Price),
                //    "",
                //    "",
                //    "",
                //    "",
                //    "",
                //    100,
                //    "",
                //    "",
                //    "",
                //    "");
                //}
                Cursor.Current = Cursors.Default;
                MessageBox.Show("상품 정보를 수신하였습니다.","상품정보 수신",MessageBoxButtons.OK,MessageBoxIcon.Information);
                
            }
        }

        private void ScrapMissHaus(string ProductUrl)
        {
            if (ProductUrl.Contains("misshaus.com/default/"))
            {
                Application.UseWaitCursor = true;
                DGSelectedList.Rows.Clear();
                DGDetailList.Rows.Clear();
                DGImageSlicedList.Rows.Clear();
                DgVariation.Rows.Clear();

                string Html = GetHtml(ProductUrl);
                var myDoc = new HtmlAgilityPack.HtmlDocument();
                myDoc.LoadHtml(Html);

                //상품 제목
                var metaObj = myDoc.DocumentNode.SelectSingleNode("//h1[@class='product-name']");
                if (metaObj != null)
                {
                    TxtProductNameKor.Text = metaObj.InnerText.Trim();
                }

                //상품 가격
                var price = myDoc.DocumentNode.SelectSingleNode("//div[@class='product_name']");
                if (price != null)
                {

                }

                var thumbList = myDoc.DocumentNode.SelectNodes("//div[@class='product_visual prd_img_wrap']/div/div/ul//img");
                if (thumbList != null)
                {

                    int imgIdx = 0;
                    //메인이미지 1장

                    if (!Directory.Exists(ImagePath))
                    {
                        Directory.CreateDirectory(ImagePath);
                        Directory.CreateDirectory(ImagePathThumb);
                    }
                    else
                    {
                        //디렉토리를 비운다.
                        DirectoryInfo di = new DirectoryInfo(ImagePath);
                        di.Delete(true);

                        Directory.CreateDirectory(ImagePath);
                        Directory.CreateDirectory(ImagePathThumb);
                    }
                    //썸네일 이미지
                    if (thumbList.Count > 0)
                    {
                        for (int i = 0; i < thumbList.Count; i++)
                        {
                            if (thumbList[i].Attributes.Count == 4)
                            {
                                imgIdx++;
                                var request_Thumb = WebRequest.Create(thumbList[i].Attributes["src"].Value.ToString());
                                var response_Thumb = (HttpWebResponse)request_Thumb.GetResponse();
                                var dataStream_Thumb = response_Thumb.GetResponseStream();
                                Bitmap bm_Thumb = new Bitmap(dataStream_Thumb);

                                DGSelectedList.Rows.Add(DGSelectedList.Rows.Count + 1, false, bm_Thumb,
                                    $"thumb_{imgIdx:000}.jpg",
                                string.Format("{0:n0}", bm_Thumb.Width),
                                string.Format("{0:n0}", bm_Thumb.Height),
                                ImagePathThumb + $"thumb_{imgIdx:000}.jpg");
                                bm_Thumb.Save(ImagePathThumb + $@"thumb_{imgIdx:000}.jpg");
                            }
                        }
                    }
                }

                var listDetail = myDoc.DocumentNode.SelectNodes("//div[@class='detail_img']//img");
                for (int i = 0; i < listDetail.Count; i++)
                {
                    var request_Detail = WebRequest.Create(listDetail[i].Attributes["src"].Value.ToString().Trim());
                    var response_Detail = (HttpWebResponse)request_Detail.GetResponse();
                    var dataStream_Detail = response_Detail.GetResponseStream();
                    Bitmap bm_Detail = new Bitmap(dataStream_Detail);

                    DGDetailList.Rows.Add(DGDetailList.Rows.Count + 1, false, bm_Detail,
                    Path.GetFileName(listDetail[i].Attributes["src"].Value.ToString().Trim()),
                    string.Format("{0:n0}", bm_Detail.Width),
                    string.Format("{0:n0}", bm_Detail.Height));
                }

                var basePrice = myDoc.DocumentNode.SelectSingleNode("//span[@class='price']/span");

                for (int i = 0; i < DgPrice.Rows.Count; i++)
                {
                    DgPrice.Rows[i].Cells["DgPrice_source_price"].Value = basePrice.InnerText.Trim();
                    DgPrice.Rows[i].Cells["DgPrice_margin"].Value = string.Format("{0:n0}", UdMargin.Value);
                    DgPrice.Rows[i].Cells["DgPrice_qty"].Value = string.Format("{0:n0}", UdQty.Value);
                }
                

                int startIdx = Html.IndexOf("model = ") + 8;
                int endIdx = Html.IndexOf("model.isLimitDc");
                string strOptionJson = Html.Substring(startIdx, endIdx - startIdx).Replace(";\n\t\t\t", "");
                dynamic jOption = JsonConvert.DeserializeObject(strOptionJson);
                var onlyOption = jOption.products;
                for (int i = 0; i < onlyOption.Count; i++)
                {
                    var priceObj = Convert.ToInt32(onlyOption[i].availablePrices[0].onlineSalePrice.Value.ToString());
                    DgVariation.Rows.Add(i + 1, false,
                    "상태",
                    "",
                    "",
                    jOption.products[i].prodName.Value.ToString(),
                    "",
                    string.Format("{0:n0}", priceObj),
                    "",
                    "",
                    "",
                    "",
                    "",
                    100,
                    "",
                    "",
                    "",
                    "");
                }

                Cursor.Current = Cursors.Default;
                MessageBox.Show("상품 정보를 수신하였습니다.", "상품정보 수신", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool HasProperty(dynamic @object, string propertyName)
        {
            Type objType = @object.GetType();

            if (objType == typeof(ExpandoObject))
            {
                return ((IDictionary<string, object>)@object).ContainsKey(Name);
            }
            else if (objType == typeof(JObject))
            {
                List<JProperty> props = ((JObject)@object).Properties().ToList();
                
                if (props.Any(p => p.Name.Equals(propertyName)))
                {
                    return true;
                }
            }

            return objType.GetProperty(Name) != null;
        }

        private void ScrapSmartStore(string ProductUrl)
        {
            if (ProductUrl.Contains("smartstore.naver.com"))
            {
                Cursor.Current = Cursors.WaitCursor;
                DGSelectedList.Rows.Clear();
                DGDetailList.Rows.Clear();
                DGImageSlicedList.Rows.Clear();
                DgVariation.Rows.Clear();

                string Html = GetHtml(ProductUrl);
                var myDoc = new HtmlAgilityPack.HtmlDocument();
                myDoc.LoadHtml(Html);
                var dom = new HtmlParser().ParseDocument(Html);

                //상품 제목
                var title = myDoc.DocumentNode.SelectSingleNode("//h3[@class='_3oDjSvLwq9 _copyable']").InnerText;

                if (title != string.Empty)
                {
                    TxtProductNameKor.Text = title;
                }

                // 배송비
                var deliveryPrice = myDoc.DocumentNode.SelectSingleNode("//span[@class='Y-_Vd4O6dS']").InnerText;
                
                // 옵션
                string temp = myDoc.DocumentNode.SelectNodes("//script").FirstOrDefault(x => x.InnerText.Contains("window.__PRELOADED_STATE__")).InnerText;
                dynamic A = JsonConvert.DeserializeObject<dynamic>(temp.Replace("window.__PRELOADED_STATE__=", string.Empty)).product.A;
                IEnumerable<dynamic> options = A.optionCombinations; // option이 저장된 Json object

                var option2Ds = new List<Option2D>();

                if (options != null)
                {
                    foreach (dynamic option in options)
                    {
                        string optionName1 = string.Empty;
                        string optionName2 = string.Empty;
                        string optionName3 = string.Empty;
                        string optionName4 = string.Empty;
                        string optionName5 = string.Empty;
                        decimal price1 = default;

                        if (HasProperty(option, "optionName1"))
                        {
                            optionName1 = option.optionName1.ToString();
                        }

                        if (HasProperty(option, "optionName2"))
                        {
                            optionName2 = option.optionName2.ToString();
                        }

                        if (HasProperty(option, "optionName3"))
                        {
                            optionName3 = option.optionName3.ToString();
                        }

                        if (HasProperty(option, "optionName4"))
                        {
                            optionName4 = option.optionName4.ToString();
                        }

                        if (HasProperty(option, "optionName5"))
                        {
                            optionName5 = option.optionName5.ToString();
                        }

                        if (HasProperty(option, "price"))
                        {
                            price1 = (decimal)option.price;
                        }

                        option2Ds.Add(ToOption2Ds(optionName1, optionName2, optionName3, optionName4, optionName5, price1));
                    }
                }

                // 원가
                string originPrice = A.salePrice.ToString();

                //할인 가격
                string price = A.discountedSalePrice.ToString();

                var thumnailUrls = new List<string>();
                thumnailUrls.Add(myDoc.DocumentNode.SelectSingleNode("//meta[@name='twitter:image']").Attributes["content"].Value);

                if (thumnailUrls != null)
                {
                    int imgIdx = 0;
                    //메인이미지 1장

                    if (!Directory.Exists(ImagePath))
                    {
                        Directory.CreateDirectory(ImagePath);
                        Directory.CreateDirectory(ImagePathThumb);
                    }
                    else
                    {
                        //디렉토리를 비운다.
                        DirectoryInfo di = new DirectoryInfo(ImagePath);
                        di.Delete(true);

                        Directory.CreateDirectory(ImagePath);
                        Directory.CreateDirectory(ImagePathThumb);
                    }
                    //썸네일 이미지
                    if (thumnailUrls.Count > 0)
                    {
                        for (int i = 0; i < thumnailUrls.Count; i++)
                        {
                            imgIdx++;
                            var request_Thumb = WebRequest.Create(thumnailUrls[i].ToString());
                            var response_Thumb = (HttpWebResponse)request_Thumb.GetResponse();
                            var dataStream_Thumb = response_Thumb.GetResponseStream();
                            Bitmap bm_Thumb = new Bitmap(dataStream_Thumb);

                            DGSelectedList.Rows.Add(DGSelectedList.Rows.Count + 1, false, bm_Thumb,
                                $"thumb_{imgIdx:000}.jpg",
                            string.Format("{0:n0}", bm_Thumb.Width),
                            string.Format("{0:n0}", bm_Thumb.Height),
                            ImagePathThumb + $"thumb_{imgIdx:000}.jpg");
                            bm_Thumb.Save(ImagePathThumb + $@"thumb_{imgIdx:000}.jpg");
                        }
                    }
                }

                //스마트 스토어 본문 이미지
                // 본문 내용의 이미지들을 얻어옴.
                var req_images = new List<req_image>();
                string tempHtml = "";
                //iframe으로 되어 있을 경우
                if (Html.Contains("<iframe src="))
                {
                    tempHtml = Html.Replace(@"\", "");
                    var source = Regex.Match(tempHtml, "<iframe.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    using (WebClient client = new WebClient())
                    {
                        string htmlCode = client.DownloadString(source);
                        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(htmlCode);
                        req_images = doc.DocumentNode.Descendants("img")
                                    .Select(e => e.GetAttributeValue("src", null))
                                    .Where(s => !String.IsNullOrEmpty(s))
                                    .Select((imageUrl, index) => new req_image
                                    {
                                        src_addr = imageUrl,
                                        save_file_name = $"detail_{index + 1:000}"
                                    }).ToList();
                    }
                }
                else
                {
                    string id = A.id.ToString();
                    string productNo = A.productNo.ToString();

                    var getimageapi = new NaverSmartStoreGetBodyImage();
                    ResNaverSmartStoreGetBodyImage rtn = getimageapi.CallApi(ProductUrl, id, productNo);
                    int count = default;

                    var renderContent = new HtmlAgilityPack.HtmlDocument();
                    renderContent.LoadHtml(rtn.RenderContent);

                    HtmlNodeCollection nodes1 = renderContent.DocumentNode.SelectNodes(
                        @"//div[@class='se-component se-image se-l-default']
                          /div[@class='se-component-content se-component-content-fit']
                          /div[@class='se-section se-section-image se-l-default se-section-align-center']
                          /div[@class='se-module se-module-image']
                          /a[@class='se-module-image-link __se_image_link __se_link']
                          /img");

                    if (nodes1 != null)
                    {
                        foreach (HtmlNode node in nodes1)
                        {
                            req_images.Add(new req_image { src_addr = node.Attributes["data-src"].Value, save_file_name = $"detail_{count + 1:000}" });
                            count++;
                        }
                    }

                    HtmlNodeCollection nodes2 = renderContent.DocumentNode.SelectNodes("//center/img");

                    if (nodes2 != null)
                    {
                        foreach (HtmlNode node in nodes2)
                        {
                            req_images.Add(new req_image { src_addr = node.Attributes["data-src"].Value, save_file_name = $"detail_{count + 1:000}" });
                            count++;
                        }
                    }
                }

                for (int i = 0; i < req_images.Count; i++)
                {
                    try
                    {
                        var request_Detail = WebRequest.Create(req_images[i].src_addr.ToString());
                        var response_Detail = (HttpWebResponse)request_Detail.GetResponse();
                        var dataStream_Detail = response_Detail.GetResponseStream();
                        Bitmap bm_Detail = new Bitmap(dataStream_Detail);

                        DGDetailList.Rows.Add(DGDetailList.Rows.Count + 1, 
                            false, 
                            bm_Detail,
                            Path.GetFileName(req_images[i].src_addr.ToString()),
                            string.Format("{0:n0}", bm_Detail.Width),
                            string.Format("{0:n0}", bm_Detail.Height));
                    }
                    catch
                    {

                    }
                }

                for (int i = 0; i < DgPrice.Rows.Count; i++)
                {
                    DgPrice.Rows[i].Cells["DgPrice_source_price"].Value = price;
                    DgPrice.Rows[i].Cells["DgPrice_margin"].Value = string.Format("{0:n0}", UdMargin.Value);
                    DgPrice.Rows[i].Cells["DgPrice_qty"].Value = string.Format("{0:n0}", UdQty.Value);
                }

                DgVariation.Enabled = true;
                for (int j = 0; j < DgPrice.Rows.Count; j++)
                {
                    string country = DgPrice.Rows[j].Cells["DgPrice_tar_country"].Value.ToString();
                    string shopeeid = DgPrice.Rows[j].Cells["DgPrice_shopee_id"].Value.ToString();

                    for (int i = 0; i < option2Ds.Count; i++)
                    {
                        var priceObj = price + Convert.ToInt32(option2Ds[i].Price.ToString());
                        DgVariation.Rows.Add(DgVariation.Rows.Count + 1,
                            country,
                            shopeeid,
                            false,
                            "상태",
                            "",
                            "",
                            option2Ds[i].Option1.ToString() + "_" + option2Ds[i].Option2.ToString(),
                            "",
                            string.Format("{0:n0}", priceObj),
                            string.Format("{0:n0}", UdMargin.Value),
                            "", //무게는 생각하면서 넣자 //string.Format("{0:n0}", UdWeight.Value),
                            "",
                            "",
                            "",
                            "",
                            string.Format("{0:n0}", UdQty.Value),
                            "",
                            "",
                            "",
                            "");
                    }
                }


                Cursor.Current = Cursors.Default;
                MessageBox.Show("상품 정보를 수신하였습니다.", "상품정보 수신", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void ScrapBeautynet(string ProductUrl)
        {
            if (ProductUrl.Contains("misshaus.com/default/"))
            {
                Application.UseWaitCursor = true;
                DGSelectedList.Rows.Clear();
                DGDetailList.Rows.Clear();
                DGImageSlicedList.Rows.Clear();
                DgVariation.Rows.Clear();

                string Html = GetHtml(ProductUrl);
                var myDoc = new HtmlAgilityPack.HtmlDocument();
                myDoc.LoadHtml(Html);

                //상품 제목
                var metaObj = myDoc.DocumentNode.SelectSingleNode("//h1[@class='product-name']");
                if (metaObj != null)
                {
                    TxtProductNameKor.Text = metaObj.InnerText.Trim();
                }

                //상품 가격
                var price = myDoc.DocumentNode.SelectSingleNode("//div[@class='product_name']");
                if (price != null)
                {

                }

                var thumbList = myDoc.DocumentNode.SelectNodes("//div[@class='product_visual prd_img_wrap']/div/div/ul//img");
                if (thumbList != null)
                {

                    int imgIdx = 0;
                    //메인이미지 1장

                    if (!Directory.Exists(ImagePath))
                    {
                        Directory.CreateDirectory(ImagePath);
                        Directory.CreateDirectory(ImagePathThumb);
                    }
                    else
                    {
                        //디렉토리를 비운다.
                        DirectoryInfo di = new DirectoryInfo(ImagePath);
                        di.Delete(true);

                        Directory.CreateDirectory(ImagePath);
                        Directory.CreateDirectory(ImagePathThumb);
                    }
                    //썸네일 이미지
                    if (thumbList.Count > 0)
                    {
                        for (int i = 0; i < thumbList.Count; i++)
                        {
                            if (thumbList[i].Attributes.Count == 4)
                            {
                                imgIdx++;
                                var request_Thumb = WebRequest.Create(thumbList[i].Attributes["src"].Value.ToString());
                                var response_Thumb = (HttpWebResponse)request_Thumb.GetResponse();
                                var dataStream_Thumb = response_Thumb.GetResponseStream();
                                Bitmap bm_Thumb = new Bitmap(dataStream_Thumb);

                                DGSelectedList.Rows.Add(DGSelectedList.Rows.Count + 1, false, bm_Thumb,
                                    $"thumb_{imgIdx:000}.jpg",
                                string.Format("{0:n0}", bm_Thumb.Width),
                                string.Format("{0:n0}", bm_Thumb.Height),
                                ImagePathThumb + $"thumb_{imgIdx:000}.jpg");
                                bm_Thumb.Save(ImagePathThumb + $@"thumb_{imgIdx:000}.jpg");
                            }
                        }
                    }
                }

                var listDetail = myDoc.DocumentNode.SelectNodes("//div[@class='detail_img']//img");
                for (int i = 0; i < listDetail.Count; i++)
                {
                    var request_Detail = WebRequest.Create(listDetail[i].Attributes["src"].Value.ToString().Trim());
                    var response_Detail = (HttpWebResponse)request_Detail.GetResponse();
                    var dataStream_Detail = response_Detail.GetResponseStream();
                    Bitmap bm_Detail = new Bitmap(dataStream_Detail);

                    DGDetailList.Rows.Add(DGDetailList.Rows.Count + 1, false, bm_Detail,
                    Path.GetFileName(listDetail[i].Attributes["src"].Value.ToString().Trim()),
                    string.Format("{0:n0}", bm_Detail.Width),
                    string.Format("{0:n0}", bm_Detail.Height));
                }

                var basePrice = myDoc.DocumentNode.SelectSingleNode("//span[@class='price']/span");

                for (int i = 0; i < DgPrice.Rows.Count; i++)
                {
                    DgPrice.Rows[i].Cells["DgPrice_source_price"].Value = basePrice.InnerText.Trim();
                    DgPrice.Rows[i].Cells["DgPrice_margin"].Value = string.Format("{0:n0}", UdMargin.Value);
                    DgPrice.Rows[i].Cells["DgPrice_qty"].Value = string.Format("{0:n0}", UdQty.Value);
                }
                

                int startIdx = Html.IndexOf("model = ") + 8;
                int endIdx = Html.IndexOf("model.isLimitDc");
                string strOptionJson = Html.Substring(startIdx, endIdx - startIdx).Replace(";\n\t\t\t", "");
                dynamic jOption = JsonConvert.DeserializeObject(strOptionJson);
                var onlyOption = jOption.products;
                DgVariation.Enabled = true;
                for (int i = 0; i < onlyOption.Count; i++)
                {
                    var priceObj = Convert.ToInt32(onlyOption[i].availablePrices[0].onlineSalePrice.Value.ToString());
                    DgVariation.Rows.Add(i + 1, false,
                    "상태",
                    "",
                    "",
                    jOption.products[i].prodName.Value.ToString(),
                    "",
                    string.Format("{0:n0}", priceObj),
                    "",
                    "",
                    "",
                    "",
                    "",
                    100,
                    "",
                    "",
                    "",
                    "");
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("상품 정보를 수신하였습니다.", "상품정보 수신", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ScrapCoupang(string ProductUrl)
        {
            ProductUrl = HttpUtility.UrlDecode(ProductUrl);

            if (ProductUrl.Contains("coupang.com/vp/products/"))
            {
                DGSelectedList.Rows.Clear();
                DGDetailList.Rows.Clear();
                DGImageSlicedList.Rows.Clear();
                DgVariation.Rows.Clear();


                Application.DoEvents();
                //현재 실제로 가져온 페이지
                string Html = GetHtml(ProductUrl);
                var myDoc = new HtmlAgilityPack.HtmlDocument();
                myDoc.LoadHtml(Html);

                //상품 제목
                var metaObj = myDoc.DocumentNode.SelectSingleNode("//meta[@property='og:title']");
                if (metaObj != null)
                {
                    var title = metaObj.Attributes["content"].Value;
                    TxtProductNameKor.Text = title;
                }

                //불렛포인트
                var bulletObj = myDoc.DocumentNode.SelectNodes("//li[@class='prod-attr-item']");
                StringBuilder sbBullet = new StringBuilder();
                string tar_lang = "";

                if (cboLang.Text == "인도네시아어")
                {
                    tar_lang = "id";
                }
                else if (cboLang.Text == "영어")
                {
                    tar_lang = "en";
                }
                else if (cboLang.Text == "말레이시아어")
                {
                    tar_lang = "ms";
                }
                else if (cboLang.Text == "태국어")
                {
                    tar_lang = "th";
                }
                else if (cboLang.Text == "중국어 번체")
                {
                    tar_lang = "zh-TW";
                }
                else if (cboLang.Text == "베트남어")
                {
                    tar_lang = "vi";
                }

                if (bulletObj != null)
                {
                    for (int i = 0; i < bulletObj.Count; i++)
                    {
                        if (!bulletObj[i].InnerText.Contains("쿠팡상품번호"))
                        {
                            string translated = string.Empty;
                            translated = translate(
                                bulletObj[i].InnerText.Trim(),
                                "ko",
                                tar_lang).Trim();
                            sbBullet.Append("-" + translated + "\r\n");
                        }
                    }
                }


                TxtProductDesc.Text = sbBullet.ToString();
                refreshPreView();
                Application.DoEvents();

                var htmlDoc = new HtmlParser().ParseDocument(Html.Replace("\n", ""));
                var headerInfo = htmlDoc.QuerySelector("#prod-clt-or-fbt-recommend");
                var tempHeaderInfo = HttpUtility.UrlDecode(headerInfo.GetAttribute("data-fodium-widget-params").ToString().Replace("\n", ""));
                var jsonHeaderInfo = JObject.Parse(tempHeaderInfo);
                var productId = jsonHeaderInfo["productId"].ToString().Trim();

                var itemId = jsonHeaderInfo["itemId"].ToString().Trim();
                var vendoritemId = jsonHeaderInfo["vendorItemId"].ToString().Trim();

                //var thumbAddr = $"http://capi.coupang.com/v3/enhanced-pdp/products/158510837?bundleId=11&vendorItemId={vendoritemId}&appVer=3.8.8&applyReconciliation=true&applyPddStandizing=true&threePlBadge=true&newGlobalBadge=true&priceGuaranteeBadge=true&applyNps=true&retailFreeDelivery=true&applyInterstellar=true&_=1544610757724";
                var thumbAddr = $"https://m.coupang.com/vm/v4/enhanced-pdp/products/{productId}?bundleId=11&vendorItemId={vendoritemId}&appVer=3.8.8&applyReconciliation=true&applyPddStandizing=true&threePlBadge=true&newGlobalBadge=true&priceGuaranteeBadge=true&applyNps=true&retailFreeDelivery=A&applyInterstellar=true&_=1544610757724";
                dynamic dynThumb = JsonConvert.DeserializeObject(GetThumbNail(thumbAddr));
                dynamic thumbList = dynThumb.rData.vendorItemDetail.resource;

                var request = WebRequest.Create(thumbList.originalSquare.url.Value.ToString());
                var response = (HttpWebResponse)request.GetResponse();
                var dataStream = response.GetResponseStream();
                Bitmap bm = new Bitmap(dataStream);
                int imgIdx = 1;
                //메인이미지 1장

                if (!Directory.Exists(ImagePath))
                {
                    Directory.CreateDirectory(ImagePath);
                    Directory.CreateDirectory(ImagePathThumb);
                }
                else
                {
                    //디렉토리를 비운다.
                    DirectoryInfo di = new DirectoryInfo(ImagePath);
                    di.Delete(true);

                    Directory.CreateDirectory(ImagePath);
                    Directory.CreateDirectory(ImagePathThumb);
                }

                //파일을 저장한다.
                string saveFilePath = ImagePathThumb + @"thumb_" + string.Format("{0:D3}", imgIdx) + ".jpg";
                bm.Save(saveFilePath);

                DGSelectedList.Rows.Add(DGSelectedList.Rows.Count + 1, false, bm,
                    $"thumb_{imgIdx:000}.jpg",
                    string.Format("{0:n0}", bm.Width), string.Format("{0:n0}", bm.Height),
                    ImagePathThumb + $"thumb_{imgIdx:000}.jpg");


                //추가 이미지
                if (thumbList.detailImageList.Count > 0)
                {
                    for (int i = 0; i < thumbList.detailImageList.Count; i++)
                    {
                        imgIdx++;
                        var request_Thumb = WebRequest.Create(thumbList.detailImageList[i].url.Value.ToString());
                        var response_Thumb = (HttpWebResponse)request_Thumb.GetResponse();
                        var dataStream_Thumb = response_Thumb.GetResponseStream();
                        Bitmap bm_Thumb = new Bitmap(dataStream_Thumb);

                        DGSelectedList.Rows.Add(DGSelectedList.Rows.Count + 1, false, bm_Thumb, $"thumb_{imgIdx:000}.jpg", string.Format("{0:n0}", bm_Thumb.Width), string.Format("{0:n0}", bm_Thumb.Height), ImagePathThumb + $"thumb_{imgIdx:000}.jpg");
                        bm_Thumb.Save(ImagePathThumb + $@"thumb_{imgIdx:000}.jpg");
                    }
                }

                var bodyHtmlUrl = $"https://m.coupang.com/vm/products/{productId}/brand-sdp/items/{itemId}/?vendorItemId={vendoritemId}";
                var bodyHtml = GetBodyHtml(bodyHtmlUrl);
                var listDetail = Get_Detail_req_images(bodyHtml).ToList();

                for (int i = 0; i < listDetail.Count; i++)
                {
                    var request_Detail = WebRequest.Create(listDetail[i].src_addr.ToString());
                    var response_Detail = (HttpWebResponse)request_Detail.GetResponse();
                    var dataStream_Detail = response_Detail.GetResponseStream();
                    Bitmap bm_Detail = new Bitmap(dataStream_Detail);

                    DGDetailList.Rows.Add(DGDetailList.Rows.Count + 1, false, bm_Detail,
                    Path.GetFileName(listDetail[i].save_file_name),
                    string.Format("{0:n0}", bm_Detail.Width),
                    string.Format("{0:n0}", bm_Detail.Height));
                }

                var productName = htmlDoc.QuerySelector(".prod-buy-header__title").TextContent;
                var price = htmlDoc.QuerySelector(".total-price").Children[0].TextContent.Replace(",", "").Replace("원", "");
                decimal dPrice = 0;
                decimal.TryParse(price, out dPrice);

                string option = ToCombinedJsonOptions(productId, itemId, vendoritemId);
                dynamic optionJsonArray = JArray.Parse(option);
                List<Option2D> option2Ds = new List<Option2D>();

                if (optionJsonArray.Count == 1)
                {
                    if (productName == optionJsonArray.First.name.ToString().Trim())
                    //if (productName != optionJsonArray.First.title.ToString().Trim())
                    {
                        option2Ds.Add(new Option2D { Option1 = "NA", Option2 = "NA", Price = dPrice });
                    }
                    else
                    {
                        option2Ds.Add(new Option2D { Option1 = "NA", Option2 = "NA", Price = dPrice });
                    }
                }
                else
                {
                    option2Ds =
                    ((IEnumerable<dynamic>)optionJsonArray)
                    .Where(o => o.remainCount != 0)
                    .Select(o =>
                        (Option2D)ToOption2Ds(
                            ((string)o.name).Split(',').ElementAtOrDefault(0),
                            ((string)o.name).Split(',').ElementAtOrDefault(1),
                            ((string)o.name).Split(',').ElementAtOrDefault(2),
                            ((string)o.name).Split(',').ElementAtOrDefault(3),
                            ((string)o.name).Split(',').ElementAtOrDefault(4),
                            (decimal)o.salesPrice))
                    //(decimal)o.salesPrice - decimal.Parse(price.Replace(",", ""))))
                    .Distinct()
                    .ToList();
                }

                // 옵션이 없으면, NA/NA 추가.
                //if (!option2Ds.Any())
                //{
                //    option2Ds.Add(new Option2D { Option1 = "NA", Option2 = "NA" });
                //}

                //마이너스 옵션에 대한 처리
                //2차원 옵션으로 변경한 후 마이너스 옵션이 있는 경우는 보정하여 다시 저장한다.
                //우선 마이너스 옵션이 가장 큰값을 찾아서 기록한다.
                bool containMinus = false;
                decimal maxMinus = 0;

                for (int i = 0; i < option2Ds.Count; i++)
                {
                    if (option2Ds[i].Price < 0)
                    {
                        containMinus = true;
                        if (maxMinus > option2Ds[i].Price)
                        {
                            maxMinus = option2Ds[i].Price;
                        }
                    }
                }

                decimal iPrice = 0;

                if (containMinus)
                {
                    maxMinus = Math.Abs(maxMinus);
                    price = price.Replace(",", "");
                    if (decimal.TryParse(price, out iPrice))
                    {
                        iPrice = iPrice - maxMinus;
                        //수집가격과 옵션값을 모두 보정한다.
                        for (int i = 0; i < option2Ds.Count; i++)
                        {
                            option2Ds[i].Price = option2Ds[i].Price + maxMinus;
                        }
                    }
                }
                else
                {
                    price = price.Replace(",", "");
                    decimal.TryParse(price, out iPrice);
                }


                decimal basePrice = 100000000;
                if (option2Ds.Count > 0)
                {
                    DgVariation.Enabled = true;
                    for (int j = 0; j < DgPrice.Rows.Count; j++)
                    {
                        string country = DgPrice.Rows[j].Cells["DgPrice_tar_country"].Value.ToString();
                        string shopeeid = DgPrice.Rows[j].Cells["DgPrice_shopee_id"].Value.ToString();

                        for (int i = 0; i < option2Ds.Count; i++)
                        {
                            string optionName = "";
                            if (option2Ds[i].Option1 != "NA" && option2Ds[i].Option2 != "NA")
                            {
                                optionName = option2Ds[i].Option1 + "_" + option2Ds[i].Option2;
                                if (basePrice > option2Ds[i].Price)
                                {
                                    basePrice = option2Ds[i].Price;
                                }
                            }
                            else if (option2Ds[i].Option1 != "NA" && option2Ds[i].Option2 == "NA")
                            {
                                optionName = option2Ds[i].Option1;
                                if (basePrice > option2Ds[i].Price)
                                {
                                    basePrice = option2Ds[i].Price;
                                }
                            }
                            else if (option2Ds[i].Option1 == "NA" && option2Ds[i].Option2 == "NA")
                            {
                                if (basePrice > option2Ds[i].Price)
                                {
                                    basePrice = option2Ds[i].Price;
                                }
                            }


                            if (optionName != string.Empty)
                            {                                
                                DgVariation.Rows.Add(DgVariation.Rows.Count + 1,
                                country,
                                shopeeid, false,
                                "상태",
                                "",
                                "",
                                optionName,
                                "",
                                string.Format("{0:n0}", option2Ds[i].Price),
                                string.Format("{0:n0}", UdMargin.Value),
                                "", //무게는 생각하면서 넣자 //string.Format("{0:n0}", UdWeight.Value),
                                "",
                                "",
                                "",
                                "",
                                string.Format("{0:n0}", UdQty.Value),
                                "",
                                "",
                                "");
                            }
                        }
                    }

                    
                }
                else
                {
                    basePrice = iPrice;
                }

                for (int i = 0; i < DgPrice.Rows.Count; i++)
                {
                    DgPrice.Rows[i].Cells["DgPrice_source_price"].Value = string.Format("{0:n0}", basePrice);
                    DgPrice.Rows[i].Cells["DgPrice_margin"].Value = string.Format("{0:n0}", UdMargin.Value);
                    DgPrice.Rows[i].Cells["DgPrice_qty"].Value = string.Format("{0:n0}", UdQty.Value);
                }
                

                if (DgVariation.Rows.Count > 0)
                {
                    Rd_None_Variation1.Checked = true;
                }
                else
                {
                    Rd_None_Variation.Checked = true;
                }
                lblThumb.Text = "기본 썸네일 [ " + DGSelectedList.Rows.Count + " ]";
                lblDetail.Text = "상세 이미지 [ " + DGDetailList.Rows.Count + " ]";

                MessageBox.Show("상품 데이터를 수신하였습니다.", "상품 데이터 수집", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Application.UseWaitCursor = false;
        }

        private void ScrapArtbox(string ProductUrl)
        {
            DGSelectedList.Rows.Clear();
            DGDetailList.Rows.Clear();
            DGImageSlicedList.Rows.Clear();
            DgVariation.Rows.Clear();

            ArtboxProduct item = ArtboxCrawling.GetProductDetail(ProductUrl);
            string title = item.Title;
            TxtProductNameKor.Text = title.Substring(0, title.LastIndexOf('(')).Trim();
            string tar_lang = string.Empty;

            if (cboLang.Text == "인도네시아어")
            {
                tar_lang = "id";
            }
            else if (cboLang.Text == "영어")
            {
                tar_lang = "en";
            }
            else if (cboLang.Text == "말레이시아어")
            {
                tar_lang = "ms";
            }
            else if (cboLang.Text == "태국어")
            {
                tar_lang = "th";
            }
            else if (cboLang.Text == "중국어 번체")
            {
                tar_lang = "zh-TW";
            }
            else if (cboLang.Text == "베트남어")
            {
                tar_lang = "vi";
            }

            TxtProductDesc.Text = HttpUtility.HtmlDecode(translate(item.BodyTopBottomContent, "ko", tar_lang));
            refreshPreView();

            List<string> mainImageUrls = JsonConvert.DeserializeObject<List<string>>(item.MainImageUrls);
            var request = WebRequest.Create(mainImageUrls[0]);
            var response = (HttpWebResponse)request.GetResponse();
            var dataStream = response.GetResponseStream();
            var bm = new Bitmap(dataStream);
            int imgIdx = 1;

            if (!Directory.Exists(ImagePath))
            {
                Directory.CreateDirectory(ImagePath);
                Directory.CreateDirectory(ImagePathThumb);
            }
            else
            {
                //디렉토리를 비운다.
                DirectoryInfo di = new DirectoryInfo(ImagePath);
                di.Delete(true);

                Directory.CreateDirectory(ImagePath);
                Directory.CreateDirectory(ImagePathThumb);
            }

            string saveFilePath = ImagePathThumb + @"thumb_" + string.Format("{0:D3}", imgIdx) + ".jpg";
            bm.Save(saveFilePath);

            DGSelectedList.Rows.Add(DGSelectedList.Rows.Count + 1, false, bm,
                $"thumb_{imgIdx:000}.jpg",
                string.Format("{0:n0}", bm.Width), string.Format("{0:n0}", bm.Height),
                ImagePathThumb + $"thumb_{imgIdx:000}.jpg");

            //추가 이미지
            if (mainImageUrls.Count > 1)
            {
                for (int i = 1; i < mainImageUrls.Count; i++)
                {
                    imgIdx++;
                    var request_Thumb = WebRequest.Create(mainImageUrls[i]);
                    var response_Thumb = (HttpWebResponse)request_Thumb.GetResponse();
                    var dataStream_Thumb = response_Thumb.GetResponseStream();
                    Bitmap bm_Thumb = new Bitmap(dataStream_Thumb);

                    DGSelectedList.Rows.Add(DGSelectedList.Rows.Count + 1, 
                        false, 
                        bm_Thumb, 
                        $"thumb_{imgIdx:000}.jpg",
                        string.Format("{0:n0}", bm_Thumb.Width),
                        string.Format("{0:n0}", bm_Thumb.Height),
                        ImagePathThumb + $"thumb_{imgIdx:000}.jpg");
                        bm_Thumb.Save(ImagePathThumb + $@"thumb_{imgIdx:000}.jpg");
                }
            }

            List<string> bodyImageUrls = JsonConvert.DeserializeObject<List<string>>(item.BodyImageUrls);

            for (int i = 0; i < bodyImageUrls.Count; i++)
            {
                var request_Detail = WebRequest.Create(bodyImageUrls[i]);
                var response_Detail = (HttpWebResponse)request_Detail.GetResponse();
                var dataStream_Detail = response_Detail.GetResponseStream();
                Bitmap bm_Detail = new Bitmap(dataStream_Detail);

                DGDetailList.Rows.Add(DGDetailList.Rows.Count + 1, 
                    false, 
                    bm_Detail,
                    Path.GetFileName(bodyImageUrls[i]),
                    string.Format("{0:n0}", bm_Detail.Width),
                    string.Format("{0:n0}", bm_Detail.Height));
            }

            List<string> options = JsonConvert.DeserializeObject<List<string>>(item.ProductSkusJson);

            if (options != null)
            {
                for (int j = 0; j < DgPrice.Rows.Count; j++)
                {
                    string country = DgPrice.Rows[j].Cells["DgPrice_tar_country"].Value.ToString();
                    string shopeeid = DgPrice.Rows[j].Cells["DgPrice_shopee_id"].Value.ToString();

                    foreach (string option in options)
                    {
                        DgVariation.Rows.Add(DgVariation.Rows.Count + 1, // No
                            country, // 판매국가, visible
                            shopeeid, // 쇼피ID, visible
                            false, // V, visible
                            "상태", // 상태, not visible
                            "", // Variation Id, not visible
                            "", // 옵션 SKU, visible
                            option, // 옵션값(수집명), visible
                            "", // 옵션값(등록용), visible
                            "", // 상품원가(원), visible
                            string.Format("{0:n0}", UdMargin.Value), // 마진(원), visible
                            "", //무게는 생각하면서 넣자 //string.Format("{0:n0}", UdWeight.Value), // 무게(Kg), visible
                            "", // PG 수수료, visible
                            "", // 판매가(원), visible
                            "", // 판매가
                            "", // 소비자가
                            string.Format("{0:n0}", UdQty.Value), // 수량
                            "", // 생성일시
                            "", // 수정일시
                            ""); // 할인명
                                 // 할인 ID 어디갔지??
                    }
                }
            }

            decimal dPrice = Convert.ToDecimal(item.Price);

            for (int i = 0; i < DgPrice.Rows.Count; i++)
            {
                DgPrice.Rows[i].Cells["DgPrice_source_price"].Value = string.Format("{0:n0}", dPrice);
                DgPrice.Rows[i].Cells["DgPrice_margin"].Value = string.Format("{0:n0}", UdMargin.Value);
                DgPrice.Rows[i].Cells["DgPrice_qty"].Value = string.Format("{0:n0}", UdQty.Value);
            }

            lblThumb.Text = "기본 썸네일 [ " + DGSelectedList.Rows.Count + " ]";
            lblDetail.Text = "상세 이미지 [ " + DGDetailList.Rows.Count + " ]";
            Application.UseWaitCursor = false;
            MessageBox.Show("상품 데이터를 수신하였습니다.", "상품 데이터 수집", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private Option2D ToOption2Ds(string optionName1, string optionName2, string optionName3, string optionName4, string optionName5, decimal optionPrice)
        {
            var option1 = string.IsNullOrWhiteSpace(optionName1) ? "NA" : optionName1;

            var option2 = "";

            if (string.IsNullOrWhiteSpace(optionName2))
            {
                option2 = "NA";
            }
            else
            {
                option2 += optionName2;

                if (!string.IsNullOrWhiteSpace(optionName3))
                    option2 += $"_{optionName3}";

                if (!string.IsNullOrWhiteSpace(optionName4))
                    option2 += $"_{optionName4}";

                if (!string.IsNullOrWhiteSpace(optionName5))
                    option2 += $"_{optionName5}";
            }

            return new Option2D
            {
                Option1 = option1.Trim(),
                Option2 = option2.Trim(),
                Price = optionPrice
            };
        }

        private string ToCombinedJsonOptions(string productId, string itemId, string vendorItemId)
        {
            //디버깅 할때 모바일웹에서 보기 바람.
            //http://m.coupang.com/vm/products/44396012?itemId=159435376&q=%EC%8B%A0%EB%B0%9C
            //또는 핸드폰 wifi 피들러로 경유하여 디버깅 가능
            //http://brocess.tistory.com/22
            //192.168.0.36 8888
            var processedOptions = "";



            //2018년 12월 2일 주소가 수정되어 변경함
            //상품의 기본 옵션을 담고 있는 페이지 호출

            var optionsPage0 = GetHtml($"https://m.coupang.com/vm/v4/enhanced-pdp/products/{productId}?bundleId=11&vendorItemId={vendorItemId}&appVer=3.8.8&applyReconciliation=true&applyPddStandizing=true&threePlBadge=true&newGlobalBadge=true&priceGuaranteeBadge=true&applyNps=true&retailFreeDelivery=A&applyInterstellar=true&_=1544610757724");
            //var optionsPage0 = shopHttpClientProvider.NewHttpClient().GetAsync($"http://capi.coupang.com/v3/enhanced-pdp/products/{productId}?bundleId=11&vendorItemId={vendorItemId}&appVer=3.8.8&applyReconciliation=true&applyPddStandizing=true&threePlBadge=true&newGlobalBadge=true&priceGuaranteeBadge=true&applyNps=true&retailFreeDelivery=B&_=1543668786329").Result.Content.ReadAsStringAsync().Result;

            //이전 코드그루 작업
            //var optionsPage0 = shopHttpClientProvider.NewHttpClient().GetAsync($"http://www.coupang.com/vp/products/{productId}/brandsdp/options/0?isFixedVendorItemId=true&itemId={itemId}&vendorItemId={vendorItemId}&attrTypeIds=0&noAttribute=yes&sdpStyle=FASHION_STYLE_TWO_WAY&selectedAttrTypeIds=0&selectedAttrValueIds=0").Result.Content.ReadAsStringAsync().Result;

            if (!string.IsNullOrEmpty(optionsPage0))
            {
                //json 파싱 및 변환
                dynamic result = JsonConvert.DeserializeObject(optionsPage0);
                //1차 옵션을 모두 가지고 온다.
                var objOption1 = result.rData.product.entityList[2].entity.options;
                var total_depth = result.rData.product.entityList[2].entity.totalDepth.Value;


                //processedOptions += JsonConvert.SerializeObject(objOption1);
                //만약 옵션이 1단 이하라면
                if (total_depth < 2)
                {
                    //현재의 옵션만 모아서 보낸다.
                    var lstOption = result.rData.options;
                    //processedOptions += "[";
                    for (int i = 0; i < objOption1.Count; i++)
                    {

                        if (i == objOption1.Count - 1)
                        {
                            processedOptions += JsonConvert.SerializeObject(objOption1);
                        }
                        else
                        {
                            processedOptions += JsonConvert.SerializeObject(objOption1) + ",";
                        }
                    }
                    //processedOptions += "]";
                }
                else
                {
                    //옵션을 얻어 오는 주소를 모두 방문하여 데이터를 얻어 온다.
                    //1차 옵션이 2개 이상인 경우 마지막 까지 호출하여 결과를 누적한다.
                    //모든 옵션을 배열로 넘기는데 다단일 경우 옵션이름에 콤마로 분리하여 다단을 표현한다.
                    for (int i = 0; i < objOption1.Count; i++)
                    {
                        //하위 옵션이 담겨 있는 주소 json안에 주소가 있음
                        //http://capi.coupang.com/v3/products/27345148/attributes/2439/options/2004479923?itemId=119049428&vendorItemId=3087407543
                        var jStr = GetHtml($"https://m.coupang.com{objOption1[i].requestUri}");
                        //var jStr = shopHttpClientProvider.NewHttpClient().GetAsync($"http://capi.coupang.com{objOption1[i].requestUri}").Result.Content.ReadAsStringAsync().Result;
                        //processedOptions += lstOption[i].name.ToString.Trim();
                        dynamic result_sub = JsonConvert.DeserializeObject(jStr);

                        for (int j = 0; j < result_sub.rData.options.Count; j++)
                        {
                            result_sub.rData.options[j].name = objOption1[i].name + "," + result_sub.rData.options[j].name;
                        }
                        if (i == objOption1.Count - 1)
                        {
                            processedOptions += JsonConvert.SerializeObject(result_sub.rData.options);
                        }
                        else
                        {
                            processedOptions += JsonConvert.SerializeObject(result_sub.rData.options) + ",";
                        }
                    }
                }

                //var optionsPage0Json = JObject.Parse(optionsPage0);

                //processedOptions += result.rData.options;
                //processedOptions += optionsPage0Json["options"];

                //var nextPageUrl = (string)optionsPage0Json["nextPageUrl"];

                //while (!string.IsNullOrWhiteSpace(nextPageUrl))
                //{
                //    var optionsNextPage = shopHttpClientProvider.NewHttpClient().GetAsync($"http://www.coupang.com{nextPageUrl}").Result.Content.ReadAsStringAsync().Result;
                //    var optionsNextPageJson = JObject.Parse(optionsNextPage);
                //    processedOptions += "!!!" + optionsNextPageJson["options"];

                //    nextPageUrl = (string)optionsNextPageJson["nextPageUrl"];
                //}

                //return processedOptions.Replace("]!!![", ",");


            }

            return processedOptions.Replace("],[", ",");
        }

        private void saveDetailImage(string url)
        {
            var request = WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var dataStream = response.GetResponseStream();
            Bitmap bm = new Bitmap(dataStream);
            int imgIdx = DGSelectedList.Rows.Count + 1;
            //메인이미지 1장

            if (!Directory.Exists(ImagePath))
            {
                Directory.CreateDirectory(ImagePath);
                Directory.CreateDirectory(ImagePathThumb);
            }

            //파일을 저장한다.
            imgIdx++;
            var request_Detail = WebRequest.Create(url);
            var response_Detail = (HttpWebResponse)request_Detail.GetResponse();
            var dataStream_Detail = response_Detail.GetResponseStream();
            Bitmap bm_Detail = new Bitmap(dataStream_Detail);

            DGDetailList.Rows.Add(DGDetailList.Rows.Count + 1, false, bm_Detail,
            Path.GetFileName(url),
            string.Format("{0:n0}", bm_Detail.Width),
            string.Format("{0:n0}", bm_Detail.Height));
        }

        private void saveThumbImage(string url)
        {
            var request = WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var dataStream = response.GetResponseStream();
            Bitmap bm = new Bitmap(dataStream);
            int imgIdx = DGSelectedList.Rows.Count + 1;
            //메인이미지 1장

            if (!Directory.Exists(ImagePath))
            {
                Directory.CreateDirectory(ImagePath);
                Directory.CreateDirectory(ImagePathThumb);
            }

            //파일을 저장한다.
            string saveFilePath = ImagePathThumb + @"thumb_" + string.Format("{0:D3}", imgIdx) + ".jpg";
            bm.Save(saveFilePath);

            DGSelectedList.Rows.Add(DGSelectedList.Rows.Count + 1, false, bm,
                $"thumb_{imgIdx:000}.jpg",
                string.Format("{0:n0}", bm.Width), string.Format("{0:n0}", bm.Height),
                ImagePathThumb + $"thumb_{imgIdx:000}.jpg");
        }
        private void FromAddProduct_DragDrop(object sender, DragEventArgs e)
        {
            Application.UseWaitCursor = true;
            string text = string.Empty;
                        
            if (e.Data.GetDataPresent("HTML Format"))
            {
                text = (string)e.Data.GetData("HTML Format");

                if (text.Contains("typeof google==="))
                {
                    var myDoc = new HtmlAgilityPack.HtmlDocument();
                    myDoc.LoadHtml(text);

                    //상품 제목
                    if(myDoc.DocumentNode.SelectSingleNode("//img[@class='irc_mi']") != null)
                    {
                        var imgObj = myDoc.DocumentNode.SelectSingleNode("//img[@class='irc_mi']").Attributes["src"].Value;

                        if (imgObj != null && imgObj != string.Empty)
                        {
                            saveThumbImage(imgObj);
                        }
                    }
                }
                else if(text.Contains("images-amazon.com"))
                {
                    string matchString = Regex.Match(text, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    saveDetailImage(matchString);
                }
                else if (text.Contains("m.media-amazon.com"))
                {
                    string matchString = Regex.Match(text, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    saveDetailImage(matchString);
                }
                else if(text.Contains("image.aladin.co.kr"))
                {
                    var myDoc = new HtmlAgilityPack.HtmlDocument();
                    myDoc.LoadHtml(text);

                    //상품 제목
                    if (myDoc.DocumentNode.SelectSingleNode("//img[@id='CoverMainImage']") != null)
                    {
                        var imgObj = myDoc.DocumentNode.SelectSingleNode("//img[@id='CoverMainImage']").Attributes["src"].Value;
                        if (imgObj != null && imgObj != string.Empty)
                        {
                            saveThumbImage(imgObj);
                        }
                    }
                    else if(myDoc.DocumentNode.SelectSingleNode("//img") != null)
                    {
                        var imgObj = myDoc.DocumentNode.SelectSingleNode("//img").Attributes["src"].Value;
                        if (imgObj != null && imgObj != string.Empty)
                        {
                            saveDetailImage(imgObj);
                        }
                    }
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                text = (string)e.Data.GetData(DataFormats.Text); // 드래그 드랍한 URI 주소가 Input 된다.
                doScrap(text);                
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    // .url for Chrome and FF, .website for IE
                    if (file.EndsWith(".url") || file.EndsWith(".website"))
                    {

                        //text += ini.IniReadValue("InternetShortcut", "URL") + Environment.NewLine;
                    }
                    else
                    {
                        MessageBox.Show("Unsupported file format for file: " + file);
                    }
                }
            }

            Application.UseWaitCursor = false;
        }

        private void DropPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void DropPanel_DragDrop(object sender, DragEventArgs e)
        {
            Application.UseWaitCursor = true;
            string text = string.Empty;
            if (e.Data.GetDataPresent("HTML Format"))
            {
                text = (string)e.Data.GetData("HTML Format");
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                text = (string)e.Data.GetData(DataFormats.Text);
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    // .url for Chrome and FF, .website for IE
                    if (file.EndsWith(".url") || file.EndsWith(".website"))
                    {

                        //text += ini.IniReadValue("InternetShortcut", "URL") + Environment.NewLine;
                    }
                    else
                    {
                        MessageBox.Show("Unsupported file format for file: " + file);
                    }
                }
            }

            if (text.Contains("coupang.com"))
            {
                DGSelectedList.Rows.Clear();
                DGDetailList.Rows.Clear();
                Application.DoEvents();
                //현재 실제로 가져온 페이지
                string Html = GetHtml(text);
                var myDoc = new HtmlAgilityPack.HtmlDocument();
                myDoc.LoadHtml(Html);

                //상품 제목
                var metaObj = myDoc.DocumentNode.SelectSingleNode("//meta[@property='og:title']");
                if (metaObj != null)
                {
                    var title = metaObj.Attributes["content"].Value;
                    TxtProductNameKor.Text = title;
                }

                //불렛포인트
                var bulletObj = myDoc.DocumentNode.SelectNodes("//li[@class='prod-attr-item']");
                StringBuilder sbBullet = new StringBuilder();
                string tar_lang = "";
                //if (tarCountry == "ID")
                //{
                //    tar_lang = "id";
                //}
                //else if (tarCountry == "SG")
                //{
                //    tar_lang = "en";
                //}
                //else if (tarCountry == "MY")
                //{
                //    tar_lang = "ms";
                //}
                //else if (tarCountry == "TH")
                //{
                //    tar_lang = "th";
                //}
                //else if (tarCountry == "TW")
                //{
                //    tar_lang = "zh-TW";
                //}
                //else if (tarCountry == "PH")
                //{
                //    tar_lang = "en";
                //}
                //else if (tarCountry == "VN")
                //{
                //    tar_lang = "vi";
                //}

                if (bulletObj != null)
                {
                    for (int i = 0; i < bulletObj.Count; i++)
                    {
                        if (!bulletObj[i].InnerText.Contains("쿠팡상품번호"))
                        {
                            //string translated = string.Empty;
                            //translated = translate(
                            //    bulletObj[i].InnerText.Trim(),
                            //    "ko",
                            //    tar_lang).Trim();
                            //sbBullet.Append("-" + translated + "\r\n");

                            sbBullet.Append("-" + bulletObj[i].InnerText.Trim() + "\r\n");
                        }
                    }
                }


                TxtProductDesc.Text = sbBullet.ToString();

                var htmlDoc = new HtmlParser().ParseDocument(Html.Replace("\n", ""));
                var headerInfo = htmlDoc.QuerySelector("#prod-clt-or-fbt-recommend");
                var tempHeaderInfo = HttpUtility.UrlDecode(headerInfo.GetAttribute("data-fodium-widget-params").ToString().Replace("\n", ""));
                var jsonHeaderInfo = JObject.Parse(tempHeaderInfo);
                var productId = jsonHeaderInfo["productId"].ToString().Trim();

                var itemId = jsonHeaderInfo["itemId"].ToString().Trim();
                var vendoritemId = jsonHeaderInfo["vendorItemId"].ToString().Trim();

                var thumbAddr = $"http://capi.coupang.com/v3/enhanced-pdp/products/158510837?bundleId=11&vendorItemId={vendoritemId}&appVer=3.8.8&applyReconciliation=true&applyPddStandizing=true&threePlBadge=true&newGlobalBadge=true&priceGuaranteeBadge=true&applyNps=true&retailFreeDelivery=true&applyInterstellar=true&_=1544610757724";
                
                dynamic dynThumb = JsonConvert.DeserializeObject(GetThumbNail(thumbAddr));
                dynamic thumbList = dynThumb.rData.vendorItemDetail.resource;

                var request = WebRequest.Create(thumbList.originalSquare.url.Value.ToString());
                var response = (HttpWebResponse)request.GetResponse();
                var dataStream = response.GetResponseStream();
                Bitmap bm = new Bitmap(dataStream);



                int imgIdx = 1;
                //메인이미지 1장
                DGSelectedList.Rows.Add(DGSelectedList.Rows.Count + 1, false, bm,
                    $"thumb_{imgIdx:000}",
                    string.Format("{0:n0}", bm.Width), string.Format("{0:n0}", bm.Height));


                //추가 이미지
                if (thumbList.detailImageList.Count > 0)
                {
                    for (int i = 0; i < thumbList.detailImageList.Count; i++)
                    {
                        var request_Thumb = WebRequest.Create(thumbList.detailImageList[i].url.Value.ToString());
                        var response_Thumb = (HttpWebResponse)request_Thumb.GetResponse();
                        var dataStream_Thumb = response_Thumb.GetResponseStream();
                        Bitmap bm_Thumb = new Bitmap(dataStream_Thumb);

                        DGSelectedList.Rows.Add(DGSelectedList.Rows.Count + 1, false, bm_Thumb,
                            $"thumb_{imgIdx++:000}",
                        string.Format("{0:n0}", bm_Thumb.Width),
                        string.Format("{0:n0}", bm_Thumb.Height));
                    }

                    //int remain = 8 - thumbList.detailImageList.Count;
                    //for (int i = 0; i < remain; i++)
                    //{
                    //    DGSelectedList.Rows.Add(DGSelectedList.Rows.Count + 1, false, bm,
                    //Path.GetFileName(thumbList.originalSquare.url.Value.ToString()),
                    //bm.Width, bm.Height);
                    //}
                }


                var bodyHtmlUrl = $"https://m.coupang.com/vm/products/{productId}/brand-sdp/items/{itemId}/?vendorItemId={vendoritemId}&style=MOBILE_BROWSER&isFashion=true&invalidSdpFromBrowser=false&deliveryType=VENDOR_DELIVERY&loyaltyMember=false";
                var bodyHtml = GetBodyHtml(bodyHtmlUrl);
                var listDetail = Get_Detail_req_images(bodyHtml).ToList();
                for (int i = 0; i < listDetail.Count; i++)
                {
                    var request_Thumb = WebRequest.Create(listDetail[i].src_addr.ToString());
                    var response_Thumb = (HttpWebResponse)request_Thumb.GetResponse();
                    var dataStream_Thumb = response_Thumb.GetResponseStream();
                    Bitmap bm_Thumb = new Bitmap(dataStream_Thumb);

                    DGDetailList.Rows.Add(DGDetailList.Rows.Count + 1, false, bm_Thumb,
                Path.GetFileName(listDetail[i].save_file_name),
                bm_Thumb.Width, bm_Thumb.Height);
                }

                var productName = htmlDoc.QuerySelector(".prod-buy-header__title").TextContent;
                var prc = htmlDoc.QuerySelector(".total-price").Children[0].TextContent.Replace(",", "").Replace("원", "");
                Cursor.Current = Cursors.Default;
            }

            Application.UseWaitCursor = false;
        }

        #region Get_Detail_req_images
        private IEnumerable<req_image> Get_Detail_req_images(string Html)
        {
            // 본문 내용의 이미지들을 얻어옴.
            //본문 이미지
            List<req_image> req_images = new List<req_image>();
            if (Html.Contains("data-src"))
            {
                req_images = new HtmlParser().ParseDocument(Html).QuerySelectorAll("img")
                .Select(img => img.GetAttribute("data-src"))
                .Select((imageUrl, index) => new req_image
                {
                    src_addr = imageUrl,
                    save_file_name = $"detail_{index + 1:000}.jpg"
                }).ToList();
            }
            else
            {
                req_images = new HtmlParser().ParseDocument(Html).QuerySelectorAll("img")
                .Select(img => img.GetAttribute("src"))
                .Select((imageUrl, index) => new req_image
                {
                    src_addr = imageUrl,
                    save_file_name = $"detail_{index + 1:000}.jpg"
                }).ToList();
            }

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(Html);

            var parse = doc.DocumentNode.SelectNodes("//div[@class='vendor-item-detail__content']");

            for (int i = req_images.Count - 1; i >= 0; i--)
            {
                //var url = req_images[i].src_addr.ToString();
                if (req_images[i].src_addr == null || req_images[i].src_addr.Contains("/akam/"))
                {
                    req_images.RemoveAt(i);
                }
            }
            return req_images;
        }
        #endregion
        public ObservableCollection<string> Proxies { get; set; }

        private string GetHtml(string url)
        {
            Uri uri = new Uri(url);
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();

            handler.CookieContainer.Add(uri, new Cookie("name", "value")); // Adding a Cookie
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            client.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            client.DefaultRequestHeaders.Add("Referer", "https://www.coupang.com/np/search?q=%EC%97%AC%EB%A6%84%EC%9B%90%ED%94%BC%EC%8A%A4&component=&filterType=&channel=home_C1&from=home_C1&traid=home_C1&trcid=121830");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "None");
            client.DefaultRequestHeaders.Add("Accept-Language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
            HttpResponseMessage response = client.GetAsync(uri).Result;
            response.EnsureSuccessStatusCode();
            var html = response.Content.ReadAsStringAsync().Result;
            return html;
        }

        private string GetBodyHtml(string url)
        {
            Uri uri = new Uri(url);
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();

            handler.CookieContainer.Add(uri, new Cookie("name", "value")); // Adding a Cookie
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate; // 이 플래그를 요청하면 uri 결과가 gzip으로 compression된 경우 자동으로 압축을 해제해준다. (URI 압축기술 관련)
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "iframe");
            client.DefaultRequestHeaders.Add("Referer", "https://m.coupang.com/vm/products/1388640524?itemId=2424173539&vendorItemId=70418238931&trcid=121830&traid=home_C1&sourceType=srp_product_ads&q=%EC%97%AC%EB%A6%84%EC%9B%90%ED%94%BC%EC%8A%A4&itemsCount=36&searchId=1f399798924b4c678ff316afddbd68ae&rank=4&isAddedCart=");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Accept-Language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
            HttpResponseMessage response = client.GetAsync(uri).Result;
            response.EnsureSuccessStatusCode();
            var html = response.Content.ReadAsStringAsync().Result;
            return html;
        }

        private string GetThumbNail(string url)
        {
            Uri uri = new Uri(url);
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();

            handler.CookieContainer.Add(uri, new Cookie("name", "value")); // Adding a Cookie
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            client.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            client.DefaultRequestHeaders.Add("Referer", "https://www.coupang.com/np/search?q=%EC%97%AC%EB%A6%84%EC%9B%90%ED%94%BC%EC%8A%A4&component=&filterType=&channel=home_C1&from=home_C1&traid=home_C1&trcid=121830");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "None");
            client.DefaultRequestHeaders.Add("Accept-Language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
            HttpResponseMessage response = client.GetAsync(uri).Result;
            response.EnsureSuccessStatusCode();
            var html = response.Content.ReadAsStringAsync().Result;
            return html;
        }

        private string GetHTML(string url)
        {
            Uri uri = new Uri(url);
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();

            handler.CookieContainer.Add(uri, new Cookie("name", "value")); // Adding a Cookie
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            client.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            client.DefaultRequestHeaders.Add("Referer", "https://www.coupang.com/np/search?q=%EC%97%AC%EB%A6%84%EC%9B%90%ED%94%BC%EC%8A%A4&component=&filterType=&channel=home_C1&from=home_C1&traid=home_C1&trcid=121830");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "None");
            client.DefaultRequestHeaders.Add("Accept-Language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
            HttpResponseMessage response = client.GetAsync(uri).Result;
            response.EnsureSuccessStatusCode();
            var html = response.Content.ReadAsStringAsync().Result;
            return html;
        }
        private string GetHotHtml(string url, ShopHttpClientProvider shopHttpClientProvider)
        {




            System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();
            try
            {

                response = shopHttpClientProvider.NewHttpClient().GetAsync(url).Result;
            }
            catch (Exception ex)
            {

            }

            response.EnsureSuccessStatusCode();
            var html = response.Content.ReadAsStringAsync().Result;
            if (html.Contains("<title>▒▒▒▒▒ 차단된 페이지 ▒▒▒▒▒</title>"))
                throw new Exception("Proxy Problem");

            return html;
        }

        private void BtnSliceChecked_Click(object sender, EventArgs e)
        {
            if (DGDetailList.Rows.Count == 0)
            {
                return;
            }

            DialogResult dlg_Result = MessageBox.Show("선택한 이미지를 자동으로 자르시겠습니까?", "상품 이미지 자동 자르기", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Application.UseWaitCursor = true;
                if (!Directory.Exists(ImagePathSlice))
                {
                    Directory.CreateDirectory(ImagePathSlice);
                }

                for (int lst = 0; lst < DGDetailList.SelectedRows.Count; lst++)
                {
                    if (DGDetailList.SelectedRows[lst].Cells["DGDetailList_image"].Value != null)
                    {
                        Image im = (Image)DGDetailList.SelectedRows[lst].Cells["DGDetailList_image"].Value;
                        Bitmap OriginalImage = new Bitmap(im, im.Width, im.Height);
                        List<Rectangle> lstRect = draw_edge_auto(OriginalImage);
                        string srcFileNameOnly = DGDetailList.SelectedRows[lst].Cells["DGDetailList_file_name"].Value.ToString().Replace(".jpg", "");
                        for (int i = 0; i < lstRect.Count; i++)
                        {
                            Rectangle rect_adj = lstRect[i];
                            Bitmap _img = new Bitmap(rect_adj.Width, rect_adj.Height);
                            Graphics g = Graphics.FromImage(_img);
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            g.DrawImage(OriginalImage, 0, 0, rect_adj, GraphicsUnit.Pixel);

                            string saveFileName = ImagePathSlice + srcFileNameOnly + @"_slice_" + (i + 1).ToString() + ".jpg";
                            _img.Save(saveFileName, ImageFormat.Jpeg);
                            DGImageSlicedList.Rows.Add(DGImageSlicedList.Rows.Count + 1, // DGImageSlicedList_no
                                false, // DGImageSlicedList_chk
                                _img, // DGImageSlicedList_image
                                string.Format(srcFileNameOnly + "_slice_" + "{0:D3}" + ".jpg", i + 1), // DGImageSlicedList_file_name
                                string.Format("{0:n0}", _img.Width), // DGImageSlicedList_width
                                string.Format("{0:n0}", _img.Height), // DGImageSlicedList_height
                                saveFileName); // DGImageSlicedList_path


                            Application.DoEvents();


                            //int nTmp = 1;
                            //if (dgcolor_detail.Rows.Count > 0)
                            //{
                            //    string last_file_name = dgcolor_detail.Rows[dgcolor_detail.Rows.Count - 1].Cells["dgcolor_detail_name"].Value.ToString().Trim();
                            //    string strTmp = Regex.Replace(last_file_name, @"\D", string.Empty);
                            //    nTmp = int.Parse(strTmp);
                            //    nTmp++;
                            //}
                            //string detail = string.Format(srcFileNameOnly + "_edit_" + "{0:D3}" + ".jpg", i + 1);
                            //string upload_path = global_var.image_root + "/" + user_id + "/contents/" + site_code + "/" + p_guid + "/" + detail;

                            //saveKtCloud(saveFileName, detail, user_id, site_code, p_guid);
                            //이미지를 올렸으면 삭제한다.
                            //File.Delete(saveFileName);
                        }
                    }
                }

                lblSlice.Text = "슬라이스 이미지 [ " + DGImageSlicedList.Rows.Count + " ]";
                Application.UseWaitCursor = false;
                MessageBox.Show("이미지를 자르기 작업을 완료 하였습니다.", "이미지 자르기 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private List<Rectangle> draw_edge_auto(Bitmap selectedSource)
        {
            //이미지의 경계선을 그리는 모듈                        
            Bitmap bitmapResult = null;
            List<Rectangle> lstRect = new List<Rectangle>();

            if (selectedSource != null)
            {
                bitmapResult = selectedSource.Laplacian3x3Filter(true);

                //이미지의 에지 디텍트 후에 그 결과물로 경계선을 모두 구한뒤 한꺼번에 잘라내고
                //그 결과물은 클라우드에 등록한다.
                lstRect = detect_bound_auto(bitmapResult);
            }

            return lstRect;
        }

        private List<Rectangle> detect_bound_auto(Bitmap bmp)
        {
            List<Rectangle> lstRect = new List<Rectangle>();

            //상 하 검색한다.            
            Rectangle rect2 = new Rectangle(0, 0, bmp.Width, bmp.Height);

            //이미지의 비트를 모두 추출한다.
            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect2, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            int pos_top = 0;
            int pos_bottom = 0;
            int pos_left = 0;
            int pos_right = 0;

            //상하를 오가며 경계를 그린다.
            //상            

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            int numBytes = 0;

            int posY = 0;
            //좌
            pos_left = 0;

            //우
            pos_right = bmp.Width;

            int bottomColorVal = 30;
            while (posY < bmp.Height)
            {
                bool found_top = false;
                for (int y = posY; y < bmp.Height; y++)
                {
                    if (found_top)
                    {
                        break;
                    }
                    else
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            numBytes = (y * (bmp.Width * 4)) + (x * 4);

                            if (rgbValues[numBytes] > 90 && rgbValues[numBytes + 1] > 90 && rgbValues[numBytes + 2] > 90)
                            {
                                //Blue, Green, Red 순서임
                                //rgbValues[numBytes] = 0;
                                //rgbValues[numBytes + 1] = 0;
                                //rgbValues[numBytes + 2] = 0;
                                pos_top = y;
                                found_top = true;
                                break;
                            }
                        }
                    }
                }


                //하
                //폭의 60% 이상부터 폭의 120%까지 뒤져서 경계선이 없어질때까지 뒤진다.
                //빈곳을 못찾으면 120% 지점이 자르는 지점이다.

                //마지막 부분을 항상 염두해 두어야 한다.
                int scanStartY = posY + Convert.ToInt32((bmp.Width * 0.6));
                int scanEndY = posY + Convert.ToInt32((bmp.Width * 1.1));

                //마지막인지 판별
                if (scanEndY > bmp.Height)
                {
                    //마지막인 경우는 검색 하지 않고 남은 부분만 영역으로 지정해 준다.                    
                    Rectangle rect = new Rectangle(pos_left, pos_bottom, pos_right - pos_left, bmp.Height - pos_bottom);
                    if (bmp.Height - pos_bottom > 250)
                    {
                        lstRect.Add(rect);
                    }

                    posY = bmp.Height + 1;
                }
                else
                {
                    bool found_bottom = false;

                    //수정 알고리즘. 위에서 아래로
                    for (int y = scanEndY; y >= scanStartY; y--)
                    {
                        if (found_bottom)
                        {
                            break;
                        }
                        else
                        {
                            //가로 픽셀을 검사한다.
                            //만약 가로 픽셀 중 하나라도 색상값이 발견되면 배경이 아니다
                            //따라서 가로가 다 블랙이어야 한다.
                            bool bk = true;
                            for (int x = 0; x < bmp.Width; x++)
                            {
                                numBytes = (y * (bmp.Width * 4)) + (x * 4);

                                if (rgbValues[numBytes] > bottomColorVal && rgbValues[numBytes + 1] > bottomColorVal && rgbValues[numBytes + 2] > bottomColorVal)
                                //if (rgbValues[numBytes] > 255 && rgbValues[numBytes + 1] > 255 && rgbValues[numBytes + 2] > 255)
                                {
                                    //Blue, Green, Red 순서임
                                    //rgbValues[numBytes] = 0;
                                    //rgbValues[numBytes + 1] = 0;
                                    //rgbValues[numBytes + 2] = 0;
                                    //pos_bottom = pos_top + y;
                                    //found_bottom = true;
                                    bk = false;
                                    break;
                                }
                            }
                            if (bk)
                            {
                                found_bottom = true;
                                pos_bottom = y;
                            }
                        }
                    }

                    if (!found_bottom)
                    {
                        scanStartY = posY + Convert.ToInt32((bmp.Width * 0.6));
                        scanEndY = posY + Convert.ToInt32((bmp.Width * 2));
                        for (int y = scanStartY; y < scanEndY; y++)
                        {
                            if (found_bottom)
                            {
                                break;
                            }
                            else
                            {
                                //가로 픽셀을 검사한다.
                                //만약 가로 픽셀 중 하나라도 색상값이 발견되면 배경이 아니다
                                //따라서 가로가 다 블랙이어야 한다.
                                bool bk = true;
                                for (int x = 0; x < bmp.Width; x++)
                                {
                                    numBytes = (y * (bmp.Width * 4)) + (x * 4);

                                    if (rgbValues[numBytes] > bottomColorVal && rgbValues[numBytes + 1] > bottomColorVal && rgbValues[numBytes + 2] > bottomColorVal)
                                    //    if (rgbValues[numBytes] != 0 && rgbValues[numBytes + 1] != 0 && rgbValues[numBytes + 2] != 0)
                                    //if (rgbValues[numBytes] > 255 && rgbValues[numBytes + 1] > 255 && rgbValues[numBytes + 2] > 255)
                                    {

                                        //Blue, Green, Red 순서임
                                        //rgbValues[numBytes] = 0;
                                        //rgbValues[numBytes + 1] = 0;
                                        //rgbValues[numBytes + 2] = 0;
                                        //pos_bottom = pos_top + y;
                                        //found_bottom = true;
                                        //단하나의 픽셀이 블랙이 아니라면 플래그를 세팅하고 벗어난다.
                                        bk = false;
                                        break;
                                    }
                                }
                                if (bk)
                                {
                                    found_bottom = true;
                                    pos_bottom = y;
                                }
                            }
                        }
                    }

                    if (!found_bottom)
                    {
                        pos_bottom = scanEndY;
                    }

                    //create graphic variable
                    //Graphics g = Graphics.FromImage(_img);
                    //int new_height = pos_bottom - pos_top;
                    //int new_width = pos_right - pos_left;
                    Rectangle rect = new Rectangle(pos_left, pos_top, pos_right - pos_left, pos_bottom - pos_top);

                    lstRect.Add(rect);
                    posY = rect.Bottom;
                }

            }

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            //// Unlock the bits.
            bmp.UnlockBits(bmpData);
            return lstRect;
        }

        private void DGImageTempletelList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DGDetailList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                bool val = !(bool)DGDetailList.Rows[0].Cells["DGDetailList_chk"].Value;
                for (int i = 0; i < DGDetailList.Rows.Count; i++)
                {
                    DGDetailList.Rows[i].Cells["DGDetailList_chk"].Value = val;
                }

                lblDetail.Select();
            }
            else if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                DGDetailList.SelectedRows[0].Cells["DGDetailList_chk"].Value = !(bool)DGDetailList.SelectedRows[0].Cells["DGDetailList_chk"].Value;
                lblDetail.Select();
            }
        }

        private void DGDetailList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FormImageView fi = new FormImageView();
            fi.im = (Image)DGDetailList.SelectedRows[0].Cells["DGDetailList_image"].Value;
            fi.Show();
        }

        private void DGImageTempletelList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 2)
            {
                FormImageView fi = new FormImageView();
                fi.im = (Image)DGImageSlicedList.SelectedRows[0].Cells["DGImageSlicedList_image"].Value;
                fi.Show();
            }
            else if(e.ColumnIndex == 3)
            {
                BtnAddToThumbList_Click(null, null);
            }
        }

        private void BtnEditSlice_Click(object sender, EventArgs e)
        {
            if (DGDetailList.Rows.Count == 0)
            {
                return;
            }

            if (DGDetailList.SelectedRows.Count > 0)
            {
                if (!Directory.Exists(ImagePathSlice))
                {
                    Directory.CreateDirectory(ImagePathSlice);
                }

                FormImageTool fit = new FormImageTool();
                fit.ImagePath = ImagePathSlice;
                fit.im = (Image)DGDetailList.SelectedRows[0].Cells["DGDetailList_image"].Value;
                fit.ShowDialog();

                //수동으로 자른 이미지 있을 수 있으므로 갱신해 준다.
                LoadExistImage();
            }
        }
        private void LoadExistImage()
        {
            DGImageSlicedList.Rows.Clear();
            if (Directory.Exists(ImagePathSlice))
            {
                int i = 1;
                WebClient wc = new WebClient();
                DirectoryInfo di = new DirectoryInfo(ImagePathSlice);
                foreach (var item in di.GetFiles())
                {
                    var content = wc.DownloadData(item.FullName);
                    using (var stream = new MemoryStream(content))
                    {
                        Image im = Image.FromStream(stream);
                        DGImageSlicedList.Rows.Add(i++, false, im, item.Name, im.Width, im.Height,
                            item.FullName);
                    }
                }
            }

            lblSlice.Text = "슬라이스 이미지 [ " + DGImageSlicedList.Rows.Count + " ]";

        }
        private void BtnAddToThumbList_Click(object sender, EventArgs e)
        {
            if (DGImageSlicedList.Rows.Count == 0 ||
                DGImageSlicedList.SelectedRows.Count == 0)
            {
                return;
            }

            for (int i = 0; i < DGImageSlicedList.SelectedRows.Count; i++)
            {
                if (DGSelectedList.Rows.Count < 9)
                {
                    DataGridViewRow dgvRow = new DataGridViewRow();
                    dgvRow = CloneWithValues(DGImageSlicedList.SelectedRows[i]);
                    DGSelectedList.Rows.Add(dgvRow);

                    // Slice 폴더에서 Thumb 폴더로 복사
                    try
                    {
                        File.Copy(DGImageSlicedList.SelectedRows[i].Cells["DGImageSlicedList_path"].Value.ToString(),
                        dgvRow.Cells[6].Value.ToString());
                    }
                    catch
                    {

                    }

                }
            }

            for (int i = 0; i < DGSelectedList.Rows.Count; i++)
            {
                DGSelectedList.Rows[i].Cells["DGSelectedList_No"].Value = i + 1;
            }

            lblThumb.Text = "기본 썸네일 [ " + DGSelectedList.Rows.Count + " ]";
        }

        private DataGridViewRow CloneWithValues(DataGridViewRow row)
        {
            DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();
            for (Int32 index = 0; index < row.Cells.Count; index++)
            {
                if (index == 6)
                {
                    clonedRow.Cells[index].Value = row.Cells[index].Value.ToString().Replace("Slice", "Thumb");
                }
                else
                {
                    clonedRow.Cells[index].Value = row.Cells[index].Value;
                }
            }
            return clonedRow;
        }

        private void DGImageSlicedList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                bool val = !(bool)DGImageSlicedList.Rows[0].Cells["DGImageSlicedList_chk"].Value;
                for (int i = 0; i < DGImageSlicedList.Rows.Count; i++)
                {
                    DGImageSlicedList.Rows[i].Cells["DGImageSlicedList_chk"].Value = val;
                }

                lblDetail.Select();
            }
            else if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                DGImageSlicedList.SelectedRows[0].Cells["DGImageSlicedList_chk"].Value = !(bool)DGImageSlicedList.SelectedRows[0].Cells["DGImageSlicedList_chk"].Value;
                lblDetail.Select();
            }
        }

        private void DGSelectedList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FormImageView fi = new FormImageView();
            fi.im = (Image)DGSelectedList.SelectedRows[0].Cells["DGSelectedList_image"].Value;
            fi.Show();
        }

        private void DGSelectedList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                bool val = !(bool)DGSelectedList.Rows[0].Cells["DGSelectedList_chk"].Value;
                for (int i = 0; i < DGSelectedList.Rows.Count; i++)
                {
                    DGSelectedList.Rows[i].Cells["DGSelectedList_chk"].Value = val;
                }

                lblDetail.Select();
            }
            else if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                DGSelectedList.SelectedRows[0].Cells["DGSelectedList_chk"].Value = !(bool)DGSelectedList.SelectedRows[0].Cells["DGSelectedList_chk"].Value;
                lblDetail.Select();
            }
        }

        private void doScrap(string address)
        {
            DialogResult dlg_Result = MessageBox.Show("현재 수집요청 자료의 저작권 및 사용권은 해당 사업자에게 있으며 \r\n" +
                "허가 없이 사용 시 민.형사상의 책임이 있으며 모든 책임은 사용한 당사자에게 있습니다. 동의 하시겠습니까?", "상품 정보 저작권 동의", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                int channel = 0;
                for (int i = 0; i < dg_site_id.Rows.Count; i++)
                {
                    if ((bool)dg_site_id.Rows[i].Cells["dg_site_id_chk"].Value == true)
                    {
                        channel++;
                    }
                }

                if (channel == 0)
                {
                    Application.UseWaitCursor = false;
                    MessageBox.Show("판매채널이 선택되지 않았습니다. 판매할 국가의 아이디를 체크하여 주세요.", "판매채널 없음", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (address != string.Empty)
                {
                    if (address.Contains("coupang.com"))
                    {
                        ScrapCoupang(address);
                    }
                    else if (address.Contains("amorepacificmall.com"))
                    {
                        ScrapAmoremall(address);
                    }
                    else if (address.Contains("misshaus.com/default/"))
                    {
                        ScrapMissHaus(address);
                    }
                    else if (address.Contains("smartstore.naver.com"))
                    {
                        ScrapSmartStore(address);
                    }
                    else if (address.Contains("artboxmall.com"))
                    {
                        if (global_var.userId.Equals("mina.ecremmoce@gmail.com") || global_var.userId.Equals("sales@fashionjc.com")) // 관리자 계정 또는 아트박스 계정만 가능
                        {
                            ScrapArtbox(address);
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
            }   
        }
        private void TxtExternalProductAddr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                string address = TxtExternalProductAddr.Text.Trim();
                if(address != string.Empty)
                {
                    doScrap(address);
                }
            }
        }

        private void BtnImageUp_Click(object sender, EventArgs e)
        {
            if (DGSelectedList.Rows.Count == 0)
            {
                return;
            }

            if (DGSelectedList.SelectedRows[0].Index != 0)
            {
                int cur_idx = DGSelectedList.SelectedRows[0].Index;
                int pre_idx = DGSelectedList.SelectedRows[0].Index - 1;

                DataGridViewRow cur_row = (DataGridViewRow)DGSelectedList.Rows[cur_idx].Clone();
                DataGridViewRow pre_row = (DataGridViewRow)DGSelectedList.Rows[pre_idx].Clone();
                for (int i = 0; i < cur_row.Cells.Count; i++)
                {
                    cur_row.Cells[i].Value = DGSelectedList.Rows[cur_idx].Cells[i].Value;
                    pre_row.Cells[i].Value = DGSelectedList.Rows[pre_idx].Cells[i].Value;
                }
                for (int i = 0; i < DGSelectedList.ColumnCount; i++)
                {
                    DGSelectedList.Rows[pre_idx].Cells[i].Value = cur_row.Cells[i].Value;
                    DGSelectedList.Rows[cur_idx].Cells[i].Value = pre_row.Cells[i].Value;
                }
                DGSelectedList.Rows[pre_idx].Selected = true;
                reIndexNo(DGSelectedList);
            }
        }

        private void BtnImagDn_Click(object sender, EventArgs e)
        {
            if (DGSelectedList.Rows.Count == 0)
            {
                return;
            }

            if (DGSelectedList.SelectedRows[0].Index != DGSelectedList.Rows.Count - 1)
            {
                int cur_idx = DGSelectedList.SelectedRows[0].Index;
                int next_idx = DGSelectedList.SelectedRows[0].Index + 1;

                DataGridViewRow cur_row = (DataGridViewRow)DGSelectedList.Rows[cur_idx].Clone();
                DataGridViewRow next_row = (DataGridViewRow)DGSelectedList.Rows[next_idx].Clone();
                for (int i = 0; i < cur_row.Cells.Count; i++)
                {
                    cur_row.Cells[i].Value = DGSelectedList.Rows[cur_idx].Cells[i].Value;
                    next_row.Cells[i].Value = DGSelectedList.Rows[next_idx].Cells[i].Value;
                }
                for (int i = 0; i < DGSelectedList.ColumnCount; i++)
                {
                    DGSelectedList.Rows[next_idx].Cells[i].Value = cur_row.Cells[i].Value;
                    DGSelectedList.Rows[cur_idx].Cells[i].Value = next_row.Cells[i].Value;
                }
                DGSelectedList.Rows[next_idx].Selected = true;
                reIndexNo(DGSelectedList);
            }
        }

        private void BtnImageTop_Click(object sender, EventArgs e)
        {
            if (DGSelectedList.Rows.Count == 0)
            {
                return;
            }

            if (DGSelectedList.SelectedRows[0].Index != 0)
            {
                DataGridViewRow cur_row = (DataGridViewRow)DGSelectedList.SelectedRows[0].Clone();

                for (int i = 0; i < cur_row.Cells.Count; i++)
                {
                    cur_row.Cells[i].Value = DGSelectedList.SelectedRows[0].Cells[i].Value;
                }

                DGSelectedList.Rows.RemoveAt(DGSelectedList.SelectedRows[0].Index);
                DGSelectedList.Rows.Insert(0, cur_row);
                DGSelectedList.Rows[0].Selected = true;
                reIndexNo(DGSelectedList);
            }
        }

        private void BtnImageBottom_Click(object sender, EventArgs e)
        {
            if (DGSelectedList.Rows.Count == 0)
            {
                return;
            }

            if (DGSelectedList.Rows.Count > 0 && DGSelectedList.SelectedRows.Count > 0)
            {
                if (DGSelectedList.SelectedRows[0].Index != DGSelectedList.Rows.Count - 1)
                {
                    DataGridViewRow cur_row = (DataGridViewRow)DGSelectedList.SelectedRows[0].Clone();

                    for (int i = 0; i < cur_row.Cells.Count; i++)
                    {
                        cur_row.Cells[i].Value = DGSelectedList.SelectedRows[0].Cells[i].Value;
                    }

                    DGSelectedList.Rows.RemoveAt(DGSelectedList.SelectedRows[0].Index);
                    DGSelectedList.Rows.Add(cur_row);
                    DGSelectedList.Rows[DGSelectedList.Rows.Count - 1].Selected = true;

                    reIndexNo(DGSelectedList);
                }
            }
        }

        private void reIndexNo(DataGridView dv)
        {
            for (int i = 0; i < dv.Rows.Count; i++)
            {
                dv.Rows[i].Cells[0].Value = i + 1;
            }
        }

        private void BtnClearImageTemplate_Click(object sender, EventArgs e)
        {
            if (DGImageTempletelList.Rows.Count == 0)
            {
                return;
            }

            DialogResult dlg_Result = MessageBox.Show("선택한 템플릿 이미지를 삭제 하시겠습니까?", "템플릿 이미지 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                List<string> FileList = new List<string>();
                for (int i = 0; i < DGImageTempletelList.SelectedRows.Count; i++)
                {
                    FileList.Add(DGImageTempletelList.SelectedRows[i].Cells["DGImageTempletelList_file_name"].Value.ToString());
                }
                DGImageTempletelList.Rows.Clear();

                //파일을 지운다.
                for (int i = 0; i < FileList.Count; i++)
                {
                    File.Delete(TemplateImagePath + @"\" + FileList[i].ToString());
                }

                LoadTemplateImage();

                lblTemplate.Text = "템플릿 이미지 [ " + DGImageTempletelList.Rows.Count + " ]";
            }

        }

        private void BtnSelectImageTemplate_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = "jpg";
            openFileDlg.Filter = "Image Files(*.jpg;*.png)|*.jpg;*.png";
            openFileDlg.Multiselect = true;
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                //DGSelectedList.Rows.Clear();
                if (!Directory.Exists(TemplateImagePath))
                {
                    Directory.CreateDirectory(TemplateImagePath);
                }
                WebClient wc = new WebClient();
                int startIdx = DGImageTempletelList.Rows.Count;
                for (int i = 0; i < openFileDlg.FileNames.Length; i++)
                {
                    string fileName = Path.GetFileName(openFileDlg.FileNames[i]);
                    string fileExt = Path.GetExtension(openFileDlg.FileNames[i]);
                    string destPath = TemplateImagePath + @"\" + $"template_{startIdx + 1:000}" + fileExt;
                    File.Copy(openFileDlg.FileNames[i], destPath);
                    var content = wc.DownloadData(destPath);
                    using (var stream = new MemoryStream(content))
                    {
                        Image im = Image.FromStream(stream);
                        DGImageTempletelList.Rows.Add(startIdx + 1, false, im, $"template_{startIdx + 1:000}.{fileExt}", im.Width, im.Height, destPath);
                    }
                    startIdx++;
                }

                lblTemplate.Text = "템플릿 이미지 [ " + DGImageTempletelList.Rows.Count + " ]";
            }
        }

        private void BtnDeleteCheckedSlice_Click(object sender, EventArgs e)
        {
            if (DGImageSlicedList.Rows.Count == 0 ||
                DGImageSlicedList.SelectedRows.Count == 0)
            {
                return;
            }

            DialogResult dlg_Result = MessageBox.Show("선택한 슬라이스 이미지를 목록에서 삭제 하시겠습니까?", "슬라이스 이미지 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                List<string> FileList = new List<string>();
                for (int i = DGImageSlicedList.SelectedRows.Count -1; i >= 0; i--)
                {
                    string path = DGImageSlicedList.Rows[DGImageSlicedList.SelectedRows[i].Index].Cells["DGImageSlicedList_path"].Value.ToString();
                    DGImageSlicedList.Rows.RemoveAt(DGImageSlicedList.SelectedRows[i].Index);
                    //파일을 실제로 삭제한다.
                    File.Delete(path);
                }

                reIndexNo(DGImageSlicedList);

                lblSlice.Text = "슬라이스 이미지 [ " + DGImageSlicedList.Rows.Count + " ]";
            }
        }

        private void DGImageTempletelList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                bool val = !(bool)DGImageTempletelList.Rows[0].Cells["DGImageTempletelList_chk"].Value;
                for (int i = 0; i < DGImageTempletelList.Rows.Count; i++)
                {
                    DGImageTempletelList.Rows[i].Cells["DGImageTempletelList_chk"].Value = val;
                }

                lblDetail.Select();
            }
            else if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                DGImageTempletelList.SelectedRows[0].Cells["DGImageTempletelList_chk"].Value = !(bool)DGImageTempletelList.SelectedRows[0].Cells["DGImageTempletelList_chk"].Value;
                lblDetail.Select();
            }
        }

        private void BtnTemplateAddToThumb_Click(object sender, EventArgs e)
        {
            if (DGImageTempletelList.Rows.Count == 0 || DGImageTempletelList.SelectedRows.Count == 0)
            {
                return;
            }

            for (int i = 0; i < DGImageTempletelList.SelectedRows.Count; i++)
            {
                if (DGSelectedList.Rows.Count < 9)
                {
                    DataGridViewRow dgvRow = new DataGridViewRow();
                    dgvRow = CloneWithValues(DGImageTempletelList.SelectedRows[i]);
                    DGSelectedList.Rows.Add(dgvRow);
                }
            }

            reIndexNo(DGSelectedList);

            lblThumb.Text = "기본 썸네일 [ " + DGSelectedList.Rows.Count + " ]";
        }

        private void DGImageTempletelList_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 2)
            {
                FormImageView fi = new FormImageView();
                fi.im = (Image)DGImageTempletelList.SelectedRows[0].Cells["DGImageTempletelList_image"].Value;
                fi.Show();
            }
            else if(e.ColumnIndex == 3)
            {
                BtnTemplateAddToThumb_Click(null, null);
            }
        }

        private void BtnDeleteCheckedDetail_Click(object sender, EventArgs e)
        {
            if (DGDetailList.Rows.Count == 0)
            {
                return;
            }

            DialogResult dlg_Result = MessageBox.Show("선택한 상세 이미지를 목록에서 삭제 하시겠습니까?", "상세 이미지 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                List<string> FileList = new List<string>();
                for (int i = DGDetailList.SelectedRows.Count - 1; i >= 0; i--)
                {
                    DGDetailList.Rows.RemoveAt(DGDetailList.SelectedRows[i].Index);
                }

                reIndexNo(DGDetailList);

                lblSlice.Text = "슬라이스 이미지 [ " + DGDetailList.Rows.Count + " ]";
            }
        }

        private void menu_thumb_check_clear_Click(object sender, EventArgs e)
        {
            BtnImageClear_Click(null, null);
        }

        private void menu_thumb_select_image_Click(object sender, EventArgs e)
        {
            BtnSelectImageFile_Click(null, null);
        }

        private void menu_thumb_go_top_Click(object sender, EventArgs e)
        {
            BtnImageTop_Click(null, null);
        }

        private void menu_thumb_go_up_Click(object sender, EventArgs e)
        {
            BtnImageUp_Click(null, null);
        }

        private void menu_thumb_go_dn_Click(object sender, EventArgs e)
        {
            BtnImagDn_Click(null, null);
        }

        private void menu_thumb_go_bottom_Click(object sender, EventArgs e)
        {
            BtnImageBottom_Click(null, null);
        }

        private void menu_detail_delete_checked_Click(object sender, EventArgs e)
        {
            BtnDeleteCheckedDetail_Click(null, null);
        }

        private void menu_detail_auto_slice_Click(object sender, EventArgs e)
        {
            BtnSliceChecked_Click(null, null);
        }

        private void menu_detail_manual_slice_Click(object sender, EventArgs e)
        {
            BtnEditSlice_Click(null, null);
        }

        private void menu_slice_checked_delete_Click(object sender, EventArgs e)
        {
            BtnDeleteCheckedSlice_Click(null, null);
        }

        private void menu_slice_add_to_thumb_Click(object sender, EventArgs e)
        {
            BtnAddToThumbList_Click(null, null);
        }

        private void menu_template_select_file_Click(object sender, EventArgs e)
        {
            BtnSelectImageTemplate_Click(null, null);
        }

        private void menu_template_checked_delete_Click(object sender, EventArgs e)
        {
            BtnClearImageTemplate_Click(null, null);
        }

        private void menu_template_add_to_thumb_Click(object sender, EventArgs e)
        {
            BtnTemplateAddToThumb_Click(null, null);
        }
        private void calcRowPrice(int rowId, string cellName, string tarCountry)
        {
            //등록기에서는 등록을 위하여 공급가로부터 시작하여 상품 판매가를 계산한다.

            decimal sourcePrice = 0;
            if (DgVariation.Rows[rowId].Cells["DgSrcVariation_supply_price"].Value != null &&
                DgVariation.Rows[rowId].Cells["DgSrcVariation_supply_price"].Value.ToString().Replace(",", "") != string.Empty)
            {
                sourcePrice = Convert.ToDecimal(DgVariation.Rows[rowId].Cells["DgSrcVariation_supply_price"].Value.ToString().Replace(",", ""));
            }

            double productWeight = 0;

            if (DgVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Value != null &&
                DgVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Value.ToString().Replace(",", "") != string.Empty)
            {
                double dWeight = Convert.ToDouble(DgVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Value.ToString().Replace(",", ""));
                productWeight = dWeight * 1000;
            }

            int qty = 0;
            if (DgVariation.Rows[rowId].Cells["DgSrcVariation_stock"].Value != null &&
                DgVariation.Rows[rowId].Cells["DgSrcVariation_stock"].Value.ToString().Replace(",", "") != string.Empty)
            {
                qty = Convert.ToInt32(DgVariation.Rows[rowId].Cells["DgSrcVariation_stock"].Value.ToString().Replace(",", ""));
            }


            decimal margin = 0;
            if (DgVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Value != null &&
                DgVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Value.ToString().Replace(",", "") != string.Empty)
            {
                margin = Convert.ToDecimal(DgVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Value.ToString().Replace(",", ""));
            }


            if (margin > 0)
            {
                DgVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Style.BackColor = Color.GreenYellow;
            }
            else
            {
                DgVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Style.BackColor = Color.Orange;
            }

            if (productWeight > 0)
            {
                DgVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Style.BackColor = Color.GreenYellow;
            }
            else
            {
                DgVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Style.BackColor = Color.Orange;
            }

            if (productWeight > 0)
            {
                DgVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Style.BackColor = Color.GreenYellow;
            }
            else
            {
                DgVariation.Rows[rowId].Cells["DgSrcVariation_weight"].Style.BackColor = Color.Orange;
            }

            if (qty > 0)
            {
                DgVariation.Rows[rowId].Cells["DgSrcVariation_stock"].Style.BackColor = Color.GreenYellow;
            }
            else
            {
                DgVariation.Rows[rowId].Cells["DgSrcVariation_stock"].Style.BackColor = Color.Orange;
            }
            //판매국가의 환율 //DB에서 가지고 온다.

            decimal rateSrc = 0;
            using (AppDbContext context = new AppDbContext())
            {
                CurrencyRate currencyRate = context.CurrencyRates.FirstOrDefault
                    (x => x.cr_code.Contains(tarCountry) && x.UserId == global_var.userId);
                if(currencyRate == null)
                {
                    MessageBox.Show("환율정보가 없습니다. 환율을 업데이트 해 주세요.","환율정보 업데이트",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    rateSrc = currencyRate.cr_exchange;
                }
            }

            
            if (sourcePrice > 0 && margin >= 0 && productWeight > 0 && rateSrc > 0)
            {
                PriceCalculator pCalc = new PriceCalculator();
                pCalc.CountryCode = tarCountry;
                pCalc.SourcePrice = sourcePrice;
                pCalc.Margin = margin;
                pCalc.Weight = productWeight;
                pCalc.CurrencyRate = rateSrc;
                pCalc.ShopeeRate = UdShopeeFee.Value;
                pCalc.PgFeeRate = udPGFee.Value;
                pCalc.RetailPriceRate = UdRetailPriceRate.Value;

                Dictionary<string, decimal> calcResult = new Dictionary<string, decimal>();
                calcResult = pCalc.calculatePrice();

                DgVariation.Rows[rowId].Cells["DgSrcVariation_pg_fee"].Value = string.Format("{0:n2}", calcResult["pgFee"]);
                DgVariation.Rows[rowId].Cells["DgSrcVariation_src_price"].Value = string.Format("{0:n1}", calcResult["targetSellPrice"]);
                DgVariation.Rows[rowId].Cells["DgSrcVariation_price_won"].Value = string.Format("{0:n0}", calcResult["targetSellPriceKRW"]);
                DgVariation.Rows[rowId].Cells["DgSrcVariation_src_original_price"].Value = string.Format("{0:n1}", calcResult["targetRetailPrice"]);

                if (calcResult["pgFee"] > 0)
                {
                    DgVariation.Rows[rowId].Cells["DgSrcVariation_pg_fee"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    DgVariation.Rows[rowId].Cells["DgSrcVariation_pg_fee"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetSellPrice"] > 0)
                {
                    DgVariation.Rows[rowId].Cells["DgSrcVariation_src_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    DgVariation.Rows[rowId].Cells["DgSrcVariation_src_price"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetSellPriceKRW"] > 0)
                {
                    DgVariation.Rows[rowId].Cells["DgSrcVariation_price_won"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    DgVariation.Rows[rowId].Cells["DgSrcVariation_price_won"].Style.BackColor = Color.Orange;
                }

                if (calcResult["targetRetailPrice"] > 0)
                {
                    DgVariation.Rows[rowId].Cells["DgSrcVariation_src_original_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    DgVariation.Rows[rowId].Cells["DgSrcVariation_src_original_price"].Style.BackColor = Color.Orange;
                }

                //마지막으로 입력한 값이 숫자니까 콤마 찍어주기
                string strVar = DgVariation.Rows[rowId].Cells[cellName].Value.ToString().Replace(",", "");
                decimal Var = 0;
                if (decimal.TryParse(strVar, out Var))
                {
                    if(cellName != "DgSrcVariation_weight")
                    {
                        DgVariation.Rows[rowId].Cells[cellName].Value = string.Format("{0:n0}", Var);
                    }
                }

                if (sourcePrice > 0)
                {
                    DgVariation.Rows[rowId].Cells["DgSrcVariation_supply_price"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    DgVariation.Rows[rowId].Cells["DgSrcVariation_supply_price"].Style.BackColor = Color.Orange;
                }

                if (margin > 0)
                {
                    DgVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    DgVariation.Rows[rowId].Cells["DgSrcVariation_margin"].Style.BackColor = Color.Orange;
                }

            }
        }

        private Dictionary<string, decimal> calcRowPrice(decimal sourcePrice, decimal margin, int weight, string tarCountry)
        {
            //판매국가의 환율
            decimal rateSrc = 0;
            using (AppDbContext context = new AppDbContext())
            {
                CurrencyRate currencyRate = context.CurrencyRates.FirstOrDefault
                    (x => x.cr_code.Contains(tarCountry) && x.UserId == global_var.userId);
                if (currencyRate != null)
                {
                    rateSrc = currencyRate.cr_exchange;
                }
            }

            Dictionary<string, decimal> calcResult = new Dictionary<string, decimal>();
            if (sourcePrice > 0 && margin >= 0 && weight > 0)
            {
                PriceCalculator pCalc = new PriceCalculator();
                pCalc.CountryCode = tarCountry;
                pCalc.SourcePrice = sourcePrice;
                pCalc.Margin = margin;
                pCalc.Weight = weight;
                pCalc.CurrencyRate = rateSrc;
                pCalc.ShopeeRate = UdShopeeFee.Value;
                pCalc.PgFeeRate = udPGFee.Value;
                pCalc.RetailPriceRate = UdRetailPriceRate.Value;                
                calcResult = pCalc.calculatePrice();
            }

            return calcResult;
        }



        private void DgVariation_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (DgVariation.Rows.Count > 0)
            {
                if ((e.ColumnIndex == 9 || e.ColumnIndex == 10 || e.ColumnIndex == 11) && e.RowIndex > -1)
                {
                    if(DgVariation.Rows[e.RowIndex].Cells["DgSrcVariation_tar_country"].Value != null)
                    {
                        string tarCountry = DgVariation.Rows[e.RowIndex].Cells["DgSrcVariation_tar_country"].Value.ToString();
                        calcRowPrice(e.RowIndex, DgVariation.Columns[e.ColumnIndex].Name, tarCountry);
                    }
                }
                else if(e.ColumnIndex == 6)
                {
                    if(DgVariation.Rows[e.RowIndex].Cells["DgSrcVariation_variation_sku"].Value != null)
                    {
                        string str_sku = DgVariation.Rows[e.RowIndex].Cells["DgSrcVariation_variation_sku"].Value.ToString();
                        string option_name = DgVariation.Rows[e.RowIndex].Cells["DgSrcVariation_Name_kor"].Value.ToString();
                        for (int i = 0; i < DgVariation.Rows.Count; i++)
                        {
                            if (DgVariation.Rows[i].Cells["DgSrcVariation_Name_kor"].Value.ToString() == option_name)
                            {
                                DgVariation.Rows[i].Cells["DgSrcVariation_variation_sku"].Value = str_sku;
                            }
                        }
                    }
                }
                else if(e.ColumnIndex == 8)
                {
                    if(DgVariation.Rows[e.RowIndex].Cells["DgSrcVariation_Name"].Value != null)
                    {
                        string option_nameKor = DgVariation.Rows[e.RowIndex].Cells["DgSrcVariation_Name_kor"].Value.ToString();
                        string option_name = DgVariation.Rows[e.RowIndex].Cells["DgSrcVariation_Name"].Value.ToString();
                        for (int i = 0; i < DgVariation.Rows.Count; i++)
                        {
                            if (DgVariation.Rows[i].Cells["DgSrcVariation_Name_kor"].Value.ToString() == option_nameKor)
                            {
                                DgVariation.Rows[i].Cells["DgSrcVariation_Name"].Value = option_name;
                            }
                        }
                    }
                }
            }
        }

        private void btnSaveMargin_Click(object sender, EventArgs e)
        {
            if(DgPrice.Rows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < DgPrice.Rows.Count; i++)
                {
                    DgPrice.Rows[i].Cells["DgPrice_margin"].Value = string.Format("{0:n0}", UdMargin.Value);
                }

                

                for (int i = 0; i < DgVariation.Rows.Count; i++)
                {
                    DgVariation.Rows[i].Cells["DgSrcVariation_margin"].Value = string.Format("{0:n0}", UdMargin.Value);
                }
                //BtnCalcSellPrice_Click(null, null);
                Cursor.Current = Cursors.Default;
                //MessageBox.Show("마진을 모두 적용하였습니다.", "마진적용", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnApplyQty_Click(object sender, EventArgs e)
        {
            if(DgPrice.Rows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < DgPrice.Rows.Count; i++)
                {
                    DgPrice.Rows[i].Cells["DgPrice_qty"].Value = string.Format("{0:n0}", UdQty.Value);
                }
                

                for (int i = 0; i < DgVariation.Rows.Count; i++)
                {
                    DgVariation.Rows[i].Cells["DgSrcVariation_stock"].Value = string.Format("{0:n0}", UdQty.Value);
                }
                Cursor.Current = Cursors.Default;
                //MessageBox.Show("수량을 모두 적용하였습니다.", "수량적용", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnApplyWeight_Click(object sender, EventArgs e)
        {
            if (DgPrice.Rows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < DgPrice.Rows.Count; i++)
                {
                    DgPrice.Rows[i].Cells["DgPrice_weight"].Value = string.Format("{0:n2}", UdWeight.Value);
                }

                
                for (int i = 0; i < DgVariation.Rows.Count; i++)
                {
                    DgVariation.Rows[i].Cells["DgSrcVariation_weight"].Value = string.Format("{0:n2}", UdWeight.Value);
                }
                Cursor.Current = Cursors.Default;
                //MessageBox.Show("무게를 모두 적용하였습니다.", "무게적용", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCopySameName_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 0; i < DgVariation.Rows.Count; i++)
            {
                if (DgVariation.Rows[i].Cells["DgSrcVariation_Name_kor"].Value != null)
                {
                    DgVariation.Rows[i].Cells["DgSrcVariation_Name"].Value = DgVariation.Rows[i].Cells["DgSrcVariation_Name_kor"].Value.ToString();
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void BtnTranslateOptionName_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("수집한 옵션명을 번역하시겠습니까?", "옵션명 번역", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            Cursor.Current = Cursors.WaitCursor;
            string tar_lang = "";
            if (cboLang2.Text == "인도네시아어")
            {
                tar_lang = "id";
            }
            else if (cboLang2.Text == "영어")
            {
                tar_lang = "en";
            }
            else if (cboLang2.Text == "말레이시아어")
            {
                tar_lang = "ms";
            }
            else if (cboLang2.Text == "태국어")
            {
                tar_lang = "th";
            }
            else if (cboLang2.Text == "중국어 번체")
            {
                tar_lang = "zh-TW";
            }
            else if (cboLang2.Text == "베트남어")
            {
                tar_lang = "vi";
            }

            Cursor.Current = Cursors.Default;


            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < DgVariation.SelectedRows.Count; i++)
                {
                    if (DgVariation.SelectedRows[i].Cells["DgSrcVariation_Name_kor"].Value != null)
                    {
                        string translated = translate(DgVariation.SelectedRows[i].Cells["DgSrcVariation_Name_kor"].Value.ToString(), "ko", tar_lang);
                        DgVariation.SelectedRows[i].Cells["DgSrcVariation_Name"].Value = HttpUtility.HtmlDecode(translated.Replace(" ", ""));

                        if (translated.Length > 20)
                        {
                            DgVariation.SelectedRows[i].Cells["DgSrcVariation_Name"].Style.BackColor = Color.Orange;
                        }
                        else
                        {
                            DgVariation.SelectedRows[i].Cells["DgSrcVariation_Name"].Style.BackColor = Color.GreenYellow;
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("옵션명을 모두 번역하였습니다.", "옵션명 번역", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("상품을 등록 하시겠습니까?", "상품등록", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                //Application.UseWaitCursor = true;
                string end_point = "https://partner.shopeemobile.com/api/v1/item/add";

                bool validateTitle = true;
                for (int i = 0; i < dg_productTitle.Rows.Count; i++)
                {
                    if (dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString().Trim() == string.Empty)
                    {
                        validateTitle = false;
                        break;
                    }
                }
                if (!validateTitle)
                {
                    Application.UseWaitCursor = false;
                    MessageBox.Show("상품명이 입력되지 않았습니다.", "상품명 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    TabMain.SelectedIndex = 0;
                    return;
                }

                if (DGSelectedList.Rows.Count == 0)
                {
                    Application.UseWaitCursor = false;
                    MessageBox.Show("상품이미지가 없습니다.", "상품이미지 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TabMain.SelectedIndex = 1;
                    return;
                }


                if (DgPrice.Rows.Count == 1)
                {
                    if (DgPrice.Rows[0].Cells["DgPrice_sell_price"].Value == null ||
                        DgPrice.Rows[0].Cells["DgPrice_sell_price"].Value.ToString().Trim() == string.Empty)
                    {
                        Application.UseWaitCursor = false;
                        MessageBox.Show("상품판매가격이 없습니다.", "상품판매가 누락",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TabMain.SelectedIndex = 3;
                        return;
                    }

                    if (DgPrice.Rows[0].Cells["DgPrice_retail_price"].Value == null ||
                        DgPrice.Rows[0].Cells["DgPrice_retail_price"].Value.ToString().Trim() == string.Empty)
                    {
                        Application.UseWaitCursor = false;
                        MessageBox.Show("상품 소비자가격이 없습니다.", "소비자가격 누락",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TabMain.SelectedIndex = 3;
                        return;
                    }

                    if (DgPrice.Rows[0].Cells["DgPrice_qty"].Value == null ||
                        DgPrice.Rows[0].Cells["DgPrice_qty"].Value.ToString().Trim() == string.Empty)
                    {
                        Application.UseWaitCursor = false;
                        MessageBox.Show("상품 수량이 없습니다.", "상품 수량 누락",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TabMain.SelectedIndex = 3;
                        return;
                    }

                    if (DgPrice.Rows[0].Cells["DgPrice_weight"].Value == null ||
                        DgPrice.Rows[0].Cells["DgPrice_weight"].Value.ToString().Trim() == string.Empty)
                    {
                        Application.UseWaitCursor = false;
                        MessageBox.Show("상품 무게가 없습니다.", "상품 무게 누락",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TabMain.SelectedIndex = 3;
                        return;
                    }
                }

                //부모 sku 검증
                bool isValidateParentOption = true;
                for (int i = 0; i < DgPrice.Rows.Count; i++)
                {
                    if (DgPrice.Rows[i].Cells["DgPrice_parent_sku"].Value == null || DgPrice.Rows[i].Cells["DgPrice_parent_sku"].Value.ToString().Trim() == string.Empty)
                    {
                        isValidateParentOption = false;
                        TabMain.SelectedIndex = 3;
                        Application.UseWaitCursor = false;
                        MessageBox.Show("상품SKU가 누락되었습니다.", "상품SKU 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);                        
                        break;
                    }

                    if (DgPrice.Rows[i].Cells["DgPrice_discount_name"].Value == null || DgPrice.Rows[i].Cells["DgPrice_discount_name"].Value.ToString().Trim() == string.Empty)
                    {
                        isValidateParentOption = false;
                        TabMain.SelectedIndex = 3;
                        Application.UseWaitCursor = false;
                        MessageBox.Show("할인이벤트가 누락되었습니다.", "할인이벤트 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                }

                if (!isValidateParentOption)
                {
                    return;
                }

                bool isValidateOption = true;
                //옵션이 있는 경우에 검증한다.
                if (DgVariation.Rows.Count > 0)
                {
                    for (int i = 0; i < DgVariation.Rows.Count - 2; i++)
                    {
                        if (DgVariation.Rows[i].Cells["DgSrcVariation_Name"].Value == null || DgVariation.Rows[i].Cells["DgSrcVariation_Name"].Value.ToString().Trim() == string.Empty)
                        {
                            isValidateOption = false;
                            TabMain.SelectedIndex = 3;
                            Application.UseWaitCursor = false;
                            MessageBox.Show("옵션명이 누락되었습니다.", "옵션명 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }

                        if (DgVariation.Rows[i].Cells["DgSrcVariation_variation_sku"].Value == null || DgVariation.Rows[i].Cells["DgSrcVariation_variation_sku"].Value.ToString().Trim() == string.Empty)
                        {
                            isValidateOption = false;
                            TabMain.SelectedIndex = 3;
                            Application.UseWaitCursor = false;
                            MessageBox.Show("옵션SKU가 누락되었습니다.", "옵션SKU 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }

                        else if (DgVariation.Rows[i].Cells["DgSrcVariation_src_price"].Value == null || DgVariation.Rows[i].Cells["DgSrcVariation_src_price"].Value.ToString().Trim() == string.Empty)
                        {
                            isValidateOption = false;
                            TabMain.SelectedIndex = 3;
                            Application.UseWaitCursor = false;
                            MessageBox.Show("판매가격이 누락되었습니다.", "판매가격 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                        else if (DgVariation.Rows[i].Cells["DgSrcVariation_src_original_price"].Value == null || DgVariation.Rows[i].Cells["DgSrcVariation_src_original_price"].Value.ToString().Trim() == string.Empty)
                        {
                            isValidateOption = false;
                            TabMain.SelectedIndex = 3;
                            Application.UseWaitCursor = false;
                            MessageBox.Show("소비자가격이 누락되었습니다.", "소비자가격 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                        else if (DgVariation.Rows[i].Cells["DgSrcVariation_stock"].Value == null || DgVariation.Rows[i].Cells["DgSrcVariation_stock"].Value.ToString().Trim() == string.Empty)
                        {
                            isValidateOption = false;
                            TabMain.SelectedIndex = 3;
                            Application.UseWaitCursor = false;
                            MessageBox.Show("재고수량이 누락되었습니다.", "재고수량 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                        else if (DgVariation.Rows[i].Cells["DgSrcVariation_weight"].Value == null || DgVariation.Rows[i].Cells["DgSrcVariation_weight"].Value.ToString().Trim() == string.Empty)
                        {
                            isValidateOption = false;
                            TabMain.SelectedIndex = 3;
                            Application.UseWaitCursor = false;
                            MessageBox.Show("옵션무게가 누락되었습니다.", "옵션무게 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                        else if (DgVariation.Rows[i].Cells["DgSrcVariation_discount_name"].Value == null || DgVariation.Rows[i].Cells["DgSrcVariation_discount_name"].Value.ToString().Trim() == string.Empty)
                        {
                            isValidateOption = false;
                            TabMain.SelectedIndex = 3;
                            Application.UseWaitCursor = false;
                            MessageBox.Show("할인이벤트가 누락되었습니다.", "할인이벤트 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                    }

                    if (!isValidateOption)
                    {
                        return;
                    }
                }

                if (txtPreview.Text.Trim() == string.Empty)
                {
                    Application.UseWaitCursor = false;
                    MessageBox.Show("상품설명이 입력되지 않았습니다.", "상품 설명 누락",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TabMain.SelectedIndex = 4;
                    return;
                }

                if (DgSaveAttribute.Rows.Count == 0)
                {
                    Application.UseWaitCursor = false;
                    MessageBox.Show("등록 대상의 카테고리가 선택되지 않았습니다.", "등록 카테고리 선택",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TabMain.SelectedIndex = 2;
                    return;
                }

                bool isValidateAttribute = true;
                for (int i = 0; i < DgAttribute.Rows.Count; i++)
                {
                    if ((bool)DgAttribute.Rows[i].Cells["DgTarAttribute_is_mandatory"].Value)
                    {
                        if (DgAttribute.Rows[i].Cells["DgAttribute_attribute_value"].Value == null ||
                            DgAttribute.Rows[i].Cells["DgAttribute_attribute_value"].Value.ToString() == string.Empty)
                        {
                            isValidateAttribute = false;
                            break;
                        }
                    }
                }

                if (!isValidateAttribute)
                {
                    Application.UseWaitCursor = false;
                    MessageBox.Show("필수 속성값이 입력되지 않았습니다.", "필수입력값 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TabMain.SelectedIndex = 2;

                    return;
                }

                List<shopee_image> lst_img = new List<shopee_image>();
                string upload_path = global_var.image_root + "/" + pGuid + "/" + "thumb";

                string OS_endpoint = "https://ssproxy2.ucloudbiz.olleh.com/v1/" + global_var.ucloudbiz_account + "/sto_sm/shopee_sm/image_upload_temp/" + pGuid;
                string SaveImagePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\ShopeeManagement\ItemImages\UploadTemp\";

                if (!Directory.Exists(SaveImagePath))
                {
                    Directory.CreateDirectory(SaveImagePath);
                }
                else
                {
                    //폴더를 비운다
                    DirectoryInfo di = new DirectoryInfo(SaveImagePath);
                    foreach (var item in di.GetFiles())
                    {
                        File.Delete(item.FullName);
                    }
                }

                int maxImageCount = 9;

                if(DGSelectedList.Rows.Count < maxImageCount)
                {
                    maxImageCount = DGSelectedList.Rows.Count;
                }

                for (int img = 0; img < maxImageCount; img++)
                {
                    string fileName = DGSelectedList.Rows[img].Cells["DGSelectedList_FileName"].Value.ToString().Trim();
                    string filePath = DGSelectedList.Rows[img].Cells["DGSelectedList_path"].Value.ToString().Trim();
                    Bitmap bm = (Bitmap)DGSelectedList.Rows[img].Cells["DGSelectedList_Image"].Value;
                    Bitmap NewImg = new Bitmap(bm);

                    //파일을 카피한다.
                    File.Copy(filePath, SaveImagePath + fileName);
                    //NewImg.Save(SaveImagePath + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //bm.Save(SaveImagePath + fileName);

                    string save_path = SaveImagePath + fileName;
                    using (FileStream fs = new FileStream(save_path, FileMode.Open, FileAccess.Read))
                    {
                        //kt 클라우드에 데이터 업로드
                        //var fileUploadeda = await client.Upload(upload_path, fs, detail);

                        //KT Object Storage
                        kt_auth_req kt_data = new kt_auth_req();
                        kt_data.get_token();
                        //***********************************************************************
                        Stream writeStream = null;
                        FileStream readStream = null;

                        //최하위 경로만 존재해도 폴더는 생성이 된다.


                        System.Net.HttpWebRequest webRequest = System.Net.HttpWebRequest.Create(OS_endpoint + "_" + fileName) as System.Net.HttpWebRequest;
                        webRequest.Method = "PUT";
                        webRequest.ContentType = "image/jpeg";
                        webRequest.Headers.Add("X-Auth-Token", global_var.kt_token);
                        webRequest.UserAgent = "ITEM_FINDER/1.0";
                        webRequest.Timeout = System.Threading.Timeout.Infinite;
                        webRequest.KeepAlive = true;
                        webRequest.SendChunked = true;
                        writeStream = webRequest.GetRequestStream();
                        int count = 0;
                        byte[] buffer = new byte[1024];
                        while ((count = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            writeStream.Write(buffer, 0, count);
                        }

                        if (readStream != null)
                        {
                            try
                            {
                                readStream.Close(); readStream.Dispose();
                            }
                            catch
                            {
                            }
                        }
                        if (writeStream != null)
                        {
                            try
                            {
                                writeStream.Close();
                                writeStream.Dispose();
                            }
                            catch
                            {
                            }
                        }
                        if (webRequest != null)
                        {
                            try
                            {
                                System.Net.WebResponse responseKT = webRequest.GetResponse();
                                responseKT.Close();
                            }
                            catch
                            {
                            }
                        }
                    }


                    shopee_image s_image = new shopee_image { url = OS_endpoint + "_" + fileName };
                    lst_img.Add(s_image);
                }

                //상품 등록 시작
                int shopee_set_preorder_days = Convert.ToInt32(Ud_pre_order_value.Value);

                for (int account = 0; account < dg_site_id.Rows.Count; account++)
                {
                    if ((bool)dg_site_id.Rows[account].Cells["dg_site_id_chk"].Value)
                    {
                        string shopeeId = dg_site_id.Rows[account].Cells["dg_site_id_id"].Value.ToString();
                        long partner_id = Convert.ToInt64(dg_site_id.Rows[account].Cells["dg_site_id_partner_id"].Value.ToString());
                        long shop_id = Convert.ToInt64(dg_site_id.Rows[account].Cells["dg_site_id_shop_id"].Value.ToString());
                        string api_key = dg_site_id.Rows[account].Cells["dg_site_id_secret_key"].Value.ToString();

                        if (shopeeId == string.Empty)
                        {
                            return;
                        }

                        Dictionary<string, decimal> dicVariationPromotionPrice = new Dictionary<string, decimal>();

                        List<shopee_variations> lst_vari = new List<shopee_variations>();
                        if (DgVariation.Rows.Count > 0)
                        {
                            //Variation이 있는 경우
                            //진짜 2티어인지 검증한다. 모든 옵션에 콤마가 있으면 2티어임.
                            bool isReal2Tier = true;
                            bool isSamePrice = true;

                            for (int vari = 0; vari < DgVariation.Rows.Count; vari++)
                            {
                                if (DgVariation.Rows[vari].Cells["DgSrcVariation_shopee_id"].Value.ToString() == shopeeId)
                                {
                                    using (shopee_variations vars = new shopee_variations())
                                    {
                                        vars.name = DgVariation.Rows[vari].Cells["DgSrcVariation_Name"].Value.ToString();
                                        vars.stock = Convert.ToInt32(DgVariation.Rows[vari].Cells["DgSrcVariation_stock"].Value.ToString().Replace(",", ""));

                                        //현지 판가로 계산하여야 한다.
                                        //정수형일 경우 정수로 넣어주어야 한다.
                                        string variTemp = "";
                                        int iVariTemp = 0;

                                        //소수점이 0이면 없이 넘어온다.
                                        variTemp = string.Format("{0:0.##}", DgVariation.Rows[vari].Cells["DgSrcVariation_src_original_price"].Value.ToString().Replace(",", ""));
                                        bool is_int_vari = int.TryParse(variTemp, out iVariTemp);

                                        string variationSku = DgVariation.Rows[vari].Cells["DgSrcVariation_variation_sku"].Value.ToString();
                                        vars.variation_sku = variationSku;

                                        if (int.TryParse(variTemp, out iVariTemp))
                                        {
                                            vars.price = iVariTemp;
                                        }
                                        else
                                        {
                                            vars.price = Convert.ToDecimal(variTemp);
                                        }

                                        //옵션의 할인가 전달
                                        string variDiscountPrice = "";
                                        decimal iVariDiscount = 0;
                                        variDiscountPrice = string.Format("{0:0.##}", DgVariation.Rows[vari].Cells["DgSrcVariation_src_price"].Value.ToString().Replace(",", ""));
                                        iVariDiscount = Convert.ToDecimal(variDiscountPrice);

                                        try
                                        {
                                            if(!dicVariationPromotionPrice.ContainsKey(vars.variation_sku))
                                            {
                                                dicVariationPromotionPrice.Add(vars.variation_sku, Convert.ToDecimal(iVariDiscount));
                                            }
                                        }
                                        catch
                                        {

                                        }
                                        
                                        lst_vari.Add(vars);
                                    }
                                }
                            }
                        }

                        int logisticCode = 0;
                        if (dg_shopee_logistics.SelectedRows.Count > 0)
                        {
                            for (int lc = 0; lc < dg_shopee_logistics.Rows.Count; lc++)
                            {
                                if (dg_shopee_logistics.Rows[lc].Cells["dg_shopee_logistics_shopee_id"].Value.ToString() ==
                                    shopeeId)
                                {
                                    logisticCode = Convert.ToInt32(dg_shopee_logistics.Rows[lc].Cells["dg_shopee_logistics_logistics_id"].Value.ToString());
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Application.UseWaitCursor = false;
                            MessageBox.Show("등록 대상의 배송 코드가 선택되지 않았습니다.", "등록 배송 코드 선택", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            return;
                        }

                        string tarCategoryId = "";

                        for (int cat = 0; cat < dg_site_id_category.Rows.Count; cat++)
                        {
                            if (dg_site_id_category.Rows[cat].Cells["dg_site_id_category_id"].Value != null)
                            {
                                if (dg_site_id_category.Rows[cat].Cells["dg_site_id_category_id"].Value.ToString() ==     shopeeId)
                                {
                                    tarCategoryId = dg_site_id_category.Rows[cat].Cells["dg_site_id_category_set_category_id"].Value.ToString();
                                    break;
                                }
                            }
                        }


                        using (AppDbContext context = new AppDbContext())
                        {
                            decimal dRetailPrice = 0;
                            decimal calcPrice = 0;
                            string compair_price = "";
                            string qty = "";
                            string strSku = "";

                            decimal supplyPrice = 0;
                            decimal margin = 0;
                            int targetSellPriceKRW = 0;
                            decimal currencyRate = 0;
                            decimal pgFee = 0;
                            DateTime dtCurrencyDate = DateTime.Now;

                            for (int cp = 0; cp < DgPrice.Rows.Count; cp++)
                            {
                                if (DgPrice.Rows[cp].Cells["DgPrice_shopee_id"].Value.ToString() ==
                                    shopeeId)
                                {
                                    dRetailPrice = Convert.ToDecimal(DgPrice.Rows[cp].Cells["DgPrice_retail_price"].Value.ToString().Replace(",", ""));
                                    calcPrice = Convert.ToDecimal(DgPrice.Rows[cp].Cells["DgPrice_sell_price"].Value.ToString().Trim().Replace(",", ""));
                                    compair_price = DgPrice.Rows[cp].Cells["DgPrice_retail_price"].Value.ToString().Trim().Replace(",", "");
                                    qty = DgPrice.Rows[cp].Cells["DgPrice_qty"].Value.ToString().Trim().Replace(",", "");
                                    strSku = DgPrice.Rows[cp].Cells["DgPrice_parent_sku"].Value?.ToString().Trim() ?? "";


                                    
                                    if (!string.IsNullOrEmpty(DgPrice.Rows[cp].Cells["DgPrice_source_price"].Value.ToString().Trim()))
                                    {
                                        decimal.TryParse(
                                            DgPrice.Rows[cp].Cells["DgPrice_source_price"].Value.ToString().Trim().Replace(",", ""), out supplyPrice);
                                    }

                                    if (!string.IsNullOrEmpty(DgPrice.Rows[cp].Cells["DgPrice_margin"].Value.ToString().Trim()))
                                    {
                                        decimal.TryParse(
                                            DgPrice.Rows[cp].Cells["DgPrice_margin"].Value.ToString().Trim().Replace(",", ""), out margin);
                                    }

                                    if (!string.IsNullOrEmpty(DgPrice.Rows[cp].Cells["DgPrice_sell_price_krw"].Value.ToString().Trim()))
                                    {
                                        int.TryParse(
                                            DgPrice.Rows[cp].Cells["DgPrice_sell_price_krw"].Value.ToString().Trim().Replace(",", ""), out targetSellPriceKRW);
                                    }

                                    if (!string.IsNullOrEmpty(DgPrice.Rows[cp].Cells["DgPrice_pgfee"].Value.ToString().Trim()))
                                    {
                                        decimal.TryParse(
                                            DgPrice.Rows[cp].Cells["DgPrice_pgfee"].Value.ToString().Trim().Replace(",", ""), out pgFee);
                                    }

                                    //판매국가 코드
                                    string tarCountry = DgPrice.Rows[cp].Cells["DgPrice_tar_country"].Value.ToString();

                                    //환율은 DB에서 조회하여 가지고 온다.
                                    CurrencyRate objCurrencyRate = context.CurrencyRates.FirstOrDefault
                                    (x => x.cr_code.Contains(tarCountry) && x.UserId == global_var.userId);
                                    if (objCurrencyRate != null)
                                    {
                                        currencyRate = objCurrencyRate.cr_exchange;
                                        dtCurrencyDate = objCurrencyRate.cr_save_date;
                                    }


                                    break;
                                }
                            }


                            if (tarCategoryId != string.Empty && dRetailPrice != 0)
                            {
                                bool shopee_set_preorder = false;
                                string sell_title = "";
                                //순서가 섞여 있을 수 있으므로 동일 아이디로 찾아온다.
                                for (int title = 0; title < dg_productTitle.Rows.Count; title++)
                                {
                                    if (shopeeId == dg_productTitle.Rows[title].Cells["dg_productTitle_id"].Value.ToString())
                                    {
                                        sell_title = dg_productTitle.Rows[title].Cells["dg_productTitle_title"].Value.ToString();
                                        break;
                                    }
                                }



                                double strWeight = Convert.ToDouble(DgPrice.Rows[0].Cells["DgPrice_weight"].Value.ToString().Trim().Replace(",", ""));

                                double iWeight = strWeight;

                                if (iWeight < 0.1)
                                {
                                    iWeight = 0.1;
                                }


                                List<shopee_attribute> lst_attr = new List<shopee_attribute>();

                                if (DgSaveAttribute.Rows.Count > 0)
                                {
                                    for (int j = 0; j < DgSaveAttribute.Rows.Count; j++)
                                    {
                                        if (DgSaveAttribute.Rows[j].Cells["DgSaveAttribute_shopee_id"].Value.ToString() == shopeeId)
                                        {
                                            if (DgSaveAttribute.Rows[j].Cells["DgSaveAttribute_value"].Value != null &&
                                            DgSaveAttribute.Rows[j].Cells["DgSaveAttribute_value"].Value.ToString() != string.Empty)
                                            {
                                                shopee_attribute att = new shopee_attribute
                                                {
                                                    attributes_id = Convert.ToInt64(DgSaveAttribute.Rows[j].Cells["DgSaveAttribute_id"].Value.ToString()),
                                                    value = DgSaveAttribute.Rows[j].Cells["DgSaveAttribute_value"].Value.ToString()
                                                };

                                                lst_attr.Add(att);
                                            }
                                        }

                                    }
                                }

                                List<shopee_logi> lst_logi = new List<shopee_logi>();
                                shopee_logi sh = new shopee_logi { logistic_id = logisticCode, enabled = true, shipping_fee = 0 };
                                lst_logi.Add(sh);

                                var lst_img_buf = new List<shopee_image>();
                                // 이미지 URI 검증
                                var uploadImg = new ReqUploadImg
                                {
                                    Images = lst_img.Select(x => x.url).ToList(),
                                    PartnerId = partner_id,
                                    ShopId = shop_id,
                                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                                };
                                ResUploadImg uploadImgResult;
                                int uploadImgRetryCount = default;

                                do
                                {
                                    uploadImgResult = uploadImg.CallApi(api_key);
                                    uploadImgRetryCount++;
                                } while ((uploadImgResult is null || uploadImgResult.Images is null) && uploadImgRetryCount < 3);

                                if (uploadImgResult != null)
                                {
                                    var liUriTrans = new List<shopee_image>();

                                    foreach (ResUploadImgImages image in uploadImgResult.Images)
                                    {
                                        if (!string.IsNullOrEmpty(image.ShopeeImageUrl))
                                        {
                                            liUriTrans.Add(new shopee_image { url = image.ShopeeImageUrl });
                                        }
                                        else // download fail 로 오류가 발생한 경우는 ShopeeImageUrl이 비어있다.
                                        {
                                            liUriTrans.Add(new shopee_image { url = image.ImageUrl });
                                        }
                                    }

                                    lst_img_buf = liUriTrans; // DTO에 Image SetuploadImgResult.Images
                                }

                                object[] obj_image = lst_img_buf.ToArray();
                                object[] obj_attr = lst_attr.ToArray();
                                object[] obj_logi = lst_logi.ToArray();
                                object[] obj_vari = lst_vari.ToArray();

                                int dts = 2;
                                if (Rd_pre_order_yes.Checked)
                                {
                                    dts = Convert.ToInt32(Ud_pre_order_value.Value);
                                }

                                //상품 설명 관련
                                //헤더 푸터를 각 판매 채널에 따라 설정 할 수 있도록 되어 있음.
                                //헤더 세트 번호와 푸터 세트 번호를 가지고 온다.
                                int HeaderSetId = 0;
                                if (dg_site_id_desc.Rows[account].Cells["dg_site_id_desc_header_id"].Value != null)
                                {
                                    string strHeaderSetId = dg_site_id_desc.Rows[account].Cells["dg_site_id_desc_header_id"].Value.ToString();
                                    if (strHeaderSetId != string.Empty)
                                    {
                                        HeaderSetId = Convert.ToInt32(strHeaderSetId);
                                    }
                                }



                                int FooterSetId = 0;
                                if (dg_site_id_desc.Rows[account].Cells["dg_site_id_desc_footer_id"].Value != null)
                                {
                                    string strFooterSetId = dg_site_id_desc.Rows[account].Cells["dg_site_id_desc_footer_id"].Value.ToString();
                                    if (strFooterSetId != string.Empty)
                                    {
                                        FooterSetId = Convert.ToInt32(strFooterSetId);
                                    }
                                }

                                string strHeader = "";
                                string strFooter = "";

                                strHeader = FillHeaderSet(HeaderSetId);
                                strFooter = FillFooterSet(FooterSetId);
                                string strDesc = strHeader + TxtProductDesc.Text + strFooter;

                                var request = new RestRequest("", RestSharp.Method.POST);
                                request.Method = Method.POST;
                                request.AddHeader("Accept", "application/json");

                                DateTime saveDate = DateTime.Now;
                                long long_time_stamp = ToUnixTime(saveDate.AddHours(-9));
                                
                                double decimal_compair_price = 0;
                                string convert_price = "";

                                if (double.TryParse(compair_price, out decimal_compair_price))
                                {
                                    convert_price = string.Format("{0:0.##}", decimal_compair_price);
                                }

                                int int_val = 0;
                                int int_val_original = 0;

                                bool is_int = int.TryParse(convert_price, out int_val);

                                Dictionary<string, object> dic_json = new Dictionary<string, object>();
                                dic_json.Add("category_id", Convert.ToInt32(tarCategoryId));
                                dic_json.Add("name", sell_title);
                                dic_json.Add("description", strDesc);

                                if (is_int)
                                {
                                    dic_json.Add("price", int_val);
                                }
                                else
                                {
                                    dic_json.Add("price", Convert.ToDouble(compair_price));
                                }

                                dic_json.Add("stock", Convert.ToInt32(qty));
                                dic_json.Add("item_sku", strSku);
                                dic_json.Add("images", obj_image);
                                dic_json.Add("variations", obj_vari);
                                dic_json.Add("attributes", obj_attr);
                                dic_json.Add("logistics", obj_logi);

                                int int_weight = 0;
                                bool is_int_weight = int.TryParse(iWeight.ToString(), out int_weight);
                                if (is_int_weight)
                                {
                                    dic_json.Add("weight", int_weight);
                                }
                                else
                                {
                                    dic_json.Add("weight", iWeight);
                                }


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

                                    if (result.msg == "Add item success")
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
                                                UserId = global_var.userId

                                                //정의는 되어 있으나 필드가 안넘어옴
                                                //shipping_fee = ItemData.item.logistics[logi].shipping_fee,
                                                //size_id = ItemData.item.logistics[logi].size_id,
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

                                        //할인ID가 있는 경우에는 할인에 추가해 주어야 한다.
                                        long ldiscountId = 0;
                                        string strDiscountName = "";
                                        for (int i = 0; i < DgPrice.Rows.Count; i++)
                                        {
                                            if (DgPrice.Rows[i].Cells["DgPrice_shopee_id"].Value.ToString() == shopeeId)
                                            {
                                                var discountId = DgPrice.Rows[i].Cells["DgPrice_discount_id"].Value.ToString().Trim();
                                                if (discountId != string.Empty)
                                                {
                                                    ldiscountId = Convert.ToInt64(discountId);
                                                    strDiscountName = DgPrice.Rows[i].Cells["DgPrice_discount_name"].Value.ToString().Trim();
                                                    break;
                                                }
                                            }
                                        }

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
                                            stock = result.item.stock,
                                            create_time = UnixTimeStampToDateTime(Convert.ToInt64(result.item.create_time)),
                                            update_time = UnixTimeStampToDateTime(Convert.ToInt64(result.item.update_time)),
                                            weight = strWeight,
                                            category_id = result.item.category_id,
                                            original_price = dRetailPrice,
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
                                            discount_id = result.item.discount_id,
                                            is_2tier_item = result.item.is_2tier_item,
                                            condition = result.item.condition,
                                            images = strResultImage.ToString(),
                                            supply_price = supplyPrice,
                                            isChanged = false,
                                            margin = margin,
                                            targetSellPriceKRW = targetSellPriceKRW,
                                            currencyRate = currencyRate,
                                            currencyDate = dtCurrencyDate,
                                            pgFee = pgFee ,
                                            savedDate = saveDate,
                                            discount_name = strDiscountName,
                                            UserId = global_var.userId
                                        };

                                        context.ItemInfoes.Add(newItemInfo);
                                        context.SaveChanges();
                                        
                                        dg_site_id_reg.Rows[account].Cells["dg_site_id_reg_result"].Value = "성공";
                                        dg_site_id_reg.Rows[account].Cells["dg_site_id_reg_item_no"].Value = result.item_id;
                                        dg_site_id_reg.Rows[account].Cells["dg_site_id_reg_msg"].Value = "";
                                        dg_site_id_reg.Rows[account].Cells["dg_site_id_reg_json"].Value = content;
                                        Application.DoEvents();


                                        

                                        //string discountName = dgSrcItemList.Rows[i].Cells["dgItemList_discount_name"].Value.ToString().Trim();


                                        if (ldiscountId != 0)
                                        {
                                            dynamic variation = result.item.variations;
                                            long item_id = result.item_id;
                                            AddDiscountItems(ldiscountId, item_id, calcPrice, variation, dicVariationPromotionPrice,
                                                partner_id, shop_id, api_key);
                                        }
                                    }
                                    else
                                    {
                                        //실패상황
                                        dg_site_id_reg.Rows[account].Cells["dg_site_id_reg_result"].Value = "실패";
                                        dg_site_id_reg.Rows[account].Cells["dg_site_id_reg_item_no"].Value = "";
                                        dg_site_id_reg.Rows[account].Cells["dg_site_id_reg_msg"].Value = result.msg;
                                        dg_site_id_reg.Rows[account].Cells["dg_site_id_reg_json"].Value = result.msg;
                                        Application.DoEvents();
                                    }

                                }
                                catch (Exception ex)
                                {
                                    //실패상황
                                    dg_site_id_reg.Rows[account].Cells["dg_site_id_reg_result"].Value = "실패";
                                    dg_site_id_reg.Rows[account].Cells["dg_site_id_reg_msg"].Value = ex.Message;
                                    dg_site_id_reg.Rows[account].Cells["dg_site_id_reg_item_no"].Value = "";
                                    dg_site_id_reg.Rows[account].Cells["dg_site_id_reg_json"].Value = "";
                                    Application.DoEvents();
                                }
                            }
                        }
                    }
                }

                Application.UseWaitCursor = false;
                MessageBox.Show("상품을 등록 하였습니다. \r\n등록된 상품은 등록기에서 상품 관리로 이관 됩니다.", "등록완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }   
        }

        private void DisplayTreeView(JToken root, string rootName)
        {
            jsonExplorer.BeginUpdate();
            try
            {
                jsonExplorer.Nodes.Clear();
                var tNode = jsonExplorer.Nodes[jsonExplorer.Nodes.Add(new TreeNode(rootName))];
                tNode.Tag = root;

                AddNode(root, tNode);

                jsonExplorer.ExpandAll();
            }
            finally
            {
                jsonExplorer.EndUpdate();
            }
        }

        private void AddNode(JToken token, TreeNode inTreeNode)
        {
            if (token == null)
                return;
            if (token is JValue)
            {
                var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(token.ToString()))];
                childNode.Tag = token;
            }
            else if (token is JObject)
            {
                var obj = (JObject)token;
                foreach (var property in obj.Properties())
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name))];
                    childNode.Tag = property;
                    AddNode(property.Value, childNode);
                }
            }
            else if (token is JArray)
            {
                var array = (JArray)token;
                for (int i = 0; i < array.Count; i++)
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(i.ToString()))];
                    childNode.Tag = array[i];
                    AddNode(array[i], childNode);
                }
            }
            else
            {
                
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

                            decimal tempPromoPrice = dicVariationPromotionPrice[lstVariation[i].variation_sku.ToString()];
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

        private void DgPrice_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {   
            if (DgPrice.Rows.Count > 0)
            {
                if(DgPrice.Rows[e.RowIndex].Cells["DgPrice_tar_country"].Value == null)
                {
                    return;
                }

                if ((e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5) && e.RowIndex > -1)
                {
                    string tarCountry = DgPrice.Rows[e.RowIndex].Cells["DgPrice_tar_country"].Value.ToString();
                    Dictionary<string, decimal> dicPrice = new Dictionary<string, decimal>();

                    decimal sourcePrice = 0;
                    if (DgPrice.Rows[e.RowIndex].Cells["DgPrice_source_price"].Value != null &&
                        DgPrice.Rows[e.RowIndex].Cells["DgPrice_source_price"].Value.ToString().Trim() != string.Empty)
                    {
                        sourcePrice = Convert.ToDecimal(DgPrice.Rows[e.RowIndex].Cells["DgPrice_source_price"].Value?.ToString().Replace(",", ""));

                        //동일하게 복사해 준다.
                        for (int i = 0; i < DgPrice.Rows.Count; i++)
                        {
                            DgPrice.Rows[i].Cells["DgPrice_source_price"].Value = sourcePrice;
                        }
                    }

                    decimal margin = 0;
                    if (DgPrice.Rows[e.RowIndex].Cells["DgPrice_margin"].Value != null &&
                        DgPrice.Rows[e.RowIndex].Cells["DgPrice_margin"].Value.ToString() != string.Empty)
                    {
                        margin = Convert.ToDecimal(DgPrice.Rows[e.RowIndex].Cells["DgPrice_margin"].Value?.ToString().Replace(",", ""));
                    }

                    int weight = 0;
                    if (DgPrice.Rows[e.RowIndex].Cells["DgPrice_weight"].Value != null &&
                        DgPrice.Rows[e.RowIndex].Cells["DgPrice_weight"].Value.ToString() != string.Empty)
                    {
                        double dWeight = Convert.ToDouble(DgPrice.Rows[e.RowIndex].Cells["DgPrice_weight"].Value?.ToString().Replace(",", ""));
                        weight = Convert.ToInt32(dWeight * 1000);
                    }


                    if (sourcePrice > 0 && margin >= 0 && weight > 0)
                    {
                        dicPrice = calcRowPrice(sourcePrice, margin, weight, tarCountry);

                        DgPrice.Rows[e.RowIndex].Cells["DgPrice_pgfee"].Value = string.Format("{0:n2}", dicPrice["pgFee"]);
                        DgPrice.Rows[e.RowIndex].Cells["DgPrice_sell_price_krw"].Value = string.Format("{0:n0}", dicPrice["targetSellPriceKRW"]);
                        DgPrice.Rows[e.RowIndex].Cells["DgPrice_sell_price"].Value = string.Format("{0:n2}", dicPrice["targetSellPrice"]);
                        DgPrice.Rows[e.RowIndex].Cells["DgPrice_retail_price"].Value = string.Format("{0:n2}", dicPrice["targetRetailPrice"]);
                    }
                }
                else if(e.ColumnIndex == 2)
                {
                    string str_sku = DgPrice.Rows[e.RowIndex].Cells["DgPrice_parent_sku"].Value.ToString();
                    for (int i = 0; i < DgPrice.Rows.Count; i++)
                    {
                        DgPrice.Rows[i].Cells["DgPrice_parent_sku"].Value = str_sku;
                    }
                }
            }
        }

        private void BtnSaveProduct_Click(object sender, EventArgs e)
        {

        }

        private void BtnViewProduct_Click(object sender, EventArgs e)
        {
            if(dg_site_id_reg.Rows.Count > 0 &&
                dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_item_no"].Value != null &&
                dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_country"].Value != null)
            {
                string siteUrl = "";
                string goods_no = dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_item_no"].Value.ToString();
                string tarCountry = dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_country"].Value.ToString();
                long partner_id = Convert.ToInt64(dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_partner_id"].Value.ToString());
                long shop_id = Convert.ToInt64(dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_shop_id"].Value.ToString());
                string api_key = dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_secret_key"].Value.ToString();

                if (tarCountry == "SG")
                {
                    siteUrl = "https://shopee.sg/product/" + shop_id + "/" + goods_no;
                }
                else if (tarCountry == "MY")
                {
                    siteUrl = "https://shopee.com.my/product/" + shop_id + "/" + goods_no;
                }
                else if (tarCountry == "ID")
                {
                    siteUrl = "https://shopee.co.id/product/" + shop_id + "/" + goods_no;
                }
                else if (tarCountry == "TH")
                {
                    siteUrl = "https://shopee.co.th/product/" + shop_id + "/" + goods_no;
                }
                else if (tarCountry == "TW")
                {
                    siteUrl = "https://shopee.tw/product/" + shop_id + "/" + goods_no;
                }
                else if (tarCountry == "PH")
                {
                    siteUrl = "https://shopee.ph/product/" + shop_id + "/" + goods_no;
                }
                else if (tarCountry == "VN")
                {
                    siteUrl = "https://shopee.vn/product/" + shop_id + "/" + goods_no;
                }
                System.Diagnostics.Process.Start("chrome.exe", siteUrl);
            }
        }

        private void FromAddProduct_Activated(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
        }

        private void DgVariation_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < DgVariation.Rows.Count; i++)
            {
                if (DgVariation.Rows[i].Cells["DgSrcVariation_Name"].Value != null)
                {
                    if(DgVariation.Rows[i].Cells["DgSrcVariation_Name"].Value.ToString().Length == 0)
                    {
                        DgVariation.Rows[i].Cells["DgSrcVariation_Name"].Style.BackColor = Color.White;
                    }
                    else if (DgVariation.Rows[i].Cells["DgSrcVariation_Name"].Value.ToString().Length > 20)
                    {
                        DgVariation.Rows[i].Cells["DgSrcVariation_Name"].Style.BackColor = Color.Orange;
                    }
                    else
                    {
                        DgVariation.Rows[i].Cells["DgSrcVariation_Name"].Style.BackColor = Color.GreenYellow;
                    }
                }
            }

            if (DgPrice.Rows.Count > 0)
            {
                if (DgPrice.SelectedRows[0].Cells["DgPrice_shopee_id"].Value != null)
                {
                    if (DgVariation.Rows[e.RowIndex].Cells["DgSrcVariation_shopee_id"].Value == null ||
                        DgVariation.Rows[e.RowIndex].Cells["DgSrcVariation_shopee_id"].Value.ToString() == string.Empty)
                    {
                        DgVariation.Rows[e.RowIndex].Cells["DgSrcVariation_shopee_id"].Value = DgPrice.SelectedRows[0].Cells["DgPrice_shopee_id"].Value.ToString();
                        DgVariation.Rows[e.RowIndex].Cells["DgSrcVariation_tar_country"].Value = DgPrice.SelectedRows[0].Cells["DgPrice_tar_country"].Value.ToString();
                    }
                }
            }
        }

        private void DgVariation_SelectionChanged(object sender, EventArgs e)
        {
            if(DgVariation.Rows.Count > 0)
            {
                DgVariation.CurrentRow.Cells["DgSrcVariation_Name"].Style.SelectionBackColor = DgVariation.CurrentRow.Cells["DgSrcVariation_Name"].Style.BackColor;
                DgVariation.CurrentRow.Cells["DgSrcVariation_Name"].Style.SelectionForeColor = Color.Blue;
            }
        }

        private void BtnTitleTypeExcel_Click(object sender, EventArgs e)
        {

        }

        private void BtnBrandExcel_Click(object sender, EventArgs e)
        {
            if (dgTitle_type.Rows.Count == 0 || dgTitle_type.SelectedRows.Count == 0)
            {
                MessageBox.Show("선택한 상품명 템플릿 종류가 없습니다.","상품명 템플릿 종류 없음",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }


            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = "xlsx";
            openFileDlg.Filter = "엑셀 파일 (*.xlsx)|*.xlsx";
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                string file_name = openFileDlg.FileName;
                string ext = Path.GetExtension(file_name);

                if (ext.Contains("xlsx"))
                {
                    var package = new ExcelPackage(new FileInfo(file_name));

                    OfficeOpenXml.ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

                    DataTable dTable = new DataTable("excel_data");

                    DataColumn dc_brand_name = new DataColumn("brand_name", typeof(string));

                    dTable.Columns.Add(dc_brand_name);

                    DataRow dr;
                    for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                    {
                        dr = dTable.NewRow();
                        dr["brand_name"] = workSheet.Cells[i, 1].Value;
                        dTable.Rows.Add(dr);
                    }

                    int TitleTypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    using (AppDbContext context = new AppDbContext())
                    {
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            string titleName = dTable.Rows[i][0].ToString();
                            TitleBrand brand = context.TitleBrands
                                .FirstOrDefault(x => x.TitleBrandName == titleName && x.TitleTypeId == TitleTypeId && x.UserId == global_var.userId);

                            if (brand == null)
                            {
                                TitleBrand newData = new TitleBrand()
                                {
                                    TitleBrandName = dTable.Rows[i][0].ToString(),
                                    TitleTypeId = TitleTypeId,
                                    UserId = global_var.userId
                                };

                                context.TitleBrands.Add(newData);
                                context.SaveChanges();
                            }
                        }
                    }

                    getListBrand(TitleTypeId);
                }
                MessageBox.Show("데이터를 모두 입력하였습니다.", "데이터 입력 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnModelExcel_Click(object sender, EventArgs e)
        {
            if (dgTitle_type.Rows.Count == 0 || dgTitle_type.SelectedRows.Count == 0)
            {
                MessageBox.Show("선택한 상품명 템플릿 종류가 없습니다.", "상품명 템플릿 종류 없음", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgBrand.Rows.Count == 0 || dgBrand.SelectedRows.Count == 0)
            {
                MessageBox.Show("선택한 브랜드가 없습니다.", "브랜드 선택 없음", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = "xlsx";
            openFileDlg.Filter = "엑셀 파일 (*.xlsx)|*.xlsx";
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                string file_name = openFileDlg.FileName;
                string ext = Path.GetExtension(file_name);

                if (ext.Contains("xlsx"))
                {
                    var package = new ExcelPackage(new FileInfo(file_name));

                    OfficeOpenXml.ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

                    DataTable dTable = new DataTable("excel_data");

                    DataColumn dc_model_name = new DataColumn("model_name", typeof(string));

                    dTable.Columns.Add(dc_model_name);

                    DataRow dr;
                    for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                    {
                        dr = dTable.NewRow();
                        dr["model_name"] = workSheet.Cells[i, 1].Value;
                        dTable.Rows.Add(dr);
                    }

                    int TitleTypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    int BrandId = Convert.ToInt32(dgBrand.SelectedRows[0].Cells["dgBrand_id"].Value.ToString());
                    using (AppDbContext context = new AppDbContext())
                    {
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            string modelName = dTable.Rows[i][0].ToString();
                            TitleModel model = context.TitleModels
                                .FirstOrDefault(x => x.TitleModelName == modelName && x.UserId == global_var.userId);

                            if (model == null)
                            {
                                TitleModel newData = new TitleModel()
                                {
                                    TitleModelName = dTable.Rows[i][0].ToString(),
                                    TitleBrandId = BrandId,
                                    UserId = global_var.userId
                                };

                                context.TitleModels.Add(newData);
                                context.SaveChanges();
                            }
                        }
                    }

                    getListModel(BrandId);
                }
                MessageBox.Show("데이터를 모두 입력하였습니다.", "데이터 입력 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TabMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            TabPage tp = TabMain.TabPages[e.Index];

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;  //optional

            // This is the rectangle to draw "over" the tabpage title
            RectangleF headerRect = new RectangleF(e.Bounds.X, e.Bounds.Y + 6, e.Bounds.Width, e.Bounds.Height - 2);

            // This is the default colour to use for the non-selected tabs
            SolidBrush sb = new SolidBrush(Color.Transparent);

            // This changes the colour if we're trying to draw the selected tabpage
            if (TabMain.SelectedIndex == e.Index)
                sb.Color = Color.FromArgb(204, 244, 255);

            // Colour the header of the current tabpage based on what we did above
            g.FillRectangle(sb, e.Bounds);

            //Remember to redraw the text - I'm always using black for title text
            g.DrawString(tp.Text, TabMain.Font, new SolidBrush(Color.Black), headerRect, sf);
        }

        private void TabMain_Image_Click(object sender, EventArgs e)
        {

        }

        private void BtnAddWaterMark_Click(object sender, EventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;
            //Bitmap bmWater = (Bitmap)DGImageTempletelList.SelectedRows[0].Cells["DGImageTempletelList_image"].Value;
            //for (int i = 0; i < DGSelectedList.Rows.Count; i++)
            //{
            //    if ((bool)DGSelectedList.Rows[i].Cells["DGSelectedList_Chk"].Value)
            //    {
            //        Bitmap bmSrc = (Bitmap)DGSelectedList.Rows[i].Cells["DGSelectedList_Image"].Value;

            //        Bitmap bm = DrawWatermark(bmWater, bmSrc, 0, 0);
            //        DGSelectedList.Rows[i].Cells["DGSelectedList_Image"].Value = bm;
            //    }
            //}
            //Cursor.Current = Cursors.Default;
            //return;

            if (DGSelectedList.Rows.Count == 0)
            {
                MessageBox.Show("워터마크 작업할 파일을 체크하여 주세요.", "워터마크 파일 체크", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //포토웍스가 있는지 확인
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software\\ando\\photoWORKS\\1.0");
            if(regKey == null)
            {
                MessageBox.Show("포토웍스가 설치되어 있지 않습니다.", "포토웍스", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                string startPath = (string)regKey.GetValue("Install_Dir", "");
                if (File.Exists(startPath + @"\photoWORKS.exe"))
                {
                    StringBuilder sb_fileName = new StringBuilder();
                    for (int i = 0; i < DGSelectedList.Rows.Count; i++)
                    {
                        if ((bool)DGSelectedList.Rows[i].Cells["DGSelectedList_Chk"].Value)
                        {
                            sb_fileName.Append(DGSelectedList.Rows[i].Cells["DGSelectedList_path"].Value.ToString() + " ");
                        }
                    }

                    if (sb_fileName.Length > 0)
                    {
                        sb_fileName.Remove(sb_fileName.Length - 1, 1);
                    }

                    System.Diagnostics.Process.Start(startPath + @"\photoWORKS.exe",
                        sb_fileName.ToString());
                }
                else
                {
                    MessageBox.Show("포토웍스 프로그램이 설치되어 있지 않습니다", "포토웍스 미설치", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void reFreshThumb()
        {
            DGSelectedList.Rows.Clear();
            if (Directory.Exists(ImagePath))
            {
                int i = 1;
                WebClient wc = new WebClient();

                DirectoryInfo dir = new DirectoryInfo(ImagePathThumb);
                bool isExistOutput = false;
                foreach (var item in dir.GetDirectories())
                {
                    if (item.FullName.Contains("output"))
                    {
                        isExistOutput = true;
                        break;
                    }
                }

                Dictionary<string, string> dicWFile = new Dictionary<string, string>();

                if (isExistOutput)
                {
                    DirectoryInfo di = new DirectoryInfo(ImagePathThumb + @"\output");
                    foreach (var item in di.GetFiles())
                    {
                        var content = wc.DownloadData(item.FullName);
                        using (var stream = new MemoryStream(content))
                        {
                            Image im = Image.FromStream(stream);
                            DGSelectedList.Rows.Add(i++, false, im, item.Name, im.Width, im.Height, item.FullName);
                            dicWFile.Add(item.Name, item.Name);
                        }
                    }

                    DirectoryInfo di2 = new DirectoryInfo(ImagePathThumb);
                    foreach (var item in di2.GetFiles())
                    {
                        if (!dicWFile.Keys.Contains(item.Name))
                        {
                            var content = wc.DownloadData(item.FullName);
                            using (var stream = new MemoryStream(content))
                            {
                                Image im = Image.FromStream(stream);
                                DGSelectedList.Rows.Add(i++, false, im, item.Name, im.Width, im.Height, item.FullName);
                                dicWFile.Add(item.Name, item.Name);


                            }
                        }
                    }
                }
                else
                {
                    DirectoryInfo di = new DirectoryInfo(ImagePathThumb);
                    foreach (var item in di.GetFiles())
                    {
                        var content = wc.DownloadData(item.FullName);
                        using (var stream = new MemoryStream(content))
                        {
                            Image im = Image.FromStream(stream);
                            DGSelectedList.Rows.Add(i++, false, im, item.Name, im.Width, im.Height, item.FullName);
                        }
                    }
                }
            }

            lblThumb.Text = "기본 썸네일 [ " + DGSelectedList.Rows.Count + " ]";
        }
        private Bitmap DrawWatermark(Bitmap watermark_bm,
    Bitmap result_bm, int x, int y)
        {
            const byte ALPHA = 128;
            // Set the watermark's pixels' Alpha components.
            Color clr;
            for (int py = 0; py < watermark_bm.Height; py++)
            {
                for (int px = 0; px < watermark_bm.Width; px++)
                {
                    clr = watermark_bm.GetPixel(px, py);
                    watermark_bm.SetPixel(px, py,
                        Color.FromArgb(ALPHA, clr.R, clr.G, clr.B));
                }
            }

            // Set the watermark's transparent color.
            watermark_bm.MakeTransparent(watermark_bm.GetPixel(0, 0));

            // Copy onto the result image.
            using (Graphics gr = Graphics.FromImage(result_bm))
            {
                gr.DrawImage(watermark_bm, x, y);
            }

            return result_bm;
        }

        private void BtnRefreshThumb_Click(object sender, EventArgs e)
        {
            reFreshThumb();
        }

        private void UdMargin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnSaveMargin_Click(null, null);
            }
        }

        private void UdQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnApplyQty_Click(null, null);
            }
        }

        private void UdWeight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnApplyWeight_Click(null, null);
            }
        }

        private void BtnApplySrcPrice_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 0; i < DgPrice.Rows.Count; i++)
            {
                DgPrice.Rows[i].Cells["DgPrice_source_price"].Value = string.Format("{0:n0}", UdSourcePrice.Value);
            }
            

            for (int i = 0; i < DgVariation.Rows.Count; i++)
            {
                DgVariation.Rows[i].Cells["DgSrcVariation_supply_price"].Value = string.Format("{0:n0}", UdSourcePrice.Value);
            }
            //BtnCalcSellPrice_Click(null, null);
            Cursor.Current = Cursors.Default;
            //MessageBox.Show("마진을 모두 적용하였습니다.", "마진적용", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UdSourcePrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnApplySrcPrice_Click(null, null);
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

        private void UdSourcePrice_ValueChanged(object sender, EventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                ConfigVar result = context.ConfigVars.SingleOrDefault(b => b.UserId == global_var.userId);
                if (result == null)
                {
                    ConfigVar newVar = new ConfigVar
                    {
                        source_price = UdSourcePrice.Value,
                        UserId = global_var.userId
                    };
                    context.ConfigVars.Add(newVar);
                    context.SaveChanges();
                }
                else
                {
                    result.source_price = UdSourcePrice.Value;
                    context.SaveChanges();
                }
            }

            BtnApplySrcPrice_Click(null, null);
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
            btnSaveMargin_Click(null, null);
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
                        weight = Convert.ToDouble(UdWeight.Value),
                        UserId = global_var.userId
                    };
                    context.ConfigVars.Add(newVar);
                    context.SaveChanges();
                }
                else
                {
                    result.weight = Convert.ToDouble(UdWeight.Value);
                    context.SaveChanges();
                }
            }
            BtnApplyWeight_Click(null, null);
        }

        private void Menu_Logistic_Sync_Click(object sender, EventArgs e)
        {
            DialogResult dlg_Result = MessageBox.Show("쇼피 배송코드 목록을 동기화 하시겠습니까?", "배송코드 동기화", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg_Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                dg_shopee_logistics.Rows.Clear();
                using (AppDbContext context = new AppDbContext())
                {
                    List<Logistic> logisticList = context.Logistics.Where(x => x.UserId == global_var.userId)
                    .OrderBy(x => x.logistic_id).ToList();

                    if (logisticList.Count > 0)
                    {
                        context.Logistics.RemoveRange(logisticList);
                        context.SaveChanges();
                    }

                }
                for (int acc = 0; acc < dg_site_id.Rows.Count; acc++)
                {
                    string country = dg_site_id.Rows[acc].Cells["dg_site_id_country"].Value.ToString();
                    string shopeeId = dg_site_id.Rows[acc].Cells["dg_site_id_id"].Value.ToString();
                    long long_time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));
                    string end_point = "https://partner.shopeemobile.com/api/v1/logistics/channel/get";
                    var client = new RestSharp.RestClient(end_point);
                    long partner_id = Convert.ToInt64(dg_site_id.Rows[acc].Cells["dg_site_id_partner_id"].Value.ToString());
                    long shop_id = Convert.ToInt64(dg_site_id.Rows[acc].Cells["dg_site_id_shop_id"].Value.ToString());
                    string api_key = dg_site_id.Rows[acc].Cells["dg_site_id_secret_key"].Value.ToString();

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
                                    for (int i = 0; i < result.logistics.Count; i++)
                                    {
                                        if (country.Contains("SG"))
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
                                            }
                                        }
                                        else if (country.Contains("ID"))
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
                                            }   
                                        }
                                        else if (country.Contains("MY"))
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
                                            }
                                            else
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
                                            }
                                        }
                                        else if (country.Contains("PH"))
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
                                            }   
                                        }
                                        else if (country.Contains("VN"))
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
                getLogisticList();
            }
        }

        private void BtnGroupExcel_Click(object sender, EventArgs e)
        {
            if (dgTitle_type.Rows.Count == 0 || dgTitle_type.SelectedRows.Count == 0)
            {
                MessageBox.Show("선택한 상품명 템플릿 종류가 없습니다.", "상품명 템플릿 종류 없음", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = "xlsx";
            openFileDlg.Filter = "엑셀 파일 (*.xlsx)|*.xlsx";
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                string file_name = openFileDlg.FileName;
                string ext = Path.GetExtension(file_name);

                if (ext.Contains("xlsx"))
                {
                    var package = new ExcelPackage(new FileInfo(file_name));

                    OfficeOpenXml.ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

                    DataTable dTable = new DataTable("excel_data");

                    DataColumn dc_group_name = new DataColumn("group_name", typeof(string));

                    dTable.Columns.Add(dc_group_name);

                    DataRow dr;
                    for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                    {
                        dr = dTable.NewRow();
                        dr["group_name"] = workSheet.Cells[i, 1].Value;
                        dTable.Rows.Add(dr);
                    }

                    int TitleTypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    using (AppDbContext context = new AppDbContext())
                    {
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            string titleName = dTable.Rows[i][0].ToString();
                            TitleGroup group = context.TitleGroups
                                .FirstOrDefault(x => x.TitleGroupName == titleName && x.TitleTypeId == TitleTypeId && x.UserId == global_var.userId);

                            if (group == null)
                            {
                                TitleGroup newData = new TitleGroup()
                                {
                                    TitleGroupName = dTable.Rows[i][0].ToString(),
                                    TitleTypeId = TitleTypeId,
                                    UserId = global_var.userId
                                };

                                context.TitleGroups.Add(newData);
                                context.SaveChanges();
                            }
                        }
                    }

                    getListGroup(TitleTypeId);
                }
                MessageBox.Show("데이터를 모두 입력하였습니다.", "데이터 입력 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnFeatureExcel_Click(object sender, EventArgs e)
        {
            if (dgTitle_type.Rows.Count == 0 || dgTitle_type.SelectedRows.Count == 0)
            {
                MessageBox.Show("선택한 상품명 템플릿 종류가 없습니다.", "상품명 템플릿 종류 없음", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = "xlsx";
            openFileDlg.Filter = "엑셀 파일 (*.xlsx)|*.xlsx";
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                string file_name = openFileDlg.FileName;
                string ext = Path.GetExtension(file_name);

                if (ext.Contains("xlsx"))
                {
                    var package = new ExcelPackage(new FileInfo(file_name));

                    OfficeOpenXml.ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

                    DataTable dTable = new DataTable("excel_data");

                    DataColumn dc_feature_name = new DataColumn("feature_name", typeof(string));

                    dTable.Columns.Add(dc_feature_name);

                    DataRow dr;
                    for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                    {
                        dr = dTable.NewRow();
                        dr["feature_name"] = workSheet.Cells[i, 1].Value;
                        dTable.Rows.Add(dr);
                    }

                    int TitleTypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    using (AppDbContext context = new AppDbContext())
                    {
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            string titleName = dTable.Rows[i][0].ToString();
                            TitleFeature feature = context.TitleFeatures
                                .FirstOrDefault(x => x.TitleFeatureName == titleName && x.TitleTypeId == TitleTypeId && x.UserId == global_var.userId);

                            if (feature == null)
                            {
                                TitleFeature newData = new TitleFeature()
                                {
                                    TitleFeatureName = dTable.Rows[i][0].ToString(),
                                    TitleTypeId = TitleTypeId,
                                    UserId = global_var.userId
                                };

                                context.TitleFeatures.Add(newData);
                                context.SaveChanges();
                            }
                        }
                    }

                    getListFeature(TitleTypeId);
                }
                MessageBox.Show("데이터를 모두 입력하였습니다.", "데이터 입력 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnOptionExcel_Click(object sender, EventArgs e)
        {
            if (dgTitle_type.Rows.Count == 0 || dgTitle_type.SelectedRows.Count == 0)
            {
                MessageBox.Show("선택한 상품명 템플릿 종류가 없습니다.", "상품명 템플릿 종류 없음", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = "xlsx";
            openFileDlg.Filter = "엑셀 파일 (*.xlsx)|*.xlsx";
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                string file_name = openFileDlg.FileName;
                string ext = Path.GetExtension(file_name);

                if (ext.Contains("xlsx"))
                {
                    var package = new ExcelPackage(new FileInfo(file_name));

                    OfficeOpenXml.ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

                    DataTable dTable = new DataTable("excel_data");

                    DataColumn dc_option_name = new DataColumn("option_name", typeof(string));

                    dTable.Columns.Add(dc_option_name);

                    DataRow dr;
                    for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                    {
                        dr = dTable.NewRow();
                        dr["option_name"] = workSheet.Cells[i, 1].Value;
                        dTable.Rows.Add(dr);
                    }

                    int TitleTypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    using (AppDbContext context = new AppDbContext())
                    {
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            string titleName = dTable.Rows[i][0].ToString();
                            TitleOption option = context.TitleOptions
                                .FirstOrDefault(x => x.TitleOptionName == titleName && x.TitleTypeId == TitleTypeId && x.UserId == global_var.userId);

                            if (option == null)
                            {
                                TitleOption newData = new TitleOption()
                                {
                                    TitleOptionName = dTable.Rows[i][0].ToString(),
                                    TitleTypeId = TitleTypeId,
                                    UserId = global_var.userId
                                };

                                context.TitleOptions.Add(newData);
                                context.SaveChanges();
                            }
                        }
                    }

                    getListOption(TitleTypeId);
                }
                MessageBox.Show("데이터를 모두 입력하였습니다.", "데이터 입력 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnRelatExcel_Click(object sender, EventArgs e)
        {
            if (dgTitle_type.Rows.Count == 0 || dgTitle_type.SelectedRows.Count == 0)
            {
                MessageBox.Show("선택한 상품명 템플릿 종류가 없습니다.", "상품명 템플릿 종류 없음", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = "xlsx";
            openFileDlg.Filter = "엑셀 파일 (*.xlsx)|*.xlsx";
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                string file_name = openFileDlg.FileName;
                string ext = Path.GetExtension(file_name);

                if (ext.Contains("xlsx"))
                {
                    var package = new ExcelPackage(new FileInfo(file_name));

                    OfficeOpenXml.ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

                    DataTable dTable = new DataTable("excel_data");

                    DataColumn dc_relat_name = new DataColumn("relat_name", typeof(string));

                    dTable.Columns.Add(dc_relat_name);

                    DataRow dr;
                    for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                    {
                        dr = dTable.NewRow();
                        dr["relat_name"] = workSheet.Cells[i, 1].Value;
                        dTable.Rows.Add(dr);
                    }

                    int TitleTypeId = Convert.ToInt32(dgTitle_type.SelectedRows[0].Cells["dgTitle_type_id"].Value.ToString());
                    using (AppDbContext context = new AppDbContext())
                    {
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            string titleName = dTable.Rows[i][0].ToString();
                            TitleRelat relat = context.TitleRelats
                                .FirstOrDefault(x => x.TitleRelatName == titleName && x.TitleTypeId == TitleTypeId && x.UserId == global_var.userId);

                            if (relat == null)
                            {
                                TitleRelat newData = new TitleRelat()
                                {
                                    TitleRelatName = dTable.Rows[i][0].ToString(),
                                    TitleTypeId = TitleTypeId,
                                    UserId = global_var.userId
                                };

                                context.TitleRelats.Add(newData);
                                context.SaveChanges();
                            }
                        }
                    }

                    getListRelat(TitleTypeId);
                }
                MessageBox.Show("데이터를 모두 입력하였습니다.", "데이터 입력 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSearchGoogle_Click(object sender, EventArgs e)
        {
            string title = "";
            if (RdTitleEng.Checked)
            {
                if(dg_productTitle.Rows.Count > 0)
                {
                    title = dg_productTitle.Rows[0].Cells["dg_productTitle_title"].Value.ToString();
                    if(title == string.Empty)
                    {
                        BtnTranslate_Click(null, null);
                        title = dg_productTitle.Rows[0].Cells["dg_productTitle_title"].Value.ToString();
                        //MessageBox.Show("영문 상품명이 없습니다.","영문 상품명 없음",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                title = TxtProductNameKor.Text.Trim();
            }
            

            System.Diagnostics.Process.Start("https://www.google.com/search?q=" + HttpUtility.UrlEncode(title));
        }

        private void MenuModel_SearchCoupang_Click(object sender, EventArgs e)
        {
            string brand = dgBrand.SelectedRows[0].Cells["dgBrand_Name"].Value.ToString();
            string title = dgModel.SelectedRows[0].Cells["dgModel_name"].Value.ToString();
            System.Diagnostics.Process.Start("https://www.coupang.com/np/search?component=&q=" + HttpUtility.UrlEncode(brand + " " + title));
        }

        private void MenuModel_SearchGoogle_Click(object sender, EventArgs e)
        {
            string brand = dgBrand.SelectedRows[0].Cells["dgBrand_Name"].Value.ToString();
            string title = dgModel.SelectedRows[0].Cells["dgModel_name"].Value.ToString();
            System.Diagnostics.Process.Start("https://www.google.com/search?q=" + HttpUtility.UrlEncode(brand + " " + title));
        }

        private void MenuModel_SearchAmazon_Click(object sender, EventArgs e)
        {
            string brand = dgBrand.SelectedRows[0].Cells["dgBrand_Name"].Value.ToString();
            string title = dgModel.SelectedRows[0].Cells["dgModel_name"].Value.ToString();
            System.Diagnostics.Process.Start("https://www.amazon.com/s?k=" + HttpUtility.UrlEncode(brand + " " + title));
            
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

            //전체 데이터를 업데이트 한다.
            for (int i = 0; i < DgPrice.Rows.Count; i++)
            {
                DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(3, i);
                DgPrice_CellValueChanged(null, et);
            }

            for (int i = 0; i < DgVariation.Rows.Count; i++)
            {
                DataGridViewCellEventArgs et2 = new DataGridViewCellEventArgs(9, i);
                DgVariation_CellValueChanged(null, et2);
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

            //전체 데이터를 업데이트 한다.
            for (int i = 0; i < DgPrice.Rows.Count; i++)
            {
                DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(3, i);
                DgPrice_CellValueChanged(null, et);
            }

            for (int i = 0; i < DgVariation.Rows.Count; i++)
            {
                DataGridViewCellEventArgs et2 = new DataGridViewCellEventArgs(9, i);
                DgVariation_CellValueChanged(null, et2);
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


            //전체 데이터를 업데이트 한다.
            for (int i = 0; i < DgPrice.Rows.Count; i++)
            {
                DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(3, i);
                DgPrice_CellValueChanged(null, et);
            }

            for (int i = 0; i < DgVariation.Rows.Count; i++)
            {
                DataGridViewCellEventArgs et2 = new DataGridViewCellEventArgs(9, i);
                DgVariation_CellValueChanged(null, et2);
            }
        }

        private void dg_site_id_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 1 && e.RowIndex == -1)
            {
                bool val = (bool)dg_site_id.Rows[0].Cells["dg_site_id_chk"].Value;
                for (int i = 0; i < dg_site_id.Rows.Count; i++)
                {
                    dg_site_id.Rows[i].Cells["dg_site_id_chk"].Value = !val;
                    
                    //동일하게 복사하기
                    dg_site_id_reg.Rows[i].Cells["dg_site_id_reg_chk"].Value = !val;
                    dg_site_id_category.Rows[i].Cells["dg_site_id_category_chk"].Value = !val;
                    dg_site_id_desc.Rows[i].Cells["dg_site_id_desc_chk"].Value = !val;


                    if ((bool)dg_site_id.Rows[i].Cells["dg_site_id_chk"].Value)
                    {
                        dg_productTitle.Rows.Add("", dg_productTitle.Rows.Count + 1, false,
                            dg_site_id.Rows[i].Cells["dg_site_id_id"].Value.ToString(), "",
                            0,
                            dg_site_id.Rows[i].Cells["dg_site_id_title_max"].Value.ToString(),
                            dg_site_id.Rows[i].Cells["dg_site_id_partner_id"].Value.ToString(),
                            dg_site_id.Rows[i].Cells["dg_site_id_shop_id"].Value.ToString(),
                            dg_site_id.Rows[i].Cells["dg_site_id_secret_key"].Value.ToString());

                        DgPrice.Rows.Add(dg_site_id.Rows[i].Cells["dg_site_id_country"].Value,
                            dg_site_id.Rows[i].Cells["dg_site_id_id"].Value);
                    }
                    else
                    {
                        dg_productTitle.Rows.Clear();
                        DgPrice.Rows.Clear();
                        DgVariation.Rows.Clear();
                        dg_shopee_discount.Rows.Clear();
                    }
                }

                metroLabel14.Select();
            }
            else if(e.ColumnIndex == 1 && e.RowIndex > -1)
            {
                bool val = (bool)dg_site_id.SelectedRows[0].Cells["dg_site_id_chk"].Value;

                
                dg_site_id.SelectedRows[0].Cells["dg_site_id_chk"].Value = !val;

                //동일하게 복사하기
                dg_site_id_reg.Rows[dg_site_id.SelectedRows[0].Index].Cells["dg_site_id_reg_chk"].Value = !val;
                dg_site_id_category.Rows[dg_site_id.SelectedRows[0].Index].Cells["dg_site_id_category_chk"].Value = !val;
                dg_site_id_desc.Rows[dg_site_id.SelectedRows[0].Index].Cells["dg_site_id_desc_chk"].Value = !val;

                if (!val)
                {
                    dg_productTitle.Rows.Add("", dg_productTitle.Rows.Count + 1, false,
                            dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString(), "",
                            0,
                            dg_site_id.SelectedRows[0].Cells["dg_site_id_title_max"].Value.ToString(),
                            dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString(),
                            dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString(),
                            dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString());

                    DgPrice.Rows.Add(dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value,
                            dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value);

                    
                }
                else
                {
                    string shopee_id = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                    //찾아서 지운다
                    for (int i = 0; i < dg_productTitle.Rows.Count; i++)
                    {
                        if(shopee_id == dg_productTitle.Rows[i].Cells["dg_productTitle_id"].Value.ToString())
                        {
                            dg_productTitle.Rows.RemoveAt(i);
                            break;
                        }
                    }

                    //찾아서 지운다
                    for (int i = 0; i < DgPrice.Rows.Count; i++)
                    {
                        if (shopee_id == DgPrice.Rows[i].Cells["DgPrice_shopee_id"].Value.ToString())
                        {
                            DgPrice.Rows.RemoveAt(i);
                            break;
                        }
                    }


                }

                metroLabel14.Select();

                //할인 이벤트 목록
                if (DgPrice.Rows.Count > 0 && DgPrice.SelectedRows.Count > 0)
                {
                    string shopeeId = DgPrice.SelectedRows[0].Cells["DgPrice_shopee_id"].Value.ToString();
                    getPromotionList(shopeeId);
                }
            }

            if (DgPrice.Rows.Count > 0 && DgPrice.SelectedRows.Count > 0)
            {
                DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(0, DgPrice.SelectedRows[0].Index);
                DgPrice_CellClick(null, et);
            }
            else if(DgPrice.Rows.Count > 0 && DgPrice.SelectedRows.Count == 0)
            {
                DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(0, 0);
                DgPrice_CellClick(null, et);
            }
        }

        private void dg_site_id_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dg_productTitle_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            checkLength();
        }

        private void dg_productTitle_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(chkCopy.Checked)
            {
                string str_title = dg_productTitle.Rows[e.RowIndex].Cells["dg_productTitle_title"].Value.ToString().Trim();
                for (int i = 0; i < dg_productTitle.Rows.Count; i++)
                {
                    dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value = str_title;
                }
            }
            
        }
        private void checkLength()
        {
            if (dg_productTitle.Rows.Count > 0)
            {
                for (int i = 0; i < dg_productTitle.Rows.Count; i++)
                {
                    int len = dg_productTitle.Rows[i].Cells["dg_productTitle_title"].Value.ToString().Length;
                    dg_productTitle.Rows[i].Cells["dg_productTitle_length"].Value = len;
                }
            }
        }
        private void dg_site_id_reg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void GetCategoryAPI(string tarCountry)
        {
            Cursor.Current = Cursors.WaitCursor;
            dgCategoryList.Rows.Clear();

            string endPoint = "https://shopeecategory.azurewebsites.net/api/ShopeeCategory?CountryCode=" + tarCountry;
            var request = new RestRequest("", RestSharp.Method.GET);
            request.Method = Method.GET;
            var client = new RestClient(endPoint);
            IRestResponse response = client.Execute(request);
            List<APIShopeeCategory> lstShopeeCategory = new List<APIShopeeCategory>();
            var result = JsonConvert.DeserializeObject<List<APIShopeeCategory>>(response.Content);

            for (int i = 0; i < result.Count; i++)
            {
                dgCategoryList.Rows.Add(i + 1,
                    result[i].Category1Name?.ToString() ?? "",
                    result[i].Category2Name?.ToString() ?? "",
                    result[i].Category3Name?.ToString() ?? "",
                    result[i].LastCategoryId.ToString());
            }

            dgCategoryList.ClearSelection();

            Cursor.Current = Cursors.Default;
        }
        private void dg_site_id_category_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DgAttribute.Rows.Clear();
            string tarCountry = dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_country"].Value.ToString();
            getFavCategoryList(dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_id"].Value.ToString());
            GetCategoryAPI(tarCountry);

            string category_id = dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_set_category_id"].Value?.ToString();
            if(category_id != string.Empty)
            {
                for (int i = 0; i < dgCategoryList.Rows.Count; i++)
                {
                    if(dgCategoryList.Rows[i].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString() == category_id)
                    {
                        dgCategoryList.Rows[i].Selected = true;
                        dgCategoryList.FirstDisplayedScrollingRowIndex = i;
                        break;
                    }
                }

                if(dgCategoryList.SelectedRows.Count > 0)
                {
                    DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(0, dgCategoryList.SelectedRows[0].Index);
                    dgCategoryList_CellClick(null, et);
                }
            }
        }

        private void BtnDeleteAttribute_Click(object sender, EventArgs e)
        {
            for (int i = DgSaveAttribute.SelectedRows.Count - 1; i >= 0 ; i--)
            {
                DgSaveAttribute.Rows.RemoveAt(DgSaveAttribute.SelectedRows[i].Index);
            }
        }

        private void Rd_None_Variation_Click(object sender, EventArgs e)
        {
            DgVariation.Rows.Clear();
            DgVariation.Enabled = false;
        }

        private void Rd_None_Variation1_Click(object sender, EventArgs e)
        {
            DgVariation.Enabled = true;
        }

        private void DgVariation_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
        }

        private void BtnAddOption_Click(object sender, EventArgs e)
        {
            string option = TxtVariation.Text.Trim();
            string optionSKU = TxtVariationSKU.Text.Trim();
            Rd_None_Variation1.Checked = true;
            DgVariation.Enabled = true;
            if (option != string.Empty && optionSKU != string.Empty)
            {
                if(DgPrice.Rows.Count == 0)
                {
                    MessageBox.Show("판매 채널이 없습니다. \r\n판매 채널을 선택하여 주세요.",
                        "판매채널 선택",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                if(dg_shopee_discount.Rows.Count == 0 || dg_shopee_discount.SelectedRows.Count == 0)
                {
                    MessageBox.Show("할인 이벤트가 없습니다. \r\n할인 이벤트를 선택하여 주세요.",
                        "할인 이벤트 선택",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }


                
                for (int i = 0; i < DgPrice.Rows.Count; i++)
                {
                    //동일한 옵션이 있는지 검증해 준다.
                    bool isValid = true;
                    for (int j = 0; j < DgVariation.Rows.Count; j++)
                    {
                        if (DgVariation.Rows[j].Cells["DgSrcVariation_Name"].Value?.ToString().Trim()  == option &&
                            DgVariation.Rows[j].Cells["DgSrcVariation_shopee_id"].Value.ToString().Trim() ==
                            DgPrice.Rows[i].Cells["DgPrice_shopee_id"].Value.ToString())
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if(isValid)
                    {
                        //옵션 추가시에 설정된 가격이나 정보가 있으면 같이 설정해 준다.
                        if(DgPrice.SelectedRows[0].Cells["DgPrice_source_price"].Value != null)
                        {
                            string DgSrcVariation_supply_price = DgPrice.SelectedRows[0].Cells["DgPrice_source_price"].Value.ToString();
                            string DgSrcVariation_margin = DgPrice.SelectedRows[0].Cells["DgPrice_margin"].Value.ToString();
                            string DgSrcVariation_weight = DgPrice.SelectedRows[0].Cells["DgPrice_weight"].Value? .ToString() ?? "";
                            string DgSrcVariation_pg_fee = DgPrice.SelectedRows[0].Cells["DgPrice_pgfee"].Value?.ToString() ?? "";
                            string DgSrcVariation_price_won = DgPrice.SelectedRows[0].Cells["DgPrice_sell_price_krw"].Value?.ToString() ?? "";
                            string DgSrcVariation_src_price = DgPrice.SelectedRows[0].Cells["DgPrice_sell_price"].Value?.ToString() ?? "";
                            string DgSrcVariation_src_original_price = DgPrice.SelectedRows[0].Cells["DgPrice_retail_price"].Value?.ToString() ?? "";
                            string DgSrcVariation_stock = DgPrice.SelectedRows[0].Cells["DgPrice_qty"].Value? .ToString() ?? "";
                            DgVariation.Rows.Add(DgVariation.Rows.Count + 1,
                            DgPrice.Rows[i].Cells["DgPrice_tar_country"].Value? .ToString() ?? "",
                            DgPrice.Rows[i].Cells["DgPrice_shopee_id"].Value?.ToString() ?? "",
                            false,
                            "",
                            "",
                            optionSKU,
                            option,
                            option,
                            DgSrcVariation_supply_price,
                            DgSrcVariation_margin,
                            DgSrcVariation_weight,
                            DgSrcVariation_pg_fee,
                            DgSrcVariation_price_won,
                            DgSrcVariation_src_price,
                            DgSrcVariation_src_original_price,
                            DgSrcVariation_stock);

                            //DgVariation.Rows[DgVariation.Rows.Count - 1].Cells["DgSrcVariation_discount_name"].Value = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_name"].Value.ToString();
                            //DgVariation.Rows[DgVariation.Rows.Count - 1].Cells["DgSrcVariation_discount_id"].Value = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_id"].Value.ToString();
                        }
                    }
                        
                    else
                    {
                        MessageBox.Show("동일한 옵션을 추가할 수 없습니다.", "옵션중복 ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }

                TxtVariation.Text = "";
                TxtVariationSKU.Text = "";

                TxtVariationSKU.Select();
            }
        }

        private void TxtVariation_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                BtnAddOption_Click(null, null);
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

        private void getFavCategoryList(string shopeeId)
        {
            dgCategoryList_fav.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<FavoriteCategoryData> favCategoryList = new List<FavoriteCategoryData>();
                favCategoryList = context.FavoriteCategoryDatas
                    .Where(x => x.shopeeId == shopeeId
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.category1_name).ThenBy(x => x.category2_name).ThenBy(x => x.category3_name).ToList();

                for (int i = 0; i < favCategoryList.Count; i++)
                {
                    dgCategoryList_fav.Rows.Add(dgCategoryList_fav.Rows.Count + 1,
                        favCategoryList[i].category1_name,
                        favCategoryList[i].category2_name,
                        favCategoryList[i].category3_name,
                        favCategoryList[i].category3_id);
                }
            }
        }
        private void getFavKeywordList()
        {
            dgFavKeyword.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<FavKeyword> favKeywordList = new List<FavKeyword>();
                favKeywordList = context.FavKeywords.Where(x => x.UserId == global_var.userId)
                    .OrderBy(x => x.Keyword).ToList();

                for (int i = 0; i < favKeywordList.Count; i++)
                {
                    dgFavKeyword.Rows.Add(favKeywordList[i].Keyword);
                }
            }
        }

        private void getFavKeywordOtherList()
        {
            dgFavKeywordOther.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<FavKeywordOther> favKeywordOtherList = new List<FavKeywordOther>();
                favKeywordOtherList = context.FavKeywordOthers.Where(x => x.UserId == global_var.userId)
                    .OrderBy(x => x.Keyword).ToList();

                for (int i = 0; i < favKeywordOtherList.Count; i++)
                {
                    dgFavKeywordOther.Rows.Add(favKeywordOtherList[i].Keyword);
                }
            }
        }

        
        private void BtnAddFavCategory_Click(object sender, EventArgs e)
        {
            if (dgCategoryList.Rows.Count > 0 &&
                dgCategoryList.SelectedRows.Count > 0 &&
                dg_site_id_category.Rows.Count > 0 &&
                dg_site_id_category.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    string shopeeId = dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_id"].Value.ToString();
                    string category3_id = dgCategoryList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();
                    var data = context.FavoriteCategoryDatas.FirstOrDefault(x => x.shopeeId == shopeeId && x.category3_id == category3_id && x.UserId == global_var.userId);

                    if(data == null)
                    {
                        FavoriteCategoryData fav = new FavoriteCategoryData
                        {
                            shopeeId = shopeeId,
                            category1_name = dgCategoryList.SelectedRows[0].Cells["dgSrcItemList_Src_cat1"].Value.ToString(),
                            category2_name = dgCategoryList.SelectedRows[0].Cells["dgSrcItemList_Src_cat2"].Value.ToString(),
                            category3_name = dgCategoryList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3"].Value.ToString(),
                            category3_id = category3_id,
                            UserId = global_var.userId
                        };
                        context.FavoriteCategoryDatas.Add(fav);
                        context.SaveChanges();
                        getFavCategoryList(dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_id"].Value.ToString());
                    }                    
                }
            }
        }

        private void dgCategoryList_fav_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgCategoryList_fav.Rows.Count > 0 &&
                dgCategoryList_fav.SelectedRows.Count > 0 &&
                dg_site_id_category.Rows.Count > 0 &&
                dg_site_id_category.SelectedRows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;                
                string strCategoryId = dgCategoryList_fav.SelectedRows[0].Cells["dgCategoryList_fav_cat3_id"].Value.ToString();
                dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_set_category_id"].Value = strCategoryId;
                dg_site_id_category.EndEdit();
                string tarCountry = dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_country"].Value.ToString();
                GetTarAttributeData(Convert.ToInt32(strCategoryId), true, tarCountry);
                Cursor.Current = Cursors.Default;
            }
        }

        private void dgFavKeyword_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
        }

        private void dgFavKeyword_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                if(dgFavKeyword.Rows[e.RowIndex].Cells[0].Value != null)
                {
                    string keyWord = dgFavKeyword.Rows[e.RowIndex].Cells[0].Value.ToString();
                    var exist = context.FavKeywords.FirstOrDefault(x => x.Keyword == keyWord && x.UserId == global_var.userId);
                    if (exist == null)
                    {
                        FavKeyword fa = new FavKeyword
                        {
                            Keyword = keyWord,
                            UserId = global_var.userId
                        };
                        context.FavKeywords.Add(fa);
                        context.SaveChanges();
                        getFavKeywordList();
                    }
                }
                
            }
        }

        private void dgFavKeyword_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(DgAttribute.Rows.Count > 0 &&
                DgAttribute.SelectedRows.Count > 0 &&
                dgFavKeyword.SelectedRows[0].Cells[0].Value != null)
            {
                DgAttribute.SelectedRows[0].Cells["DgAttribute_attribute_value"].Value = dgFavKeyword.SelectedRows[0].Cells[0].Value.ToString();
            }
        }

        private void dgFavKeyword_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            
        }

        private void dgFavKeyword_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (dgFavKeyword.Rows.Count > 0)
            {
                string keyword = dgFavKeyword.Rows[e.Row.Index].Cells[0].Value.ToString();
                using (AppDbContext context = new AppDbContext())
                {
                    var fWord = context.FavKeywords.FirstOrDefault(x => x.Keyword == keyword && x.UserId == global_var.userId);
                    if (fWord != null)
                    {
                        context.FavKeywords.Remove(fWord);
                        context.SaveChanges();
                    }
                }
            }
        }

        private void BtnDelFavCategory_Click(object sender, EventArgs e)
        {
            if (dgCategoryList_fav.Rows.Count > 0 &&
                dgCategoryList_fav.SelectedRows.Count > 0 &&
                dg_site_id_category.Rows.Count > 0 &&
                dg_site_id_category.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    string shopeeId = dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_id"].Value.ToString();
                    string category3_id = dgCategoryList_fav.SelectedRows[0].Cells["dgCategoryList_fav_cat3_id"].Value.ToString();
                    var data = context.FavoriteCategoryDatas.FirstOrDefault(x => x.shopeeId == shopeeId && x.category3_id == category3_id && x.UserId == global_var.userId);

                    if (data != null)
                    {   
                        context.FavoriteCategoryDatas.Remove(data);
                        context.SaveChanges();
                        dgCategoryList_fav.Rows.RemoveAt(dgCategoryList_fav.SelectedRows[0].Index);
                    }
                }
            }
        }

        private void BtnFavCategorySearch_Click(object sender, EventArgs e)
        {
            string keyWord = TxtFavCategorySearchText.Text.Trim().ToUpper();
            if (dg_site_id_category.Rows.Count > 0 && dg_site_id_category.SelectedRows.Count > 0 &&
                keyWord != string.Empty)
            {
                Cursor.Current = Cursors.WaitCursor;

                string shopeeId = dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_id"].Value.ToString();
                dgCategoryList_fav.Rows.Clear();
                

                using (AppDbContext context = new AppDbContext())
                {
                    List<FavoriteCategoryData> favList = new List<FavoriteCategoryData>();
                    favList = context.FavoriteCategoryDatas
                        .Where(s =>
                        s.shopeeId == shopeeId &&
                        (s.category1_name.ToString().ToUpper().Contains(keyWord) ||
                        s.category2_name.ToString().ToUpper().Contains(keyWord) ||
                        s.category3_name.ToString().ToUpper().Contains(keyWord))
                        && s.UserId == global_var.userId)
                        .OrderBy(x => x.category1_name).ThenBy(x => x.category2_name).ThenBy(x => x.category3_name).ToList();


                    for (int i = 0; i < favList.Count; i++)
                    {
                        dgCategoryList_fav.Rows.Add(dgCategoryList_fav.Rows.Count + 1,
                            favList[i].category1_name,
                            favList[i].category2_name,
                            favList[i].category3_name,
                            favList[i].category3_id);
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void TxtFavCategorySearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                BtnFavCategorySearch_Click(null, null);
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

                if (dg_site_id.Rows.Count > 0 && dg_site_id.SelectedRows.Count > 0)
                {
                    DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(0, dg_site_id.SelectedRows[0].Index);
                    dg_site_id_CellClick(null, et);
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("환율 정보를 업데이트 하였습니다.", "환율정보 업데이트", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSyncShippingData_Click(object sender, EventArgs e)
        {
            Menu_Logistic_Sync_Click(null, null);
        }

        private void BtnSetHeader_Click(object sender, EventArgs e)
        {
            if(dg_list_header_set.Rows.Count > 0 &&
                dg_list_header_set.SelectedRows.Count > 0 &&
                dg_site_id_desc.Rows.Count > 0 &&
                dg_site_id_desc.SelectedRows.Count > 0)
            {
                if((bool)dg_site_id_desc.SelectedRows[0].Cells["dg_site_id_desc_chk"].Value == true)
                {
                    dg_site_id_desc.SelectedRows[0].Cells["dg_site_id_desc_header_id"].Value =
                    dg_list_header_set.SelectedRows[0].Cells["dg_headerset_id"].Value.ToString();

                    dg_site_id_desc.SelectedRows[0].Cells["dg_site_id_desc_header_name"].Value =
                        dg_list_header_set.SelectedRows[0].Cells["dg_headerset_name"].Value.ToString();
                }
                
            }
        }

        private void BtnSetFooter_Click(object sender, EventArgs e)
        {
            if (dg_list_footer_set.Rows.Count > 0 &&
                dg_list_footer_set.SelectedRows.Count > 0 &&
                dg_site_id_desc.Rows.Count > 0 &&
                dg_site_id_desc.SelectedRows.Count > 0)
            {
                if ((bool)dg_site_id_desc.SelectedRows[0].Cells["dg_site_id_desc_chk"].Value == true)
                {
                    dg_site_id_desc.SelectedRows[0].Cells["dg_site_id_desc_footer_id"].Value =
                    dg_list_footer_set.SelectedRows[0].Cells["dg_footerset_id"].Value.ToString();

                    dg_site_id_desc.SelectedRows[0].Cells["dg_site_id_desc_footer_name"].Value =
                        dg_list_footer_set.SelectedRows[0].Cells["dg_footerset_name"].Value.ToString();
                }   
            }
        }

        private void TabMain_Category_Attribute_Click(object sender, EventArgs e)
        {

        }

        private void dgFavKeywordOther_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgAttribute.Rows.Count > 0 &&
                DgAttribute.SelectedRows.Count > 0 &&
                dgFavKeywordOther.SelectedRows[0].Cells[0].Value != null)
            {
                DgAttribute.SelectedRows[0].Cells["DgAttribute_attribute_value"].Value = dgFavKeywordOther.SelectedRows[0].Cells[0].Value.ToString();
            }
        }

        private void dgFavKeywordOther_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                if (dgFavKeywordOther.Rows[e.RowIndex].Cells[0].Value != null)
                {
                    string keyWord = dgFavKeywordOther.Rows[e.RowIndex].Cells[0].Value.ToString();
                    var exist = context.FavKeywordOthers.FirstOrDefault(x => x.Keyword == keyWord && x.UserId == global_var.userId);
                    if (exist == null)
                    {
                        FavKeywordOther fa = new FavKeywordOther
                        {
                            Keyword = keyWord,
                            UserId = global_var.userId
                        };
                        context.FavKeywordOthers.Add(fa);
                        context.SaveChanges();
                        getFavKeywordOtherList();
                    }
                }

            }
        }

        private void dgFavKeywordOther_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (dgFavKeywordOther.Rows.Count > 0)
            {
                string keyword = dgFavKeywordOther.Rows[e.Row.Index].Cells[0].Value.ToString();
                using (AppDbContext context = new AppDbContext())
                {
                    var fWord = context.FavKeywordOthers.FirstOrDefault(x => x.Keyword == keyword && x.UserId == global_var.userId);
                    if (fWord != null)
                    {
                        context.FavKeywordOthers.Remove(fWord);
                        context.SaveChanges();
                    }
                }
            }
        }

        private void DgPrice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //할인 이벤트 목록
            if (DgPrice.Rows.Count > 0 && DgPrice.SelectedRows.Count > 0)
            {
                string shopeeId = DgPrice.SelectedRows[0].Cells["DgPrice_shopee_id"].Value.ToString();
                getPromotionList(shopeeId);
            }
            else if(DgPrice.Rows.Count > 0 && DgPrice.SelectedRows.Count == 0)
            {
                string shopeeId = DgPrice.Rows[0].Cells["DgPrice_shopee_id"].Value.ToString();
                getPromotionList(shopeeId);
            }
        }

        private void PromotionMenu_Sync_Click(object sender, EventArgs e)
        {
            if(DgPrice.Rows.Count > 0 && DgPrice.SelectedRows.Count > 0)
            {
                DialogResult dlg_Result = MessageBox.Show("쇼피 할인이벤트 목록을 동기화 하시겠습니까?", "할인 이벤트 동기화", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg_Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    dg_shopee_discount.Rows.Clear();
                    string shopeeId = DgPrice.SelectedRows[0].Cells["DgPrice_shopee_id"].Value.ToString();
                    long partner_id = 0;
                    long shop_id = 0;
                    string api_key = "";

                    //관련 정보를 찾아온다.
                    for (int i = 0; i < dg_site_id.Rows.Count; i++)
                    {
                        if(dg_site_id.Rows[i].Cells["dg_site_id_id"].Value.ToString() == shopeeId)
                        {
                            partner_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_partner_id"].Value.ToString());
                            shop_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_shop_id"].Value.ToString());
                            api_key = dg_site_id.Rows[i].Cells["dg_site_id_secret_key"].Value.ToString();
                            break;
                        }
                    }
                    int pagination_offset = 0;
                    int pagination_entries_per_page = 100;


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
        }

        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
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
            if (dg_shopee_discount.Rows.Count > 0 && dg_shopee_discount.SelectedRows.Count > 0)
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

        private void BtnAddDiscountEvent_Click(object sender, EventArgs e)
        {
            if(dg_shopee_discount.Rows.Count > 0 &&
                DgPrice.Rows.Count > 0 && 
                DgPrice.SelectedRows.Count > 0 &&
                dg_shopee_discount.SelectedRows.Count > 0)
            {
                DgPrice.SelectedRows[0].Cells["DgPrice_discount_name"].Value = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_name"].Value.ToString();
                DgPrice.SelectedRows[0].Cells["DgPrice_discount_id"].Value = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_id"].Value.ToString();

                string shopeeId = DgPrice.SelectedRows[0].Cells["DgPrice_shopee_id"].Value.ToString();
                for (int i = 0; i < DgVariation.Rows.Count; i++)
                {
                    if(DgVariation.Rows[i].Cells["DgSrcVariation_shopee_id"].Value.ToString() == shopeeId)
                    {
                        DgVariation.Rows[i].Cells["DgSrcVariation_discount_name"].Value = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_name"].Value.ToString();
                        DgVariation.Rows[i].Cells["DgSrcVariation_discount_id"].Value = dg_shopee_discount.SelectedRows[0].Cells["dg_shopee_discount_discount_id"].Value.ToString();
                    }
                }
            }


        }

        private void dg_site_id_reg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dg_site_id_reg.Rows.Count > 0)
            {
                string siteUrl = "";
                string goods_no = dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_item_no"].Value.ToString();
                string tarCountry = dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_country"].Value.ToString();
                long partner_id = Convert.ToInt64(dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_partner_id"].Value.ToString());
                long shop_id = Convert.ToInt64(dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_shop_id"].Value.ToString());
                string api_key = dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_secret_key"].Value.ToString();

                if (tarCountry == "SG")
                {
                    siteUrl = "https://shopee.sg/product/" + shop_id + "/" + goods_no;
                }
                else if (tarCountry == "MY")
                {
                    siteUrl = "https://shopee.com.my/product/" + shop_id + "/" + goods_no;
                }
                else if (tarCountry == "ID")
                {
                    siteUrl = "https://shopee.co.id/product/" + shop_id + "/" + goods_no;
                }
                else if (tarCountry == "TH")
                {
                    siteUrl = "https://shopee.co.th/product/" + shop_id + "/" + goods_no;
                }
                else if (tarCountry == "TW")
                {
                    siteUrl = "https://shopee.tw/product/" + shop_id + "/" + goods_no;
                }
                else if (tarCountry == "PH")
                {
                    siteUrl = "https://shopee.ph/product/" + shop_id + "/" + goods_no;
                }
                else if (tarCountry == "VN")
                {
                    siteUrl = "https://shopee.vn/product/" + shop_id + "/" + goods_no;
                }
                System.Diagnostics.Process.Start("chrome.exe", siteUrl);
            }
        }

        private void DgAttribute_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(DgAttribute.Rows[e.RowIndex].Cells["DgAttribute_attribute_value"].Value != null &&
               DgAttribute.Rows[e.RowIndex].Cells["DgAttribute_attribute_value"].Value.ToString().Trim() != string.Empty)
            {
                DataGridViewRow dgvrow = null;

                if (DgSaveAttribute.Rows.Count > 0)
                {
                    dgvrow = DgSaveAttribute.Rows
                        .Cast<DataGridViewRow>()
                        .Where(
                        r => r.Cells["DgSaveAttribute_id"].Value.ToString().Equals(DgAttribute.Rows[DgAttribute.SelectedRows[0].Index].Cells["DgAttribute_attribute_id"].Value.ToString())
                        && r.Cells["DgSaveAttribute_shopee_id"].Value.ToString().Equals(dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_id"].Value.ToString()))
                        .FirstOrDefault();
                }

                if (dgvrow is null)
                {
                    DgSaveAttribute.Rows.Add(DgSaveAttribute.Rows.Count + 1, // DgSaveAttribute_no, visible true
                    false, // DgSaveAttribute_chk, visible false
                    dg_site_id_category.SelectedRows[0].Cells["dg_site_id_category_id"].Value.ToString(), // DgSaveAttribute_shopee_id, visible true
                    DgAttribute.Rows[DgAttribute.SelectedRows[0].Index].Cells["DgAttribute_attribute_name"].Value.ToString().Trim(), // DgSaveAttribute_name, visible true
                    DgAttribute.Rows[DgAttribute.SelectedRows[0].Index].Cells["DgAttribute_attribute_id"].Value.ToString().Trim(), // DgSaveAttribute_id, visible true
                    (bool)DgAttribute.Rows[DgAttribute.SelectedRows[0].Index].Cells["DgTarAttribute_is_mandatory"].Value, // DgSaveAttribute_is_mandatory, visible true
                    DgAttribute.Rows[DgAttribute.SelectedRows[0].Index].Cells["DgAttribute_attribute_value"].Value.ToString().Trim()); // DgSaveAttribute_value, visible true
                }
                else
                {
                    int selectedIndex = dgvrow.Index;

                    DataGridViewRow selectedRow = DgSaveAttribute.Rows[selectedIndex];
                    selectedRow.Cells["DgSaveAttribute_value"].Value = DgAttribute.Rows[DgAttribute.SelectedRows[0].Index].Cells["DgAttribute_attribute_value"].Value.ToString().Trim();
                }
            }
        }

        private void UdRetailPriceRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                //전체 데이터를 업데이트 한다.
                for (int i = 0; i < DgPrice.Rows.Count; i++)
                {
                    DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(3, i);
                    DgPrice_CellValueChanged(null, et);
                }

                for (int i = 0; i < DgVariation.Rows.Count; i++)
                {
                    DataGridViewCellEventArgs et2 = new DataGridViewCellEventArgs(9, i);
                    DgVariation_CellValueChanged(null, et2);
                }
            }
        }

        private void udPGFee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                //전체 데이터를 업데이트 한다.
                for (int i = 0; i < DgPrice.Rows.Count; i++)
                {
                    DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(3, i);
                    DgPrice_CellValueChanged(null, et);
                }

                for (int i = 0; i < DgVariation.Rows.Count; i++)
                {
                    DataGridViewCellEventArgs et2 = new DataGridViewCellEventArgs(9, i);
                    DgVariation_CellValueChanged(null, et2);
                }
            }
        }

        private void UdShopeeFee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                //전체 데이터를 업데이트 한다.
                for (int i = 0; i < DgPrice.Rows.Count; i++)
                {
                    DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(3, i);
                    DgPrice_CellValueChanged(null, et);
                }

                for (int i = 0; i < DgVariation.Rows.Count; i++)
                {
                    DataGridViewCellEventArgs et2 = new DataGridViewCellEventArgs(9, i);
                    DgVariation_CellValueChanged(null, et2);
                }
            }
        }

        private void UdPayoneerFee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                //전체 데이터를 업데이트 한다.
                for (int i = 0; i < DgPrice.Rows.Count; i++)
                {
                    DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(3, i);
                    DgPrice_CellValueChanged(null, et);
                }

                for (int i = 0; i < DgVariation.Rows.Count; i++)
                {
                    DataGridViewCellEventArgs et2 = new DataGridViewCellEventArgs(9, i);
                    DgVariation_CellValueChanged(null, et2);
                }
            }
        }

        private void dg_shopee_discount_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            BtnAddDiscountEvent_Click(null, null);
        }

        private void dg_list_header_set_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            BtnSetHeader_Click(null, null);
        }

        private void dg_list_footer_set_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            BtnSetFooter_Click(null, null);
        }

        private void BtnSelectDetailImageFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = "jpg";
            openFileDlg.Filter = "Image Files(*.JPG;*.PNG)|*.JPG;*.PNG|All files (*.*)|*.*";
            openFileDlg.Multiselect = true;
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                //DGSelectedList.Rows.Clear();
                if (!Directory.Exists(ImagePathThumb))
                {
                    Directory.CreateDirectory(ImagePathThumb);
                }
                int startIdx = DGDetailList.Rows.Count;
                WebClient wc = new WebClient();
                for (int i = 0; i < openFileDlg.FileNames.Length; i++)
                {
                    string fileName = Path.GetFileName(openFileDlg.FileNames[i]);
                    string fileExt = Path.GetExtension(openFileDlg.FileNames[i]);
                    string destPath = ImagePathThumb + @"\" + $"thumb_{startIdx + 1:000}" + fileExt;

                    File.Copy(openFileDlg.FileNames[i], destPath, true);
                    var content = wc.DownloadData(destPath);
                    using (var stream = new MemoryStream(content))
                    {
                        Image im = Image.FromStream(stream);
                        DGDetailList.Rows.Add(startIdx + 1, false, im, $"thumb_{startIdx + 1:000}.jpg", im.Width, im.Height,
                            destPath);
                    }
                    startIdx++;
                }

                lblThumb.Text = "기본 썸네일 [ " + DGSelectedList.Rows.Count + " ]";
            }
        }

        private void BtnManageAttributeData_Click(object sender, EventArgs e)
        {
            FormAttributeManage fa = new FormAttributeManage();
            fa.ShowDialog();
        }

        private void BtnGetDiscountList_Click(object sender, EventArgs e)
        {
            if (DgPrice.Rows.Count > 0 && DgPrice.SelectedRows.Count > 0)
            {
                DialogResult dlg_Result = MessageBox.Show("쇼피 할인이벤트 목록을 동기화 하시겠습니까?", "할인 이벤트 동기화", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg_Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    dg_shopee_discount.Rows.Clear();
                    string shopeeId = DgPrice.SelectedRows[0].Cells["DgPrice_shopee_id"].Value.ToString();
                    long partner_id = 0;
                    long shop_id = 0;
                    string api_key = "";

                    //관련 정보를 찾아온다.
                    for (int i = 0; i < dg_site_id.Rows.Count; i++)
                    {
                        if (dg_site_id.Rows[i].Cells["dg_site_id_id"].Value.ToString() == shopeeId)
                        {
                            partner_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_partner_id"].Value.ToString());
                            shop_id = Convert.ToInt64(dg_site_id.Rows[i].Cells["dg_site_id_shop_id"].Value.ToString());
                            api_key = dg_site_id.Rows[i].Cells["dg_site_id_secret_key"].Value.ToString();
                            break;
                        }
                    }
                    int pagination_offset = 0;
                    int pagination_entries_per_page = 100;


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
                        if (result.discount.Count > 0)
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
                        else
                        {
                            MessageBox.Show("진행중인 할인 이벤트가 없습니다.\r\n" +
                                "셀러센터의 Market Centre > My Discount Promotions 에서 새로운 할인이벤트를 생성해 주세요.\r\n" + 
                                "신규 생성 시 설정한 시작 시간 이후에 활성화 되며 활성화 이후에 할인 이벤트 동기화 버튼을 클릭해 주세요.",
                                "할인 이벤트 목록 없음",MessageBoxButtons.OK,MessageBoxIcon.Information);
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
        }

        private void BtnDelOption_Click(object sender, EventArgs e)
        {
            if(DgVariation.Rows.Count > 0 && DgVariation.SelectedRows.Count > 0)
            {
                DgVariation.Rows.RemoveAt(DgVariation.SelectedRows[0].Index);
            }
        }

        private void BtnDoScrap_Click(object sender, EventArgs e)
        {
            string address = TxtExternalProductAddr.Text.Trim();
            if (address != string.Empty)
            {
                doScrap(address);
            }
        }

        private void Dg_site_id_reg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_json"].Value != null)
            {
                string json = dg_site_id_reg.SelectedRows[0].Cells["dg_site_id_reg_json"].Value.ToString().Trim();
                if (json != string.Empty)
                {
                    Deserialize(json);
                }
            }
            
        }

        private void Deserialize(string json)
        {
            jsonExplorer.Nodes.Clear();
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Dictionary<string, object> dic = js.Deserialize<Dictionary<string, object>>(json);

                TreeNode rootNode = new TreeNode("Root");
                jsonExplorer.Nodes.Add(rootNode);
                BuildTree(dic, rootNode);

                jsonExplorer.ExpandAll();
            }
            catch (ArgumentException argE)
            {
                MessageBox.Show("JSON data is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void BuildTree(Dictionary<string, object> dictionary, TreeNode node)
        {
            foreach (KeyValuePair<string, object> item in dictionary)
            {
                TreeNode parentNode = new TreeNode(item.Key);
                node.Nodes.Add(parentNode);

                try
                {
                    dictionary = (Dictionary<string, object>)item.Value;
                    BuildTree(dictionary, parentNode);
                }
                catch (InvalidCastException dicE)
                {
                    try
                    {
                        ArrayList list = (ArrayList)item.Value;
                        foreach (string value in list)
                        {
                            TreeNode finalNode = new TreeNode(value);
                            finalNode.ForeColor = Color.Blue;
                            parentNode.Nodes.Add(finalNode);
                        }

                    }
                    catch (InvalidCastException ex)
                    {
                        TreeNode finalNode = new TreeNode(item.Value.ToString());
                        finalNode.ForeColor = Color.Blue;
                        parentNode.Nodes.Add(finalNode);
                    }
                }
            }
        }

        private void MetroButton1_Click_1(object sender, EventArgs e)
        {
            DgVariation.Rows.Clear();
            for (int i = 0; i < 100; i++)
            {
                DgVariation.Rows.Add(i + 1, "aa", "aaaa", "aaaaadf", false);
            }
        }

        private void BtnRefreshCurrency_Click(object sender, EventArgs e)
        {
            Fill_from_Currency_Names();
        }

        private void BtnDelOptionAll_Click(object sender, EventArgs e)
        {
            DgVariation.Rows.Clear();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;


            string title = "";
            if (RdTitleEng.Checked)
            {
                if (dg_productTitle.Rows.Count > 0)
                {
                    title = dg_productTitle.Rows[0].Cells["dg_productTitle_title"].Value.ToString();
                    if (title == string.Empty)
                    {
                        BtnTranslate_Click(null, null);
                        title = dg_productTitle.Rows[0].Cells["dg_productTitle_title"].Value.ToString();
                        //MessageBox.Show("영문 상품명이 없습니다.","영문 상품명 없음",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                title = TxtProductNameKor.Text.Trim();
            }

            string siteUrl = "https://www.amazon.com/s?k=" + HttpUtility.UrlEncode(title) + "&ref=nb_sb_noss";
            System.Diagnostics.Process.Start("chrome.exe", siteUrl);
            Cursor.Current = Cursors.Default;
        }

        private void BtnSearchEbay_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;


            string title = "";
            if (RdTitleEng.Checked)
            {
                if (dg_productTitle.Rows.Count > 0)
                {
                    title = dg_productTitle.Rows[0].Cells["dg_productTitle_title"].Value.ToString();
                    if (title == string.Empty)
                    {
                        BtnTranslate_Click(null, null);
                        title = dg_productTitle.Rows[0].Cells["dg_productTitle_title"].Value.ToString();
                        //MessageBox.Show("영문 상품명이 없습니다.","영문 상품명 없음",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                title = TxtProductNameKor.Text.Trim();
            }

            string siteUrl = "https://www.ebay.com/sch/i.html?_from=R40&_trksid=m570.l1313&_nkw=" + HttpUtility.UrlEncode(title) + "&_sacat=0";
            System.Diagnostics.Process.Start("chrome.exe", siteUrl);
            Cursor.Current = Cursors.Default;
        }

        private void BtnSearchEbay2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;


            string title = "";
            if (RdTitleEng.Checked)
            {
                if (dg_productTitle.Rows.Count > 0)
                {
                    title = dg_productTitle.Rows[0].Cells["dg_productTitle_title"].Value.ToString();
                    if (title == string.Empty)
                    {
                        BtnTranslate_Click(null, null);
                        title = dg_productTitle.Rows[0].Cells["dg_productTitle_title"].Value.ToString();
                        //MessageBox.Show("영문 상품명이 없습니다.","영문 상품명 없음",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                title = TxtProductNameKor.Text.Trim();
            }

            string siteUrl = "https://www.ebay.com/sch/i.html?_from=R40&_trksid=m570.l1313&_nkw=" + HttpUtility.UrlEncode(title) + "&_sacat=0";
            System.Diagnostics.Process.Start("chrome.exe", siteUrl);
            Cursor.Current = Cursors.Default;
        }

        private void BtnSearchAmazon2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;


            string title = "";
            if (RdTitleEng.Checked)
            {
                if (dg_productTitle.Rows.Count > 0)
                {
                    title = dg_productTitle.Rows[0].Cells["dg_productTitle_title"].Value.ToString();
                    if (title == string.Empty)
                    {
                        BtnTranslate_Click(null, null);
                        title = dg_productTitle.Rows[0].Cells["dg_productTitle_title"].Value.ToString();
                        //MessageBox.Show("영문 상품명이 없습니다.","영문 상품명 없음",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                title = TxtProductNameKor.Text.Trim();
            }

            string siteUrl = "https://www.amazon.com/s?k=" + HttpUtility.UrlEncode(title) + "&ref=nb_sb_noss";
            System.Diagnostics.Process.Start("chrome.exe", siteUrl);
            Cursor.Current = Cursors.Default;
        }

        private void BtnAddToThumbList2_Click(object sender, EventArgs e)
        {
            if (DGDetailList.Rows.Count == 0 ||
                DGDetailList.SelectedRows.Count == 0)
            {
                return;
            }

            for (int i = 0; i < DGDetailList.SelectedRows.Count; i++)
            {
                if (DGSelectedList.Rows.Count < 9)
                {
                    DataGridViewRow dgvRow = new DataGridViewRow();
                    dgvRow = CloneWithValues(DGDetailList.SelectedRows[i]);
                    DGSelectedList.Rows.Add(dgvRow);

                    //파일을 복사해 준다.
                    try
                    {
                        File.Copy(DGDetailList.Rows[i].Cells["DGImageSlicedList_path"].Value.ToString(),
                        dgvRow.Cells[6].Value.ToString());
                    }
                    catch
                    {

                    }

                }
            }

            for (int i = 0; i < DGSelectedList.Rows.Count; i++)
            {
                DGSelectedList.Rows[i].Cells["DGSelectedList_No"].Value = i + 1;
            }

            lblThumb.Text = "기본 썸네일 [ " + DGSelectedList.Rows.Count + " ]";
        }
    }   
}
