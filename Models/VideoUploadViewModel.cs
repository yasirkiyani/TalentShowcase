using System.ComponentModel.DataAnnotations;

namespace NewTalent.Models;

public class VideoUploadViewModel
{
    [Required(ErrorMessage = "Please select a video file")]
    [Display(Name = "Video File")]
    [AllowedExtensions(new string[] { ".mp4", ".avi", ".mov", ".wmv", ".webm", ".mkv", ".flv" }, ErrorMessage = "Only video files (MP4, AVI, MOV, WMV, WebM, MKV, FLV) are allowed.")]
    [MaxFileSize(500 * 1024 * 1024, ErrorMessage = "Maximum file size is 500MB")]
    public IFormFile? VideoFile { get; set; }

    [Required(ErrorMessage = "Video title is required")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    [Display(Name = "Video Title")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select a category")]
    [Display(Name = "Category")]
    public string Category { get; set; } = string.Empty;

    [Display(Name = "Skill Level")]
    public string SkillLevel { get; set; } = "beginner";

    [Display(Name = "Video Type")]
    public string VideoType { get; set; } = "performance"; // performance, tutorial, exhibition, other

    [Display(Name = "Tags")]
    public string? Tags { get; set; }

    [Display(Name = "Language")]
    public string Language { get; set; } = "english";

    [Display(Name = "Age Restriction")]
    public string AgeRestriction { get; set; } = "all";

    [Display(Name = "Privacy Setting")]
    public string Privacy { get; set; } = "public";

    [Display(Name = "Allow Comments")]
    public bool AllowComments { get; set; } = true;

    [Display(Name = "Allow Likes")]
    public bool AllowLikes { get; set; } = true;

    [Display(Name = "Allow Shares")]
    public bool AllowShares { get; set; } = true;

    [Display(Name = "Allow Downloads")]
    public bool AllowDownloads { get; set; } = false;

    // For saving as draft
    public bool IsDraft { get; set; } = false;

    // Available categories for dropdown
    public List<string> Categories { get; set; } = new()
    {
        "Music", "Dance", "Art", "Acting", "Coding", "Writing", "Comedy", "Sports", "Cooking", "Magic", "Other"
    };

    // Available video types
    public List<string> VideoTypes { get; set; } = new()
    {
        "performance", "tutorial", "exhibition", "behind_the_scenes", "other"
    };

    // Supported video formats
    public List<string> SupportedFormats { get; set; } = new()
    {
        "MP4", "AVI", "MOV", "WMV", "WebM", "MKV", "FLV"
    };
}

// Custom validation attributes
public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!_extensions.Contains(extension))
            {
                return new ValidationResult(GetErrorMessage());
            }
        }

        return ValidationResult.Success;
    }

    protected string GetErrorMessage()
    {
        return $"Only {string.Join(", ", _extensions)} files are allowed.";
    }
}

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly long _maxFileSize;

    public MaxFileSizeAttribute(long maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            if (file.Length > _maxFileSize)
            {
                return new ValidationResult(GetErrorMessage());
            }
        }

        return ValidationResult.Success;
    }

    protected string GetErrorMessage()
    {
        var maxSizeInMB = _maxFileSize / (1024 * 1024);
        return $"Maximum file size is {maxSizeInMB}MB.";
    }
}
