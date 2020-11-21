using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompTrain.Shared.Models.Data
{
    public class Wod
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Required, Column(Order = 1, TypeName ="date")]
        public DateTime Date { get; set; }
        [Required, Column(Order = 2)]
        public string Name { get; set; }

        [Column(Order = 3)]
        public string Description { get; set; }

        public ICollection<Workout> Workouts { get; set; }
    }
}
