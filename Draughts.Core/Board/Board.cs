using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Board
    {
        private const string DEFAULT_STATE = "0202020220202020020202021010101001010101404040400404040440404040";
        private const string EMPTY_STATE =   "0101010110101010010101011010101001010101101010100101010110101010";

        private Game _game;
        private int[] _data;

        public BoardState State => new BoardState(this);
        public string StateData => StateAsString();

        public Square this[int row, int column]
        {
            get
            {
                if (row < 0 || row > 7 || column < 0 || column > 7) return null;

                int state = (_data[(row * 8) + column]) - 48; // chars are ints - nice!
                SquareColour colour = state == 0 ? SquareColour.Yellow : SquareColour.Gray;
                Piece piece = null;
                if (state > 1)
                {
                    PieceColour pieceColour = state > 3 ? PieceColour.White : PieceColour.Black;
                    Player player = state > 3 ? _game.WhitePlayer : _game.BlackPlayer;
                    piece = new Piece(pieceColour, this, row, column, player);
                    if (state == 3 || state == 5) piece.IsCrowned = true;
                }

                return new Square(colour, piece, row, column);
            }
        }

        public IEnumerable<Square> Squares
        {
            get
            {
                for (int row = 0; row < 8; row++)
                {
                    for (int column = 0; column < 8; column++)
                    {
                        yield return this[row, column];
                    }
                }
            }
        }

        public void Clear(int row, int column)
        {
            if (row < 0 || row > 7 || column < 0 || column > 7) return;

            _data[(row * 8) + column] = EMPTY_STATE[(row * 8) + column] - 48;
        }

        public void Occupy(int row, int column, Piece piece)
        {
            if (row < 0 || row > 7 || column < 0 || column > 7) return;
            if (EMPTY_STATE[(row * 8) + column] == 0) return;

            _data[(row * 8) + column] = (piece != null ? (piece.IsCrowned ? (piece.Colour == PieceColour.Black ? 3 : 5) : piece.Colour == PieceColour.Black ? 2 : 4) : 1);
        }

        public Board Clone()
        {
            return new Board(_game, _data);
        }

        private int[] ArrayFromStateString(string state)
        {
            int[] data = new int[64];
            int index = 0;
            foreach(char square in state)
            {
                data[index++] = square - 48;
            }
            return data;
        }

        private string StateAsString()
        {
            string state = "";
            foreach(int square in _data)
            {
                state += square.ToString();
            }
            return state;
        }

        internal Board(Game game)
        {
            _game = game;
            _data = ArrayFromStateString(DEFAULT_STATE); // default board state
        }

        internal Board(Game game, int[] data)
        {
            _game = game;
            _data = data; // default board state
        }
    }
}
