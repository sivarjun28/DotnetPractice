using System;

namespace Exercise03
{
    // 1. Event data for PlayerMoved
    public class PlayerMovedEventArgs : EventArgs
    {
        public int X { get; init; }
        public int Y { get; init; }
        public DateTime Timestamp { get; init; }
    }

    // 2. Event data for ItemCollected
    public class ItemCollectedEventArgs : EventArgs
    {
        public string ItemName { get; init; }
        public int Points { get; init; }
    }

    // 3. Game class (event publisher)
    public class Game
    {
        // Events
        public event EventHandler<PlayerMovedEventArgs>? PlayerMoved;
        public event EventHandler<ItemCollectedEventArgs>? ItemCollected;
        public event EventHandler? GameOver;

        // Internal state
        private int playerX = 0;
        private int playerY = 0;
        private int score = 0;

        // Public read-only property
        public int Score => score;

        // Player movement method
        public void MovePlayer(int deltaX, int deltaY)
        {
            playerX += deltaX;
            playerY += deltaY;

            PlayerMoved?.Invoke(this, new PlayerMovedEventArgs
            {
                X = playerX,
                Y = playerY,
                Timestamp = DateTime.Now
            });
        }

        // Item collection method
        public void CollectItem(string itemName, int points)
        {
            score += points;

            ItemCollected?.Invoke(this, new ItemCollectedEventArgs
            {
                ItemName = itemName,
                Points = points
            });

            if (score >= 100)
            {
                GameOver?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    // 4. Logger class
    public class GameLogger
    {
        public void OnPlayerMoved(object? sender, PlayerMovedEventArgs e)
        {
            Console.WriteLine($"[{e.Timestamp:HH:mm:ss}] Player moved to ({e.X}, {e.Y})");
        }

        public void OnItemCollected(object? sender, ItemCollectedEventArgs e)
        {
            Console.WriteLine($"Collected {e.ItemName} (+{e.Points} points)");
        }
    }

    // 5. Score tracker class
    public class ScoreTracker
    {
        public void OnItemCollected(object? sender, ItemCollectedEventArgs e)
        {
            if (sender is Game game)
            {
                Console.WriteLine($"Total Score: {game.Score}");
            }
        }
    }

    // 6. Program entry point
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            GameLogger logger = new GameLogger();
            ScoreTracker tracker = new ScoreTracker();

            // Subscribe to events
            game.PlayerMoved += logger.OnPlayerMoved;
            game.ItemCollected += logger.OnItemCollected;
            game.ItemCollected += tracker.OnItemCollected;

            // Lambda subscription
            EventHandler<PlayerMovedEventArgs> warningHandler = (sender, e) =>
            {
                if (e.X > 10 || e.Y > 10)
                    Console.WriteLine("Warning: Player is far from origin!");
            };

            game.PlayerMoved += warningHandler;

            // Play game
            game.MovePlayer(5, 3);
            game.CollectItem("Coin", 10);

            game.MovePlayer(2, -1);
            game.CollectItem("Gem", 25);

            game.MovePlayer(15, 15);

            // Unsubscribe example
            Console.WriteLine("\nUnsubscribing logger from PlayerMoved...\n");
            game.PlayerMoved -= logger.OnPlayerMoved;

            // Test after unsubscribe
            game.MovePlayer(1, 1);
        }
    }
}