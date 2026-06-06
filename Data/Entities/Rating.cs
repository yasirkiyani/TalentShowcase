namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing a user's rating on a video (1-5 stars)
/// </summary>
public class Rating
{
    public int Id { get; set; }

    // Rating value (1-5 stars)
    public int Stars { get; set; }

    // Optional review/comment with the rating
    public string? Review { get; set; }

    public DateTime RatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int VideoId { get; set; }
    public Video Video { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
