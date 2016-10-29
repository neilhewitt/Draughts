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
        private Func<IEnumerable<Move>, Move, Move> _selectMoveCallback;

        public string Name { get; private set; }
        public PieceColour Colour { get; }
        public bool IsComputerPlayer { get; private set; }
        public Player Opponent => Colour == PieceColour.Black ? _game.WhitePlayer : _game.BlackPlayer;

        internal Func<IEnumerable<Move>, Move, Move> SelectMoveCallback => _selectMoveCallback;

        public int PiecesRemaining => _game.Board.Squares.Count(s => s.IsOccupied && s.Occupier.Owner == this);
        public int PiecesCaptured { get; private set; }

        internal IEnumerable<Move> GetValidMoves(out Move bestMove, bool useRandomBestMove = false)
        {
            IEnumerable<Move> validMoves = Core.Move.ValidMovesFor(_game.Board, this);
            // we can use either random best move (pick any from valid moves) or MiniMax (AI play-ahead up to n generations)
            bestMove = useRandomBestMove ? RandomBestMove(validMoves) : MiniMaxBestMove(validMoves, 4);
            {
                if (bestMove != null && bestMove.PiecesTaken > 0)
                {
                    // we *have* to take this move
                    return new List<Move>() { bestMove };
                }
                else
                {
                    return validMoves;
                }
            }
        }

        internal void Move(Move move)
        {
            Square origin = _game.Board[move.Start.Row, move.Start.Column];
            origin.Occupier.Move(move);
        }

        internal void CapturedAPiece()
        {
            PiecesCaptured++;
        }

        internal void BecomeHuman(string name, Func<IEnumerable<Move>, Move, Move> selectMoveCallback)
        {
            Name = name;
            IsComputerPlayer = false;
            _selectMoveCallback = selectMoveCallback;
        }

        private Move MiniMaxBestMove(IEnumerable<Move> validMoves, int maxMovesAhead)
        {
            // minmax algorithm plays n moves ahead and notes which move at the first level leads to the least opponent pieces remaining and the most player pieces remaining
            // which it recommends as the best move
            Board newBoard = _game.Board.Clone();
            MiniMaxResult result = MiniMax(newBoard, this, 0, maxMovesAhead, null, validMoves);

            // if best move takes no pieces but valid moves do take pieces, we have to pick one (any one) that takes pieces
            if (result.Move != null && result.Move.PiecesTaken == 0 && validMoves.Any(m => m.PiecesTaken > 0)) return RandomBestMove(validMoves);
            // sometimes there is no best move within n moves ahead, so just use a random of the valid moves available
            if (result.Move == null) return RandomBestMove(validMoves);
            // otherwise, return the recommended best move
            return result.Move;
        }

        private MiniMaxResult MiniMax(Board board, Player player, int generation, int maxGenerations, MiniMaxResult current, IEnumerable<Move> validMoves = null)
        {
            if (current == null) current = new MiniMaxResult() { Move = null, MaxPlayerPiecesRemaining = 0, MaxOpponentPiecesRemaining = 12 };
            if (generation >= maxGenerations || current.MaxOpponentPiecesRemaining == 0) // we've reached the limit, or a winning move has been found already
                return current;
            generation++;
            
            validMoves = validMoves ?? Core.Move.ValidMovesFor(board, player);
            foreach(Move move in Randomize(validMoves)) // randomize the move order to avoid first-place bias for sets of equally good moves
            {
                Board newBoard = board.Clone();
                newBoard[move.Start.Row, move.Start.Column].Occupier.Move(move);

                int playerCount = newBoard.Squares.Count(s => s.IsOccupied && s.Occupier.Owner == player);
                int opponentCount = newBoard.Squares.Count(s => s.IsOccupied && s.Occupier.Owner == player.Opponent);
                if (opponentCount < current.MaxOpponentPiecesRemaining || playerCount > current.MaxPlayerPiecesRemaining)
                {
                    if (generation == 1) current.Move = move; // only 1st generation moves are actually valid, others are projections
                    current.MaxPlayerPiecesRemaining = playerCount;
                    current.MaxOpponentPiecesRemaining = opponentCount;
                }

                if (current.Move == null) maxGenerations++; // go deeper if we can't find anything at all
                current = MiniMax(newBoard, player.Opponent, generation, maxGenerations, current);
            }

            return current;
        }

        private class MiniMaxResult
        {
            public Move Move { get; set; }
            public int MaxPlayerPiecesRemaining { get; set; }
            public int MaxOpponentPiecesRemaining { get; set; }
        }

        private Move RandomBestMove(IEnumerable<Move> validMoves)
        {
            Move bestMove = Randomize(validMoves).FirstOrDefault();
            return bestMove;
        }

        private IEnumerable<T> Randomize<T>(IEnumerable<T> input)
        {
            int count = input.Count();
            List<T> inputAsList = new List<T>(input);

            int[] indices = Enumerable.Range(0, count).ToArray();

            for (int i = 0; i < count; ++i)
            {
                int position = _random.Next(i, count);
                yield return inputAsList[indices[position]];
                indices[position] = indices[i];
            }
        }

        public Player(Game game, PieceColour colour)
        {
            Name = "Computer";
            _game = game;
            Colour = colour;
            IsComputerPlayer = true;
            _selectMoveCallback = (moves, move) => move;
        }
    }
}
