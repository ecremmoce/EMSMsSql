using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ItemWholesale
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idx { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("ItemInfo")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long item_id { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("ItemInfo")]
        [StringLength(150)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public decimal unit_price { get; set; }
        
        public virtual ItemInfo ItemInfo { get; set; }
    }
}
