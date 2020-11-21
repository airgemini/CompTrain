using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CompTrain.Shared.Models.Data
{
    public class RestDay
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
