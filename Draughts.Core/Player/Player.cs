using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Player
    {
        private static Random _random = new Random(DateTime.Now.Millisecond);

        private Game _game;

        public string Name { get; }
        public PieceColour Colour { get; }
        public bool IsComputerPlayer { get; }

        public int PiecesRemaining => _game.Board.Squares.Count(s => s.IsOccupied && s.Occupier.Owner == this);
        public int PiecesTaken { get; private set; }

        internal IEnumerable<Move> GetValidMoves(out Move bestMove)
        {
            List<Move> moves = new List<Move>();
            foreach (Piece piece in _game.Board.Squares.Where(s => s.IsOccupied && s.Occupier.Owner == this).Select(s => s.Occupier))
            {
                moves.AddRange(piece.GetMoves());
            }

            bestMove = GetBestMove(moves);
            {
                if (bestMove.PiecesTaken > 0)
                {
                    // we *have* to take this move
                    return new List<Move>() { bestMove };
                }
                else
                {
                    return moves;
                }
            }
        }

        private Move GetBestMove(IEnumerable<Move> validMoves)
        {
            Dictionary<Move, int> movesByPiecesTaken = new Dictionary<Move, int>(); // moves are unique, count may not be, hence backwards
            foreach (Move move in validMoves)
            {
                movesByPiecesTaken.Add(move, move.PiecesTaken);
            }

            if (movesByPiecesTaken.Count > 0)
            {
                IEnumerable<Move> bestMoves = movesByPiecesTaken.Where(x => x.Value == movesByPiecesTaken.Values.Max()).Select(x => x.Key);
                if (bestMoves.Count() == 1)
                {
                    return bestMoves.First(); // we must always pick the move that takes the most pieces, if one exists
                }
                else
                {
                    while (true)
                    {
                        Move bestMove = bestMoves.Skip(_random.Next(bestMoves.Count() - 1)).First(); // pick a random from the available best moves
                        if (movesByPiecesTaken.Values.Max() == 0) // if none of these moves takes any pieces, we can apply extra rules...
                        {
                            // try not to move next to a piece that could then take you
                            List<Move> badMoves = new List<Move>();
                            foreach (Move move in bestMoves)
                            {
                                int rowStep = (Colour == PieceColour.Black ? 1 : -1);
                                Square left = _game.Board[move.End.Row + rowStep, move.End.Column - 1];
                                Square right = _game.Board[move.End.Row + rowStep, move.End.Column + 1];
                                if ((left != null && left.IsOccupied && left.Occupier.Owner != this) || (right != null && right.IsOccupied && right.Occupier.Owner != this))
                                {
                                    badMoves.Add(move);
                                }
                            }
                            if (bestMoves.Any(x => !badMoves.Contains(x)))
                            {
                                // there are moves available that are not bad, so let's use only those
                                bestMoves = bestMoves.Where(x => !badMoves.Contains(x));
                            }

                            // any piece that could become crowned now is the best move - pick any of those
                            IEnumerable<Move> crownMoves = bestMoves.Where(m => !m.PieceIsCrowned &&
                                ((Colour == PieceColour.Black && m.End.Row == 7) || (Colour == PieceColour.White && m.End.Row == 0)));
                            if (crownMoves.Count() > 0)
                            {
                                bestMove = crownMoves.Skip(_random.Next(crownMoves.Count() - 1)).First();
                                return bestMove;
                            }

                            // otherwise, make crowned pieces go backwards preferentially to avoid corner-dwelling situations
                            if (bestMoves.Any(m => m.PieceIsCrowned))
                            {
                                IEnumerable<Move> bestMoveSubset = bestMoves.Where(m => m.PieceIsCrowned &&
                                    ((Colour == PieceColour.Black && m.End.Row <= m.Start.Row && m.End.Row > 3)
                                    || (Colour == PieceColour.White && m.End.Row >= m.Start.Row && m.End.Row < 4)));
                                if (bestMoveSubset.Count() > 0)
                                {
                                    bestMove = bestMoveSubset.Skip(_random.Next(bestMoveSubset.Count() - 1)).First();
                                    return bestMove;
                                }
                            }
                        }

                        return bestMove;
                    }
                }
            }

            return null;
        }

        internal bool Move(Move move)
        {
            Square origin = _game.Board[move.Start.Row, move.Start.Column];
            origin.Occupier.Move(move);
            return true;
        }

        internal void TakePiece()
        {
            PiecesTaken++;
        }

        public Player(string name, Game game, PieceColour colour, bool isComputerPlayer)
        {
            Name = name;
            _game = game;
            Colour = colour;
            IsComputerPlayer = isComputerPlayer;
        }
    }
}
