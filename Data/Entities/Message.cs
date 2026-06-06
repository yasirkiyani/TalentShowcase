namespace NewTalent.Data.Entities;

/// <summary>
/// Entity representing a message between users
/// </summary>
public class Message
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public bool IsRead { get; set; } = false;
    public DateTime ReadAt { get; set; }

    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;

    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;

    public int ReceiverId { get; set; }
    public User Receiver { get; set; } = null!;
}
