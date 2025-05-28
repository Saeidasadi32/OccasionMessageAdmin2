using Microsoft.EntityFrameworkCore;
using OccasionMessageAdmin.Shared.Models;

namespace OccasionMessageAdmin.Web.Data;

public class OccasionDbContext : DbContext
{
    public OccasionDbContext(DbContextOptions<OccasionDbContext> options) : base(options) { }

    public DbSet<Holiday> Holidays { get; set; } = null!;
    public DbSet<CalendarDay> CalendarDays { get; set; } = null!;
    public DbSet<OccasionMessage> OccasionMessages { get; set; } = null!;
    public DbSet<OccasionMessageChannel> OccasionMessageChannels { get; set; } = null!;
    public DbSet<Ad> Ads { get; set; } = null!;
    public DbSet<AdChannel> AdChannels { get; set; } = null!;
    public DbSet<MessageLog> MessageLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.HasKey(e => e.HolidayID);
            entity.Property(e => e.HolidayName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<CalendarDay>(entity =>
        {
            entity.HasKey(e => e.DayId);
            entity.Property(e => e.CalendarType).HasDefaultValue((byte)1);
        });

        modelBuilder.Entity<OccasionMessage>(entity =>
        {
            entity.HasKey(e => e.MessageID);
            entity.Property(e => e.LanguageCode).IsRequired().HasMaxLength(10);
            entity.HasOne(d => d.Holiday)
                .WithMany()
                .HasForeignKey(d => d.HolidayID);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<OccasionMessageChannel>(entity =>
        {
            entity.HasKey(e => e.ChannelMessageID);
            entity.Property(e => e.MessageText).IsRequired();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasOne(d => d.OccasionMessage)
                .WithMany()
                .HasForeignKey(d => d.MessageID);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Ad>(entity =>
        {
            entity.HasKey(e => e.AdID);
            entity.Property(e => e.AdTitle).IsRequired().HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<AdChannel>(entity =>
        {
            entity.HasKey(e => e.AdChannelID);
            entity.Property(e => e.MessageText).IsRequired();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasOne(d => d.Ad)
                .WithMany()
                .HasForeignKey(d => d.AdID);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<MessageLog>(entity =>
        {
            entity.HasKey(e => e.LogID);
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.SentAt).HasDefaultValueSql("GETDATE()");
        });
    }
}