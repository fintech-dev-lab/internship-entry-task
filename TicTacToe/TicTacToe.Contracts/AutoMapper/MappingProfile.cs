using AutoMapper;
using TicTacToe.Contracts.DTO;
using TicTacToe.Core.Entities;

namespace TicTacToe.Contracts.AutoMapper;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<MoveDto, Move>().ReverseMap();
        
        CreateMap<UserDto, User>().ReverseMap();
        
        CreateMap<GameResponse, Game>().ReverseMap();
    }
}