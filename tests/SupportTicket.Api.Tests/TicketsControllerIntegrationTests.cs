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
            AssignedTo = "TestUser"
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
    public async Task PatchStatus_ValidTransition_Returns200()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest
        {
            Title = "Status transition test",
            Description = "Testing PATCH status",
            Priority = "Medium"
        });
        var created = await createResponse.Content.ReadFromJsonAsync<Ticket>();

        var patchResponse = await _client.PatchAsJsonAsync(
            $"/api/tickets/{created!.Id}/status",
            TicketStatus.InProgress);

        Assert.Equal(HttpStatusCode.OK, patchResponse.StatusCode);
        var updated = await patchResponse.Content.ReadFromJsonAsync<Ticket>();
        Assert.Equal(TicketStatus.InProgress, updated!.Status);
    }

    [Fact]
    public async Task PatchStatus_InvalidTransition_Returns400()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest
        {
            Title = "Invalid transition test",
            Description = "Will try Resolved to Open",
            Priority = "Low"
        });
        var created = await createResponse.Content.ReadFromJsonAsync<Ticket>();

        await _client.PatchAsJsonAsync($"/api/tickets/{created!.Id}/status", TicketStatus.InProgress);
        await _client.PatchAsJsonAsync($"/api/tickets/{created.Id}/status", TicketStatus.Resolved);

        var invalidResponse = await _client.PatchAsJsonAsync(
            $"/api/tickets/{created.Id}/status",
            TicketStatus.Open);

        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);
    }

    [Fact]
    public async Task GetTickets_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/tickets");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var tickets = await response.Content.ReadFromJsonAsync<List<Ticket>>();
        Assert.NotNull(tickets);
    }
}
