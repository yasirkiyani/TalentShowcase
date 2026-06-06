namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing a leaderboard entry
/// </summary>
public class Leaderboard
{
    public int Id { get; set; }

    public string Category { get; set; } = string.Empty; // Music, Dance, Art, etc.
    public string Period { get; set; } = string.Empty; // weekly, monthly, yearly, all_time
    public string Type { get; set; } = string.Empty; // overall, contest, challenge

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int Rank { get; set; }
    public int Points { get; set; }
    public int Wins { get; set; }
    public int Participations { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
