using System.Collections.Generic;

namespace ShopeeManagement
{
    public class Auth0ErrorLogin
    {
        public IList<Auth0ErrorLoginIds> Ids { get; set; }
    }

    public class Auth0ErrorLoginIds
    {
        public string LoginId { get; set; }

        public string LoginPw { get; set; }
    }
}
