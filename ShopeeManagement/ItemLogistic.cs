using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ItemLogistic
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long logistic_id { get; set; }
        [Key]
        [Column(Order = 1)]
        [InverseProperty("ItemLogistics")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long item_id { get; set; }
        [Key]
        [Column(Order = 2)]
        [InverseProperty("ItemLogistics")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }
        public string logistic_name { get; set; }
        public bool enabled { get; set; }
        public decimal shipping_fee { get; set; }
        public long size_id { get; set; }
        public bool is_free { get; set; }
        public decimal estimated_shipping_fee { get; set; }

        public virtual ItemInfo ItemInfo { get; set; }
    }
}
