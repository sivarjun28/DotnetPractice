using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.Requests;

namespace LibraryManagementSystem.Services.Interfaces
{
public interface IMemberService
{
    Task<Member> RegisterMemberAsync(RegisterMemberRequest request);
    Task<Member> RenewMembershipAsync(int memberId, int months);
    Task<Member?> GetMemberByEmailAsync(string email);
    Task<List<Member>> GetExpiringMembershipsAsync(int daysAhead);
    Task DeactivateMemberAsync(int memberId);
}
}