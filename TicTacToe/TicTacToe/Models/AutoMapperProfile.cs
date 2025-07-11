using AutoMapper;
using TicTacToe.Data.Entites;
using TicTacToe.ViewModels.Response;

namespace TicTacToe.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Game, GameDto>();
            CreateMap<Move, MoveDto>();

        }
    }
}
