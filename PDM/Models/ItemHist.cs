﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDM.Models
{
    public class ItemHist : BaseEntity
    {

        [Required]
        [StringLength(100)]
        public string InternalCode { get; set; }

        public decimal Enter { get; set; }

        public decimal Exit { get; set; }

        public decimal Thickness { get; set; }

        public int Band { get; set; }

        public bool Elastic { get; set; }

        [Required]
        [StringLength(150)]
        public string Color { get; set; }

        public Status Status { get; set; }

        [Required]
        [StringLength(150)]
        public string MadeBy { get; set; }


        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }


        public int MachineTypeId { get; set; }

        [ForeignKey("MachineTypeId")]
        public virtual MachineType MachineType { get; set; }



        [ForeignKey("ItemTypeId")]
        public virtual ItemType ItemType { get; set; }

        public int ItemTypeId { get; set; }

    }
}
