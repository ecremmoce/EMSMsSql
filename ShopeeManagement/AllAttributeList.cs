using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class AllAttributeList
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string country { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long category_id { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long attribute_id { get; set; }

        public string attribute_name { get; set; }
        public string attribute_name_kor { get; set; }
        public bool is_mandatory { get; set; }
        public string attribute_type { get; set; }
        public string input_type { get; set; }
        public string options { get; set; }
        public string options_kor { get; set; }
        public string values_original_value { get; set; }
        public string values_translate_value { get; set; }
        public string values_translate_kor { get; set; }
    }
}
