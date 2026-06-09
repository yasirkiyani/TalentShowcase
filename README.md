# TalentShowcase

A comprehensive web-based talent showcase platform built with ASP.NET Core 8.0 MVC and SQL Server. This platform enables artists, performers, and creative professionals to showcase their talents, connect with communities, participate in contests, find mentorship opportunities, and discover job opportunities.

## 🌟 Features

### Core Features
- **User Authentication**: Session-based registration and login system
- **Video Showcase**: Upload and showcase talent videos (supports up to 1GB files)
- **User Profiles**: Comprehensive profiles with talent categories, bio, and privacy settings
- **Social Interactions**: Like, comment, share, and rate videos
- **Communities**: Join and create talent-based communities for networking
- **Contests**: Participate in talent contests with public voting
- **Mentorship**: Find mentors or become a mentor to guide aspiring talents
- **Job Board**: Browse and apply for talent-related job opportunities
- **Collaborations**: Find collaborators for creative projects
- **Messaging**: Direct messaging system for networking
- **Notifications**: Real-time notification system for user engagement
- **Analytics**: Track video performance and user statistics
- **Leaderboards**: Compete with other artists on leaderboards

### Advanced Features
- **Talent Scout Profiles**: Professional talent scouting profiles
- **Achievements & Awards**: Recognition system for accomplishments
- **Certifications**: Display professional certifications
- **User Preferences**: Customizable user experience
- **Video Processing**: Support for multiple video formats (MP4, AVI, MOV, WebM)
- **Privacy Controls**: Granular privacy settings for profiles and videos
- **Skill Levels**: Categorize content by skill level (beginner, intermediate, advanced)

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server
- **ORM**: Entity Framework Core 8.0.27
- **Authentication**: Session-based authentication
- **File Upload**: Support for large file uploads (up to 1GB)
- **Frontend**: Razor Views with CSS and JavaScript
- **Server**: Kestrel with IIS support

## 📁 Project Structure

```
NewTalent/
├── Controllers/
│   ├── BaseController.cs          # Base controller with helper methods
│   ├── HomeController.cs           # Home controller
│   └── TalentController.cs         # Main controller with all actions
├── Data/
│   ├── Entities/                   # Database entities (30+ models)
│   │   ├── User.cs
│   │   ├── Video.cs
│   │   ├── Comment.cs
│   │   ├── Like.cs
│   │   ├── Share.cs
│   │   ├── Rating.cs
│   │   ├── Message.cs
│   │   ├── Conversation.cs
│   │   ├── Job.cs
│   │   ├── JobApplication.cs
│   │   ├── Notification.cs
│   │   ├── Community.cs
│   │   ├── CommunityMember.cs
│   │   ├── CommunityPost.cs
│   │   ├── Contest.cs
│   │   ├── ContestEntry.cs
│   │   ├── MentorshipSession.cs
│   │   ├── MentorshipProfile.cs
│   │   ├── Collaboration.cs
│   │   ├── Achievement.cs
│   │   ├── Award.cs
│   │   ├── Certification.cs
│   │   └── ... (and more)
│   └── NewTalentDbcontext.cs      # Database context
├── Models/                         # View Models
│   ├── IndexViewModel.cs
│   ├── VideoViewModel.cs
│   ├── ProfileViewModel.cs
│   ├── CommunityViewModel.cs
│   ├── ContestViewModel.cs
│   └── ... (20+ view models)
├── Views/
│   ├── Shared/                     # Shared layouts and components
│   └── Talent/                     # Talent controller views (27 views)
│       ├── Index.cshtml
│       ├── Explore.cshtml
│       ├── Upload.cshtml
│       ├── Profile.cshtml
│       ├── Communities.cshtml
│       ├── Contests.cshtml
│       ├── Jobs.cshtml
│       ├── Mentorship.cshtml
│       ├── Collaborations.cshtml
│       ├── Analytics.cshtml
│       └── ... (and more)
├── Services/
│   └── NotificationService.cs      # Notification management service
├── wwwroot/                        # Static files
│   ├── css/                        # Stylesheets
│   ├── js/                         # JavaScript files
│   ├── lib/                        # Third-party libraries
│   ├── videos/                     # Uploaded videos
│   ├── profilepics/                # User profile pictures
│   └── contests/                   # Contest-related assets
├── Migrations/                     # Database migrations
├── Program.cs                      # Application entry point
├── appsettings.json                # Application configuration
├── seed_data.sql                   # Sample data for testing
└── NewTalent.csproj                # Project file
```

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (local or remote)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd NewTalent
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure database connection**
   
   Update the connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=YOUR_SERVER;Initial Catalog=NewTalent;Integrated Security=True;Trust Server Certificate=True"
     }
   }
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run seed data (optional)**
   
   Execute the `seed_data.sql` file in your SQL Server to populate sample data:
   ```sql
   -- Open seed_data.sql in SQL Server Management Studio
   -- Execute the script to create sample users and videos
   ```

6. **Run the application**
   ```bash
   dotnet run
   ```

7. **Access the application**
   
   Open your browser and navigate to:
   ```
   https://localhost:5001
   ```

## 📊 Database Schema

The application uses a comprehensive database schema with 30+ tables:

### Core Tables
- **Users**: User accounts with profiles, privacy settings, and talent categories
- **Videos**: Video uploads with metadata, analytics, and engagement metrics
- **Comments**: User comments on videos
- **Likes**: User likes on videos (unique per user per video)
- **Shares**: Video shares tracking
- **Ratings**: Video ratings with average calculations

