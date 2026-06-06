using Microsoft.EntityFrameworkCore;
using NewTalent.Data.Entities;

namespace NewTalent.Data
{
    public class NewTalentDbContext : DbContext
    {
        public NewTalentDbContext(DbContextOptions<NewTalentDbContext> options)
            : base(options)
        {
        }

        // ================= TABLES =================
        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunityMember> CommunityMembers { get; set; }
        public DbSet<CommunityPost> CommunityPosts { get; set; }
        public DbSet<CommunityPostComment> CommunityPostComments { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<ContestEntry> ContestEntries { get; set; }
        public DbSet<MentorshipSession> MentorshipSessions { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }
        public DbSet<Collaboration> Collaborations { get; set; }
        public DbSet<CollaborationApplication> CollaborationApplications { get; set; }
        public DbSet<CollaborationParticipant> CollaborationParticipants { get; set; }
        public DbSet<MentorshipProfile> MentorshipProfiles { get; set; }
        public DbSet<MentorshipReview> MentorshipReviews { get; set; }
        public DbSet<ContestEntryVote> ContestEntryVotes { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }
        public DbSet<TalentScoutProfile> TalentScoutProfiles { get; set; }
        public DbSet<VideoAnalytics> VideoAnalytics { get; set; }
        public DbSet<UserAnalytics> UserAnalytics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USER
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Username)
                .IsUnique();

