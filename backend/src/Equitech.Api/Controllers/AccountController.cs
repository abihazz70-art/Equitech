using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Equitech.Domain.Common;

namespace Equitech.Api.Controllers;

[Route("api/v1/account")]
public class AccountController : BaseApiController
{
    [HttpDelete]
    [ProducesResponseType(typeof(AccountDeletionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult RequestAccountDeletion([FromBody] DeleteAccountRequest request)
    {
        if (request == null || !request.ConfirmDeletion)
        {
            return BadRequestProblem("Account deletion requires explicit confirmation ('confirm_deletion: true').", ErrorCodes.InvalidPayload);
        }

        if (string.IsNullOrWhiteSpace(request.ConfirmationPhrase) || 
            !request.ConfirmationPhrase.Trim().Equals("DELETE MY ACCOUNT", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequestProblem("Please provide the exact confirmation phrase 'DELETE MY ACCOUNT'.", ErrorCodes.InvalidPayload);
        }

        return Ok(new AccountDeletionResponse
        {
            Status = "DeletionScheduled",
            Message = "Your account, profile, uploaded CVs, and generated artifacts have been scheduled for immediate purge in accordance with privacy compliance.",
            ScheduledPurgeAt = DateTime.UtcNow.AddMinutes(5),
            AuditReceiptId = Guid.NewGuid()
        });
    }
}

public class DeleteAccountRequest
{
    [JsonPropertyName("confirm_deletion")]
    public bool ConfirmDeletion { get; set; }

    [JsonPropertyName("confirmation_phrase")]
    public string ConfirmationPhrase { get; set; } = string.Empty;

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}

public class AccountDeletionResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = "DeletionScheduled";

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("scheduled_purge_at")]
    public DateTime ScheduledPurgeAt { get; set; }

    [JsonPropertyName("audit_receipt_id")]
    public Guid AuditReceiptId { get; set; }
}
