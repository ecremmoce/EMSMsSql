using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ItemInfo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long item_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(150)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        public string shopeeAccount { get; set; }
        public long shopid { get; set; }
        public string item_sku { get; set; }
        public string status { get; set; }
        public string name { get; set; }
        public string description { get; set; }        
        public string currency { get; set; }
        public bool has_variation { get; set; }
        public decimal price { get; set; }
        public decimal shopeeFee { get; set; }
        public decimal pgFee { get; set; }
        public decimal virtualBankFee { get; set; }
        public int stock { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public double weight { get; set; }
        public long category_id { get; set; }
        public decimal original_price { get; set; }
        public float rating_star { get; set; }
        public int cmt_count { get; set; }
        public int sales { get; set; }
        public int views { get; set; }
        public int likes { get; set; }
        public float package_length { get; set; }
        public float package_width { get; set; }
        public float package_height { get; set; }
        public int days_to_ship { get; set; }
        public string size_chart { get; set; }
        public string condition { get; set; }
        public long discount_id { get; set; }
        public string discount_name { get; set; }
        public bool is_2tier_item { get; set; }
        public string images { get; set; }
        public decimal supply_price { get; set; }
        public decimal margin { get; set; }
        public bool isChanged { get; set; }
        public int SetHeaderId { get; set; }
        public int SetFooterId { get; set; }
        public string SetHeaderName { get; set; }
        public string SetFooterName { get; set; }
        public int targetSellPriceKRW { get; set; }
        public int targetRetailPriceKRW { get; set; }
        public decimal currencyRate { get; set; }
        public DateTime currencyDate { get; set; }
        public DateTime savedDate { get; set; }

        [InverseProperty("ItemInfo")]
        public ICollection<ItemAttribute> ItemAttributes { get; set; }
        [InverseProperty("ItemInfo")]
        //public ICollection<ItemImage> ItemImages { get; set; }
        public ICollection<ItemLogistic> ItemLogistics { get; set; }
        [InverseProperty("ItemInfo")]
        public ICollection<ItemVariation> ItemVariations { get; set; }
        [InverseProperty("ItemInfo")]
        public ICollection<ItemWholesale> ItemWholesales { get; set; }
    }
}
