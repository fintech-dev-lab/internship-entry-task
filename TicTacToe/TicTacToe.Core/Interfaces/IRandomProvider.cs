using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Core.Interfaces
{
    public interface IRandomProvider
    {
        int Next(int max);
    }
}
