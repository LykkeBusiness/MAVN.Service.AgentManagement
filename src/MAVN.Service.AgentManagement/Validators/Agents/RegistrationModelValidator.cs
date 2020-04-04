using System;
using FluentValidation;
using JetBrains.Annotations;
using MAVN.Service.AgentManagement.Client.Models.Agents;

namespace MAVN.Service.AgentManagement.Validators.Agents
{
    [UsedImplicitly]
    public class RegistrationModelValidator : AbstractValidator<RegistrationModel>
    {
        public RegistrationModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(o => o.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Customer id required.");

            RuleFor(o => o.FirstName)
                .NotEmpty()
                .WithMessage("First name required.")
                .MaximumLength(100)
                .WithMessage("First name shouldn't be longer than 100 characters.");

            RuleFor(o => o.LastName)
                .NotEmpty()
                .WithMessage("Last name required.")
                .MaximumLength(100)
                .WithMessage("Last name shouldn't be longer than 100 characters.");


            RuleFor(o => o.CountryOfResidenceId)
                .GreaterThan(0)
                .WithMessage("Country of residence required.");

            RuleFor(o => o.Note)
                .MaximumLength(2000)
                .WithMessage("Note shouldn't be longer than 2000 characters.");

            RuleFor(o => o.Images)
                .NotEmpty()
                .WithMessage("Images required");

            RuleForEach(o => o.Images)
                .SetValidator(new ImageModelValidator());
        }
    }
}
