using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services.Implementations;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models.Requests;
using Xunit;

public class MemberServiceTests
{
    private MemberService GetService(LibraryDbContext context)
    {
        return new MemberService(context);
    }

    // -------------------------
    // 1. Register Member
    // -------------------------
    [Fact]
    public async Task RegisterMember_ShouldCreateMember()
    {
        var context = TestDbContextFactory.Create();
        var service = GetService(context);

        var request = new RegisterMemberRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Phone = "9999999999",
            Address = "Bangalore"
        };

        var result = await service.RegisterMemberAsync(request);

        Assert.NotNull(result);
        Assert.Equal("john@test.com", result.Email);
        Assert.True(context.Members.Any());
    }

    // -------------------------
    // 2. Get By Email
    // -------------------------
    [Fact]
    public async Task GetMemberByEmail_ShouldReturnMember()
    {
        var context = TestDbContextFactory.Create();

        context.Members.Add(new Member
        {
            Id = 1,
            Email = "test@mail.com",
            FirstName = "Test",
            LastName = "User",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        var service = GetService(context);

        var result = await service.GetMemberByEmailAsync("test@mail.com");

        Assert.NotNull(result);
        Assert.Equal("Test", result!.FirstName);
    }

    // -------------------------
    // 3. Deactivate Member
    // -------------------------
    [Fact]
    public async Task DeactivateMember_ShouldSetInactive()
    {
        var context = TestDbContextFactory.Create();

        var member = new Member
        {
            Id = 1,
            FirstName = "John",
            IsActive = true,
            Email = "a@b.com",
            CreatedAt = DateTime.UtcNow
        };

        context.Members.Add(member);
        await context.SaveChangesAsync();

        var service = GetService(context);

        await service.DeactivateMemberAsync(1);

        Assert.False(member.IsActive);
    }

    // -------------------------
    // 4. Renew Membership
    // -------------------------
    [Fact]
    public async Task RenewMembership_ShouldExtendExpiry()
    {
        var context = TestDbContextFactory.Create();

        var member = new Member
        {
            Id = 1,
            Email = "test@test.com",
            IsActive = true,
            MembershipExpiryDate = DateTime.UtcNow.AddMonths(1),
            CreatedAt = DateTime.UtcNow
        };

        context.Members.Add(member);
        await context.SaveChangesAsync();

        var service = GetService(context);

        var result = await service.RenewMembershipAsync(1, 2);

        Assert.True(result.MembershipExpiryDate > DateTime.UtcNow.AddMonths(2));
    }

    // -------------------------
    // 5. Expiring Memberships
    // -------------------------
    [Fact]
    public async Task GetExpiringMemberships_ShouldReturnExpiringMembers()
    {
        var context = TestDbContextFactory.Create();

        context.Members.Add(new Member
        {
            Id = 1,
            Email = "a@test.com",
            IsActive = true,
            MembershipExpiryDate = DateTime.UtcNow.AddDays(2),
            CreatedAt = DateTime.UtcNow
        });

        context.Members.Add(new Member
        {
            Id = 2,
            Email = "b@test.com",
            IsActive = true,
            MembershipExpiryDate = DateTime.UtcNow.AddMonths(2),
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        var service = GetService(context);

        var result = await service.GetExpiringMembershipsAsync(5);

        Assert.Single(result);
    }
}