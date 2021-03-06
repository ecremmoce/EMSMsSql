﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopeeManagement
{
    class ItemAttributeDraft
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long attribute_id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("ItemInfoDraft")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemInfoDraftId { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("ItemInfoDraft")]
        [StringLength(150)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }
        public string attribute_name { get; set; }
        public bool is_mandatory { get; set; }
        public string attribute_type { get; set; }
        public string attribute_value { get; set; }
        public long src_item_id { get; set; }
        public string src_shopeeAccount { get; set; }
        public string tar_shopeeAccount { get; set; }
        public virtual ItemInfoDraft ItemInfoDraft { get; set; }
    }
}
