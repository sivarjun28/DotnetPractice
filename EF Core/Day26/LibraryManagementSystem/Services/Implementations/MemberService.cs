using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.Requests;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Implementations
{
    public class MemberService : IMemberService
    {

        private readonly LibraryDbContext _context;
        public MemberService(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task DeactivateMemberAsync(int memberId)
        {
            var member = await _context.Members.FindAsync(memberId);

            if (member == null)
            {
                throw new Exception("Member not found.");
            }

            member.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Member>> GetExpiringMembershipsAsync(int daysAhead)
        {

            var targetDate = DateTime.UtcNow.AddDays(daysAhead);

            return await _context.Members
                        .Where(m => m.MembershipExpiryDate != null &&
                        m.MembershipExpiryDate <= targetDate &&
                        m.IsActive)
                        .ToListAsync();
        }

        public async Task<Member?> GetMemberByEmailAsync(string email)
        {
            return await _context.Members
                            .FirstOrDefaultAsync(m => m.Email == email);
        }

        public async Task<Member> RegisterMemberAsync(RegisterMemberRequest request)
        {
            var existingEmail = await _context.Members
                .FirstOrDefaultAsync(m => m.Email == request.Email);
            if (existingEmail != null)
            {
                throw new Exception("A member with this email already exists.");
            }
            var member = new Member
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                MembershipDate = DateTime.UtcNow,
                MembershipExpiryDate = DateTime.UtcNow.AddMonths(12),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<Member> RenewMembershipAsync(int memberId, int months)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member == null)
            {
                throw new Exception("Member Not Found");
            }
            var baseDate = member.MembershipExpiryDate ?? DateTime.UtcNow;

            member.MembershipExpiryDate = baseDate.AddMonths(months);

            member.IsActive = true;
            await _context.SaveChangesAsync();
            return member;
        }
    }
}