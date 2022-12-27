
using Business.Handlers.ShareRateHistories.Commands;
using FluentValidation;

namespace Business.Handlers.ShareRateHistories.ValidationRules
{

    public class CreateShareRateHistoryValidator : AbstractValidator<CreateShareRateHistoryCommand>
    {
        public CreateShareRateHistoryValidator()
        {
            RuleFor(x => x.ShareId).NotEmpty();
            RuleFor(x => x.Rate).NotEmpty().GreaterThan(0);
            RuleFor(x => x.UpdatedDate).NotEmpty();
        }
    }
    public class UpdateShareRateHistoryValidator : AbstractValidator<UpdateShareRateHistoryCommand>
    {
        public UpdateShareRateHistoryValidator()
        {
            RuleFor(x => x.ShareId).NotEmpty();
            RuleFor(x => x.Rate).NotEmpty().GreaterThan(0);
            RuleFor(x => x.UpdatedDate).NotEmpty();
        }
    }
}