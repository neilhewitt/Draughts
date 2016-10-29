using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Move
    {
        #region static

        public static IEnumerable<Move> ValidMovesFor(Board board, Player player)
        {
            List<Move> validMoves = new List<Move>();
            foreach (Piece piece in board.Squares.Where(s => s.IsOccupied && s.Occupier.Owner == player).Select(s => s.Occupier))
            {
                validMoves.AddRange(piece.GetMoves());
            }

            return validMoves;
        }

        #endregion

        private IList<MoveStep> _steps;

        public MoveStep Start => _steps.FirstOrDefault();
        public MoveStep End => _steps.LastOrDefault();
        public int PiecesTaken => _steps.Count() - 2;
        public bool PieceIsCrowned { get; }

        public IEnumerable<MoveStep> Steps => _steps;

        public void Add(int row, int column)
        {
            MoveStep node = new MoveStep(row, column, null);
            node.AddNext(_steps.FirstOrDefault());
            _steps.Insert(0, node);
        }

        internal Move(Piece piece)
        {
            _steps = new List<MoveStep>();
            PieceIsCrowned = piece.IsCrowned;
        }
    }
}
