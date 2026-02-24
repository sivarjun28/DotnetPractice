using System;

class Exercise05
{
    static void Main()
    {
        int totalGames = 0, totalAttempts = 0, highScore = int.MaxValue;
        double averageAttempts = 0;
        bool playAgain = true;

        while (playAgain)
        {
            totalGames++;
            int attempts = 0;
            int targetNumber = GenerateRandomNumber();
            int score = int.MaxValue;
            int difficulty = SelectDifficulty();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nWelcome to the Number Guessing Game!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Guess the number between 1 and {difficulty}");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Enter your guess: ");
                Console.ForegroundColor = ConsoleColor.White;

                string input = Console.ReadLine();
                if (int.TryParse(input, out int guess))
                {
                    attempts++;
                    if (guess == targetNumber)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Correct! The number was {targetNumber}. It took you {attempts} attempts.");
                        score = attempts;
                        break;
                    }
                    else if (guess < targetNumber)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Higher! Try again.");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Lower! Try again.");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

            // Calculate high score and statistics
            if (score < highScore)
            {
                highScore = score;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"New High Score! {highScore} attempts.");
            }

            totalAttempts += attempts;
            averageAttempts = totalAttempts / (double)totalGames;

            // Ask user if they want to play again
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Do you want to play again? (y/n): ");
            Console.ForegroundColor = ConsoleColor.White;

            string playAgainInput = Console.ReadLine().ToLower();
            if (playAgainInput != "y" && playAgainInput != "yes")
            {
                playAgain = false;
            }
        }

        // Display statistics after game ends
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nGame Over! You played {totalGames} games with an average of {averageAttempts:F2} attempts.");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Your highest score was {highScore} attempts.");
        Console.ForegroundColor = ConsoleColor.White;
    }

    // Method to generate random number based on difficulty
    static int GenerateRandomNumber()
    {
        int difficulty = SelectDifficulty();
        Random random = new Random();
        return random.Next(1, difficulty + 1);
    }

    // Method to select difficulty level
    static int SelectDifficulty()
    {
        int difficulty = 100; // Default to Medium

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nSelect Difficulty: ");
        Console.WriteLine("1. Easy (1-10)");
        Console.WriteLine("2. Medium (1-50)");
        Console.WriteLine("3. Hard (1-100)");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Enter choice (1/2/3): ");

        string choice = Console.ReadLine();
        if (choice == "1")
        {
            difficulty = 10;
        }
        else if (choice == "2")
        {
            difficulty = 50;

        }
        else if (choice == "3")
        {
            difficulty = 100;
        }

        return difficulty;
    }
}