using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entety
{
    public partial class oopgroup1Context : DbContext
    {
        public oopgroup1Context()
        {
        }

        public oopgroup1Context(DbContextOptions<oopgroup1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Tweet> Tweets { get; set; }
        public virtual DbSet<TweetsView> TweetsViews { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserToRetweet> UserToRetweets { get; set; }
        public virtual DbSet<UserToUser> UserToUsers { get; set; }
        public virtual DbSet<UsersAndTweetsView> UsersAndTweetsViews { get; set; }
        public virtual DbSet<UsersView> UsersViews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=****;Database=****;User Id=****;Password=****;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tweet>(entity =>
            {
                entity.ToTable("Tweet");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tweets)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tweet__UserId__4F47C5E3");
            });

            modelBuilder.Entity<TweetsView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("TweetsView");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Username, "UQ__User__536C85E4DC84E820")
                    .IsUnique();

                entity.Property(e => e.Biography).IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserToRetweet>(entity =>
            {
                entity.ToTable("UserToRetweet");

                entity.HasOne(d => d.Tweet)
                    .WithMany(p => p.UserToRetweets)
                    .HasForeignKey(d => d.TweetId)
                    .HasConstraintName("FK__UserToRet__Tweet__540C7B00");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserToRetweets)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserToRet__UserI__5224328E");
            });

            modelBuilder.Entity<UserToUser>(entity =>
            {
                entity.ToTable("UserToUser");

                entity.HasOne(d => d.Following)
                    .WithMany(p => p.UserToUserFollowings)
                    .HasForeignKey(d => d.FollowingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserToUse__Follo__51300E55");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserToUserUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserToUse__UserI__503BEA1C");
            });

            modelBuilder.Entity<UsersAndTweetsView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("UsersAndTweetsView");

                entity.Property(e => e.Biography).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Message)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsersView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("UsersView");

                entity.Property(e => e.Biography).IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
