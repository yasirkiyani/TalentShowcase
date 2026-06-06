namespace NewTalent.Data.Entities;

/// <summary>
/// Entity for contest entries/submissions
/// </summary>
public class ContestEntry
{
    public int Id { get; set; }
    public int ContestId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MediaUrl { get; set; } = string.Empty;
    public string Status { get; set; } = "pending"; // pending, under_review, approved, rejected, winner
    public int Rank { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedAt { get; set; }

    // Navigation properties
    public Contest Contest { get; set; } = null!;
    public User User { get; set; } = null!;
    public List<ContestEntryVote> Votes { get; set; } = new();
}
