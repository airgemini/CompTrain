using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CompTrain.Shared.Models.Data
{
    public class Benchmarkresult
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Required, Column(Order = 1)]
        public int BenchmarkId { get; set; }

        [Column(Order = 3)]
        public TimeSpan ResultTime { get; set; }

        [Column(Order = 4)]
        public int ResultRep { get; set; }

        [Column(Order = 5, TypeName = "decimal(5, 2)")]
        public Decimal ResultWeight { get; set; }

        [Column(Order = 6, TypeName = "decimal(5, 2)")]
        public Decimal ResultDistance { get; set; }

        public Benchmark Benchmark { get; set; }
        
        [ForeignKey("AthleteId")]
        public Athlete Athlete { get; set; }


    }
}
