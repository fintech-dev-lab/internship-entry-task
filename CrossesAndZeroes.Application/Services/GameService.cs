using CrossesAndZeroes.Application.Abstractions;
using CrossesAndZeroes.Application.DTOs;
using CrossesAndZeroes.Domain.Entities;
using CrossesAndZeroes.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesAndZeroes.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _repo;

        private readonly int _boardSize;
        private readonly int _winLength;

        public GameService(IGameRepository repo, IConfiguration config)
        {
            _repo = repo;

            
            _boardSize = int.Parse(config["Game:BoardSize"] ?? "3");
            _winLength = int.Parse(config["Game:WinLength"] ?? "3");
        }

        public async Task<Game> CreateGameAsync()
        {
            var game = new Game(Guid.NewGuid(),_boardSize, _winLength);
            await _repo.SaveAsync(game);
            return game;
        }

        public async Task<Game?> GetGameAsync(Guid id)
        {
            return await _repo.GetAsync(id);
        }

        public async Task<Game> MakeMoveAsync(Guid id, MoveDto move)
        {
            var game = await _repo.GetAsync(id);
            if (game == null)
                throw new Exception("Игра не найдена");

            game.ApplyMove(move.Row, move.Col);
            await _repo.SaveAsync(game);
            return game;
        }
    }
}
