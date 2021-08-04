using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiConsole
{
    class Piece
    {
        public bool empty;
        public char character;

        public Piece()
        {
            character = '.';
            empty = true;
        }

        /*Constructor Used for deepcopy*/
        public Piece(char c, bool e)
        {
            this.character = c;
            this.empty = e;
        }

        /*Used to copy pieces for board for the minimax*/
        public Piece DeepCopy()
        {
            Piece pieceDeepCopy = new Piece(this.character, this.empty);

            return pieceDeepCopy;
        }

        //Sets piece to whoever is player 1
        public void setP1()
        {
            character = 'X';
            empty = false;
        }

        //Sets piece to whoever is player 2
        public void setP2()
        {
            character = 'O';
            empty = false;
        }

        //Sets piece to empty
        public void setEmpty()
        {
            character = '.';
            empty = true;
        }

    }
}
