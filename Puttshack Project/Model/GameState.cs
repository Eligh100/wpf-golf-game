using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Puttshack_API.Models
{
    public enum GameState
    {
        BallOnTee = 0,
        RegularShot = -10,
        SuperTubeActivated = 25,
        BonusPointsActivated = 15,
        HazardActivated = -25,
        BallInHole = 0
    }
}
