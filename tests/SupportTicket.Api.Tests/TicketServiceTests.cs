using Microsoft.EntityFrameworkCore;
using SupportTicket.Api.Data;
using SupportTicket.Api.Models;
using SupportTicket.Api.Services;

namespace SupportTicket.Api.Tests;

public class TicketServiceTests
{
    [Fact]
    public async Task ValidTransitions_ShouldSucceed_And_InvalidTransitions_ShouldBeRejected()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);
        var service = new TicketService(context);

        var created = await service.CreateAsync(new CreateTicketRequest
        {
            Title = "Login issue",
            Description = "User cannot sign in",
            Priority = "High",
            AssignedTo = "Asha"
        });

        Assert.True(created.Success);
        Assert.Equal(TicketStatus.Open, created.Value!.Status);

        var progressed = await service.UpdateStatusAsync(created.Value.Id, TicketStatus.InProgress);
        Assert.True(progressed.Success);
        Assert.Equal(TicketStatus.InProgress, progressed.Value!.Status);

        var resolved = await service.UpdateStatusAsync(created.Value.Id, TicketStatus.Resolved);
        Assert.True(resolved.Success);
        Assert.Equal(TicketStatus.Resolved, resolved.Value!.Status);

        var invalid = await service.UpdateStatusAsync(created.Value.Id, TicketStatus.Open);
        Assert.False(invalid.Success);
        Assert.Contains("Invalid transition", invalid.Error);
    }

    [Fact]
    public async Task CreateAsync_ShouldReject_WhitespaceTitleAndDescription()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);
        var service = new TicketService(context);

        var result = await service.CreateAsync(new CreateTicketRequest
        {
            Title = "   ",
            Description = " ",
            Priority = "High"
        });

        Assert.False(result.Success);
        Assert.Contains("Title", result.Error);
    }
}
