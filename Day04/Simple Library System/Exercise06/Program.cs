using System;
using System.Collections.Generic;
using System.Linq;  // Include LINQ for FirstOrDefault and Any

namespace Exercise06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var library = new Library();
            
            // Create and add books to the library
            var book1 = new Book("1234", "Python", "Guido van Rossum");
            var book2 = new Book("6789", "Java", "James Gosling");
            library.AddBook(book1);
            library.AddBook(book2);

            // Create and register a member
            var member1 = new Member(23, "Arjun");
            library.RegisterMember(member1);

            // Borrow the first book
            library.BorrowBook(member1, book1); // Should show that Arjun borrowed Python

            // List borrowed books (Arjun should have 1 book, "Python")
            library.ListBorrowedBooks(member1);

            // Borrow the second book
            library.BorrowBook(member1, book2); // Should show that Arjun borrowed Java

            // List borrowed books again (Arjun should have 2 books: "Python" and "Java")
            library.ListBorrowedBooks(member1);

            // Return the first book
            library.ReturnBook(member1, book1); // Should show that Arjun returned Python

            // List borrowed books again (Arjun should now only have "Java")
            library.ListBorrowedBooks(member1);

            // Calculate late fees for the first book (Python)
            library.CalculateLateFees(book1); // This should print a late fee if the book was borrowed more than 14 days ago

            // Check book availability for the second book (Java)
            bool isAvailable = library.checkBookAvailability("6789"); // Should print if Java is available or not
            Console.WriteLine($"Is 'Java' available? {isAvailable}");

            // Search books by title
            var booksByTitle = library.SearchBooksByTitle("Java"); // Should return the book with title "Java"
            Console.WriteLine("Books found by title 'Java':");
            foreach (var book in booksByTitle)
            {
                Console.WriteLine($"- {book.Title} by {book.Author}");
            }

            // Search books by author
            var booksByAuthor = library.SearchBooksByAuthor("Guido van Rossum"); // Should return the book by this author
            Console.WriteLine("Books found by author 'Guido van Rossum':");
            foreach (var book in booksByAuthor)
            {
                Console.WriteLine($"- {book.Title} by {book.Author}");
            }

            // Remove the first book
            library.RemoveBook(book1); // Should remove Python from the library

            // Check availability for the removed book
            isAvailable = library.checkBookAvailability("1234"); // Should print that "Python" is no longer available
            Console.WriteLine($"Is 'Python' available? {isAvailable}");
        }
    }

    public class Book
    {
        public string ISBN { get; }
        public string Title { get; }
        public string Author { get; }
        public bool IsAvailable { get; private set; }

        public Book(string isbn, string title, string author)
        {
            ISBN = isbn;
            Title = title;
            Author = author;
            IsAvailable = true;
        }

        public void BorrowBook()
        {
            if (!IsAvailable)
            {
                throw new InvalidOperationException("This Book is not available for borrowing");
            }
            IsAvailable = false;
        }

        public void ReturnBook()
        {
            IsAvailable = true;
        }
    }

    public class Member
    {
        public int MemberId { get; }
        public string Name { get; }
        public List<Book> BorrowedBooks { get; }

        public Member(int memberId, string name)
        {
            MemberId = memberId;
            Name = name;
            BorrowedBooks = new List<Book>();
        }

        public void BorrowBook(Book book)
        {
            if (BorrowedBooks.Count >= 3)
            {
                throw new InvalidOperationException("A member can borrow a maximum of 3 books");
            }
            BorrowedBooks.Add(book);
        }

        public void ReturnBook(Book book)
        {
            if (BorrowedBooks.Contains(book))
            {
                BorrowedBooks.Remove(book);
            }
            else
            {
                throw new InvalidOperationException("This book was not borrowed by the member.");
            }
        }

        public List<Book> GetBorrowedBooks()
        {
            return new List<Book>(BorrowedBooks);
        }
    }

    public class Library
    {
        private List<Book> books;
        private List<Member> members;
        private Dictionary<Book, DateTime> borrowDates;

        public Library()
        {
            books = new List<Book>();
            members = new List<Member>();
            borrowDates = new Dictionary<Book, DateTime>();
        }

        public void AddBook(Book book)
        {
            if (!books.Any(i => i.ISBN == book.ISBN))
                books.Add(book);
            else
                Console.WriteLine("Book already exists in the library.");
        }

        public void RemoveBook(Book book)
        {
            books.Remove(book);
        }

        public void RegisterMember(Member member)
        {
            if (!members.Any(m => m.MemberId == member.MemberId))
                members.Add(member);
            else
                Console.WriteLine("Member already Registered");
        }

        public bool checkBookAvailability(string isbn)
        {
            var book = books.FirstOrDefault(b => b.ISBN == isbn);
            return book != null && book.IsAvailable;
        }

        public void BorrowBook(Member member, Book book)
        {
            if (!book.IsAvailable)
            {
                Console.WriteLine("Sorry, this book is not available.");
                return;
            }
            book.BorrowBook();
            member.BorrowBook(book);
            borrowDates[book] = DateTime.Now;
            Console.WriteLine($"{member.Name} borrowed {book.Title}");
        }

        public void ReturnBook(Member member, Book book)
        {
            if (member.BorrowedBooks.Contains(book))
            {
                book.ReturnBook();
                member.ReturnBook(book);
                borrowDates.Remove(book);
                Console.WriteLine($"{member.Name} returned {book.Title}.");
            }
            else
            {
                Console.WriteLine("This book was not borrowed by the member.");
            }
        }

        public void CalculateLateFees(Book book)
        {
            if (borrowDates.ContainsKey(book))
            {
                DateTime borrowedOn = borrowDates[book];
                int daysLate = (DateTime.Now - borrowedOn).Days - 14;
                if (daysLate > 0)
                {
                    Console.WriteLine($"Late fee for {book.Title}: ${daysLate * 0.5}");
                }
            }
        }

        public List<Book> SearchBooksByTitle(string title)
        {
            return books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Book> SearchBooksByAuthor(string author)
        {
            return books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public void ListBorrowedBooks(Member member)
        {
            var borrowedBooks = member.GetBorrowedBooks();
            Console.WriteLine($"{member.Name} borrowed the following books:");
            if (borrowedBooks.Count == 0)
            {
                Console.WriteLine("No books borrowed yet.");
            }
            foreach (var book in borrowedBooks)
            {
                Console.WriteLine($"- {book.Title}");
            }
        }
    }
}