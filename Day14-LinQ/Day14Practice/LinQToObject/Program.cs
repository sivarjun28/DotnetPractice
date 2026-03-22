using System;
using System.Linq.Expressions;
namespace LinQToObject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("data.txt");

            var longLines = lines
                .Where(line => line.Length > 50)
                .OrderBy(line => line)
                .ToList();

            Console.WriteLine("Filtered Lines:");
            foreach (var line in longLines)
            {
                Console.WriteLine(line);
            }

            //Read and parse CsV
            var products = File.ReadAllLines("products.csv")
                .Skip(1) // Skip header
                .Select(line => line.Split(','))
                .Select(parts => new Product
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1],
                    Price = decimal.Parse(parts[2])
                })
                .ToList();
            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Price: {product.Price}");
            }

            // Find files in directory
            string[] allFiles = Directory.GetFiles(@"C:\Data", "*.*", SearchOption.AllDirectories);
            var csvFiles = allFiles
                .Where(f => f.EndsWith(".csv"))
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.Length)
                .Take(5);
            foreach (var file in csvFiles)
            {
                Console.WriteLine($"File: {file.Name}, Size: {file.Length} bytes");
            }

            // Group files by extension
            var filesByExtension = Directory.GetFiles(@"C:\Data")
                .Select(f => new FileInfo(f))
                .GroupBy(f => f.Extension)
                .Select(g => new
                {
                    Extension = g.Key,
                    Count = g.Count(),
                    TotalSize = g.Sum(f => f.Length)
                });
            foreach (var group in filesByExtension)
            {
                Console.WriteLine($"Extension: {group.Extension}, Count: {group.Count}, Total Size: {group.TotalSize} bytes");
            }
            // Find large files
            var largeFiles = Directory.GetFiles(@"C:\Data", "*.*", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f))
                .Where(f => f.Length > 1024 * 1024)  // > 1MB
                .OrderByDescending(f => f.Length)
                .Select(f => new
                {
                    f.Name,
                    SizeMB = f.Length / (1024 * 1024),
                    f.LastWriteTime
                });


            // Find duplicate files by name
            var duplicates = Directory.GetFiles(@"C:\Data", "*.*", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f))
                .GroupBy(f => f.Name)
                .Where(g => g.Count() > 1)
                .Select(g => new
                {
                    FileName = g.Key,
                    Count = g.Count(),
                    Paths = g.Select(f => f.FullName).ToList()
                });
            foreach (var dup in duplicates)
            {
                Console.WriteLine($"Duplicate File: {dup.FileName}, Count: {dup.Count}");
                foreach (var path in dup.Paths)
                {
                    Console.WriteLine($" - {path}");
                }
            }

            // Find empty directories

            var emptyDirs = Directory.GetDirectories(@"C:\Data", "*", SearchOption.AllDirectories)
                .Where(d => !Directory.EnumerateFileSystemEntries(d).Any())
                .ToList();
            // Part 2: LINQ with Strings 


            string text = "The quick brown fox jumps over the lazy dog";

            var words = text.Split(' ');

            var longWords = words.Where(w => w.Length > 4)
                .OrderBy(w => w)
                .ToList();
            Console.WriteLine("Long Words:");
            foreach (var word in longWords)
            {
                Console.WriteLine(word);
            }

            //char Frequency
            var charFrequency = text.
                Where(c => char.IsLetter(c))
                .GroupBy(c => char.ToLower(c))
                .Select(g => new
                {
                    Character = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count);
            Console.WriteLine("Character Frequency:");
            foreach (var item in charFrequency)
            {
                Console.WriteLine($"Character: {item.Character}, Count: {item.Count}");
            }

            // Find palindromes
            List<string> words1 = new() { "radar", "hello", "level", "world", "civic" };

            var palindromes = words1
                .Where(w => w == new string(w.Reverse().ToArray()));

            var stats = words
                .Select(w => new
                {
                    Word = w,
                    Length = w.Length,
                    Vowels = w.Count(c => "aeiou".Contains(char.ToLower(c))),
                    consonants = w.Count(c => char.IsLetter(c) && !"aeiou".Contains(char.ToLower(c)))

                });


            string document = File.ReadAllText("document.txt");
            // Word count
            var words2 = document
                .Split(new[] { ' ', '\n', '\r', '\t', '.', ',', '!', '?' },
                       StringSplitOptions.RemoveEmptyEntries);
            var wordCount = words2
     .GroupBy(w => w.ToLower())
     .Select(g => new { Word = g.Key, Count = g.Count() })
     .OrderByDescending(x => x.Count)
     .Take(20);

            foreach (var item in wordCount)
            {
                System.Console.WriteLine($"Word: {item.Word}, Count: {item.Count}");
            }

            // Find sentences
            var sentences = document
                .Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => s.Length > 0);

            var longSentences = sentences
                .Where(s => s.Split(' ').Length > 15)
                .ToList();

            // Extract emails
            string text1 = "Contact us at support@example.com or sales@company.org";

            var emails = text1
                .Split(' ')
                .Where(w => w.Contains('@') && w.Contains('.'));

            List<int> numbers = new() { 1, 2, 3, 4, 5 };

            // DEFERRED - no execution yet
            var query = numbers.Where(n =>
            {
                Console.WriteLine($"Checking {n}");
                return n > 2;
            });

            Console.WriteLine("Query created");

            // Executes when iterated
            foreach (int n in query)
            {
                Console.WriteLine($"Result: {n}");
            }

            // IMMEDIATE - executes now
            var list = numbers.Where(n =>
            {
                Console.WriteLine($"Checking {n}");
                return n > 2;
            }).ToList();

            Console.WriteLine("List created");


            var largeList = Enumerable.Range(1, 10_000_000).ToList();
            // Ordered parallel query
            var ordered = largeList
                .AsParallel()
                .AsOrdered()
                .Where(n => n % 2 == 0)
                .Take(1000);
            foreach (var item in ordered)
            {
                System.Console.WriteLine(item);
            }
        }







    }
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }


    public static class LinqExtensions
    {
        // Custom ForEach extension
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        // Batch items
        public static IEnumerable<IEnumerable<T>> Batch<T>(
            this IEnumerable<T> source,
            int batchSize)
        {
            var batch = new List<T>(batchSize);

            foreach (T item in source)
            {
                batch.Add(item);

                if (batch.Count == batchSize)
                {
                    yield return batch;
                    batch = new List<T>(batchSize);
                }
            }

            if (batch.Count > 0)
            {
                yield return batch;
            }
        }

        // DistinctBy (C# 10 already has this, but as example)
        public static IEnumerable<T> DistinctBy<T, TKey>(
            this IEnumerable<T> source,
            Func<T, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();

            foreach (T item in source)
            {
                if (seenKeys.Add(keySelector(item)))
                {
                    yield return item;
                }
            }
        }

        // MaxBy
        public static T? MaxBy<T, TKey>(
            this IEnumerable<T> source,
            Func<T, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            T? max = default;
            TKey? maxKey = default;
            bool hasValue = false;

            foreach (T item in source)
            {
                TKey key = keySelector(item);

                if (!hasValue || key.CompareTo(maxKey) > 0)
                {
                    max = item;
                    maxKey = key;
                    hasValue = true;
                }
            }

            return max;
        }
    }

    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T? GetById(int id);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly List<T> data = new();

        public IQueryable<T> GetAll()
        {
            return data.AsQueryable();
        }

        public T? GetById(int id)
        {
            // Assumes T has Id property
            var idProperty = typeof(T).GetProperty("Id");
            return data.FirstOrDefault(item =>
                (int)idProperty.GetValue(item)! == id);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return data.AsQueryable().Where(predicate);
        }
    }

    

}