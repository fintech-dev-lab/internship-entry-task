using System.Text.Json.Serialization;

namespace Segrom.СrossesProject.Host.DTO;

public record struct PositionDto(
	[property: JsonPropertyName("x")]
	uint X,
	[property: JsonPropertyName("y")]
	uint Y
	);