using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.Entities;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Services.Implementations;
using Xunit;

public class LoanServiceTests
{
    private LoanService GetService(LibraryDbContext context)
    {
        return new LoanService(context);
    }

    // -------------------------------
    // 1. CheckoutBookAsync
    // -------------------------------
    [Fact]
    public async Task CheckoutBook_ShouldCreateLoan()
    {
        var context = TestDbContextFactory.Create();

        context.Books.Add(new Book
        {
            Id = 1,
            Title = "Test Book",
            AvailableCopies = 5,
            TotalCopies = 5,
            Price = 100,
            Isbn = "123",
            Publisher = "Test",
            Pages = 100,
            PublishedDate = DateTime.UtcNow
        });

        context.Members.Add(new Member
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            IsActive = true,
            MembershipDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        var service = GetService(context);

        var loan = await service.CheckoutBookAsync(1, 1);

        Assert.NotNull(loan);
        Assert.Equal(1, loan.BookId);
        Assert.Equal(1, context.Loans.Count());
    }

    // -------------------------------
    // 2. ReturnBookAsync
    // -------------------------------
    [Fact]
    public async Task ReturnBook_ShouldUpdateLoanAndIncreaseCopies()
    {
        var context = TestDbContextFactory.Create();

        var book = new Book
        {
            Id = 1,
            Title = "Test",
            AvailableCopies = 4,
            TotalCopies = 5
        };

        var loan = new Loan
        {
            Id = 1,
            BookId = 1,
            MemberId = 1,
            LoanDate = DateTime.UtcNow.AddDays(-10),
            DueDate = DateTime.UtcNow.AddDays(-5),
            Status = LoanStatus.Active
        };

        context.Books.Add(book);
        context.Loans.Add(loan);
        await context.SaveChangesAsync();

        var service = GetService(context);

        var result = await service.ReturnBookAsync(1);

        Assert.NotNull(result.ReturnDate);
        Assert.True(result.LateFee >= 0);
        Assert.Equal(5, book.AvailableCopies);
    }

    // -------------------------------
    // 3. Active Loans
    // -------------------------------
    [Fact]
    public async Task GetActiveLoans_ShouldReturnOnlyActive()
    {
        var context = TestDbContextFactory.Create();

        context.Loans.Add(new Loan
        {
            Id = 1,
            MemberId = 1,
            Status = LoanStatus.Active,
            DueDate = DateTime.UtcNow.AddDays(5)
        });

        context.Loans.Add(new Loan
        {
            Id = 2,
            MemberId = 1,
            Status = LoanStatus.Returned,
            DueDate = DateTime.UtcNow.AddDays(5)
        });

        await context.SaveChangesAsync();

        var service = GetService(context);

        var result = await service.GetActiveLoansByMemberAsync(1);

        Assert.Single(result);
    }

    // -------------------------------
    // 4. Overdue Loans
    // -------------------------------
    [Fact]
    public async Task GetOverdueLoans_ShouldReturnOnlyOverdue()
    {
        var context = TestDbContextFactory.Create();

        context.Loans.Add(new Loan
        {
            Id = 1,
            Status = LoanStatus.Active,
            DueDate = DateTime.UtcNow.AddDays(-2)
        });

        context.Loans.Add(new Loan
        {
            Id = 2,
            Status = LoanStatus.Active,
            DueDate = DateTime.UtcNow.AddDays(5)
        });

        await context.SaveChangesAsync();

        var service = GetService(context);

        var result = await service.GetOverdueLoansAsync();

        Assert.Single(result);
    }

    // -------------------------------
    // 5. CalculateLateFee
    // -------------------------------
    [Fact]
    public async Task CalculateLateFee_ShouldReturnCorrectFee()
    {
        var context = TestDbContextFactory.Create();

        context.Loans.Add(new Loan
        {
            Id = 1,
            DueDate = DateTime.UtcNow.AddDays(-5),
            ReturnDate = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        var service = GetService(context);

        var fee = await service.CalculateLateFeeAsync(1);

        Assert.True(fee > 0);
    }
}