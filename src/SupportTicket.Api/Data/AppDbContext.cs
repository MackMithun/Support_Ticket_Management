using Microsoft.EntityFrameworkCore;
using SupportTicket.Api.Models;

namespace SupportTicket.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>()
            .HasMany(t => t.Comments)
            .WithOne()
            .HasForeignKey(c => c.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Ticket>().HasData(
            new Ticket
            {
                Id = 1,
                Title = "VPN access issue",
                Description = "Sales team cannot connect to the VPN from home office.",
                Priority = "High",
                Status = TicketStatus.Open,
                AssignedTo = "Mina",
                CreatedBy = "System",
                CreatedAt = new DateTime(2026, 7, 18, 8, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 7, 18, 8, 0, 0, DateTimeKind.Utc)
            },
            new Ticket
            {
                Id = 2,
                Title = "Invoice export problem",
                Description = "Export to PDF is failing for invoices with discounts.",
                Priority = "Medium",
                Status = TicketStatus.InProgress,
                AssignedTo = "Jordan",
                CreatedBy = "System",
                CreatedAt = new DateTime(2026, 7, 19, 9, 30, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 7, 19, 9, 30, 0, DateTimeKind.Utc)
            });

        modelBuilder.Entity<Comment>().HasData(
            new Comment
            {
                Id = 1,
                TicketId = 1,
                Message = "We are validating the VPN profile settings.",
                CreatedBy = "Mina",
                CreatedAt = new DateTime(2026, 7, 18, 8, 15, 0, DateTimeKind.Utc)
            });
    }
}
