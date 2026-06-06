namespace NewTalent.Data.Entities;

/// <summary>
/// Entity for talent contests
/// </summary>
public class Contest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string PrizeAmount { get; set; } = string.Empty;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; }
    public int MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
    public string SkillLevel { get; set; } = "all"; // beginner, intermediate, advanced, all
    public string Status { get; set; } = "active"; // active, completed, upcoming
    public bool IsFeatured { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Judging and voting
    public string JudgingType { get; set; } = "public"; // public, judges, hybrid
    public DateTime? VotingStartDate { get; set; }
    public DateTime? VotingEndDate { get; set; }
    public bool AllowPublicVoting { get; set; } = true;
    public int MaxVotesPerUser { get; set; } = 1;

    // Prizes and recognition
    public string? PrizeDescription { get; set; }
    public string? CareerOpportunity { get; set; }
    public string? SponsorName { get; set; }

    // Foreign key to the user who created the contest
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;

    // Navigation properties
    public List<ContestEntry> Entries { get; set; } = new();
}
