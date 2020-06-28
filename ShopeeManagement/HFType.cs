using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class HFType
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HFTypeID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(150)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        public string HFTypeName { get; set; }
        
        [InverseProperty("HFType")]
        public ICollection<HFList> HFLists { get; set; }
    }
}
