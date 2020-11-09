using Newtonsoft.Json;
using RestSharp;
using ShopeeManagement.DTOs.Res;
using System.Collections.Generic;
using System.Net;

namespace ShopeeManagement.DTOs.Req
{
    public class ReqUploadImg : ReqBase
    {
        [JsonProperty("images")]
        public List<string> Images { get; set; }

        [JsonProperty("partner_id")]
        public long PartnerId { get; set; }

        [JsonProperty("shopid")]
        public long ShopId { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        public ResUploadImg CallApi(string apiKey)
        {
            string sourceUri = "https://partner.shopeemobile.com/api/v1/image/upload";
            string serializedObjectData = JsonConvert.SerializeObject(this);
            var restClient = new RestClient(sourceUri);
            var restRequest = new RestRequest
            {
                Method = Method.POST,
                RequestFormat = DataFormat.Json
            };
            string authorization = HashString($"{sourceUri}|{serializedObjectData}", apiKey);
            restRequest.AddHeader(nameof(authorization), authorization);
            restRequest.AddJsonBody(serializedObjectData);
            IRestResponse response = restClient.Execute(restRequest);

            if (response is null || response.StatusCode != HttpStatusCode.OK || response.Content is null)
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<ResUploadImg>(response.Content);
            }
        }
    }
}
