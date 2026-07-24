using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SupportTicket.Api.Models;

namespace SupportTicket.Api.Tests;

public class TicketsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TicketsControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("environment", "Testing");
        }).CreateClient();
    }

    [Fact]
    public async Task PostTicket_ValidRequest_Returns201()
    {
        var response = await _client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest
        {
            Title = "Integration test ticket",
            Description = "Created via WebApplicationFactory",
            Priority = "High",
            AssignedTo = "Asha"
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var ticket = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.NotNull(ticket);
        Assert.Equal("Integration test ticket", ticket.Title);
        Assert.Equal(TicketStatus.Open, ticket.Status);
    }

    [Fact]
    public async Task PostTicket_EmptyTitle_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest
        {
            Title = "",
            Description = "Missing title",
            Priority = "Low"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetTicketById_Existing_Returns200()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest
        {
            Title = "Detail view test",
            Description = "Fetch by id",
            Priority = "Medium"
        });
        var created = await createResponse.Content.ReadFromJsonAsync<Ticket>();

        var response = await _client.GetAsync($"/api/tickets/{created!.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var ticket = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.Equal("Detail view test", ticket!.Title);
    }

    [Fact]
    public async Task PutTicket_UpdatesFields_Returns200()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest
        {
            Title = "Original title",
            Description = "Original description",
            Priority = "Low"
        });
        var created = await createResponse.Content.ReadFromJsonAsync<Ticket>();

        var response = await _client.PutAsJsonAsync($"/api/tickets/{created!.Id}", new UpdateTicketRequest
        {
            Title = "Updated title",
            Priority = "High",
            AssignedTo = "Jordan"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.Equal("Updated title", updated!.Title);
        Assert.Equal("High", updated.Priority);
        Assert.Equal("Jordan", updated.AssignedTo);
    }

    [Fact]
    public async Task GetTickets_SearchFilter_ReturnsMatching()
    {
        await _client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest
        {
            Title = "UniqueSearchTermXYZ",
            Description = "Search test",
            Priority = "Low"
        });

        var response = await _client.GetAsync("/api/tickets?search=UniqueSearchTermXYZ");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var tickets = await response.Content.ReadFromJsonAsync<List<Ticket>>();
        Assert.Contains(tickets!, t => t.Title.Contains("UniqueSearchTermXYZ"));
    }

    [Fact]
    public async Task PatchStatus_OpenToInProgress_Returns200()
    {
        var ticket = await CreateTicket();
        var response = await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.InProgress);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.Equal(TicketStatus.InProgress, updated!.Status);
    }

    [Fact]
    public async Task PatchStatus_InProgressToResolved_Returns200()
    {
        var ticket = await CreateTicket();
        await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.InProgress);
        var response = await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.Resolved);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.Equal(TicketStatus.Resolved, updated!.Status);
    }

    [Fact]
    public async Task PatchStatus_ResolvedToClosed_Returns200()
    {
        var ticket = await CreateTicket();
        await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.InProgress);
        await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.Resolved);
        var response = await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.Closed);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.Equal(TicketStatus.Closed, updated!.Status);
    }

    [Fact]
    public async Task PatchStatus_OpenToCancelled_Returns200()
    {
        var ticket = await CreateTicket();
        var response = await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.Cancelled);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.Equal(TicketStatus.Cancelled, updated!.Status);
    }

    [Fact]
    public async Task PatchStatus_InProgressToCancelled_Returns200()
    {
        var ticket = await CreateTicket();
        await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.InProgress);
        var response = await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.Cancelled);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.Equal(TicketStatus.Cancelled, updated!.Status);
    }

    [Fact]
    public async Task PatchStatus_InvalidTransition_Returns400()
    {
        var ticket = await CreateTicket();
        await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.InProgress);
        await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.Resolved);

        var invalidResponse = await _client.PatchAsJsonAsync(
            $"/api/tickets/{ticket.Id}/status",
            TicketStatus.Open);

        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);
    }

    [Fact]
    public async Task PatchStatus_ClosedToAny_Returns400()
    {
        var ticket = await CreateTicket();
        await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.InProgress);
        await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.Resolved);
        await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.Closed);

        var response = await _client.PatchAsJsonAsync($"/api/tickets/{ticket.Id}/status", TicketStatus.Open);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostComment_ValidRequest_Returns200()
    {
        var ticket = await CreateTicket();
        var response = await _client.PostAsJsonAsync($"/api/tickets/{ticket.Id}/comments", new CreateCommentRequest
        {
            Message = "Integration test comment",
            CreatedBy = "Mina"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var comment = await response.Content.ReadFromJsonAsync<Comment>();
        Assert.Equal("Integration test comment", comment!.Message);
    }

    [Fact]
    public async Task GetTickets_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/tickets");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var tickets = await response.Content.ReadFromJsonAsync<List<Ticket>>();
        Assert.NotNull(tickets);
    }

    [Fact]
    public async Task GetUsers_ReturnsSeededUsers()
    {
        var response = await _client.GetAsync("/api/users");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var users = await response.Content.ReadFromJsonAsync<List<User>>();
        Assert.NotNull(users);
        Assert.True(users.Count >= 4);
    }

    private async Task<Ticket> CreateTicket()
    {
        var response = await _client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest
        {
            Title = $"Ticket {Guid.NewGuid():N}",
            Description = "State machine test",
            Priority = "Medium"
        });
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<Ticket>())!;
    }
}
