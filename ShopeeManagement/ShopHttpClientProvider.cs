using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    abstract class ShopHttpClientProvider
    {
        public abstract HttpClient NewHttpClient();
    }
}
