namespace NewTalent.Models;

/// <summary>
/// ViewModel for a user notification
/// </summary>
public class NotificationViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "info";
    public string? Link { get; set; }

    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedAgo => GetTimeAgo(CreatedAt);

    public string IconClass => GetIconClass(Type);
    public string BadgeColor => GetBadgeColor(Type);

    private static string GetTimeAgo(DateTime dateTime)
    {
        var span = DateTime.UtcNow - dateTime;
        if (span.TotalDays > 1) return $"{(int)span.TotalDays}d ago";
        if (span.TotalHours > 1) return $"{(int)span.TotalHours}h ago";
        if (span.TotalMinutes > 1) return $"{(int)span.TotalMinutes}m ago";
        return "Just now";
    }

    private static string GetIconClass(string type)
    {
        return type.ToLower() switch
        {
            "like" => "bi bi-heart-fill",
            "comment" => "bi bi-chat-fill",
            "message" => "bi bi-envelope-fill",
            "contest" => "bi bi-trophy-fill",
            "job" => "bi bi-briefcase-fill",
            _ => "bi bi-info-circle-fill"
        };
    }

    private static string GetBadgeColor(string type)
    {
        return type.ToLower() switch
        {
            "like" => "danger",
            "comment" => "primary",
            "message" => "info",
            "contest" => "warning",
            "job" => "success",
            _ => "secondary"
        };
    }
}

/// <summary>
/// ViewModel for the notifications page
/// </summary>
public class NotificationsPageViewModel
{
    public List<NotificationViewModel> Notifications { get; set; } = new();
    public int UnreadCount { get; set; }
    public int TotalCount { get; set; }

    // Filter options
    public string? TypeFilter { get; set; }
    public bool ShowUnreadOnly { get; set; } = false;

    public List<string> Types { get; set; } = new()
    {
        "all", "like", "comment", "message", "contest", "job"
    };
}