            // VIDEO
            modelBuilder.Entity<Video>()
                .HasOne(v => v.User)
                .WithMany(u => u.Videos)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // COMMENT
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Video)
                .WithMany(v => v.Comments)
                .HasForeignKey(c => c.VideoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // LIKE (unique per user per video)
            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.VideoId, l.UserId })
                .IsUnique();

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Video)
                .WithMany(v => v.Likes)
                .HasForeignKey(l => l.VideoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // SHARE
            modelBuilder.Entity<Share>()
                .HasOne(s => s.Video)
                .WithMany(v => v.Shares)
                .HasForeignKey(s => s.VideoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Share>()
                .HasOne(s => s.User)
                .WithMany(u => u.Shares)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // RATING (unique per user per video)
            modelBuilder.Entity<Rating>()
                .HasIndex(r => new { r.VideoId, r.UserId })
                .IsUnique();

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Video)
                .WithMany()
                .HasForeignKey(r => r.VideoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // CONVERSATION
            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.User1)
                .WithMany()
                .HasForeignKey(c => c.User1Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.User2)
                .WithMany()
                .HasForeignKey(c => c.User2Id)
                .OnDelete(DeleteBehavior.NoAction);

            // MESSAGE
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            // JOB
            modelBuilder.Entity<Job>()
                .HasOne(j => j.PostedBy)
                .WithMany()
                .HasForeignKey(j => j.PostedById)
                .OnDelete(DeleteBehavior.NoAction);

            // JOB APPLICATION
            modelBuilder.Entity<JobApplication>()
                .HasIndex(ja => new { ja.JobId, ja.ApplicantId })
                .IsUnique();

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(ja => ja.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.Applicant)
                .WithMany()
                .HasForeignKey(ja => ja.ApplicantId)
                .OnDelete(DeleteBehavior.NoAction);

            // NOTIFICATION
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // COMMUNITY
            modelBuilder.Entity<Community>()
                .HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            // COMMUNITY MEMBER (unique per user per community)
            modelBuilder.Entity<CommunityMember>()
                .HasIndex(cm => new { cm.CommunityId, cm.UserId })
                .IsUnique();

            modelBuilder.Entity<CommunityMember>()
                .HasOne(cm => cm.Community)
                .WithMany(c => c.Members)
                .HasForeignKey(cm => cm.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CommunityMember>()
                .HasOne(cm => cm.User)
                .WithMany()
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // COMMUNITY POST
            modelBuilder.Entity<CommunityPost>()
                .HasOne(cp => cp.Community)
                .WithMany(c => c.Posts)
                .HasForeignKey(cp => cp.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CommunityPost>()
                .HasOne(cp => cp.Author)
                .WithMany()
                .HasForeignKey(cp => cp.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);

            // COMMUNITY POST COMMENT
            modelBuilder.Entity<CommunityPostComment>()
                .HasOne(cpc => cpc.Post)
                .WithMany(cp => cp.Comments)
                .HasForeignKey(cpc => cpc.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CommunityPostComment>()
                .HasOne(cpc => cpc.User)
                .WithMany()
                .HasForeignKey(cpc => cpc.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // CONTEST
            modelBuilder.Entity<Contest>()
                .HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            // CONTEST ENTRY (unique per user per contest)
            modelBuilder.Entity<ContestEntry>()
                .HasIndex(ce => new { ce.ContestId, ce.UserId })
                .IsUnique();

            modelBuilder.Entity<ContestEntry>()
                .HasOne(ce => ce.Contest)
                .WithMany(c => c.Entries)
                .HasForeignKey(ce => ce.ContestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContestEntry>()
                .HasOne(ce => ce.User)
                .WithMany()
                .HasForeignKey(ce => ce.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // MENTORSHIP SESSION
            modelBuilder.Entity<MentorshipSession>()
                .HasOne(ms => ms.Mentor)
                .WithMany(u => u.MentorshipSessionsAsMentor)
                .HasForeignKey(ms => ms.MentorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MentorshipSession>()
                .HasOne(ms => ms.Mentee)
                .WithMany(u => u.MentorshipSessionsAsMentee)
                .HasForeignKey(ms => ms.MenteeId)
                .OnDelete(DeleteBehavior.NoAction);

            // ACHIEVEMENT
            modelBuilder.Entity<Achievement>()
                .HasOne(a => a.User)
                .WithMany(u => u.Achievements)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // AWARD
            modelBuilder.Entity<Award>()
                .HasOne(a => a.User)
                .WithMany(u => u.Awards)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // CERTIFICATION
            modelBuilder.Entity<Certification>()
                .HasOne(c => c.User)
                .WithMany(u => u.Certifications)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // USER PREFERENCE
            modelBuilder.Entity<UserPreference>()
                .HasOne(up => up.User)
                .WithOne()
                .HasForeignKey<UserPreference>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // COLLABORATION
            modelBuilder.Entity<Collaboration>()
                .HasOne(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            // COLLABORATION APPLICATION
            modelBuilder.Entity<CollaborationApplication>()
                .HasOne(ca => ca.Collaboration)
                .WithMany(c => c.Applications)
                .HasForeignKey(ca => ca.CollaborationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CollaborationApplication>()
                .HasOne(ca => ca.Applicant)
                .WithMany()
                .HasForeignKey(ca => ca.ApplicantId)
                .OnDelete(DeleteBehavior.NoAction);

            // COLLABORATION PARTICIPANT
            modelBuilder.Entity<CollaborationParticipant>()
                .HasOne(cp => cp.Collaboration)
                .WithMany(c => c.Participants)
                .HasForeignKey(cp => cp.CollaborationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CollaborationParticipant>()
                .HasOne(cp => cp.User)
                .WithMany()
                .HasForeignKey(cp => cp.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // MENTORSHIP PROFILE
            modelBuilder.Entity<MentorshipProfile>()
                .HasOne(mp => mp.User)
                .WithOne()
                .HasForeignKey<MentorshipProfile>(mp => mp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // MENTORSHIP REVIEW
            modelBuilder.Entity<MentorshipReview>()
                .HasOne(mr => mr.MentorshipProfile)
                .WithMany(mp => mp.Reviews)
                .HasForeignKey(mr => mr.MentorshipProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MentorshipReview>()
                .HasOne(mr => mr.Reviewer)
                .WithMany()
                .HasForeignKey(mr => mr.ReviewerId)
                .OnDelete(DeleteBehavior.NoAction);

            // CONTEST ENTRY VOTE
            modelBuilder.Entity<ContestEntryVote>()
                .HasOne(cev => cev.ContestEntry)
                .WithMany()
                .HasForeignKey(cev => cev.ContestEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContestEntryVote>()
                .HasOne(cev => cev.Voter)
                .WithMany()
                .HasForeignKey(cev => cev.VoterId)
                .OnDelete(DeleteBehavior.Cascade);

            // LEADERBOARD
            modelBuilder.Entity<Leaderboard>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // TALENT SCOUT PROFILE
            modelBuilder.Entity<TalentScoutProfile>()
                .HasOne(tsp => tsp.User)
                .WithOne()
                .HasForeignKey<TalentScoutProfile>(tsp => tsp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TalentScoutProfile>()
                .HasMany(tsp => tsp.Jobs)
                .WithOne(j => j.TalentScoutProfile)
                .HasForeignKey(j => j.TalentScoutProfileId)
                .OnDelete(DeleteBehavior.SetNull);

            // VIDEO ANALYTICS
            modelBuilder.Entity<VideoAnalytics>()
                .HasOne(va => va.Video)
                .WithMany()
                .HasForeignKey(va => va.VideoId)
                .OnDelete(DeleteBehavior.Cascade);

            // USER ANALYTICS
            modelBuilder.Entity<UserAnalytics>()
                .HasOne(ua => ua.User)
                .WithMany()
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}