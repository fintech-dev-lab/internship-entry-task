using System.Text.Json.Serialization;

namespace Segrom.СrossesProject.Host.Dtos;

public record GameDto(
	[property: JsonPropertyName("game_id")]
	Guid GameId,
	[property: JsonPropertyName("field")]
	byte[,] Field,
	[property: JsonPropertyName("current_player")]
	byte CurrentPlayer,
	[property: JsonPropertyName("winner")]
	byte? Winner
	);