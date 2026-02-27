using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Exercise05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter text (type END on new line to finish):");

            // Read multiple lines of input until 'END' is entered
            List<string> inputLines = new List<string>();
            string line;
            while ((line = Console.ReadLine()) != "END")
            {
                inputLines.Add(line);
            }

            // Join the input lines into one text string
            string text = string.Join(" ", inputLines);

            // Remove punctuation and convert to lowercase
            text = Regex.Replace(text, @"[^\w\s]", "").ToLower();

            // Split the text into words
            string[] words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Count frequency of each word using a dictionary
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (wordCount.ContainsKey(word)) // Use ContainsKey instead of Contains
                {
                    wordCount[word]++;
                }
                else
                {
                    wordCount[word] = 1;
                }
            }

            // Calculate statistics
            int totalWords = words.Length;
            int uniqueWords = wordCount.Count;
            double averageWordLength = words.Average(w => w.Length);

            // Display the statistics
            Console.WriteLine("\n=== Analysis ===");
            Console.WriteLine($"Total words: {totalWords}");
            Console.WriteLine($"Unique words: {uniqueWords}");
            Console.WriteLine($"Average word length: {averageWordLength:F1}");

            // Display top 5 most frequent words
            Console.WriteLine("\nTop 5 most frequent words:");
            var topWords = wordCount.OrderByDescending(w => w.Value)
                                    .Take(5)
                                    .ToList();

            for (int i = 0; i < topWords.Count; i++)
            {
                var word = topWords[i];
                Console.WriteLine($"{i + 1}. {word.Key} - {word.Value} time{(word.Value > 1 ? "s" : "")}");
            }
        }
    }
}