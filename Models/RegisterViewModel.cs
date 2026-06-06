using System.ComponentModel.DataAnnotations;

namespace NewTalent.Models;

public class RegisterViewModel
{
    // Step 1: Basic Information
    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username is required")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please confirm your password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // Step 2: Profile Information
    [Display(Name = "Profile Picture")]
    public IFormFile? ProfilePicture { get; set; }

    [Display(Name = "Bio")]
    [MaxLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
    public string? Bio { get; set; }

    [Display(Name = "Selected Talents")]
    public List<string> SelectedTalents { get; set; } = new();

    [Display(Name = "Location")]
    public string? Location { get; set; }

    [Display(Name = "Website")]
    [Url(ErrorMessage = "Enter a valid URL")]
    public string? Website { get; set; }

    // Step 3: Preferences & Agreement
    [Display(Name = "Skill Level")]
    public string SkillLevel { get; set; } = "beginner";

    [Display(Name = "Email Notifications")]
    public bool EmailNotifications { get; set; } = true;

    [Display(Name = "Newsletter")]
    public bool Newsletter { get; set; } = true;

    [Display(Name = "Opportunity Alerts")]
    public bool OpportunityAlerts { get; set; } = true;

    [Display(Name = "Public Profile")]
    public bool PublicProfile { get; set; } = true;

    [Display(Name = "Show Email on Profile")]
    public bool ShowEmail { get; set; } = false;

    [Required(ErrorMessage = "You must agree to the terms and conditions")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the terms")]
    [Display(Name = "Agree to Terms")]
    public bool AgreeToTerms { get; set; }

    // Available talent options for checkboxes
    public List<string> AvailableTalents { get; set; } = new()
    {
        "Music", "Dance", "Art", "Acting", "Coding", "Writing", "Comedy", "Sports", "Other"
    };
}
