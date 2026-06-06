namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing a conversation between two users
/// </summary>
public class Conversation
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastMessageAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int User1Id { get; set; }
    public User User1 { get; set; } = null!;

    public int User2Id { get; set; }
    public User User2 { get; set; } = null!;

    // Navigation
    public List<Message> Messages { get; set; } = new();
}
