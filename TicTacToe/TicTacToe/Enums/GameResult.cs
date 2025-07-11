using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TicTacToe.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GameResult
    {
        InProgress,
        XWin,
        OWin,
        Draw,
    }
}
