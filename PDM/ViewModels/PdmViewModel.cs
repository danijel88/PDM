using PDM.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDM.ViewModels
{
    public class PdmViewModel : BaseEntity
    {
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        public int ItemId { get; set; }
    }
}
