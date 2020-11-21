using CompTrain.Shared.Models.Wod;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompTrain.Shared.Validators
{
    public class WorkoutRequestsValidator : AbstractValidator<WorkoutRequest>
    {
        public WorkoutRequestsValidator()
        {
            RuleFor(w => w.Description).NotEmpty().WithMessage(x => $"Inserisci il testo del workout da svolgere per il {x.Workouttype.Name}");
            RuleFor(w => w.ResulttypeId).NotEmpty().WithMessage(x => $"Seleziona che tipo di risultato deve essere impostato per il {x.Workouttype.Name}");
            RuleFor(w => w.Workouttype).NotEmpty();
        }
    }
}
