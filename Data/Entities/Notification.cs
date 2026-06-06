namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing a user notification
/// </summary>
public class Notification
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "info"; // info, like, comment, message, contest, job
    public string? Link { get; set; }

    public bool IsRead { get; set; } = false;
    public DateTime ReadAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
