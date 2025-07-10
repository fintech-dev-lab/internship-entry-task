using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Data;
using TicTacToe.Data.Entites;

namespace TicTacToe.Tests.Extentions
{
    public static class ApplicationContextExtentions
    {
        public static void SeedData(this ApplicationContext dbContext)
        {
            var game1 = new Game()
            { 
                Id = 1,
                BoardSize = 5,
                CurrentSymbol = Enums.TicTacToeSymbol.X,
                ETag = Guid.NewGuid(),
                Result = Enums.GameResult.InProgress,
                WinLenght = 3,                  
            };

            var game2 = new Game()
            {
                Id = 2,
                BoardSize = 3,
                CurrentSymbol = Enums.TicTacToeSymbol.X,
                ETag = Guid.NewGuid(),
                Result = Enums.GameResult.XWin,
                WinLenght = 3,
            };

            var game3 = new Game()
            {
                Id = 3,
                BoardSize = 3,
                CurrentSymbol = Enums.TicTacToeSymbol.X,
                ETag = Guid.NewGuid(),
                Result = Enums.GameResult.InProgress,
                WinLenght = 3,
            };

            var game4 = new Game()
            {
                Id = 4,
                BoardSize = 3,
                CurrentSymbol = Enums.TicTacToeSymbol.X,
                ETag = Guid.NewGuid(),
                Result = Enums.GameResult.InProgress,
                WinLenght = 3,
            };
            dbContext.Games.AddRange(game1, game2, game3);
            dbContext.SaveChanges();
        }
    }
}
