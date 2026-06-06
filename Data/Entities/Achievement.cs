namespace NewTalent.Data.Entities;

public class Achievement
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateAwarded { get; set; }
    public string? IconClass { get; set; } = "bi bi-trophy-fill";
    public string? BadgeColor { get; set; } = "text-warning";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
