using System;
using System.Collections.Generic;

class TicTacToe
{
    // Variables to track the board, players, and score
    static char[,] board;
    static Dictionary<string, int> score = new Dictionary<string, int> { { "X", 0 }, { "O", 0 } };
    static int boardSize = 3; // Default to 3x3 board

    static void Main(string[] args)
    {
        string playAgain = "y";
        while (playAgain.ToLower() == "y")
        {
            Console.Clear();
            SetupBoard();
            DisplayBoard();
            char? winner = null; // Nullable char for winner
            bool isDraw = false;
            string currentPlayer = "X";

            // Play until there's a winner or a draw
            while (winner == null && !isDraw)
            {
                Console.WriteLine($"Player {currentPlayer}, it's your turn.");
                int row, col;
                if (currentPlayer == "O" && IsAIEnabled())  // AI plays for "O"
                {
                    (row, col) = GetAIMove();
                    Console.WriteLine($"AI plays at {row + 1},{col + 1}");
                }
                else
                {
                    (row, col) = GetPlayerMove();
                }

                // Make the move
                board[row, col] = currentPlayer[0]; // Use the char value of currentPlayer
                DisplayBoard();

                // Check for a winner
                winner = CheckWinner();
                if (winner.HasValue) // Nullable check for winner
                {
                    Console.WriteLine($"Player {winner} wins!");
                    score[winner.ToString()]++; // Update score for the winner
                    break;
                }

                // Check for draw
                isDraw = CheckDraw();

                // Switch player
                currentPlayer = (currentPlayer == "X") ? "O" : "X";
            }

            // If it's a draw
            if (isDraw)
            {
                Console.WriteLine("It's a draw!");
            }

            // Display current score
            DisplayScore();

            // Ask to play again
            Console.WriteLine("Do you want to play again? (y/n): ");
            playAgain = Console.ReadLine();
        }

        Console.WriteLine("Thanks for playing!");
    }

    // Set up the board with default or custom size
    static void SetupBoard()
    {
        Console.WriteLine("Enter the size of the board (3, 4, 5): ");
        string sizeInput = Console.ReadLine();
        boardSize = (sizeInput == "4" || sizeInput == "5") ? int.Parse(sizeInput) : 3;

        board = new char[boardSize, boardSize];
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                board[i, j] = '-';
            }
        }
    }

    // Display the board
    static void DisplayBoard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                Console.Write(board[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    // Get a player's move with validation
    static (int, int) GetPlayerMove()
    {
        int row = -1, col = -1;
        bool validMove = false;

        while (!validMove)
        {
            Console.WriteLine("Enter row and column (1-based): ");
            string[] input = Console.ReadLine().Split(',');

            if (input.Length == 2 && int.TryParse(input[0], out row) && int.TryParse(input[1], out col))
            {
                row--; col--; // Convert to 0-based index
                if (row >= 0 && row < boardSize && col >= 0 && col < boardSize && board[row, col] == '-')
                {
                    validMove = true;
                }
                else
                {
                    Console.WriteLine("Invalid move! Try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input! Please enter row and column in format: row,col");
            }
        }

        return (row, col);
    }

    // AI makes a random valid move
    static (int, int) GetAIMove()
    {
        Random rand = new Random();
        int row, col;
        do
        {
            row = rand.Next(boardSize);
            col = rand.Next(boardSize);
        } while (board[row, col] != '-');

        return (row, col);
    }

    // Check if a player has won
    static char? CheckWinner()
    {
        // Check rows
        for (int i = 0; i < boardSize; i++)
        {
            if (AllEqual(board[i, 0], board[i, 1], board[i, 2]))
                return board[i, 0]; // Return a char
        }

        // Check columns
        for (int i = 0; i < boardSize; i++)
        {
            if (AllEqual(board[0, i], board[1, i], board[2, i]))
                return board[0, i];
        }

        // Check diagonals
        if (AllEqual(board[0, 0], board[1, 1], board[2, 2]))
            return board[0, 0];
        if (AllEqual(board[0, 2], board[1, 1], board[2, 0]))
            return board[0, 2];

        return null;
    }

    // Check if all items are equal (used for rows/columns/diagonals)
    static bool AllEqual(char a, char b, char c)
    {
        return a == b && b == c && a != '-';
    }

    // Check if the board is full (draw)
    static bool CheckDraw()
    {
        foreach (var cell in board)
        {
            if (cell == '-')
                return false;
        }
        return true;
    }

    // Display current score
    static void DisplayScore()
    {
        Console.WriteLine("Current score:");
        foreach (var player in score)
        {
            Console.WriteLine($"{player.Key}: {player.Value}");
        }
    }

    // Check if AI is enabled (playing as O)
    static bool IsAIEnabled()
    {
        Console.WriteLine("Enable AI opponent? (y/n): ");
        return Console.ReadLine().ToLower() == "y";
    }
}
