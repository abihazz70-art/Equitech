using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Equitech.Domain.Common;

namespace Equitech.Api.Controllers;

[Route("api/v1/cvs")]
public class CvsController : BaseApiController
{
    private static readonly Dictionary<Guid, CvSessionState> _sessions = new();

    [HttpPost]
    [ProducesResponseType(typeof(CvUploadSessionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult CreateUploadSession([FromBody] CreateCvUploadSessionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            return BadRequestProblem("Field 'fileName' is required.", ErrorCodes.InvalidPayload);
        }

        var allowedExtensions = new[] { ".pdf", ".docx" };
        var ext = Path.GetExtension(request.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(ext))
        {
            return BadRequestProblem($"Unsupported file format '{ext}'. Allowed formats: PDF, DOCX.", ErrorCodes.InvalidPayload);
        }

        if (request.FileSizeBytes <= 0 || request.FileSizeBytes > 15 * 1024 * 1024)
        {
            return BadRequestProblem("File size must be between 1 byte and 15 MB.", ErrorCodes.InvalidPayload);
        }

        var sessionId = Guid.NewGuid();
        var uploadUrl = $"https://storage.supabase.co/storage/v1/upload/cvs/{sessionId}?token={Guid.NewGuid():N}";

        _sessions[sessionId] = new CvSessionState
        {
            Id = sessionId,
            FileName = request.FileName,
            MimeType = request.MimeType ?? "application/octet-stream",
            Size = request.FileSizeBytes,
            Status = "AwaitingUpload",
            CreatedAt = DateTime.UtcNow
        };

        var response = new CvUploadSessionResponse
        {
            SessionId = sessionId,
            UploadUrl = uploadUrl,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            MaxSizeBytes = 15 * 1024 * 1024
        };

        return Created($"/api/v1/cvs/{sessionId}", response);
    }

    [HttpPost("{id:guid}/process")]
    [ProducesResponseType(typeof(CvProcessingStatusResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public IActionResult StartCvProcessing(Guid id)
    {
        if (!_sessions.TryGetValue(id, out var session))
        {
            return NotFoundProblem($"CV session '{id}' does not exist.", ErrorCodes.ResourceNotFound);
        }

        session.Status = "Processing";
        session.ProcessedAt = DateTime.UtcNow;

        return Accepted(new CvProcessingStatusResponse
        {
            CvId = id,
            Status = "Processing",
            Message = "CV parsing and OCR text extraction initiated.",
            EstimatedCompletionSeconds = 4
        });
    }
}

public class CreateCvUploadSessionRequest
{
    [Required]
    [JsonPropertyName("file_name")]
    public string FileName { get; set; } = string.Empty;

    [JsonPropertyName("mime_type")]
    public string? MimeType { get; set; }

    [Required]
    [JsonPropertyName("file_size_bytes")]
    public long FileSizeBytes { get; set; }
}

public class CvUploadSessionResponse
{
    [JsonPropertyName("session_id")]
    public Guid SessionId { get; set; }

    [JsonPropertyName("upload_url")]
    public string UploadUrl { get; set; } = string.Empty;

    [JsonPropertyName("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [JsonPropertyName("max_size_bytes")]
    public long MaxSizeBytes { get; set; }
}

public class CvProcessingStatusResponse
{
    [JsonPropertyName("cv_id")]
    public Guid CvId { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Processing";

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("estimated_completion_seconds")]
    public int EstimatedCompletionSeconds { get; set; } = 5;
}

public class CvSessionState
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public long Size { get; set; }
    public string Status { get; set; } = "Created";
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}
