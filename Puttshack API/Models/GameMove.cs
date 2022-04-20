using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Puttshack_API.Models
{
    public class GameMove
    {
        public GameState MoveId { get; set; }

        public string MoveName { get; set; }

        public int MoveScore { get; set; }

    }
}
