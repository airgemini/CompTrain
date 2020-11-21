using CompTrain.Shared.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CompTrain.Shared.Models.Wod
{
    public class WorkoutRequest
    {
        public string Description { get; set; }
        public Workouttype Workouttype { get; set; }
        public int ResulttypeId { get; set; }
    }
}
