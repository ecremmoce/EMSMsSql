using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class CurrencyRate
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idx { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        public string cr_code { get; set; }
        public string cr_name { get; set; }
        public decimal cr_money_buy { get; set; }
        public decimal cr_money_sell { get; set; }
        public decimal cr_transfer_send { get; set; }
        public decimal cr_transfer_receive { get; set; }
        public decimal cr_check_sell { get; set; }
        public decimal cr_exchange { get; set; }
        public decimal cr_dollar_rate { get; set; }
        public DateTime cr_save_date { get; set; }
    }
}
