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
    public partial class FormDeleteDiscountEvent : MetroForm
    {
        public FormDeleteDiscountEvent()
        {
            InitializeComponent();
        }

        public long discount_id;
        public long i_partner_id;
        public long i_shop_id;
        public string api_key;

        private void FormDeleteDiscountEvent_Load(object sender, EventArgs e)
        {

        }
        private static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
        private void btn_delete_discount_Click(object sender, EventArgs e)
        {
            if (discount_id != 0)
            {
                DialogResult Result = MessageBox.Show("쇼피 할인 프로모션을 삭제하시겠습니까?", "쇼피 할인 프로모션 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    long time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));
                    string end_point = "https://partner.shopeemobile.com/api/v1/discount/delete";
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("discount_id", discount_id);
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
            string sbinary = string.Empty;

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }
    }
}
