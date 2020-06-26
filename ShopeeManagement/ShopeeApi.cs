using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ShopeeApi
    {
        private long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        private long getUnixTime()
        {
            DateTime dtNow = DateTime.Now.AddHours(-9);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((dtNow - epoch).TotalSeconds);
        }
        public string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        public dynamic GetItemDetail(long item_id, long partner_id, long shop_id, string secret_key)
        {
            //------------------------------------상품 목록 획득 ----------------------------------                
            long time_stamp = getUnixTime();
            string endPoint = "https://partner.shopeemobile.com/api/v1/item/get";

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("item_id", item_id);
            dic.Add("partner_id", partner_id);
            dic.Add("shopid", shop_id);
            dic.Add("timestamp", time_stamp);

            var request = new RestRequest("", Method.POST);
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(JsonConvert.SerializeObject(dic));
            request.AddHeader("authorization",
                HashString(endPoint + "|" + JsonConvert.SerializeObject(dic),
                secret_key));

            var client = new RestClient(endPoint);
            IRestResponse response = client.Execute(request);
            dynamic result = null;

            try
            {

                result = JsonConvert.DeserializeObject(response.Content);
                if (result.msg != null)
                {
                    result = null;
                }

            }
            catch
            {

            }
            return result;
        }
        private string HashString(string StringToHash, string HashKey)
        {
            //특수문자의 오류로 인하여 엔코딩을 UTF8로 변경함.
            System.Text.UTF8Encoding myEncoder = new UTF8Encoding();
            byte[] key = myEncoder.GetBytes(HashKey);
            byte[] Text = myEncoder.GetBytes(StringToHash);
            HMACSHA256 myHMACSHA256 = new HMACSHA256(key);
            byte[] HashCode = myHMACSHA256.ComputeHash(Text);
                string hash = ByteToString(HashCode);
            return hash.ToLower();
        }
        public dynamic GetTierVariations(long item_id, long partner_id, long shopid, string secretKey)
        {
            //------------------------------------상품 Tier정보 획득 ----------------------------------                
            long time_stamp = getUnixTime();
            string endPoint = "https://partner.shopeemobile.com/api/v1/item/tier_var/get";

            Dictionary<string, object> dic_tier_info = new Dictionary<string, object>();
            dic_tier_info.Add("item_id", item_id);
            dic_tier_info.Add("partner_id", partner_id);
            dic_tier_info.Add("shopid", shopid);
            dic_tier_info.Add("timestamp", time_stamp);

            var request_tier_info = new RestRequest("", Method.POST);
            request_tier_info.Method = Method.POST;
            request_tier_info.AddHeader("Accept", "application/json");
            request_tier_info.AddJsonBody(new
            {
                item_id = item_id,
                partner_id = partner_id,
                shopid = shopid,
                timestamp = time_stamp
            });
            request_tier_info.AddHeader("authorization",
                HashString(endPoint + "|" + JsonConvert.SerializeObject(dic_tier_info),
                secretKey));

            var client_tier_info = new RestClient(endPoint);
            IRestResponse response_tier_info = client_tier_info.Execute(request_tier_info);

            dynamic result_tier_info = null;

            try
            {
                result_tier_info = JsonConvert.DeserializeObject(response_tier_info.Content);
                if(result_tier_info.msg != null && result_tier_info.msg.ToString().Contains("This api can only support item which has 2-tier variations"))
                {
                    result_tier_info = null;
                }
            }
            catch(Exception ex)
            {

            }

            return result_tier_info;
        }

        public dynamic AddVariations(long item_id, List<shopee_variations> ListVariation, long partner_id, long shopid, string secretKey)
        {
            long timeStamp = getUnixTime();
            dynamic rtn;
            string endPoint = "https://partner.shopeemobile.com/api/v1/item/add_variations";

            Dictionary<string, object> dic_jsonStock = new Dictionary<string, object>();
            dic_jsonStock.Add("item_id", item_id);
            dic_jsonStock.Add("variations", ListVariation);
            dic_jsonStock.Add("partner_id", partner_id);
            dic_jsonStock.Add("shopid", shopid);
            dic_jsonStock.Add("timestamp", timeStamp);


            var requestStock = new RestRequest("", RestSharp.Method.POST);


            requestStock.AddJsonBody(JsonConvert.SerializeObject(dic_jsonStock));


            requestStock.Method = Method.POST;
            requestStock.AddHeader("Accept", "application/json");
            requestStock.AddHeader("authorization",
                HashString(endPoint + "|" + JsonConvert.SerializeObject(dic_jsonStock),
                secretKey));

            var clientStock = new RestClient(endPoint);

            IRestResponse responseStock = clientStock.Execute(requestStock);
            var contentStock = responseStock.Content;

            dynamic result = null;

            try
            {
                result = JsonConvert.DeserializeObject(responseStock.Content);
            }
            catch
            {

            }

            return result;
        }
        public bool InitTierVariation(Item2TierVariationInit initTier, long partner_id, long shopid, string secretKey)
        {
            long timeStamp = getUnixTime();
            bool rtn = false;
            string endPoint = "https://partner.shopeemobile.com/api/v1/item/tier_var/init";

            Dictionary<string, object> dic_jsonStock = new Dictionary<string, object>();
            dic_jsonStock.Add("item_id", initTier.item_id);
            dic_jsonStock.Add("tier_variation", initTier.tier_variation);
            dic_jsonStock.Add("variation", initTier.variation);
            dic_jsonStock.Add("partner_id", partner_id);
            dic_jsonStock.Add("shopid", shopid);
            dic_jsonStock.Add("timestamp", timeStamp);

            var requestStock = new RestRequest("", RestSharp.Method.POST);
            requestStock.AddJsonBody(JsonConvert.SerializeObject(dic_jsonStock));

            requestStock.Method = Method.POST;
            requestStock.AddHeader("Accept", "application/json");
            requestStock.AddHeader("authorization",
                HashString(endPoint + "|" + JsonConvert.SerializeObject(dic_jsonStock),
                secretKey));

            var clientStock = new RestClient(endPoint);

            try
            {
                IRestResponse responseStock = clientStock.Execute(requestStock);
                var contentStock = responseStock.Content;
                rtn = true; 
            }
            catch
            {
                rtn = false;
            }


            return rtn;
        }
        public bool DeleteVariation(long item_id, long variation_id, long partner_id, long shopid, string secretKey)
        {
            long timeStamp = getUnixTime();
            bool rtn = false;
            string endPoint = "https://partner.shopeemobile.com/api/v1/item/delete_variation";

            Dictionary<string, object> dic_jsonStock = new Dictionary<string, object>();
            dic_jsonStock.Add("item_id", item_id);
            dic_jsonStock.Add("variation_id", variation_id);
            dic_jsonStock.Add("partner_id", partner_id);
            dic_jsonStock.Add("shopid", shopid);
            dic_jsonStock.Add("timestamp", timeStamp);


            var requestStock = new RestRequest("", RestSharp.Method.POST);


            requestStock.AddJsonBody(JsonConvert.SerializeObject(dic_jsonStock));


            requestStock.Method = Method.POST;
            requestStock.AddHeader("Accept", "application/json");
            requestStock.AddHeader("authorization",
                HashString(endPoint + "|" + JsonConvert.SerializeObject(dic_jsonStock),
                secretKey));

            var clientStock = new RestClient(endPoint);
            IRestResponse response = clientStock.Execute(requestStock);
            var contentStock = response.Content;

            dynamic result = JsonConvert.DeserializeObject(response.Content);

            if (result.error != null && result.error.ToString().Contains("error_not_exists"))
            {
                rtn = false;
            }
            else
            {
                rtn = true;
            }

            return rtn;
        }
        private string HashStringASCII(string StringToHash, string HashKey)
        {
            //System.Text.UTF8Encoding myEncoder = new UTF8Encoding();
            System.Text.ASCIIEncoding myEncoder = new ASCIIEncoding();
            byte[] key = myEncoder.GetBytes(HashKey);
            byte[] Text = myEncoder.GetBytes(StringToHash);
            HMACSHA256 myHMACSHA256 = new HMACSHA256(key);
            byte[] HashCode = myHMACSHA256.ComputeHash(Text);
            string hash = ByteToString(HashCode);
            return hash.ToLower();
        }
        public bool AddTierVariation(long item_id, List<variation> lst2TierVariation, long partner_id, long shopid, string secretKey)
        {
            //lst2TierVariation.RemoveAt(0);
            long timeStamp = getUnixTime();
            bool rtn = false;
            string endPoint = "https://partner.shopeemobile.com/api/v1/item/tier_var/add";

            Dictionary<string, object> dic_jsonStock = new Dictionary<string, object>();
            dic_jsonStock.Add("partner_id", partner_id);
            dic_jsonStock.Add("shopid", shopid);
            dic_jsonStock.Add("timestamp", timeStamp);
            dic_jsonStock.Add("item_id", item_id);
            dic_jsonStock.Add("variation", lst2TierVariation);

            var requestStock = new RestRequest("", RestSharp.Method.POST);

            requestStock.AddJsonBody(JsonConvert.SerializeObject(dic_jsonStock));

            requestStock.Method = Method.POST;
            requestStock.AddHeader("Accept", "application/json");
            requestStock.AddHeader("authorization",
                HashString(endPoint + "|" + JsonConvert.SerializeObject(dic_jsonStock),
                secretKey));

            var clientStock = new RestClient(endPoint);

            try
            {
                IRestResponse responseStock = clientStock.Execute(requestStock);
                var contentStock = responseStock.Content;
                rtn = true;
            }
            catch
            {
                rtn = false;
            }


            return rtn;
        }
        public bool UpdateVariationStockBatch(List<ItemVariation> itemVariation, long partner_id, long shopid, string secretKey)
        {
            long timeStamp = getUnixTime();
            bool rtn = false;

            string endPointStock = "https://partner.shopeemobile.com/api/v1/items/update/vars_stock";
            var requestStock = new RestRequest("", RestSharp.Method.POST);

            List<ShopeeVariationBatch> lstBatch = new List<ShopeeVariationBatch>();
            for (int vari = 0; vari < itemVariation.Count; vari++)
            {
                var varBatch = new ShopeeVariationBatch
                {
                    variation_id = itemVariation[vari].variation_id,
                    stock = itemVariation[vari].stock,
                    item_id = itemVariation[vari].item_id
                };

                lstBatch.Add(varBatch);
            }

            Dictionary<string, object> dic_jsonStock = new Dictionary<string, object>();
            dic_jsonStock.Add("partner_id", partner_id);
            dic_jsonStock.Add("shopid", shopid);
            dic_jsonStock.Add("timestamp", timeStamp);
            dic_jsonStock.Add("variations", lstBatch);

            requestStock.AddJsonBody(JsonConvert.SerializeObject(dic_jsonStock));            

            requestStock.Method = Method.POST;
            requestStock.AddHeader("Accept", "application/json");
            requestStock.AddHeader("authorization",
                HashString(endPointStock + "|" + JsonConvert.SerializeObject(dic_jsonStock),
                secretKey));

            try
            {
                var clientStock = new RestClient(endPointStock);
                IRestResponse responseStock = clientStock.Execute(requestStock);
                var contentStock = responseStock.Content;
                rtn = true;
            }
            catch
            {
                rtn = false;
            }

            return rtn;
        }
        public bool UpdateStock(long itemId, int stock, long partner_id, long shopid, string secretKey)
        {
            long timeStamp = getUnixTime();
            bool rtn = false;
            Dictionary<string, object> dic_jsonStock = new Dictionary<string, object>();
            dic_jsonStock.Add("item_id", itemId);
            dic_jsonStock.Add("stock", stock);
            dic_jsonStock.Add("partner_id", partner_id);
            dic_jsonStock.Add("shopid", shopid);
            dic_jsonStock.Add("timestamp", timeStamp);

            string endPointStock = "https://partner.shopeemobile.com/api/v1/items/update_stock";
            var requestStock = new RestRequest("", RestSharp.Method.POST);


            requestStock.AddJsonBody(JsonConvert.SerializeObject(dic_jsonStock));


            requestStock.Method = Method.POST;
            requestStock.AddHeader("Accept", "application/json");
            requestStock.AddHeader("authorization",
                HashString(endPointStock + "|" + JsonConvert.SerializeObject(dic_jsonStock),
                secretKey));

            var clientStock = new RestClient(endPointStock);

            try
            {
                IRestResponse responseStock = clientStock.Execute(requestStock);
                var contentStock = responseStock.Content;
                rtn = true;
            }
            catch
            {
                rtn = false;
            }

            return rtn;
        }
        public bool UpdatePrice(long itemId, decimal price, long partner_id, long shopid, string secretKey)
        {
            long timeStamp = getUnixTime();
            bool rtn = false;
            Dictionary<string, object> dic_jsonStock = new Dictionary<string, object>();
            dic_jsonStock.Add("item_id", itemId);
            dic_jsonStock.Add("price", price);
            dic_jsonStock.Add("partner_id", partner_id);
            dic_jsonStock.Add("shopid", shopid);
            dic_jsonStock.Add("timestamp", timeStamp);

            string endPointStock = "https://partner.shopeemobile.com/api/v1/items/update_price";
            var requestStock = new RestRequest("", RestSharp.Method.POST);

            var jsonString = JsonConvert.SerializeObject(dic_jsonStock);
            requestStock.AddJsonBody(jsonString);
            requestStock.Method = Method.POST;
            requestStock.AddHeader("Accept", "application/json");
            requestStock.AddHeader("authorization",
                HashString(endPointStock + "|" + jsonString,
                secretKey));

            var clientStock = new RestClient(endPointStock);

            try
            {
                IRestResponse responseStock = clientStock.Execute(requestStock);
                var contentStock = responseStock.Content;
                rtn = true;
            }
            catch
            {
                rtn = false;
            }

            return rtn;
        }

        public bool UpdateVariationPrice(long item_id, long variation_id, decimal price, long partner_id, long shopid, string secret_key)
        {
            bool rtn = true;
            string convert_price = string.Format("{0:0.##}", price);
            int int_val = 0;
            bool is_int = int.TryParse(convert_price, out int_val);

            long ltime_stamp = ToUnixTime(DateTime.Now.AddHours(-9));
            string end_point = "https://partner.shopeemobile.com/api/v1/items/update_variation_price";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("item_id", item_id);
            dic.Add("variation_id", variation_id);
            if (is_int)
            {
                dic.Add("price", int_val);
            }
            else
            {
                dic.Add("price", price);
            }
            dic.Add("partner_id", partner_id);
            dic.Add("shopid", shopid);
            dic.Add("timestamp", ltime_stamp);


            var client = new RestClient(end_point);
            var request = new RestRequest("", Method.POST);
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(JsonConvert.SerializeObject(dic));

            request.AddHeader("authorization",
                        HashString(end_point + "|" + JsonConvert.SerializeObject(dic),
                        secret_key));
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
                    rtn = false;
                }
            }

            return rtn;
        }
        public dynamic UpdateItem(ItemInfo itemData, List<shopee_attribute> ListItemAttribute, long partner_id, long shop_id, string secretKey)
        {
            long timeStamp = getUnixTime();
            dynamic rtn;
            string endPoint = "https://partner.shopeemobile.com/api/v1/item/update";

            DateTime dtCurrencyDate = DateTime.Now;

            Dictionary<string, object> dic_json = new Dictionary<string, object>();
            var request = new RestRequest("", RestSharp.Method.POST);
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");

            if (ListItemAttribute.Count > 0)
            {
                string pSku = "";
                if(itemData.item_sku == null)
                {
                    pSku = " ";
                }
                else
                {
                    pSku = itemData.item_sku;
                }
                dic_json.Add("item_id", itemData.item_id);
                dic_json.Add("category_id", itemData.category_id);
                dic_json.Add("name", itemData.name);
                dic_json.Add("description", itemData.description);

                if(pSku.Trim() != string.Empty)
                {
                    dic_json.Add("item_sku", pSku);
                }
                else
                {
                    dic_json.Add("item_sku", " ");
                }
                
                dic_json.Add("attributes", ListItemAttribute);
                dic_json.Add("days_to_ship", itemData.days_to_ship);
                dic_json.Add("weight", itemData.weight);
                dic_json.Add("partner_id", partner_id);
                dic_json.Add("shopid", shop_id);
                dic_json.Add("timestamp", timeStamp);

                request.AddJsonBody(JsonConvert.SerializeObject(dic_json));
            }
            else
            {
                dic_json.Add("item_id", itemData.item_id);
                dic_json.Add("category_id", itemData.category_id);
                dic_json.Add("name", itemData.name);
                dic_json.Add("description", itemData.description);
                if (itemData.item_sku != string.Empty)
                {
                    dic_json.Add("item_sku", " ");
                }
                else
                {
                    dic_json.Add("item_sku", "");
                }
                dic_json.Add("item_sku", itemData.item_sku);
                dic_json.Add("days_to_ship", itemData.days_to_ship);
                dic_json.Add("weight", itemData.weight);
                dic_json.Add("partner_id", partner_id);
                dic_json.Add("shopid", shop_id);
                dic_json.Add("timestamp", timeStamp);

                request.AddJsonBody(JsonConvert.SerializeObject(dic_json));
            }


            request.AddHeader("authorization",
                HashString(endPoint + "|" + JsonConvert.SerializeObject(dic_json),
                secretKey));

            try
            {
                var client = new RestClient(endPoint);
                IRestResponse response = client.Execute(request);
                dynamic result = JsonConvert.DeserializeObject(response.Content);

                rtn = result;                
            }
            catch (Exception)
            {
                rtn = null;
            }

            return rtn;
        }
        public void UpdateTierVariationList(long item_id, long partner_id,
            long shopid, string secret_key, List<string> lstOptions, List<string> lstImageUrl)
        {
            //옵션의 목록을 갱신해 준다.
            List<tier_variation> lst_tier_var = new List<tier_variation>();

            tier_variation tv = new tier_variation
            {
                name = "Variation",
                options = lstOptions.ToArray()
            };
            lst_tier_var.Add(tv);


            long long_time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));

            Dictionary<string, object> dic_tier_json = new Dictionary<string, object>();
            dic_tier_json.Add("item_id", item_id);
            dic_tier_json.Add("tier_variation", lst_tier_var.ToArray());
            dic_tier_json.Add("partner_id", partner_id);
            dic_tier_json.Add("shopid", shopid);
            dic_tier_json.Add("timestamp", long_time_stamp);
            
            var end_point = "https://partner.shopeemobile.com/api/v1/item/tier_var/update_list";

            var jsonObject = JsonConvert.SerializeObject(dic_tier_json);

            var request = new RestRequest("", Method.POST);
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(jsonObject);

            
            request.AddHeader("authorization",
                HashString(end_point + "|" + jsonObject,
                secret_key));
            var client = new RestClient(end_point);
            IRestResponse response = client.Execute(request);
            var content = response.Content;

            //[0,0]을 제외한 나머지 부분의 수량들을 채운다.
            //각 옵션의 나머지 데이터를 채워 준다.

        }
        public bool DeleteDiscountItem(long discount_id, long item_id, long variation_id, long partner_id,
            long shopid, string secret_key)
        {
            bool rtn = true;
            //할인 이벤트에 설정된 아이템을 삭제한다.
            long time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));
            string end_point = "https://partner.shopeemobile.com/api/v1/discount/item/delete";


            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (variation_id == 0)
            {   
                dic.Add("discount_id", discount_id);
                dic.Add("item_id", item_id);
                dic.Add("partner_id", partner_id);
                dic.Add("shopid", shopid);
                dic.Add("timestamp", time_stamp);
            }
            else
            {
                dic.Add("discount_id", discount_id);
                dic.Add("item_id", item_id);
                dic.Add("variation_id", variation_id);
                dic.Add("partner_id", partner_id);
                dic.Add("shopid", shopid);
                dic.Add("timestamp", time_stamp);
            }

            var client = new RestClient(end_point);
            var request = new RestRequest("", Method.POST);
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(JsonConvert.SerializeObject(dic));

            request.AddHeader("authorization",
                    HashString(end_point + "|" + JsonConvert.SerializeObject(dic),
                    secret_key));
            IRestResponse response = client.Execute(request);

            dynamic result = JsonConvert.DeserializeObject(response.Content);

            if(result.error != null && result.error.ToString().Contains("error_model_remove_model_in_promotion"))
            {
                rtn = false;
            }           
            else
            {
                rtn = true;
            }

            return rtn;
        }

        public bool AddDiscountItem(long discount_id, List<PromotionItem> promotionItem, long partner_id,
            long shopid, string secret_key)
        {
            bool rtn = false;
            //할인 이벤트에 설정된 아이템을 삭제한다.
            long time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));
            string end_point = "https://partner.shopeemobile.com/api/v1/discount/items/add";

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("discount_id", discount_id);
            dic.Add("items", promotionItem);
            dic.Add("partner_id", partner_id);
            dic.Add("shopid", shopid);
            dic.Add("timestamp", time_stamp);

            var client = new RestClient(end_point);
            var request = new RestRequest("", Method.POST);
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");

            var jsonString = 
                JsonConvert.SerializeObject(
                dic, 
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            request.AddJsonBody(jsonString);
            request.AddHeader("authorization",
                    HashString(end_point + "|" + jsonString,
                    secret_key));
            IRestResponse response = client.Execute(request);

            dynamic result = JsonConvert.DeserializeObject(response.Content);

            if (result.error != null && result.error.ToString().Contains("error_not_exists"))
            {
                rtn = false;
            }
            else
            {
                rtn = true;
            }

            return rtn;
        }
        public bool DeleteVariationAll(dynamic result_tier_info, long item_id, long partner_id, long shopid, string secret_key)
        {
            bool rtn = false;   
            //0,0을 제외한 모든 옵션을 지운다
            //0,0의 variation_id를 리턴한다.
            string end_point_delvari = "https://partner.shopeemobile.com/api/v1/item/delete_variation";
            if(result_tier_info.variations != null)
            {
                for (int i = 0; i < result_tier_info.variations.Count; i++)
                {
                    //옵션 삭제
                    long long_time_stamp_delvari = ToUnixTime(DateTime.Now.AddHours(-9));
                    Dictionary<string, object> dic_json_delvari = new Dictionary<string, object>();
                    dic_json_delvari.Add("item_id", item_id);
                    dic_json_delvari.Add("variation_id", Convert.ToUInt64(result_tier_info.variations[i].variation_id));
                    dic_json_delvari.Add("partner_id", partner_id);
                    dic_json_delvari.Add("shopid", shopid);
                    dic_json_delvari.Add("timestamp", long_time_stamp_delvari);

                    var request_delvari = new RestRequest("", Method.POST);
                    request_delvari.Method = Method.POST;
                    request_delvari.AddHeader("Accept", "application/json");
                    request_delvari.AddJsonBody(new
                    {
                        item_id = item_id,
                        variation_id = Convert.ToUInt64(result_tier_info.variations[i].variation_id),
                        partner_id = partner_id,
                        shopid = shopid,
                        timestamp = long_time_stamp_delvari
                    });

                    request_delvari.AddHeader("authorization",
                        HashString(end_point_delvari + "|" + JsonConvert.SerializeObject(dic_json_delvari),
                        secret_key));
                    var client_delvari = new RestClient(end_point_delvari);
                    IRestResponse response = client_delvari.Execute(request_delvari);

                    dynamic result = JsonConvert.DeserializeObject(response.Content);

                    if (result.error != null && result.error.ToString().Contains("error_model_remove_model_in_promotion"))
                    {
                        rtn = false;
                    }
                    else
                    {
                        rtn = true;
                    }
                }
            }
            

            return rtn;
        }
    }
}
