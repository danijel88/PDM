using PDM.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDM.ViewModels
{
    public class ItemImageViewModel : BaseEntity
    {
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        public int ItemId { get; set; }

        public bool Download { get; set; }
    }
}
