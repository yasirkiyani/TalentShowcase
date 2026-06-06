using System.ComponentModel.DataAnnotations;

namespace NewTalent.Models;

/// <summary>
/// ViewModel for rating a video
/// </summary>
public class RatingViewModel
{
    public int Id { get; set; }
    public int VideoId { get; set; }
    public int UserId { get; set; }
    public int Stars { get; set; }
    public string? Review { get; set; }
    public DateTime RatedAt { get; set; } = DateTime.UtcNow;
    public string RatedAgo => GetTimeAgo(RatedAt);
    public string UserName { get; set; } = string.Empty;
    public string UserAvatar { get; set; } = string.Empty;

    private static string GetTimeAgo(DateTime dateTime)
    {
        var span = DateTime.UtcNow - dateTime;
        if (span.TotalDays > 1) return $"{(int)span.TotalDays}d ago";
        if (span.TotalHours > 1) return $"{(int)span.TotalHours}h ago";
        if (span.TotalMinutes > 1) return $"{(int)span.TotalMinutes}m ago";
        return "Just now";
    }
}

/// <summary>
/// ViewModel for adding a rating
/// </summary>
public class AddRatingViewModel
{
    public int VideoId { get; set; }
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars")]
    public int Stars { get; set; }
    public string? Review { get; set; }
}
