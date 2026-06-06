namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing a user's application to a job
/// </summary>
public class JobApplication
{
    public int Id { get; set; }

    public string CoverLetter { get; set; } = string.Empty;
    public string? ResumeUrl { get; set; }
    public string? PortfolioUrl { get; set; }

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "pending"; // pending, reviewed, shortlisted, rejected, hired

    // Foreign Keys
    public int JobId { get; set; }
    public Job Job { get; set; } = null!;

    public int ApplicantId { get; set; }
    public User Applicant { get; set; } = null!;
}
