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
    public partial class FormSetCategoryAndAttribute : MetroForm
    {
        public FormProductManage fp;
        public FormSetCategoryAndAttribute()
        {
            InitializeComponent();
        }
        public long partner_id;
        public long shop_id;
        public string api_key;
        private bool isLoaded = false;
        public string country = "";
        public long categoryId;
        public long itemId;

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
        private void GetSrcAttributeData(int categoryID, bool isKor)
        {
            Cursor.Current = Cursors.WaitCursor;
            DgSrcAttribute.Rows.Clear();

            using (AppDbContext context = new AppDbContext())
            {
                List<ItemAttribute> attributeList = context.ItemAttributes
                                .Where(b => b.item_id == itemId)
                                .OrderByDescending(s => s.is_mandatory)
                                .ThenBy(s => s.attribute_name)
                                .ToList();

                if (attributeList.Count > 0)
                {
                    for (int i = 0; i < attributeList.Count; i++)
                    {
                        DataGridViewComboBoxCell NewComboCell = new DataGridViewComboBoxCell();
                        NewComboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                        dynamic option = null;
                        if (option != null)
                        {
                            for (int j = 0; j < option.Count; j++)
                            {
                                NewComboCell.Items.Add(option[j].original_value.ToString());
                            }
                        }

                        if (isKor)
                        {
                            DgSrcAttribute.Rows.Add(i + 1,
                            getAttributeNameKor(attributeList[i].attribute_id),
                            attributeList[i].attribute_type,
                            attributeList[i].attribute_id.ToString(),
                            attributeList[i].attribute_type.ToString(),
                            (bool)attributeList[i].is_mandatory,
                            null,
                            false,
                            attributeList[i].attribute_value);
                        }
                        else
                        {
                            DgSrcAttribute.Rows.Add(i + 1,
                            attributeList[i].attribute_name.ToString(),
                            attributeList[i].attribute_type,
                            attributeList[i].attribute_id.ToString(),
                            attributeList[i].attribute_type.ToString(),
                            (bool)attributeList[i].is_mandatory,
                            null,
                            false,
                            attributeList[i].attribute_value);
                        }

                        DgSrcAttribute.Rows[DgSrcAttribute.Rows.Count - 1].Cells[6] = NewComboCell;
                    }
                }
            }
            isLoaded = true;
        }
        private void FormSetCategory_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            Cursor.Current = Cursors.WaitCursor;
            TxtSrcCategoryId.Text = categoryId.ToString();
            GetCategoryAPI(country);

            for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
            {
                if (dgSrcItemList.Rows[i].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString() == TxtSrcCategoryId.Text.Trim())
                {
                    dgSrcItemList.Rows[i].Selected = true;
                    dgSrcItemList.FirstDisplayedScrollingRowIndex = i;
                    break;
                }
            }
            GetAttribute();
            Application.DoEvents();
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

            dgSrcItemList.ClearSelection();

            Cursor.Current = Cursors.Default;
        }
        private string getAttributeNameKor(long attributeId)
        {
            string nameKor = "";
            using (AppDbContext context = new AppDbContext())
            {
                var result = context.AllAttributeLists.FirstOrDefault(x => x.attribute_id == attributeId);
                if(context != null)
                {
                    nameKor = result.attribute_name_kor;
                }

            }

            return nameKor;
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
                    long ItemId = Convert.ToInt64(TxtItemId.Text.ToString());
                    List<ItemAttribute> attributeList = context.ItemAttributes
                                    .Where(b => b.item_id == ItemId
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
                            //DgSrcAttribute.Rows.Add(i + 1,
                            //    attributeList[i].attribute_name,
                            //    attributeList[i].attribute_type,
                            //    attributeList[i].attribute_id,
                            //    attributeList[i].attribute_value);
                        }
                    }
                }

                isLoaded = true;
                //dynamic dynAttributes = JsonConvert.DeserializeObject(TxtSrcAttributeStr.Text);
                //for (int i = 0; i < dynAttributes.Count; i++)
                //{
                //    for (int j = 0; j < DgSrcAttribute.Rows.Count; j++)
                //    {
                //        if (dynAttributes[i].attribute_name == DgSrcAttribute.Rows[j].Cells["DgSrcAttribute_attribute_name"].Value.ToString())
                //        {
                //            DgSrcAttribute.Rows[j].Cells["DgSrcAttribute_attribute_value"].Value = dynAttributes[i].attribute_value;
                //        }
                //    }
                //}
            }
        }
        private void GetAttribute()
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
            cboTarOption.HeaderText = "옵션";
            cboTarOption.Name = "DgTarAttribute_option";

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
            DgTarAttribute.Columns.Add("DgTarAttribute_is_complete", "완료여부");
            DgTarAttribute.Columns.Add("DgTarAttribute_attribute_value", "설정된 값");

            DgTarAttribute.Columns[0].Width = 28;
            DgTarAttribute.Columns[1].Width = 250;
            DgTarAttribute.Columns[2].Width = 87;
            DgTarAttribute.Columns[3].Width = 70;
            DgTarAttribute.Columns[4].Width = 104;
            DgTarAttribute.Columns[5].Width = 36;
            DgTarAttribute.Columns[6].Width = 250;
            DgTarAttribute.Columns[7].Width = 80;
            DgTarAttribute.Columns[8].Width = 250;

            DgTarAttribute.Columns[0].ReadOnly = true;
            DgTarAttribute.Columns[1].ReadOnly = true;
            DgTarAttribute.Columns[2].ReadOnly = true;
            DgTarAttribute.Columns[3].ReadOnly = true;
            DgTarAttribute.Columns[4].ReadOnly = true;
            DgTarAttribute.Columns[5].ReadOnly = true;
            DgTarAttribute.Columns[6].ReadOnly = false;
            DgTarAttribute.Columns[7].ReadOnly = true;
            DgTarAttribute.Columns[8].ReadOnly = false;


            DgTarAttribute.Columns[2].Visible = false;
            DgTarAttribute.Columns[4].Visible = false;
            DgTarAttribute.Columns[7].Visible = false;

            DgTarAttribute.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgTarAttribute.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DgTarAttribute.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgTarAttribute.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgTarAttribute.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgTarAttribute.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgTarAttribute.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DgTarAttribute.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgTarAttribute.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void GetCategory()
        {
            Cursor.Current = Cursors.WaitCursor;
            dgSrcItemList.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                var lstCategory = context.ShopeeCategorOnlys.Where(x => x.Country == country)
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
                    if (dgSrcItemList.Rows[i].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString() == categoryId.ToString())
                    {
                        dgSrcItemList.Rows[i].Selected = true;
                        dgSrcItemList.FirstDisplayedScrollingRowIndex = i;
                        break;
                    }
                }
            }

            if(dgSrcItemList.Rows.Count == 0)
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

            GetCategoryAPI(TxtSrcCountry.Text, keyWord);
            Cursor.Current = Cursors.Default;
        }

        private void TxtSearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
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
        private void FormSetCategoryAndAttribute_Activated(object sender, EventArgs e)
        {
            if(!isLoaded)
            {
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();
                GetTarAttributeDataAPI(Convert.ToInt32(categoryId), true, country);
                GetSrcAttributeData2(Convert.ToInt32(categoryId), false);
                Cursor.Current = Cursors.Default;
            }
        }
        private void GetSrcAttributeData2(int categoryID, bool isKor)
        {
            Cursor.Current = Cursors.WaitCursor;
            DgSrcAttribute.Rows.Clear();

            using (AppDbContext context = new AppDbContext())
            {
                List<ItemAttribute> attributeList = context.ItemAttributes
                                .Where(b => b.item_id == itemId)
                                .OrderByDescending(s => s.is_mandatory)
                                .ThenBy(s => s.attribute_name)
                                .ToList();

                if (attributeList.Count > 0)
                {
                    for (int i = 0; i < attributeList.Count; i++)
                    {
                        DataGridViewComboBoxCell NewComboCell = new DataGridViewComboBoxCell();
                        NewComboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                        dynamic option = null;
                        if (option != null)
                        {
                            for (int j = 0; j < option.Count; j++)
                            {
                                NewComboCell.Items.Add(option[j].original_value.ToString());
                            }
                        }

                        if (isKor)
                        {
                            DgSrcAttribute.Rows.Add(i + 1,
                            getAttributeNameKor(attributeList[i].attribute_id),
                            attributeList[i].attribute_type,
                            attributeList[i].attribute_id.ToString(),
                            attributeList[i].attribute_type.ToString(),
                            (bool)attributeList[i].is_mandatory,
                            null,
                            false,
                            attributeList[i].attribute_value);
                        }
                        else
                        {
                            DgSrcAttribute.Rows.Add(i + 1,
                            attributeList[i].attribute_name.ToString(),
                            attributeList[i].attribute_type,
                            attributeList[i].attribute_id.ToString(),
                            attributeList[i].attribute_type.ToString(),
                            (bool)attributeList[i].is_mandatory,
                            null,
                            false,
                            attributeList[i].attribute_value);
                        }

                        DgSrcAttribute.Rows[DgSrcAttribute.Rows.Count - 1].Cells[6] = NewComboCell;
                    }
                }
            }
            isLoaded = true;
        }
        private void dgSrcItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string strCategoryId = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();
            GetTarAttributeDataAPI(Convert.ToInt32(strCategoryId), true, country);
            checkMatchData();
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
                            result[i].AttributeName.ToString());
                DgTarAttribute.Rows[DgTarAttribute.Rows.Count - 1].Cells[6] = NewComboCell;
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
        private void GetTarAttributeData(int categoryID, bool isTranslated, string tarCountry)
        {
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

                        DgTarAttribute.Rows.Add(i + 1,
                            attributeName,
                            savedAttrList[i].input_type.ToString(),
                            savedAttrList[i].attribute_id.ToString(),
                            savedAttrList[i].attribute_type.ToString(),
                            (bool)savedAttrList[i].is_mandatory,
                            null,
                            false,
                            "",
                            savedAttrList[i].attribute_name.ToString());

                        DgTarAttribute.Rows[DgTarAttribute.Rows.Count - 1].Cells[6] = NewComboCell;
                    }

                    DgTarAttribute.Sort(DgTarAttribute.Columns[5], ListSortDirection.Descending);
                }
                else
                {
                    MessageBox.Show("속성 데이터가 없습니다.\r\n속성데이터 관리에서 해당 국가의 속성 데이터를 업로드 해 주세요.", "속성 데이터 누락", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //BtnManageAttributeData_Click(null, null);
                }

                
            }
        }

        private void checkMatchData()
        {
            //if (txtSrcTitle.Text.Length > 0)
            //{

            //    string brandName = txtSrcTitle.Text;

            //    var pattern = @"\[(.*?)\]";
            //    var matches = Regex.Matches(brandName, pattern);

            //    foreach (Match m in matches)
            //    {
            //        brandName = m.Groups[1].ToString();
            //    }

            //    if (brandName != string.Empty)
            //    {
            //        for (int i = 0; i < DgTarAttribute.Rows.Count; i++)
            //        {
            //            if (DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_name"].Value.ToString() == "브랜드" ||
            //                    DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_name"].Value.ToString() == "상표" ||
            //                    DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_name"].Value.ToString() == "Brand")
            //            {
            //                DgTarAttribute.Rows[i].Cells["DgTarAttribute_attribute_value"].Value = brandName;
            //            }
            //        }
            //    }
            //}

            //원본과 대상의 언어가 다를때만 DB에서 가지고 와서 비교한다.
            bool compairDB = true;

            if(rdSrcKor.Checked && rdTarKor.Checked)
            {
                compairDB = false;
            }
            else if(rdSrcLocal.Checked && rdTarLocal.Checked)
            {
                compairDB = false;
            }

            if (compairDB)
            {
                Dictionary<string, string> dicAttrName = new Dictionary<string, string>();
                //선택된 카테고리의 속성값을 가지고 온다.
                using (AppDbContext context = new AppDbContext())
                {
                    //현재 선택된 카테고리의 속성을 모두 가지고 온다.
                    long curSelectedCategory = Convert.ToInt64(dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString());
                    List<AllAttributeList> savedAttrList = context.AllAttributeLists
                        .Where(b => b.category_id == curSelectedCategory &&
                                    b.country == country)
                        .OrderByDescending(x => x.is_mandatory)
                        .ThenBy(x => x.attribute_name)
                        .ToList();

                    for (int i = 0; i < savedAttrList.Count; i++)
                    {
                        if(rdSrcKor.Checked && rdTarLocal.Checked)
                        {
                            dicAttrName.Add(savedAttrList[i].attribute_name, savedAttrList[i].attribute_name_kor);                            
                        }
                        else if(rdSrcLocal.Checked && rdTarKor.Checked)
                        {
                            dicAttrName.Add(savedAttrList[i].attribute_name_kor, savedAttrList[i].attribute_name);
                        }
                    }


                    for (int i = 0; i < DgSrcAttribute.Rows.Count; i++)
                    {
                        string srcName = DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_name"].Value.ToString();
                        for (int j = 0; j < DgTarAttribute.Rows.Count; j++)
                        {
                            if(dicAttrName.ContainsKey(DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_name"].Value.ToString()))
                            {
                                string tarName = dicAttrName[DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_name"].Value.ToString()];
                                if (srcName == tarName)
                                {
                                    DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_value"].Value =
                                        DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_value"].Value.ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //두개의 언어가 같으므로 그냥 그리드의 필드가 같으면 된다.
                for (int i = 0; i < DgSrcAttribute.Rows.Count; i++)
                {
                    for (int j = 0; j < DgTarAttribute.Rows.Count; j++)
                    {
                        if (DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_name"].Value.ToString() ==
                            DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_name"].Value.ToString())
                        {
                            DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_value"].Value =
                                DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_value"].Value.ToString();
                            break;
                        }
                    }
                }
            }
        }

        private void GetTarAttributeData(int categoryID)
        {
            DgTarAttribute.Rows.Clear();
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
                for (int i = 0; i < result_attribute.attributes.Count; i++)
                {
                    DataGridViewComboBoxCell NewComboCell = new DataGridViewComboBoxCell();
                    NewComboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;

                    dynamic option = result_attribute.attributes[i].values;
                    for (int j = 0; j < option.Count; j++)
                    {
                        NewComboCell.Items.Add(option[j].original_value.ToString());
                    }

                    DgTarAttribute.Rows.Add(i + 1,
                        result_attribute.attributes[i].attribute_name.ToString(),
                        result_attribute.attributes[i].input_type.ToString(),
                        result_attribute.attributes[i].attribute_id.ToString(),
                        result_attribute.attributes[i].attribute_type.ToString(),
                        (bool)result_attribute.attributes[i].is_mandatory,
                        null,
                        false,
                        "");

                    DgTarAttribute.Rows[DgTarAttribute.Rows.Count - 1].Cells[6] = NewComboCell;
                }
            }
        }

        private void BtnTranslate_Click(object sender, EventArgs e)
        {
            string selectedCategoryId = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();

            GetTarAttributeData(Convert.ToInt32(selectedCategoryId), true, country);
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
            if (DgTarAttribute.CurrentCell.ColumnIndex == 6 && e.Control is ComboBox)
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
        }

        private void BtnViewOriginal_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string selectedCategoryId = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();

            GetTarAttributeData(Convert.ToInt32(selectedCategoryId), false, country);
            Cursor.Current = Cursors.Default;
        }

        private void saveCategory(long ItemId, int categoryId)
        {
            if(ItemId != 0 && categoryId != 0)
            {
                if (ItemId != 0 && categoryId != 0)
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemInfo result = context.ItemInfoes.SingleOrDefault(
                                b => b.item_id == ItemId && b.UserId == global_var.userId);

                        if (result != null)
                        {
                            result.category_id = categoryId;
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
            DialogResult Result = MessageBox.Show("카테고리 정보를 저장 하시겠습니까?", "카테고리정보 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                long ItemId = Convert.ToInt64(TxtItemId.Text.Trim());
                TxtSrcCategoryId.Text = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();
                int categoryId = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString());

                saveCategory(ItemId, categoryId);
                fp.isChanged = true;

                MessageBox.Show("저장하였습니다.","저장완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSaveAttribute_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("카테고리 정보를 업데이트 하고 신규로 작성한 속성값을 저장 하시겠습니까?", "속성값 신규저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                //속성값을 새로 저장하면 기존 데이터는 완전히 지우고 새로 저장한다.
                long ItemId = Convert.ToInt64(TxtItemId.Text.Trim());
                TxtSrcCategoryId.Text = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();
                int categoryId = Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString());

                saveCategory(ItemId, categoryId);

                TxtSrcCategoryId.Text = categoryId.ToString();

                using (AppDbContext context = new AppDbContext())
                {
                    List<ItemAttribute> attList = context.ItemAttributes
                        .Where(b => b.item_id == ItemId
                        && b.UserId == global_var.userId)
                        .OrderBy(x => x.attribute_id).ToList();

                    if (attList != null)
                    {
                        context.ItemAttributes.RemoveRange(attList);
                        context.SaveChanges();
                    }
                }

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
                            ItemAttribute newData = new ItemAttribute
                            {
                                attribute_id = tarAttributeId,
                                attribute_name = tarAttributeName,
                                is_mandatory = isMandatory,
                                attribute_type = attributeType,
                                attribute_value = tarAttributeValue,
                                item_id = ItemId,
                                UserId = global_var.userId
                            };
                            context.ItemAttributes.Add(newData);
                            context.SaveChanges();
                        }
                    }
                }

                using (AppDbContext context = new AppDbContext())
                {
                    ItemInfo result = context.ItemInfoes.SingleOrDefault(
                            b => b.item_id == ItemId && b.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.isChanged = true;
                        context.SaveChanges();
                    }
                }

                GetSrcAttributeData2(Convert.ToInt32(categoryId), false);
                fp.isChanged = true;
                MessageBox.Show("저장하였습니다.", "저장완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }   
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("기존 저장된 속성값을 저장 하시겠습니까?", "속성값 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                long ItemId = Convert.ToInt64(TxtItemId.Text.Trim());

                for (int i = 0; i < DgSrcAttribute.Rows.Count; i++)
                {
                    long attributeId = Convert.ToInt64(DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_id"].Value.ToString());
                    using (AppDbContext context = new AppDbContext())
                    {
                        ItemAttribute result = context.ItemAttributes.SingleOrDefault(
                                b => b.item_id == ItemId && 
                                b.attribute_id == attributeId
                                && b.UserId == global_var.userId);
                        string srcAttributeValue = "";
                        if (result != null)
                        {
                            if (DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_value"].Value == null)
                            {
                                srcAttributeValue = "";
                            }
                            else
                            {
                                srcAttributeValue = DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_value"].Value.ToString().Trim();
                            }

                            result.attribute_value = srcAttributeValue;
                            context.SaveChanges();
                        }
                    }
                }

                using (AppDbContext context = new AppDbContext())
                {
                    ItemInfo result = context.ItemInfoes.SingleOrDefault(
                            b => b.item_id == ItemId && b.UserId == global_var.userId);

                    if (result != null)
                    {
                        result.isChanged = true;
                        context.SaveChanges();
                    }
                }

                fp.isChanged = true;
                MessageBox.Show("속성값을 저장 하였습니다.","속성값 저장",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }   
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void Menu_Attr1_copy_cell_Click(object sender, EventArgs e)
        {
            int row = DgSrcAttribute.SelectedRows[0].Index;
            if (row > -1)
            {
                string srcAttributeValue = "";
                if (DgSrcAttribute.SelectedRows[0].Cells["DgSrcAttribute_attribute_value"].Value == null)
                {
                    srcAttributeValue = "";
                }
                else
                {
                    srcAttributeValue = DgSrcAttribute.SelectedRows[0].Cells["DgSrcAttribute_attribute_value"].Value.ToString().Trim();
                }

                if (srcAttributeValue.Trim() != string.Empty)
                {
                    Clipboard.SetText(srcAttributeValue);
                }
            }
        }

        private void DgSrcAttribute_MouseUp(object sender, MouseEventArgs e)
        {
            //// Load context menu on right mouse click
            //DataGridView.HitTestInfo hitTestInfo;
            //if (e.Button == MouseButtons.Right)
            //{
            //    hitTestInfo = DgSrcAttribute.HitTest(e.X, e.Y);
            //    // If column is first column
            //    if (hitTestInfo.Type == DataGridViewHitTestType.Cell && hitTestInfo.ColumnIndex == 0)
            //        Menu_Attr1.Show(DgSrcAttribute, new Point(e.X, e.Y));
            //    // If column is second column
            //    if (hitTestInfo.Type == DataGridViewHitTestType.Cell && hitTestInfo.ColumnIndex == 1)
            //        Menu_Attr1.Show(DgSrcAttribute, new Point(e.X, e.Y));
            //}
        }

        private void BtnViewOriginalSaved_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            GetTarAttributeDataAPI(Convert.ToInt32(categoryId), true, country);
            Cursor.Current = Cursors.Default;
        }

        private void BtnTranslateSaved_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            GetTarAttributeDataAPI(Convert.ToInt32(categoryId), true, country);
            Cursor.Current = Cursors.Default;
        }

        private void RdSrcKor_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            GetTarAttributeDataAPI(Convert.ToInt32(categoryId), true, country);
            checkMatchData();

            Cursor.Current = Cursors.Default;
        }

        private void RdSrcLocal_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            GetTarAttributeDataAPI(Convert.ToInt32(categoryId), true, country);
            checkMatchData();
            Cursor.Current = Cursors.Default;
        }

        private void RdTarKor_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string selectedCategoryId = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();

            GetTarAttributeData(Convert.ToInt32(selectedCategoryId), true, country);
            checkMatchData();
            Cursor.Current = Cursors.Default;
        }

        private void RdTarLocal_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string selectedCategoryId = dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString();

            GetTarAttributeData(Convert.ToInt32(selectedCategoryId), false, country);
            checkMatchData();
            Cursor.Current = Cursors.Default;
        }
    }
}
