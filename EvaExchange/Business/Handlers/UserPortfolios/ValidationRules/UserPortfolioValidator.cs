
using Business.Handlers.UserPortfolios.Commands;
using FluentValidation;

namespace Business.Handlers.UserPortfolios.ValidationRules
{

    public class CreateUserPortfolioValidator : AbstractValidator<CreateUserPortfolioCommand>
    {
        public CreateUserPortfolioValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ShareId).NotEmpty();
            RuleFor(x => x.Lot).GreaterThanOrEqualTo(0);
        }
    }
    public class UpdateUserPortfolioValidator : AbstractValidator<UpdateUserPortfolioCommand>
    {
        public UpdateUserPortfolioValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ShareId).NotEmpty();
        }
    }
}