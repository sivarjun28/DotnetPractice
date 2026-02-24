using System;
namespace Exercise02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("=== Text Analyzer ===\n");
            System.Console.Write("Enter a sentence ");
            string? input = System.Console.ReadLine();
             if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("No input provided.");
                return;
            }
            // TODO: Count characters (including spaces)
            int countChar = input.Length;
            System.Console.WriteLine($"Character count for {input} including spaces is {countChar}");
            
            // TODO: Count characters (excluding spaces)
            int charCountWithoutSpaces = input.Replace(" ", "").Length;
            System.Console.WriteLine($"Character count without spaces: {charCountWithoutSpaces}");
            
            // TODO: Count words (split by spaces)
            string[] words = input.Split(new[] {' ','\t'}, StringSplitOptions.RemoveEmptyEntries);
            int wordCount = words.Length;
            System.Console.WriteLine($"count of the words that split by spaces {wordCount}");
            // TODO: Display transformations
            System.Console.WriteLine($"uppercase : {input.ToUpper()}");
            System.Console.WriteLine($"lowercase: {input.ToLower()}");
            bool contains = input.Contains("programming");
            System.Console.WriteLine($"contains: {contains}");
            string replace = input.Replace("c#", "dotnet");
            System.Console.WriteLine($"Replacing the word from {input}: {replace}");

            string[] split2 = input.Split(" ");
            string joinWords = string.Join(",", split2);
            System.Console.WriteLine($"joining words with comma, {joinWords} ");
            // TODO: Check for keywords (e.g., "C#", "programming")
            string[] keywords = {"c#", "programming"};
            if(keywords.Any(keyword => input.Contains(keyword)))
            {
                System.Console.WriteLine("keywords are found");
    
            }
            else
            {
                System.Console.WriteLine("Keywords are not found");
            }
            // TODO: Extract first 3 words

            var words1 = input.Split(' ').Take(3);

            System.Console.WriteLine($"First three words are:  {string.Join(" ",words1)}");
        }
    }
}
