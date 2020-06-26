using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ConfigVar
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        public decimal pg_fee { get; set; }
        public decimal shopee_fee { get; set; }
        public decimal payoneer_fee { get; set; }
        public decimal retail_price_rate { get; set; }
        public decimal margin { get; set; }
        public decimal qty { get; set; }
        public int dts { get; set; }
        public decimal source_price { get; set; }
        public double weight { get; set; }
        public int discountQty { get; set; }
    }
}
