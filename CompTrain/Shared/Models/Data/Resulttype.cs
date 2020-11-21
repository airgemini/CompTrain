using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CompTrain.Shared.Models.Data
{
    public class Resulttype
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        [Required, Column(Order = 1)]
        public string Name { get; set; }

        [Required, Column(Order = 2)]
        public string ResultColumn { get; set; }

        [Required, Column(Order = 3)]
        public int OrderNum { get; set; }

        [Required, Column(Order = 4)]
        public bool IsDefault { get; set; }

        public ICollection<Workout> Workouts { get; set; }
    }
}
