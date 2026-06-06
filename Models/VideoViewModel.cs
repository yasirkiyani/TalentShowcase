namespace NewTalent.Models;

public class VideoViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string SkillLevel { get; set; } = string.Empty;

    public string ThumbnailUrl { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string Duration { get; set; } = "0:00";

    // Engagement stats
    public int ViewsCount { get; set; }
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
    public int SharesCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public bool IsSavedByCurrentUser { get; set; }

    // Rating information
    public double AverageRating { get; set; }
    public int TotalRatings { get; set; }
    public int? UserRating { get; set; } // Current user's rating for this video

    // Tags
    public string Tags { get; set; } = string.Empty;

    // Artist info
    public int ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public string ArtistProfilePicture { get; set; } = string.Empty;
    public bool IsVerified { get; set; }

    // Privacy & settings
    public string Privacy { get; set; } = "public";
    public bool AllowComments { get; set; } = true;
    public bool AllowLikes { get; set; } = true;
    public bool AllowShares { get; set; } = true;
    public bool AllowDownloads { get; set; } = false;
    public string AgeRestriction { get; set; } = "all";

    // Timestamps
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public string UploadedAgo => GetTimeAgo(UploadedAt);

    // Comments on this video
    public List<CommentViewModel> Comments { get; set; } = new();

    private static string GetTimeAgo(DateTime dateTime)
    {
        var span = DateTime.UtcNow - dateTime;
        if (span.TotalDays > 365) return $"{(int)(span.TotalDays / 365)} year(s) ago";
        if (span.TotalDays > 30) return $"{(int)(span.TotalDays / 30)} month(s) ago";
        if (span.TotalDays > 1) return $"{(int)span.TotalDays} day(s) ago";
        if (span.TotalHours > 1) return $"{(int)span.TotalHours} hour(s) ago";
        if (span.TotalMinutes > 1) return $"{(int)span.TotalMinutes} minute(s) ago";
        return "Just now";
    }
}

public class CommentViewModel
{
    public int Id { get; set; }
    public int VideoId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserAvatar { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int LikesCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedAgo => GetTimeAgo(CreatedAt);
    public List<CommentViewModel> Replies { get; set; } = new();

    private static string GetTimeAgo(DateTime dateTime)
    {
        var span = DateTime.UtcNow - dateTime;
        if (span.TotalDays > 1) return $"{(int)span.TotalDays}d ago";
        if (span.TotalHours > 1) return $"{(int)span.TotalHours}h ago";
        if (span.TotalMinutes > 1) return $"{(int)span.TotalMinutes}m ago";
        return "Just now";
    }
}

public class AddCommentViewModel
{
    public int VideoId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int? ParentCommentId { get; set; }
}
