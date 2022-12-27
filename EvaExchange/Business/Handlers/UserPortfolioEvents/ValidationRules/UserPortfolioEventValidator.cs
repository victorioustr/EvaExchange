
using Business.Handlers.UserPortfolioEvents.Commands;
using FluentValidation;

namespace Business.Handlers.UserPortfolioEvents.ValidationRules
{

    public class CreateUserPortfolioEventValidator : AbstractValidator<CreateUserPortfolioEventCommand>
    {
        public CreateUserPortfolioEventValidator()
        {
            RuleFor(x => x.UserPortfolioId).NotEmpty();
            RuleFor(x => x.Lot).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Rate).GreaterThan(0);
        }
    }
    public class UpdateUserPortfolioEventValidator : AbstractValidator<UpdateUserPortfolioEventCommand>
    {
        public UpdateUserPortfolioEventValidator()
        {
            RuleFor(x => x.UserPortfolioId).NotEmpty();
            RuleFor(x => x.Lot).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Rate).GreaterThan(0);
        }
    }
}