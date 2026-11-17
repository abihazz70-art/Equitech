using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Equitech.Domain.Common;

namespace Equitech.Api.Controllers;

[Route("api/v1/notifications")]
public class NotificationsController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<NotificationItemDto>), StatusCodes.Status200OK)]
    public IActionResult ListNotifications([FromQuery] string? status = null)
    {
        var items = new List<NotificationItemDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Type = "InterviewReminder",
                Title = "Upcoming Mock Interview Practice",
                Message = "Scheduled practice for Software Developer role at Novus Cloud Software.",
                Channel = "InApp",
                ScheduledAt = DateTime.UtcNow.AddHours(4),
                Status = "Scheduled",
                IsRead = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                Type = "JobMatchAlert",
                Title = "New Verified Match Found",
                Message = "PaySphere Technologies posted 'Data Analyst - Merchant Analytics' (Match Score: 88%).",
                Channel = "InApp",
                ScheduledAt = DateTime.UtcNow.AddDays(-1),
                Status = "Delivered",
                IsRead = true
            }
        };

        if (!string.IsNullOrEmpty(status))
        {
            items = items.Where(i => i.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        return Ok(items);
    }
}

public class NotificationItemDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("channel")]
    public string Channel { get; set; } = "InApp";

    [JsonPropertyName("scheduled_at")]
    public DateTime ScheduledAt { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Scheduled";

    [JsonPropertyName("is_read")]
    public bool IsRead { get; set; }
}
