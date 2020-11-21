using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CompTrain.Shared.Models.Data
{
    public class Plane
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Required, Column(Order = 1)]
        public bool Active { get; set; }

        [Required, Column(Order = 2)]
        public string Name { get; set; }

        [Required, Column(Order = 3)]
        public Int16 Duration { get; set; }

        [Required, Column(Order = 4, TypeName = "decimal(5, 2)")]
        public Decimal Price { get; set; }
    }
}
