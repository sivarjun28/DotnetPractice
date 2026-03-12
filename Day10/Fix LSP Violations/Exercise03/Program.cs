using System;

// Base class for all birds
public abstract class Bird
{
    public string Name { get; set; } = string.Empty;

    public abstract void Move();  // All birds can move
}

// For birds that can fly
public abstract class FlyingBird : Bird
{
    public abstract void Fly();  // Only FlyingBirds have this capability
}

// For birds that can't fly
public abstract class NonFlyingBird : Bird
{
    // NonFlyingBirds don't have the `Fly` method
}

// Concrete classes for specific birds

// Sparrow can fly
public class Sparrow : FlyingBird
{
    public override void Move()
    {
        Console.WriteLine($"{Name} is hopping");
    }

    public override void Fly()
    {
        Console.WriteLine($"{Name} is flying fast");
    }
}

// Penguin can't fly
public class Penguin : NonFlyingBird
{
    public override void Move()
    {
        Console.WriteLine($"{Name} is swimming");
    }
}

// Ostrich can't fly
public class Ostrich : NonFlyingBird
{
    public override void Move()
    {
        Console.WriteLine($"{Name} is running");
    }
}

// Methods to test substitutability



// Test Code
public class Program
{
    public static void Main()
    {

        void MakeBirdMove(Bird bird)
        {
            bird.Move();  // All birds can move, so no exceptions here
        }

        void MakeFlyingBirdFly(FlyingBird bird)
        {
            bird.Fly();  // Only FlyingBirds can fly
        }
        // Create different birds
        Bird sparrow = new Sparrow { Name = "Sparrow" };
        Bird penguin = new Penguin { Name = "Penguin" };
        Bird ostrich = new Ostrich { Name = "Ostrich" };

        // Test movement (all birds can move)
        MakeBirdMove(sparrow);   // Output: Sparrow is hopping
        MakeBirdMove(penguin);   // Output: Penguin is swimming
        MakeBirdMove(ostrich);   // Output: Ostrich is running

        // Test flying (only flying birds can fly)
        MakeFlyingBirdFly((FlyingBird)sparrow);  // Output: Sparrow is flying fast
        // MakeFlyingBirdFly((FlyingBird)penguin);  // Throws exception at runtime (uncomment to see error)
        // MakeFlyingBirdFly((FlyingBird)ostrich);  // Throws exception at runtime (uncomment to see error)
    }
}