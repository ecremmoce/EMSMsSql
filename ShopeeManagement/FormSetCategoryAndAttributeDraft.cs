using MetroFramework.Forms;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormSetCategoryAndAttributeDraft : MetroForm
    {
        public FormUploader fp;
        public FormSetCategoryAndAttributeDraft()
        {
            InitializeComponent();
        }

        public int ItemInfoDraftId;
        public string srcCountry;
        public string tarCountry;
        public string srcCategory;
        public string tarCategory;
        public long srcItemId;
        public long partner_id;
        public long shop_id;
        public string api_key;
        public string tarShopeeId;
        private bool isLoaded = false;

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
        private void FormSetCategory_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            GetCategory();
            GridInit();
            Application.DoEvents();
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
        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }
        private void GetSrcAttributeData(int categoryID)
        {
            Cursor.Current = Cursors.WaitCursor;
            DgSrcAttribute.Rows.Clear();
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
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            IRestResponse response_attribute = client_attribute.Execute(request_attribute);
            var content_attribute = response_attribute.Content;
            dynamic result_attribute = JsonConvert.DeserializeObject(content_attribute);
            if (result_attribute != null && result_attribute.attributes != null)
            {
                for (int i = 0; i < result_attribute.attributes.Count; i++)
                {
                    DataGridViewComboBoxCell NewComboCell = new DataGridViewComboBoxCell();
                    NewComboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;

                    dynamic option = result_attribute.attributes[i].values;
                    for (int j = 0; j < option.Count; j++)
                    {
                        NewComboCell.Items.Add(option[j].original_value.ToString());
                    }

                    DgSrcAttribute.Rows.Add(i + 1,
                        result_attribute.attributes[i].attribute_name.ToString(),
                        result_attribute.attributes[i].input_type.ToString(),
                        result_attribute.attributes[i].attribute_id.ToString(),
                        result_attribute.attributes[i].attribute_type.ToString(),
                        (bool)result_attribute.attributes[i].is_mandatory,
                        null,
                        false,
                        "");
                    DgSrcAttribute.Rows[DgSrcAttribute.Rows.Count - 1].Cells[6] = NewComboCell;
                }

                using (AppDbContext context = new AppDbContext())
                {
                    List<ItemAttributeDraft> attributeList = context.ItemAttributeDrafts
                                    .Where(b => b.src_item_id == srcItemId &&
                                    b.tar_shopeeAccount == tarShopeeId
                                    && b.UserId == global_var.userId)
                                    .OrderBy(x => x.is_mandatory).ToList();

                    if (attributeList.Count > 0)
                    {
                        for (int i = 0; i < attributeList.Count; i++)
                        {
                            for (int j = 0; j < DgSrcAttribute.Rows.Count; j++)
                            {
                                if (attributeList[i].attribute_name == DgSrcAttribute.Rows[j].Cells["DgSrcAttribute_attribute_name"].Value.ToString())
                                {
                                    DgSrcAttribute.Rows[j].Cells["DgSrcAttribute_attribute_value"].Value = attributeList[i].attribute_value;
                                }
                            }
                            
                        }
                    }
                }

                isLoaded = true;
            }
        }

        private void GetSrcAttributeData2(int categoryID)
        {
            Cursor.Current = Cursors.WaitCursor;
            DgSrcAttribute.Rows.Clear();

            using (AppDbContext context = new AppDbContext())
            {
                List<ItemAttributeDraft> attributeList = context.ItemAttributeDrafts
                                .Where(b => b.src_item_id == srcItemId &&
                                b.tar_shopeeAccount == tarShopeeId
                                && b.UserId == global_var.userId)
                                .OrderByDescending(x => x.is_mandatory).ToList();

                if (attributeList.Count > 0)
                {
                    for (int i = 0; i < attributeList.Count; i++)
                    {
                        DataGridViewComboBoxCell NewComboCell = new DataGridViewComboBoxCell();
                        NewComboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                        dynamic option = null;
                        if(option != null)
                        {
                            for (int j = 0; j < option.Count; j++)
                            {
                                NewComboCell.Items.Add(option[j].original_value.ToString());
                            }
                        }

                        DgSrcAttribute.Rows.Add(i + 1,
                            attributeList[i].attribute_name.ToString(),
                        attributeList[i].attribute_type,
                        attributeList[i].attribute_id.ToString(),
                        attributeList[i].attribute_type.ToString(),
                        (bool)attributeList[i].is_mandatory,
                        null,
                        false,
                        attributeList[i].attribute_value);
                        DgSrcAttribute.Rows[DgSrcAttribute.Rows.Count - 1].Cells[6] = NewComboCell;
                    }
                }
            }
            isLoaded = true;
        }

        private void GridInit()
        {
            DataGridViewComboBoxColumn cboOption = new DataGridViewComboBoxColumn();
            cboOption.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            cboOption.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            cboOption.HeaderText = "옵션";
            cboOption.Name = "DgSrcAttribute_option";

            DataGridViewCheckBoxColumn chkMandatory = new DataGridViewCheckBoxColumn();
            chkMandatory.HeaderText = "필수";
            chkMandatory.Name = "DgSrcAttribute_is_mandatory";

            DgSrcAttribute.Columns.Add("DgSrcAttribute_no", "No");
            DgSrcAttribute.Columns.Add("DgSrcAttribute_attribute_name", "속성명");
            DgSrcAttribute.Columns.Add("DgSrcAttribute_input_type", "입력타입");
            DgSrcAttribute.Columns.Add("DgSrcAttribute_attribute_id", "속성ID");
            DgSrcAttribute.Columns.Add("DgSrcAttribute_attribute_type", "속성타입");
            DgSrcAttribute.Columns.Add(chkMandatory);
            DgSrcAttribute.Columns.Add(cboOption);
            DgSrcAttribute.Columns.Add("DgSrcAttribute_is_complete", "완료여부");
            DgSrcAttribute.Columns.Add("DgSrcAttribute_attribute_value", "설정된 값");
            

            DgSrcAttribute.Columns[0].Width = 28;
            DgSrcAttribute.Columns[1].Width = 250;
            DgSrcAttribute.Columns[2].Width = 87;
            DgSrcAttribute.Columns[3].Width = 70;
            DgSrcAttribute.Columns[4].Width = 104;
            DgSrcAttribute.Columns[5].Width = 36;
            DgSrcAttribute.Columns[6].Width = 250;
            DgSrcAttribute.Columns[7].Width = 80;
            DgSrcAttribute.Columns[8].Width = 250;
            

            DgSrcAttribute.Columns[0].ReadOnly = true;
            DgSrcAttribute.Columns[1].ReadOnly = true;
            DgSrcAttribute.Columns[2].ReadOnly = true;
            DgSrcAttribute.Columns[3].ReadOnly = true;
            DgSrcAttribute.Columns[4].ReadOnly = true;
            DgSrcAttribute.Columns[5].ReadOnly = true;
            DgSrcAttribute.Columns[6].ReadOnly = false;
            DgSrcAttribute.Columns[7].ReadOnly = true;
            DgSrcAttribute.Columns[8].ReadOnly = false;
            

            DgSrcAttribute.Columns[2].Visible = false;
            DgSrcAttribute.Columns[4].Visible = false;
            DgSrcAttribute.Columns[6].Visible = false;
            DgSrcAttribute.Columns[7].Visible = false;

            DgSrcAttribute.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgSrcAttribute.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DgSrcAttribute.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgSrcAttribute.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgSrcAttribute.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgSrcAttribute.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgSrcAttribute.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DgSrcAttribute.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgSrcAttribute.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewComboBoxColumn cboTarOption = new DataGridViewComboBoxColumn();
            cboTarOption.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            cboTarOption.HeaderText = "옵션원본";
            cboTarOption.Name = "DgTarAttribute_option";

            DataGridViewComboBoxColumn cboTarOption2 = new DataGridViewComboBoxColumn();
            cboTarOption2.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            cboTarOption2.HeaderText = "옵션번역";
            cboTarOption2.Name = "DgTarAttribute_option2";

            DataGridViewCheckBoxColumn chkTarMandatory = new DataGridViewCheckBoxColumn();
            chkTarMandatory.HeaderText = "필수";
            chkTarMandatory.Name = "DgTarAttribute_is_mandatory";

            DgTarAttribute.Columns.Add("DgTarAttribute_no", "No");
            DgTarAttribute.Columns.Add("DgTarAttribute_attribute_name", "속성명");
            DgTarAttribute.Columns.Add("DgTarAttribute_input_type", "입력타입");
            DgTarAttribute.Columns.Add("DgTarAttribute_attribute_id", "속성ID");
            DgTarAttribute.Columns.Add("DgTarAttribute_attribute_type", "속성타입");
            DgTarAttribute.Columns.Add(chkTarMandatory);
            DgTarAttribute.Columns.Add(cboTarOption);
            DgTarAttribute.Columns.Add(cboTarOption2);
            DgTarAttribute.Columns.Add("DgTarAttribute_is_complete", "완료여부");
            DgTarAttribute.Columns.Add("DgTarAttribute_attribute_value", "설정된 값");
            DgTarAttribute.Columns.Add("DgTarAttribute_attribute_name_src", "속성명_원본");

            DgTarAttribute.Columns[0].Width = 28;
            DgTarAttribute.Columns[1].Width = 140;
            DgTarAttribute.Columns[2].Width = 87;
            DgTarAttribute.Columns[3].Width = 70;
            DgTarAttribute.Columns[4].Width = 104;
            DgTarAttribute.Columns[5].Width = 36;
            DgTarAttribute.Columns[6].Width = 200;
            DgTarAttribute.Columns[7].Width = 200;
            DgTarAttribute.Columns[8].Width = 80;
            DgTarAttribute.Columns[9].Width = 250;
            DgTarAttribute.Columns[10].Width = 250;

            DgTarAttribute.Columns[0].ReadOnly = true;
            DgTarAttribute.Columns[1].ReadOnly = true;
            DgTarAttribute.Columns[2].ReadOnly = true;
            DgTarAttribute.Columns[3].ReadOnly = true;
            DgTarAttribute.Columns[4].ReadOnly = true;
            DgTarAttribute.Columns[5].ReadOnly = true;
            DgTarAttribute.Columns[6].ReadOnly = false;
            DgTarAttribute.Columns[7].ReadOnly = false;
            DgTarAttribute.Columns[8].ReadOnly = true;
            DgTarAttribute.Columns[9].ReadOnly = false;
            DgTarAttribute.Columns[10].ReadOnly = true;


            DgTarAttribute.Columns[2].Visible = false;
            DgTarAttribute.Columns[4].Visible = false;
            DgTarAttribute.Columns[8].Visible = false;
            DgTarAttribute.Columns[10].Visible = false;

            DgTarAttribute.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgTarAttribute.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DgTarAttribute.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgTarAttribute.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgTarAttribute.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgTarAttribute.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgTarAttribute.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DgTarAttribute.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DgTarAttribute.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgTarAttribute.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DgTarAttribute.Columns[5].SortMode = DataGridViewColumnSortMode.Automatic;
        }

        private void GetCategory()
        {
            Cursor.Current = Cursors.WaitCursor;
            dgSrcItemList.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                var lstCategory = context.ShopeeCategorOnlys.Where(x => x.Country == tarCountry)
                        .OrderBy(x => x.CatLevel1)
                        .ThenBy(x => x.CatLevel2)
                        .ThenBy(x => x.CatLevel3)
                        .ThenBy(x => x.CatLevel4)
                        .ThenBy(x => x.CatLevel5).ToList();

                for (int i = 0; i < lstCategory.Count; i++)
                {
                    dgSrcItemList.Rows.Add(i + 1, lstCategory[i].CatLevel1, lstCategory[i].CatLevel2, lstCategory[i].CatLevel3,
                        lstCategory[i].CatId);
                }


                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString() == tarCategory.ToString())
                    {
                        dgSrcItemList.Rows[i].Selected = true;
                        dgSrcItemList.FirstDisplayedScrollingRowIndex = i;
                        break;
                    }
                }
            }

            if (dgSrcItemList.Rows.Count == 0)
            {
                MessageBox.Show("해당 국가의 카테고리 정보가 없습니다.\r\n환경설정 > 카테고리 목록 > 최신 카테고리 데이터 업데이트를 클릭하세요", "카테고리 데이터 없음", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FormConfig fc = new FormConfig("kor");
                fc.ShowDialog();
                GetCategory();
            }

            Cursor.Current = Cursors.Default;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dgSrcItemList.Rows.Clear();
            string keyWord = TxtSearchText.Text.Trim().ToUpper();

            GetCategoryAPI(TxtTarCountry.Text, keyWord);
            Cursor.Current = Cursors.Default;
        }

        private void TxtSearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnSearch_Click(null, null);
            }
        }
        private void GetCategoryAPI(string tarCountry, string keyWord)
        {
            Cursor.Current = Cursors.WaitCursor;
            dgSrcItemList.Rows.Clear();

            string endPoint = "https://shopeecategory.azurewebsites.net/api/ShopeeCategory?CountryCode=" + tarCountry + "&CategorySearch=" + keyWord;
            var request = new RestRequest("", RestSharp.Method.GET);
            request.Method = Method.GET;
            var client = new RestClient(endPoint);
            IRestResponse response = client.Execute(request);
            List<APIShopeeCategory> lstShopeeCategory = new List<APIShopeeCategory>();
            var result = JsonConvert.DeserializeObject<List<APIShopeeCategory>>(response.Content);

            for (int i = 0; i < result.Count; i++)
            {
                dgSrcItemList.Rows.Add(i + 1,
                    result[i].Category1Name?.ToString() ?? "",
                    result[i].Category2Name?.ToString() ?? "",
                    result[i].Category3Name?.ToString() ?? "",
                    result[i].LastCategoryId.ToString());
            }

            dgSrcItemList.ClearSelection();

            Cursor.Current = Cursors.Default;
        }

        private void GetTarAttributeDataAPI(int categoryID, bool isTranslated, string tarCountry)
        {
            DgTarAttribute.Rows.Clear();

            string endPoint = "https://shopeecategory.azurewebsites.net/api/ShopeeAttribute?CategoryId=" + categoryID + "&CountryCode=" + tarCountry;
            var request = new RestRequest("", RestSharp.Method.GET);
            request.Method = Method.GET;
            var client = new RestClient(endPoint);
            IRestResponse response = client.Execute(request);
            List<APIShopeeCategory> lstShopeeCategory = new List<APIShopeeCategory>();
            var result = JsonConvert.DeserializeObject<dynamic>(response.Content);

            for (int i = 0; i < result.Count; i++)
            {
                Dictionary<string, string> dicCombo = new Dictionary<string, string>();
                DataGridViewComboBoxCell NewComboCell = new DataGridViewComboBoxCell();
                NewComboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;

                DataGridViewComboBoxCell NewComboCell2 = new DataGridViewComboBoxCell();
                NewComboCell2.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;

                List<string> lstOptionsKor = new List<string>();
                List<string> lstOptionsSrc = new List<string>();



                if (result[i].InputType.ToString() == "COMBO_BOX" || result[i].InputType.ToString() == "DROP_DOWN")
                {
                    var options = JsonConvert.DeserializeObject<dynamic>(result[i].Options.Value.ToString());

                    for (int j = 0; j < options.Count; j++)
                    {
                        dicCombo.Add(options[j].ToString(), options[j].ToString());
                    }
                }
                else if (result[i].InputType.ToString() == "TEXT_FILED")
                {

                }
                else
                {

                }

                if (dicCombo.Count > 0)
                {
                    NewComboCell.DataSource = new BindingSource(dicCombo, null);
                    NewComboCell.DisplayMember = "Key";
                    NewComboCell.ValueMember = "Value";
                }
                DgTarAttribute.Rows.Add(i + 1,
                            result[i].AttributeName.ToString(),
                            result[i].InputType.ToString(),
                            result[i].AttributeId.ToString(),
                            result[i].AttributeType.ToString(),
                            (bool)result[i].isMandatory,
                            null,
                            false,
                            "",
                            "");
                DgTarAttribute.Rows[DgTarAttribute.Rows.Count - 1].Cells[6] = NewComboCell;
                DgTarAttribute.Rows[DgTarAttribute.Rows.Count - 1].Cells[7] = NewComboCell2;
            }


            //수정 필요

            string brandName = txtSrcTitle.Text.Trim();

            var pattern = @"\[(.*?)\]";
            var matches = Regex.Matches(brandName, pattern);

            foreach (Match m in matches)
            {
                brandName = m.Groups[1].ToString();
            }

            if (brandName != string.Empty)
            {
                for (int i = 0; i < DgSrcAttribute.Rows.Count; i++)
                {
                    if (DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_name"].Value.ToString() == "브랜드" ||
                            DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_name"].Value.ToString() == "상표" ||
                            DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_name"].Value.ToString() == "Brand")
                    {
                        DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_value"].Value = brandName;
                    }
                }
            }
        }
        private void dgSrcItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string strCategoryId = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();
            TxtSrcCategoryId.Text = strCategoryId;

            GetTarAttributeDataAPI(Convert.ToInt32(strCategoryId), true, tarCountry);            

            GetTarAttributeValue();
            
            Cursor.Current = Cursors.Default;
        }

        private void GetTarAttributeData(int categoryID, bool isTranslated)
        {
            validateAttributeList();
            DgTarAttribute.Rows.Clear();

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

                        DataGridViewComboBoxCell NewComboCell2 = new DataGridViewComboBoxCell();
                        NewComboCell2.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;

                        List<string> lstOptionsKor = new List<string>();
                        List<string> lstOptionsSrc = new List<string>();

                        lstOptionsKor = savedAttrList[i].options_kor.ToString().Split('^').ToList();
                        lstOptionsSrc = savedAttrList[i].options.ToString().Split('^').ToList();


                        Dictionary<string, string> dicCombo = new Dictionary<string, string>();
                        Dictionary<string, string> dicCombo2 = new Dictionary<string, string>();


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

                        for (int j = 0; j < lstOptionsSrc.Count; j++)
                        {
                            try
                            {
                                
                                dicCombo2.Add(lstOptionsKor[j], lstOptionsSrc[j]);
                            }
                            catch
                            {

                            }
                        }

                        


                        if (dicCombo.Count > 0)
                        {
                            NewComboCell.DataSource = new BindingSource(dicCombo, null);
                            NewComboCell.DisplayMember = "Key";
                            NewComboCell.ValueMember = "Value";

                            NewComboCell2.DataSource = new BindingSource(dicCombo2, null);
                            NewComboCell2.DisplayMember = "Key";
                            NewComboCell2.ValueMember = "Value";
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

                        DgTarAttribute.Rows.Add(i + 1,
                            attributeName,
                            savedAttrList[i].input_type.ToString(),
                            savedAttrList[i].attribute_id.ToString(),
                            savedAttrList[i].attribute_type.ToString(),
                            (bool)savedAttrList[i].is_mandatory,
                            null,
                            null,
                            false,
                            "",
                            "");

                        DgTarAttribute.Rows[DgTarAttribute.Rows.Count - 1].Cells[6] = NewComboCell;
                        DgTarAttribute.Rows[DgTarAttribute.Rows.Count - 1].Cells[7] = NewComboCell2;
                    }

                    DgTarAttribute.Sort(DgTarAttribute.Columns[5], ListSortDirection.Descending);
                }
            }
        }

        private void BtnTranslate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string src_lang = string.Empty;
            string tar_lang = string.Empty;

            if (srcCountry == "ID")
            {
                src_lang = "id";
            }
            else if (srcCountry == "SG")
            {
                src_lang = "en";
            }
            else if (srcCountry == "MY")
            {
                src_lang = "ms";
            }
            else if (srcCountry == "TH")
            {
                src_lang = "th";
            }
            else if (srcCountry == "TW")
            {
                src_lang = "zh-TW";
            }
            else if (srcCountry == "PH")
            {
                src_lang = "en";
            }
            else if (srcCountry == "VN")
            {
                src_lang = "vi";
            }


            if (tarCountry == "ID")
            {
                tar_lang = "id";
            }
            else if (tarCountry == "SG")
            {
                tar_lang = "en";
            }
            else if (tarCountry == "MY")
            {
                tar_lang = "ms";
            }
            else if (tarCountry == "TH")
            {
                tar_lang = "th";
            }
            else if (tarCountry == "TW")
            {
                tar_lang = "zh-TW";
            }
            else if (tarCountry == "PH")
            {
                tar_lang = "en";
            }
            else if (tarCountry == "VN")
            {
                tar_lang = "vi";
            }



            string translate_lang = "ko";
            //원본 번역
            for (int i = 0; i < DgSrcAttribute.Rows.Count; i++)
            {
                string translated = string.Empty;
                translated = translate(
                    DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_name"].Value.ToString(),
                    src_lang,
                    translate_lang).Trim();

                DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_name"].Value = translated;

                if (!translated.Contains("브랜드") && !translated.Contains("상표"))
                {
                    string translated2 = "";
                    DataGridViewComboBoxCell lstCombo = (DataGridViewComboBoxCell)DgSrcAttribute.Rows[i].Cells[6];
                    DataGridViewComboBoxCell NewComboCell = new DataGridViewComboBoxCell();

                    Dictionary<string, string> dicCombo = new Dictionary<string, string>();

                    for (int j = 0; j < lstCombo.Items.Count; j++)
                    {
                        translated2 = translate(
                        lstCombo.Items[j].ToString(),
                        src_lang,
                        translate_lang).Trim();
                        dicCombo.Add(translated2, lstCombo.Items[j].ToString());
                    }
                    NewComboCell.DataSource = new BindingSource(dicCombo, null);
                    NewComboCell.DisplayMember = "Key";
                    NewComboCell.ValueMember = "Value";
                    //DgSrcAttribute.Rows[i].Cells[6] = NewComboCell;
                }
            }

            for (int i = 0; i < DgTarAttribute.Rows.Count; i++)
            {
                string translated = string.Empty;
                translated = translate(
                    DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_name"].Value.ToString(),
                    tar_lang,
                    translate_lang).Trim();

                DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_name"].Value = translated;

                if (!translated.Contains("브랜드") && !translated.Contains("상표"))
                {
                    string translated2 = "";
                    DataGridViewComboBoxCell lstCombo = (DataGridViewComboBoxCell)DgTarAttribute.Rows[i].Cells[6];
                    DataGridViewComboBoxCell NewComboCell = new DataGridViewComboBoxCell();

                    Dictionary<string, string> dicCombo = new Dictionary<string, string>();

                    for (int j = 0; j < lstCombo.Items.Count; j++)
                    {
                        translated2 = translate(
                        lstCombo.Items[j].ToString(),
                        tar_lang,
                        translate_lang).Trim();
                        try
                        {
                            dicCombo.Add(translated2, lstCombo.Items[j].ToString());
                        }
                        catch
                        {

                        }
                    }

                    if (dicCombo.Count > 0)
                    {
                        NewComboCell.DataSource = new BindingSource(dicCombo, null);
                        NewComboCell.DisplayMember = "Key";
                        NewComboCell.ValueMember = "Value";

                        DgTarAttribute.Rows[i].Cells[6] = NewComboCell;
                    }
                }
            }


            Cursor.Current = Cursors.Default;

            MessageBox.Show("번역을 완료 하였습니다.", "번역 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    { "key", "AIzaSyCJ3pAmvUU0d0L-lq7QlKp_aAfcchCPzRY" },
                    { "source", src_lang },
                    { "target", tar_lang},
                    { "q", src_str }
                };
                    string GoogleTranslateApiUrl = "https://www.googleapis.com/language/translate/v2";
                    try
                    {
                        var responseBytes = webClient.UploadValues(GoogleTranslateApiUrl, "POST", data);
                        var json = Encoding.UTF8.GetString(responseBytes);
                        //var result = JsonConvert.DeserializeObject<dynamic>(json);
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

        private void DgTarAttribute_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if ((DgTarAttribute.CurrentCell.ColumnIndex == 6 || DgTarAttribute.CurrentCell.ColumnIndex == 7
                                                             || DgTarAttribute.CurrentCell.ColumnIndex == 9) && e.Control is ComboBox)
            {
                ComboBox comboBox = e.Control as ComboBox;
                comboBox.SelectedIndexChanged -= LastColumnComboSelectionChanged;
                comboBox.SelectedIndexChanged += LastColumnComboSelectionChanged;
            }
        }

        private void LastColumnComboSelectionChanged(object sender, EventArgs e)
        {
            var currentcell = DgTarAttribute.CurrentCellAddress;
            var sendingCB = sender as DataGridViewComboBoxEditingControl;
            if (sendingCB.SelectedItem != null)
            {
                var typeName = sendingCB.SelectedItem.GetType().Name.ToString();
                if (typeName.Contains("KeyValuePair"))
                {
                    KeyValuePair<string, string> cboInfo = (KeyValuePair<string, string>)sendingCB.SelectedItem;
                    DgTarAttribute.Rows[currentcell.Y].Cells["DgTarAttribute_attribute_value"].Value = cboInfo.Value.ToString();
                }
                else
                {
                    DgTarAttribute.Rows[currentcell.Y].Cells["DgTarAttribute_attribute_value"].Value = sendingCB.SelectedItem.ToString();
                }

            }

            saveAttributeData();
        }

        private void BtnViewOriginal_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string strCategoryId = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();
            GetTarAttributeData(Convert.ToInt32(strCategoryId), false);

            GetTarAttributeValue();
            Cursor.Current = Cursors.Default;
        }

        private void saveCategory(long ItemId, int categoryId)
        {
            if (ItemId != 0 && categoryId != 0)
            {
                if (ItemId != 0 && categoryId != 0)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemInfoDraft result = context.ItemInfoDrafts.SingleOrDefault(
                                b => b.src_item_id == ItemId && b.tar_shopeeAccount == tarShopeeId
                                && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            result.category_id_tar = categoryId;
                            result.isChanged = true;
                            context.SaveChanges();

                            fp.TempCategoryId = categoryId.ToString();
                        }
                    }
                }
            }
        }
        private void BtnSetCategory_Click(object sender, EventArgs e)
        {
            int selectedCategoryId = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString());

            string pervCategoryId = TxtTarCategoryId.Text.Trim();            
            DialogResult Result = MessageBox.Show("기존 카테고리 번호 : " + 
                pervCategoryId + "에서 신규 카테고리 번호 : " + selectedCategoryId.ToString() + "로 변경 하시겠습니까?", "카테고리정보 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                long ItemId = Convert.ToInt64(srcItemId);
                
                TxtTarCategoryId.Text = selectedCategoryId.ToString();
                saveCategory(ItemId, selectedCategoryId);
                fp.isChanged = true;

                MessageBox.Show("저장하였습니다.", "저장완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void saveAttributeData()
        {
            //속성값을 새로 저장하면 기존 데이터는 완전히 지우고 새로 저장한다.
            long ItemId = srcItemId;
            //TxtSrcCategoryId.Text = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();
            int newCategoryId = Convert.ToInt32(TxtSrcCategoryId.Text.Trim());

            //대상과 연결된 상품의 번호일치하는 속성을 모두 삭제한다.
            using (AppDbContext context = new AppDbContext())
            {
                List<ItemAttributeDraftTar> attList = context.ItemAttributeDraftTars
                    .Where(b => b.src_item_id == ItemId && b.tar_shopeeAccount == tarShopeeId
                    && b.UserId == global_var.userId)
                    .OrderBy(x => x.attribute_id).ToList();

                if (attList != null)
                {
                    context.ItemAttributeDraftTars.RemoveRange(attList);
                    context.SaveChanges();
                }
            }

            saveCategory(ItemId, newCategoryId);

            for (int i = 0; i < DgTarAttribute.Rows.Count; i++)
            {
                int tarAttributeId = Convert.ToInt32(DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_id"].Value.ToString());
                string tarAttributeName = DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_name"].Value.ToString();
                string tarAttributeValue = "";

                bool isMandatory = (bool)DgTarAttribute.Rows[i].Cells[5].Value;
                if (DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_value"].Value == null)
                {
                    tarAttributeValue = "";
                }
                else
                {
                    tarAttributeValue = DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_value"].Value.ToString().Trim();
                }

                string attributeType = DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_type"].Value.ToString().Trim();

                if (tarAttributeValue != string.Empty)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemAttributeDraftTar newData = new ItemAttributeDraftTar
                        {
                            attribute_id = tarAttributeId,
                            attribute_name = tarAttributeName,
                            is_mandatory = isMandatory,
                            attribute_type = attributeType,
                            attribute_value = tarAttributeValue,
                            src_item_id = ItemId,
                            tar_shopeeAccount = tarShopeeId,
                            ItemInfoDraftId = ItemInfoDraftId,
                            UserId = global_var.userId
                        };
                        context.ItemAttributeDraftTars.Add(newData);
                        context.SaveChanges();
                    }
                }
            }

            using (AppDbContext context = new AppDbContext())
            {
                ItemInfo result = context.ItemInfoes.SingleOrDefault(
                        b => b.item_id == ItemId
                        && b.UserId == global_var.userId);

                if (result != null)
                {
                    result.isChanged = true;
                    context.SaveChanges();
                }
            }
            fp.isChanged = true;

            //MessageBox.Show("저장완료","저장완료",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void saveAttributeDataFav()
        {
            //속성값을 새로 저장하면 기존 데이터는 완전히 지우고 새로 저장한다.
            long ItemId = srcItemId;
            TxtSrcCategoryId.Text = dgCategoryList_fav.SelectedRows[0].Cells["dgCategoryList_fav_cat3_id"].Value.ToString();
            int newCategoryId = Convert.ToInt32(dgCategoryList_fav.SelectedRows[0].Cells["dgCategoryList_fav_cat3_id"].Value.ToString());

            //대상과 연결된 상품의 번호일치하는 속성을 모두 삭제한다.
            using (AppDbContext context = new AppDbContext())
            {
                List<ItemAttributeDraftTar> attList = context.ItemAttributeDraftTars
                    .Where(b => b.src_item_id == ItemId
                    && b.tar_shopeeAccount == tarShopeeId
                    && b.UserId == global_var.userId)
                    .OrderBy(x => x.attribute_id).ToList();

                if (attList != null)
                {
                    context.ItemAttributeDraftTars.RemoveRange(attList);
                    context.SaveChanges();
                }
            }

            saveCategory(ItemId, newCategoryId);

            for (int i = 0; i < DgTarAttribute.Rows.Count; i++)
            {
                int tarAttributeId = Convert.ToInt32(DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_id"].Value.ToString());
                string tarAttributeName = DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_name"].Value.ToString();
                string tarAttributeValue = "";

                bool isMandatory = (bool)DgTarAttribute.Rows[i].Cells[5].Value;
                if (DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_value"].Value == null)
                {
                    tarAttributeValue = "";
                }
                else
                {
                    tarAttributeValue = DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_value"].Value.ToString().Trim();
                }

                string attributeType = DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_type"].Value.ToString().Trim();

                if (tarAttributeValue != string.Empty)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemAttributeDraftTar newData = new ItemAttributeDraftTar
                        {
                            attribute_id = tarAttributeId,
                            attribute_name = tarAttributeName,
                            is_mandatory = isMandatory,
                            attribute_type = attributeType,
                            attribute_value = tarAttributeValue,
                            src_item_id = ItemId,
                            tar_shopeeAccount = tarShopeeId,
                            ItemInfoDraftId = ItemInfoDraftId,
                            UserId = global_var.userId
                        };
                        context.ItemAttributeDraftTars.Add(newData);
                        context.SaveChanges();
                    }
                }
            }

            using (AppDbContext context = new AppDbContext())
            {
                ItemInfo result = context.ItemInfoes.SingleOrDefault(
                        b => b.item_id == ItemId
                        && b.UserId == global_var.userId);

                if (result != null)
                {
                    result.isChanged = true;
                    context.SaveChanges();
                }
            }
            fp.isChanged = true;
        }
        private void BtnSaveAttribute_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("카테고리 정보를 업데이트 하고 신규로 작성한 속성값을 저장 하시겠습니까?", "속성값 신규저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                saveAttributeData();
                MessageBox.Show("저장하였습니다.", "저장완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            //닫기 전에 필수 입력값이 빠진것이 있는지 검증해 준다.
            bool Validate = true;
            for (int i = 0; i < DgTarAttribute.Rows.Count; i++)
            {
                if((bool)DgTarAttribute.Rows[i].Cells["DgTarAttribute_is_mandatory"].Value)
                {
                    if(DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_value"].Value == null ||
                        DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_value"].Value.ToString().Trim() == string.Empty)
                    {
                        Validate = false;
                        break;
                    }
                }
            }

            Validate = true;
            if (Validate)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("입력되지 않은 필수 데이터가 있습니다.", "미입력 필수값", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void DgSrcAttribute_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 1 || e.ColumnIndex == 3 || e.ColumnIndex == 8)
                {
                    if (DgSrcAttribute.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim() != string.Empty)
                    {
                        Clipboard.SetText(DgSrcAttribute.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim());
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

        private void validateAttributeList()
        {
            using (AppDbContext context = new AppDbContext())
            {
                var lst = context.AllAttributeLists.FirstOrDefault();
                if(lst == null)
                {
                    MessageBox.Show("속성 목록을 업로드 하여 주세요","속성 목록 누락",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    FormAttributeManage fa = new FormAttributeManage();
                    fa.ShowDialog();
                }
                
            }
        }
        private void GetCategoryAPI(string tarCountry)
        {
            Cursor.Current = Cursors.WaitCursor;
            dgSrcItemList.Rows.Clear();

            string endPoint = "https://shopeecategory.azurewebsites.net/api/ShopeeCategory?CountryCode=" + tarCountry;
            var request = new RestRequest("", RestSharp.Method.GET);
            request.Method = Method.GET;
            var client = new RestClient(endPoint);
            IRestResponse response = client.Execute(request);
            List<APIShopeeCategory> lstShopeeCategory = new List<APIShopeeCategory>();
            var result = JsonConvert.DeserializeObject<List<APIShopeeCategory>>(response.Content);

            for (int i = 0; i < result.Count; i++)
            {
                dgSrcItemList.Rows.Add(i + 1,
                    result[i].Category1Name?.ToString() ?? "",
                    result[i].Category2Name?.ToString() ?? "",
                    result[i].Category3Name?.ToString() ?? "",
                    result[i].LastCategoryId.ToString());
            }

            //dgSrcItemList.ClearSelection();

            for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
            {
                if (dgSrcItemList.Rows[i].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString() == tarCategory.ToString())
                {
                    dgSrcItemList.Rows[i].Selected = true;
                    dgSrcItemList.FirstDisplayedScrollingRowIndex = i;
                    break;
                }
            }

            Cursor.Current = Cursors.Default;
        }
        private void FormSetCategoryAndAttributeDraft_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            set_double_buffer();
            TxtSrcCountry.Text = srcCountry;
            TxtTarCountry.Text = tarCountry;
            TxtSrcCategoryId.Text = srcCategory;
            TxtTarCategoryId.Text = tarCategory;
            GetCategoryAPI(tarCountry);
            GridInit();
            getFavCategoryList(tarShopeeId);
            getFavKeywordList();
            getFavKeywordOtherList();
            //validateAttributeList();
            GetSrcAttributeData2(Convert.ToInt32(TxtSrcCategoryId.Text.Trim()));

            if (tarCategory == "0")
            {
                MessageBox.Show("일치하는 카테고리가 없어 모든 카테고리를 로드하였습니다.\r\n카테고리를 설정하여 주세요.",
                    "카테고리 자동 설정 불가", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(0, dgSrcItemList.SelectedRows[0].Index);
                dgSrcItemList_CellClick(null, et);

                GetTarAttributeValue();
            }
            
            Cursor.Current = Cursors.Default;
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
        private void GetTarAttributeValue()
        {
            using (AppDbContext context = new AppDbContext())
            {
                List<ItemAttributeDraftTar> attributeList = context.ItemAttributeDraftTars
                                .Where(b => b.src_item_id == srcItemId &&
                                b.tar_shopeeAccount == tarShopeeId
                                && b.UserId == global_var.userId)
                                .OrderByDescending(x => x.is_mandatory).ToList();

                //List<ItemAttributeDraftTar> attributeList = context.ItemAttributeDraftTars
                //                .Where(b => b.src_item_id == srcItemId                               
                //                && b.UserId == global_var.userId).ToList();
                //.OrderByDescending(x => x.is_mandatory).ToList();

                if (attributeList.Count > 0)
                {
                    //이미 설정된 값이 있으면 설정된 값으로 넣어 준다.
                    //DB에서 이전에 설정된 값을 설정하는것
                    for (int i = 0; i < attributeList.Count; i++)
                    {
                        for (int j = 0; j < DgTarAttribute.Rows.Count; j++)
                        {
                            if (attributeList[i].attribute_id == 
                                Convert.ToInt64(DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_id"].Value.ToString()))
                            {
                                DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_value"].Value = attributeList[i].attribute_value;
                                break;
                            }
                        }
                    }

                    //다 넣었으면 빈 데이터에 대한 검증을 한번더 한다.
                    for (int i = 0; i < DgSrcAttribute.Rows.Count; i++)
                    {
                        //대상의 속성이름과 일치하는 원본의 이름을 가지고 온다.
                        if (DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_value"].Value.ToString().Trim() != string.Empty)
                        {
                            string srcAttrName = DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_name"].Value.ToString().Trim().ToUpper();

                            AttributeNameMap mapData = null;                            
                            //원본이 인도네시아인 경우
                            if (srcCountry == "ID")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.ID.ToUpper() == srcAttrName.ToUpper());
                            }
                            else if (srcCountry == "SG")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.SG.ToUpper() == srcAttrName.ToUpper());
                            }
                            else if (srcCountry == "MY")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.MY.ToUpper() == srcAttrName.ToUpper());
                            }
                            else if (srcCountry == "TH")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.TH.ToUpper() == srcAttrName.ToUpper());
                            }
                            else if (srcCountry == "TW")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.TW.ToUpper() == srcAttrName.ToUpper());
                            }
                            else if (srcCountry == "VN")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.VN.ToUpper() == srcAttrName.ToUpper());
                            }
                            else if (srcCountry == "PH")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.PH.ToUpper() == srcAttrName.ToUpper());
                            }

                            


                            if (mapData != null)
                            {
                                //자료가 있으면 일단 찾아본다.
                                //무조건 1개만 있어야 함
                                string tarAttrName = "";
                                if (tarCountry == "ID")
                                {
                                    tarAttrName = mapData.ID;
                                }
                                else if (tarCountry == "SG")
                                {
                                    tarAttrName = mapData.SG;
                                }
                                else if (tarCountry == "MY")
                                {
                                    tarAttrName = mapData.MY;
                                }
                                else if (tarCountry == "TH")
                                {
                                    tarAttrName = mapData.TH;
                                }
                                else if (tarCountry == "TW")
                                {
                                    tarAttrName = mapData.TW;
                                }
                                else if (tarCountry == "VN")
                                {
                                    tarAttrName = mapData.VN;
                                }
                                else if (tarCountry == "PH")
                                {
                                    tarAttrName = mapData.PH;
                                }

                                for (int j = 0; j < DgTarAttribute.Rows.Count; j++)
                                {
                                    if(DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_name_src"].Value != null)
                                    {
                                        if (DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_name_src"].Value.ToString() == tarAttrName)
                                        {
                                            if (DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_value"].Value.ToString().Trim() == string.Empty)
                                            {
                                                //이미 설정된 경우는 바꾸면 안됨
                                                //비어 있는 경우만 바꿀 수 있음
                                                DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_value"].Value =
                                                DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_value"].Value.ToString().Trim();
                                                break;
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                    //saveAttributeData();

                }
                else
                {
                    //설정된 값이 없는 상황인 경우 이름 맵핑 데이터를 참조하여 넣어준다.
                    //원본에서 브랜드가 설정되어 있으면 넣어준다.
                    for (int i = 0; i < DgSrcAttribute.Rows.Count; i++)
                    {
                        //대상의 속성이름과 일치하는 원본의 이름을 가지고 온다.

                        string srcAttrName = DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_name"].Value.ToString().ToUpper();
                        //원본국가 기준으로 맵핑 데이터를 가지고 온다.

                        AttributeNameMap mapData = null;
                        if (srcCountry == "ID")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.ID.ToUpper() == srcAttrName);
                        }
                        else if (srcCountry == "SG")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.SG.ToUpper() == srcAttrName);
                        }
                        else if (srcCountry == "MY")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.MY.ToUpper() == srcAttrName);
                        }
                        else if (srcCountry == "TH")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.TH.ToUpper() == srcAttrName);
                        }
                        else if (srcCountry == "TW")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.TW.ToUpper() == srcAttrName);
                        }
                        else if (srcCountry == "VN")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.VN.ToUpper() == srcAttrName);
                        }
                        else if (srcCountry == "PH")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.PH.ToUpper() == srcAttrName);
                        }

                        if (mapData != null)
                        {
                            //자료가 있으면 일단 찾아본다.
                            //무조건 1개만 있어야 함

                            string tarData = "";
                            if (tarCountry == "ID")
                            {
                                tarData = mapData.ID;
                            }
                            else if (tarCountry == "SG")
                            {
                                tarData = mapData.SG;
                            }
                            else if (tarCountry == "MY")
                            {
                                tarData = mapData.MY;
                            }
                            else if (tarCountry == "TH")
                            {
                                tarData = mapData.TH;
                            }
                            else if (tarCountry == "TW")
                            {
                                tarData = mapData.TW;
                            }
                            else if (tarCountry == "VN")
                            {
                                tarData = mapData.VN;
                            }
                            else if (tarCountry == "PH")
                            {
                                tarData = mapData.PH;
                            }


                            for (int j = 0; j < DgTarAttribute.Rows.Count; j++)
                            {
                                if(DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_name_src"].Value != null)
                                {
                                    if (DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_name_src"].Value.ToString() == tarData)
                                    {
                                        DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_value"].Value =
                                        DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_value"].Value.ToString();
                                        break;
                                    }
                                }
                                
                            }
                        }
                    }
                    //saveAttributeData();
                }
            }
        }

        private void GetTarAttributeValueFav()
        {
            using (AppDbContext context = new AppDbContext())
            {
                List<ItemAttributeDraftTar> attributeList = context.ItemAttributeDraftTars
                                .Where(b => b.src_item_id == srcItemId &&
                                b.tar_shopeeAccount == tarShopeeId
                                && b.UserId == global_var.userId)
                                .OrderByDescending(x => x.is_mandatory).ToList();

                if (attributeList.Count > 0)
                {
                    //이미 설정된 값이 있으면 설정된 값으로 넣어 준다.
                    //DB에서 이전에 설정된 값을 설정하는것
                    for (int i = 0; i < attributeList.Count; i++)
                    {
                        for (int j = 0; j < DgTarAttribute.Rows.Count; j++)
                        {
                            if (attributeList[i].attribute_id ==
                                Convert.ToInt64(DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_id"].Value.ToString()))
                            {
                                DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_value"].Value = attributeList[i].attribute_value;
                                break;
                            }
                        }
                    }

                    //다 넣었으면 빈 데이터에 대한 검증을 한번더 한다.
                    for (int i = 0; i < DgSrcAttribute.Rows.Count; i++)
                    {
                        //대상의 속성이름과 일치하는 원본의 이름을 가지고 온다.
                        if (DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_value"].Value.ToString().Trim() != string.Empty)
                        {
                            string srcAttrName = DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_name"].Value.ToString().Trim().ToUpper();

                            AttributeNameMap mapData = null;
                            //원본이 인도네시아인 경우
                            if (srcCountry == "ID")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.ID.ToUpper() == srcAttrName.ToUpper());
                            }
                            else if (srcCountry == "SG")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.SG.ToUpper() == srcAttrName.ToUpper());
                            }
                            else if (srcCountry == "MY")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.MY.ToUpper() == srcAttrName.ToUpper());
                            }
                            else if (srcCountry == "TH")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.TH.ToUpper() == srcAttrName.ToUpper());
                            }
                            else if (srcCountry == "TW")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.TW.ToUpper() == srcAttrName.ToUpper());
                            }
                            else if (srcCountry == "VN")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.VN.ToUpper() == srcAttrName.ToUpper());
                            }
                            else if (srcCountry == "PH")
                            {
                                mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.PH.ToUpper() == srcAttrName.ToUpper());
                            }




                            if (mapData != null)
                            {
                                //자료가 있으면 일단 찾아본다.
                                //무조건 1개만 있어야 함
                                string tarAttrName = "";
                                if (tarCountry == "ID")
                                {
                                    tarAttrName = mapData.ID;
                                }
                                else if (tarCountry == "SG")
                                {
                                    tarAttrName = mapData.SG;
                                }
                                else if (tarCountry == "MY")
                                {
                                    tarAttrName = mapData.MY;
                                }
                                else if (tarCountry == "TH")
                                {
                                    tarAttrName = mapData.TH;
                                }
                                else if (tarCountry == "TW")
                                {
                                    tarAttrName = mapData.TW;
                                }
                                else if (tarCountry == "VN")
                                {
                                    tarAttrName = mapData.VN;
                                }
                                else if (tarCountry == "PH")
                                {
                                    tarAttrName = mapData.PH;
                                }

                                for (int j = 0; j < DgTarAttribute.Rows.Count; j++)
                                {
                                    if (DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_name_src"].Value.ToString() == tarAttrName)
                                    {
                                        if (DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_value"].Value.ToString().Trim() == string.Empty)
                                        {
                                            //이미 설정된 경우는 바꾸면 안됨
                                            //비어 있는 경우만 바꿀 수 있음
                                            DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_value"].Value =
                                            DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_value"].Value.ToString().Trim();
                                            break;
                                        }

                                    }
                                }
                            }
                        }
                    }
                    saveAttributeDataFav();

                }
                else
                {
                    //설정된 값이 없는 상황인 경우 이름 맵핑 데이터를 참조하여 넣어준다.
                    //원본에서 브랜드가 설정되어 있으면 넣어준다.
                    for (int i = 0; i < DgSrcAttribute.Rows.Count; i++)
                    {
                        //대상의 속성이름과 일치하는 원본의 이름을 가지고 온다.

                        string srcAttrName = DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_name"].Value.ToString().ToUpper();
                        //원본국가 기준으로 맵핑 데이터를 가지고 온다.

                        AttributeNameMap mapData = null;
                        if (srcCountry == "ID")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.ID.ToUpper() == srcAttrName);
                        }
                        else if (srcCountry == "SG")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.SG.ToUpper() == srcAttrName);
                        }
                        else if (srcCountry == "MY")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.MY.ToUpper() == srcAttrName);
                        }
                        else if (srcCountry == "TH")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.TH.ToUpper() == srcAttrName);
                        }
                        else if (srcCountry == "TW")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.TW.ToUpper() == srcAttrName);
                        }
                        else if (srcCountry == "VN")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.VN.ToUpper() == srcAttrName);
                        }
                        else if (srcCountry == "PH")
                        {
                            mapData = context.AttributeNameMaps.SingleOrDefault
                            (b => b.PH.ToUpper() == srcAttrName);
                        }

                        if (mapData != null)
                        {
                            //자료가 있으면 일단 찾아본다.
                            //무조건 1개만 있어야 함

                            string tarData = "";
                            if (tarCountry == "ID")
                            {
                                tarData = mapData.ID;
                            }
                            else if (tarCountry == "SG")
                            {
                                tarData = mapData.SG;
                            }
                            else if (tarCountry == "MY")
                            {
                                tarData = mapData.MY;
                            }
                            else if (tarCountry == "TH")
                            {
                                tarData = mapData.TH;
                            }
                            else if (tarCountry == "TW")
                            {
                                tarData = mapData.TW;
                            }
                            else if (tarCountry == "VN")
                            {
                                tarData = mapData.VN;
                            }
                            else if (tarCountry == "PH")
                            {
                                tarData = mapData.PH;
                            }


                            for (int j = 0; j < DgTarAttribute.Rows.Count; j++)
                            {
                                if (DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_name_src"].Value.ToString() == tarData)
                                {
                                    DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_value"].Value =
                                    DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_value"].Value.ToString();
                                    break;
                                }
                            }
                        }
                    }
                    saveAttributeDataFav();
                }
            }
        }

        private void BtnManageAttribute_Click(object sender, EventArgs e)
        {
            FormAttributeManage fa = new FormAttributeManage();
            fa.ShowDialog();
        }

        private void BtnSyncAttributeSelected_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("선택한 카테고리의 속성 정보를 수집 하시겠습니까?", "속성정보 수집", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                if (dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString() != "#N/A")
                {
                    GetTarAttributeData(tarCountry,
                    Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString()),
                    partner_id, shop_id, api_key);
                }
                DataGridViewCellEventArgs et = new DataGridViewCellEventArgs(0, dgSrcItemList.SelectedRows[0].Index);
                dgSrcItemList_CellClick(null, et);
            }
            MessageBox.Show("선택한 속성정보를 수집하였습니다.", "속성정보 수집완료", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void GetTarAttributeData(string country, int categoryID, long partner_id, long shop_id, string api_key)
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

            if (result_attribute.attributes != null)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    //해당 카테고리의 데이터를 삭제하고 넣는다.
                    List<AllAttributeList> attributeList = context.AllAttributeLists
                            .Where(b => b.country == country &&
                            b.category_id == categoryID)
                            .OrderBy(b => b.attribute_id).ToList();
                    if (attributeList != null && attributeList.Count > 0)
                    {
                        context.AllAttributeLists.RemoveRange(attributeList);
                    }

                    for (int i = 0; i < result_attribute.attributes.Count; i++)
                    {
                        StringBuilder sb_options = new StringBuilder();
                        StringBuilder sb_values_original = new StringBuilder();
                        StringBuilder sb_values_translate = new StringBuilder();

                        dynamic optionVar = result_attribute.attributes[i].values;
                        for (int j = 0; j < optionVar.Count; j++)
                        {
                            sb_options.Append(optionVar[j].original_value.ToString().Trim() + "^");
                            sb_values_original.Append(optionVar[j].original_value.ToString().Trim() + "^");
                            if (optionVar[j].translate_value.ToString().Trim() == "")
                            {
                                sb_values_translate.Append(optionVar[j].original_value.ToString().Trim() + "^");
                            }
                            else
                            {
                                sb_values_translate.Append(optionVar[j].translate_value.ToString().Trim() + "^");
                            }
                        }

                        if (sb_options.Length > 0)
                        {
                            sb_options.Remove(sb_options.Length - 1, 1);
                            sb_values_original.Remove(sb_options.Length - 1, 1);
                            sb_values_translate.Remove(sb_options.Length - 1, 1);
                        }
                        AllAttributeList newList = new AllAttributeList
                        {
                            country = country,
                            category_id = categoryID,
                            attribute_id = result_attribute.attributes[i].attribute_id,
                            attribute_name = result_attribute.attributes[i].attribute_name,
                            attribute_type = result_attribute.attributes[i].attribute_type,
                            input_type = result_attribute.attributes[i].input_type,
                            options = sb_options.ToString(),
                            values_original_value = sb_values_original.ToString(),
                            values_translate_value = sb_values_translate.ToString(),
                            is_mandatory = result_attribute.attributes[i].is_mandatory
                        };

                        context.AllAttributeLists.Add(newList);
                        context.SaveChanges();
                    }
                }
            }
        }

        private void BtnViewTranslated_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string strCategoryId = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();
            GetTarAttributeData(Convert.ToInt32(strCategoryId), true);

            GetTarAttributeValue();
            Cursor.Current = Cursors.Default;
        }

        private void DgTarAttribute_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (DgTarAttribute.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null
                    && DgTarAttribute.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim() != string.Empty)
                {
                    Clipboard.SetText(DgTarAttribute.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim());
                }
            }
        }

        private void BtnFavCategorySearch_Click(object sender, EventArgs e)
        {
            string keyWord = TxtFavCategorySearchText.Text.Trim().ToUpper();
            if (keyWord != string.Empty)
            {
                Cursor.Current = Cursors.WaitCursor;
                dgCategoryList_fav.Rows.Clear();


                using (AppDbContext context = new AppDbContext())
                {
                    List<FavoriteCategoryData> favList = new List<FavoriteCategoryData>();
                    favList = context.FavoriteCategoryDatas
                        .Where(s =>
                        s.shopeeId == tarShopeeId &&
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

        private void BtnDelFavCategory_Click(object sender, EventArgs e)
        {
            if (dgCategoryList_fav.Rows.Count > 0 &&
                dgCategoryList_fav.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    string category3_id = dgCategoryList_fav.SelectedRows[0].Cells["dgCategoryList_fav_cat3_id"].Value.ToString();
                    var data = context.FavoriteCategoryDatas.FirstOrDefault(x => x.shopeeId == tarShopeeId && x.category3_id == category3_id && x.UserId == global_var.userId);

                    if (data != null)
                    {
                        context.FavoriteCategoryDatas.Remove(data);
                        context.SaveChanges();
                        dgCategoryList_fav.Rows.RemoveAt(dgCategoryList_fav.SelectedRows[0].Index);
                    }
                }
            }
        }

        private void BtnAddFavCategory_Click(object sender, EventArgs e)
        {
            if (dgSrcItemList.Rows.Count > 0 &&
                dgSrcItemList.SelectedRows.Count > 0)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    string category3_id = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();
                    var data = context.FavoriteCategoryDatas.FirstOrDefault(x => x.shopeeId == tarShopeeId && x.category3_id == category3_id && x.UserId == global_var.userId);

                    if (data == null)
                    {
                        FavoriteCategoryData fav = new FavoriteCategoryData
                        {
                            shopeeId = tarShopeeId,
                            category1_name = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat1"].Value.ToString(),
                            category2_name = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat2"].Value.ToString(),
                            category3_name = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3"].Value.ToString(),
                            category3_id = category3_id,
                            UserId = global_var.userId
                        };
                        context.FavoriteCategoryDatas.Add(fav);
                        context.SaveChanges();
                        getFavCategoryList(tarShopeeId);
                    }
                }
            }
        }

        private void dgFavKeyword_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgTarAttribute.Rows.Count > 0 &&
                DgTarAttribute.SelectedRows.Count > 0 &&
                dgFavKeyword.SelectedRows[0].Cells[0].Value != null)
            {
                DgTarAttribute.SelectedRows[0].Cells["DgTarAttribute_attribute_value"].Value = dgFavKeyword.SelectedRows[0].Cells[0].Value.ToString();
            }
        }

        private void dgFavKeyword_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                if (dgFavKeyword.Rows[e.RowIndex].Cells[0].Value != null)
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

        private void dgFavKeywordOther_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgTarAttribute.Rows.Count > 0 &&
                DgTarAttribute.SelectedRows.Count > 0 &&
                dgFavKeywordOther.SelectedRows[0].Cells[0].Value != null)
            {
                DgTarAttribute.SelectedRows[0].Cells["DgTarAttribute_attribute_value"].Value = dgFavKeywordOther.SelectedRows[0].Cells[0].Value.ToString();
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

        private void BtnManageAttribute2_Click(object sender, EventArgs e)
        {
            FormConfig fa = new FormConfig("kor");
            fa.tabMain.SelectedIndex = 3;
            fa.ShowDialog();
        }

        private void DgCategoryList_fav_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgCategoryList_fav.Rows.Count > 0 &&
                dgCategoryList_fav.SelectedRows.Count > 0      )
            {
                Cursor.Current = Cursors.WaitCursor;
                string strCategoryId = dgCategoryList_fav.SelectedRows[0].Cells["dgCategoryList_fav_cat3_id"].Value.ToString();
                GetTarAttributeData(Convert.ToInt32(strCategoryId), true);
                GetTarAttributeValueFav();

                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnSaveAttributeData_Click(object sender, EventArgs e)
        {
            saveAttributeData();
        }
    }
}
