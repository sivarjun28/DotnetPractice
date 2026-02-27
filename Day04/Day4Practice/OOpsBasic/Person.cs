namespace OOpsBasic
{
    public class Person
    {
        private string name;
        private int age;

        // Constructor to initialize name and age
        public Person(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        // Method to introduce the person
        public void Introduce()
        {
            Console.WriteLine($"Hello! Good Morning I am {name}, {age} years old");
        }
    }
}