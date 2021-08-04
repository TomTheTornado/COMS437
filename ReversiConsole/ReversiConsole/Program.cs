using System;
using System.Threading;

namespace ReversiConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            bool inSession = true;
            bool playerGoesFirst = true;
            int depth = 1;
            Board board = new Board();
            board.Initialize();
            //Reads if you go 1st or 2nd
            Console.WriteLine("Would you like to go 1st or 2nd? Enter \"1\" or \"2\".");
            int input = Convert.ToInt32(Console.ReadLine());

            if (input == 2)
            {
                playerGoesFirst = false;
                board.switchPlayer();
            }

            //Reads AI Difficulty
            Console.WriteLine("Difficulty of the AI? Enter difficulty from \"1\" to \"10\".");
            int input2 = Convert.ToInt32(Console.ReadLine());
            depth = input2;
            board.maxDepth = depth;

            board.Draw();
            if (!playerGoesFirst)//if you go second
            {
                moveAI(board);
                board.Draw();
                board.switchPlayer();
                board.switchPiece();
            }
            while (inSession)//while actual game is running
            {
                if (board.isTerminal()) { inSession = false;  break; }//Checks to see if game is over

                //User Input
                GetInput(board);
                board.Draw();


                if (board.isTerminal()) { inSession = false; break; }//Checks to see if game is over

                //Gives the illusion the computer is thinking
                Console.Write("\nThinking.");
                Thread.Sleep(300);
                Console.Write(".");
                Thread.Sleep(300);
                Console.Write(".");
                Thread.Sleep(300);

                //AI move
                moveAI(board);
                board.Draw();

            }

            printScore(board);
        }

        /*Prints final results*/
        static void printScore(Board board)
        {
            double pScore = 0;
            double aiScore = 0;
            if (board.player == "Player")
            {
                pScore = board.getPlayerScore();
                board.switchPlayer();
                board.switchPiece();
                aiScore = board.getPlayerScore();
            }
            else if (board.player == "AI")
            {
                aiScore = board.getPlayerScore();
                board.switchPlayer();
                board.switchPiece();
                pScore = board.getPlayerScore();
            }

            Console.Write("Game Over.");
            if (pScore > aiScore)
            {
                Console.WriteLine("Player Wins! Score: " + pScore + "-" + aiScore);
            }
            else if (pScore < aiScore)
            {
                Console.WriteLine("AI Wins! Score: " + pScore + "-" + aiScore);
            }
            else
            {
                Console.WriteLine("Draw. Score: " + pScore + "-" + aiScore);
            }
        }

        /*Handles AI moves*/
        static void moveAI(Board board)
        {
            var result = minimax(board, board.player, board.maxDepth, 0, Double.NegativeInfinity, Double.PositiveInfinity);
            Move m = result.Item2;
            if (m != null)
            {
                Console.WriteLine("AI Move: " + (m.x + 1) + " " + (m.y + 1));
                board.flipBoard(m.y, m.x, board.playerPiece);
            }
        }

        /*Uses minimax algorithm to find best move*/
        static Tuple<double, Move> minimax(Board board, string player, int maxDepth, int curDepth, double alpha, double beta)
        {
            double bestScore = 0;
            Move bestMove = new Move(0, 0);

            if(curDepth == maxDepth || board.isTerminal())
            {
                return new Tuple<double, Move>(board.getPlayerScore(), null); //return score
            }

            if(board.player == player){bestScore = Double.NegativeInfinity;}
            else{bestScore = Double.PositiveInfinity;}

            foreach (Move m in board.getMoves()){
                Board copy = board.DeepCopy();
                copy.flipBoard(m.y, m.x, copy.playerPiece);


                var result = minimax(copy, player, maxDepth, curDepth + 1, alpha, beta);
                double currentScore = result.Item1;
                Move currentMove = result.Item2;
                
                if(board.player == player)
                {
                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        bestMove = m;
                    }
                    if (alpha < bestScore)
                    {
                        alpha = bestScore;
                    }
                    if(beta <= alpha) { break; }//Alpha beta pruning
                }
                else
                {
                    if(currentScore < bestScore)
                    {
                        bestScore = currentScore;
                        bestMove = m;
                    }
                    if (beta > bestScore)
                    {
                        beta = bestScore;
                    }
                    if (beta <= alpha) { break; }//Alpha beta pruning
                }
            }
            return new Tuple<double, Move>(bestScore, bestMove);
        }

        /*Gets Input of move*/
        static void GetInput(Board board)
        {
            Console.WriteLine();
            Console.WriteLine("Enter your desired cell as \"x y\" with both numbers being in the 1 to 8 range.");
            string[] inputs = Console.ReadLine().Split();
            if(String.IsNullOrWhiteSpace(inputs[0]) || inputs.Length < 2 || !Char.IsDigit(inputs[0], 0) || !Char.IsDigit(inputs[1], 0))
            {
                Console.WriteLine("\nInvalid input, please try again.");
                GetInput(board);
                return;
            }


            int x = int.Parse(inputs[0]); //convert string to numbers
            int y = int.Parse(inputs[1]);
            if(x < 1 || x > 8 || y < 1 || y > 8)
            {
                Console.WriteLine("\nInvalid input, please try again.");
                GetInput(board);
            }
            else if (board.legalMove(y-1,x-1, board.playerPiece))//if legal move, change
            {
                board.flipBoard(y - 1, x - 1, board.playerPiece);
            }
            else
            {
                Console.WriteLine("\nInvalid move, please try again.");
                GetInput(board);
            }

        }


    }
}
