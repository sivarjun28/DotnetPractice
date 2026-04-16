using LibraryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

    
public static class TestDbContextFactory
{
    public static LibraryDbContext Create()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        return new LibraryDbContext(options);
    }
}