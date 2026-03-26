using Microsoft.AspNetCore.Mvc;
using Exercise03.Models;

namespace Exercise03.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static List<Book> books = new List<Book>
        {
            new Book { Id = 1, Title = "Book One", Author = "Author A", ISBN = "111", PublishedDate = DateTime.Now, CategoryId = 1, CategoryName = "Fiction" },
            new Book { Id = 2, Title = "Book Two", Author = "Author B", ISBN = "222", PublishedDate = DateTime.Now, CategoryId = 2, CategoryName = "Science" },
            new Book { Id = 3, Title = "Book Four", Author = "Author C", ISBN = "333", PublishedDate = DateTime.Now, CategoryId = 3, CategoryName = "Fiction" },
            new Book { Id = 3, Title = "Book Five", Author = "Author D", ISBN = "444", PublishedDate = DateTime.Now, CategoryId = 3, CategoryName = "Science" },
            new Book { Id = 4, Title = "Book Six", Author = "Author E", ISBN = "555", PublishedDate = DateTime.Now, CategoryId = 4, CategoryName = "Fiction" },
            new Book { Id = 5, Title = "Book Seven", Author = "Author F", ISBN = "666", PublishedDate = DateTime.Now, CategoryId = 5, CategoryName = "Science" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAll()
        {
            return Ok(books);
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetById(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            book.Id = books.Max(b => b.Id) + 1;
            books.Add(book);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Book updatedBook)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.ISBN = updatedBook.ISBN;
            book.PublishedDate = updatedBook.PublishedDate;
            book.CategoryId = updatedBook.CategoryId;
            book.CategoryName = updatedBook.CategoryName;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            books.Remove(book);
            return NoContent();
        }

        // ✅ Get books by category
        [HttpGet("category/{categoryId}")]
        public ActionResult<IEnumerable<Book>> GetByCategory(int categoryId)
        {
            var result = books.Where(b => b.CategoryId == categoryId).ToList();
            return Ok(result);
        }
        // ✅ Get books by author
        [HttpGet("author/{author}")]
        public ActionResult<IEnumerable<Book>> GetByAuthor(string author)
        {
            var result = books.Where(b => b.Author.ToLower() == author.ToLower()).ToList();
            return Ok(result);
        }
        //✅ Search by ISBN / Title
        [HttpGet("search")]
        public ActionResult<IEnumerable<Book>> Search([FromQuery] string title, [FromQuery] string author, [FromQuery] string category)
        {
            var result = books.AsEnumerable();

            if (!string.IsNullOrEmpty(title))
                result = result.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(author))
                result = result.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(category))
                result = result.Where(b => b.CategoryName.Contains(category, StringComparison.OrdinalIgnoreCase));

            if (!result.Any())
                return NotFound();

            return Ok(result);
        }
    }
}