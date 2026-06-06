namespace NewTalent.Data.Entities;

public class UserPreference
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    // Preferred categories (comma-separated)
    public string PreferredCategories { get; set; } = string.Empty;
    
    // Preferred skill levels (comma-separated)
    public string PreferredSkillLevels { get; set; } = string.Empty;
    
    // Preferred video types (comma-separated)
    public string PreferredVideoTypes { get; set; } = string.Empty;
    
    // Interaction tracking for recommendations
    public int TotalVideosWatched { get; set; } = 0;
    public int TotalLikesGiven { get; set; } = 0;
    public int TotalCommentsMade { get; set; } = 0;
    public int TotalSharesMade { get; set; } = 0;
    
    // Last activity timestamp
    public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;
    
    // Recommendation settings
    public bool EnableRecommendations { get; set; } = true;
    public string RecommendationAlgorithm { get; set; } = "collaborative"; // collaborative, content_based, hybrid
    
    // Created/Updated timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
