using MetroFramework.Forms;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormAttributeManage : MetroForm
    {
        public FormAttributeManage()
        {
            InitializeComponent();
        }
        private void GetCategory(string tarCountry)
        {
            dgSrcItemList.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {

                if (tarCountry == "ID")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories.OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_id })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3,
                            categoryListDist[i].cat_id);
                    }
                }
                else if (tarCountry == "MY")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories.OrderBy(x => x.my_l1).ThenBy(x => x.my_l2).ThenBy(x => x.my_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_my })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3,
                            categoryListDist[i].cat_my);
                    }
                }
                else if (tarCountry == "PH")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories.OrderBy(x => x.ph_l1).ThenBy(x => x.ph_l2).ThenBy(x => x.ph_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_ph })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l1,
                            categoryListDist[i].cat_ph);
                    }
                }
                else if (tarCountry == "SG")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories.OrderBy(x => x.sg_l1).ThenBy(x => x.sg_l2).ThenBy(x => x.sg_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_sg })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l1,
                            categoryListDist[i].cat_sg);
                    }
                }
                else if (tarCountry == "TH")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories.OrderBy(x => x.th_l1).ThenBy(x => x.th_l2).ThenBy(x => x.th_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_th })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l1,
                            categoryListDist[i].cat_th);
                    }
                }
                else if (tarCountry == "TW")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories.OrderBy(x => x.tw_l1).ThenBy(x => x.tw_l2).ThenBy(x => x.tw_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_tw })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l1,
                            categoryListDist[i].cat_tw);
                    }
                }
                else if (tarCountry == "VN")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories.OrderBy(x => x.vn_l1).ThenBy(x => x.vn_l2).ThenBy(x => x.vn_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_vn })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].vn_l1, categoryListDist[i].vn_l2, categoryListDist[i].vn_l1,
                            categoryListDist[i].cat_vn);
                    }
                }
            }
        }

        private void GetAttribute(string tarCountry)
        {
            dgAttributeList.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<AllAttributeList> attributeList = new List<AllAttributeList>();
                attributeList = context.AllAttributeLists
                        .Where(x => x.country == tarCountry)
                        .OrderBy(x => x.is_mandatory).ThenBy(x => x.attribute_id).ThenBy(x => x.attribute_name).ToList();
                
                for (int i = 0; i < attributeList.Count; i++)
                {
                    dgAttributeList.Rows.Add(i + 1,
                        attributeList[i].category_id,
                        attributeList[i].attribute_id,
                        attributeList[i].attribute_name,
                        attributeList[i].attribute_name_kor,
                        attributeList[i].is_mandatory,
                        attributeList[i].attribute_type,
                        attributeList[i].input_type,
                        attributeList[i].options,
                        attributeList[i].options_kor,
                        attributeList[i].values_original_value,
                        attributeList[i].values_translate_value,
                        attributeList[i].values_translate_kor);
                }
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
        private void FormAttributeManage_Load(object sender, EventArgs e)
        {
            set_double_buffer();
            getShopeeAccount();
            getAttributeNameMap();
        }
        private void getAttributeNameMap()
        {
            DgAttributeMap.Rows.Clear();
            using (AppDbContext context = new AppDbContext())
            {
                List<AttributeNameMap> mapList = context.AttributeNameMaps.OrderBy(x => x.KR).ToList();

                if(mapList.Count > 0)
                {
                    for (int i = 0; i < mapList.Count; i++)
                    {
                        DgAttributeMap.Rows.Add(i + 1, false,
                            mapList[i].KR,
                            mapList[i].SG,
                            mapList[i].MY,
                            mapList[i].ID,
                            mapList[i].TH,
                            mapList[i].TW,
                            mapList[i].VN,
                            mapList[i].PH,
                            mapList[i].Idx);         
                    }
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

        public static void UnLockMaxParallel()
        {
            int prevThreads, prevPorts;
            ThreadPool.GetMinThreads(out prevThreads, out prevPorts);
            ThreadPool.SetMinThreads(50, prevPorts);
        }
        private void BtnSyncAttribute_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("모든 카테고리별 속성 정보를 수집 하시겠습니까?", "속성정보 수집", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();
                long partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                long shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                string api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
                string end_point = "https://partner.shopeemobile.com/api/v1/item/add";
                string shopeeId = dg_site_id.SelectedRows[0].Cells["dg_site_id_id"].Value.ToString();
                string country = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();

                CancellationTokenSource cts = new CancellationTokenSource();

                progressBar.Value = 0;
                progressBar.Maximum = dgSrcItemList.Rows.Count;

                List<int> lstCategory = new List<int>();
                for (int i = 0; i < dgSrcItemList.Rows.Count; i++)
                {
                    if (dgSrcItemList.Rows[i].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString() != "#N/A")
                    {
                        //GetTarAttributeData(country,
                        //Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString()),
                        //partner_id, shop_id, api_key);

                        lstCategory.Add(Convert.ToInt32(dgSrcItemList.Rows[i].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString()));
                    }
                }



                #region EDITED For 이미지 병렬 로드
                UnLockMaxParallel();
                #endregion
                var current_synchronization_context = TaskScheduler.FromCurrentSynchronizationContext();

                int cur_idx = 0;
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    Task.Run(() =>
                    {
                        Parallel.ForEach(lstCategory, new ParallelOptions { MaxDegreeOfParallelism = 100 }, LstRow =>
                        {
                            Cursor.Current = Cursors.WaitCursor;

                            if (cts.IsCancellationRequested) return;
                            var category_id = LstRow;
                            if (category_id != 0)
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                GetTarAttributeData(country, category_id, partner_id, shop_id, api_key);
                                cur_idx++;
                                progressBar.Invoke((MethodInvoker)delegate ()
                                {
                                    progressBar.Value = cur_idx;
                                });
                                Cursor.Current = Cursors.WaitCursor;
                            }
                            Cursor.Current = Cursors.WaitCursor;
                        });
                    }).ContinueWith(t =>
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("모든 속성정보를 수집하였습니다.", "속성정보 수집완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    });
                }
                catch (Exception ex)
                {
                    cur_idx++;
                    progressBar.Invoke((MethodInvoker)delegate ()
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        progressBar.Value = cur_idx;
                    });
                }
            }

            


            
        }
        private static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
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
                            if(optionVar[j].translate_value.ToString().Trim() == "")
                            {
                                sb_values_translate.Append(optionVar[j].original_value.ToString().Trim() + "^");
                            }
                            else
                            {
                                sb_values_translate.Append(optionVar[j].translate_value.ToString().Trim() + "^");
                            }
                        }

                        if(sb_options.Length > 0)
                        {
                            sb_options.Remove(sb_options.Length - 1, 1);
                        }

                        if (sb_values_original.Length > 0)
                        {
                            sb_values_original.Remove(sb_values_original.Length - 1, 1);
                        }

                        if (sb_values_translate.Length > 0)
                        {
                            sb_values_translate.Remove(sb_values_translate.Length - 1, 1);
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
        private void dg_site_id_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string country = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
            GetCategory(country);
            GetAttribute(country);
            Cursor.Current = Cursors.Default;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dgSrcItemList.Rows.Clear();
            string keyWord = TxtSearchText.Text.Trim().ToUpper();
            string tarCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();

            using (AppDbContext context = new AppDbContext())
            {
                if (tarCountry == "ID")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories
                        .Where(x => x.id_l1.ToString().ToUpper().Contains(keyWord) ||
                        x.id_l2.ToString().ToUpper().Contains(keyWord) ||
                        x.id_l3.ToString().ToUpper().Contains(keyWord))
                        .OrderBy(x => x.id_l1).ThenBy(x => x.id_l2).ThenBy(x => x.id_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_id })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].id_l1, categoryListDist[i].id_l2, categoryListDist[i].id_l3,
                            categoryListDist[i].cat_id);
                    }
                }
                else if (tarCountry == "MY")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories
                        .Where(x => x.my_l1.ToString().ToUpper().Contains(keyWord) ||
                        x.my_l2.ToString().ToUpper().Contains(keyWord) ||
                        x.my_l3.ToString().ToUpper().Contains(keyWord))
                        .OrderBy(x => x.my_l1).ThenBy(x => x.my_l2).ThenBy(x => x.my_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_my })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].my_l1, categoryListDist[i].my_l2, categoryListDist[i].my_l3,
                            categoryListDist[i].cat_my);
                    }
                }
                else if (tarCountry == "PH")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories
                        .Where(x => x.ph_l1.ToString().ToUpper().Contains(keyWord) ||
                        x.ph_l2.ToString().ToUpper().Contains(keyWord) ||
                        x.ph_l3.ToString().ToUpper().Contains(keyWord))
                        .OrderBy(x => x.ph_l1).ThenBy(x => x.ph_l2).ThenBy(x => x.ph_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_ph })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].ph_l1, categoryListDist[i].ph_l2, categoryListDist[i].ph_l3,
                            categoryListDist[i].cat_ph);
                    }
                }
                else if (tarCountry == "SG")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories
                        .Where(x => x.sg_l1.ToString().ToUpper().Contains(keyWord) ||
                        x.sg_l2.ToString().ToUpper().Contains(keyWord) ||
                        x.sg_l3.ToString().ToUpper().Contains(keyWord))
                        .OrderBy(x => x.sg_l1).ThenBy(x => x.sg_l2).ThenBy(x => x.sg_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_sg })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].sg_l1, categoryListDist[i].sg_l2, categoryListDist[i].sg_l3,
                            categoryListDist[i].cat_sg);
                    }
                }
                else if (tarCountry == "TH")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories
                        .Where(x => x.th_l1.ToString().ToUpper().Contains(keyWord) ||
                        x.th_l2.ToString().ToUpper().Contains(keyWord) ||
                        x.th_l3.ToString().ToUpper().Contains(keyWord))
                        .OrderBy(x => x.th_l1).ThenBy(x => x.th_l2).ThenBy(x => x.th_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_th })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].th_l1, categoryListDist[i].th_l2, categoryListDist[i].th_l3,
                            categoryListDist[i].cat_th);
                    }
                }
                else if (tarCountry == "TW")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories
                        .Where(x => x.tw_l1.ToString().ToUpper().Contains(keyWord) ||
                        x.tw_l2.ToString().ToUpper().Contains(keyWord) ||
                        x.tw_l3.ToString().ToUpper().Contains(keyWord))
                        .OrderBy(x => x.tw_l1).ThenBy(x => x.tw_l2).ThenBy(x => x.tw_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_tw })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].tw_l1, categoryListDist[i].tw_l2, categoryListDist[i].tw_l3,
                            categoryListDist[i].cat_tw);
                    }
                }
                else if (tarCountry == "VN")
                {
                    List<ShopeeCategory> categoryListDist = new List<ShopeeCategory>();
                    categoryListDist = context.ShopeeCategories
                        .Where(x => x.vn_l1.ToString().ToUpper().Contains(keyWord) ||
                        x.vn_l2.ToString().ToUpper().Contains(keyWord) ||
                        x.vn_l3.ToString().ToUpper().Contains(keyWord))
                        .OrderBy(x => x.vn_l1).ThenBy(x => x.vn_l2).ThenBy(x => x.vn_l3).ToList();

                    categoryListDist = categoryListDist.GroupBy(s => new { s.cat_vn })
                            .Select(x => x.FirstOrDefault())
                            .ToList<ShopeeCategory>();

                    for (int i = 0; i < categoryListDist.Count; i++)
                    {
                        dgSrcItemList.Rows.Add(i + 1, categoryListDist[i].vn_l1, categoryListDist[i].vn_l2, categoryListDist[i].vn_l3,
                            categoryListDist[i].cat_vn);
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void TxtSearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                BtnSearch_Click(null, null);
            }
        }

        private void BtnSyncAttributeSelected_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("선택한 카테고리의 속성 정보를 수집 하시겠습니까?", "속성정보 수집", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                long partner_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_partner_id"].Value.ToString());
                long shop_id = Convert.ToInt64(dg_site_id.SelectedRows[0].Cells["dg_site_id_shop_id"].Value.ToString());
                string api_key = dg_site_id.SelectedRows[0].Cells["dg_site_id_secret_key"].Value.ToString();
                string country = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();

                if (dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString() != "#N/A")
                {
                    GetTarAttributeData(country,
                    Convert.ToInt32(dgSrcItemList.SelectedRows[0].Cells["dgSrcItemList_Src_cat3_id"].Value.ToString()),
                    partner_id, shop_id, api_key);
                }
            }
            MessageBox.Show("선택한 속성정보를 수집하였습니다.", "속성정보 수집완료", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void BtnTranslateAttributeName_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("선택한 계정의 속성명을 번역 하시겠습니까?", "속성명 번역", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                UnLockMaxParallel();

                //우선 번역해야 할 단어중에 중복이 많으므로 추린다.
                Dictionary<string, string> dicSrc = new Dictionary<string, string>();
                for (int i = 0; i < dgAttributeList.Rows.Count; i++)
                {
                    string txt = dgAttributeList.Rows[i].Cells["dgAttributeList_attribute_name"].Value.ToString().Trim();
                    if (!dicSrc.ContainsKey(txt))
                    {
                        dicSrc.Add(txt, "");
                    }

                }

                List<string> lstList = new List<string>();
                lstList = dicSrc.Keys.ToList();
                CancellationTokenSource cts = new CancellationTokenSource();
                var current_synchronization_context = TaskScheduler.FromCurrentSynchronizationContext();
                string srcCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
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

                string translate_lang = "ko";
                progressBar.Value = 0;
                progressBar.Maximum = dgAttributeList.Rows.Count;

                int cur_idx = 0;
                try
                {
                    Task.Run(() =>
                    {
                        Parallel.ForEach(lstList,
                            new ParallelOptions { MaxDegreeOfParallelism = 20 }
                            , dgRow =>
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                try
                                {
                                    if (cts.IsCancellationRequested) return;

                                    //var ic = dgRow.Cells["dg_dep_option_eng"] as DataGridViewTextBoxCell;

                                    Cursor.Current = Cursors.WaitCursor;
                                    string src_title = dgRow;
                                    if (src_title != string.Empty)
                                    {
                                        string translated = string.Empty;
                                        translated = translate(
                                        src_title,
                                        src_lang,
                                        translate_lang).Trim();

                                        dicSrc[src_title] = translated;

                                        cur_idx++;
                                        progressBar.Invoke((MethodInvoker)delegate ()
                                        {
                                            progressBar.Value = cur_idx;
                                        });
                                        Cursor.Current = Cursors.WaitCursor;
                                    }
                                    Thread.Sleep(100);
                                    Cursor.Current = Cursors.WaitCursor;
                                }
                                catch
                                {

                                }
                            });
                    }).ContinueWith(t =>
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        for (int i = 0; i < dgAttributeList.Rows.Count; i++)
                        {
                            string srcText = dgAttributeList.Rows[i].Cells["dgAttributeList_attribute_name"].Value.ToString();
                            dgAttributeList.Rows[i].Cells["dgAttributeList_attribute_name_kor"].Value = dicSrc[srcText];

                            long category_id = Convert.ToInt64(dgAttributeList.Rows[i].Cells["dgAttributeList_category_id"].Value.ToString());
                            long attributeId = Convert.ToInt64(dgAttributeList.Rows[i].Cells["dgAttributeList_attribute_id"].Value.ToString());

                            //저장한다.
                            using (AppDbContext context = new AppDbContext())
                            {
                                AllAttributeList result = context.AllAttributeLists.SingleOrDefault(
                                        b => b.country == srcCountry &&
                                        b.category_id == category_id &&
                                        b.attribute_id == attributeId);

                                if (result != null)
                                {
                                    result.attribute_name_kor = dicSrc[srcText];
                                    context.SaveChanges();
                                }
                            }
                        }
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("속성명 번역을 완료 하였습니다.", "속성명 번역", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    });
                }
                catch
                {
                    // ignore
                }


                

                Cursor.Current = Cursors.Default;
                
            }
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

        private void BtnTranslateOption_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("선택한 계정의 옵션을 번역 하시겠습니까?", "옵션 번역", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                UnLockMaxParallel();

                //우선 번역해야 할 단어중에 중복이 많으므로 추린다.
                Dictionary<string, string> dicSrc = new Dictionary<string, string>();
                //브랜드는 번역하지 않는다.
                for (int i = 0; i < dgAttributeList.Rows.Count; i++)
                {
                    string txt = dgAttributeList.Rows[i].Cells["dgAttributeList_options"].Value.ToString().Trim();
                    //배열로 만든다.
                    List<string> lstOption = txt.Split('^').ToList<string>();

                    for (int j = 0; j < lstOption.Count; j++)
                    {
                        if (!dicSrc.ContainsKey(lstOption[j]))
                        {
                            dicSrc.Add(lstOption[j], "");
                        }
                    }
                }

                List<string> lstList = new List<string>();
                lstList = dicSrc.Keys.ToList();
                CancellationTokenSource cts = new CancellationTokenSource();
                var current_synchronization_context = TaskScheduler.FromCurrentSynchronizationContext();
                string srcCountry = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
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

                string translate_lang = "ko";
                progressBar.Value = 0;
                progressBar.Maximum = dgAttributeList.Rows.Count;

                int cur_idx = 0;
                try
                {
                    Task.Run(() =>
                    {
                        Parallel.ForEach(lstList,
                            new ParallelOptions { MaxDegreeOfParallelism = 10 }
                            , dgRow =>
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                try
                                {
                                    if (cts.IsCancellationRequested) return;

                                    //var ic = dgRow.Cells["dg_dep_option_eng"] as DataGridViewTextBoxCell;

                                    Cursor.Current = Cursors.WaitCursor;
                                    string src_title = dgRow;
                                    if (src_title != string.Empty)
                                    {
                                        string translated = string.Empty;
                                        translated = translate(
                                        src_title,
                                        src_lang,
                                        translate_lang).Trim();

                                        dicSrc[src_title] = translated;

                                        cur_idx++;
                                        progressBar.Invoke((MethodInvoker)delegate ()
                                        {
                                            progressBar.Value = cur_idx;
                                        });
                                        Cursor.Current = Cursors.WaitCursor;
                                    }
                                    Thread.Sleep(100);
                                    Cursor.Current = Cursors.WaitCursor;
                                }
                                catch
                                {

                                }
                            });
                    }).ContinueWith(t =>
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        for (int i = 0; i < dgAttributeList.Rows.Count; i++)
                        {
                            string txt = dgAttributeList.Rows[i].Cells["dgAttributeList_options"].Value.ToString().Trim();
                            //배열로 만든다.
                            List<string> lstOption = txt.Split('^').ToList<string>();
                            StringBuilder sb_result = new StringBuilder();
                            for (int j = 0; j < lstOption.Count; j++)
                            {
                                if (dicSrc.ContainsKey(lstOption[j]))
                                {
                                    sb_result.Append(dicSrc[lstOption[j]] + "^");
                                }
                            }

                            if(sb_result.Length > 0)
                            {
                                sb_result.Remove(sb_result.Length - 1, 1);
                            }

                            dgAttributeList.Rows[i].Cells["dgAttributeList_options_kor"].Value = sb_result.ToString();

                            long category_id = Convert.ToInt64(dgAttributeList.Rows[i].Cells["dgAttributeList_category_id"].Value.ToString());
                            long attributeId = Convert.ToInt64(dgAttributeList.Rows[i].Cells["dgAttributeList_attribute_id"].Value.ToString());

                            //저장한다.
                            using (AppDbContext context = new AppDbContext())
                            {
                                AllAttributeList result = context.AllAttributeLists.SingleOrDefault(
                                        b => b.country == srcCountry &&
                                        b.category_id == category_id &&
                                        b.attribute_id == attributeId);

                                if (result != null)
                                {
                                    result.options_kor = sb_result.ToString();
                                    context.SaveChanges();
                                }
                            }
                        }
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("옵션명 번역을 완료 하였습니다.", "옵션명 번역", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    });
                }
                catch
                {
                    // ignore
                }

                Cursor.Current = Cursors.Default;
            }
        }

        private void dgAttributeList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                string val = dgAttributeList.SelectedRows[0].Cells[e.ColumnIndex].Value.ToString().Trim();
                if(val != string.Empty)
                {
                    Clipboard.SetText(val);
                }
            }
        }

        private void dgAttributeList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnSaveExcel_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("속성목록을 CSV파일로 저장 하시겠습니까?", "CSV 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                //파일 저장 위치 선택.
                SaveFileDialog saveDlg = new SaveFileDialog();
                saveDlg.DefaultExt = ".csv";
                saveDlg.InitialDirectory = System.Environment.CurrentDirectory;
                saveDlg.Filter = "csv (*.csv)|*.csv|txt (*txt)|*.txt|All files (*.*)|*.*";
                saveDlg.FileName = "AttributeList_" +
                    dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString() +
                    "_" + DateTime.Now.ToString("yyyy-MM-dd");

                //엑셀로 만들었으나 셀 최대 길이가 32767이라 포기 CSV로 간다.
                //SaveFileDialog saveDlg = new SaveFileDialog();
                //saveDlg.DefaultExt = ".xlsx";
                //saveDlg.Filter = "Excel File (*.xlsx)|*.xlsx";
                //saveDlg.FileName = "AttributeList_" +
                //    dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString() +
                //    "_" + DateTime.Now.ToString("yyyy-MM-dd") +
                //    ".xlsx";
                if (saveDlg.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    DataTable dt = new DataTable("bulk_data");
                    DataColumn dc_no = new DataColumn("번호", typeof(string));
                    DataColumn dc_category_id = new DataColumn("카테고리ID", typeof(string));                    
                    DataColumn dc_attribute_id = new DataColumn("attributeId", typeof(string));
                    DataColumn dc_attribute_name = new DataColumn("속성명", typeof(string));
                    DataColumn dc_attribute_name_kor = new DataColumn("속성명(한글)", typeof(string));
                    DataColumn dc_is_mandatory = new DataColumn("필수", typeof(string));
                    DataColumn dc_attribute_type = new DataColumn("속성타입", typeof(string));
                    DataColumn dc_input_type = new DataColumn("입력타입", typeof(string));
                    DataColumn dc_options = new DataColumn("옵션", typeof(string));
                    DataColumn dc_options_kor = new DataColumn("옵션(한글)", typeof(string));
                    DataColumn dc_values_original_value = new DataColumn("원본데이터", typeof(string));
                    DataColumn dc_values_translate_value = new DataColumn("번역데이터", typeof(string));
                    DataColumn dc_values_translate_kor = new DataColumn("번역(한글)", typeof(string));

                    dt.Columns.Add(dc_no);
                    dt.Columns.Add(dc_category_id);
                    dt.Columns.Add(dc_attribute_id);
                    dt.Columns.Add(dc_attribute_name);
                    dt.Columns.Add(dc_attribute_name_kor);
                    dt.Columns.Add(dc_is_mandatory);
                    dt.Columns.Add(dc_attribute_type);
                    dt.Columns.Add(dc_input_type);
                    dt.Columns.Add(dc_options);
                    dt.Columns.Add(dc_options_kor);
                    dt.Columns.Add(dc_values_original_value);
                    dt.Columns.Add(dc_values_translate_value);
                    dt.Columns.Add(dc_values_translate_kor);

                    DataRow dr;

                    for (int i = 0; i < dgAttributeList.Rows.Count; i++)
                    {
                        dr = dt.NewRow();
                        dr["번호"] = dgAttributeList.Rows[i].Cells["dgAttributeList_no"].Value?.ToString() ?? "";
                        dr["카테고리ID"] = dgAttributeList.Rows[i].Cells["dgAttributeList_category_id"].Value?.ToString() ?? "";
                        dr["attributeId"] = dgAttributeList.Rows[i].Cells["dgAttributeList_attribute_id"].Value?.ToString() ?? "";
                        dr["속성명"] = dgAttributeList.Rows[i].Cells["dgAttributeList_attribute_name"].Value?.ToString() ?? "";
                        dr["속성명(한글)"] = dgAttributeList.Rows[i].Cells["dgAttributeList_attribute_name_kor"].Value?.ToString() ?? "";
                        dr["필수"] = dgAttributeList.Rows[i].Cells["dgAttributeList_is_mandatory"].Value?.ToString() ?? "";
                        dr["속성타입"] = dgAttributeList.Rows[i].Cells["dgAttributeList_attribute_type"].Value?.ToString() ?? "";
                        dr["입력타입"] = dgAttributeList.Rows[i].Cells["dgAttributeList_input_type"].Value?.ToString() ?? "";
                        dr["옵션"] = HttpUtility.HtmlDecode(dgAttributeList.Rows[i].Cells["dgAttributeList_options"].Value?.ToString().Trim() ?? "");                        
                        dr["옵션(한글)"] = HttpUtility.HtmlDecode(dgAttributeList.Rows[i].Cells["dgAttributeList_options_kor"].Value?.ToString() ?? "");
                        dr["원본데이터"] = HttpUtility.HtmlDecode(dgAttributeList.Rows[i].Cells["dgAttributeList_values_original_value"].Value?.ToString() ?? "");
                        dr["번역데이터"] = HttpUtility.HtmlDecode(dgAttributeList.Rows[i].Cells["dgAttributeList_values_translate_value"].Value?.ToString() ?? "");
                        dr["번역(한글)"] = HttpUtility.HtmlDecode(dgAttributeList.Rows[i].Cells["dgAttributeList_values_translate_kor"].Value?.ToString() ?? "");
                        dt.Rows.Add(dr);
                    }

                    try
                    {
                        //파일 저장을 위해 스트림 생성.
                        FileStream fs = new FileStream(saveDlg.FileName, FileMode.Create, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

                        //컬럼 이름들을 ","로 나누고 저장.
                        string line = string.Join(",", dt.Columns.Cast<object>());
                        sw.WriteLine(line);

                        //row들을 ","로 나누고 저장.
                        foreach (DataRow item in dt.Rows)
                        {
                            line = string.Join(",", item.ItemArray.Cast<object>());
                            sw.WriteLine(line);
                        }

                        sw.Close();
                        fs.Close();

                        System.Diagnostics.Process.Start(saveDlg.FileName);
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("CSV파일을 저장하였습니다.", "CSV파일 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //DumpExcel(dt, saveDlg.FileName);
                    }
                    catch (Exception ex)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(ex.Message,"에러발생", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DumpExcel(DataTable tbl, string file_name)
        {
            progressBar.Value = 0;
            progressBar.Maximum = tbl.Rows.Count;
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Attribute List");

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(tbl, true);
                string row_cnt = (tbl.Rows.Count + 1).ToString();
                using (ExcelRange rng = ws.Cells["A1:M1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(79, 129, 189));  //Set color to dark blue
                    rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
                }

                using (ExcelRange rng_all = ws.Cells["A1:M" + row_cnt + string.Empty])
                {
                    rng_all.AutoFitColumns();
                    rng_all.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rng_all.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rng_all.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rng_all.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rng_all.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng_all.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 25;
                ws.Column(5).Width = 25;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 20;
                ws.Column(9).Width = 20;
                ws.Column(10).Width = 20;
                ws.Column(11).Width = 20;

                //전체 시트 잠금
                //ws.Protection.IsProtected = true;
                //부분시트 풀기
                //ws.Column(5).Style.Locked = false;
                //ws.Column(6).Style.Locked = false;
                //ws.Column(1).Style.Locked = false;
                //ws.Column(7).Style.Locked = false;

                //wordwrap 주기
                //ws.Column(3).Style.WrapText = true;
                //ws.Column(5).Style.WrapText = true;
                //ws.Column(6).Style.WrapText = true;
                //ws.Column(7).Style.WrapText = true;
                using (FileStream fs = new FileStream(file_name, FileMode.Create))
                {
                    pck.SaveAs(fs);
                }
            }
            System.Diagnostics.Process.Start(file_name);
            MessageBox.Show("엑셀파일을 저장하였습니다.", "엑셀파일 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            try
            {
                StreamReader sr = new StreamReader(strFilePath);
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = Regex.Split(sr.ReadLine(), ",");
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }


        private void BtnUploadCsv_Click(object sender, EventArgs e)
        {
            //업로드 할 때는 모두 삭제하고 업로드 한다.
            string country = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
            dgAttributeList.Rows.Clear();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CSV Files (*.csv)|*.csv";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                progressBar.Value = 0;
                DataTable dt = ConvertCSVtoDataTable(dialog.FileName);
                if (dt != null)
                {
                    //정상적으로 읽어 온 후 삭제 한다.
                    using (AppDbContext context = new AppDbContext())
                    {
                        List<AllAttributeList> attributeList = new List<AllAttributeList>();
                        attributeList = context.AllAttributeLists
                                .Where(x => x.country == country).ToList();

                        if(attributeList.Count > 0)
                        {
                            context.AllAttributeLists.RemoveRange(attributeList);
                            context.SaveChanges();
                        }

                        progressBar.Maximum = dt.Rows.Count;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dgAttributeList.Rows.Add(
                                dt.Rows[i][0].ToString(),
                                dt.Rows[i][1].ToString(),
                                dt.Rows[i][2].ToString(),
                                dt.Rows[i][3].ToString(),
                                dt.Rows[i][4].ToString(),
                                dt.Rows[i][5].ToString(),
                                dt.Rows[i][6].ToString(),
                                dt.Rows[i][7].ToString(),
                                dt.Rows[i][8].ToString(),
                                dt.Rows[i][9].ToString(),
                                dt.Rows[i][10].ToString(),
                                dt.Rows[i][11].ToString(),
                                dt.Rows[i][12].ToString());

                            bool isMan = false;
                            if(dt.Rows[i][5].ToString().ToUpper() == "TRUE")
                            {
                                isMan = true;
                            }
                            if(dt.Rows[i][1].ToString() =="6561")
                            {
                                var a = "";
                            }
                            AllAttributeList newData = new AllAttributeList
                            {
                                country = country,
                                category_id = Convert.ToInt64(dt.Rows[i][1].ToString()),
                                attribute_id = Convert.ToInt64(dt.Rows[i][2].ToString()),
                                attribute_name = dt.Rows[i][3].ToString(),
                                attribute_name_kor = dt.Rows[i][4].ToString(),
                                is_mandatory = isMan,
                                attribute_type = dt.Rows[i][6].ToString(),
                                input_type = dt.Rows[i][7].ToString(),
                                options = dt.Rows[i][8].ToString(),
                                options_kor = dt.Rows[i][9].ToString(),
                                values_original_value = dt.Rows[i][10].ToString(),
                                values_translate_value = dt.Rows[i][11].ToString(),
                                values_translate_kor = dt.Rows[i][12].ToString()
                            };

                            context.AllAttributeLists.Add(newData);
                            context.SaveChanges();

                            progressBar.Value = i + 1;
                        }
                    }

                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("업로드를 완료 하였습니다.","업로드 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnDeleteAttributeData_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("선택국가의 속성 정보를 삭제 하시겠습니까?", "속성정보 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();
                string country = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();

                using (AppDbContext context = new AppDbContext())
                {
                    List<AllAttributeList> attributeList = new List<AllAttributeList>();
                    attributeList = context.AllAttributeLists
                            .Where(x => x.country == country).ToList();

                    if (attributeList.Count > 0)
                    {
                        context.AllAttributeLists.RemoveRange(attributeList);
                        context.SaveChanges();
                    }

                    //버전 업데이트
                    var res = context.TemplateVersions.FirstOrDefault();
                    if (res != null)
                    {
                        if(country =="ID")
                        {
                            res.attribute_ID = 0;
                        }
                        else if(country == "MY")
                        {
                            res.attribute_MY = 0;
                        }
                        else if (country == "SG")
                        {
                            res.attribute_SG = 0;
                        }
                        else if (country == "PH")
                        {
                            res.attribute_PH = 0;
                        }
                        else if (country == "TH")
                        {
                            res.attribute_TH = 0;
                        }
                        else if (country == "TW")
                        {
                            res.attribute_TW = 0;
                        }
                        else if (country == "VN")
                        {
                            res.attribute_VN = 0;
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        
                    }
                }
                dgAttributeList.Rows.Clear();
                MessageBox.Show("속성정보를 모두 삭제하였습니다.","속성 삭제",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void BtnAddAttributeMapData_Click(object sender, EventArgs e)
        {
            string KR = txt_KR.Text.Trim();
            string SG = txt_SG.Text.Trim();
            string MY = txt_MY.Text.Trim();
            string ID = txt_ID.Text.Trim();
            string TH = txt_TH.Text.Trim();
            string TW = txt_TW.Text.Trim();
            string VN = txt_VN.Text.Trim();
            string PH = txt_PH.Text.Trim();

            if(KR== string.Empty &&
               SG == string.Empty &&
               MY == string.Empty &&
               ID == string.Empty &&
               TH == string.Empty &&
               TW == string.Empty &&
               VN == string.Empty &&
               PH == string.Empty)
            {
                return;
            }


            DialogResult Result = MessageBox.Show("맵핑 정보를 추가 하시겠습니까?", "맵핑정보 추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    AttributeNameMap result = context.AttributeNameMaps.SingleOrDefault(
                                        b => b.KR == KR &&
                                        b.SG == SG &&
                                        b.MY == MY &&
                                        b.ID == ID &&
                                        b.TH == TH &&
                                        b.TW == TW &&
                                        b.VN == VN &&
                                        b.PH == PH);

                    if (result != null)
                    {
                        MessageBox.Show("동일한 데이터가 이미 존재합니다.","데이터 중복",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    else
                    {
                        AttributeNameMap newData = new AttributeNameMap
                        {
                            KR = KR,
                            SG = SG,
                            MY = MY,
                            ID = ID,
                            TH = TH,
                            TW = TW,
                            VN = VN,
                            PH = PH
                        };

                        context.AttributeNameMaps.Add(newData);
                        context.SaveChanges();

                        txt_KR.Text = "";
                        txt_SG.Text = "";
                        txt_MY.Text = "";
                        txt_ID.Text = "";
                        txt_TH.Text = "";
                        txt_TW.Text = "";
                        txt_VN.Text = "";
                        txt_PH.Text = "";
                        getAttributeNameMap();

                        //DgAttributeMap.Rows.Add(DgAttributeMap.Rows.Count + 1, false,
                        //    KR,
                        //    SG,
                        //    MY,
                        //    ID,
                        //    TH,
                        //    TW,
                        //    VN,
                        //    PH);
                    }
                }
            }
        }

        private void BtnDeleteAttributeMapData_Click(object sender, EventArgs e)
        {
            if(DgAttributeMap.Rows.Count > 0)
            {
                DialogResult Result = MessageBox.Show("카테고리 정보를 저장 하시겠습니까?", "카테고리정보 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    for (int i = 0; i < DgAttributeMap.Rows.Count; i++)
                    {
                        if((bool)DgAttributeMap.Rows[i].Cells["DgAttributeMap_Chk"].Value)
                        {
                            int Idx = Convert.ToInt32(DgAttributeMap.Rows[i].Cells["DgAttributeMap_Idx"].Value.ToString());

                            using (AppDbContext context = new AppDbContext())
                            {
                                AttributeNameMap result = context.AttributeNameMaps.SingleOrDefault(
                                                    b => b.Idx == Idx);

                                if (result != null)
                                {
                                    context.AttributeNameMaps.Remove(result);
                                    context.SaveChanges();

                                    txt_KR.Text = "";
                                    txt_SG.Text = "";
                                    txt_MY.Text = "";
                                    txt_ID.Text = "";
                                    txt_TH.Text = "";
                                    txt_TW.Text = "";
                                    txt_VN.Text = "";
                                    txt_PH.Text = "";
                                }
                                
                            }
                        }
                    }


                    getAttributeNameMap();
                    MessageBox.Show("체크한 맵핑 데이터를 삭제하였습니다.", "맵핑데이터 삭제",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
        }

        private void BtnUpdateAttributeMapData_Click(object sender, EventArgs e)
        {
            string KR = txt_KR.Text.Trim();
            string SG = txt_SG.Text.Trim();
            string MY = txt_MY.Text.Trim();
            string ID = txt_ID.Text.Trim();
            string TH = txt_TH.Text.Trim();
            string TW = txt_TW.Text.Trim();
            string VN = txt_VN.Text.Trim();
            string PH = txt_PH.Text.Trim();

            if (KR == string.Empty &&
               SG == string.Empty &&
               MY == string.Empty &&
               ID == string.Empty &&
               TH == string.Empty &&
               TW == string.Empty &&
               VN == string.Empty &&
               PH == string.Empty)
            {
                return;
            }

            int idxNo = Convert.ToInt32(DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_Idx"].Value.ToString());
            DialogResult Result = MessageBox.Show("맵핑 정보를 수정 하시겠습니까?", "맵핑정보 수정", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    AttributeNameMap result = context.AttributeNameMaps.SingleOrDefault(
                                        b => b.KR == KR &&
                                        b.SG == SG &&
                                        b.MY == MY &&
                                        b.ID == ID &&
                                        b.TH == TH &&
                                        b.TW == TW &&
                                        b.VN == VN &&
                                        b.PH == PH);

                    if (result != null)
                    {
                        MessageBox.Show("동일한 데이터가 이미 존재합니다.", "데이터 중복", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {

                        AttributeNameMap resultCur = context.AttributeNameMaps.SingleOrDefault(
                                        b => b.Idx == idxNo);

                        if(resultCur != null)
                        {
                            resultCur.KR = KR;
                            resultCur.SG = SG;
                            resultCur.MY = MY;
                            resultCur.ID = ID;
                            resultCur.TH = TH;
                            resultCur.TW = TW;
                            resultCur.VN = VN;
                            resultCur.PH = PH;
                            context.SaveChanges();


                            DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_KR"].Value = KR;
                            DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_SG"].Value = SG;
                            DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_MY"].Value = MY;
                            DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_ID"].Value = ID;
                            DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_TH"].Value = TH;
                            DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_TW"].Value = TW;
                            DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_VN"].Value = VN;
                            DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_PH"].Value = PH;
                        }
                    }
                }
            }
        }

        private void DgAttributeMap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                if(DgAttributeMap.Rows.Count > 0 && DgAttributeMap.SelectedRows.Count > 0)
                {
                    txt_KR.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_KR"].Value.ToString();
                    txt_SG.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_SG"].Value.ToString();
                    txt_MY.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_MY"].Value.ToString();
                    txt_ID.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_ID"].Value.ToString();
                    txt_TH.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_TH"].Value.ToString();
                    txt_TW.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_TW"].Value.ToString();
                    txt_VN.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_VN"].Value.ToString();
                    txt_PH.Text = DgAttributeMap.SelectedRows[0].Cells["DgAttributeMap_PH"].Value.ToString();
                }
            }
        }
        public DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                OfficeOpenXml.ExcelWorksheet ws = package.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                for (int i = 1; i < ws.Dimension.End.Column + 1; i++)
                {
                    if (ws.Cells[1, i].Value == null)
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
                        if (ws.Cells[i, j + 1].Value == null)
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
        private void BtnUploadMappingData_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".xlsx";
            dialog.Filter = "Excel File (*.xlsx)|*.xlsx";
            dialog.FileName = "AttributeList_" +
                dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString() +
                "_" + DateTime.Now.ToString("yyyy-MM-dd") +
                ".xlsx";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DataTable dt_excel = GetDataTableFromExcel(dialog.FileName, true);

                for (int i = 0; i < dt_excel.Rows.Count; i++)
                {
                    string KR = dt_excel.Rows[i][1].ToString().Trim();
                    string SG = dt_excel.Rows[i][2].ToString().Trim();
                    string MY = dt_excel.Rows[i][3].ToString().Trim();
                    string ID = dt_excel.Rows[i][4].ToString().Trim();
                    string TH = dt_excel.Rows[i][5].ToString().Trim();
                    string TW = dt_excel.Rows[i][6].ToString().Trim();
                    string VN = dt_excel.Rows[i][7].ToString().Trim();
                    string PH = dt_excel.Rows[i][8].ToString().Trim();

                    if (KR == string.Empty &&
                       SG == string.Empty &&
                       MY == string.Empty &&
                       ID == string.Empty &&
                       TH == string.Empty &&
                       TW == string.Empty &&
                       VN == string.Empty &&
                       PH == string.Empty)
                    {
                        return;
                    }

                    else
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            AttributeNameMap result = context.AttributeNameMaps.SingleOrDefault(
                                                b => b.KR == KR &&
                                                b.SG == SG &&
                                                b.MY == MY &&
                                                b.ID == ID &&
                                                b.TH == TH &&
                                                b.TW == TW &&
                                                b.VN == VN &&
                                                b.PH == PH);

                            if (result == null)
                            { 
                                AttributeNameMap newData = new AttributeNameMap
                                {
                                    KR = KR,
                                    SG = SG,
                                    MY = MY,
                                    ID = ID,
                                    TH = TH,
                                    TW = TW,
                                    VN = VN,
                                    PH = PH
                                };

                                context.AttributeNameMaps.Add(newData);
                                context.SaveChanges();

                                DgAttributeMap.Rows.Add(DgAttributeMap.Rows.Count + 1, false,
                                    KR,
                                    SG,
                                    MY,
                                    ID,
                                    TH,
                                    TW,
                                    VN,
                                    PH);
                            }
                        }
                    }
                }
                getAttributeNameMap();
                MessageBox.Show("엑셀데이터를 업로드 하였습니다.", "엑셀 업로드", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnLoadAttribute_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string country = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
            GetAttribute(country);
            Cursor.Current = Cursors.Default;
        }

        private void BtnDownloadAttribute_Click(object sender, EventArgs e)
        {
            //국가별로 저장한다.
            if(dg_site_id.Rows.Count > 0 && dg_site_id.SelectedRows.Count > 0)
            {
                string country = dg_site_id.SelectedRows[0].Cells["dg_site_id_country"].Value.ToString();
                string fileName = "";
                string versionDownAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/attribute/";
                if (country == "ID")
                {
                    fileName = "AttributeList_ID.csv";
                    versionDownAddr = versionDownAddr + "VERSION_ID.txt";
                }
                else if(country == "SG")
                {
                    fileName = "AttributeList_SG.csv";
                    versionDownAddr = versionDownAddr + "VERSION_SG.txt";
                }
                else if (country == "MY")
                {
                    fileName = "AttributeList_MY.csv";
                    versionDownAddr = versionDownAddr + "VERSION_MY.txt";
                }
                else if (country == "TH")
                {
                    fileName = "AttributeList_TH.csv";
                    versionDownAddr = versionDownAddr + "VERSION_TH.txt";
                }
                else if (country == "TW")
                {
                    fileName = "AttributeList_TW.csv";
                    versionDownAddr = versionDownAddr + "VERSION_TW.txt";
                }
                else if (country == "PH")
                {
                    fileName = "AttributeList_PH.csv";
                    versionDownAddr = versionDownAddr + "VERSION_PH.txt";
                }

                using (AppDbContext context = new AppDbContext())
                {
                    FormDownloadData form = new FormDownloadData();
                    System.Net.WebClient wc = new System.Net.WebClient();
                    byte[] raw = wc.DownloadData(versionDownAddr);

                    long version = Convert.ToInt64(System.Text.Encoding.UTF8.GetString(raw));

                    var result = context.TemplateVersions.FirstOrDefault();
                    if (result != null)
                    {
                        bool validate = false;

                        if(country == "SG")
                        {
                            if(result.attribute_SG < version)
                            {
                                validate = true;
                                form.templateCurVersion = result.attribute_SG;
                            }
                        }
                        else if (country == "MY")
                        {
                            if (result.attribute_MY < version)
                            {
                                validate = true;
                                form.templateCurVersion = result.attribute_MY;
                            }
                        }
                        else if (country == "ID")
                        {
                            if (result.attribute_ID < version)
                            {
                                validate = true;
                                form.templateCurVersion = result.attribute_ID;
                            }
                        }
                        else if (country == "PH")
                        {
                            if (result.attribute_PH < version)
                            {
                                validate = true;
                                form.templateCurVersion = result.attribute_PH;
                            }
                        }
                        else if (country == "TW")
                        {
                            if (result.attribute_TW < version)
                            {
                                validate = true;
                                form.templateCurVersion = result.attribute_TW;
                            }
                        }
                        else if (country == "TH")
                        {
                            if (result.attribute_TH < version)
                            {
                                validate = true;
                                form.templateCurVersion = result.attribute_TH;
                            }
                        }

                        if (validate)
                        {
                            form.country = country;
                            form.templateNewVersion = version;
                            form.templateName = country + " 속성 번역 템플릿";
                            form.downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/attribute/" + fileName;
                            form.ShowDialog();
                        }
                    }
                    else
                    {
                        form.country = country;
                        form.templateNewVersion = version;
                        form.templateCurVersion = 0;
                        form.templateName = country + " 속성 번역 템플릿";
                        form.downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/attribute/" + fileName;
                        form.ShowDialog();
                    }
                }
            }
        }

        private void BtnDownloadMappingData_Click(object sender, EventArgs e)
        {
            using (AppDbContext context = new AppDbContext())
            {
                FormDownloadData form = new FormDownloadData();
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] raw = wc.DownloadData("https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/attribute_name_map/version.txt");

                long version = Convert.ToInt64(System.Text.Encoding.UTF8.GetString(raw));

                var result = context.TemplateVersions.FirstOrDefault();
                if (result != null)
                {
                    if (result.attribute_name_map < version)
                    {
                        form.templateCurVersion = result.attribute_name_map;
                        form.templateNewVersion = version;
                        form.templateName = "속성명 맵핑 템플릿";
                        form.downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/attribute_name_map/attribute_name_map.xlsx";
                        form.ShowDialog();
                    }
                }
                else
                {

                    form.templateNewVersion = version;
                    form.templateCurVersion = 0;
                    form.templateName = "속성명 맵핑 템플릿";
                    form.downloadAddr = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/template/attribute_name_map/attribute_name_map.xlsx";
                    form.ShowDialog();
                }
            }
        }

        private void BtnAttributeNameMapLoad_Click(object sender, EventArgs e)
        {
            getAttributeNameMap();
        }
    }
}
