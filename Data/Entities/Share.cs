namespace NewTalent.Data.Entities;

public class Share
{
    public int Id { get; set; }

    public int VideoId { get; set; }
    public Video Video { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime SharedAt { get; set; } = DateTime.UtcNow;
}