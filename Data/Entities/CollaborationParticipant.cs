namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing a participant in a collaboration
/// </summary>
public class CollaborationParticipant
{
    public int Id { get; set; }

    public int CollaborationId { get; set; }
    public Collaboration Collaboration { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string Role { get; set; } = string.Empty; // e.g., "Lead Vocalist", "Choreographer", "Director"
    public string? Contribution { get; set; }
    
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LeftAt { get; set; }
}
