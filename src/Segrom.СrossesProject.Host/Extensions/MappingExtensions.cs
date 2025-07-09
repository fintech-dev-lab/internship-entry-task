using Segrom.СrossesProject.Domain.Entities;
using Segrom.СrossesProject.Domain.ValueObjects;
using Segrom.СrossesProject.Host.Dtos;

namespace Segrom.СrossesProject.Host.Extensions;

internal static class MappingExtensions
{
	public static GameDto ToDto(this Game dto)
	{
		var size = dto.Field.Size;
		var field = new byte[size, size];
		
		for (var y = 0; y < size; y++)
		{
			for (var x = 0; x < size; x++)
			{
				field[x,y] =  (byte)dto.Field.Cells[x,y];
			}
		}

		return new GameDto(
			GameId: dto.Id,
			Field: field,
			CurrentPlayer: (byte)dto.CurrentPlayer,
			Winner: dto.Winner.HasValue ? (byte)dto.Winner.Value : null
		);
	}

	public static PositionDto ToDto(this Position domain) => new(domain.X, domain.Y);
	public static Position ToDomain(this PositionDto dto) => new(dto.X, dto.Y);
}