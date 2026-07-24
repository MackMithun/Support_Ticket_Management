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
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Mina", Email = "mina@company.com", Role = UserRole.Agent },
            new User { Id = 2, Name = "Jordan", Email = "jordan@company.com", Role = UserRole.Agent },
            new User { Id = 3, Name = "Asha", Email = "asha@company.com", Role = UserRole.Analyst },
            new User { Id = 4, Name = "Admin", Email = "admin@company.com", Role = UserRole.Admin });

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
