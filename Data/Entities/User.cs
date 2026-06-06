using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NewTalent.Data.Entities;

public class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string? ProfilePicture { get; set; }
    public string? Bio { get; set; }

    public string SelectedTalents { get; set; } = string.Empty;

    public string? Location { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }

    // Privacy Settings
    public bool PublicProfile { get; set; } = true;
    public bool ShowEmail { get; set; } = false;
    public bool AllowMessages { get; set; } = true;
    public bool ShowStats { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public List<Video> Videos { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<Like> Likes { get; set; } = new();
    public List<Share> Shares { get; set; } = new();
    public List<MentorshipSession> MentorshipSessionsAsMentor { get; set; } = new();
    public List<MentorshipSession> MentorshipSessionsAsMentee { get; set; } = new();
    public List<Achievement> Achievements { get; set; } = new();
    public List<Award> Awards { get; set; } = new();
    public List<Certification> Certifications { get; set; } = new();
}