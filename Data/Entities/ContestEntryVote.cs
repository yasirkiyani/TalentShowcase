namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing a vote on a contest entry
/// </summary>
public class ContestEntryVote
{
    public int Id { get; set; }

    public int ContestEntryId { get; set; }
    public ContestEntry ContestEntry { get; set; } = null!;

    public int VoterId { get; set; }
    public User Voter { get; set; } = null!;

    public int Score { get; set; } // 1-10 score for judging
    public string? Comment { get; set; } // optional comment with vote
    public DateTime VotedAt { get; set; } = DateTime.UtcNow;

    // For professional judges
    public bool IsJudgeVote { get; set; } = false;
    public int? JudgeId { get; set; }
}
