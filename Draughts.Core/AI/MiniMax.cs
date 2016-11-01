﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class MiniMax
    {
        private Game _game;
        private Player _player;

        public Move FindBestMove(IEnumerable<Move> validMoves, int maxMovesAhead)
        {
            // minimax algorithm plays n moves ahead and notes which move at the first level leads to the least opponent pieces remaining and the most player pieces remaining
            // which it recommends as the best move
            Board newBoard = _game.Board.Clone();
            MiniMaxResult playerResult = new MiniMaxResult();
            MiniMaxResult opponentResult = new MiniMaxResult();
            GenerateMiniMax(newBoard, _player, 0, 0, maxMovesAhead, playerResult, opponentResult);

            Move bestMove = playerResult.BestMove;

            // if best move takes no pieces but valid moves do take pieces, we have to pick one (any one) that takes pieces
            if (bestMove != null && bestMove.PiecesTaken == 0 && validMoves.Any(m => m.PiecesTaken > 0))
                return validMoves.Where(m => m.PiecesTaken > 0).RandomFirstOrDefault();

            // if no moves take any pieces but there are crowned pieces in the lower half of the opposite board, filter only the
            // backwards moves to encourage them to move back into the middle
            // a sufficient MiniMax depth would reveal this strategy but would take too long :-(
            if (bestMove != null && bestMove.PiecesTaken == 0)
            {
                IEnumerable<Move> crownedMoves = validMoves.Where(m => m.PieceIsCrowned &&
                ((_player.Colour == PieceColour.Black && m.Start.Row > 3 && m.End.Row < m.Start.Row) || (_player.Colour == PieceColour.White && m.Start.Row < 4 && m.End.Row > m.Start.Row)));
                if (crownedMoves.Count() > 0)
                    return crownedMoves.RandomFirstOrDefault();
            }

            // sometimes there is no best move within n moves ahead (all moves equally good / bad), so just use a random choice of the valid moves available
            if (bestMove == null)
                return validMoves.RandomFirstOrDefault();

            // otherwise, return the recommended best move
            return bestMove;
        }

        private void GenerateMiniMax(Board board, Player player, int playerGeneration, int opponentGeneration, int maxGenerations, MiniMaxResult playerResult, MiniMaxResult opponentResult)
        {
            playerGeneration++;
            if (playerGeneration >= maxGenerations || playerResult.OpponentPiecesRemaining == 0)
            {
                return; // we've reached the limit, or a winning move has been found already
            }

            IEnumerable<Move> validMoves = board.ValidMovesFor(player);
            if (!playerResult.BestMovePerGeneration.ContainsKey(playerGeneration))
            {
                playerResult.BestMovePerGeneration.Add(playerGeneration, null);
            }

            foreach (Move move in validMoves.Randomize()) // randomize the move order to avoid first-place bias for sets of equally good moves
            {
                Board newBoard = board.Clone();
                newBoard[move.Start.Row, move.Start.Column].Occupier.Move(move);

                int playerCount = newBoard.Squares.Count(s => s.IsOccupied && s.Occupier.Owner == player);
                int opponentCount = newBoard.Squares.Count(s => s.IsOccupied && s.Occupier.Owner == player.Opponent);
                if (opponentCount <= playerResult.OpponentPiecesRemaining || playerCount > opponentResult.PlayerPiecesRemaining 
                    || playerResult.BestMovePerGeneration[playerGeneration] == null)
                {
                    playerResult.PlayerPiecesRemaining = playerCount;
                    playerResult.OpponentPiecesRemaining = opponentCount;
                    playerResult.BestMovePerGeneration[playerGeneration] = move;
                    GenerateMiniMax(newBoard, player.Opponent, opponentGeneration, playerGeneration, maxGenerations, opponentResult, playerResult);
                }
            }
        }

        public MiniMax(Game game, Player player)
        {
            _game = game;
            _player = player;
        }
    }
}
