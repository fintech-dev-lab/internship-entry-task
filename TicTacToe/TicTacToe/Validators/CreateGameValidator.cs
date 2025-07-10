using FluentValidation;
using TicTacToe.ViewModels.Request;

namespace TicTacToe.Validators
{
    public class CreateGameValidator : AbstractValidator<CreateGameDto>
    { 
        public CreateGameValidator()
        {
            //TODO: перепроверитьвсе условия
            RuleFor(g => g.BoardSize).NotNull().When(g => g.WinLenght != null);
            RuleFor(g => g.WinLenght).NotNull().When(g => g.BoardSize != null);

            RuleFor(g => g.BoardSize).GreaterThan(2).When(g => g.BoardSize != null);
            RuleFor(g => g.WinLenght).GreaterThan(2).LessThanOrEqualTo(g => g.BoardSize).When(g => g.WinLenght != null);
        }
    }
}
