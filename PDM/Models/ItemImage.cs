using System.ComponentModel.DataAnnotations.Schema;

namespace PDM.Models
{
    public class ItemImage : BaseEntity
    {

        public bool Download { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        public int ItemId { get; set; }
    }
}
