using System.ComponentModel.DataAnnotations;

namespace NewTalent.Models;

public class CommunityViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Community name is required")]
    [Display(Name = "Community Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select a category")]
    [Display(Name = "Category")]
    public string Category { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;
    public int MembersCount { get; set; }
    public string Status { get; set; } = "Active";
    public int PostsToday { get; set; }
    public string Privacy { get; set; } = "public";
    public bool AllowPosts { get; set; } = true;
    public bool AllowEvents { get; set; } = true;
    public string? Rules { get; set; }

    // Current user state
    public bool IsJoined { get; set; } = false;
    public bool IsAdmin { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class CommunitiesPageViewModel
{
    public List<CommunityViewModel> MyCommunities { get; set; } = new();
    public List<CommunityViewModel> AllCommunities { get; set; } = new();

    // For creating a new community
    public CommunityViewModel NewCommunity { get; set; } = new();

    // Filters
    public string? CategoryFilter { get; set; }
    public string SortBy { get; set; } = "popular";

    public List<string> Categories { get; set; } = new()
    {
        "Music", "Dance", "Art", "Acting", "Coding", "Writing", "Comedy", "Sports", "Other"
    };
}

public class CommunityPostViewModel
{
    public int Id { get; set; }
    public int CommunityId { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorAvatar { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? MediaUrl { get; set; }
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
    public bool IsLiked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<CommentViewModel> Comments { get; set; } = new();
}

public class CommunityViewViewModel
{
    public CommunityViewModel Community { get; set; } = new();
    public List<CommunityPostViewModel> Posts { get; set; } = new();
    public List<ProfileViewModel> Members { get; set; } = new();
    public List<VideoViewModel> FeaturedVideos { get; set; } = new();

    // For new post submission
    public string NewPostContent { get; set; } = string.Empty;
}

public class CommunityOptionsViewModel
{
    public CommunityViewModel Community { get; set; } = new();

    // Notification settings
    public bool NotifyNewPosts { get; set; } = true;
    public bool NotifyNewMembers { get; set; } = false;
    public bool NotifyEvents { get; set; } = true;

    // Member role
    public string MemberRole { get; set; } = "member"; // member, moderator, admin
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public int ContributionCount { get; set; }
}
