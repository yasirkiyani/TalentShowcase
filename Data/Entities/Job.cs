namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing a job/opportunity posting
/// </summary>
public class Job
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public string? Benefits { get; set; }

    public string Category { get; set; } = string.Empty;
    public string JobType { get; set; } = "full-time"; // full-time, part-time, contract, freelance
    public string Location { get; set; } = string.Empty;
    public bool IsRemote { get; set; } = false;

    public string SalaryRange { get; set; } = string.Empty;
    public string ExperienceLevel { get; set; } = "entry"; // entry, mid, senior, executive

    public string CompanyName { get; set; } = string.Empty;
    public string? CompanyLogo { get; set; }
    public string? CompanyWebsite { get; set; }

    public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    public DateTime? Deadline { get; set; }
    public bool IsActive { get; set; } = true;

    // Additional fields for networking
    public string? TalentCategories { get; set; } // comma-separated talent categories sought
    public string? AuditionDetails { get; set; } // audition information
    public bool IsFeatured { get; set; } = false;

    // Foreign Key
    public int PostedById { get; set; }
    public User PostedBy { get; set; } = null!;

    // Optional link to TalentScoutProfile
    public int? TalentScoutProfileId { get; set; }
    public TalentScoutProfile? TalentScoutProfile { get; set; }

    // Navigation
    public List<JobApplication> Applications { get; set; } = new();
}
