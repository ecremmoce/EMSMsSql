using System;

namespace ShopeeManagement
{
    public class CoupangGetQueryString
    {
        public long ItemId { get; set; }

        public CoupangGetQueryStringGetParameters Parameters { get; set; }

        public void DeserializeUri(string uri)
        {
            // sample: https://m.coupang.com/vm/products/140216498?vendorItemId=4000601927&sourceType=HOME_TRENDING_ADS&searchId=feed-1604303030117-trending_ads-56481&isAddedCart=
            // sample: https://m.coupang.com/vm/products/2945640?vendorItemId=3021092607&sourceType=HOME_PERSONALIZED_ADS&searchId=feed-1604301457959-personalized_ads&isAddedCart=

            int seperatorIdx = uri.IndexOf('?');
            int itemIdStartIdx = uri.Substring(0, seperatorIdx).LastIndexOf('/');
            ItemId = Convert.ToInt64(uri.Substring(itemIdStartIdx + 1, seperatorIdx - itemIdStartIdx - 1));
            string getParameters = uri.Substring(seperatorIdx);
            string[] parameters = getParameters.Split('&');
            var ParameterObject = new CoupangGetQueryStringGetParameters();

            foreach (string item in parameters)
            {
                if (item.Contains("vendorItemId"))
                {
                    ParameterObject.VendorItemId = Convert.ToInt64(item.Split('=')[1]);
                }
                else if (item.Contains("sourceType"))
                {
                    ParameterObject.SourceType = item.Split('=')[1];
                }
                else if (item.Contains("searchId"))
                {
                    string searchId = item.Split('=')[1];
                    ParameterObject.SearchId = new CoupangGetQueryStringGetParametersSearchId
                    {
                        TimeStamp = Convert.ToInt64(searchId.Substring(5, 13)),
                        SourceType = searchId.Substring(19),
                    };
                }
                else
                {
                    ParameterObject.IsAddedCart = item.Split('=')[1];
                }
            }

            Parameters = ParameterObject;
        }
    }

    public class CoupangGetQueryStringGetParameters
    {
        public long VendorItemId { get; set; }

        public string SourceType { get; set; }

        public CoupangGetQueryStringGetParametersSearchId SearchId { get; set; }

        public string IsAddedCart { get; set; }
    }

    public class CoupangGetQueryStringGetParametersSearchId
    {
        public long TimeStamp { get; set; }

        public string SourceType { get; set; }

        public string MakeSearchId()
        {
            return $"feed-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}-{SourceType}";
        }
    }
}
