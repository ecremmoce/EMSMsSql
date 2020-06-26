using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class PromotionItem
    {
        public long item_id { get; set; }
        public decimal item_promotion_price { get; set; }
        public int purchase_limit { get; set; }
        public string UserId { get; set; }
        public List<PromotionVariations> variations { get; set; }
    }

    class PromotionVariations
    {
        public long variation_id { get; set; }
        public decimal variation_promotion_price { get; set; }
        public string UserId { get; set; }
    }
}
