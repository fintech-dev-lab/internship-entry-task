using FluentValidation;
using TicTacToe.ViewModels.Request;

namespace TicTacToe.Validators
{
    public class CreateMoveValidator : AbstractValidator<CreateMoveDto>
    {
        public CreateMoveValidator()
        {
            //TODO: перепроверитьвсе условия
            RuleFor(g => g.Row).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(g => g.Column).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(m => m.Symbol).NotNull().IsInEnum();
        }
    }
}
