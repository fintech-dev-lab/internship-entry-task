using TicTacToe.Application.Interfaces;

namespace TicTacToe.Infrastructure.Services
{
    public class SystemRandomProvider : IRandomProvider
    {
        [ThreadStatic]
        private static Random? _local;

        private static Random Inst
        {
            get
            {
                if (_local is null)
                {
                    _local = new Random(Environment.CurrentManagedThreadId);
                }
                return _local;
            }
        }

        public bool ShouldOccur(int percentageChance)
        {
            if (percentageChance < 0 || percentageChance > 100)
                throw new ArgumentOutOfRangeException(nameof(percentageChance), "Percentage must be between 0 and 100.");

            return Inst.Next(1, 101) <= percentageChance;
        }
    }
}
