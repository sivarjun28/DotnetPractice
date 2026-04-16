using LibraryManagementSystem.Models.Entities;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface ILoanService
    {
        Task<Loan> CheckoutBookAsync(int bookId, int memberId, int loanDays);
        Task<Loan> ReturnBookAsync(int loanId);
        Task<List<Loan>> GetOverdueLoansAsync();
        Task<List<Loan>> GetActiveLoansByMemberAsync(int memberId);
        Task<Decimal> CalculateLateFeeAsync(int loanId);
    }
}