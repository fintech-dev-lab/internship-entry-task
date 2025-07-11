using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Core.Interfaces;

namespace TicTacToe.Core.Entities
{
    public class DefaultRandomProvider : IRandomProvider
    {
        private readonly Random _random = new();

        public int Next(int max) => _random.Next(max);
    }

}
