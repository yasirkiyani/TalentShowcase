namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing analytics data for a video
/// </summary>
public class VideoAnalytics
{
    public int Id { get; set; }

    public int VideoId { get; set; }
    public Video Video { get; set; } = null!;

    // View metrics
    public int TotalViews { get; set; } = 0;
    public int UniqueViews { get; set; } = 0;
    public double AverageWatchTime { get; set; } = 0; // in seconds
    public double CompletionRate { get; set; } = 0; // percentage

    // Engagement metrics
    public int TotalLikes { get; set; } = 0;
    public int TotalComments { get; set; } = 0;
    public int TotalShares { get; set; } = 0;
    public int TotalRatings { get; set; } = 0;
    public double AverageRating { get; set; } = 0;

    // Demographics (simplified - could be expanded with separate tables)
    public string? TopCountries { get; set; } // comma-separated top countries
    public string? TopCities { get; set; } // comma-separated top cities
    public string? AgeGroups { get; set; } // comma-separated age group percentages
    public string? GenderDistribution { get; set; } // comma-separated gender percentages

    // Traffic sources
    public int DirectViews { get; set; } = 0;
    public int SearchViews { get; set; } = 0;
    public int SocialViews { get; set; } = 0;
    public int ExternalViews { get; set; } = 0;

    // Timestamps
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
