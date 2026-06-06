namespace NewTalent.Data.Entities;

/// <summary>
/// Entity for community posts
/// </summary>
public class CommunityPost
{
    public int Id { get; set; }
    public int CommunityId { get; set; }
    public int AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? MediaUrl { get; set; }
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Community Community { get; set; } = null!;
    public User Author { get; set; } = null!;
    public List<CommunityPostComment> Comments { get; set; } = new();
}
