namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing a mentor's profile
/// </summary>
public class MentorshipProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // Mentor details
    public string Title { get; set; } = string.Empty; // e.g., "Professional Music Producer", "Award-winning Choreographer"
    public string Bio { get; set; } = string.Empty;
    public string? Experience { get; set; } // years of experience
    public string? ExpertiseAreas { get; set; } // comma-separated areas of expertise
    public string? Qualifications { get; set; } // degrees, certifications, awards
    public string? PortfolioUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? LinkedInUrl { get; set; }

    // Mentorship availability
    public bool IsAvailable { get; set; } = true;
    public int MaxMentees { get; set; } = 5;
    public int CurrentMentees { get; set; } = 0;
    public string SessionFormat { get; set; } = "online"; // online, in_person, hybrid
    public decimal HourlyRate { get; set; } = 0; // 0 means free
    public string? AvailabilitySchedule { get; set; } // e.g., "Weekdays 9AM-5PM"

    // Rating and reviews
    public double AverageRating { get; set; } = 0;
    public int TotalReviews { get; set; } = 0;

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public List<MentorshipSession> MentorshipSessions { get; set; } = new();
    public List<MentorshipReview> Reviews { get; set; } = new();
}
