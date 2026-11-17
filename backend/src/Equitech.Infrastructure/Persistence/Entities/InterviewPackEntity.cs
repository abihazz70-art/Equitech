using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equitech.Infrastructure.Persistence.Entities;

[Table("interview_packs")]
public class InterviewPackEntity
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("user_id")]
    public Guid UserId { get; set; } = Guid.Parse("00000000-0000-0000-0000-000000000001");

    [Column("job_id")]
    [MaxLength(100)]
    public string JobId { get; set; } = string.Empty;

    [Column("profile_id")]
    [MaxLength(100)]
    public string ProfileId { get; set; } = string.Empty;

    [Column("version")]
    public int Version { get; set; } = 1;

    [Column("content_json", TypeName = "jsonb")]
    public string ContentJson { get; set; } = "{}";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
