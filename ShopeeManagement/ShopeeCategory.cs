using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ShopeeCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idx { get; set; }
        public string id_l1 { get; set; }
        public string id_l2 { get; set; }
        public string id_l3 { get; set; }
        public string my_l1 { get; set; }
        public string my_l2 { get; set; }
        public string my_l3 { get; set; }
        public string ph_l1 { get; set; }
        public string ph_l2 { get; set; }
        public string ph_l3 { get; set; }
        public string sg_l1 { get; set; }
        public string sg_l2 { get; set; }
        public string sg_l3 { get; set; }
        public string th_l1 { get; set; }
        public string th_l2 { get; set; }
        public string th_l3 { get; set; }
        public string tw_l1 { get; set; }
        public string tw_l2 { get; set; }
        public string tw_l3 { get; set; }
        public string vn_l1 { get; set; }
        public string vn_l2 { get; set; }
        public string vn_l3 { get; set; }
        public string cat_id { get; set; }
        public string cat_my { get; set; }
        public string cat_ph { get; set; }
        public string cat_sg { get; set; }
        public string cat_th { get; set; }
        public string cat_tw { get; set; }
        public string cat_vn { get; set; }
    }
}
