using CompTrain.Shared.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CompTrain.Shared.Models.Wod
{
    public class WodRequest
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Data.Wod Wod { get; set; }

        public IList<WorkoutRequest> WorkoutRequests { get; set; }

        public IList<Resulttype> Resulttypes { get; set; }

        public IList<Workouttype> Workouttypes { get; set; }

        public bool IsSaved { get; set; }
    }
}
