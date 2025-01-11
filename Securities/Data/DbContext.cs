using Microsoft.EntityFrameworkCore;
using Securities.Models;

namespace Securities.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDocument> ProductDocuments { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<AuctionParticipant> AuctionParticipants { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // AuctionParticipant configurations
    modelBuilder.Entity<AuctionParticipant>()
        .HasKey(ap => new { ap.AuctionID, ap.UserID });

    modelBuilder.Entity<AuctionParticipant>()
        .HasOne(ap => ap.Auction)
        .WithMany(a => a.Participants)
        .HasForeignKey(ap => ap.AuctionID);

    modelBuilder.Entity<AuctionParticipant>()
        .HasOne(ap => ap.User)
        .WithMany(u => u.Participations)
        .HasForeignKey(ap => ap.UserID);

    // Auction configurations
    modelBuilder.Entity<Auction>()
        .HasOne(a => a.Products)
        .WithMany(p => p.Auctions)
        .HasForeignKey(a => a.PropertyID)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Auction>()
        .HasOne(a => a.Winner)
        .WithMany(u => u.WonAuctions)
        .HasForeignKey(a => a.WinnerID)
        .IsRequired(false)
        .OnDelete(DeleteBehavior.Restrict);

    // Bid configurations
    modelBuilder.Entity<Bid>()
        .HasOne(b => b.Auction)
        .WithMany(a => a.Bids)
        .HasForeignKey(b => b.AuctionID)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Bid>()
        .HasOne(b => b.Bidder)
        .WithMany(u => u.Bids)
        .HasForeignKey(b => b.BidderID)
        .OnDelete(DeleteBehavior.Restrict);

    // Product configurations
    modelBuilder.Entity<Product>()
        .HasOne(p => p.Seller)
        .WithMany(u => u.Products)
        .HasForeignKey(p => p.SellerID)
        .OnDelete(DeleteBehavior.Restrict);

    // ProductDocument configurations
    modelBuilder.Entity<ProductDocument>()
        .HasOne(pd => pd.Products)
        .WithMany(p => p.LegalDocuments)
        .HasForeignKey(pd => pd.ProductID)
        .OnDelete(DeleteBehavior.Cascade);

    // Notification configurations
    modelBuilder.Entity<Notification>()
        .HasOne(n => n.User)
        .WithMany(u => u.Notifications)
        .HasForeignKey(n => n.UserID)
        .OnDelete(DeleteBehavior.Cascade);

    // Default values and constraints
    modelBuilder.Entity<Auction>()
        .Property(a => a.Status)
        .HasDefaultValue("Pending");

    modelBuilder.Entity<Auction>()
        .Property(a => a.IsAutoExtend)
        .HasDefaultValue(true);

    modelBuilder.Entity<Auction>()
        .Property(a => a.AutoExtendMinutes)
        .HasDefaultValue(5);

    modelBuilder.Entity<User>()
        .Property(u => u.IsActive)
        .HasDefaultValue(true);

    modelBuilder.Entity<User>()
        .Property(u => u.CreatedDate)
        .HasDefaultValueSql("GETDATE()");

    modelBuilder.Entity<Notification>()
        .Property(n => n.IsRead)
        .HasDefaultValue(false);

    modelBuilder.Entity<Notification>()
        .Property(n => n.CreatedDate)
        .HasDefaultValueSql("GETDATE()");

    modelBuilder.Entity<Product>()
        .Property(p => p.CreatedDate)
        .HasDefaultValueSql("GETDATE()");

    modelBuilder.Entity<Bid>()
        .Property(b => b.BidTime)
        .HasDefaultValueSql("GETDATE()");

    // Indexes
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

    modelBuilder.Entity<User>()
        .HasIndex(u => u.Username)
        .IsUnique();

    modelBuilder.Entity<Product>()
        .HasIndex(p => p.Title);

    modelBuilder.Entity<Auction>()
        .HasIndex(a => a.StartDate);

    modelBuilder.Entity<Auction>()
        .HasIndex(a => a.EndDate);

    modelBuilder.Entity<Bid>()
        .HasIndex(b => b.BidTime);
}
    }
}