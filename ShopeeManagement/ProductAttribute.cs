using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ProductAttribute
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        public string tarShopeeAccount { get; set; }
        public long srcProductId { get; set; }
        public int tarAttributeId { get; set; }
        public string tarAttributeName { get; set; }
        public string tarAttributeValue { get; set; }
        public bool isMandatory { get; set; }
    }
}
