namespace Equitech.Domain.Common;

public static class ErrorCodes
{
    public const string InvalidPayload = "ERR_INVALID_PAYLOAD";
    public const string ResourceNotFound = "ERR_RESOURCE_NOT_FOUND";
    public const string UnauthorizedAccess = "ERR_UNAUTHORIZED";
    public const string ForbiddenAccess = "ERR_FORBIDDEN";
    public const string Conflict = "ERR_CONFLICT";
    public const string ApprovalRequired = "ERR_APPROVAL_REQUIRED";
    public const string ApprovalExpired = "ERR_APPROVAL_EXPIRED";
    public const string QuotaExceeded = "ERR_QUOTA_EXCEEDED";
    public const string InternalServerError = "ERR_INTERNAL_SERVER_ERROR";
}
