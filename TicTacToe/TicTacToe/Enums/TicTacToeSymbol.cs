using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TicTacToe.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TicTacToeSymbol
    {
        X,
        O
    }
}
