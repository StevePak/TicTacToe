﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TicTacToe
{
    class Program
    {
        private const string playerToken = " XOABCDEFGHIJKLMNPQRSTUVWYZ";
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
                Console.WriteLine("Please enter the file name that you want to load the game from:");
                string fileName = Console.ReadLine();


                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                GameState state = (GameState)formatter.Deserialize(stream);
                stream.Close();
                numPlayers = state.numPlayers;
                boardSize = state.boardSize;
                winCondition = state.winCondition;
                currentPlayer = state.currentPlayer;
                gameBoard = state.gameBoard;
            }
            else
            {
                //Input number of players
                Console.WriteLine("Please enter the number of players:");
                while (!int.TryParse(Console.ReadLine(), out numPlayers) || numPlayers < 2 || numPlayers > 26)
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
                currentPlayer = 1;
            }
            #endregion

            GamePlay(gameBoard, currentPlayer, numPlayers, winCondition, boardSize);
        }

        private static void GamePlay(char[,] gameBoard, int currentPlayer, int numPlayers, int winCondition, int boardSize)
        {
            bool gameWon = false;
            while (!gameWon)
            {
               
                
                for (int i = currentPlayer; i <= numPlayers; i++)
                {
                    printBoard(gameBoard, boardSize);
                    char token = playerToken[i];
                    Console.WriteLine($"Player {i}, token {token}:");
                    int row = -1;
                    int col = -1;
                    
                    do
                    {
                        Console.WriteLine("Please enter the row or type s to save:");
                        string rowOrSave = Console.ReadLine().Trim().ToLower();
                        if(rowOrSave == "s")
                        {
                            saveGame(gameBoard, i, numPlayers, winCondition, boardSize);
                            Console.WriteLine("Game successfully saved");
                            return;
                        }
                        while (!int.TryParse(rowOrSave, out row) || row <= 0 || row > boardSize)
                        {
                            Console.WriteLine("That is not a valid value. Please try again:");
                            rowOrSave = Console.ReadLine().Trim().ToLower();
                        }
                        Console.WriteLine("Please enter the column:");
                        while (!int.TryParse(Console.ReadLine(), out col) || col <= 0 || col > boardSize)
                        {
                            Console.WriteLine("That is not a valid value. Please try again:");
                        }
                        row--;
                        col--;
                        if (gameBoard[row, col] != '\0')
                        {
                            Console.WriteLine("This spot is taken. Please try again");
                        }
                    } while (gameBoard[row, col] != '\0');
                    gameBoard[row, col] = token;
                    bool win = checkWin(gameBoard, row, col, winCondition, boardSize);
                    if (win)
                    {
                        Console.WriteLine($"Congratulations! Player {i} won the game!");
                        printBoard(gameBoard, boardSize);
                        return;
                    }
                }
                currentPlayer = 1;
            }
        }

        private static bool checkWin(char[,] gameBoard, int row, int column, int winCondition, int boardSize)
        {
            char token = gameBoard[row, column];
            //check left to right
            int rowCheck = row;
            int colCheck = column;
            while (colCheck >= 0 && gameBoard[rowCheck, colCheck] == token)
            {
                colCheck--;
            }
            colCheck++;

            int count = 0;

            while (colCheck < boardSize && gameBoard[rowCheck, colCheck] == token)
            {
                count++;
                colCheck++;     
            }

            if (count >= winCondition)
            {
                return true;
            }
            //check up to down

            rowCheck = row;
            colCheck = column;

            while (rowCheck >= 0 && gameBoard[rowCheck, colCheck] == token)
            {
                rowCheck--;
            }
            rowCheck++;

            count = 0;

            while (rowCheck < boardSize && gameBoard[rowCheck, colCheck] == token)
            {
                count++;
                rowCheck++;
            }

            if (count >= winCondition)
            {
                return true;
            }
            //check top left to bottom right

            rowCheck = row;
            colCheck = column;

            while (rowCheck >= 0 && colCheck >= 0 && gameBoard[rowCheck, colCheck] == token)
            {
                rowCheck--;
                colCheck--;
            }
            rowCheck++;
            colCheck++;

            count = 0;

            while (rowCheck < boardSize && colCheck < boardSize && gameBoard[rowCheck, colCheck] == token)
            {
                count++;
                rowCheck++;
                colCheck++;
            }

            if (count >= winCondition)
            {
                return true;
            }

            //check bottom left to top right

            rowCheck = row;
            colCheck = column;

            while (rowCheck < boardSize && colCheck > 0 && gameBoard[rowCheck, colCheck] == token)
            {
                rowCheck++;
                colCheck--;
            }
            rowCheck--;
            colCheck++;

            count = 0;

            while (rowCheck >= 0 && colCheck < boardSize && gameBoard[rowCheck, colCheck] == token)
            {
                count++;
                rowCheck--;
                colCheck++;
            }

            if (count >= winCondition)
            {
                return true;
            }
            return false;
        }

        private static void saveGame(char[,] gameBoard, int currentPlayer, int numPlayers, int winCondition, int boardSize)
        {
            Console.WriteLine("Please enter the file name that you want to save the game to:");
            string fileName = Console.ReadLine();
            var state = new GameState();
            state.gameBoard = gameBoard;
            state.currentPlayer = currentPlayer;
            state.numPlayers = numPlayers;
            state.winCondition = winCondition;
            state.boardSize = boardSize;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, state);
            stream.Close();
        }

        private static void printBoard(char[,] gameBoard, int boardSize)
        {
            string firstLine = " ";
            for (int i = 1; i <= boardSize; i++)
            {
                firstLine += $"   {i}";
            }
            Console.WriteLine(firstLine);

            for (int i = 0; i < boardSize; i++)
            {
                string line1 = $"{i + 1}  ";
                string line2 = "   ";
                for (int j = 0; j < boardSize; j++)
                {
                    line1 += $" {gameBoard[i, j]} ";
                    line2 += "---";
                    if (j != boardSize - 1)
                    {
                        line1 += "|";
                        line2 += "+";
                    }
                    
                }
                if (i == boardSize - 1)
                {
                    line2 = "";
                }
                Console.WriteLine(line1);
                Console.WriteLine(line2);
            }
        }
    }

    [Serializable]
    public class GameState
    {
        public char[,] gameBoard { get; set; }
        public int currentPlayer { get; set; }
        public int numPlayers { get; set; }
        public int winCondition { get; set; }
        public int boardSize { get; set; }
    }
}

