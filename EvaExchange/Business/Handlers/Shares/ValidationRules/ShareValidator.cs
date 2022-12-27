
using Business.Handlers.Shares.Commands;
using FluentValidation;

namespace Business.Handlers.Shares.ValidationRules
{

    public class CreateShareValidator : AbstractValidator<CreateShareCommand>
    {
        public CreateShareValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(3, 100);
            RuleFor(x => x.Code).NotEmpty().Length(3, 3);
            RuleFor(x => x.Rate).NotEmpty().GreaterThan(0);
        }
    }
    public class UpdateShareValidator : AbstractValidator<UpdateShareCommand>
    {
        public UpdateShareValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(3, 100);
            RuleFor(x => x.Code).NotEmpty().Length(3, 3);
            RuleFor(x => x.Rate).NotEmpty().GreaterThan(0);
        }
    }
}