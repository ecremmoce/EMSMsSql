using Newtonsoft.Json;
using System;

namespace ShopeeManagement.DTOs.Res
{
    public class ShopeeSearchAttribute
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("AttributeId")]
        public long AttributeId { get; set; }

        [JsonProperty("AttributeName")]
        public string AttributeName { get; set; }

        [JsonProperty("AttributeNameKo")]
        public string AttributeNameKo { get; set; }

        [JsonProperty("IsMandatory")]
        public bool IsMandatory { get; set; }

        [JsonProperty("AttributeType")]
        public string AttributeType { get; set; }

        [JsonProperty("InputType")]
        public string InputType { get; set; }

        [JsonProperty("Options")]
        public string[] Options { get; set; }

        [JsonProperty("OptionsKo")]
        public string[] OptionsKo { get; set; }
    }
}
