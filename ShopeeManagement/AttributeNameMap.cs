using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class AttributeNameMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idx { get; set; }
        public string KR { get; set; }
        public string SG { get; set; }
        public string MY { get; set; }
        public string ID { get; set; }
        public string TH { get; set; }
        public string TW { get; set; }
        public string VN { get; set; }
        public string PH { get; set; }
    }
}
