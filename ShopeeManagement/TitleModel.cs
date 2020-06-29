using System;
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
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(Order = 1)]
        [ForeignKey("TitleBrand")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TitleBrandId { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("TitleBrand")]
        [StringLength(150)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }
        public string TitleModelName { get; set; }
        
        public virtual TitleBrand TitleBrand { get; set; }
    }
}
