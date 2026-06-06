namespace NewTalent.Data.Entities;

public class Comment
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // FK
    public int VideoId { get; set; }
    public Video Video { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}