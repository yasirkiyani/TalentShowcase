namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing analytics data for a user
/// </summary>
public class UserAnalytics
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // Follower metrics
    public int TotalFollowers { get; set; } = 0;
    public int NewFollowersThisWeek { get; set; } = 0;
    public int NewFollowersThisMonth { get; set; } = 0;

    // Content metrics
    public int TotalVideos { get; set; } = 0;
    public int TotalViews { get; set; } = 0;
    public int TotalLikes { get; set; } = 0;
    public int TotalComments { get; set; } = 0;
    public int TotalShares { get; set; } = 0;

    // Engagement metrics
    public double AverageEngagementRate { get; set; } = 0; // percentage
    public double AverageViewDuration { get; set; } = 0; // in seconds
    public int TotalWatchTime { get; set; } = 0; // in seconds

    // Top performing content
    public string? TopVideoId { get; set; }
    public int TopVideoViews { get; set; } = 0;

    // Audience demographics
    public string? TopCountries { get; set; } // comma-separated top countries
    public string? AgeGroups { get; set; } // comma-separated age group percentages
    public string? GenderDistribution { get; set; } // comma-separated gender percentages

    // Timestamps
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
