namespace NewTalent.Models;

public class MentorCollabsViewModel
{
    public List<Data.Entities.MentorshipProfile> Mentors { get; set; } = new();
    public List<Data.Entities.Collaboration> Collaborations { get; set; } = new();
    public string? CategoryFilter { get; set; }
    public string? ExpertiseFilter { get; set; }
    public string? SearchQuery { get; set; }
    public string? ProjectTypeFilter { get; set; }
    public bool IsLoggedIn { get; set; }
}
