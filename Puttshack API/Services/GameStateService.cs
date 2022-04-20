using Puttshack_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Puttshack_API.Services
{
    public class GameStateService
    {
        private static int BASE_REGULAR_SHOTS = 5;
        private static int BASE_SUPERTUBES = 1;
        private static int BASE_HAZARDS = 1;
        private static int BASE_BONUSES = 1;
        private static int BASE_BALL_IN_HOLE = 1;
        public List<GameMove> PossibleRemainingMoves { get; set; } 

        private GameMove BallOnTeeMove = new GameMove
        {
            MoveId = GameState.BallOnTee,
            MoveName = "Ball On Tee",
            MoveScore = (int)GameState.BallOnTee
        };

        private GameMove RegularShotMove = new GameMove
        {
            MoveId = GameState.RegularShot,
            MoveName = "Regular Shot",
            MoveScore = (int)GameState.RegularShot
        };

        private GameMove SuperTubeMove = new GameMove
        {
            MoveId = GameState.SuperTubeActivated,
            MoveName = "Supertube Activated!",
            MoveScore = (int)GameState.SuperTubeActivated
        };

        private GameMove HazardMove = new GameMove
        {
            MoveId = GameState.HazardActivated,
            MoveName = "Hazard Activated!",
            MoveScore = (int)GameState.HazardActivated
        };

        private GameMove BonusPointsMove = new GameMove
        {
            MoveId = GameState.BonusPointsActivated,
            MoveName = "Bonus Points Activated!",
            MoveScore = (int)GameState.BonusPointsActivated
        };

        private GameMove BallInHoleMove = new GameMove
        {
            MoveId = GameState.BallInHole,
            MoveName = "Ball in Hole!",
            MoveScore = (int)GameState.BallInHole
        };

        /// <summary>
        /// Initialises the 'PossibleRemainingMoves' list
        /// </summary>
        public GameStateService ()
        {
            PossibleRemainingMoves = new List<GameMove>();
        }

        /// <summary>
        /// Starts the game by setting the first move to 'Ball on Tee'
        /// </summary>
        /// <returns>'Ball on Tee' move</returns>
        public GameMove StartGame()
        {
            return BallOnTeeMove;
        }

        /// <summary>
        /// Determines the next move
        /// </summary>
        /// <param name="currentMoveNumber">How many moves into the game the user is</param>
        /// <returns>The next game move</returns>
        public GameMove NextMove(int currentMoveNumber)
        {
            if (currentMoveNumber == 1)
            {
                SetStartingMoves();
            } 
            else
            {
                SetRemainingMoves(currentMoveNumber);
            }

            GameMove nextMove = GetNextMove();

            return nextMove;
        }

        /// <summary>
        /// Picks a random move from the list of remaining moves
        /// </summary>
        /// <returns>The next randomly picked game move</returns>
        private GameMove GetNextMove()
        {
            Random rnd = new Random();
            var nextMoveIndex = rnd.Next(0, PossibleRemainingMoves.Count);

            return PossibleRemainingMoves[nextMoveIndex];
        }
        
        /// <summary>
        /// Uses the number of moves elapsed to increase probability of ball in hole
        /// </summary>
        /// <param name="noOfCompletedMoves">How far into the game the user is - determines the likelihood of 'Ball In Hole'</param>
        private void SetRemainingMoves(int noOfCompletedMoves)
        {
            PossibleRemainingMoves.Clear();

            if ((noOfCompletedMoves-1) <= BASE_REGULAR_SHOTS)
            {
                PossibleRemainingMoves.AddRange(Enumerable.Repeat(RegularShotMove, BASE_REGULAR_SHOTS - (noOfCompletedMoves - 1)));
            }

            PossibleRemainingMoves.Add(HazardMove);
            PossibleRemainingMoves.Add(BonusPointsMove);
            
            PossibleRemainingMoves.AddRange(Enumerable.Repeat(BallInHoleMove, BASE_BALL_IN_HOLE + (noOfCompletedMoves - 1)));
        }

        /// <summary>
        /// Sets base starting moves - can be adjusted for difficulty by changing the base static fields
        /// </summary>
        private void SetStartingMoves()
        {
            PossibleRemainingMoves.Clear();

            PossibleRemainingMoves.AddRange(Enumerable.Repeat(RegularShotMove, BASE_REGULAR_SHOTS));
            PossibleRemainingMoves.Add(SuperTubeMove);
            PossibleRemainingMoves.Add(HazardMove);
            PossibleRemainingMoves.Add(BonusPointsMove);
            PossibleRemainingMoves.Add(BallInHoleMove);
        }


    }
}
