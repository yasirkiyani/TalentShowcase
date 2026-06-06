namespace NewTalent.Data.Entities;

/// <summary>
/// Entity for user communities
/// </summary>
public class Community
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int MembersCount { get; set; }
    public string Status { get; set; } = "Active";
    public int PostsToday { get; set; }
    public string Privacy { get; set; } = "public";
    public bool AllowPosts { get; set; } = true;
    public bool AllowEvents { get; set; } = true;
    public string? Rules { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key to the user who created the community
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;

    // Navigation properties
    public List<CommunityMember> Members { get; set; } = new();
    public List<CommunityPost> Posts { get; set; } = new();
}
