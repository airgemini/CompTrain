using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CompTrain.Shared.Models.Data
{
    public class Subscription
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }


        [Required, Column(Order = 2)]
        public int PlaneId { get; set; }


        [Required, Column(Order = 3)]
        public string Name { get; set; }

        [Required, Column(Order = 4)]
        public string Status { get; set; }

        [Required, Column(Order = 5)]
        public string Paymentdetail { get; set; }

        [Required, Column(Order = 6)]
        public DateTime Start { get; set; }

        [Required, Column(Order = 7)]
        public DateTime End { get; set; }


        public Plane Plane { get; set; }
        [ForeignKey("AthleteId")]
        public Athlete Athlete { get; set; }
    }
}
