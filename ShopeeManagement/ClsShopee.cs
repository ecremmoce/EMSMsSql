using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ClsShopee
    {
        public string EndPoint { get; set; }
        public IList<ShopeeAccount> GetShopeeAccountList()
        {
            using (AppDbContext context = new AppDbContext())
            {
                List<ShopeeAccount> accountList = context.ShopeeAccounts.Where(x => x.UserId == global_var.userId).OrderBy(x => x.ShopeeCountry).ToList();
                return accountList;
            }
        }
        public dynamic GetDiscountsList(string discount_status, int pagination_offset, int pagination_entries_per_page,
            long partner_id, long shop_id, string api_key)
        {
            long time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));
            string end_point = "https://partner.shopeemobile.com/api/v1/discounts/get";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("discount_status", discount_status);
            dic.Add("pagination_offset", pagination_offset);
            dic.Add("pagination_entries_per_page", pagination_entries_per_page);
            dic.Add("partner_id", partner_id);
            dic.Add("shopid", shop_id);
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
            return result;
        }
        private static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
        public DataTable get_order_list(long partner_id, long shop_id, DateTime from_time, DateTime to_time, DateTime ts, string secret_key, string seller_id)
        {
            long long_from_time = ToUnixTime(from_time);
            long long_to_time = ToUnixTime(to_time);
            long long_time_stamp = ToUnixTime(ts);
            //string epo_create_time_from = ToUnixTime(create_time_from);
            EndPoint = "https://partner.shopeemobile.com/api/v1/orders/basics";
            var client = new RestSharp.RestClient(EndPoint);

            //테스트 데이터
            //long_from_time = 1485597948;
            //long_to_time = 1486116348;
            //long_time_stamp = 1487203199;
            //partner_id = 10002;
            //shop_id = 204428;

            Dictionary<string, long> dic_json = new Dictionary<string, long>();
            dic_json.Add("create_time_from", long_from_time);
            dic_json.Add("create_time_to", long_to_time);
            dic_json.Add("pagination_entries_per_page", 100);
            dic_json.Add("pagination_offset", 0);
            dic_json.Add("partner_id", partner_id);
            dic_json.Add("shopid", shop_id);
            dic_json.Add("timestamp", long_time_stamp);

            string body = JsonConvert.SerializeObject(dic_json);
            string auth_str = EndPoint + "|" + body;
            string authorization = HashString(auth_str, secret_key);
            var request = new RestSharp.RestRequest("", RestSharp.Method.POST);
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(JsonConvert.SerializeObject(dic_json));
            request.AddHeader("authorization", authorization);

            RestSharp.IRestResponse response = client.Execute(request);
            var content = response.Content;

            DataTable dt = new DataTable();

            dynamic result = JsonConvert.DeserializeObject(content);
            List<string> lst_order = new List<string>();
            Dictionary<string, string> dic_ord = new Dictionary<string, string>();

            //첫페이지는 무조건 가지고 온다.
            for (int i = 0; i < result.orders.Count; i++)
            {
                lst_order.Add(result.orders[i].ordersn.Value.ToString());
                dic_ord.Add(result.orders[i].ordersn.Value.ToString(), "aaa");
            }


            if (result != null && result.more != null)
            {
                if (result.more.Value)
                {
                    bool parseNext = true;
                    int page = 10;
                    while (parseNext)
                    {
                        dynamic result_next = get_order_list_next_page(partner_id, shop_id,
                        from_time, to_time, ts, secret_key, seller_id, page);

                        if (result_next.more.Value)
                        {
                            parseNext = true;
                        }
                        else
                        {
                            parseNext = false;
                        }
                        for (int i = 0; i < result_next.orders.Count; i++)
                        {
                            if (!dic_ord.ContainsKey(result_next.orders[i].ordersn.Value.ToString()))
                            {
                                lst_order.Add(result_next.orders[i].ordersn.Value.ToString());
                                dic_ord.Add(result_next.orders[i].ordersn.Value.ToString(), "aaa");
                            }
                        }
                        page = page + 10;
                    }

                }
                else
                {

                }


                if (lst_order.Count > 0)
                {
                    dt = get_order_detail(lst_order, partner_id, shop_id, long_time_stamp, secret_key, seller_id);
                }
            }
            try
            {

            }
            catch (Exception ex)
            {

            }


            return dt;
        }

        public dynamic get_order_list_next_page(long partner_id, long shop_id, DateTime from_time, DateTime to_time,
            DateTime ts, string secret_key, string seller_id, int page)
        {
            long long_from_time = ToUnixTime(from_time);
            long long_to_time = ToUnixTime(to_time);
            long long_time_stamp = ToUnixTime(DateTime.Now.AddHours(-9));
            //string epo_create_time_from = ToUnixTime(create_time_from);
            EndPoint = "https://partner.shopeemobile.com/api/v1/orders/basics";
            var client = new RestSharp.RestClient(EndPoint);

            //테스트 데이터
            //long_from_time = 1485597948;
            //long_to_time = 1486116348;
            //long_time_stamp = 1487203199;
            //partner_id = 10002;
            //shop_id = 204428;

            Dictionary<string, long> dic_json = new Dictionary<string, long>();
            dic_json.Add("create_time_from", long_from_time);
            dic_json.Add("create_time_to", long_to_time);
            dic_json.Add("pagination_entries_per_page", 100);
            dic_json.Add("pagination_offset", page);
            dic_json.Add("partner_id", partner_id);
            dic_json.Add("shopid", shop_id);
            dic_json.Add("timestamp", long_time_stamp);


            string body = JsonConvert.SerializeObject(dic_json);
            string auth_str = EndPoint + "|" + body;
            string authorization = HashString(auth_str, secret_key);
            var request = new RestSharp.RestRequest("", RestSharp.Method.POST);
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(JsonConvert.SerializeObject(dic_json));
            request.AddHeader("authorization", authorization);

            RestSharp.IRestResponse response = client.Execute(request);
            var content = response.Content;
            dynamic result = JsonConvert.DeserializeObject(content);

            return result;
        }

        public IList<ShopeeCategory> GetShopeeCategoryList()
        {
            using (AppDbContext context = new AppDbContext())
            {
                List<ShopeeCategory> categoryList = context.ShopeeCategories.OrderBy(x => x.Idx).ToList();
                return categoryList;
            }
        }

        public IList<ShippingRateSlsSg> GetShippingRateSG()
        {
            using (AppDbContext context = new AppDbContext())
            {
                List<ShippingRateSlsSg> rateList = context.ShippingRateSlsSgs.OrderBy(x => x.Weight).ToList();
                return rateList;
            }
        }

        public IList<ShippingRateSlsMy> GetShippingRateMY()
        {
            using (AppDbContext context = new AppDbContext())
            {
                List<ShippingRateSlsMy> rateList = context.ShippingRateSlsMys.OrderBy(x => x.Weight).ToList();
                return rateList;
            }
        }
        public IList<ShippingRateDRTh> GetShippingRateTH()
        {
            using (AppDbContext context = new AppDbContext())
            {
                List<ShippingRateDRTh> rateList = context.ShippingRateDRThs.OrderBy(x => x.Weight).ToList();
                return rateList;
            }
        }

        public IList<ShippingRateYTOTw> GetShippingRateTW()
        {
            using (AppDbContext context = new AppDbContext())
            {
                List<ShippingRateYTOTw> rateList = context.ShippingRateYTOTws.OrderBy(x => x.Weight).ToList();
                return rateList;
            }
        }

        public IList<ShippingRateSlsId> GetShippingRateID()
        {
            using (AppDbContext context = new AppDbContext())
            {
                List<ShippingRateSlsId> rateList = context.ShippingRateSlsIds.OrderBy(x => x.Weight).ToList();
                return rateList;
            }
        }
        public IList<ShippingRateSlsPh> GetShippingRatePH()
        {
            using (AppDbContext context = new AppDbContext())
            {
                List<ShippingRateSlsPh> rateList = context.ShippingRateSlsPhs.OrderBy(x => x.Weight).ToList();
                return rateList;
            }
        }
        public IList<ShippingRateDRVn> GetShippingRateVN()
        {
            using (AppDbContext context = new AppDbContext())
            {
                List<ShippingRateDRVn> rateList = context.ShippingRateDRVns.OrderBy(x => x.Weight).ToList();
                return rateList;
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
            hash = hash.ToLower();

            return hash.ToLower();
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

        public dynamic shopee_order_detail(List<string> ordersn_list, long long_partner_id, long long_shopid,
            long long_timestamp, string secret_key, string seller_id)
        {
            EndPoint = "https://partner.shopeemobile.com/api/v1/orders/detail";
            var client = new RestSharp.RestClient(EndPoint);
            var kk = JsonConvert.SerializeObject(ordersn_list, Formatting.None);
            StringBuilder sb_lst = new StringBuilder();
            sb_lst.Append("[");
            for (int i = 0; i < ordersn_list.Count; i++)
            {
                if (i == ordersn_list.Count - 1)
                {
                    sb_lst.Append("\"" + ordersn_list[i].ToString() + "\"");
                }
                else
                {
                    sb_lst.Append("\"" + ordersn_list[i].ToString() + "\",");
                }
            }
            sb_lst.Append("]");
            string str_body = "{\"ordersn_list\":" + sb_lst.ToString() + ",\"partner_id\":" + long_partner_id.ToString() + ",\"shopid\":" + long_shopid.ToString() + ",\"timestamp\":" + long_timestamp.ToString() + "}";
            string auth_str = EndPoint + "|" + str_body;
            string authorization = HashString(auth_str, secret_key);
            var request = new RestSharp.RestRequest("", RestSharp.Method.POST);
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");

            request.AddJsonBody(new { ordersn_list = ordersn_list, partner_id = long_partner_id, shopid = long_shopid, timestamp = long_timestamp });
            request.AddHeader("authorization", authorization);

            RestSharp.IRestResponse response = client.Execute(request);
            var content = response.Content;

            dynamic result = JsonConvert.DeserializeObject(content);
            return result;
        }
        public DataTable get_order_detail(List<string> ordersn_list, long long_partner_id, long long_shopid, long long_timestamp, string secret_key, string seller_id)
        {
            DataTable dt = new DataTable("order_data");
            dt.Columns.Add("No", typeof(string));
            dt.Columns.Add("V", typeof(bool));
            dt.Columns.Add("셀러ID", typeof(string));
            dt.Columns.Add("주문일자", typeof(string));
            dt.Columns.Add("수정일자", typeof(string));
            dt.Columns.Add("주문번호", typeof(string));
            dt.Columns.Add("상품명", typeof(string));
            dt.Columns.Add("SKU", typeof(string));
            dt.Columns.Add("옵션명", typeof(string));
            dt.Columns.Add("수량", typeof(string));
            dt.Columns.Add("총액", typeof(string));
            dt.Columns.Add("배송비", typeof(string));
            dt.Columns.Add("총지불액", typeof(string));
            dt.Columns.Add("실배송비", typeof(string));
            dt.Columns.Add("국가", typeof(string));
            dt.Columns.Add("배송사", typeof(string));
            dt.Columns.Add("송장번호", typeof(string));
            dt.Columns.Add("상태", typeof(string));
            dt.Columns.Add("지불방법", typeof(string));
            dt.Columns.Add("메시지", typeof(string));
            dt.Columns.Add("배송기간", typeof(string));
            dt.Columns.Add("통화", typeof(string));
            dt.Columns.Add("cod", typeof(string));
            dt.Columns.Add("신고상품", typeof(string));
            dt.Columns.Add("옵션할인가", typeof(string));
            dt.Columns.Add("옵션ID", typeof(string));
            dt.Columns.Add("상품ID", typeof(string));
            dt.Columns.Add("옵션상품가", typeof(string));
            dt.Columns.Add("수신자_타운", typeof(string));
            dt.Columns.Add("수신자_도시", typeof(string));
            dt.Columns.Add("수신자_이름", typeof(string));
            dt.Columns.Add("수신자_구역", typeof(string));
            dt.Columns.Add("수신자_국가", typeof(string));
            dt.Columns.Add("수신자_우편번호", typeof(string));
            dt.Columns.Add("수신자_전체주소", typeof(string));
            dt.Columns.Add("수신자_연락처", typeof(string));
            dt.Columns.Add("수신자_주", typeof(string));
            dt.Columns.Add("partner_id", typeof(string));
            dt.Columns.Add("shopid", typeof(string));
            dt.Columns.Add("secret_key", typeof(string));
            if (ordersn_list.Count > 50)
            {
                List<string> tempList = new List<string>();

                for (int k = 0; k < ordersn_list.Count; k++)
                {
                    if (tempList.Count < 50)
                    {
                        tempList.Add(ordersn_list[k].ToString());

                        //마지막에 도달했을 경우
                        if (k == ordersn_list.Count - 1)
                        {
                            //목록페이지 호출
                            dynamic result = shopee_order_detail(tempList, long_partner_id, long_shopid, long_timestamp, secret_key, seller_id);
                            for (int i = 0; i < result.orders.Count; i++)
                            {
                                //합포장 때문에 상품을 합쳐줘야 한다. 상품정보만 다중으로 들어오고 나머지는 하나만 들어온다.
                                DataRow dr;
                                dr = dt.NewRow();
                                dr["No"] = i + 1;
                                dr["V"] = false;
                                dr["셀러ID"] = seller_id;
                                dr["주문일자"] = UnixTimeStampToDateTime(Convert.ToInt64(result.orders[i].create_time));
                                dr["수정일자"] = UnixTimeStampToDateTime(Convert.ToInt64(result.orders[i].update_time));
                                dr["주문번호"] = result.orders[i].ordersn;
                                //MessageBox.Show(result.orders[i].items[0].item_name.ToString());
                                //MessageBox.Show(result.orders[i].items.Count.ToString());
                                for (int j = 0; j < result.orders[i].items.Count; j++)
                                {
                                    if (j == 0)
                                    {
                                        dr["상품명"] = result.orders[i].items[j].item_name;
                                        dr["SKU"] = result.orders[i].items[j].item_sku;
                                        dr["옵션명"] = result.orders[i].items[j].variation_sku;
                                        dr["수량"] = result.orders[i].items[j].variation_quantity_purchased;
                                        dr["옵션할인가"] = result.orders[i].items[j].variation_discounted_price;
                                        dr["옵션ID"] = result.orders[i].items[j].variation_id;
                                        dr["옵션명"] = result.orders[i].items[j].variation_name;
                                        dr["상품ID"] = result.orders[i].items[j].item_id;
                                        dr["옵션상품가"] = result.orders[i].items[j].variation_original_price;
                                    }
                                    else
                                    {
                                        dr["상품명"] = dr["상품명"].ToString() + "\r\n" + result.orders[i].items[j].item_name;
                                        dr["SKU"] = dr["SKU"].ToString() + "\r\n" + result.orders[i].items[j].item_sku;
                                        dr["옵션명"] = dr["옵션명"].ToString() + "\r\n" + result.orders[i].items[j].variation_sku;
                                        dr["수량"] = dr["수량"].ToString() + "\r\n" + result.orders[i].items[j].variation_quantity_purchased;
                                        dr["옵션할인가"] = dr["옵션할인가"].ToString() + "\r\n" + result.orders[i].items[j].variation_discounted_price;
                                        dr["옵션ID"] = dr["옵션ID"].ToString() + "\r\n" + result.orders[i].items[j].variation_id;
                                        dr["옵션명"] = dr["옵션명"].ToString() + "\r\n" + result.orders[i].items[j].variation_name;
                                        dr["상품ID"] = dr["상품ID"].ToString() + "\r\n" + result.orders[i].items[j].item_id;
                                        dr["옵션상품가"] = dr["옵션상품가"].ToString() + "\r\n" + result.orders[i].items[j].variation_original_price;
                                    }
                                }

                                dr["총액"] = result.orders[i].total_amount;
                                dr["배송비"] = result.orders[i].estimated_shipping_fee;
                                dr["총지불액"] = result.orders[i].escrow_amount;
                                dr["실배송비"] = result.orders[i].actual_shipping_cost;
                                dr["국가"] = result.orders[i].country;
                                dr["배송사"] = result.orders[i].shipping_carrier;
                                dr["송장번호"] = result.orders[i].tracking_no;
                                dr["상태"] = result.orders[i].order_status;
                                dr["지불방법"] = result.orders[i].payment_method;
                                dr["메시지"] = result.orders[i].message_to_seller;
                                dr["배송기간"] = result.orders[i].days_to_ship;
                                dr["통화"] = result.orders[i].currency;
                                dr["cod"] = result.orders[i].cod;
                                dr["신고상품"] = result.orders[i].goods_to_declare;
                                dr["수신자_타운"] = result.orders[i].recipient_address.town;
                                dr["수신자_도시"] = result.orders[i].recipient_address.city;
                                dr["수신자_이름"] = result.orders[i].recipient_address.name;
                                dr["수신자_구역"] = result.orders[i].recipient_address.district;
                                dr["수신자_국가"] = result.orders[i].recipient_address.country;
                                dr["수신자_우편번호"] = result.orders[i].recipient_address.zipcode;
                                dr["수신자_전체주소"] = result.orders[i].recipient_address.full_address;
                                dr["수신자_연락처"] = result.orders[i].recipient_address.phone;
                                dr["수신자_주"] = result.orders[i].recipient_address.state;
                                dr["partner_id"] = long_partner_id.ToString();
                                dr["shopid"] = long_shopid.ToString();
                                dr["secret_key"] = secret_key;
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    else
                    {
                        //목록페이지 호출
                        dynamic result = shopee_order_detail(tempList, long_partner_id, long_shopid, long_timestamp, secret_key, seller_id);
                        for (int i = 0; i < result.orders.Count; i++)
                        {
                            //합포장 때문에 상품을 합쳐줘야 한다. 상품정보만 다중으로 들어오고 나머지는 하나만 들어온다.
                            DataRow dr;
                            dr = dt.NewRow();
                            dr["No"] = i + 1;
                            dr["V"] = false;
                            dr["셀러ID"] = seller_id;
                            dr["주문일자"] = UnixTimeStampToDateTime(Convert.ToInt64(result.orders[i].create_time));
                            dr["수정일자"] = UnixTimeStampToDateTime(Convert.ToInt64(result.orders[i].update_time));
                            dr["주문번호"] = result.orders[i].ordersn;
                            //MessageBox.Show(result.orders[i].items[0].item_name.ToString());
                            //MessageBox.Show(result.orders[i].items.Count.ToString());
                            for (int j = 0; j < result.orders[i].items.Count; j++)
                            {
                                if (j == 0)
                                {
                                    dr["상품명"] = result.orders[i].items[j].item_name;
                                    dr["SKU"] = result.orders[i].items[j].item_sku;
                                    dr["옵션명"] = result.orders[i].items[j].variation_sku;
                                    dr["수량"] = result.orders[i].items[j].variation_quantity_purchased;
                                    dr["옵션할인가"] = result.orders[i].items[j].variation_discounted_price;
                                    dr["옵션ID"] = result.orders[i].items[j].variation_id;
                                    dr["옵션명"] = result.orders[i].items[j].variation_name;
                                    dr["상품ID"] = result.orders[i].items[j].item_id;
                                    dr["옵션상품가"] = result.orders[i].items[j].variation_original_price;
                                }
                                else
                                {
                                    dr["상품명"] = dr["상품명"].ToString() + "\r\n" + result.orders[i].items[j].item_name;
                                    dr["SKU"] = dr["SKU"].ToString() + "\r\n" + result.orders[i].items[j].item_sku;
                                    dr["옵션명"] = dr["옵션명"].ToString() + "\r\n" + result.orders[i].items[j].variation_sku;
                                    dr["수량"] = dr["수량"].ToString() + "\r\n" + result.orders[i].items[j].variation_quantity_purchased;
                                    dr["옵션할인가"] = dr["옵션할인가"].ToString() + "\r\n" + result.orders[i].items[j].variation_discounted_price;
                                    dr["옵션ID"] = dr["옵션ID"].ToString() + "\r\n" + result.orders[i].items[j].variation_id;
                                    dr["옵션명"] = dr["옵션명"].ToString() + "\r\n" + result.orders[i].items[j].variation_name;
                                    dr["상품ID"] = dr["상품ID"].ToString() + "\r\n" + result.orders[i].items[j].item_id;
                                    dr["옵션상품가"] = dr["옵션상품가"].ToString() + "\r\n" + result.orders[i].items[j].variation_original_price;
                                }
                            }

                            dr["총액"] = result.orders[i].total_amount;
                            dr["배송비"] = result.orders[i].estimated_shipping_fee;
                            dr["총지불액"] = result.orders[i].escrow_amount;
                            dr["실배송비"] = result.orders[i].actual_shipping_cost;
                            dr["국가"] = result.orders[i].country;
                            dr["배송사"] = result.orders[i].shipping_carrier;
                            dr["송장번호"] = result.orders[i].tracking_no;
                            dr["상태"] = result.orders[i].order_status;
                            dr["지불방법"] = result.orders[i].payment_method;
                            dr["메시지"] = result.orders[i].message_to_seller;
                            dr["배송기간"] = result.orders[i].days_to_ship;
                            dr["통화"] = result.orders[i].currency;
                            dr["cod"] = result.orders[i].cod;
                            dr["신고상품"] = result.orders[i].goods_to_declare;
                            dr["수신자_타운"] = result.orders[i].recipient_address.town;
                            dr["수신자_도시"] = result.orders[i].recipient_address.city;
                            dr["수신자_이름"] = result.orders[i].recipient_address.name;
                            dr["수신자_구역"] = result.orders[i].recipient_address.district;
                            dr["수신자_국가"] = result.orders[i].recipient_address.country;
                            dr["수신자_우편번호"] = result.orders[i].recipient_address.zipcode;
                            dr["수신자_전체주소"] = result.orders[i].recipient_address.full_address;
                            dr["수신자_연락처"] = result.orders[i].recipient_address.phone;
                            dr["수신자_주"] = result.orders[i].recipient_address.state;
                            dr["partner_id"] = long_partner_id.ToString();
                            dr["shopid"] = long_shopid.ToString();
                            dr["secret_key"] = secret_key;
                            dt.Rows.Add(dr);
                        }
                        tempList.Clear();
                        tempList.Add(ordersn_list[k].ToString());
                    }
                }
            }
            else
            {
                EndPoint = "https://partner.shopeemobile.com/api/v1/orders/detail";
                var client = new RestSharp.RestClient(EndPoint);
                var kk = JsonConvert.SerializeObject(ordersn_list, Formatting.None);
                StringBuilder sb_lst = new StringBuilder();
                sb_lst.Append("[");
                for (int i = 0; i < ordersn_list.Count; i++)
                {
                    if (i == ordersn_list.Count - 1)
                    {
                        sb_lst.Append("\"" + ordersn_list[i].ToString() + "\"");
                    }
                    else
                    {
                        sb_lst.Append("\"" + ordersn_list[i].ToString() + "\",");
                    }
                }
                sb_lst.Append("]");
                string str_body = "{\"ordersn_list\":" + sb_lst.ToString() + ",\"partner_id\":" + long_partner_id.ToString() + ",\"shopid\":" + long_shopid.ToString() + ",\"timestamp\":" + long_timestamp.ToString() + "}";
                string auth_str = EndPoint + "|" + str_body;
                string authorization = HashString(auth_str, secret_key);
                var request = new RestSharp.RestRequest("", RestSharp.Method.POST);
                request.Method = Method.POST;
                request.AddHeader("Accept", "application/json");

                request.AddJsonBody(new { ordersn_list = ordersn_list, partner_id = long_partner_id, shopid = long_shopid, timestamp = long_timestamp });
                request.AddHeader("authorization", authorization);

                RestSharp.IRestResponse response = client.Execute(request);
                var content = response.Content;
                if (content.Contains("error_auth"))
                {
                    dt = null;
                }
                else
                {
                    dynamic result = JsonConvert.DeserializeObject(content);



                    for (int i = 0; i < result.orders.Count; i++)
                    {
                        //합포장 때문에 상품을 합쳐줘야 한다. 상품정보만 다중으로 들어오고 나머지는 하나만 들어온다.
                        DataRow dr;
                        dr = dt.NewRow();
                        dr["No"] = i + 1;
                        dr["V"] = false;
                        dr["셀러ID"] = seller_id;
                        dr["주문일자"] = UnixTimeStampToDateTime(Convert.ToInt64(result.orders[i].create_time));
                        dr["수정일자"] = UnixTimeStampToDateTime(Convert.ToInt64(result.orders[i].update_time));
                        dr["주문번호"] = result.orders[i].ordersn;
                        //MessageBox.Show(result.orders[i].items[0].item_name.ToString());
                        //MessageBox.Show(result.orders[i].items.Count.ToString());
                        for (int j = 0; j < result.orders[i].items.Count; j++)
                        {
                            if (j == 0)
                            {
                                dr["상품명"] = result.orders[i].items[j].item_name;
                                dr["SKU"] = result.orders[i].items[j].item_sku;
                                dr["옵션명"] = result.orders[i].items[j].variation_sku;
                                dr["수량"] = result.orders[i].items[j].variation_quantity_purchased;
                                dr["옵션할인가"] = result.orders[i].items[j].variation_discounted_price;
                                dr["옵션ID"] = result.orders[i].items[j].variation_id;
                                dr["옵션명"] = result.orders[i].items[j].variation_name;
                                dr["상품ID"] = result.orders[i].items[j].item_id;
                                dr["옵션상품가"] = result.orders[i].items[j].variation_original_price;
                            }
                            else
                            {
                                dr["상품명"] = dr["상품명"].ToString() + "\r\n" + result.orders[i].items[j].item_name;
                                dr["SKU"] = dr["SKU"].ToString() + "\r\n" + result.orders[i].items[j].item_sku;
                                dr["옵션명"] = dr["옵션명"].ToString() + "\r\n" + result.orders[i].items[j].variation_sku;
                                dr["수량"] = dr["수량"].ToString() + "\r\n" + result.orders[i].items[j].variation_quantity_purchased;
                                dr["옵션할인가"] = dr["옵션할인가"].ToString() + "\r\n" + result.orders[i].items[j].variation_discounted_price;
                                dr["옵션ID"] = dr["옵션ID"].ToString() + "\r\n" + result.orders[i].items[j].variation_id;
                                dr["옵션명"] = dr["옵션명"].ToString() + "\r\n" + result.orders[i].items[j].variation_name;
                                dr["상품ID"] = dr["상품ID"].ToString() + "\r\n" + result.orders[i].items[j].item_id;
                                dr["옵션상품가"] = dr["옵션상품가"].ToString() + "\r\n" + result.orders[i].items[j].variation_original_price;
                            }
                        }

                        dr["총액"] = result.orders[i].total_amount;
                        dr["배송비"] = result.orders[i].estimated_shipping_fee;
                        dr["총지불액"] = result.orders[i].escrow_amount;
                        dr["실배송비"] = result.orders[i].actual_shipping_cost;
                        dr["국가"] = result.orders[i].country;
                        dr["배송사"] = result.orders[i].shipping_carrier;
                        dr["송장번호"] = result.orders[i].tracking_no;
                        dr["상태"] = result.orders[i].order_status;
                        dr["지불방법"] = result.orders[i].payment_method;
                        dr["메시지"] = result.orders[i].message_to_seller;
                        dr["배송기간"] = result.orders[i].days_to_ship;
                        dr["통화"] = result.orders[i].currency;
                        dr["cod"] = result.orders[i].cod;
                        dr["신고상품"] = result.orders[i].goods_to_declare;
                        dr["수신자_타운"] = result.orders[i].recipient_address.town;
                        dr["수신자_도시"] = result.orders[i].recipient_address.city;
                        dr["수신자_이름"] = result.orders[i].recipient_address.name;
                        dr["수신자_구역"] = result.orders[i].recipient_address.district;
                        dr["수신자_국가"] = result.orders[i].recipient_address.country;
                        dr["수신자_우편번호"] = result.orders[i].recipient_address.zipcode;
                        dr["수신자_전체주소"] = result.orders[i].recipient_address.full_address;
                        dr["수신자_연락처"] = result.orders[i].recipient_address.phone;
                        dr["수신자_주"] = result.orders[i].recipient_address.state;
                        dr["partner_id"] = long_partner_id.ToString();
                        dr["shopid"] = long_shopid.ToString();
                        dr["secret_key"] = secret_key;
                        dt.Rows.Add(dr);
                    }
                }
            }





            return dt;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
