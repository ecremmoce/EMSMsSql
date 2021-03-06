﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ShopeeAccount
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
        public string ShopeeCountry { get; set; }
        public string ShopeeId { get; set; }
        public string PartnerId { get; set; }
        public string ShopId { get; set; }
        public string SecretKey { get; set; }
    }
}
