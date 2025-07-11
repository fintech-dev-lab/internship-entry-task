using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesAndZeroes.Domain.Entities
{
    public class Move
    {
        public string Player { get; } = "";
        public int Row { get; }
        public int Col { get; }
    }
}
