using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ShopeeCategoryOnly
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(150)]
        public string UserId { get; set; }
        public string CatLevel1 { get; set; }
        public string CatLevel2 { get; set; }
        public string CatLevel3 { get; set; }
        public string CatLevel4 { get; set; }
        public string CatLevel5 { get; set; }
        public string CatId { get; set; }
        public string Country { get; set; }
    }
}
