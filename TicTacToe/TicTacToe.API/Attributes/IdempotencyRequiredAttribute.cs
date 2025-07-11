namespace TicTacToe.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class IdempotencyRequiredAttribute : Attribute
    {
    }
}
