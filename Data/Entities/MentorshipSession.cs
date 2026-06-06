namespace NewTalent.Data.Entities;

/// <summary>
/// Entity for mentorship sessions
/// </summary>
public class MentorshipSession
{
    public int Id { get; set; }
    public int MentorId { get; set; }
    public int MenteeId { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string Status { get; set; } = "scheduled"; // scheduled, confirmed, completed, cancelled
    public string? Notes { get; set; }
    public int DurationMinutes { get; set; } = 60;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User Mentor { get; set; } = null!;
    public User Mentee { get; set; } = null!;
}
