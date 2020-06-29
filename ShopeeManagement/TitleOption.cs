using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class TitleOption
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(Order = 1)]
        [ForeignKey("TitleType")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TitleTypeId { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("TitleType")]
        [StringLength(150)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }
        public string TitleOptionName { get; set; }

        public virtual TitleType TitleType { get; set; }
    }
}
