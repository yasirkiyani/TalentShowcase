namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing a collaboration opportunity between users
/// </summary>
public class Collaboration
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Project details
    public string Category { get; set; } = string.Empty; // Music, Dance, Art, etc.
    public string ProjectType { get; set; } = string.Empty; // joint_performance, contest, creative_project, other
    public string RequiredSkills { get; set; } = string.Empty; // comma-separated skills
    public string? ProjectTimeline { get; set; }
    
    // Status
    public string Status { get; set; } = "open"; // open, in_progress, completed, cancelled
    public DateTime Deadline { get; set; }
    
    // Creator info
    public int CreatorId { get; set; }
    public User Creator { get; set; } = null!;
    
    // Participants
    public int MaxParticipants { get; set; } = 2;
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation
    public List<CollaborationApplication> Applications { get; set; } = new();
    public List<CollaborationParticipant> Participants { get; set; } = new();
}
