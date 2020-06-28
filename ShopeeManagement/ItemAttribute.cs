using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ItemAttribute
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long attribute_id { get; set; }
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

        public string attribute_name { get; set; }
        public bool is_mandatory { get; set; }
        public string attribute_type { get; set; }
        public string attribute_value { get; set; }
        
        public virtual ItemInfo ItemInfo { get; set; }
    }
}
