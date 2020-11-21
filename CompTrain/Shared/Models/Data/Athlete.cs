using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompTrain.Shared.Models.Data
{
    public class Athlete : IdentityUser
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [PersonalData]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [PersonalData]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [PersonalData]
        public DateTime Birthday { get; set; }

        [Required]
        [PersonalData]
        public char Sex { get; set; }
    }
}
