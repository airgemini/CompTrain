using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CompTrain.Shared.Models.Data
{
    public class Workouttype
    {
        [Key, Column(Order = 1)]
        public int Id { get; set; }
        [Required, Column(Order = 2)]
        public string Name { get; set; }
        
        [Required, Column(Order = 3)]
        public int OrderNum { get; set; }

        public bool IsAutomatic { get; set; } = true;

        public ICollection<Workout> Workouts { get; set; }
    }
}
