using System;
using System.Text.RegularExpressions;
namespace Exercise02
{

    /*
    Word Frequency: Count word occurrences (case-insensitive)
Sentence Analysis: Extract and analyze sentences
Character Statistics: Letter frequency, vowel/consonant ratio
Find Patterns: Extract emails, URLs, phone numbers
Readability Score: Calculate average words per sentence
Unique Words: Find words that appear only once
    */
    internal class Program
    {
        static void Main(string[] args)
        {
            string article = @"
Artificial Intelligence is transforming the world. Machine learning algorithms 
can analyze vast amounts of data. Is AI the future? Yes, it certainly is! 
Contact us at support@example.com for more information.

Deep learning models achieve remarkable accuracy. Neural networks mimic the 
human brain. What makes them so powerful? Their ability to learn from data.
";

            var analyzer = new TextAnalyzer(article);

            // Word frequency
            var wordFreq = analyzer.GetWordFrequency();
            Console.WriteLine("Top 10 Words:");
            foreach (var kv in wordFreq.OrderByDescending(kv => kv.Value).Take(10))
            {
                Console.WriteLine($"{kv.Key} - {kv.Value} occurrences");
            }

            // Sentence analysis
            var sentences = analyzer.AnalyzeSentences();
            Console.WriteLine($"\nTotal Sentences: {sentences.Count}");

            // Character statistics
            var stats = analyzer.GetStatistics();
            Console.WriteLine($"\nText Statistics:");
            Console.WriteLine($"Total Words: {stats.TotalWords}");
            Console.WriteLine($"Unique Words: {stats.UniqueWords}");
            Console.WriteLine($"Total Sentences: {stats.TotalSentences}");
            Console.WriteLine($"Average Words/Sentence: {stats.AverageWordsPerSentence:F1}");
            Console.WriteLine($"Vowels: {stats.VowelCount}, Consonants: {stats.ConsonantCount}");
            Console.WriteLine($"Vowel/Consonant Ratio: {stats.VowelConsonantRatio:F2}");

            // Extract emails
            var emails = analyzer.ExtractEmails();
            Console.WriteLine($"\nEmails Found: {string.Join(", ", emails)}");

            // Unique words
            var uniqueWords = analyzer.UniqueWords();
            Console.WriteLine($"\nUnique Words: {string.Join(", ", uniqueWords.Take(10))}...");

        }
    }

    public class TextAnalyzer
    {
        private readonly string text;
        public TextAnalyzer(string text)
        {
            this.text = text;
        }

        public Dictionary<string, int> GetWordFrequency()
        {
            char[] separators = new char[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?', ';', ':', '(', ')', '"', '\'' };
            var words = text.ToLower()
                            .Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return words.GroupBy(w => w)
                            .ToDictionary(g => g.Key, g => g.Count());
        }

        //Sentece analysis
        public List<SentenceInfo> AnalyzeSentences()
        {
            char[] sentenceSaperators = new char[] { '.', '?', '!' };
            var sentenceList = text.Split(sentenceSaperators, StringSplitOptions.RemoveEmptyEntries);

            return sentenceList.Select(s =>
            {
                string trimmed = s.Trim();
                int wordCount = trimmed.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
                int charCount = trimmed.Count(c => !Char.IsWhiteSpace(c));



                return new SentenceInfo
                {
                    Text = trimmed,
                    WordCount = wordCount,
                    CharacterCount = charCount,
                    IsQuestion = trimmed.EndsWith("?"),
                    IsExclamation = trimmed.EndsWith("!")
                };

            }).ToList();
        }

        public Dictionary<char, int> GetCharacterFrequency()
        {
            return text.ToLower()
                    .Where(Char.IsLetter)
                    .GroupBy(c => c)
                    .ToDictionary(g => g.Key, g => g.Count());
        }

        public TextStatistics GetStatistics()
        {
            var wordFreq = GetWordFrequency();
            var sentences = AnalyzeSentences();
            var chars = GetCharacterFrequency();
            int vowels = chars.Where(kv => "aeiou".Contains(kv.Key)).Sum(kv => kv.Value);
            int consonants = chars.Where(kv => !"aeiou".Contains(kv.Key)).Sum(kv => kv.Value);

            return new TextStatistics
            {
                TotalWords = wordFreq.Values.Sum(),
                UniqueWords = wordFreq.Count,
                TotalSentences = sentences.Count,
                AverageWordsPerSentence = sentences.Any() ? sentences.Average(s => s.WordCount) : 0,
                AverageSentenceLength = sentences.Any() ? sentences.Average(s => s.CharacterCount) : 0,
                VowelCount = vowels,
                ConsonantCount = consonants,
                VowelConsonantRatio = consonants != 0 ? (double)vowels / consonants : 0
            };

        }

        public List<string> ExtractEmails()
        {
            var words = text.Split(new char[] { ' ', '\t', '\n', '\r', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Where(w => w.Contains("@") && w.Contains("."))
                       .Select(w => w.TrimEnd('.', ',', ';'))
                       .Distinct()
                       .ToList();
        }

        public List<string> UniqueWords()
        {
            var wordFreq = GetWordFrequency();
            return wordFreq.Where(kv => kv.Value == 1)
                            .Select(kv => kv.Key)
                            .ToList();
        }
    }
    public class SentenceInfo
    {
        public string Text { get; set; } = string.Empty;
        public int WordCount { get; set; }
        public int CharacterCount { get; set; }
        public bool IsQuestion { get; set; }
        public bool IsExclamation { get; set; }
    }

    public class TextStatistics
    {
        public int TotalWords { get; set; }
        public int UniqueWords { get; set; }
        public int TotalSentences { get; set; }
        public double AverageWordsPerSentence { get; set; }
        public double AverageSentenceLength { get; set; }
        public int VowelCount { get; set; }
        public int ConsonantCount { get; set; }
        public double VowelConsonantRatio { get; set; }
    }
}
