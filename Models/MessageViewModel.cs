namespace NewTalent.Models;

/// <summary>
/// ViewModel for a message in a conversation
/// </summary>
public class MessageViewModel
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string SenderAvatar { get; set; } = string.Empty;
    public int ReceiverId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public string SentAgo => GetTimeAgo(SentAt);
    public bool IsFromCurrentUser { get; set; }

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
/// ViewModel for a conversation
/// </summary>
public class ConversationViewModel
{
    public int Id { get; set; }
    public int User1Id { get; set; }
    public string User1Name { get; set; } = string.Empty;
    public string User1Avatar { get; set; } = string.Empty;
    public int User2Id { get; set; }
    public string User2Name { get; set; } = string.Empty;
    public string User2Avatar { get; set; } = string.Empty;
    public DateTime LastMessageAt { get; set; } = DateTime.UtcNow;
    public string LastMessageAgo => GetTimeAgo(LastMessageAt);
    public string LastMessagePreview { get; set; } = string.Empty;
    public int UnreadCount { get; set; }
    public List<MessageViewModel> Messages { get; set; } = new();

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
/// ViewModel for the inbox page
/// </summary>
public class InboxViewModel
{
    public List<ConversationViewModel> Conversations { get; set; } = new();
    public int TotalUnread { get; set; }
}

/// <summary>
/// ViewModel for sending a message
/// </summary>
public class SendMessageViewModel
{
    public int ConversationId { get; set; }
    public int ReceiverId { get; set; }
    public string Content { get; set; } = string.Empty;
}
