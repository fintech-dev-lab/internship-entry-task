namespace TicTacToe.Application.Interfaces
{
    public interface IRandomProvider
    {
        /// <summary>
        /// Возвращает true с указанной вероятностью в процентах.
        /// </summary>
        /// <param name="percentageChance">Вероятность от 0 до 100.</param>
        /// <returns></returns>
        bool ShouldOccur(int percentageChance);
    }
}
