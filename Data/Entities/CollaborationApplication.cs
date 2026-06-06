namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing an application to join a collaboration
/// </summary>
public class CollaborationApplication
{
    public int Id { get; set; }

    public int CollaborationId { get; set; }
    public Collaboration Collaboration { get; set; } = null!;

    public int ApplicantId { get; set; }
    public User Applicant { get; set; } = null!;

    public string Message { get; set; } = string.Empty;
    public string? Skills { get; set; } // applicant's relevant skills
    public string? PortfolioUrl { get; set; }

    public string Status { get; set; } = "pending"; // pending, accepted, rejected
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedAt { get; set; }
}
