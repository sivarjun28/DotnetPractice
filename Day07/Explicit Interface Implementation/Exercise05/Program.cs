using System;

namespace Exercise05
{
    // Interfaces with conflicting method names
    public interface IEnglishSpeaker
    {
        string Greet();
        string Goodbye();
    }

    public interface ISpanishSpeaker
    {
        string Greet();
        string Goodbye();
    }

    public interface IFrenchSpeaker
    {
        string Greet();
        string Goodbye();
    }

    // PolyglotPerson implements all three interfaces
    public class PolyglotPerson : IEnglishSpeaker, ISpanishSpeaker, IFrenchSpeaker
    {
        public string Name { get; set; } = string.Empty;

        // Explicit implementations for each language
        string IEnglishSpeaker.Greet() => $"{Name} says: Hello!";
        string IEnglishSpeaker.Goodbye() => "Goodbye!";

        string ISpanishSpeaker.Greet() => $"{Name} says: ¡Hola!";
        string ISpanishSpeaker.Goodbye() => "¡Adiós!";

        string IFrenchSpeaker.Greet() => $"{Name} says: Bonjour!";
        string IFrenchSpeaker.Goodbye() => "Au revoir!";

        // Public method to greet in a specified language
        public void GreetInLanguage(string language)
        {
            string greeting = language.ToLower() switch
            {
                "english" => ((IEnglishSpeaker)this).Greet(),
                "spanish" => ((ISpanishSpeaker)this).Greet(),
                "french" => ((IFrenchSpeaker)this).Greet(),
                _ => "Language not supported"
            };

            Console.WriteLine(greeting);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a PolyglotPerson instance
            PolyglotPerson polyglotPerson = new() { Name = "Alice" };

            // Using explicit interface implementations
            IEnglishSpeaker english = polyglotPerson;
            Console.WriteLine(english.Greet());
            Console.WriteLine(english.Goodbye());

            ISpanishSpeaker spanish = polyglotPerson;
            Console.WriteLine(spanish.Greet());
            Console.WriteLine(spanish.Goodbye());

            IFrenchSpeaker french = polyglotPerson;
            Console.WriteLine(french.Greet());
            Console.WriteLine(french.Goodbye());

            // Using the public GreetInLanguage method to choose a greeting language
            polyglotPerson.GreetInLanguage("english");
            polyglotPerson.GreetInLanguage("spanish");
            polyglotPerson.GreetInLanguage("french");
            polyglotPerson.GreetInLanguage("german");  // Unsupported language
        }
    }
}