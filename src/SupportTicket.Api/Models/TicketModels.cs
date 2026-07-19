using System.ComponentModel.DataAnnotations;

namespace SupportTicket.Api.Models;

public enum TicketStatus
{
    Open,
    InProgress,
    Resolved,
    Closed,
    Cancelled
}

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public string? AssignedTo { get; set; }
    public string CreatedBy { get; set; } = "System";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public List<Comment> Comments { get; set; } = new();
}

public class Comment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class CreateTicketRequest
{
    [Required]
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Priority { get; set; } = "Medium";

    public string? AssignedTo { get; set; }
}

public class UpdateTicketRequest
{
    [StringLength(120)]
    public string? Title { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    public string? Priority { get; set; }

    public string? AssignedTo { get; set; }
}

public class CreateCommentRequest
{
    [Required]
    [StringLength(2000)]
    public string Message { get; set; } = string.Empty;

    [Required]
    public string CreatedBy { get; set; } = string.Empty;
}

public class ServiceResult<T>
{
    public bool Success { get; set; }
    public T? Value { get; set; }
    public string? Error { get; set; }

    public static ServiceResult<T> Ok(T value) => new() { Success = true, Value = value };
    public static ServiceResult<T> Fail(string error) => new() { Success = false, Error = error };
}
