using System.Text.Json.Serialization;

namespace Segrom.СrossesProject.Host.Dtos;

public record struct PositionDto(
	[property: JsonPropertyName("x")]
	uint X,
	[property: JsonPropertyName("y")]
	uint Y
	);