using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    public class Auth0ErrorLogin
    {
        [Key]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string LoginId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string LoginPW { get; set; }
    }
}
