using CompTrain.Shared.Models.Wod;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompTrain.Shared.Validators
{
    public class WodRequestValidator: AbstractValidator<WodRequest>
    {
        public WodRequestValidator()
        {
            RuleFor(w => w.Date).NotEmpty().WithMessage("E' importante specificare una data");
            RuleFor(w => w.Name).NotEmpty().WithMessage("Dai un nome al wod").MaximumLength(50).WithMessage("Il nome è troppo lungo");
            RuleFor(w => w.WorkoutRequests).NotEmpty().WithMessage("Una programmazione senza workout non va bene!");
            /*
            RuleFor(w => w.Resulttypes).NotEmpty();
            RuleFor(w => w.Workouttypes).NotEmpty();
            */
            RuleForEach(w => w.WorkoutRequests).SetValidator(new WorkoutRequestsValidator());
        }
    }
}
