namespace NewTalent.Data.Entities;

public class Award
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string Title { get; set; } = string.Empty;
    public string? Organization { get; set; }
    public string? Description { get; set; }
    public DateTime DateReceived { get; set; }
    public string? CertificateUrl { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
