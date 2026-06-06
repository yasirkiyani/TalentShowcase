namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing a review for a mentor
/// </summary>
public class MentorshipReview
{
    public int Id { get; set; }

    public int MentorshipProfileId { get; set; }
    public MentorshipProfile MentorshipProfile { get; set; } = null!;

    public int ReviewerId { get; set; }
    public User Reviewer { get; set; } = null!;

    public int Rating { get; set; } // 1-5 stars
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
