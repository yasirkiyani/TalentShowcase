namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing a talent scout/recruiter profile
/// </summary>
public class TalentScoutProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // Scout details
    public string CompanyName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty; // e.g., "Talent Scout", "Recruiter", "Casting Director"
    public string Bio { get; set; } = string.Empty;
    public string? Industry { get; set; } // e.g., "Music", "Film", "Television", "Theater"
    public string? Specializations { get; set; } // comma-separated specializations
    public string? CompanyWebsite { get; set; }
    public string? LinkedInUrl { get; set; }

    // Availability
    public bool IsActive { get; set; } = true;
    public bool IsVerified { get; set; } = false;

    // Stats
    public int TotalJobsPosted { get; set; } = 0;
    public int TotalHires { get; set; } = 0;

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public List<Job> Jobs { get; set; } = new();
}
