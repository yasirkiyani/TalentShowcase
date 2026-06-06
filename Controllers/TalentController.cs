using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewTalent.Data;
using NewTalent.Data.Entities;
using NewTalent.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TalentShowcasing.Models;
using System.Collections.Generic;

namespace NewTalent.Controllers;

public class TalentController : BaseController
{
    private readonly ILogger<TalentController> _logger;
    private readonly NewTalentDbContext _context;

    public TalentController(ILogger<TalentController> logger, NewTalentDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    // Helper method to get current user
    private User? GetCurrentUser()
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
            return null;

        return _context.Users.FirstOrDefault(x => x.Email == userEmail);
    }

    // Allowed pages for guest users
    private static readonly string[] GuestAllowedPages = { "Index", "Explore", "Communities", "CommunityOptions", "CommunityView", "Login", "Register" };

    // Check if current action is allowed for guests
    private bool IsGuestAllowed(string actionName)
    {
        return GuestAllowedPages.Contains(actionName);
    }


   

    // ================= HOME PAGES =================

    public IActionResult Index()
    {
        var model = new IndexViewModel();

        // Load dynamic data from database
        model.TotalArtists = _context.Users.Count();
        model.TotalVideos = _context.Videos.Count();
        model.TotalCommunities = 5; // Placeholder for communities count

        var currentUserId = GetCurrentUserId();

        // Load all videos in a single feed (Instagram-style)
        var allVideos = _context.Videos
            .Include(v => v.User)
            .Include(v => v.Likes).ThenInclude(l => l.User)
            .Include(v => v.Comments).ThenInclude(c => c.User)
            .Include(v => v.Shares)
            .OrderByDescending(v => v.UploadedAt)
            .Select(v => new VideoViewModel
            {
                Id = v.Id,
                Title = v.Title,
                Description = v.Description,
                VideoUrl = v.VideoUrl,
                ThumbnailUrl = string.IsNullOrEmpty(v.ThumbnailUrl) ? "" : v.ThumbnailUrl,
                Category = v.Category,
                SkillLevel = v.SkillLevel,
                ViewsCount = v.ViewsCount,
                LikesCount = v.Likes.Count,
                CommentsCount = v.Comments.Count,
                SharesCount = v.Shares.Count,
                ArtistId = v.User.Id,
                ArtistName = v.User.Username,
                ArtistProfilePicture = string.IsNullOrEmpty(v.User.ProfilePicture) ? "" : v.User.ProfilePicture,
                UploadedAt = v.UploadedAt,
                Tags = string.IsNullOrEmpty(v.Tags) ? "" : v.Tags,
                IsLikedByCurrentUser = currentUserId.HasValue && v.Likes.Any(l => l.UserId == currentUserId.Value),
                Comments = v.Comments.OrderByDescending(c => c.CreatedAt).Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserName = c.User.Username,
                    UserAvatar = string.IsNullOrEmpty(c.User.ProfilePicture) ? "" : c.User.ProfilePicture,
                    CreatedAt = c.CreatedAt,
                    LikesCount = 0
                }).ToList()
            })
            .ToList();

        model.TrendingVideos = allVideos;
        model.FeaturedVideos = allVideos.Take(6).ToList();
        model.NewestVideos = allVideos.Take(6).ToList();

        // Load featured artists (users with most videos)
        var featuredArtists = _context.Users
            .OrderByDescending(u => u.Videos.Count)
            .Take(4)
            .Select(u => new ProfileViewModel
            {
                FullName = $"{u.FirstName} {u.LastName}",
                Username = u.Username,
                ProfilePicture = u.ProfilePicture ?? "/images/default.png",
                Bio = u.Bio,
                Location = u.Location
            })
            .ToList();

        model.FeaturedArtists = featuredArtists;

        // Load categories with video counts
        var categories = new[] { "Music", "Dance", "Art", "Acting", "Coding", "Writing", "Comedy", "Sports" };
        var categoryIcons = new Dictionary<string, string>
        {
            { "Music", "bi-music-note-beamed" },
            { "Dance", "bi-person-dancing" },
            { "Art", "bi-palette" },
            { "Acting", "bi-theater-masks" },
            { "Coding", "bi-code-slash" },
            { "Writing", "bi-pencil-square" },
            { "Comedy", "bi-emoji-laughing" },
            { "Sports", "bi-trophy" }
        };

        var categoryList = categories.Select(cat => new CategorySummary
        {
            Name = cat,
            IconClass = categoryIcons.TryGetValue(cat, out var icon) ? icon : "bi-star",
            VideoCount = _context.Videos.Count(v => v.Category == cat),
            ColorClass = GetCategoryColorClass(cat)
        }).ToList();

        model.Categories = categoryList;

        model.IsLoggedIn = IsUserLoggedIn();
        model.CurrentUserName = HttpContext.Session.GetString("UserName");

        return View(model);
    }

    private static string GetCategoryColorClass(string category)
    {
        return category switch
        {
            "Music" => "text-primary",
            "Dance" => "text-success",
            "Art" => "text-warning",
            "Acting" => "text-info",
            "Coding" => "text-secondary",
            "Writing" => "text-dark",
            "Comedy" => "text-danger",
            "Sports" => "text-primary",
            _ => "text-muted"
        };
    }

    public IActionResult Explore(string? category, string? skillLevel, string? videoType, string? location, string? quality, string sortBy, string? dateRange, string? search, string? tags)
    {
        var model = new ExploreViewModel
        {
            CategoryFilter = category,
            SkillLevelFilter = skillLevel,
            VideoTypeFilter = videoType,
            LocationFilter = location,
            QualityFilter = quality,
            SortBy = sortBy ?? "trending",
            DateRange = dateRange,
            SearchQuery = search,
            TagsFilter = tags
        };

        var currentUserId = GetCurrentUserId();

        // Build query
        var videosQuery = _context.Videos
            .Include(v => v.User)
            .Include(v => v.Likes).ThenInclude(l => l.User)
            .Include(v => v.Comments).ThenInclude(c => c.User)
            .Include(v => v.Shares)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(category))
        {
            videosQuery = videosQuery.Where(v => v.Category == category);
        }

        if (!string.IsNullOrEmpty(skillLevel))
        {
            videosQuery = videosQuery.Where(v => v.SkillLevel == skillLevel);
        }

        if (!string.IsNullOrEmpty(videoType))
        {
            videosQuery = videosQuery.Where(v => v.VideoType == videoType);
        }

        if (!string.IsNullOrEmpty(location))
        {
            videosQuery = videosQuery.Where(v => v.User.Location != null && v.User.Location.Contains(location));
        }

        if (!string.IsNullOrEmpty(quality))
        {
            videosQuery = videosQuery.Where(v => v.Quality == quality);
        }

        if (!string.IsNullOrEmpty(search))
        {
            videosQuery = videosQuery.Where(v =>
                v.Title.Contains(search) ||
                v.Description.Contains(search) ||
                v.User.Username.Contains(search) ||
                (v.Tags != null && v.Tags.Contains(search)));
        }

        if (!string.IsNullOrEmpty(tags))
        {
            var tagList = tags.Split(',').Select(t => t.Trim()).ToList();
            videosQuery = videosQuery.Where(v =>
                tagList.Any(tag => v.Tags != null && v.Tags.Contains(tag)));
        }

        // Apply date range filter
        if (!string.IsNullOrEmpty(dateRange))
        {
            var now = DateTime.UtcNow;
            videosQuery = dateRange switch
            {
                "today" => videosQuery.Where(v => v.UploadedAt >= now.Date),
                "week" => videosQuery.Where(v => v.UploadedAt >= now.AddDays(-7)),
                "month" => videosQuery.Where(v => v.UploadedAt >= now.AddMonths(-1)),
                "year" => videosQuery.Where(v => v.UploadedAt >= now.AddYears(-1)),
                _ => videosQuery
            };
        }

        // Apply sorting
        videosQuery = sortBy switch
        {
            "recent" => videosQuery.OrderByDescending(v => v.UploadedAt),
            "popular" => videosQuery.OrderByDescending(v => v.Likes.Count + v.Comments.Count),
            "liked" => videosQuery.OrderByDescending(v => v.Likes.Count),
            "viewed" => videosQuery.OrderByDescending(v => v.ViewsCount),
            "recommended" => GetRecommendedVideosQuery(videosQuery, currentUserId),
            _ => videosQuery.OrderByDescending(v => v.ViewsCount) // trending (default)
        };

        // Get total count
        model.TotalResults = videosQuery.Count();

        // Apply pagination
        var videos = videosQuery
            .Skip((model.CurrentPage - 1) * model.PageSize)
            .Take(model.PageSize)
            .Select(v => new VideoViewModel
            {
                Id = v.Id,
                Title = v.Title,
                Description = v.Description,
                VideoUrl = v.VideoUrl,
                ThumbnailUrl = string.IsNullOrEmpty(v.ThumbnailUrl) ? "" : v.ThumbnailUrl,
                Category = v.Category,
                SkillLevel = v.SkillLevel,
                ViewsCount = v.ViewsCount,
                LikesCount = v.Likes.Count,
                CommentsCount = v.Comments.Count,
                SharesCount = v.Shares.Count,
                ArtistId = v.User.Id,
                ArtistName = v.User.Username,
                ArtistProfilePicture = string.IsNullOrEmpty(v.User.ProfilePicture) ? "" : v.User.ProfilePicture,
                UploadedAt = v.UploadedAt,
                Tags = string.IsNullOrEmpty(v.Tags) ? "" : v.Tags,
                IsLikedByCurrentUser = currentUserId.HasValue && v.Likes.Any(l => l.UserId == currentUserId.Value),
                Comments = v.Comments.OrderByDescending(c => c.CreatedAt).Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserName = c.User.Username,
                    UserAvatar = string.IsNullOrEmpty(c.User.ProfilePicture) ? "" : c.User.ProfilePicture,
                    CreatedAt = c.CreatedAt,
                    LikesCount = 0
                }).ToList()
            })
            .ToList();

        model.Videos = videos;

        // Load personalized recommendations if user is logged in
        if (currentUserId.HasValue && model.ShowRecommendations)
        {
            model.RecommendedVideos = GetPersonalizedRecommendations(currentUserId.Value, 6);
        }

        return View(model);
    }

    // ================= LOAD MORE VIDEOS =================
    [HttpGet]
    public IActionResult LoadMoreVideos(int page, string? category, string? skillLevel, string? videoType, string? location, string? quality, string sortBy, string? dateRange, string? search, string? tags)
    {
        var currentUserId = GetCurrentUserId();
        var pageSize = 6;

        // Build query
        var videosQuery = _context.Videos
            .Include(v => v.User)
            .Include(v => v.Likes)
            .Include(v => v.Comments)
            .Include(v => v.Shares)
            .Include(v => v.Ratings)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(category))
        {
            videosQuery = videosQuery.Where(v => v.Category == category);
        }

        if (!string.IsNullOrEmpty(skillLevel))
        {
            videosQuery = videosQuery.Where(v => v.SkillLevel == skillLevel);
        }

        if (!string.IsNullOrEmpty(videoType))
        {
            videosQuery = videosQuery.Where(v => v.VideoType == videoType);
        }

        if (!string.IsNullOrEmpty(location))
        {
            videosQuery = videosQuery.Where(v => v.User.Location != null && v.User.Location.Contains(location));
        }

        if (!string.IsNullOrEmpty(quality))
        {
            videosQuery = videosQuery.Where(v => v.Quality == quality);
        }

        if (!string.IsNullOrEmpty(search))
        {
            videosQuery = videosQuery.Where(v =>
                v.Title.Contains(search) ||
                v.Description.Contains(search) ||
                v.User.Username.Contains(search) ||
                (v.Tags != null && v.Tags.Contains(search)));
        }

        if (!string.IsNullOrEmpty(tags))
        {
            var tagList = tags.Split(',').Select(t => t.Trim()).ToList();
            videosQuery = videosQuery.Where(v =>
                tagList.Any(tag => v.Tags != null && v.Tags.Contains(tag)));
        }

        // Apply date range filter
        if (!string.IsNullOrEmpty(dateRange))
        {
            var now = DateTime.UtcNow;
            videosQuery = dateRange switch
            {
                "today" => videosQuery.Where(v => v.UploadedAt >= now.Date),
                "week" => videosQuery.Where(v => v.UploadedAt >= now.AddDays(-7)),
                "month" => videosQuery.Where(v => v.UploadedAt >= now.AddMonths(-1)),
                "year" => videosQuery.Where(v => v.UploadedAt >= now.AddYears(-1)),
                _ => videosQuery
            };
        }

        // Apply sorting
        videosQuery = sortBy switch
        {
            "recent" => videosQuery.OrderByDescending(v => v.UploadedAt),
            "popular" => videosQuery.OrderByDescending(v => v.Likes.Count + v.Comments.Count),
            "liked" => videosQuery.OrderByDescending(v => v.Likes.Count),
            "viewed" => videosQuery.OrderByDescending(v => v.ViewsCount),
            "recommended" => GetRecommendedVideosQuery(videosQuery, currentUserId),
            _ => videosQuery.OrderByDescending(v => v.ViewsCount)
        };

        // Apply pagination
        var videos = videosQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(v => new
            {
                v.Id,
                v.Title,
                v.Description,
                v.VideoUrl,
                v.ThumbnailUrl,
                v.Category,
                v.SkillLevel,
                v.ViewsCount,
                LikesCount = v.Likes.Count,
                CommentsCount = v.Comments.Count,
                SharesCount = v.Shares.Count,
                v.Duration,
                v.UploadedAt,
                ArtistId = v.User.Id,
                ArtistName = v.User.Username,
                ArtistProfilePicture = string.IsNullOrEmpty(v.User.ProfilePicture) ? "" : v.User.ProfilePicture,
                IsLikedByCurrentUser = currentUserId.HasValue && v.Likes.Any(l => l.UserId == currentUserId.Value),
                IsSavedByCurrentUser = false,
                AverageRating = v.Ratings.Any() ? v.Ratings.Average(r => r.Stars) : 0,
                TotalRatings = v.Ratings.Count
            })
            .ToList();

        return Json(new { success = true, videos });
    }

    // Helper method to get recommended videos query
    private IQueryable<Video> GetRecommendedVideosQuery(IQueryable<Video> baseQuery, int? currentUserId)
    {
        if (!currentUserId.HasValue)
        {
            return baseQuery.OrderByDescending(v => v.ViewsCount);
        }

        // Get user's preferences
        var userPreference = _context.UserPreferences.FirstOrDefault(up => up.UserId == currentUserId.Value);
        if (userPreference == null)
        {
            return baseQuery.OrderByDescending(v => v.ViewsCount);
        }

        // Apply preference-based filtering
        var query = baseQuery;

        if (!string.IsNullOrEmpty(userPreference.PreferredCategories))
        {
            var categories = userPreference.PreferredCategories.Split(',').Select(c => c.Trim()).ToList();
            query = query.Where(v => categories.Contains(v.Category));
        }

        if (!string.IsNullOrEmpty(userPreference.PreferredSkillLevels))
        {
            var skillLevels = userPreference.PreferredSkillLevels.Split(',').Select(s => s.Trim()).ToList();
            query = query.Where(v => skillLevels.Contains(v.SkillLevel));
        }

        if (!string.IsNullOrEmpty(userPreference.PreferredVideoTypes))
        {
            var videoTypes = userPreference.PreferredVideoTypes.Split(',').Select(t => t.Trim()).ToList();
            query = query.Where(v => videoTypes.Contains(v.VideoType));
        }

        return query.OrderByDescending(v => v.ViewsCount);
    }

    // Helper method to get personalized recommendations
    private List<VideoViewModel> GetPersonalizedRecommendations(int userId, int count)
    {
        var userPreference = _context.UserPreferences.FirstOrDefault(up => up.UserId == userId);
        
        if (userPreference == null || !userPreference.EnableRecommendations)
        {
            // Return trending videos if no preferences
            return _context.Videos
                .Include(v => v.User)
                .Include(v => v.Likes)
                .OrderByDescending(v => v.ViewsCount)
                .Take(count)
                .Select(v => new VideoViewModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    Description = v.Description,
                    VideoUrl = v.VideoUrl,
                    ThumbnailUrl = string.IsNullOrEmpty(v.ThumbnailUrl) ? "" : v.ThumbnailUrl,
                    Category = v.Category,
                    SkillLevel = v.SkillLevel,
                    ViewsCount = v.ViewsCount,
                    LikesCount = v.Likes.Count,
                    CommentsCount = 0,
                    SharesCount = 0,
                    ArtistId = v.User.Id,
                    ArtistName = v.User.Username,
                    ArtistProfilePicture = string.IsNullOrEmpty(v.User.ProfilePicture) ? "" : v.User.ProfilePicture,
                    UploadedAt = v.UploadedAt,
                    Tags = string.IsNullOrEmpty(v.Tags) ? "" : v.Tags,
                    IsLikedByCurrentUser = false
                })
                .ToList();
        }

        // Get videos based on user preferences
        var query = _context.Videos
            .Include(v => v.User)
            .Include(v => v.Likes)
            .AsQueryable();

        // Apply preference filters
        if (!string.IsNullOrEmpty(userPreference.PreferredCategories))
        {
            var categories = userPreference.PreferredCategories.Split(',').Select(c => c.Trim()).ToList();
            query = query.Where(v => categories.Contains(v.Category));
        }

        if (!string.IsNullOrEmpty(userPreference.PreferredSkillLevels))
        {
            var skillLevels = userPreference.PreferredSkillLevels.Split(',').Select(s => s.Trim()).ToList();
            query = query.Where(v => skillLevels.Contains(v.SkillLevel));
        }

        if (!string.IsNullOrEmpty(userPreference.PreferredVideoTypes))
        {
            var videoTypes = userPreference.PreferredVideoTypes.Split(',').Select(t => t.Trim()).ToList();
            query = query.Where(v => videoTypes.Contains(v.VideoType));
        }

        return query
            .OrderByDescending(v => v.ViewsCount)
            .Take(count)
            .Select(v => new VideoViewModel
            {
                Id = v.Id,
                Title = v.Title,
                Description = v.Description,
                VideoUrl = v.VideoUrl,
                ThumbnailUrl = string.IsNullOrEmpty(v.ThumbnailUrl) ? "" : v.ThumbnailUrl,
                Category = v.Category,
                SkillLevel = v.SkillLevel,
                ViewsCount = v.ViewsCount,
                LikesCount = v.Likes.Count,
                CommentsCount = 0,
                SharesCount = 0,
                ArtistId = v.User.Id,
                ArtistName = v.User.Username,
                ArtistProfilePicture = string.IsNullOrEmpty(v.User.ProfilePicture) ? "" : v.User.ProfilePicture,
                UploadedAt = v.UploadedAt,
                Tags = string.IsNullOrEmpty(v.Tags) ? "" : v.Tags,
                IsLikedByCurrentUser = v.Likes.Any(l => l.UserId == userId)
            })
            .ToList();
    }

    public IActionResult Communities()
    {
        var currentUserId = GetCurrentUserId();
        var model = new CommunitiesPageViewModel();

        // Load user's communities
        if (currentUserId.HasValue)
        {
            var myCommunities = _context.CommunityMembers
                .Where(cm => cm.UserId == currentUserId.Value)
                .Include(cm => cm.Community)
                .Select(cm => new CommunityViewModel
                {
                    Id = cm.Community.Id,
                    Name = cm.Community.Name,
                    Description = cm.Community.Description,
                    Category = cm.Community.Category,
                    ImageUrl = cm.Community.ImageUrl,
                    MembersCount = cm.Community.MembersCount,
                    Status = cm.Community.Status,
                    PostsToday = cm.Community.PostsToday,
                    Privacy = cm.Community.Privacy,
                    AllowPosts = cm.Community.AllowPosts,
                    AllowEvents = cm.Community.AllowEvents,
                    Rules = cm.Community.Rules,
                    IsJoined = true,
                    IsAdmin = cm.Role == "admin",
                    CreatedAt = cm.Community.CreatedAt
                })
                .ToList();

            model.MyCommunities = myCommunities;
        }

        // Load all communities
        var allCommunities = _context.Communities
            .Include(c => c.Members)
            .OrderByDescending(c => c.MembersCount)
            .Select(c => new CommunityViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Category = c.Category,
                ImageUrl = c.ImageUrl,
                MembersCount = c.MembersCount,
                Status = c.Status,
                PostsToday = c.PostsToday,
                Privacy = c.Privacy,
                AllowPosts = c.AllowPosts,
                AllowEvents = c.AllowEvents,
                Rules = c.Rules,
                IsJoined = currentUserId.HasValue && c.Members.Any(m => m.UserId == currentUserId.Value),
                CreatedAt = c.CreatedAt
            })
            .ToList();

        model.AllCommunities = allCommunities;

        return View(model);
    }

    public IActionResult CommunityOptions() => View();

    public IActionResult CommunityView() => View();

    // ================= JOIN COMMUNITY =================
    [HttpPost]
    public async Task<IActionResult> JoinCommunity(int communityId)
    {
        if (!IsUserLoggedIn())
            return Json(new { success = false, message = "Please login first" });

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return Json(new { success = false, message = "User not found" });

        var existingMember = await _context.CommunityMembers
            .FirstOrDefaultAsync(cm => cm.CommunityId == communityId && cm.UserId == currentUserId.Value);

        if (existingMember != null)
            return Json(new { success = false, message = "Already a member" });

        var member = new CommunityMember
        {
            CommunityId = communityId,
            UserId = currentUserId.Value,
            Role = "member",
            JoinedAt = DateTime.UtcNow,
            ContributionCount = 0
        };

        _context.CommunityMembers.Add(member);

        // Update community members count
        var community = await _context.Communities.FindAsync(communityId);
        if (community != null)
            community.MembersCount++;

        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Successfully joined community" });
    }

    // ================= CREATE COMMUNITY =================
    [HttpPost]
    public async Task<IActionResult> CreateCommunity(string Name, string Description, string Category, string Privacy, bool AllowPosts, bool AllowEvents, string Rules, IFormFile? ImageFile)
    {
        if (!IsUserLoggedIn())
            return Json(new { success = false, message = "Please login first" });

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return Json(new { success = false, message = "User not found" });

        string imageUrl = string.Empty;

        if (ImageFile != null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/communities");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fullPath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            imageUrl = "/communities/" + fileName;
        }

        var community = new Community
        {
            Name = Name,
            Description = Description,
            Category = Category,
            ImageUrl = imageUrl,
            MembersCount = 1,
            Status = "Active",
            PostsToday = 0,
            Privacy = Privacy,
            AllowPosts = AllowPosts,
            AllowEvents = AllowEvents,
            Rules = Rules,
            CreatedAt = DateTime.UtcNow,
            CreatedById = currentUserId.Value
        };

        _context.Communities.Add(community);
        await _context.SaveChangesAsync();

        // Add creator as admin member
        var member = new CommunityMember
        {
            CommunityId = community.Id,
            UserId = currentUserId.Value,
            Role = "admin",
            JoinedAt = DateTime.UtcNow,
            ContributionCount = 0
        };

        _context.CommunityMembers.Add(member);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Community created successfully" });
    }

    // ================= CONNECT USER =================
    [HttpPost]
    public async Task<IActionResult> ConnectUser(int userId)
    {
        if (!IsUserLoggedIn())
            return Json(new { success = false, message = "Please login first" });

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return Json(new { success = false, message = "User not found" });

        // Placeholder - would need ConnectionRequest entity
        // For now, just return success
        return Json(new { success = true, message = "Connection request sent!" });
    }

    // ================= CREATE CONTEST =================
    [HttpPost]
    public async Task<IActionResult> CreateContest(string Title, string Description, string Category, string PrizeAmount, int MaxParticipants, string SkillLevel, DateTime StartDate, DateTime EndDate, IFormFile? ImageFile)
    {
        if (!IsUserLoggedIn())
            return Json(new { success = false, message = "Please login first" });

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return Json(new { success = false, message = "User not found" });

        string imageUrl = string.Empty;

        if (ImageFile != null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/contests");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fullPath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            imageUrl = "/contests/" + fileName;
        }

        var contest = new Contest
        {
            Title = Title,
            Description = Description,
            Category = Category,
            ImageUrl = imageUrl,
            PrizeAmount = PrizeAmount,
            StartDate = StartDate,
            EndDate = EndDate,
            MaxParticipants = MaxParticipants,
            CurrentParticipants = 0,
            SkillLevel = SkillLevel,
            Status = "active",
            IsFeatured = false,
            CreatedAt = DateTime.UtcNow,
            CreatedById = currentUserId.Value
        };

        _context.Contests.Add(contest);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Contest created successfully", contestId = contest.Id });
    }

    // ================= JOIN CONTEST =================
    [HttpPost]
    public async Task<IActionResult> JoinContest(int contestId, string? Title = null, string? Description = null, string? MediaUrl = null)
    {
        if (!IsUserLoggedIn())
            return Json(new { success = false, message = "Please login first" });

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return Json(new { success = false, message = "User not found" });

        var contest = await _context.Contests.FindAsync(contestId);
        if (contest == null)
            return Json(new { success = false, message = "Contest not found" });

        if (contest.Status != "active")
            return Json(new { success = false, message = "Contest is not active" });

        if (contest.CurrentParticipants >= contest.MaxParticipants)
            return Json(new { success = false, message = "Contest is full" });

        var existingEntry = await _context.ContestEntries
            .FirstOrDefaultAsync(ce => ce.ContestId == contestId && ce.UserId == currentUserId.Value);

        if (existingEntry != null)
            return Json(new { success = false, message = "Already participated in this contest" });

        var entry = new ContestEntry
        {
            ContestId = contestId,
            UserId = currentUserId.Value,
            Title = Title ?? "Entry",
            Description = Description ?? "Joined contest",
            MediaUrl = MediaUrl ?? "",
            SubmittedAt = DateTime.UtcNow,
            Status = "submitted"
        };

        _context.ContestEntries.Add(entry);
        contest.CurrentParticipants++;
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Contest entry submitted successfully" });
    }

    // ================= CONTEST DETAILS =================
    [HttpGet]
    public async Task<IActionResult> ContestDetails(int id)
    {
        var contest = await _context.Contests
            .Include(c => c.CreatedBy)
            .Include(c => c.Entries)
            .ThenInclude(e => e.User)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contest == null)
            return Json(new { success = false, message = "Contest not found" });

        var participants = contest.Entries.Select(e => new
        {
            e.User.Username,
            e.User.ProfilePicture,
            e.Title,
            e.Description,
            e.SubmittedAt
        }).ToList();

        return Json(new
        {
            success = true,
            contest = new
            {
                contest.Id,
                contest.Title,
                contest.Description,
                contest.Category,
                contest.PrizeAmount,
                contest.MaxParticipants,
                contest.CurrentParticipants,
                contest.StartDate,
                contest.EndDate,
                contest.Status,
                contest.SkillLevel,
                contest.ImageUrl,
                CreatedBy = contest.CreatedBy?.Username ?? "Unknown"
            },
            participants
        });
    }

    // ================= CREATE MENTORSHIP SESSION =================
    [HttpPost]
    public async Task<IActionResult> CreateMentorshipSession(int MentorId, string Topic, DateTime ScheduledDate, string Description)
    {
        if (!IsUserLoggedIn())
            return Json(new { success = false, message = "Please login first" });

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return Json(new { success = false, message = "User not found" });

        var mentor = await _context.Users.FindAsync(MentorId);
        if (mentor == null)
            return Json(new { success = false, message = "Mentor not found" });

        var session = new MentorshipSession
        {
            MentorId = MentorId,
            MenteeId = currentUserId.Value,
            Topic = Topic,
            ScheduledDate = ScheduledDate,
            Description = Description,
            Status = "scheduled",
            CreatedAt = DateTime.UtcNow
        };

        _context.MentorshipSessions.Add(session);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Mentorship session created successfully", sessionId = session.Id });
    }

    // ================= GET COMMUNITY DETAILS =================
    [HttpGet]
    public async Task<IActionResult> GetCommunityDetails(int communityId)
    {
        var community = await _context.Communities
            .Include(c => c.CreatedBy)
            .Include(c => c.Members)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(c => c.Id == communityId);

        if (community == null)
            return Json(new { success = false, message = "Community not found" });

        var creator = new
        {
            community.CreatedBy.Id,
            community.CreatedBy.Username,
            community.CreatedBy.FirstName,
            community.CreatedBy.LastName,
            community.CreatedBy.ProfilePicture,
            community.CreatedBy.Bio,
            community.CreatedBy.SelectedTalents
        };

        var members = community.Members.Select(m => new
        {
            m.User.Id,
            m.User.Username,
            m.User.FirstName,
            m.User.LastName,
            m.User.ProfilePicture,
            m.User.Bio,
            m.User.SelectedTalents,
            m.Role,
            m.JoinedAt,
            m.ContributionCount
        }).ToList();

        return Json(new
        {
            success = true,
            community = new
            {
                community.Id,
                community.Name,
                community.Description,
                community.Category,
                community.ImageUrl,
                community.MembersCount,
                community.Status,
                community.Privacy,
                community.Rules,
                community.CreatedAt
            },
            creator,
            members,
            totalMembers = members.Count
        });
    }

    // ================= GET CONTEST DETAILS =================
    [HttpGet]
    public async Task<IActionResult> GetContestDetails(int contestId)
    {
        var contest = await _context.Contests
            .Include(c => c.CreatedBy)
            .Include(c => c.Entries)
                .ThenInclude(e => e.User)
            .FirstOrDefaultAsync(c => c.Id == contestId);

        if (contest == null)
            return Json(new { success = false, message = "Contest not found" });

        var creator = new
        {
            contest.CreatedBy.Id,
            contest.CreatedBy.Username,
            contest.CreatedBy.FirstName,
            contest.CreatedBy.LastName,
            contest.CreatedBy.ProfilePicture,
            contest.CreatedBy.Bio,
            contest.CreatedBy.SelectedTalents
        };

        var participants = contest.Entries.Select(e => new
        {
            EntryId = e.Id,
            UserId = e.User.Id,
            e.User.Username,
            e.User.FirstName,
            e.User.LastName,
            e.User.ProfilePicture,
            e.User.Bio,
            e.User.SelectedTalents,
            EntryTitle = e.Title,
            EntryDescription = e.Description,
            e.MediaUrl,
            e.Status,
            e.Rank,
            e.SubmittedAt,
            e.ReviewedAt
        }).ToList();

        return Json(new
        {
            success = true,
            contest = new
            {
                contest.Id,
                contest.Title,
                contest.Description,
                contest.Category,
                contest.ImageUrl,
                contest.PrizeAmount,
                contest.StartDate,
                contest.EndDate,
                contest.MaxParticipants,
                contest.CurrentParticipants,
                contest.SkillLevel,
                contest.Status,
                contest.IsFeatured,
                contest.CreatedAt
            },
            creator,
            participants,
            totalParticipants = participants.Count
        });
    }

    // ================= GET MENTOR DETAILS =================
    [HttpGet]
    public async Task<IActionResult> GetMentorDetails(int mentorId)
    {
        var mentor = await _context.Users
            .Include(u => u.MentorshipSessionsAsMentor)
                .ThenInclude(ms => ms.Mentee)
            .FirstOrDefaultAsync(u => u.Id == mentorId);

        if (mentor == null)
            return Json(new { success = false, message = "Mentor not found" });

        var sessions = mentor.MentorshipSessionsAsMentor
            .Where(ms => ms.Status != "cancelled")
            .Select(ms => new
            {
                ms.Id,
                ms.Topic,
                ms.ScheduledDate,
                ms.Status,
                ms.Description,
                mentee = new
                {
                    ms.Mentee.Id,
                    ms.Mentee.Username,
                    ms.Mentee.FirstName,
                    ms.Mentee.LastName,
                    ms.Mentee.ProfilePicture,
                    ms.Mentee.Bio,
                    ms.Mentee.SelectedTalents
                }
            }).ToList();

        return Json(new
        {
            success = true,
            mentor = new
            {
                mentor.Id,
                mentor.Username,
                mentor.FirstName,
                mentor.LastName,
                mentor.ProfilePicture,
                mentor.Bio,
                mentor.SelectedTalents,
                mentor.Location,
                mentor.Videos.Count
            },
            sessions,
            totalSessions = sessions.Count
        });
    }

    // ================= GET MENTEE DETAILS =================
    [HttpGet]
    public async Task<IActionResult> GetMenteeDetails(int menteeId)
    {
        var mentee = await _context.Users
            .Include(u => u.MentorshipSessionsAsMentee)
                .ThenInclude(ms => ms.Mentor)
            .FirstOrDefaultAsync(u => u.Id == menteeId);

        if (mentee == null)
            return Json(new { success = false, message = "Mentee not found" });

        var sessions = mentee.MentorshipSessionsAsMentee
            .Where(ms => ms.Status != "cancelled")
            .Select(ms => new
            {
                ms.Id,
                ms.Topic,
                ms.ScheduledDate,
                ms.Status,
                ms.Description,
                mentor = new
                {
                    ms.Mentor.Id,
                    ms.Mentor.Username,
                    ms.Mentor.FirstName,
                    ms.Mentor.LastName,
                    ms.Mentor.ProfilePicture,
                    ms.Mentor.Bio,
                    ms.Mentor.SelectedTalents
                }
            }).ToList();

        return Json(new
        {
            success = true,
            mentee = new
            {
                mentee.Id,
                mentee.Username,
                mentee.FirstName,
                mentee.LastName,
                mentee.ProfilePicture,
                mentee.Bio,
                mentee.SelectedTalents,
                mentee.Location,
                mentee.Videos.Count
            },
            sessions,
            totalSessions = sessions.Count
        });
    }

    [HttpGet]
    public IActionResult GetVideoComments(int videoId)
    {
        var comments = _context.Comments
            .Where(c => c.VideoId == videoId)
            .Include(c => c.User)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new
            {
                c.Id,
                c.Content,
                UserName = c.User.Username,
                UserAvatar = string.IsNullOrEmpty(c.User.ProfilePicture) ? "" : c.User.ProfilePicture,
                CreatedAgo = GetTimeAgo(c.CreatedAt)
            })
            .ToList();

        return Json(comments);
    }

    private static string GetTimeAgo(DateTime dateTime)
    {
        var span = DateTime.UtcNow - dateTime;
        if (span.TotalDays > 1) return $"{(int)span.TotalDays}d ago";
        if (span.TotalHours > 1) return $"{(int)span.TotalHours}h ago";
        if (span.TotalMinutes > 1) return $"{(int)span.TotalMinutes}m ago";
        return "Just now";
    }



    //Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // 👈 pura session khatam

        return RedirectToAction("Login", "Talent");
    }

    // ================= LOGIN =================

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _context.Users
            .FirstOrDefault(x => x.Email == model.Email && x.PasswordHash == model.Password);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("UserEmail", user.Email);
        HttpContext.Session.SetString("UserName", user.Username);
        HttpContext.Session.SetString("UserPic", user.ProfilePicture ?? "");

        return RedirectToAction("Profile");
    }

    // ================= REGISTER =================

    [HttpGet]
    public IActionResult Register()
    {
        var model = new RegisterViewModel
        {
            AvailableTalents = new List<string>()
        {
            "Music", "Dance", "Art", "Acting", "Coding", "Writing", "Comedy", "Sports", "Other"
        }
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AvailableTalents ??= new List<string>();
            return View(model);
        }

        string profilePath = null;

        // ================= PROFILE PICTURE SAVE =================
        if (model.ProfilePicture != null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(model.ProfilePicture.FileName);

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profilepics");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fullPath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await model.ProfilePicture.CopyToAsync(stream);
            }

            profilePath = "/profilepics/" + fileName;
        }

        // ================= USER SAVE =================
        var user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Username = model.Username,
            PasswordHash = model.Password,
            Bio = model.Bio,
            Location = model.Location,
            ProfilePicture = profilePath,
            SelectedTalents = string.Join(",", model.SelectedTalents ?? new List<string>()),
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // ❗ OPTIONAL: session yahan mat set karo (login pe set hota hai)

        return RedirectToAction("Login");
    }

    // ================= UPLOAD VIDEO =================

    [HttpGet]
    public IActionResult Upload()
    {
        if (!IsUserLoggedIn())
            return RedirectToAction("Login");

        var model = new VideoUploadViewModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(VideoUploadViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return View(model);
        }

        string videoPath = null;
        string videoFormat = string.Empty;
        long fileSize = 0;

        // ===== VIDEO SAVE =====
        if (model.VideoFile != null)
        {
            var fileExtension = Path.GetExtension(model.VideoFile.FileName).ToLower();
            var fileName = Guid.NewGuid() + fileExtension;
            fileSize = model.VideoFile.Length;

            // Determine video format from extension
            videoFormat = fileExtension.Replace(".", "").ToUpper();

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fullPath = Path.Combine(folderPath, fileName);

            try
            {
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.VideoFile.CopyToAsync(stream);
                }

                videoPath = "/videos/" + fileName;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error saving video file: " + ex.Message);
                return View(model);
            }
        }
        else
        {
            ModelState.AddModelError("VideoFile", "Please select a video file");
            return View(model);
        }

        // ===== SAVE TO DATABASE =====
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            ModelState.AddModelError("", "You must be logged in to upload videos");
            return View(model);
        }

        try
        {
            // Determine quality based on file size (simple heuristic)
            string quality = "SD";
            if (fileSize > 100 * 1024 * 1024) // > 100MB
                quality = "4K";
            else if (fileSize > 50 * 1024 * 1024) // > 50MB
                quality = "Full HD";
            else if (fileSize > 20 * 1024 * 1024) // > 20MB
                quality = "HD";

            var video = new Video
            {
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
                SkillLevel = model.SkillLevel,
                Tags = model.Tags,
                VideoUrl = videoPath,
                ThumbnailUrl = "",
                ViewsCount = 0,
                UploadedAt = DateTime.UtcNow,
                UserId = currentUserId.Value,
                VideoFormat = videoFormat,
                VideoType = model.VideoType,
                FileSize = fileSize,
                Duration = "0:00", // Will be updated by video processing service
                Quality = quality,
                IsProcessed = false,
                StreamingUrl = null,
                Privacy = model.Privacy,
                AllowComments = model.AllowComments,
                AllowLikes = model.AllowLikes,
                AllowShares = model.AllowShares,
                AllowDownloads = model.AllowDownloads
            };

            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            return RedirectToAction("Explore");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error saving video to database: " + ex.Message);
            return View(model);
        }
    }

    // ================= LIKE VIDEO =================
    [HttpPost]
    public async Task<IActionResult> LikeVideo(int videoId)
    {
        if (!IsUserLoggedIn())
        {
            return Json(new { success = false, message = "Please login or register first" });
        }

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return Json(new { success = false, message = "Please login or register first" });
        }

        var existingLike = await _context.Likes
            .FirstOrDefaultAsync(l => l.VideoId == videoId && l.UserId == currentUserId);

        if (existingLike != null)
        {
            // Unlike
            _context.Likes.Remove(existingLike);
            await _context.SaveChangesAsync();
            return Json(new { success = true, liked = false, message = "Video unliked" });
        }
        else
        {
            // Like
            var like = new Like
            {
                VideoId = videoId,
                UserId = currentUserId.Value,
                LikedAt = DateTime.UtcNow
            };
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
            return Json(new { success = true, liked = true, message = "Video liked" });
        }
    }

    // ================= COMMENT ON VIDEO =================
    [HttpPost]
    public async Task<IActionResult> CommentVideo(int videoId, string content)
    {
        if (!IsUserLoggedIn())
        {
            return Json(new { success = false, message = "Please login or register first" });
        }

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return Json(new { success = false, message = "Please login or register first" });
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            return Json(new { success = false, message = "Comment cannot be empty" });
        }

        var comment = new Comment
        {
            VideoId = videoId,
            UserId = currentUserId.Value,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Comment added" });
    }

    // ================= SHARE VIDEO =================
    [HttpPost]
    public async Task<IActionResult> ShareVideo(int videoId)
    {
        if (!IsUserLoggedIn())
        {
            return Json(new { success = false, message = "Please login or register first" });
        }

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return Json(new { success = false, message = "Please login or register first" });
        }

        var existingShare = await _context.Shares
            .FirstOrDefaultAsync(s => s.VideoId == videoId && s.UserId == currentUserId);

        if (existingShare == null)
        {
            var share = new Share
            {
                VideoId = videoId,
                UserId = currentUserId.Value,
                SharedAt = DateTime.UtcNow
            };
            _context.Shares.Add(share);
            await _context.SaveChangesAsync();
        }

        return Json(new { success = true, message = "Video shared" });
    }

    // ================= RATE VIDEO =================
    [HttpPost]
    public async Task<IActionResult> RateVideo(int videoId, int stars, string? review)
    {
        if (!IsUserLoggedIn())
        {
            return Json(new { success = false, message = "Please login or register first" });
        }

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return Json(new { success = false, message = "Please login or register first" });
        }

        if (stars < 1 || stars > 5)
        {
            return Json(new { success = false, message = "Rating must be between 1 and 5 stars" });
        }

        var existingRating = await _context.Ratings
            .FirstOrDefaultAsync(r => r.VideoId == videoId && r.UserId == currentUserId);

        if (existingRating != null)
        {
            // Update existing rating
            existingRating.Stars = stars;
            existingRating.Review = review;
            existingRating.RatedAt = DateTime.UtcNow;
        }
        else
        {
            // Add new rating
            var rating = new Rating
            {
                VideoId = videoId,
                UserId = currentUserId.Value,
                Stars = stars,
                Review = review,
                RatedAt = DateTime.UtcNow
            };
            _context.Ratings.Add(rating);
        }

        // Update video rating summary
        var video = await _context.Videos.FindAsync(videoId);
        if (video != null)
        {
            var ratings = await _context.Ratings.Where(r => r.VideoId == videoId).ToListAsync();
            video.TotalRatings = ratings.Count;
            video.AverageRating = ratings.Count > 0 ? ratings.Average(r => r.Stars) : 0;
        }

        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Rating submitted", averageRating = video?.AverageRating, totalRatings = video?.TotalRatings });
    }

    // ================= GET VIDEO RATINGS =================
    [HttpGet]
    public async Task<IActionResult> GetVideoRatings(int videoId)
    {
        var ratings = await _context.Ratings
            .Include(r => r.User)
            .Where(r => r.VideoId == videoId)
            .OrderByDescending(r => r.RatedAt)
            .Select(r => new
            {
                r.Id,
                r.Stars,
                r.Review,
                r.RatedAt,
                UserName = r.User.Username,
                UserAvatar = r.User.ProfilePicture
            })
            .ToListAsync();

        return Json(ratings);
    }

    // ================= UPDATE VIDEO PRIVACY =================
    [HttpPost]
    public async Task<IActionResult> UpdateVideoPrivacy(int videoId, string privacy, bool allowComments, bool allowLikes, bool allowShares, bool allowDownloads)
    {
        if (!IsUserLoggedIn())
        {
            return Json(new { success = false, message = "Please login first" });
        }

        var currentUserId = GetCurrentUserId();
        var video = await _context.Videos.FindAsync(videoId);

        if (video == null || video.UserId != currentUserId)
        {
            return Json(new { success = false, message = "Video not found or unauthorized" });
        }

        video.Privacy = privacy;
        video.AllowComments = allowComments;
        video.AllowLikes = allowLikes;
        video.AllowShares = allowShares;
        video.AllowDownloads = allowDownloads;

        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Privacy settings updated" });
    }

    // ================= ANALYTICS DASHBOARD =================
    public IActionResult Analytics()
    {
        if (!IsUserLoggedIn())
        {
            return RedirectToAction("Login");
        }

        var currentUserId = GetCurrentUserId();
        var user = _context.Users.Find(currentUserId);

        if (user == null)
        {
            return RedirectToAction("Login");
        }

        // Get global totals (all videos on platform)
        var allVideos = _context.Videos.ToList();
        var globalTotalViews = allVideos.Sum(v => v.ViewsCount);
        var globalTotalLikes = allVideos.Sum(v => v.Likes.Count);
        var globalTotalComments = allVideos.Sum(v => v.Comments.Count);
        var globalTotalVideos = allVideos.Count;

        // Get user analytics
        var userAnalytics = _context.UserAnalytics
            .FirstOrDefault(ua => ua.UserId == currentUserId);

        // Get video analytics
        var videoAnalytics = _context.VideoAnalytics
            .Include(va => va.Video)
            .Where(va => va.Video.UserId == currentUserId)
            .OrderByDescending(va => va.TotalViews)
            .ToList();

        // Get user's videos
        var videos = _context.Videos
            .Where(v => v.UserId == currentUserId)
            .ToList();

        // Calculate user-specific totals
        var userTotalViews = videos.Sum(v => v.ViewsCount);
        var userTotalLikes = videos.Sum(v => v.Likes.Count);
        var userTotalComments = videos.Sum(v => v.Comments.Count);
        var userTotalShares = videos.Sum(v => v.Shares.Count);

        ViewBag.UserAnalytics = userAnalytics;
        ViewBag.VideoAnalytics = videoAnalytics;
        ViewBag.TotalViews = globalTotalViews;
        ViewBag.TotalLikes = globalTotalLikes;
        ViewBag.TotalComments = globalTotalComments;
        ViewBag.TotalShares = userTotalShares;
        ViewBag.TotalVideos = globalTotalVideos;
        ViewBag.UserTotalViews = userTotalViews;
        ViewBag.UserTotalLikes = userTotalLikes;
        ViewBag.UserTotalComments = userTotalComments;
        ViewBag.UserTotalVideos = videos.Count;

        return View();
    }

    // ================= PROFILE =================
    public IActionResult Profile()
    {
        if (!IsUserLoggedIn())
        {
            return RedirectToAction("Login");
        }

        var currentUserId = GetCurrentUserId();
        var user = _context.Users.Find(currentUserId);

        if (user == null)
        {
            return RedirectToAction("Login");
        }

        var videos = _context.Videos
            .Where(v => v.UserId == currentUserId)
            .OrderByDescending(v => v.UploadedAt)
            .ToList();

        var model = new ProfileViewModel
        {
            UserId = user.Id,
            FullName = $"{user.FirstName} {user.LastName}",
            Username = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            Location = user.Location,
            Website = user.Website,
            ProfilePicture = string.IsNullOrEmpty(user.ProfilePicture) ? "https://ui-avatars.com/api/?name=" + Uri.EscapeDataString($"{user.FirstName} {user.LastName}") + "&background=random" : user.ProfilePicture,
            Bio = user.Bio,
            SelectedTalents = user.SelectedTalents ?? string.Empty,
            VideosCount = videos.Count,
            FollowersCount = 0,
            FollowingCount = 0,
            AwardsCount = 0,
            IsOwnProfile = true,
            IsFollowing = false,
            Videos = videos.Select(v => new VideoViewModel
            {
                Id = v.Id,
                Title = v.Title,
                Description = v.Description,
                VideoUrl = v.VideoUrl,
                ThumbnailUrl = v.ThumbnailUrl,
                ViewsCount = v.ViewsCount,
                LikesCount = v.Likes.Count,
                CommentsCount = v.Comments.Count,
                UploadedAt = v.UploadedAt
            }).ToList(),
            Achievements = new List<AchievementViewModel>(),
            PublicProfile = user.PublicProfile,
            ShowEmail = user.ShowEmail,
            AllowMessages = user.AllowMessages,
            ShowStats = user.ShowStats
        };

        return View(model);
    }

    // ================= EDIT PROFILE =================
    public IActionResult EditProfile()
    {
        if (!IsUserLoggedIn())
        {
            return RedirectToAction("Login");
        }

        var currentUserId = GetCurrentUserId();
        var user = _context.Users.Find(currentUserId);

        if (user == null)
        {
            return RedirectToAction("Login");
        }

        return View(user);
    }

    [HttpPost]
    public IActionResult EditProfile(User user)
    {
        if (!IsUserLoggedIn())
        {
            return RedirectToAction("Login");
        }

        var currentUserId = GetCurrentUserId();
        var existingUser = _context.Users.Find(currentUserId);

        if (existingUser == null)
        {
            return RedirectToAction("Login");
        }

        if (ModelState.IsValid)
        {
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            existingUser.Location = user.Location;
            existingUser.Website = user.Website;
            existingUser.Bio = user.Bio;
            existingUser.SelectedTalents = user.SelectedTalents;
            existingUser.ProfilePicture = user.ProfilePicture;
            existingUser.PublicProfile = user.PublicProfile;
            existingUser.ShowEmail = user.ShowEmail;
            existingUser.AllowMessages = user.AllowMessages;
            existingUser.ShowStats = user.ShowStats;

            _context.SaveChanges();

            return RedirectToAction("Profile");
        }

        return View(user);
    }

    // ================= CONTESTS =================
    public IActionResult Contests(string? category, string? status, string? skillLevel)
    {
        var query = _context.Contests
            .Include(c => c.CreatedBy)
            .Include(c => c.Entries)
            .AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(c => c.Category == category);
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(c => c.Status == status);
        }

        if (!string.IsNullOrEmpty(skillLevel))
        {
            query = query.Where(c => c.SkillLevel == skillLevel || c.SkillLevel == "all");
        }

        var contests = query.OrderByDescending(c => c.IsFeatured).ThenByDescending(c => c.CreatedAt).ToList();

        return View(contests);
    }

    // ================= MENTORSHIP & COLLABS =================
    public IActionResult MentorCollabs(string? category, string? expertise, string? search, string? projectType)
    {
        var currentUserId = GetCurrentUserId();
        var isLoggedIn = IsUserLoggedIn();

        // Get mentors
        var mentorsQuery = _context.MentorshipProfiles
            .Include(m => m.User)
            .AsQueryable();

        if (!string.IsNullOrEmpty(expertise))
        {
            mentorsQuery = mentorsQuery.Where(m => m.ExpertiseAreas != null && m.ExpertiseAreas.Contains(expertise));
        }

        if (!string.IsNullOrEmpty(search))
        {
            mentorsQuery = mentorsQuery.Where(m =>
                m.User.Username.Contains(search) ||
                m.Bio.Contains(search) ||
                (m.ExpertiseAreas != null && m.ExpertiseAreas.Contains(search)));
        }

        var mentors = mentorsQuery.ToList();

        // Get collaborations
        var collabsQuery = _context.Collaborations
            .Include(c => c.Creator)
            .Include(c => c.Participants)
            .AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            collabsQuery = collabsQuery.Where(c => c.Category == category);
        }

        if (!string.IsNullOrEmpty(projectType))
        {
            collabsQuery = collabsQuery.Where(c => c.ProjectType == projectType);
        }

        if (!string.IsNullOrEmpty(search))
        {
            collabsQuery = collabsQuery.Where(c =>
                c.Title.Contains(search) ||
                c.Description.Contains(search) ||
                (c.RequiredSkills != null && c.RequiredSkills.Contains(search)));
        }

        var collabs = collabsQuery.ToList();

        var model = new MentorCollabsViewModel
        {
            Mentors = mentors,
            Collaborations = collabs,
            CategoryFilter = category,
            ExpertiseFilter = expertise,
            SearchQuery = search,
            ProjectTypeFilter = projectType,
            IsLoggedIn = isLoggedIn
        };

        return View(model);
    }

    // ================= FIND MENTORS (LEGACY - REDIRECTS TO MENTORCOLLABS) =================
    public IActionResult FindMentors(string? category, string? expertise, string? search)
    {
        var query = _context.MentorshipProfiles
            .Include(mp => mp.User)
            .Include(mp => mp.Reviews)
            .Where(mp => mp.IsAvailable)
            .AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(mp => mp.ExpertiseAreas != null && mp.ExpertiseAreas.Contains(category));
        }

        if (!string.IsNullOrEmpty(expertise))
        {
            query = query.Where(mp => mp.ExpertiseAreas != null && mp.ExpertiseAreas.Contains(expertise));
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(mp =>
                mp.Title.Contains(search) ||
                mp.Bio.Contains(search) ||
                mp.User.Username.Contains(search));
        }

        var mentors = query.OrderByDescending(mp => mp.AverageRating).ToList();

        return View(mentors);
    }

    // ================= COLLABORATIONS =================
    public IActionResult Collaborations(string? category, string? projectType, string? search)
    {
        var query = _context.Collaborations
            .Include(c => c.Creator)
            .Include(c => c.Applications)
            .Include(c => c.Participants)
            .Where(c => c.Status == "open")
            .AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(c => c.Category == category);
        }

        if (!string.IsNullOrEmpty(projectType))
        {
            query = query.Where(c => c.ProjectType == projectType);
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c =>
                c.Title.Contains(search) ||
                c.Description.Contains(search) ||
                c.RequiredSkills.Contains(search));
        }

        var collaborations = query.OrderByDescending(c => c.CreatedAt).ToList();

        return View(collaborations);
    }

    // ================= CREATE COLLABORATION =================
    public IActionResult CreateCollaboration()
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return RedirectToAction("Login");
        }

        return View();
    }

    [HttpPost]
    public IActionResult CreateCollaboration(Collaboration collaboration)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return RedirectToAction("Login");
        }

        if (ModelState.IsValid)
        {
            collaboration.CreatorId = currentUserId.Value;
            collaboration.Status = "open";
            collaboration.CreatedAt = DateTime.UtcNow;
            collaboration.UpdatedAt = DateTime.UtcNow;

            _context.Collaborations.Add(collaboration);
            _context.SaveChanges();

            return RedirectToAction("Collaborations");
        }

        return View(collaboration);
    }

    // ================= BECOME A MENTOR =================
    public IActionResult BecomeAMentor()
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return RedirectToAction("Login");
        }

        // Check if user already has a mentor profile
        var existingProfile = _context.MentorshipProfiles.FirstOrDefault(mp => mp.UserId == currentUserId.Value);
        if (existingProfile != null)
        {
            return RedirectToAction("MentorCollabs");
        }

        return View();
    }

    [HttpPost]
    public IActionResult BecomeAMentor(MentorshipProfile profile)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return RedirectToAction("Login");
        }

        if (ModelState.IsValid)
        {
            profile.UserId = currentUserId.Value;
            profile.CreatedAt = DateTime.UtcNow;
            profile.UpdatedAt = DateTime.UtcNow;
            profile.AverageRating = 0;
            profile.TotalReviews = 0;

            _context.MentorshipProfiles.Add(profile);
            _context.SaveChanges();

            return RedirectToAction("MentorCollabs");
        }

        return View(profile);
    }

    // ================= LEADERBOARD =================
    public IActionResult Leaderboard()
    {
        var currentUserId = GetCurrentUserId();
        var model = new LeaderboardViewModel();

        // Calculate Top Creators (based on videos, views, likes)
        var users = _context.Users
            .Include(u => u.Videos)
            .ThenInclude(v => v.Likes)
            .Include(u => u.Videos)
            .ThenInclude(v => v.Comments)
            .Where(u => u.Videos.Any())
            .ToList();

        var creatorsList = users.Select(u => new
        {
            User = u,
            TotalViews = u.Videos.Sum(v => v.ViewsCount),
            TotalLikes = u.Videos.Sum(v => v.Likes.Count),
            TotalVideos = u.Videos.Count,
            TotalComments = u.Videos.Sum(v => v.Comments.Count)
        }).ToList();

        var maxViews = creatorsList.Any() ? creatorsList.Max(c => c.TotalViews) : 1;
        var maxLikes = creatorsList.Any() ? creatorsList.Max(c => c.TotalLikes) : 1;
        var maxVideos = creatorsList.Any() ? creatorsList.Max(c => c.TotalVideos) : 1;

        var topCreators = creatorsList
            .Select(c => new LeaderboardEntryViewModel
            {
                UserId = c.User.Id,
                Username = c.User.Username,
                ProfilePicture = string.IsNullOrEmpty(c.User.ProfilePicture) ? "https://ui-avatars.com/api/?name=" + Uri.EscapeDataString(c.User.Username) + "&background=random" : c.User.ProfilePicture,
                TotalViews = c.TotalViews,
                TotalLikes = c.TotalLikes,
                TotalVideos = c.TotalVideos,
                TotalFollowers = 0,
                Score = maxViews > 0 && maxLikes > 0 && maxVideos > 0 
                    ? ((c.TotalViews * 0.4 / maxViews) + (c.TotalLikes * 0.4 / maxLikes) + (c.TotalVideos * 0.2 / maxVideos)) * 100 
                    : 0,
                IsCurrentUser = currentUserId == c.User.Id
            })
            .OrderByDescending(c => c.Score)
            .Take(20)
            .ToList();

        for (int i = 0; i < topCreators.Count; i++)
        {
            topCreators[i].Rank = i + 1;
        }

        model.TopCreators = topCreators;

        // Calculate Top Videos (based on views, likes, comments)
        var videos = _context.Videos
            .Include(v => v.User)
            .Include(v => v.Likes)
            .Include(v => v.Comments)
            .ToList();

        var maxVideoViews = videos.Any() ? videos.Max(v => v.ViewsCount) : 1;
        var maxVideoLikes = videos.Any() ? videos.Max(v => v.Likes.Count) : 1;
        var maxVideoComments = videos.Any() ? videos.Max(v => v.Comments.Count) : 1;

        var topVideos = videos
            .Select(v => new VideoLeaderboardEntryViewModel
            {
                VideoId = v.Id,
                Title = v.Title,
                ThumbnailUrl = string.IsNullOrEmpty(v.ThumbnailUrl) ? "" : v.ThumbnailUrl,
                ViewsCount = v.ViewsCount,
                LikesCount = v.Likes.Count,
                CommentsCount = v.Comments.Count,
                Score = maxVideoViews > 0 && maxVideoLikes > 0 && maxVideoComments > 0 
                    ? ((v.ViewsCount * 0.5 / maxVideoViews) + (v.Likes.Count * 0.3 / maxVideoLikes) + (v.Comments.Count * 0.2 / maxVideoComments)) * 100 
                    : 0,
                ArtistName = v.User.Username,
                ArtistAvatar = string.IsNullOrEmpty(v.User.ProfilePicture) ? "https://ui-avatars.com/api/?name=" + Uri.EscapeDataString(v.User.Username) + "&background=random" : v.User.ProfilePicture
            })
            .OrderByDescending(v => v.Score)
            .Take(20)
            .ToList();

        for (int i = 0; i < topVideos.Count; i++)
        {
            topVideos[i].Rank = i + 1;
        }

        model.TopVideos = topVideos;

        // Calculate Contest Winners (based on views, likes, videos in contests)
        var contests = _context.Contests
            .Include(c => c.Entries)
            .ThenInclude(e => e.User)
            .ThenInclude(u => u.Videos)
            .ThenInclude(v => v.Likes)
            .Include(c => c.Entries)
            .ThenInclude(e => e.User)
            .ThenInclude(u => u.Videos)
            .ThenInclude(v => v.Comments)
            .Where(c => c.EndDate <= DateTime.UtcNow || c.StartDate <= DateTime.UtcNow)
            .ToList();

        var ongoingContests = contests.Where(c => c.EndDate > DateTime.UtcNow).ToList();
        var completedContests = contests.Where(c => c.EndDate <= DateTime.UtcNow).ToList();

        // Ongoing contests leaderboard
        var ongoingContestEntries = new List<ContestLeaderboardEntryViewModel>();
        foreach (var contest in ongoingContests.Take(5))
        {
            var contestParticipants = contest.Entries
                .Select(e => new
                {
                    Entry = e,
                    TotalViews = e.User.Videos.Where(v => v.UploadedAt >= contest.StartDate).Sum(v => v.ViewsCount),
                    TotalLikes = e.User.Videos.Where(v => v.UploadedAt >= contest.StartDate).Sum(v => v.Likes.Count),
                    TotalVideos = e.User.Videos.Where(v => v.UploadedAt >= contest.StartDate).Count()
                })
                .ToList();

            if (contestParticipants.Any())
            {
                var maxContestViews = contestParticipants.Max(cp => cp.TotalViews);
                var maxContestLikes = contestParticipants.Max(cp => cp.TotalLikes);
                var maxContestVideos = contestParticipants.Max(cp => cp.TotalVideos);

                var rankedParticipants = contestParticipants
                    .Select(cp => new ContestLeaderboardEntryViewModel
                    {
                        ContestId = contest.Id,
                        ContestTitle = contest.Title,
                        UserId = cp.Entry.UserId,
                        Username = cp.Entry.User.Username,
                        ProfilePicture = string.IsNullOrEmpty(cp.Entry.User.ProfilePicture) ? "https://ui-avatars.com/api/?name=" + Uri.EscapeDataString(cp.Entry.User.Username) + "&background=random" : cp.Entry.User.ProfilePicture,
                        TotalViews = cp.TotalViews,
                        TotalLikes = cp.TotalLikes,
                        TotalVideos = cp.TotalVideos,
                        Score = ((cp.TotalViews * 0.4 / maxContestViews) + (cp.TotalLikes * 0.4 / maxContestLikes) + (cp.TotalVideos * 0.2 / maxContestVideos)) * 100,
                        IsWinner = false,
                        IsOngoing = true,
                        ContestEndDate = contest.EndDate
                    })
                    .OrderByDescending(c => c.Score)
                    .Take(10)
                    .ToList();

                for (int i = 0; i < rankedParticipants.Count; i++)
                {
                    rankedParticipants[i].Rank = i + 1;
                }

                ongoingContestEntries.AddRange(rankedParticipants);
            }
        }

        model.OngoingContests = ongoingContestEntries;

        // Completed contest winners
        var contestWinners = new List<ContestLeaderboardEntryViewModel>();
        foreach (var contest in completedContests.OrderByDescending(c => c.EndDate).Take(5))
        {
            var contestParticipants = contest.Entries
                .Select(e => new
                {
                    Entry = e,
                    TotalViews = e.User.Videos.Where(v => v.UploadedAt >= contest.StartDate && v.UploadedAt <= contest.EndDate).Sum(v => v.ViewsCount),
                    TotalLikes = e.User.Videos.Where(v => v.UploadedAt >= contest.StartDate && v.UploadedAt <= contest.EndDate).Sum(v => v.Likes.Count),
                    TotalVideos = e.User.Videos.Where(v => v.UploadedAt >= contest.StartDate && v.UploadedAt <= contest.EndDate).Count()
                })
                .ToList();

            if (contestParticipants.Any())
            {
                var maxContestViews = contestParticipants.Max(cp => cp.TotalViews);
                var maxContestLikes = contestParticipants.Max(cp => cp.TotalLikes);
                var maxContestVideos = contestParticipants.Max(cp => cp.TotalVideos);

                var rankedParticipants = contestParticipants
                    .Select(cp => new ContestLeaderboardEntryViewModel
                    {
                        ContestId = contest.Id,
                        ContestTitle = contest.Title,
                        UserId = cp.Entry.UserId,
                        Username = cp.Entry.User.Username,
                        ProfilePicture = string.IsNullOrEmpty(cp.Entry.User.ProfilePicture) ? "https://ui-avatars.com/api/?name=" + Uri.EscapeDataString(cp.Entry.User.Username) + "&background=random" : cp.Entry.User.ProfilePicture,
                        TotalViews = cp.TotalViews,
                        TotalLikes = cp.TotalLikes,
                        TotalVideos = cp.TotalVideos,
                        Score = ((cp.TotalViews * 0.4 / maxContestViews) + (cp.TotalLikes * 0.4 / maxContestLikes) + (cp.TotalVideos * 0.2 / maxContestVideos)) * 100,
                        IsWinner = true,
                        IsOngoing = false,
                        ContestEndDate = contest.EndDate
                    })
                    .OrderByDescending(c => c.Score)
                    .Take(10)
                    .ToList();

                for (int i = 0; i < rankedParticipants.Count; i++)
                {
                    rankedParticipants[i].Rank = i + 1;
                    if (i == 0) rankedParticipants[i].IsWinner = true;
                }

                contestWinners.AddRange(rankedParticipants);
            }
        }

        model.ContestWinners = contestWinners;

        return View(model);
    }

    // ================= JOBS =================
    public IActionResult Jobs(string? category, string? jobType, string? location, string? search)
    {
        var query = _context.Jobs
            .Include(j => j.PostedBy)
            .Where(j => j.IsActive)
            .AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(j => j.Category == category);
        }

        if (!string.IsNullOrEmpty(jobType))
        {
            query = query.Where(j => j.JobType == jobType);
        }

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(j => j.Location.Contains(location));
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(j =>
                j.Title.Contains(search) ||
                j.Description.Contains(search) ||
                j.CompanyName.Contains(search));
        }

        var jobs = query.OrderByDescending(j => j.IsFeatured).ThenByDescending(j => j.PostedAt).ToList();

        var currentUserId = GetCurrentUserId();
        var model = new JobBoardViewModel
        {
            Jobs = jobs.Select(j => new JobViewModel
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                Requirements = j.Requirements,
                Benefits = j.Benefits,
                Category = j.Category,
                JobType = j.JobType,
                Location = j.Location,
                IsRemote = j.IsRemote,
                SalaryRange = j.SalaryRange,
                ExperienceLevel = j.ExperienceLevel,
                CompanyName = j.CompanyName,
                CompanyLogo = j.CompanyLogo,
                CompanyWebsite = j.CompanyWebsite,
                PostedAt = j.PostedAt,
                Deadline = j.Deadline,
                IsActive = j.IsActive,
                PostedById = j.PostedById,
                PostedByName = j.PostedBy?.Username ?? "Unknown",
                PostedByAvatar = string.IsNullOrEmpty(j.PostedBy?.ProfilePicture) ? "https://ui-avatars.com/api/?name=" + Uri.EscapeDataString(j.PostedBy?.Username ?? "Unknown") + "&background=random" : j.PostedBy.ProfilePicture,
                ApplicationsCount = 0,
                HasApplied = false,
                IsOwner = currentUserId == j.PostedById
            }).ToList(),
            TotalJobs = jobs.Count,
            CategoryFilter = category,
            JobTypeFilter = jobType,
            LocationFilter = location,
            SearchQuery = search
        };

        return View(model);
    }

    // ================= CREATE JOB =================
    [HttpPost]
    public async Task<IActionResult> CreateJob(string Title, string CompanyName, string Category, string JobType, string Location, string SalaryRange, string Description, string? ExperienceLevel, string? RequiredSkills, bool IsRemote)
    {
        if (!IsUserLoggedIn())
            return Json(new { success = false, message = "Please login first" });

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return Json(new { success = false, message = "User not found" });

        var job = new Job
        {
            Title = Title,
            CompanyName = CompanyName,
            Category = Category,
            JobType = JobType,
            Location = Location,
            SalaryRange = SalaryRange,
            Description = Description,
            ExperienceLevel = ExperienceLevel ?? "entry",
            TalentCategories = RequiredSkills,
            IsRemote = IsRemote,
            PostedById = currentUserId.Value,
            PostedAt = DateTime.UtcNow,
            IsActive = true,
            IsFeatured = false
        };

        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Job posted successfully", jobId = job.Id });
    }

    // ================= JOB DETAILS =================
    [HttpGet]
    public async Task<IActionResult> JobDetails(int id)
    {
        var job = await _context.Jobs
            .Include(j => j.PostedBy)
            .Include(j => j.Applications)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (job == null)
            return Json(new { success = false, message = "Job not found" });

        var currentUserId = GetCurrentUserId();
        var hasApplied = currentUserId.HasValue && job.Applications.Any(a => a.ApplicantId == currentUserId.Value);

        return Json(new
        {
            success = true,
            job = new
            {
                job.Id,
                job.Title,
                job.Description,
                job.Category,
                job.JobType,
                job.Location,
                job.SalaryRange,
                job.ExperienceLevel,
                RequiredSkills = job.TalentCategories,
                job.IsRemote,
                job.CompanyName,
                job.PostedAt,
                CreatorName = job.PostedBy?.Username ?? "Unknown",
                CreatorProfilePicture = job.PostedBy?.ProfilePicture,
                PostedAgo = GetTimeAgo(job.PostedAt),
                HasApplied = hasApplied,
                IsOwner = currentUserId.HasValue && job.PostedById == currentUserId.Value
            }
        });
    }

    // ================= JOIN JOB =================
    [HttpPost]
    public async Task<IActionResult> JoinJob(int jobId)
    {
        if (!IsUserLoggedIn())
            return Json(new { success = false, message = "Please login first" });

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return Json(new { success = false, message = "User not found" });

        var job = await _context.Jobs
            .Include(j => j.PostedBy)
            .FirstOrDefaultAsync(j => j.Id == jobId);

        if (job == null)
            return Json(new { success = false, message = "Job not found" });

        if (!job.IsActive)
            return Json(new { success = false, message = "Job is not active" });

        var existingApplication = await _context.JobApplications
            .FirstOrDefaultAsync(ja => ja.JobId == jobId && ja.ApplicantId == currentUserId.Value);

        if (existingApplication != null)
            return Json(new { success = false, message = "Already applied to this job" });

        var application = new JobApplication
        {
            JobId = jobId,
            ApplicantId = currentUserId.Value,
            AppliedAt = DateTime.UtcNow,
            Status = "pending"
        };

        _context.JobApplications.Add(application);

        // Create notification for job creator
        var currentUser = await _context.Users.FindAsync(currentUserId.Value);
        var notification = new Notification
        {
            UserId = job.PostedById,
            Title = "New Job Application",
            Message = $"{currentUser?.Username ?? "A user"} has sent a job joining request. Please check their details.",
            Type = "job_application",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Job application submitted successfully" });
    }

    // ================= APPROVE JOB APPLICATION =================
    [HttpPost]
    public async Task<IActionResult> ApproveJobApplication(int applicationId)
    {
        if (!IsUserLoggedIn())
            return Json(new { success = false, message = "Please login first" });

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return Json(new { success = false, message = "User not found" });

        var application = await _context.JobApplications
            .Include(ja => ja.Job)
            .Include(ja => ja.Applicant)
            .FirstOrDefaultAsync(ja => ja.Id == applicationId);

        if (application == null)
            return Json(new { success = false, message = "Application not found" });

        if (application.Job.PostedById != currentUserId.Value)
            return Json(new { success = false, message = "You are not authorized to approve this application" });

        application.Status = "hired";

        // Create notification for applicant
        var notification = new Notification
        {
            UserId = application.ApplicantId,
            Title = "Job Application Approved",
            Message = "You have been selected for this opportunity. Thank you",
            Type = "job_approved",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Application approved successfully" });
    }

    // ================= REJECT JOB APPLICATION =================
    [HttpPost]
    public async Task<IActionResult> RejectJobApplication(int applicationId)
    {
        if (!IsUserLoggedIn())
            return Json(new { success = false, message = "Please login first" });

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return Json(new { success = false, message = "User not found" });

        var application = await _context.JobApplications
            .Include(ja => ja.Job)
            .FirstOrDefaultAsync(ja => ja.Id == applicationId);

        if (application == null)
            return Json(new { success = false, message = "Application not found" });

        if (application.Job.PostedById != currentUserId.Value)
            return Json(new { success = false, message = "You are not authorized to reject this application" });

        application.Status = "rejected";

        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Application rejected successfully" });
    }

    // ================= MESSAGES =================
    public IActionResult Messages()
    {
        if (!IsUserLoggedIn())
        {
            return RedirectToAction("Login");
        }

        var currentUserId = GetCurrentUserId();
        var conversations = _context.Conversations
            .Include(c => c.User1)
            .Include(c => c.User2)
            .Include(c => c.Messages)
            .Where(c => c.User1Id == currentUserId || c.User2Id == currentUserId)
            .OrderByDescending(c => c.LastMessageAt)
            .ToList();

        return View(conversations);
    }

    // ================= NOTIFICATIONS =================
    public IActionResult Notifications(string? typeFilter, bool showUnreadOnly = false)
    {
        if (!IsUserLoggedIn())
        {
            return RedirectToAction("Login");
        }

        var currentUserId = GetCurrentUserId();
        var query = _context.Notifications
            .Where(n => n.UserId == currentUserId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(typeFilter) && typeFilter != "all")
        {
            query = query.Where(n => n.Type == typeFilter);
        }

        if (showUnreadOnly)
        {
            query = query.Where(n => !n.IsRead);
        }

        var notifications = query.OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationViewModel
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type,
                Link = n.Link,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            })
            .ToList();

        var model = new NotificationsPageViewModel
        {
            Notifications = notifications,
            TypeFilter = typeFilter,
            ShowUnreadOnly = showUnreadOnly,
            UnreadCount = notifications.Count(n => !n.IsRead)
        };

        return View(model);
    }

    // ================= MY APPLICATIONS =================
    public IActionResult MyApplications()
    {
        if (!IsUserLoggedIn())
        {
            return RedirectToAction("Login");
        }

        var currentUserId = GetCurrentUserId();
        var applications = _context.JobApplications
            .Include(ja => ja.Job)
            .Where(ja => ja.ApplicantId == currentUserId)
            .OrderByDescending(ja => ja.AppliedAt)
            .ToList();

        return View(applications);
    }

    // ================= MY COLLABORATIONS =================
    public IActionResult MyCollaborations()
    {
        if (!IsUserLoggedIn())
        {
            return RedirectToAction("Login");
        }

        var currentUserId = GetCurrentUserId();
        var collaborations = _context.Collaborations
            .Include(c => c.Creator)
            .Include(c => c.Participants)
            .Where(c => c.CreatorId == currentUserId || c.Participants.Any(p => p.UserId == currentUserId))
            .OrderByDescending(c => c.CreatedAt)
            .ToList();

        return View(collaborations);
    }

    // ================= MY COLLABORATION APPLICATIONS =================
    public IActionResult MyCollaborationApplications()
    {
        if (!IsUserLoggedIn())
        {
            return RedirectToAction("Login");
        }

        var currentUserId = GetCurrentUserId();
        var applications = _context.CollaborationApplications
            .Include(ca => ca.Collaboration)
            .ThenInclude(c => c.Creator)
            .Where(ca => ca.ApplicantId == currentUserId)
            .OrderByDescending(ca => ca.AppliedAt)
            .ToList();

        return View(applications);
    }

    // ================= MY SESSIONS =================
    public IActionResult MySessions()
    {
        if (!IsUserLoggedIn())
        {
            return RedirectToAction("Login");
        }

        var currentUserId = GetCurrentUserId();
        var sessions = _context.MentorshipSessions
            .Include(ms => ms.Mentor)
            .Include(ms => ms.Mentee)
            .Where(ms => ms.MentorId == currentUserId || ms.MenteeId == currentUserId)
            .OrderByDescending(ms => ms.ScheduledDate)
            .ToList();

        return View(sessions);
    }

    // ================= SCOUT PROFILES =================
    public IActionResult ScoutProfiles()
    {
        if (!IsUserLoggedIn())
        {
            return RedirectToAction("Login");
        }

        var profiles = _context.TalentScoutProfiles
            .Include(tsp => tsp.User)
            .Where(tsp => tsp.IsActive)
            .OrderByDescending(tsp => tsp.TotalHires)
            .ToList();

        return View(profiles);
    }
}