using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ShopeeVariationPrice
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idx { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(150)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }
        public string SrcShopeeCountry { get; set; }
        public string SrcShopeeId { get; set; }
        public string TarShopeeCountry { get; set; }
        public string TarShopeeId { get; set; }

        public string productId { get; set; }
        public string productSKU { get; set; }
        public string productName { get; set; }
        
        public string variation_id { get; set; }
        public string variation_sku { get; set; }
        public string variation_name { get; set; }

        public int product_weight { get; set; }
        public decimal TarNetPrice { get; set; }
        public decimal TarRetail_price { get; set; }
    }
}
