using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    public class Auth0ErrorLoginMAC
    {
        [Key]
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string MAC { get; set; }
    }
}
