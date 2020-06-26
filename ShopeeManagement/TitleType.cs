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
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }
        public string TitleTypeName { get; set; }

        public ICollection<TitleBrand> TitleBrands { get; set; }
        public ICollection<TitleModel> TitleModels { get; set; }
        public ICollection<TitleGroup> TitleGroups { get; set; }
        public ICollection<TitleFeature> TitleFeatures { get; set; }
        public ICollection<TitleOption> TitleOptions { get; set; }
        public ICollection<TitleRelat> TitleRelats { get; set; }
    }
}
