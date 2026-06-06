namespace NewTalent.Models;

public class ContestViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Prize { get; set; } = string.Empty;
    public string PrizeAmount { get; set; } = string.Empty;
    public string Status { get; set; } = "active"; // active, upcoming, ended
    public string BadgeColor { get; set; } = "success";

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string TimeRemaining => GetTimeRemaining();

    public int ParticipantsCount { get; set; }
    public int MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string OrganizerName { get; set; } = string.Empty;
    public bool IsEnrolled { get; set; } = false;
    public bool HasParticipated { get; set; } = false;
    public string SkillLevel { get; set; } = "all";
    public bool IsFeatured { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<ContestEntryViewModel> TopEntries { get; set; } = new();
    public List<string> Rules { get; set; } = new();
    public List<string> Prizes { get; set; } = new();

    private string GetTimeRemaining()
    {
        var remaining = EndDate - DateTime.UtcNow;
        if (remaining.TotalDays > 1) return $"{(int)remaining.TotalDays} days left";
        if (remaining.TotalHours > 1) return $"{(int)remaining.TotalHours} hours left";
        return "Ending soon";
    }
}

public class ContestEntryViewModel
{
    public int Id { get; set; }
    public int ContestId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserAvatar { get; set; } = string.Empty;
    public VideoViewModel? Video { get; set; }
    public int VotesCount { get; set; }
    public int Rank { get; set; }
    public bool IsVotedByCurrentUser { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    // Additional properties for backend data
    public string ContestTitle { get; set; } = string.Empty;
    public string ContestImageUrl { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MediaUrl { get; set; } = string.Empty;
    public string Status { get; set; } = "pending";
    public DateTime? ReviewedAt { get; set; }
}

public class ContestsPageViewModel
{
    public List<ContestViewModel> ActiveContests { get; set; } = new();
    public List<ContestViewModel> UpcomingContests { get; set; } = new();
    public List<ContestViewModel> PastContests { get; set; } = new();
    public List<ContestEntryViewModel> MyEntries { get; set; } = new();
    public List<ContestEntryViewModel> RecentWinners { get; set; } = new();
    public ContestViewModel? FeaturedContest { get; set; }

    // Filters
    public string? CategoryFilter { get; set; }
    public string? StatusFilter { get; set; }

    public List<string> Categories { get; set; } = new()
    {
        "Music", "Dance", "Art", "Acting", "Coding", "Writing", "Comedy", "Sports"
    };
}
