using Microsoft.AspNetCore.Mvc;
using SupportTicket.Api.Models;
using SupportTicket.Api.Services;
using SupportTicket.Api.Data;

namespace SupportTicket.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly TicketService _service;

    public TicketsController(AppDbContext context)
    {
        _service = new TicketService(context);
    }

    [HttpGet]
    public async Task<ActionResult<List<Ticket>>> Get([FromQuery] string? search, [FromQuery] string? status)
    {
        TicketStatus? parsedStatus = null;
        if (Enum.TryParse<TicketStatus>(status, true, out var parsed))
        {
            parsedStatus = parsed;
        }

        var tickets = await _service.GetAllAsync(search, parsedStatus);
        return Ok(tickets);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Ticket>> GetById(int id)
    {
        var ticket = await _service.GetByIdAsync(id);
        return ticket is null ? NotFound() : Ok(ticket);
    }

    [HttpPost]
    public async Task<ActionResult<Ticket>> Create([FromBody] CreateTicketRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var result = await _service.CreateAsync(request);
        if (!result.Success)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Ticket>> Update(int id, [FromBody] UpdateTicketRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var result = await _service.UpdateAsync(id, request);
        if (!result.Success)
        {
            return result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true
                ? NotFound()
                : BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<Ticket>> UpdateStatus(int id, [FromBody] TicketStatus newStatus)
    {
        var result = await _service.UpdateStatusAsync(id, newStatus);
        if (!result.Success)
        {
            return result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true
                ? NotFound()
                : BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("{id:int}/comments")]
    public async Task<ActionResult<Comment>> AddComment(int id, [FromBody] CreateCommentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var result = await _service.AddCommentAsync(id, request);
        if (!result.Success)
        {
            return result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true
                ? NotFound()
                : BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
