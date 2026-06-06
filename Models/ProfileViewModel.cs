namespace NewTalent.Models;

public class AchievementViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateAwarded { get; set; }
    public string IconClass { get; set; } = "bi bi-trophy-fill";
    public string BadgeColor { get; set; } = "text-warning";
}

public class ProfileViewModel
{
    // Basic User Info
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
    public string ProfilePicture { get; set; } = "https://picsum.photos/seed/profile/150/150";
    public string? Bio { get; set; }

    // Talents & Skills
    public string SelectedTalents { get; set; } = string.Empty;
   

    // Stats
    public int VideosCount { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
    public int AwardsCount { get; set; }

    // Is current user viewing their own profile?
    public bool IsOwnProfile { get; set; } = true;
    public bool IsFollowing { get; set; } = false;

    // Content
    public List<VideoViewModel> Videos { get; set; } = new();
    public List<AchievementViewModel> Achievements { get; set; } = new();

    // Privacy Settings
    public bool PublicProfile { get; set; } = true;
    public bool ShowEmail { get; set; } = false;
    public bool AllowMessages { get; set; } = true;
    public bool ShowStats { get; set; } = true;

    // Available talent options for settings tab
    public List<string> AvailableTalents { get; set; } = new()
    {
        "Music", "Dance", "Art", "Acting", "Coding", "Writing", "Comedy", "Sports", "Other"
    };
}
