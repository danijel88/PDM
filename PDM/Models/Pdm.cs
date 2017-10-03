using System.ComponentModel.DataAnnotations.Schema;

namespace PDM.Models
{
    public class Pdm : BaseEntity
    {
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        public int ItemId { get; set; }
    }
}
