using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{    
    class HFList
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HFListID { get; set; }

        [Key]
        [Column(Order = 1)]
        [InverseProperty("HFLists")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int HFTypeID { get; set; }

        [Key]
        [Column(Order = 2)]
        [InverseProperty("HFLists")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        public string HFName { get; set; }

        public string HFContent { get; set; }

        public virtual HFType HFType { get; set; }
    }
}
