using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class global_var
    {
        public static string auth0_accessToken = "TZu0LGPGIkoX_Z6XgYCOccNfkH7JuhACy5Na0B-yePKA0568hgJmZ2nH3MyRAiJp";
        public static DateTime auth0_expire_date;
        public static string str_dbcon = string.Empty;
        public static int colorStart = 100;
        public static int colorEnd = 255;
        public static string image_root = "https://ssproxy2.ucloudbiz.olleh.com/v1/AUTH_8556e13c15264137a6a9e78811845b46/sto_sm";
        public static string kt_token;
        public static DateTime kt_token_exp;
        public static string ucloudbiz_account = "";
        public static string ucloudbiz_account_json = "";
        public static string userId = "";
    }
}
