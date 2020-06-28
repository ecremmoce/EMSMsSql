using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ItemVariationDraft
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long variation_id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("ItemInfoDraft")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemInfoDraftId { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("ItemInfoDraft")]
        [StringLength(150)]
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
        public long src_item_id { get; set; }
        public string src_shopeeAccount { get; set; }
        public string tar_shopeeAccount { get; set; }
        public decimal supply_price { get; set; }
        public bool isChanged { get; set; }
        public decimal margin { get; set; }
        public double weight { get; set; }
        public int targetSellPriceKRW { get; set; }
        public int targetRetailPriceKRW { get; set; }
        public decimal currencyRate { get; set; }
        public decimal pgFee { get; set; }
        public DateTime currencyDate { get; set; }
        public virtual ItemInfoDraft ItemInfoDraft { get; set; }
    }
}
