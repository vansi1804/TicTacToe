using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Minimax_AlphaBeta
    {
        private int maxDepth;
        public Minimax_AlphaBeta(int maxDepth)
        {
            this.maxDepth = maxDepth;
        }

        public int alphaBetaPruning(Board board, Board.State player, int depth, double alpha, double beta)
        {
             Board.State opponent = (player == Board.State.X) ? Board.State.O : Board.State.X;

            if (board.isGameOver() || depth++ == maxDepth)
            {
                return score(board, player);
            }

            if (board.getTurn() == player)
            {
                int indexOfBestMove = -1;

                List<int> moves = board.getAvailableStates();
                for (int i = 0; i< moves.Count; i++)
                {
                    int theMove = moves[i];
                    Board modifiedBoard = board.getCopyDeep();
                    modifiedBoard.move(theMove);
                    int score = alphaBetaPruning(modifiedBoard, player,depth, alpha, beta);

                    if (score > alpha)
                    {
                        alpha = score;
                        indexOfBestMove = theMove;
                    }
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                if (indexOfBestMove != -1)
                {
                    board.move(indexOfBestMove);
                }
                return (int)alpha;
            }
            else 
            {
                int indexOfBestMove = -1;

                List<int> moves = board.getAvailableStates();
                for (int i = 0; i < moves.Count; i++)
                {
                    int theMove = moves[i];
                    Board modifiedBoard = board.getCopyDeep();
                    modifiedBoard.move(theMove);

                    int score = alphaBetaPruning(modifiedBoard, player, depth, alpha, beta);

                    if (score < beta)
                    {
                        beta = score;
                        indexOfBestMove = theMove;
                    }
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                if (indexOfBestMove != -1)
                {
                    board.move(indexOfBestMove);
                }
                return (int)beta;
            }
        }


        public int score(Board board, Board.State player)
        {
            Board.State opponent = (player == Board.State.X) ? Board.State.O : Board.State.X;
            if (board.isGameOver() && board.getWinner() == player)
            {
                return 1;
            }
            else if (board.isGameOver() && board.getWinner() == opponent)
            {
                return -1;
            }
            else
            {
                return board.heuristic();
            }
        }
    }
}
