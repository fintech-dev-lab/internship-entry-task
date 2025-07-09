using System.Text.Json.Serialization;

namespace Segrom.СrossesProject.Host.DTO;

public record GameDto(
	[property: JsonPropertyName("game_id")]
	Guid GameId,
	[property: JsonPropertyName("field")]
	char[][] Field,
	[property: JsonPropertyName("current_player")]
	byte CurrentPlayer,
	[property: JsonPropertyName("winner")]
	byte? Winner
	);