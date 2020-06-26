﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class TitleModel
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }
        public string TitleModelName { get; set; }
        //public int TitleTypeId { get; set; }
        public int TitleBrandId { get; set; }
        //public virtual TitleType TitleType { get; set; }
        public virtual TitleBrand TitleBrand { get; set; }
    }
}