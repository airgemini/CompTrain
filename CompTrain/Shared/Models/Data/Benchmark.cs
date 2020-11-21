using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CompTrain.Shared.Models.Data
{
    public class Benchmark
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Required, Column(Order = 1)]
        public string Name { get; set; }

        [Required, Column(Order = 2)]
        public int ResulttypeId { get; set; }

        public Resulttype Resulttype { get; set; }
    }
}
