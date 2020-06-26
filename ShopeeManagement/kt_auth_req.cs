using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    public class Domain
    {
        public string id { get; set; }
    }

    public class User
    {
        public string name { get; set; }
        public Domain domain { get; set; }
        public string password { get; set; }
    }

    public class Password
    {
        public User user { get; set; }
    }

    public class Identity
    {
        public string[] methods { get; set; }
        public Password password { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
    }

    public class Scope
    {
        public Project project { get; set; }
    }

    public class Auth
    {
        public Identity identity { get; set; }
        public Scope scope { get; set; }
    }

    public class kt_auth_req
    {
        public Auth auth { get; set; }
        public bool validate_token()
        {
            bool rtn = false;
            if (global_var.kt_token_exp == null || global_var.kt_token_exp < DateTime.Now)
            {
                get_token();
            }
            return rtn;
        }
        public bool put_file2(string src_img_path, string OS_save_path, string tar_img_path)
        {
            bool rtn = false;

            if (File.Exists(src_img_path))
            {
                try
                {
                    Stream writeStream = null;
                    FileStream readStream = null;

                    System.Net.HttpWebRequest webRequest = System.Net.HttpWebRequest.Create(OS_save_path + Path.GetFileName(tar_img_path)) as System.Net.HttpWebRequest;
                    webRequest.Method = "PUT";
                    webRequest.ContentType = "image/jpeg";
                    webRequest.Headers.Add("X-Auth-Token", global_var.kt_token);
                    webRequest.UserAgent = "ITEM_FINDER/1.0";
                    webRequest.Timeout = 10000; // System.Threading.Timeout.Infinite;
                    webRequest.KeepAlive = true;
                    //webRequest.SendChunked = true;
                    writeStream = webRequest.GetRequestStream();
                    readStream = new FileStream(src_img_path, FileMode.Open, FileAccess.Read, FileShare.Read);
                    byte[] buffer = new byte[1024];
                    int count = 0;
                    while ((count = readStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        writeStream.Write(buffer, 0, count);
                    }

                    if (readStream != null)
                    {
                        try
                        {
                            readStream.Close();
                            readStream.Dispose();
                        }
                        catch
                        {
                            System.Windows.Forms.MessageBox.Show("읽기 스트림 닫기 오류");
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
                            System.Windows.Forms.MessageBox.Show("쓰기 스트림 닫기 오류");
                        }
                    }

                    if (webRequest != null)
                    {
                        try
                        {
                            using (WebResponse response = webRequest.GetResponse())
                            {
                                response.Close();
                            }

                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show("요청 닫기 오류 " + ex.Message);
                        }
                    }
                }
                catch
                {
                    //MessageBox.Show("put_file2 쓰기 오류\r\n" + ex.Message);
                }

            }
            return rtn;
        }
        public dynamic get_file_list(string user_id, string site_code, string p_guid)
        {
            if (string.IsNullOrEmpty(global_var.kt_token))
            {
                get_token();
            }
            dynamic rtn = null;
            string image_path = site_code + "/" + p_guid;
            var client_list = new RestClient("https://ssproxy2.ucloudbiz.olleh.com/v1/" + global_var.ucloudbiz_account + "/sto_itemfinder?path=" + user_id + "/contents/" + image_path);
            RestRequest rq_list = new RestRequest(Method.GET);
            rq_list.AddHeader("X-Auth-Token", global_var.kt_token);
            //rq_list.AddParameter("application/json; charset=utf-8", json.ToString(), ParameterType.RequestBody);
            IRestResponse rp_list = client_list.Execute(rq_list);
            if (rp_list.StatusCode == HttpStatusCode.OK)
            {
                rtn = JsonConvert.DeserializeObject<dynamic>(rp_list.Content);
            }

            return rtn;
        }
        public bool rename_file(string user_id, string site_code, string p_guid, string src_file, string tar_file)
        {
            bool rtn = false;
            string image_path = site_code + "/" + p_guid;
            string src_full_path = "/sto_itemfinder/" + user_id + "/contents/" + site_code + "/" + p_guid + "/" + src_file;
            var client_list = new RestClient("https://ssproxy2.ucloudbiz.olleh.com/v1/" + global_var.ucloudbiz_account + "/sto_itemfinder/" + user_id + "/contents/" + site_code + "/" + p_guid + "/" + tar_file);
            RestRequest rq_list = new RestRequest(Method.PUT);
            rq_list.AddHeader("X-Auth-Token", global_var.kt_token);
            rq_list.AddHeader("X-Copy-From", src_full_path);
            //rq_list.AddParameter("application/json; charset=utf-8", json.ToString(), ParameterType.RequestBody);
            IRestResponse rp_list = client_list.Execute(rq_list);
            if (rp_list.StatusCode == HttpStatusCode.Created)
            {
                rtn = true;
            }

            return rtn;
        }
        public void save_object(string user_id, string site_code, string pGuid, string file_name, FileStream fs)
        {
            Stream writeStream = null;
            FileStream readStream = null;
            string OS_endpoint = "https://ssproxy2.ucloudbiz.olleh.com/v1/" + global_var.ucloudbiz_account + "/sto_sm";
            //최하위 경로만 존재해도 폴더는 생성이 된다.
            string OS_save_path = OS_endpoint + "/" + pGuid.ToString() + "/";

            System.Net.HttpWebRequest webRequest = System.Net.HttpWebRequest.Create(OS_save_path + file_name) as System.Net.HttpWebRequest;
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
                    readStream.Close();
                    readStream.Dispose();
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
                    System.Net.WebResponse response = webRequest.GetResponse();
                    response.Close();
                }
                catch
                {
                }
            }
        }
        public dynamic delete_file(string user_id, string site_code, string p_guid, string file_name)
        {
            dynamic rtn = null;
            string image_path = site_code + "/" + p_guid + "/" + file_name;
            var client_list = new RestClient("https://ssproxy2.ucloudbiz.olleh.com/v1/" + global_var.ucloudbiz_account + "/sto_itemfinder/" + user_id + "/contents/" + image_path);
            RestRequest rq_list = new RestRequest(Method.DELETE);
            rq_list.AddHeader("X-Auth-Token", global_var.kt_token);
            //rq_list.AddParameter("application/json; charset=utf-8", json.ToString(), ParameterType.RequestBody);
            IRestResponse rp_list = client_list.Execute(rq_list);
            if (rp_list.StatusCode == HttpStatusCode.NoContent)
            {
                rtn = JsonConvert.DeserializeObject<dynamic>(rp_list.Content);
            }

            return rtn;
        }
        public void get_token()
        {
            //kt토큰은 만료 되었을 때 다시 가지고 온다.
            if (global_var.kt_token == string.Empty)
            {
                var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
                var response = httpClient.PostAsync(
                        "https://ssproxy2.ucloudbiz.olleh.com:5000/v3/auth/tokens",
                        new StringContent(global_var.ucloudbiz_account_json, Encoding.UTF8, "application/json"))
                    .Result.EnsureSuccessStatusCode();

                string XAuthToken = response.Headers.GetValues("X-Subject-Token").First();
                global_var.kt_token = XAuthToken;

                string exp_date = response.Headers.GetValues("Date").First();
                DateTime convertedDate = DateTime.Parse(exp_date);
                convertedDate = convertedDate.AddHours(1);
                global_var.kt_token_exp = convertedDate;
            }
            else
            {
                if (global_var.kt_token_exp < DateTime.Now)
                {
                    var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
                    var response = httpClient.PostAsync(
                            "https://ssproxy2.ucloudbiz.olleh.com:5000/v3/auth/tokens",
                            new StringContent(global_var.ucloudbiz_account_json, Encoding.UTF8, "application/json"))
                        .Result.EnsureSuccessStatusCode();

                    string XAuthToken = response.Headers.GetValues("X-Subject-Token").First();
                    global_var.kt_token = XAuthToken;

                    string exp_date = response.Headers.GetValues("Date").First();
                    DateTime convertedDate = DateTime.Parse(exp_date);
                    convertedDate = convertedDate.AddHours(1);
                    global_var.kt_token_exp = convertedDate;
                }
            }
        }
    }
}
