namespace NewTalent.Data.Entities;

public class Video
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string VideoUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;
    public string SkillLevel { get; set; } = "beginner";

    public string? Tags { get; set; }

    // Video Format & Type
    public string VideoFormat { get; set; } = string.Empty; // MP4, AVI, MOV, WebM, etc.
    public string VideoType { get; set; } = "performance"; // performance, tutorial, exhibition, other
    public long FileSize { get; set; } = 0; // in bytes
    public string Duration { get; set; } = "0:00"; // video duration
    public string Quality { get; set; } = "HD"; // SD, HD, Full HD, 4K

    // Streaming & Processing
    public bool IsProcessed { get; set; } = false;
    public string? StreamingUrl { get; set; } // URL for optimized streaming version
    public bool ThumbnailGenerated { get; set; } = false;

    public int ViewsCount { get; set; } = 0;

    // Privacy & Permissions
    public string Privacy { get; set; } = "public"; // public, private, unlisted
    public bool AllowComments { get; set; } = true;
    public bool AllowLikes { get; set; } = true;
    public bool AllowShares { get; set; } = true;
    public bool AllowDownloads { get; set; } = false;

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    // FK
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // Navigation
    public List<Comment> Comments { get; set; } = new();
    public List<Like> Likes { get; set; } = new();
    public List<Share> Shares { get; set; } = new();
    public List<Rating> Ratings { get; set; } = new();

    // Rating summary fields (computed from Ratings)
    public double AverageRating { get; set; } = 0;
    public int TotalRatings { get; set; } = 0;
}