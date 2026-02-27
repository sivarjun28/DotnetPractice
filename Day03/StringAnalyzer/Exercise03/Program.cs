using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exercise03
{
    class StringAnalyzer
    {
        private string text;
        
        public StringAnalyzer(string input)
        {
            text = input ?? string.Empty;
        }
        
        public (int vowels, int consonants) CountVowelsAndConsonants()
        {
            int vowels = 0, consonants = 0;
            string vowelChars = "aeiouAEIOU";
            
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    if (vowelChars.Contains(c))
                        vowels++;
                    else
                        consonants++;
                }
            }
            
            return (vowels, consonants);
        }
        
        public char GetMostFrequentCharacter()
        {
            Dictionary<char, int> frequency = new Dictionary<char, int>();
            
            foreach (char c in text.ToLower())
            {
                if (char.IsLetter(c))
                {
                    if (frequency.ContainsKey(c))
                        frequency[c]++;
                    else
                        frequency[c] = 1;
                }
            }
            
            if (frequency.Count == 0)
                return '\0';
            
            return frequency.OrderByDescending(p => p.Value).First().Key;
        }
        
        public bool IsPalindrome()
        {
            // Remove non-alphanumeric and compare
            string cleaned = new string(text.Where(char.IsLetterOrDigit).ToArray()).ToLower();
            
            int left = 0, right = cleaned.Length - 1;
            while (left < right)
            {
                if (cleaned[left] != cleaned[right])
                    return false;
                left++;
                right--;
            }
            return true;
        }
        
        public Dictionary<string, int> GetWordFrequency()
        {
            Dictionary<string, int> wordFrequency = new Dictionary<string, int>();
            
            // Remove punctuation and split by spaces to get words
            string cleanedText = RemovePunctuation();
            string[] words = cleanedText.Split(new[] { ' ', '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var word in words)
            {
                string lowerWord = word.ToLower();
                if (wordFrequency.ContainsKey(lowerWord))
                    wordFrequency[lowerWord]++;
                else
                    wordFrequency[lowerWord] = 1;
            }
            
            return wordFrequency;
        }

        public string RemovePunctuation()
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in text)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public string ToProperTitleCase()
        {
            // Split the string into words, then capitalize each word's first letter
            var words = text.Split(new[] { ' ', '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var titleCasedWords = words.Select(word => 
                char.ToUpper(word[0]) + word.Substring(1).ToLower()).ToArray();
            
            return string.Join(" ", titleCasedWords);
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a text: ");
            string input = Console.ReadLine() ?? "";
            
            StringAnalyzer analyzer = new StringAnalyzer(input);
            
            var (vowels, consonants) = analyzer.CountVowelsAndConsonants();
            Console.WriteLine($"Vowels: {vowels}, Consonants: {consonants}");
            
            Console.WriteLine($"Most frequent character: {analyzer.GetMostFrequentCharacter()}");
            Console.WriteLine($"Is palindrome: {analyzer.IsPalindrome()}");
            
            var wordFrequency = analyzer.GetWordFrequency();
            Console.WriteLine("Word Frequency:");
            foreach (var word in wordFrequency)
            {
                Console.WriteLine($"{word.Key}: {word.Value}");
            }

            Console.WriteLine($"Text without punctuation: {analyzer.RemovePunctuation()}");
            Console.WriteLine($"Title case: {analyzer.ToProperTitleCase()}");
        }
    }
}