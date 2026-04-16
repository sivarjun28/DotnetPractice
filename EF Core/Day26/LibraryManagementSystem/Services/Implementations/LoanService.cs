using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.Entities;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Implementations
{
    public class LoanService : ILoanService
    {
        private readonly LibraryDbContext _context;
        private const decimal LateFeePerDay = 0.50m;
        public LoanService(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<decimal> CalculateLateFeeAsync(int loanId)
        {
            var loan = await _context.Loans.FindAsync(loanId);
            if(loan == null)
                throw new Exception("Loan Not Found");
            
            DateTime endDate;
            if(loan.ReturnDate.HasValue)
                endDate = loan.ReturnDate.Value;
            else 
                endDate = DateTime.UtcNow;
            
            if(endDate <= loan.DueDate)
                return 0;
            var daysLate = (endDate - loan.DueDate).Days;

            return daysLate * LateFeePerDay;
        }

        public async Task<Loan> CheckoutBookAsync(int bookId, int memberId, int loanDays = 14)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                throw new Exception("Book Not Found");
            if (book.AvailableCopies <= 0)
                throw new Exception("Book is Not Available");
            var member = await _context.Members.FindAsync(memberId);
            if (member == null && !member.IsActive)
                throw new Exception("Invalid or Inactive member");

            var hasOverDueLoans = await _context.Loans
                                .AnyAsync(l => l.MemberId == memberId &&
                                        l.Status == LoanStatus.Active &&
                                        l.DueDate < DateTime.UtcNow);
            if (hasOverDueLoans)
                throw new Exception("Member has overdues");

            var loan = new Loan
            {
                BookId = bookId,
                MemberId = memberId,
                DueDate = DateTime.UtcNow.AddDays(loanDays),
                LoanDate = DateTime.UtcNow,
                Status = LoanStatus.Active
            };
            book.AvailableCopies--;
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return loan;
        }

        public async Task<List<Loan>> GetActiveLoansByMemberAsync(int memberId)
        {
            return await _context.Loans
            .Where(l => l.MemberId == memberId && l.Status == LoanStatus.Active)
            .OrderBy(l => l.DueDate)
            .ToListAsync();
        }

        public async Task<Loan> ReturnBookAsync(int loanId)
        {
            var loan = await _context.Loans.FindAsync(loanId);

            if (loan == null)
                throw new Exception("Loan Not Found");

            if (loan.ReturnDate != null)
                throw new Exception("Book Already Returned");

            var book = await _context.Books.FindAsync(loan.BookId);
            if (book == null)
                throw new Exception("Book Not Found");
            loan.ReturnDate = DateTime.UtcNow;

            if (loan.ReturnDate > loan.DueDate)
            {
                var daysLate = (loan.ReturnDate.Value - loan.DueDate).Days;
                loan.LateFee = daysLate * LateFeePerDay;
                loan.Status = LoanStatus.Overdue;
            }
            else
            {
                loan.LateFee = 0;
                loan.Status = LoanStatus.Returned;
            }

            book.AvailableCopies++;

            await _context.SaveChangesAsync();

            return loan;

        }

        public async Task<List<Loan>> GetOverdueLoansAsync()
        {
            var today = DateTime.UtcNow;

            return await _context.Loans
                            .Where(l => l.Status == LoanStatus.Active && l.DueDate < today)
                            .OrderBy(l => l.DueDate)
                            .ToListAsync();
        }
    }
}