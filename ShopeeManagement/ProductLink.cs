using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ProductLink
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idx { get; set; }
        
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(150)]
        public string UserId { get; set; }
        public string SourceCountry { get; set; }
        public long SourceProductId { get; set; }
        public string SourceAccount { get; set; }
        public string TargetCountry { get; set; }
        public long TargetProductId { get; set; }
        public string TargetAccount { get; set; }
    }
}
