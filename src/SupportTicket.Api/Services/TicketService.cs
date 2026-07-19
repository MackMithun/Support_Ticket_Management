using Microsoft.EntityFrameworkCore;
using SupportTicket.Api.Data;
using SupportTicket.Api.Models;

namespace SupportTicket.Api.Services;

public class TicketService
{
    private readonly AppDbContext _context;

    public TicketService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ticket>> GetAllAsync(string? search = null, TicketStatus? status = null)
    {
        var query = _context.Tickets.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
        }

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        return await query.OrderByDescending(t => t.CreatedAt).Include(t => t.Comments).ToListAsync();
    }

    public async Task<Ticket?> GetByIdAsync(int id)
    {
        return await _context.Tickets.Include(t => t.Comments).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<ServiceResult<Ticket>> CreateAsync(CreateTicketRequest request)
    {
        var validationError = ValidateCreateRequest(request);
        if (validationError is not null)
        {
            return ServiceResult<Ticket>.Fail(validationError);
        }

        var ticket = new Ticket
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Priority = request.Priority.Trim(),
            AssignedTo = request.AssignedTo?.Trim(),
            CreatedBy = "Analyst"
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        return ServiceResult<Ticket>.Ok(ticket);
    }

    public async Task<ServiceResult<Ticket>> UpdateAsync(int id, UpdateTicketRequest request)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket is null)
        {
            return ServiceResult<Ticket>.Fail("Ticket not found.");
        }

        var validationError = ValidateUpdateRequest(request);
        if (validationError is not null)
        {
            return ServiceResult<Ticket>.Fail(validationError);
        }

        if (!string.IsNullOrWhiteSpace(request.Title)) ticket.Title = request.Title.Trim();
        if (!string.IsNullOrWhiteSpace(request.Description)) ticket.Description = request.Description.Trim();
        if (!string.IsNullOrWhiteSpace(request.Priority)) ticket.Priority = request.Priority.Trim();
        if (request.AssignedTo is not null) ticket.AssignedTo = request.AssignedTo.Trim();

        ticket.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return ServiceResult<Ticket>.Ok(ticket);
    }

    public async Task<ServiceResult<Ticket>> UpdateStatusAsync(int id, TicketStatus newStatus)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket is null)
        {
            return ServiceResult<Ticket>.Fail("Ticket not found.");
        }

        if (!IsValidTransition(ticket.Status, newStatus))
        {
            return ServiceResult<Ticket>.Fail($"Invalid transition from {ticket.Status} to {newStatus}.");
        }

        ticket.Status = newStatus;
        ticket.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return ServiceResult<Ticket>.Ok(ticket);
    }

    public async Task<ServiceResult<Comment>> AddCommentAsync(int id, CreateCommentRequest request)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket is null)
        {
            return ServiceResult<Comment>.Fail("Ticket not found.");
        }

        if (string.IsNullOrWhiteSpace(request.Message) || string.IsNullOrWhiteSpace(request.CreatedBy))
        {
            return ServiceResult<Comment>.Fail("Comment message and author are required.");
        }

        var comment = new Comment
        {
            TicketId = id,
            Message = request.Message.Trim(),
            CreatedBy = request.CreatedBy.Trim()
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return ServiceResult<Comment>.Ok(comment);
    }

    private static string? ValidateCreateRequest(CreateTicketRequest request)
    {
        if (request is null)
        {
            return "Request body is required.";
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return "Title is required.";
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            return "Description is required.";
        }

        if (string.IsNullOrWhiteSpace(request.Priority))
        {
            return "Priority is required.";
        }

        return null;
    }

    private static string? ValidateUpdateRequest(UpdateTicketRequest request)
    {
        if (request is null)
        {
            return "Request body is required.";
        }

        if (request.Title is not null && string.IsNullOrWhiteSpace(request.Title))
        {
            return "Title cannot be empty.";
        }

        if (request.Description is not null && string.IsNullOrWhiteSpace(request.Description))
        {
            return "Description cannot be empty.";
        }

        if (request.Priority is not null && string.IsNullOrWhiteSpace(request.Priority))
        {
            return "Priority cannot be empty.";
        }

        return null;
    }

    private static bool IsValidTransition(TicketStatus current, TicketStatus next)
    {
        return (current, next) switch
        {
            (TicketStatus.Open, TicketStatus.InProgress) => true,
            (TicketStatus.Open, TicketStatus.Cancelled) => true,
            (TicketStatus.InProgress, TicketStatus.Resolved) => true,
            (TicketStatus.InProgress, TicketStatus.Cancelled) => true,
            (TicketStatus.Resolved, TicketStatus.Closed) => true,
            _ => false
        };
    }
}
