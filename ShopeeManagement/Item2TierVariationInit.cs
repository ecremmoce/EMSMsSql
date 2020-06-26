using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class Item2TierVariationInit
    {
        public long item_id { get; set; }
        public List<tier_variation> tier_variation { get; set; }
        public List<variation> variation { get; set; }
    }

    class tier_variation
    {
        public string name { get; set; }
        public string[] options { get; set; }
    }
    
    class variation
    {
        public int[] tier_index { get; set; }
        public int stock { get; set; }
        public object price { get; set; }
        public string variation_sku { get; set; }
    }
}
