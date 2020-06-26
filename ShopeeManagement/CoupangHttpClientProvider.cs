using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.TimeSpan;

namespace ShopeeManagement
{
    class CoupangHttpClientProvider : ShopHttpClientProvider
    {
        private readonly bool _useProxy;
        private readonly IEnumerable<string> _proxies;

        public CoupangHttpClientProvider(bool useProxy, IEnumerable<string> proxies)
        {
            _useProxy = useProxy;
            _proxies = proxies;
        }

        public override HttpClient NewHttpClient()
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None
            };

            if (_useProxy)
            {
                //handler.UseProxy = true;
                handler.UseProxy = _useProxy;


                handler.Proxy = new WebProxy($"http://{_proxies.PickRandom()}", false);
            }

            var client = new HttpClient(handler)
            {
                Timeout = FromSeconds(30)
            };

            client.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Accept-Language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
            client.DefaultRequestHeaders.Add("Referer", "http://www.coupang.com/");




            return client;
        }
    }
}
