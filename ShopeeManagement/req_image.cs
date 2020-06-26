using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    public partial class req_image
    {
        public int idx { get; set; }
        public string user_id { get; set; }
        public string site_code { get; set; }
        public System.Guid p_guid { get; set; }
        public string src_addr { get; set; }
        public string save_file_name { get; set; }
        public bool is_fetched { get; set; }
        public bool is_complete { get; set; }
        public System.DateTime create_date { get; set; }
    }
}
