using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class TemplateFooter
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        public string ShopeeAccount { get; set; }
        public int OrderIdx { get; set; }
        public int HFListID { get; set; }
        public string HFName { get; set; }
        public int SetFooterId { get; set; }
        
        public virtual SetFooter SetFooter { get; set; }
    }
}
