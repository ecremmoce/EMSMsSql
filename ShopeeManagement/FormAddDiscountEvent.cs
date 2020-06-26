using MetroFramework.Forms;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopeeManagement
{
    public partial class FormAddDiscountEvent : MetroForm
    {
        public FormAddDiscountEvent()
        {
            InitializeComponent();
        }
        public string discount_id;
        public long i_partner_id;
        public long i_shop_id;
        public string api_key;
        private void FormAddDiscountEvent_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(discount_id))
            {
                btn_create_discount.Text = "할인 수정";
                dt_start_date.Enabled = false;
            }
            else
            {
                dt_end_date.Value = DateTime.Now.AddYears(3);
            }
        }

        private static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        private void btn_create_discount_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(discount_id))
            {
                DialogResult Result = MessageBox.Show("쇼피 할인 프로모션을 생성하시겠습니까?", "쇼피 할인 프로모션 생성", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    long time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));
                    long start_time = ToUnixTime(dt_start_date.Value.AddHours(-8).AddMinutes(-59).AddSeconds(-10));
                    long end_time = ToUnixTime(dt_end_date.Value.AddHours(-9));
                    string end_point = "https://partner.shopeemobile.com/api/v1/discount/add";
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("discount_name", txt_discount_name.Text.Trim());
                    dic.Add("start_time", start_time);
                    dic.Add("end_time", end_time);
                    dic.Add("partner_id", i_partner_id);
                    dic.Add("shopid", i_shop_id);
                    dic.Add("timestamp", time_stamp);

                    var client = new RestClient(end_point);
                    var request = new RestRequest("", Method.POST);
                    request.Method = Method.POST;
                    request.AddHeader("Accept", "application/json");
                    request.AddJsonBody(JsonConvert.SerializeObject(dic));
                    request.AddHeader("authorization",
                                HashString(end_point + "|" + JsonConvert.SerializeObject(dic),
                                api_key));
                    IRestResponse response = client.Execute(request);
                    var content = response.Content;
                    dynamic result = null;
                    if (!content.Contains("504 Gateway Time-out") && !content.Contains("502 Bad Gateway"))
                    {
                        try
                        {
                            result = JsonConvert.DeserializeObject(content);
                        }
                        catch
                        {

                        }
                    }

                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                DialogResult Result = MessageBox.Show("쇼피 할인 프로모션을 수정하시겠습니까?", "쇼피 할인 프로모션 수정", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    long time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));
                    long start_time = ToUnixTime(dt_start_date.Value);
                    long end_time = ToUnixTime(dt_end_date.Value);
                    string end_point = "https://partner.shopeemobile.com/api/v1/discount/update";
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("discount_id", discount_id);
                    dic.Add("discount_name", txt_discount_name.Text.Trim());
                    //dic.Add("start_time", start_time);
                    dic.Add("end_time", end_time);
                    dic.Add("partner_id", i_partner_id);
                    dic.Add("shopid", i_shop_id);
                    dic.Add("timestamp", time_stamp);

                    var client = new RestClient(end_point);
                    var request = new RestRequest("", Method.POST);
                    request.Method = Method.POST;
                    request.AddHeader("Accept", "application/json");
                    request.AddJsonBody(JsonConvert.SerializeObject(dic));
                    request.AddHeader("authorization",
                                HashString(end_point + "|" + JsonConvert.SerializeObject(dic),
                                api_key));
                    IRestResponse response = client.Execute(request);
                    var content = response.Content;
                    dynamic result = null;
                    if (!content.Contains("504 Gateway Time-out") && !content.Contains("502 Bad Gateway"))
                    {
                        try
                        {
                            if (content.Contains("You may only shorten the period of time."))
                            {
                                MessageBox.Show("종료 시간은 단축만 가능합니다.", "종료시간 설정 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else if (content.Contains("End time should be 1 hour later than start time."))
                            {
                                MessageBox.Show("종료시간은 시작시간 보다 1시간 늦어야 합니다.", "종료시간 설정 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                result = JsonConvert.DeserializeObject(content);
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("수정하였습니다.", "수정 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        private string HashString(string StringToHash, string HashKey)
        {
            System.Text.UTF8Encoding myEncoder = new UTF8Encoding();            
            byte[] key = myEncoder.GetBytes(HashKey);
            byte[] Text = myEncoder.GetBytes(StringToHash);
            HMACSHA256 myHMACSHA256 = new HMACSHA256(key);
            byte[] HashCode = myHMACSHA256.ComputeHash(Text);
            string hash5 = ByteToString(HashCode);
            return hash5.ToLower();
        }

        public static string ByteToString(byte[] buff)
        {
            string sbinary = string.Empty;

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }
    }
}
