using Microsoft.AspNetCore.Mvc;

namespace NewTalent.Controllers;

/// <summary>
/// Base controller with common helper methods
/// </summary>
public class BaseController : Controller
{
    /// <summary>
    /// Get current user ID from session
    /// </summary>
    protected int? GetCurrentUserId()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        return userId;
    }

    /// <summary>
    /// Check if user is logged in
    /// </summary>
    protected bool IsUserLoggedIn()
    {
        return GetCurrentUserId() != null;
    }

    /// <summary>
    /// Get current user from session (legacy method using email)
    /// </summary>
    protected int? GetCurrentUserIdByEmail()
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
            return null;
        
        // This is a fallback method - ideally all controllers should use UserId
        return null;
    }
}
