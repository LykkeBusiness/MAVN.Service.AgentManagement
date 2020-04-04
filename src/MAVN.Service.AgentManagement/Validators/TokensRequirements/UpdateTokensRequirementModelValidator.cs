using FluentValidation;
using MAVN.Service.AgentManagement.Client.Models.Requirements;

namespace MAVN.Service.AgentManagement.Validators.TokensRequirements
{
    public class UpdateTokensRequirementModelValidator: AbstractValidator<UpdateTokensRequirementModel>
    {
        public UpdateTokensRequirementModelValidator()
        {
            RuleFor(m => m.Amount)
                .NotNull()
                .NotEmpty()
                .WithMessage("Reward should not be empty")
                .GreaterThanOrEqualTo(0m)
                .WithMessage("The amount of tokens should be equal or greater than 0.");
        }
    }
}
