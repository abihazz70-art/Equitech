using Equitech.Domain.Models;

namespace Equitech.Domain.Interfaces;

public interface IInterviewPackRepository
{
    Task<InterviewPackDto> SavePackAsync(InterviewPackDto pack, CancellationToken ct = default);
    Task<InterviewPackDto?> GetPackByIdAsync(Guid packId, CancellationToken ct = default);
    Task<IEnumerable<InterviewPackDto>> GetRecentPacksAsync(int count = 10, CancellationToken ct = default);
}

public interface ISampleDataProvider
{
    IReadOnlyList<JobDescriptionDto> GetSampleJobs();
    IReadOnlyList<CandidateProfileDto> GetSampleCandidates();
    (JobDescriptionDto Job, CandidateProfileDto Candidate)? GetPair(string roleKey);
}
