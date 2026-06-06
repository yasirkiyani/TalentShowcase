namespace NewTalent.Models;

/// <summary>
/// ViewModel for leaderboard entries
/// </summary>
public class LeaderboardEntryViewModel
{
    public int Rank { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = string.Empty;
    public int TotalViews { get; set; }
    public int TotalLikes { get; set; }
    public int TotalFollowers { get; set; }
    public int TotalVideos { get; set; }
    public double Score { get; set; } // Percentage score
    public bool IsCurrentUser { get; set; }
}

/// <summary>
/// ViewModel for video leaderboard entries
/// </summary>
public class VideoLeaderboardEntryViewModel
{
    public int Rank { get; set; }
    public int VideoId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public int ViewsCount { get; set; }
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
    public double Score { get; set; } // Percentage score
    public string ArtistName { get; set; } = string.Empty;
    public string ArtistAvatar { get; set; } = string.Empty;
}

/// <summary>
/// ViewModel for contest leaderboard entries
/// </summary>
public class ContestLeaderboardEntryViewModel
{
    public int Rank { get; set; }
    public int ContestId { get; set; }
    public string ContestTitle { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = string.Empty;
    public int TotalViews { get; set; }
    public int TotalLikes { get; set; }
    public int TotalVideos { get; set; }
    public double Score { get; set; } // Percentage score
    public bool IsWinner { get; set; }
    public bool IsOngoing { get; set; }
    public DateTime? ContestEndDate { get; set; }
}

/// <summary>
/// ViewModel for the leaderboard page
/// </summary>
public class LeaderboardViewModel
{
    public List<LeaderboardEntryViewModel> TopCreators { get; set; } = new();
    public List<VideoLeaderboardEntryViewModel> TopVideos { get; set; } = new();
    public List<ContestLeaderboardEntryViewModel> ContestWinners { get; set; } = new();
    public List<ContestLeaderboardEntryViewModel> OngoingContests { get; set; } = new();

    // Filter options
    public string Period { get; set; } = "all"; // all, week, month, year
    public string Category { get; set; } = "all";

    public List<string> Periods { get; set; } = new()
    {
        "all", "week", "month", "year"
    };

    public List<string> Categories { get; set; } = new()
    {
        "all", "Music", "Dance", "Art", "Acting", "Coding", "Writing", "Comedy", "Sports"
    };
}
