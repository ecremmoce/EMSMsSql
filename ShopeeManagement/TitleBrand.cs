using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopeeManagement
{
    class TitleBrand
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
        public string TitleBrandName { get; set; }

        public virtual TitleType TitleType { get; set; }
        
        [InverseProperty("TitleBrand")]
        public ICollection<TitleModel> TitleModels { get; set; }
    }
}
