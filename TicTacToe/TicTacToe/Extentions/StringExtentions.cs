namespace TicTacToe.Extentions
{
    public static class StringExtentions
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
