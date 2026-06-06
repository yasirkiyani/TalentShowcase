using System.ComponentModel.DataAnnotations;

namespace NewTalent.Models;

/// <summary>
/// ViewModel for a job/opportunity
/// </summary>
public class JobViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public string? Benefits { get; set; }

    public string Category { get; set; } = string.Empty;
    public string JobType { get; set; } = "full-time";
    public string Location { get; set; } = string.Empty;
    public bool IsRemote { get; set; } = false;

    public string SalaryRange { get; set; } = string.Empty;
    public string ExperienceLevel { get; set; } = "entry";

    public string CompanyName { get; set; } = string.Empty;
    public string? CompanyLogo { get; set; }
    public string? CompanyWebsite { get; set; }

    public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    public DateTime? Deadline { get; set; }
    public bool IsActive { get; set; } = true;
    public string PostedAgo => GetTimeAgo(PostedAt);
    public string DeadlineText => Deadline.HasValue ? Deadline.Value.ToString("MMM dd, yyyy") : "No deadline";

    public int PostedById { get; set; }
    public string PostedByName { get; set; } = string.Empty;
    public string PostedByAvatar { get; set; } = string.Empty;

    public int ApplicationsCount { get; set; }
    public bool HasApplied { get; set; } = false;
    public bool IsOwner { get; set; } = false;

    private static string GetTimeAgo(DateTime dateTime)
    {
        var span = DateTime.UtcNow - dateTime;
        if (span.TotalDays > 30) return $"{(int)(span.TotalDays / 30)} month(s) ago";
        if (span.TotalDays > 1) return $"{(int)span.TotalDays} day(s) ago";
        if (span.TotalHours > 1) return $"{(int)span.TotalHours} hour(s) ago";
        return "Just now";
    }
}

/// <summary>
/// ViewModel for a job application
/// </summary>
public class JobApplicationViewModel
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;

    public string CoverLetter { get; set; } = string.Empty;
    public string? ResumeUrl { get; set; }
    public string? PortfolioUrl { get; set; }

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public string AppliedAgo => GetTimeAgo(AppliedAt);
    public string Status { get; set; } = "pending";
    public string StatusBadgeColor => GetStatusBadgeColor(Status);

    public int ApplicantId { get; set; }
    public string ApplicantName { get; set; } = string.Empty;
    public string ApplicantAvatar { get; set; } = string.Empty;

    private static string GetTimeAgo(DateTime dateTime)
    {
        var span = DateTime.UtcNow - dateTime;
        if (span.TotalDays > 1) return $"{(int)span.TotalDays}d ago";
        if (span.TotalHours > 1) return $"{(int)span.TotalHours}h ago";
        return "Just now";
    }

    private static string GetStatusBadgeColor(string status)
    {
        return status.ToLower() switch
        {
            "pending" => "warning",
            "reviewed" => "info",
            "shortlisted" => "primary",
            "rejected" => "danger",
            "hired" => "success",
            _ => "secondary"
        };
    }
}

/// <summary>
/// ViewModel for the job board page
/// </summary>
public class JobBoardViewModel
{
    public List<JobViewModel> Jobs { get; set; } = new();
    public List<JobApplicationViewModel> MyApplications { get; set; } = new();
    public int TotalJobs { get; set; }

    // Filters
    public string? CategoryFilter { get; set; }
    public string? JobTypeFilter { get; set; }
    public string? LocationFilter { get; set; }
    public string? ExperienceFilter { get; set; }
    public string? SearchQuery { get; set; }
    public bool ShowRemoteOnly { get; set; } = false;
    public string SortBy { get; set; } = "recent";

    // Pagination
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalPages => (int)Math.Ceiling((double)TotalJobs / PageSize);

    public List<string> Categories { get; set; } = new()
    {
        "Music", "Dance", "Art", "Acting", "Coding", "Writing", "Comedy", "Sports", "Other"
    };

    public List<string> JobTypes { get; set; } = new()
    {
        "full-time", "part-time", "contract", "freelance"
    };

    public List<string> ExperienceLevels { get; set; } = new()
    {
        "entry", "mid", "senior", "executive"
    };
}

/// <summary>
/// ViewModel for creating a job
/// </summary>
public class CreateJobViewModel
{
    [Required(ErrorMessage = "Job title is required")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Requirements are required")]
    public string Requirements { get; set; } = string.Empty;

    public string? Benefits { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job type is required")]
    public string JobType { get; set; } = "full-time";

    [Required(ErrorMessage = "Location is required")]
    public string Location { get; set; } = string.Empty;

    public bool IsRemote { get; set; } = false;

    public string SalaryRange { get; set; } = string.Empty;

    [Required(ErrorMessage = "Experience level is required")]
    public string ExperienceLevel { get; set; } = "entry";

    [Required(ErrorMessage = "Company name is required")]
    public string CompanyName { get; set; } = string.Empty;

    public string? CompanyLogo { get; set; }
    public string? CompanyWebsite { get; set; }

    public DateTime? Deadline { get; set; }

    // Additional fields for networking
    public string? TalentCategories { get; set; }
    public string? AuditionDetails { get; set; }
    public bool IsFeatured { get; set; } = false;
}

/// <summary>
/// ViewModel for applying to a job
/// </summary>
public class ApplyJobViewModel
{
    public int JobId { get; set; }

    [Required(ErrorMessage = "Cover letter is required")]
    public string CoverLetter { get; set; } = string.Empty;

    public string? ResumeUrl { get; set; }
    public string? PortfolioUrl { get; set; }
}
