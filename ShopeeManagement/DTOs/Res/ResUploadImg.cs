using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShopeeManagement.DTOs.Res
{
    public class ResUploadImg
    {
        [JsonProperty("images")]
        public List<ResUploadImgImages> Images { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }

    public class ResUploadImgImages
    {
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("shopee_image_url")]
        public string ShopeeImageUrl { get; set; }
    }
}
