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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormSetAttributeClone : MetroForm
    {
        public FormSetAttributeClone()
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
        private void FormSetAttribute_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            Cursor.Current = Cursors.WaitCursor;
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

            Cursor.Current = Cursors.Default;
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
        private static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
        private void GetTarAttributeData(int categoryID)
        {
            
            string partner_id = TxtTarPartnerId.Text.Trim();
            string shop_id = TxtTarShopId.Text.Trim();
            string api_key = TxtTarSecretKey.Text.Trim();

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
        private void GetSrcAttributeData(int categoryID)
        {   
            string partner_id = TxtSrcPartnerId.Text.Trim();
            string shop_id = TxtSrcShopId.Text.Trim();
            string api_key = TxtSrcSecretKey.Text.Trim();

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
                    DgSrcAttribute.Rows[DgSrcAttribute.Rows.Count-1].Cells[6] = NewComboCell;
                }

                dynamic dynAttributes = JsonConvert.DeserializeObject(TxtSrcAttributeStr.Text);
                for (int i = 0; i < dynAttributes.Count; i++)
                {
                    for (int j = 0; j < DgSrcAttribute.Rows.Count; j++)
                    {
                        if (dynAttributes[i].attribute_name == DgSrcAttribute.Rows[j].Cells["DgSrcAttribute_attribute_name"].Value.ToString())
                        {
                            DgSrcAttribute.Rows[j].Cells["DgSrcAttribute_attribute_value"].Value = dynAttributes[i].attribute_value;
                        }
                    }
                }
            }
        }

        private void DgSrcAttribute_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                if(e.ColumnIndex == 1 || e.ColumnIndex == 3 || e.ColumnIndex == 8)
                {
                    if(DgSrcAttribute.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim() != string.Empty)
                    {
                        Clipboard.SetText(DgSrcAttribute.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim());
                    }
                }
            }
        }

        private void btn_save_tarAttribute_data_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("등록할 상품의 속성 정보를 저장 하시겠습니까?", "속성정보 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                string tarShopeeAccount = TxtTarShopeeAccount.Text.Trim();
                long srcProductId = Convert.ToInt64(TxtSrcProductId.Text.Trim());

                //모든 속성값을 지운 다음 다시 저장한다.
                using (AppDbContext context = new AppDbContext())
                {
                    List<ProductAttribute> attList = context.ProductAttributes
                        .Where(b => b.tarShopeeAccount == tarShopeeAccount &&
                                   b.srcProductId == srcProductId
                                   && b.UserId == global_var.userId)
                        .OrderBy(x => x.tarAttributeId).ToList();

                    if (attList != null)
                    {
                        context.ProductAttributes.RemoveRange(attList);
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
                    

                    if(tarAttributeValue != string.Empty)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            ProductAttribute result = context.ProductAttributes.SingleOrDefault(
                                    b => b.tarShopeeAccount == tarShopeeAccount &&
                                    b.srcProductId == srcProductId &&
                                    b.tarAttributeId == tarAttributeId
                                    && b.UserId == global_var.userId);

                            if (result != null)
                            {
                                result.tarAttributeValue = tarAttributeValue;
                                result.isMandatory = isMandatory;
                                context.SaveChanges();
                            }
                            else
                            {
                                ProductAttribute newData = new ProductAttribute
                                {
                                    tarShopeeAccount = tarShopeeAccount,
                                    srcProductId = srcProductId,
                                    tarAttributeId = tarAttributeId,
                                    tarAttributeName = tarAttributeName,
                                    tarAttributeValue = tarAttributeValue,
                                    isMandatory = isMandatory,
                                    UserId = global_var.userId
                                };
                                context.ProductAttributes.Add(newData);
                                context.SaveChanges();
                            }
                        }
                    }
                }

                MessageBox.Show("저장하였습니다.","저장완료",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }   
        }

        private void FormSetAttribute_Activated(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (DgSrcAttribute.Rows.Count == 0)
            {
                GetSrcAttributeData(Convert.ToInt32(TxtSrcCategoryId.Text.Trim()));
                GetTarAttributeData(Convert.ToInt32(TxtTarCategoryId.Text.Trim()));

                //저장된 값이 있으면 가지고 온다            
                long tarProductId = Convert.ToInt64(TxtSrcProductId.Text.Trim());
                string tarShopeeAccount = TxtTarShopeeAccount.Text.Trim();
                using (AppDbContext context = new AppDbContext())
                {
                    List<ProductAttribute> attributeList = context.ProductAttributes
                        .Where(x => x.srcProductId == tarProductId
                        && x.tarShopeeAccount == tarShopeeAccount
                        && x.UserId == global_var.userId)
                        .OrderBy(x => x.tarAttributeId).ToList();

                    if (attributeList != null)
                    {
                        for (int i = 0; i < attributeList.Count; i++)
                        {
                            for (int j = 0; j < DgTarAttribute.Rows.Count; j++)
                            {
                                if (attributeList[i].tarAttributeId ==
                                    Convert.ToInt32(DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_id"].Value.ToString()))
                                {
                                    DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_value"].Value = attributeList[i].tarAttributeValue.ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                if(typeName.Contains("KeyValuePair"))
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

        private void DgTarAttribute_Sorted(object sender, EventArgs e)
        {
            for (int i = 0; i < DgTarAttribute.Rows.Count; i++)
            {
                DgTarAttribute.Rows[i].Cells["DgTarAttribute_no"].Value = i + 1;
            }
        }

        private void DgSrcAttribute_Sorted(object sender, EventArgs e)
        {
            for (int i = 0; i < DgSrcAttribute.Rows.Count; i++)
            {
                DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_no"].Value = i + 1;
            }
        }

        private void DgTarAttribute_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 1 || e.ColumnIndex == 3)
                {
                    if (DgTarAttribute.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim() != string.Empty)
                    {
                        Clipboard.SetText(DgTarAttribute.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim());
                    }
                }
            }
        }

        private void BtnTranslate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string src_lang = string.Empty;
            string tar_lang = string.Empty;

            if (TxtSrcCountry.Text == "ID")
            {
                src_lang = "id";
            }
            else if (TxtSrcCountry.Text == "SG")
            {
                src_lang = "en";
            }
            else if (TxtSrcCountry.Text == "MY")
            {
                src_lang = "ms";
            }
            else if (TxtSrcCountry.Text == "TH")
            {
                src_lang = "th";
            }
            else if (TxtSrcCountry.Text == "TW")
            {
                src_lang = "zh-TW";
            }
            else if (TxtSrcCountry.Text == "PH")
            {
                src_lang = "en";
            }
            else if (TxtSrcCountry.Text == "VN")
            {
                src_lang = "vi";
            }

            if (TxtTarCountry.Text == "ID")
            {
                tar_lang = "id";
            }
            else if (TxtTarCountry.Text == "SG")
            {
                tar_lang = "en";
            }
            else if (TxtTarCountry.Text == "MY")
            {
                tar_lang = "ms";
            }
            else if (TxtTarCountry.Text == "TH")
            {
                tar_lang = "th";
            }
            else if (TxtTarCountry.Text == "TW")
            {
                tar_lang = "zh-TW";
            }
            else if (TxtTarCountry.Text == "PH")
            {
                tar_lang = "en";
            }
            else if (TxtTarCountry.Text == "VN")
            {
                tar_lang = "vi";
            }

            string translate_lang = "ko";

            for (int i = 0; i < DgSrcAttribute.Rows.Count; i++)
            {
                string translated = string.Empty;
                translated = translate(
                    DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_name"].Value.ToString(), 
                    src_lang,
                    translate_lang).Trim();

                DgSrcAttribute.Rows[i].Cells["DgSrcAttribute_attribute_name"].Value = translated;

                if(!translated.Contains("브랜드") && !translated.Contains("상표"))
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

                    if(dicCombo.Count > 0)
                    {
                        NewComboCell.DataSource = new BindingSource(dicCombo, null);
                        NewComboCell.DisplayMember = "Key";
                        NewComboCell.ValueMember = "Value";

                        DgTarAttribute.Rows[i].Cells[6] = NewComboCell;
                    }
                }
            }


            Cursor.Current = Cursors.Default;

            MessageBox.Show("번역을 완료 하였습니다.","번역 완료",MessageBoxButtons.OK,MessageBoxIcon.Information);

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

        private void BtnViewOriginal_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DgSrcAttribute.Rows.Clear();
            DgTarAttribute.Rows.Clear();
            GetSrcAttributeData(Convert.ToInt32(TxtSrcCategoryId.Text.Trim()));
            GetTarAttributeData(Convert.ToInt32(TxtTarCategoryId.Text.Trim()));

            //저장된 값이 있으면 가지고 온다            
            long tarProductId = Convert.ToInt64(TxtSrcProductId.Text.Trim());
            string tarShopeeAccount = TxtTarShopeeAccount.Text.Trim();
            using (AppDbContext context = new AppDbContext())
            {
                List<ProductAttribute> attributeList = context.ProductAttributes
                    .Where(x => x.srcProductId == tarProductId
                    && x.tarShopeeAccount == tarShopeeAccount
                    && x.UserId == global_var.userId)
                    .OrderBy(x => x.tarAttributeId).ToList();

                if (attributeList != null)
                {
                    for (int i = 0; i < attributeList.Count; i++)
                    {
                        for (int j = 0; j < DgTarAttribute.Rows.Count; j++)
                        {
                            if (attributeList[i].tarAttributeId ==
                                Convert.ToInt32(DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_id"].Value.ToString()))
                            {
                                DgTarAttribute.Rows[j].Cells["DgTarAttribute_attribute_value"].Value = attributeList[i].tarAttributeValue.ToString();
                                break;
                            }
                        }
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }
    }
}
