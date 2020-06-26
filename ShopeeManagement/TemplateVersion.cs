using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class TemplateVersion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int Id { get; set; }
        public long category_map { get; set; }
        public long attribute_ID { get; set; }
        public long attribute_MY { get; set; }
        public long attribute_SG { get; set; }
        public long attribute_TH { get; set; }
        public long attribute_TW { get; set; }
        public long attribute_PH { get; set; }
        public long attribute_VN { get; set; }
        public long shipping_rate_ID { get; set; }
        public long shipping_rate_MY { get; set; }
        public long shipping_rate_PH { get; set; }
        public long shipping_rate_SG { get; set; }
        public long shipping_rate_TW { get; set; }
        public long shipping_rate_TH { get; set; }
        public long shipping_rate_VN { get; set; }
        public long attribute_name_map { get; set; }

        public long category_id { get; set; }
        public long category_my { get; set; }
        public long category_ph { get; set; }
        public long category_sg { get; set; }
        public long category_th { get; set; }
        public long category_tw { get; set; }
        public long category_vn { get; set; }
    }
}
