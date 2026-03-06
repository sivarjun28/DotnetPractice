using System;
using System.Text.RegularExpressions;
namespace Exercise02
{
    /*
    Abstract class: FileSystemItem
    - Properties: Name, Path, CreatedDate, Size [abstract]
    - Methods: GetInfo(), Delete() [abstract], Move(newPath)

    Concrete classes:
    1. File : FileSystemItem
       - Properties: Extension, Content
       - Size: Length of content
       - Delete: Remove file

    2. Directory : FileSystemItem
       - Properties: List<FileSystemItem> Children
       - Size: Sum of children sizes
       - Delete: Delete all children first
       - Methods: AddItem(), RemoveItem()

    3. Shortcut : FileSystemItem
       - Properties: TargetPath
       - Size: Always 0
       - Delete: Remove shortcut only
    */
    public abstract class FileSystemItem
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;

        public DateTime createdDate { get; set; } = DateTime.Now;
        public abstract long Size { get; }
        public abstract void Delete();
        public virtual void Move(string newPath)
        {
            System.Console.WriteLine($"Moving {Name} from {Path} to {newPath}");
            Path = newPath;
        }

        public virtual void GetInfo()
        {
            System.Console.WriteLine($"Name: {Name}");
            System.Console.WriteLine($"Path: {Path}");
            System.Console.WriteLine($"created date: {createdDate}");
            System.Console.WriteLine($"Size {Size} bytes");
        }
    }

    public class File : FileSystemItem
    {
        public string Extension { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public override long Size => Content.Length;
        public override void Delete()
        {
            System.Console.WriteLine($"Deleting file: {Name}");
        }
        public override void GetInfo()
        {
            base.GetInfo();
            System.Console.WriteLine($"Type: file (.{Extension})");
        }
    }
    public class Directory : FileSystemItem
    {
        private List<FileSystemItem> children = new();
        public IReadOnlyList<FileSystemItem> Children => children.AsReadOnly();
        public override long Size
        {
            get
            {
                long totalSize = 0;
                foreach (var child in children)
                {
                    totalSize += child.Size;
                }
                return totalSize;
            }
        }

        public void AddItem(FileSystemItem item)
        {
            children.Add(item);
            System.Console.WriteLine($"Added {item.Name} to {Name}");
        }

        public void RemoveItem(string name)
        {
            var item = children.Find(i => i.Name == name);
            if (item != null)
            {
                children.Remove(item);
                System.Console.WriteLine($"Removed {item.Name} from {Name}");
            }
            else
            {
                System.Console.WriteLine($"Item {name} Not found in {Name}");
            }
        }

        public override void Delete()
        {
            System.Console.WriteLine($"Deleting directory: {Name}");
            foreach (var child in children)
            {
                child.Delete();
            }
        }
        public void ListContents()
        {
            System.Console.WriteLine($"Listing contents i directory: {Name}");
            foreach (var child in children)
            {
                child.GetInfo();
            }
        }

    }

    public class Shortcut : FileSystemItem
    {
        public string TargetPath { get; set; } = string.Empty;
        public override long Size => 0;

        public override void Delete()
        {
            Console.WriteLine($"Deleting shortcut: {Name}");
            // Simulate shortcut deletion
        }

        public override void GetInfo()
        {
            base.GetInfo();
            Console.WriteLine($"Type: Shortcut");
            Console.WriteLine($"Target Path: {TargetPath}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Directory root = new() { Name = "Root", Path = "C:\\" };
            Directory documents = new() { Name = "Documents", Path = "C:\\Documents" };
            File file1 = new() { Name = "readme", Extension = "txt", Content = "Hello world", Path = "C:\\Documents\\readme.txt" };
            File file2 = new() { Name = "data", Extension = "json", Content = "{\"key\":\"value\"}", Path = "C:\\Documents\\data.json" };
            // Add a shortcut to the directory
            Shortcut shortcut1 = new() { Name = "Shortcut to Documents", Path = "C:\\Root\\shortcut.lnk", TargetPath = "C:\\Documents" };
            root.AddItem(shortcut1);

            // Get information about the root directory
            root.GetInfo();

            // List contents of the documents directory
            documents.ListContents();

            // Get total size of the root directory
            Console.WriteLine($"\nTotal size of {root.Name}: {root.Size} bytes");

            // Delete the root directory (and all its contents)
            root.Delete();
        }
    }
}
