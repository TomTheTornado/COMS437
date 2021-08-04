using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiConsole
{
    class Board
    {

        public Piece[,] board;
        public string player;
        public string otherPlayer;
        public char playerPiece;
        public char otherPiece;
        public int maxDepth;
        public Board()
        {
            board = new Piece[8, 8];
            player = "Player";
            otherPlayer = "AI";
            playerPiece = 'X';
            otherPiece = 'O';
        }

        /*Constructor Used for deepcopy*/
        public Board(Piece[,] p, string play, string oPlay, char piece, char oPiece, int depth)
        {
            board = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                //Console.WriteLine(p[i, 0].character + " " + p[i, 0].empty);
                board[i, 0] = p[i, 0].DeepCopy();
                board[i, 1] = p[i, 1].DeepCopy();
                board[i, 2] = p[i, 2].DeepCopy();
                board[i, 3] = p[i, 3].DeepCopy();
                board[i, 4] = p[i, 4].DeepCopy();
                board[i, 5] = p[i, 5].DeepCopy();
                board[i, 6] = p[i, 6].DeepCopy();
                board[i, 7] = p[i, 7].DeepCopy();
            }
            this.player = play;
            this.otherPlayer = oPlay;
            this.playerPiece = piece;
            this.otherPiece = oPiece;
            this.maxDepth = depth;
        }

        /*Copy board for the minimax*/
        public Board DeepCopy()
        {
            Board boardDeepCopy = new Board(this.board, this.player, this.otherPlayer, this.playerPiece, this.otherPiece, this.maxDepth);

            return boardDeepCopy;
        }

        public void setPlayer(string s)
        {
            player = s;
        }

        /*Switch Player*/
        public void switchPlayer()
        {
            if(player == "Player"){player = "AI"; otherPlayer = "Player"; }
            else { player = "Player"; otherPlayer = "AI"; }
        }

        /*Switch Assigned Pieces*/
        public void switchPiece()
        {
            if (playerPiece == 'X') { playerPiece = 'O'; otherPiece = 'X'; }
            else { playerPiece = 'X'; otherPiece = 'O'; }
        }

        /*Initializes game board*/
        public void Initialize()
        {
            for (int i = 0; i < 8; i++)
            {
                board[i, 0] = new Piece();
                board[i, 1] = new Piece();
                board[i, 2] = new Piece();
                board[i, 3] = new Piece();
                board[i, 4] = new Piece();
                board[i, 5] = new Piece();
                board[i, 6] = new Piece();
                board[i, 7] = new Piece();
            }
            board[3, 3].setP1();
            board[3, 4].setP2();
            board[4, 3].setP2();
            board[4, 4].setP1();
        }

        /*Draws game board*/
        public void Draw()
        {
            Console.WriteLine("\n12345678\n");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(board[i, 0].character);
                Console.Write(board[i, 1].character);
                Console.Write(board[i, 2].character);
                Console.Write(board[i, 3].character);
                Console.Write(board[i, 4].character);
                Console.Write(board[i, 5].character);
                Console.Write(board[i, 6].character);
                Console.Write(board[i, 7].character);
                Console.WriteLine(" " + (i+1));
            }
        }

        /*Flips pieces on the gameboard*/
        public void flipBoard(int y, int x, char c)
        {
            Piece p = board[y, x];
            char p1, p2;
            if (c == 'X') { p1 = 'X'; p2 = 'O'; board[y, x].setP1(); }
            else { p1 = 'O'; p2 = 'X'; board[y, x].setP2(); }


            int i = 0;
            bool skip = true;
            //CHANGES CARDINAL DIRECTIONS
            //Changes below
            for (i = y + 1; i < 8; i++)
            {
                if(board[i, x].character == p2) { continue; }
                else if (board[i, x].character == p1) { skip = false; break; }
                else if (board[i, x].character == '.') { break; }
            }
            if (!skip)
            {
                for (int j = y; j < i; j++)
                {
                    setBoard(j, x, p1);
                }
            }

            //Changes above
            skip = true;
            for (i = y - 1; i >= 0; i--)
            {
                if (board[i, x].character == p2) { continue; }
                else if (board[i, x].character == p1) { skip = false; break; }
                else if (board[i, x].character == '.') { break; }
            }
            if (!skip)
            {
                for (int j = y; j > i; j--)
                {
                    setBoard(j, x, p1);
                }
            }

            //Changes right
            skip = true;
            for (i = x + 1; i < 8; i++)
            {
                if (board[y, i].character == p2) { continue; }
                else if (board[y, i].character == p1) { skip = false; break; }
                else if (board[y, i].character == '.') { break; }
            }
            if (!skip)
            {
                for (int j = x; j < i; j++)
                {
                    setBoard(y, j, p1);
                }
            }

            //Changes left
            skip = true;
            for (i = x - 1; i >= 0; i--)
            {
                if (board[y, i].character == p2) { continue; }
                else if (board[y, i].character == p1) { skip = false; break; }
                else if (board[y, i].character == '.') { break; }
            }
            if (!skip)
            {
                for (int j = x; j > i; j--)
                {
                    setBoard(y, j, p1);
                }
            }

            //CHANGES DIAGONALS
            //Changes bottom right
            int k = x + 1;
            skip = true;
            for (i = y + 1; i < 8; i++)
            {
                if (k < 8)
                {
                    if (board[i, k].character == p2) { k++; continue; }
                    else if (board[i, k].character == p1) { skip = false; break; }
                    else if (board[i, k].character == '.') { break; }
                    
                }
                else { break; }
            }
            int l = x;
            if (!skip)
            {
                for (int j = y; j < i; j++)
                {
                    setBoard(j, l, p1);
                    l++;
                }
            }

            //Changes bottom left
            k = x - 1;
            skip = true;
            for (i = y + 1; i < 8; i++)
            {
                if (k >= 0)
                {
                    if (board[i, k].character == p2) { k--; continue; }
                    else if (board[i, k].character == p1) { skip = false; break; }
                    else if (board[i, k].character == '.') { break; }
                }
                else { break; }
            }
            l = x;
            if (!skip)
            {
                for (int j = y; j < i; j++)
                {
                    setBoard(j, l, p1);
                    l--;
                }
            }

            //Changes top right
            k = x + 1;
            skip = true;
            for (i = y - 1; i >= 0; i--)
            {
                if (k < 8)
                {
                    if (board[i, k].character == p2) { k++; continue; }
                    else if (board[i, k].character == p1) { skip = false; break; }
                    else if (board[i, k].character == '.') { break; }
                    
                }
                else { break; }
            }
            l = x;
            if (!skip)
            {
                for (int j = y; j > i; j--)
                {
                    setBoard(j, l, p1);
                    l++;
                }
            }

            //Changes top left
            k = x - 1;
            skip = true;
            for (i = y - 1; i >= 0; i--)
            {
                if (k >= 0)
                {
                    if (board[i, k].character == p2) { k--; continue; }
                    else if (board[i, k].character == p1) { skip = false; break; }
                    else if (board[i, k].character == '.') { break; }
                    
                }
                else { break; }
            }
            l = x;
            if (!skip)
            {
                for (int j = y; j > i; j--)
                {
                    setBoard(j, l, p1);
                    l--;
                }
            }
            switchPlayer();
            switchPiece();
           
        }

        /*Changes the game board*/
        private void setBoard(int y, int x, char p1)
        {
            if (p1 == 'X') { board[y, x].setP1(); }
            else if (p1 == 'O') { board[y, x].setP2(); }
        }

        /*Checks if legal move*/
        public bool legalMove(int y, int x, char c)
        {
            Piece p = board[y, x];
            char p1, p2;
            if (c == 'X') { p1 = 'X'; p2 = 'O'; }
            else { p1 = 'O'; p2 = 'X'; }

            //Makes sure that the desired move is empty on the board
            if (p.empty)
            {
                bool p1Check = false;
                bool p2Check = false;
                //CHECK CARDINAL DIRECTIONS
                //Checks below
                for (int i = y + 1; i < 8; i++)
                {
                    if (board[i, x].character == p2) { p2Check = true; }
                    else if (board[i, x].character == p1) { p1Check = true; break; }
                    else { break; }
                }
                if (p1Check && p2Check) { return true; }
                else { p1Check = false; p2Check = false; }

                //Checks above
                for (int i = y - 1; i >= 0; i--)
                {
                    if (board[i, x].character == p2) { p2Check = true; }
                    else if (board[i, x].character == p1) { p1Check = true; break; }
                    else { break; }
                }
                if (p1Check && p2Check) { return true; }
                else { p1Check = false; p2Check = false; }

                //Checks right
                for (int i = x + 1; i < 8; i++)
                {
                    if (board[y, i].character == p2) { p2Check = true; }
                    else if (board[y, i].character == p1) { p1Check = true; break; }
                    else { break; }
                }
                if (p1Check && p2Check) { return true; }
                else { p1Check = false; p2Check = false; }

                //Checks left
                for (int i = x - 1; i >= 0; i--)
                {
                    if (board[y, i].character == p2) { p2Check = true; }
                    else if (board[y, i].character == p1) { p1Check = true; break; }
                    else { break; }
                }
                if (p1Check && p2Check) { return true; }
                else { p1Check = false; p2Check = false; }


                //CHECK DIAGONALS
                //Checks bottom right
                int j = x + 1;
                for (int i = y + 1; i < 8; i++)
                {
                    if(j < 8)
                    {
                        if (board[i, j].character == p2) { p2Check = true; }
                        else if (board[i, j].character == p1) { p1Check = true; break; }
                        else { break; }
                        j++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (p1Check && p2Check) { return true; }
                else { p1Check = false; p2Check = false; }

                //Checks bottom left
                j = x - 1;
                for (int i = y + 1; i < 8; i++)
                {
                    if (j >= 0)
                    {
                        if (board[i, j].character == p2) { p2Check = true; }
                        else if (board[i, j].character == p1) { p1Check = true; break; }
                        else { break; }
                        j--;
                    }
                    else
                    {
                        break;
                    }
                }
                if (p1Check && p2Check) { return true; }
                else { p1Check = false; p2Check = false; }

                //Checks top right
                j = x + 1;
                for (int i = y - 1; i >= 0; i--)
                {
                    if (j < 8)
                    {
                        if (board[i, j].character == p2) { p2Check = true; }
                        else if (board[i, j].character == p1) { p1Check = true; break; }
                        else { break; }
                        j++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (p1Check && p2Check) { return true; }
                else { p1Check = false; p2Check = false; }

                //Checks top left
                j = x - 1;
                for (int i = y - 1; i >= 0; i--)
                {
                    if (j >= 0)
                    {
                        if (board[i, j].character == p2) { p2Check = true; }
                        else if (board[i, j].character == p1) { p1Check = true; break; }
                        else { break; }
                        j--;
                    }
                    else
                    {
                        break;
                    }
                }
                if (p1Check && p2Check) { return true; }
                else { p1Check = false; p2Check = false; }


                return false;
            }
            return false;
        }

        /*Checks if any moves are left for existing piece*/
        public bool anyMoves(char piece)
        {
            for(int i = 0; i<8; i++)
            {
                for(int j = 0; j<8; j++)
                {
                    if (legalMove(i, j, piece))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /*Returns all possible moves left for the player piece*/
        public Move[] getMoves()
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (legalMove(i, j, playerPiece))
                    {
                        moves.Add(new Move(j, i));
                    }
                }
            }
            Move[] arr = moves.ToArray();
            return arr;
        }

        /*Checks if no moves left for player*/
        public bool isTerminal()
        {
            if(anyMoves(playerPiece))
            {
                return false;
            }
            return true;
        }

        /*Gets the score for the current player*/
        public double getPlayerScore()
        {
            double score = 0.0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[j,i].character == playerPiece)
                    {
                        score++;
                    }
                }
            }
            return score;
        }
    }
}
