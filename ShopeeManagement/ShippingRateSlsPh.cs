using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ShippingRateSlsPh
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idx { get; set; }
        public int Weight { get; set; }
        public decimal ZoneA { get; set; }
        public decimal ZoneB { get; set; }
        public decimal ZoneC { get; set; }
        public decimal HiddenFee { get; set; }
    }
}
