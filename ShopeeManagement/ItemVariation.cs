using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ItemVariation
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long variation_id { get; set; }
        [Key]
        [Column(Order = 1)]
        [InverseProperty("ItemVariations")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long item_id { get; set; }
        [Key]
        [Column(Order = 2)]
        [InverseProperty("ItemVariations")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }
        public string variation_sku { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public string status { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public decimal original_price { get; set; }
        public int discount_id { get; set; }
        public double weight { get; set; }
        public int margin { get; set; }
        public int supplyPrice { get; set; }
        public int targetSellPriceKRW { get; set; }
        public int targetRetailPriceKRW { get; set; }
        public decimal currencyRate { get; set; }
        public decimal pgFee { get; set; }
        public DateTime currencyDate { get; set; }

        public virtual ItemInfo ItemInfo { get; set; }
    }
}
