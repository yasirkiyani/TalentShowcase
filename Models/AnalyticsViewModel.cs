namespace NewTalent.Models;

public class AnalyticsDataPoint
{
    public string Label { get; set; } = string.Empty;
    public int Value { get; set; }
    public string Date { get; set; } = string.Empty;
    public int Views { get; set; }
    public int Likes { get; set; }
}

public class AnalyticsViewModel
{
    // Overview Stats
    public int TotalViews { get; set; }
    public int TotalLikes { get; set; }
    public int TotalComments { get; set; }
    public int TotalShares { get; set; }
    public int TotalFollowers { get; set; }
    public int TotalVideos { get; set; }

    // Growth percentages (compared to last period)
    public double ViewsGrowth { get; set; }
    public double LikesGrowth { get; set; }
    public double CommentsGrowth { get; set; }
    public double FollowersGrowth { get; set; }

    // Chart data (last 30 days)
    public List<AnalyticsDataPoint> ViewsOverTime { get; set; } = new();
    public List<AnalyticsDataPoint> LikesOverTime { get; set; } = new();
    public List<AnalyticsDataPoint> FollowersOverTime { get; set; } = new();
    public List<AnalyticsDataPoint> CommentsOverTime { get; set; } = new();
    public List<AnalyticsDataPoint> ChartData { get; set; } = new();

    // Top performing videos
    public List<VideoViewModel> TopVideos { get; set; } = new();

    // Audience demographics
    public List<AnalyticsDataPoint> ViewsByCategory { get; set; } = new();
    public List<AnalyticsDataPoint> ViewsByCountry { get; set; } = new();
    public List<AnalyticsDataPoint> ViewsByDevice { get; set; } = new();
    public List<AnalyticsDataPoint> ViewsByAge { get; set; } = new();
    public Dictionary<string, int> AudienceDemographics { get; set; } = new();

    // Engagement breakdown
    public double AverageWatchTime { get; set; }
    public double EngagementRate { get; set; }
    public double ClickThroughRate { get; set; }

    // Date range filter
    public string DateRange { get; set; } = "30days";
    public DateTime FromDate { get; set; } = DateTime.UtcNow.AddDays(-30);
    public DateTime ToDate { get; set; } = DateTime.UtcNow;
}
