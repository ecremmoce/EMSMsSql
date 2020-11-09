using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Net;

namespace ShopeeManagement.APIs
{
    public class NaverSmartStoreGetBodyImage
    {
        public ResNaverSmartStoreGetBodyImage CallApi(string uri, string item1, string item2)
        {
            var smartStoreCookie = new CookieContainer();
            //string cookieString = smartStoreCookie.GetCookies(new Uri("https://smartstore.naver.com")).OfType<Cookie>().FirstOrDefault(cookie => cookie.Name.Equals("")).Value;

            var client = new RestClient($"https://smartstore.naver.com/i/v1/products/{item1}/contents/{item2}/PC")
            {
                CookieContainer = smartStoreCookie
            };
            var request = new RestRequest(Method.GET);
            request.AddHeader("Host", "smartstore.naver.com");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept", "application/json, text/plain, */*");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.183 Safari/537.36");
            request.AddHeader("Sec-Fetch-Site", "same-origin");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Referer", uri);
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Accept-Language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
            //request.AddHeader("X-CSRFToken", cookieString);

            IRestResponse response = client.Execute(request);
            string content = response.Content;

            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<ResNaverSmartStoreGetBodyImage>(content);
            }
        }
    }

    public class ResNaverSmartStoreGetBodyImage
    {
        [JsonProperty("contentId")]
        public long ContentId { get; set; }

        [JsonProperty("editorType")]
        public string EditorType { get; set; }

        [JsonProperty("textContent")]
        public string TextContent { get; set; }

        [JsonProperty("renderContent")]
        public string RenderContent { get; set; }

        [JsonProperty("mobileProductDetailPreviewType")]
        public string MobileProductDetailPreviewType { get; set; }

        [JsonProperty("mobileDetailType")]
        public string MobileDetailType { get; set; }

        [JsonProperty("snapImages")]
        public Uri[] SnapImages { get; set; }
    }
}
