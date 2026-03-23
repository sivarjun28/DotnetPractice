using System;
using System.Collections.Frozen;
namespace Name
{




    public class FileAnalysis
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public long SizeBytes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Directory { get; set; } = string.Empty;


    }

    public class ExtensionStats
    {
        public int FileCount { get; set; }
        public long TotalSizeBytes { get; set; }
        public long AverageSizeBytes { get; set; }
        public string LargestFile { get; set; } = string.Empty;
        public long LargestFileSize { get; set; }
    }

    public class DuplicateFileInfo
    {
        public string FileName { get; set; } = string.Empty;
        public List<string> Paths { get; set; } = new();
        public int Count { get; set; }

    }

    public class DirectoryStats
    {
        public int FileCount { get; set; }
        public long TotalSizeBytes { get; set; }
        public string MostCommonExtension { get; set; } = string.Empty;
    }

    public class FileSystemAnalyzer
    {
        private readonly string rootPath;
        public FileSystemAnalyzer(string rootPath)
        {
            this.rootPath = rootPath;
        }
        public List<FileAnalysis> ScanFiles()
        {
            return Directory
                    .GetFiles(rootPath, "*", SearchOption.AllDirectories)
                    .Select(filePath =>
                    {
                        var fileInfo = new FileInfo(filePath);
                        return new FileAnalysis
                        {

                            FilePath = filePath,
                            FileName = fileInfo.Name,
                            Extension = fileInfo.Extension,
                            SizeBytes = fileInfo.Length,
                            CreatedDate = fileInfo.CreationTime,
                            ModifiedDate = fileInfo.LastWriteTime,
                            Directory = fileInfo.DirectoryName ?? string.Empty
                        };
                    }).ToList();


            throw new NotImplementedException();
        }

        public Dictionary<string, ExtensionStats> GetExtensionStatistics()
        {
            var files = ScanFiles();
            //Group by extension and calculate statistics

            return files
                    .GroupBy(f => f.Extension)
                    .ToDictionary(
                        g => g.Key,
                        g =>
                        {
                            var largest = g.OrderByDescending(f => f.SizeBytes).First();
                            return new ExtensionStats
                            {
                                FileCount = g.Count(),
                                TotalSizeBytes = g.Sum(s => s.SizeBytes),
                                AverageSizeBytes = (long)g.Average(s => s.SizeBytes),
                                LargestFile = largest.FileName,
                                LargestFileSize = largest.SizeBytes
                            };
                        }
                    );
            throw new NotImplementedException();
        }

        public List<DuplicateFileInfo> FindDSuplicateNames()
        {
            var files = ScanFiles();
            return files
                    .GroupBy(f => f.FileName)
                    .Where(g => g.Select(f => f.Directory).Distinct().Count() > 1)
                    .Select(g => new DuplicateFileInfo
                    {
                        FileName = g.Key,
                        Paths = g.Select(f => f.FilePath).ToList(),
                        Count = g.Count()
                    }).ToList();

            throw new NotImplementedException();

        }
        public List<FileAnalysis> FindOldFiles(int months)
        {
            var files = ScanFiles();
            //Find files not modified in specified months
            var cutoff = DateTime.Now.AddMonths(-months);
            return files
                    .Where(f => f.ModifiedDate < cutoff)
                    .ToList();

            throw new NotImplementedException();
        }

        public List<FileAnalysis> GetLargestFiles(int count)
        {
            var files = ScanFiles();

            return files
                    .OrderByDescending(f => f.SizeBytes)
                    .Take(count)
                    .ToList();

            throw new NotImplementedException();
        }

        public Dictionary<string, DirectoryStats> GetDirectoryStats()
        {
            var files = ScanFiles();
            return files
                    .GroupBy(f => f.Directory)
                    .ToDictionary(
                        g => g.Key,
                        g => new DirectoryStats
                        {
                            FileCount = g.Count(),
                            TotalSizeBytes = g.Sum(x => x.SizeBytes),
                            MostCommonExtension = g.
                                        GroupBy(f => f.Extension)
                                        .OrderByDescending(x => x.Count())
                                        .First().Key
                        }
                    );

        }


    }
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Data";
            var analyzer = new FileSystemAnalyzer(path);
            //1.Extension statistics
            System.Console.WriteLine("Extension statistics: ");
            var extStats = analyzer.GetExtensionStatistics();
            foreach (var kvp in extStats)
            {
                var ext = kvp.Key;
                var stat = kvp.Value;
                System.Console.WriteLine(
                    $"{ext,-6} - {stat.FileCount} files, " +
                $"{FormatSize(stat.TotalSizeBytes)} total, " +
                $"{FormatSize(stat.AverageSizeBytes)} average, " +
                $"largest: {stat.LargestFile} ({FormatSize(stat.LargestFileSize)})"
                );
            }

            // 2. Duplicate Files
            Console.WriteLine("\nDuplicate Files Found:\n");
            var duplicates = analyzer.FindDSuplicateNames();
            foreach (var dup in duplicates)
            {
                System.Console.WriteLine($"{dup.FileName}- fount in {dup.Count} locations");
                foreach (var pathItem in dup.Paths)
                {
                    System.Console.WriteLine($"  -{pathItem}");
                }
                System.Console.WriteLine();
            }
            //3.Old files
            System.Console.WriteLine("Old Files");
            var oldFiles = analyzer.FindOldFiles(6);
            System.Console.WriteLine($"\n old files(not modified in 6 months): {oldFiles.Count}");

            //4. GetLargest files
            System.Console.WriteLine("Largest Files");
            var largest = analyzer.GetLargestFiles(3);
            foreach (var large in largest)
            {
                System.Console.WriteLine($"{large.FileName} - {FormatSize(large.SizeBytes)}");
            }
            if (largest.Any())
            {
                var biggest = largest.First();
                Console.WriteLine($"\nLargest file: {biggest.FileName} ({FormatSize(biggest.SizeBytes)})");
            }

            //  5. Directory statistics
            System.Console.WriteLine("\n Directory Statistics");
            var dirStats = analyzer.GetDirectoryStats();
            foreach (var kvp in dirStats)
            {
                System.Console.WriteLine(
                    $"{kvp.Key}\n" +
                    $" Files: {kvp.Value.FileCount}, " +
                    $" size: {FormatSize(kvp.Value.TotalSizeBytes)}, " +
                    $"Most Common: {kvp.Value.MostCommonExtension}"
                );
            }



        }
        static string FormatSize(long bytes)
        {
            double size = bytes;
            string[] units = { "B", "KB", "MB", "GB" };
            int unit = 0;
            while (size >= 1024 && unit < units.Length - 1)
            {
                size /= 1024;
                unit++;
            }
            return $"{size:F1} {units[unit]}";
        }
    }
}