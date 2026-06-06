namespace NewTalent.Data.Entities;

public class Certification
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string Title { get; set; } = string.Empty;
    public string? IssuingOrganization { get; set; }
    public string? CredentialId { get; set; }
    public string? CredentialUrl { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? Description { get; set; }
    public string? CertificateUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
