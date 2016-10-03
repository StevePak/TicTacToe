using System;

namespace TicTacToe
{
    class Program
    {
        const string playerToken = "XOABCDEFGHIJKLMNPQRSTUVWYZ";
        static void Main(string[] args)
        {
            
            int numPlayers, boardSize, winCondition, currentPlayer;
            char[,] gameBoard;


            //Initialize Game State - via input or save file
            #region Initialize Game State

            Console.WriteLine("Welcome to TicTacToe! Would you like to resume a game? (y/n)");
            char continueGame = Console.ReadKey().KeyChar;
            if (Char.ToLower(continueGame) == 'y')
            {
                numPlayers = 2;
                boardSize = 3;
                winCondition = 3;
                currentPlayer = 0;
                gameBoard = new char[3, 3];
            }
            else
            {
                //Input number of players
                Console.WriteLine("Please enter the number of players:");
                while(!int.TryParse(Console.ReadLine(), out numPlayers) || numPlayers < 2 || numPlayers > 26)
                {
                    Console.WriteLine("That is not a valid value. Please try again:");
                }

                //Input game board size
                Console.WriteLine("Please enter the game board dimensions:");
                while (!int.TryParse(Console.ReadLine(), out boardSize) || boardSize < 3 || boardSize > 999)
                {
                    Console.WriteLine("That is not a valid value. Please try again:");
                }
                gameBoard = new char[boardSize, boardSize];

                //Input win condition
                Console.WriteLine("Please enter the win condition sequence count:");
                while (!int.TryParse(Console.ReadLine(), out winCondition) || winCondition <= 2 || winCondition > boardSize)
                {
                    Console.WriteLine("That is not a valid value. Please try again:");
                }
                currentPlayer = 0;
            }
            #endregion

            #region Game Play
            bool gameWon = false;
            while (!gameWon)
            {
                gameWon = GamePlay(gameBoard, currentPlayer, numPlayers, winCondition);
            }

            #endregion
        }

        private static bool GamePlay(char[,] gameBoard, int currentPlayer, int numPlayers, int winCondition)
        {
            throw new NotImplementedException();

            //print board
            for (int i = currentPlayer; i < numPlayers; i++)
            {
                char token = playerToken[i];
                //get row input
                //get column input
            }
        }
    }
}
