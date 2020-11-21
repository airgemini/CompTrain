using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CompTrain.Shared.Models.Data
{
    public class Workoutresult
    {
        [Key, Column(Order = 1)]
        public int Id { get; set; }

        [Required, Column(Order = 2)]
        public int WorkoutId { get; set; }

        [Required, Column(Order = 4)]
        public bool Rx { get; set; }

        [Column(Order = 5)]
        public TimeSpan ResultTime { get; set; }

        [Column(Order = 6)]
        public int ResultRep { get; set; }

        [Column(Order = 7, TypeName = "decimal(5, 2)")]
        public Decimal ResultWeight { get; set; }

        [Column(Order = 8, TypeName = "decimal(5, 2)")]
        public Decimal ResultDistance { get; set; }

        public Workout Workout { get; set; }
        [ForeignKey("AthleteId")]
        public Athlete Athlete { get; set; }
    }
}
