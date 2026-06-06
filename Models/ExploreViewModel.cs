namespace NewTalent.Models;

public class ExploreViewModel
{
    // Filter values (from query string)
    public string? CategoryFilter { get; set; }
    public string? SkillLevelFilter { get; set; }
    public string? VideoTypeFilter { get; set; }
    public string? LocationFilter { get; set; }
    public string? QualityFilter { get; set; }
    public string SortBy { get; set; } = "trending";
    public string? DateRange { get; set; }
    public string? SearchQuery { get; set; }
    public string? TagsFilter { get; set; }

    // Results
    public List<VideoViewModel> Videos { get; set; } = new();
    public List<VideoViewModel> RecommendedVideos { get; set; } = new(); // Personalized recommendations
    public int TotalResults { get; set; }

    // Pagination
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalPages => (int)Math.Ceiling((double)TotalResults / PageSize);

    // Available filter options
    public List<string> Categories { get; set; } = new()
    {
        "Music", "Dance", "Art", "Acting", "Coding", "Writing", "Comedy", "Sports", "Cooking", "Magic", "Other"
    };

    public List<string> SkillLevels { get; set; } = new()
    {
        "Beginner", "Intermediate", "Advanced", "Professional"
    };

    public List<string> VideoTypes { get; set; } = new()
    {
        "performance", "tutorial", "exhibition", "behind_the_scenes", "other"
    };

    public List<string> Qualities { get; set; } = new()
    {
        "SD", "HD", "Full HD", "4K"
    };

    public List<string> SortOptions { get; set; } = new()
    {
        "trending", "recent", "popular", "liked", "viewed", "recommended"
    };

    public List<string> DateRanges { get; set; } = new()
    {
        "today", "week", "month", "year", "all"
    };

    // User preference tracking for recommendations
    public bool ShowRecommendations { get; set; } = true;
    public string? UserPreferredCategory { get; set; }
}
