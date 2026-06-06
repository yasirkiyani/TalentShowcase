namespace NewTalent.Models;

public class MentorViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<string> Expertise { get; set; } = new();

    public double Rating { get; set; }
    public int ReviewsCount { get; set; }
    public int StudentsCount { get; set; }
    public int SessionsCount { get; set; }
    public string HourlyRate { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public bool IsFeatured { get; set; } = false;

    public List<string> Languages { get; set; } = new();
    public List<string> AvailableDays { get; set; } = new();
    public bool IsFollowing { get; set; } = false;

    // Additional properties for backend data
    public string SelectedTalents { get; set; } = string.Empty;
    public int VideoCount { get; set; }
    public DateTime? NextSession { get; set; }
    public string SessionStatus { get; set; } = "active";
}

public class MentorshipSessionViewModel
{
    public int Id { get; set; }
    public int MentorId { get; set; }
    public string MentorName { get; set; } = string.Empty;
    public string MentorAvatar { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Duration { get; set; } = "60 min";
    public string Price { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public string Status { get; set; } = "scheduled"; // scheduled, completed, cancelled
    public string MeetingLink { get; set; } = string.Empty;

    // Additional properties for backend data
    public string Topic { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string MenteeName { get; set; } = string.Empty;
}

public class MentorshipPageViewModel
{
    // Featured & top mentors
    public List<MentorViewModel> FeaturedMentors { get; set; } = new();
    public List<MentorViewModel> AllMentors { get; set; } = new();

    // My sessions (if logged in)
    public List<MentorshipSessionViewModel> UpcomingSessions { get; set; } = new();
    public List<MentorshipSessionViewModel> PastSessions { get; set; } = new();

    // Additional properties for backend data
    public List<MentorViewModel> MyMentors { get; set; } = new();
    public List<MenteeViewModel> MyMentees { get; set; } = new();
    public List<MentorViewModel> AvailableMentors { get; set; } = new();

    // Filters
    public string? CategoryFilter { get; set; }
    public string? ExpertiseFilter { get; set; }
    public string? AvailabilityFilter { get; set; }
    public string SortBy { get; set; } = "rating";

    public List<string> Categories { get; set; } = new()
    {
        "Music", "Dance", "Art", "Acting", "Coding", "Writing", "Comedy", "Sports"
    };
}

public class MenteeViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string SelectedTalents { get; set; } = string.Empty;
    public int SessionCount { get; set; }
    public int Progress { get; set; }
}
