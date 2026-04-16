using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Implementations;
using Xunit;

public class BookRepositoryTests
{
    [Fact]
    public async Task CreateAsync_ShouldAddBook()
    {
        var context = TestDbContextFactory.Create();
        var repo = new BookRepository(context);

        var book = new Book
        {
            Title = "Test Book",
            Isbn = "123",
            Price = 100,
            AvailableCopies = 5,
            TotalCopies = 5,
            Publisher = "Test Publisher",
            Pages = 100,
            PublishedDate = DateTime.UtcNow
        };

        var result = await repo.CreateAsync(book);

        Assert.Equal("Test Book", result.Title);
        Assert.Equal(1, context.Books.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnBooks()
    {
        var context = TestDbContextFactory.Create();
        context.Books.Add(new Book { Title = "A", Isbn = "1", Price = 10, AvailableCopies = 1 });
        context.Books.Add(new Book { Title = "B", Isbn = "2", Price = 20, AvailableCopies = 0 });
        await context.SaveChangesAsync();

        var repo = new BookRepository(context);

        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("A", result.First().Title);
    }

    [Fact]
    public async Task GetAvailableAsync_ShouldReturnOnlyAvailableBooks()
    {
        var context = TestDbContextFactory.Create();

        context.Books.Add(new Book { Title = "A", Isbn = "1", AvailableCopies = 1 });
        context.Books.Add(new Book { Title = "B", Isbn = "2", AvailableCopies = 0 });

        await context.SaveChangesAsync();

        var repo = new BookRepository(context);

        var result = await repo.GetAvailableAsync();

        Assert.Single(result);
        Assert.Equal("A", result[0].Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBook()
    {
        var context = TestDbContextFactory.Create();

        var book = new Book { Id = 1, Title = "Test", Isbn = "123" };
        context.Books.Add(book);
        await context.SaveChangesAsync();

        var repo = new BookRepository(context);

        var result = await repo.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Test", result!.Title);
    }

    [Fact]
    public async Task SearchBooksAsync_ShouldReturnDto()
    {
        var context = TestDbContextFactory.Create();

        context.Books.Add(new Book
        {
            Title = "Clean Code",
            Isbn = "111",
            Price = 500,
            AvailableCopies = 3,
            Publisher = "Prentice",
            Pages = 300,
            PublishedDate = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        var repo = new BookRepository(context);

        var result = await repo.SearchBooksAsync("Clean");

        Assert.Single(result);
        Assert.Equal("Clean Code", result[0].Title);
    }
}