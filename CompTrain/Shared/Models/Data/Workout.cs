using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CompTrain.Shared.Models.Data
{
    public class Workout
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        
        [Required, Column(Order = 1)]
        public int WodId { get; set; }

        [Required, Column(Order = 2)]
        public int WorkouttypeId { get; set; }

        [Required, Column(Order = 3)]
        public int ResulttypeId { get; set; }
        

        [Required, Column(Order = 4)]
        public string Description { get; set; }

        public Wod Wod { get; set; }
        public Workouttype Workouttype { get; set; }
        
        public virtual Resulttype Resulttype { get; set; }
    }
}
