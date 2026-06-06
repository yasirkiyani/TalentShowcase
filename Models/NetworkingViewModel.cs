namespace NewTalent.Models;

public class NetworkingProfileViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public List<string> Talents { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public int FollowersCount { get; set; }
    public int MutualConnections { get; set; }
    public bool IsFollowing { get; set; } = false;
    public bool IsVerified { get; set; } = false;
    public string SkillLevel { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public bool IsOnline { get; set; } = false;

    // Additional properties for backend data
    public int VideoCount { get; set; }
    public bool IsConnected { get; set; } = false;
    public string SelectedTalents { get; set; } = string.Empty;
}

public class ConnectionRequestViewModel
{
    public int Id { get; set; }
    public int FromUserId { get; set; }
    public string FromUserName { get; set; } = string.Empty;
    public string FromUserAvatar { get; set; } = string.Empty;
    public string FromUserTitle { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "pending"; // pending, accepted, declined
}

public class NetworkingPageViewModel
{
    // Suggested people to connect with
    public List<NetworkingProfileViewModel> SuggestedConnections { get; set; } = new();

    // People user is following
    public List<NetworkingProfileViewModel> Following { get; set; } = new();

    // People following user
    public List<NetworkingProfileViewModel> Followers { get; set; } = new();

    // Pending connection requests
    public List<ConnectionRequestViewModel> PendingRequests { get; set; } = new();

    // Trending creators
    public List<NetworkingProfileViewModel> TrendingCreators { get; set; } = new();

    // Additional properties for backend data
    public List<NetworkingProfileViewModel> Profiles { get; set; } = new();
    public List<ConnectionRequestViewModel> ConnectionRequests { get; set; } = new();

    // Filters
    public string? CategoryFilter { get; set; }
    public string? LocationFilter { get; set; }
    public string? SkillLevelFilter { get; set; }
    public string? SearchQuery { get; set; }
    public string SortBy { get; set; } = "mutual";

    // Stats
    public int TotalConnections { get; set; }
    public int TotalFollowers { get; set; }
    public int TotalFollowing { get; set; }

    public List<string> Categories { get; set; } = new()
    {
        "Music", "Dance", "Art", "Acting", "Coding", "Writing", "Comedy", "Sports"
    };
}
