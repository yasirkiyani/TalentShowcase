namespace NewTalent.Data.Entities;

/// <summary>
/// Entity for community post comments
/// </summary>
public class CommunityPostComment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public CommunityPost Post { get; set; } = null!;
    public User User { get; set; } = null!;
}
