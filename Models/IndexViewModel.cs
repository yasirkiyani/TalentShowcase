namespace NewTalent.Models;

public class IndexViewModel
{
    // Hero section
    public string HeroTitle { get; set; } = "Showcase Your Talent to the World";
    public string HeroSubtitle { get; set; } = "Join thousands of talented artists, musicians, dancers, and creators";
    public int TotalArtists { get; set; }
    public int TotalVideos { get; set; }
    public int TotalCommunities { get; set; }

    // Featured / Trending Videos
    public List<VideoViewModel> TrendingVideos { get; set; } = new();
    public List<VideoViewModel> FeaturedVideos { get; set; } = new();
    public List<VideoViewModel> NewestVideos { get; set; } = new();

    // Featured Artists
    public List<ProfileViewModel> FeaturedArtists { get; set; } = new();

    // Active Contests preview
    public List<ContestViewModel> ActiveContests { get; set; } = new();

    // Popular Communities preview
    public List<CommunityViewModel> PopularCommunities { get; set; } = new();

    // Categories with video counts
    public List<CategorySummary> Categories { get; set; } = new();

    // Is user logged in?
    public bool IsLoggedIn { get; set; } = false;
    public string? CurrentUserName { get; set; }
}

public class CategorySummary
{
    public string Name { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public string ColorClass { get; set; } = string.Empty;
    public int VideoCount { get; set; }
    public string ThumbnailUrl { get; set; } = string.Empty;
}
