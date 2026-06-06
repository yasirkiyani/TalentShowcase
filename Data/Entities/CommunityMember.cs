namespace NewTalent.Data.Entities;

/// <summary>
/// Entity for community membership
/// </summary>
public class CommunityMember
{
    public int Id { get; set; }
    public int CommunityId { get; set; }
    public int UserId { get; set; }
    public string Role { get; set; } = "member"; // member, moderator, admin
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public int ContributionCount { get; set; }

    // Navigation properties
    public Community Community { get; set; } = null!;
    public User User { get; set; } = null!;
}
