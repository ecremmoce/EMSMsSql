using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class TitleType
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(150)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }
        public string TitleTypeName { get; set; }

        [InverseProperty("TitleType")]
        public ICollection<TitleBrand> TitleBrands { get; set; }
        [InverseProperty("TitleType")]
        public ICollection<TitleGroup> TitleGroups { get; set; }
        [InverseProperty("TitleType")]
        public ICollection<TitleFeature> TitleFeatures { get; set; }
        [InverseProperty("TitleType")]
        public ICollection<TitleOption> TitleOptions { get; set; }
        [InverseProperty("TitleType")]
        public ICollection<TitleRelat> TitleRelats { get; set; }
    }
}
