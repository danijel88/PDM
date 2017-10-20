using System.ComponentModel.DataAnnotations.Schema;

namespace PDM.Models
{
    public class Proposal : BaseEntity
    {
        public decimal Enter { get; set; }

        public decimal Exit { get; set; }

        public decimal Thickness { get; set; }

        public int Band { get; set; }

        public bool Elastic { get; set; }

        public int ItemId { get; set; }

        public string Notes { get; set; }

        public Status Status { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
    }
}