### Social Features
- **Messages & Conversations**: Direct messaging system
- **Communities**: User communities with posts and members
- **CommunityPosts & CommunityPostComments**: Community content
- **Notifications**: User notifications with read status

### Professional Features
- **Jobs & JobApplications**: Job postings and applications
- **Contests & ContestEntries**: Talent contests with voting
- **ContestEntryVotes**: Public voting system
- **MentorshipProfiles & MentorshipSessions**: Mentorship program
- **MentorshipReviews**: Mentor reviews and ratings
- **Collaborations & CollaborationApplications**: Project collaboration
- **CollaborationParticipants**: Collaboration team members

### Recognition & Analytics
- **Achievements & Awards**: User accomplishments
- **Certifications**: Professional certifications
- **Leaderboards**: Competition rankings
- **VideoAnalytics**: Video performance metrics
- **UserAnalytics**: User engagement statistics
- **TalentScoutProfiles**: Professional talent scout profiles

### Configuration
- **UserPreferences**: User customization settings
- **UserPreferences**: Privacy and notification preferences

## 🔧 Configuration

### Server Limits

The application is configured to handle large file uploads (up to 1GB):

```csharp
// Kestrel server limits
options.Limits.MaxRequestBodySize = 1073741824; // 1GB
options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(5);

// IIS limits (if using IIS)
options.MaxRequestBodySize = 1073741824; // 1GB

// Form options
options.MultipartBodyLengthLimit = 1073741824; // 1GB
```

### Default Route

The default route is configured to:
```
{controller=Talent}/{action=Explore}/{id?}
```

This means the application starts at the Explore page by default.

## 🎯 Main Pages

### Public Pages (Guest Access)
- **Index**: Home page with trending videos and statistics
- **Explore**: Browse all videos with filters
- **Communities**: View and join communities
- **CommunityView**: View community details and posts
- **Login**: User login page
- **Register**: User registration page

### Authenticated Pages
- **Profile**: User profile management
- **Upload**: Upload new videos
- **EditProfile**: Edit user profile information
- **Messages**: View and send messages
- **Notifications**: View user notifications
- **MyApplications**: View job applications
- **MyCollaborations**: View collaboration projects
- **MySessions**: View mentorship sessions
- **Analytics**: View video and user analytics

### Feature Pages
- **Contests**: Browse and participate in contests
- **Jobs**: Browse job opportunities
- **Collaborations**: Find collaboration opportunities
- **BecomeAMentor**: Create mentorship profile
- **FindMentors**: Browse available mentors
- **MentorCollabs**: Mentor collaboration opportunities
- **Leaderboard**: View talent rankings
- **ScoutProfiles**: View talent scout profiles
- **CommunityOptions**: Community management options

## 🎨 Features Detail

### Video Upload
- Support for multiple formats: MP4, AVI, MOV, WebM
- Video types: performance, tutorial, exhibition, other
- Automatic thumbnail generation
- Video processing status tracking
- Privacy controls (public, private, unlisted)
- Engagement settings (comments, likes, shares, downloads)
- Skill level categorization
- Tag system for discoverability

### Communities
- Create and join talent-based communities
- Community posts and comments
- Privacy settings (public, private)
- Member management
- Community rules and guidelines
- Activity tracking

### Contests
- Create talent contests
- Public voting system
- Judge-based or hybrid judging
- Prize management
- Participant limits
- Skill level filtering
- Featured contest support

### Mentorship
- Create mentor profiles
- Set availability and rates
- Session management
- Review and rating system
- Portfolio and qualification display
- Multiple session formats (online, in-person, hybrid)

### Jobs
- Post job opportunities
- Job applications tracking
- Talent category filtering
- Remote job support
- Company profiles
- Salary ranges and experience levels

### Collaborations
- Create collaboration projects
- Application system
- Participant management
- Project timeline tracking
- Skill requirements

## 🔐 Authentication

The application uses session-based authentication:
- User email stored in session
- User ID stored in session
- Guest access allowed for specific pages
- Authentication check in BaseController

## 📈 Analytics

The platform provides comprehensive analytics:
- **Video Analytics**: Views, engagement, demographics
- **User Analytics**: Profile visits, video performance, engagement rates
- **Leaderboards**: Rankings by category and skill level

## 🌐 API Endpoints

The application follows MVC pattern with controller actions:
- All actions are in `TalentController`
- Base controller provides helper methods
- Session management for authentication
- Entity Framework for data access

## 🧪 Testing

Sample data is provided in `seed_data.sql` with:
- 6 sample users with diverse talents
- 3-4 videos per user
- Various talent categories (Music, Dance, Art, Acting, Coding, Comedy, Writing)
- Sample profiles and metadata

## 📝 Development

### Adding New Features

1. Create entity in `Data/Entities/`
2. Add DbSet in `NewTalentDbContext.cs`
3. Create view model in `Models/`
4. Add action in `TalentController.cs`
5. Create view in `Views/Talent/`
6. Update database with migrations

### Database Migrations

```bash
# Create new migration
dotnet ef migrations add MigrationName

# Apply migration
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

## 🤝 Contributing

Contributions are welcome! Please follow these steps:
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 👥 Authors

- Development Team

## 🙏 Acknowledgments

- Built with ASP.NET Core 8.0
- Entity Framework Core for data access
- SQL Server for database management
- Sample videos from sample-videos.com
- Profile pictures from ui-avatars.com

## 📞 Support

For support and questions, please open an issue in the repository.

---

**Note**: This is a comprehensive talent showcase platform designed to help artists and creative professionals showcase their work, connect with others, and discover opportunities in the entertainment industry.
