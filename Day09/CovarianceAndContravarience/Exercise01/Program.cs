using System;
namespace Exercise01
{

    public interface IReader<out T>
    {
        T Read();
        IEnumerable<T> ReadAll();
    }

    public interface IWriter<in T>
    {
        void Write(T item);
        void WriteAll(IEnumerable<T> items);
    }

    public class Animal
    {
        public string Name{set; get;} = string.Empty;
        public int Age{set; get;}
    }

    public class Dog : Animal
    {
        public string Breed{get; set;} = string.Empty;
    }

    public class Cat : Animal
    {
        public int lives {get; set;} = 9;
    }

    public class DogReader : IReader<Dog>
    {
        private List<Dog> dogs = new()
        {
            new Dog{Name = "Buddy", Age = 2, Breed = "Golder Retriever"},
            new Dog{Name = "snoopy", Age = 1, Breed = "German"}
        };

        public Dog Read() =>dogs[0];
        public IEnumerable<Dog> ReadAll() => dogs;
    }

    public class AnimalWriter : IWriter<Animal>
    {
        public void Write(Animal item)
        {
            System.Console.WriteLine($"writing animal: {item.Name}");
        }

        public void WriteAll(IEnumerable<Animal> items)
        {
            foreach(var item in items)
            {
                Write(item);
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            IReader<Dog> dogReader = new DogReader();

            IReader<Animal> animalReader = dogReader;

            Animal animal = animalReader.Read();
            System.Console.WriteLine($"ReadAnimal :{animal.Name}");

            IWriter<Animal> animalWriter = new AnimalWriter();

            IWriter<Dog> dogWriter = animalWriter;
            dogWriter.Write(new Dog{Name = "Rex", Age = 4});

            dogWriter.WriteAll(new List<Dog>
        {
            new Dog { Name = "Charlie", Age = 2, Breed = "Beagle" },
            new Dog { Name = "Rocky", Age = 7, Breed = "Bulldog" }
        });

            IWriter<Cat> catWriter = animalWriter;
            catWriter.Write(new Cat{Name = "snoopy", Age = 2, lives = 9});

            catWriter.WriteAll(new List<Cat>
            {
                new Cat{Name = "pinky", Age = 2, lives = 9},
                new Cat{Name = "dar", Age = 1, lives= 9}
            });
        }
    }
}
