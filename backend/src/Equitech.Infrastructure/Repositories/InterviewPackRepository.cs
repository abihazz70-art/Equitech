using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Equitech.Domain.Interfaces;
using Equitech.Domain.Models;
using Equitech.Infrastructure.Persistence;
using Equitech.Infrastructure.Persistence.Entities;

namespace Equitech.Infrastructure.Repositories;

public class InterviewPackRepository : IInterviewPackRepository
{
    private readonly EquitechDbContext? _dbContext;
    private readonly ILogger<InterviewPackRepository> _logger;
    private static readonly ConcurrentDictionary<Guid, InterviewPackDto> _fallbackCache = new();
    private readonly HttpClient _httpClient;
    private readonly string? _supabaseUrl;
    private readonly string? _supabaseKey;

    public InterviewPackRepository(
        ILogger<InterviewPackRepository> logger, 
        EquitechDbContext? dbContext = null,
        HttpClient? httpClient = null)
    {
        _logger = logger;
        _dbContext = dbContext;
        _httpClient = httpClient ?? new HttpClient();

        _supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL") 
            ?? Environment.GetEnvironmentVariable("NEXT_PUBLIC_SUPABASE_URL");
        _supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_KEY") 
            ?? Environment.GetEnvironmentVariable("NEXT_PUBLIC_SUPABASE_PUBLISHABLE_KEY");
    }

    public async Task<InterviewPackDto> SavePackAsync(InterviewPackDto pack, CancellationToken ct = default)
    {
        // 1. In-memory cache
        _fallbackCache[pack.PackId] = pack;
        var json = JsonSerializer.Serialize(pack);

        // 2. Direct EF Core PostgreSQL
        if (_dbContext != null)
        {
            try
            {
                var entity = new InterviewPackEntity
                {
                    Id = pack.PackId,
                    JobId = pack.JobId,
                    ProfileId = pack.ProfileId,
                    ContentJson = json,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.InterviewPacks.Add(entity);
                await _dbContext.SaveChangesAsync(ct);
                _logger.LogInformation("Successfully persisted interview pack {PackId} to Supabase EF Core database.", pack.PackId);
                return pack;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "EF Core write skipped or table not initialized yet. Attempting REST sync if available.");
            }
        }

        // 3. Supabase PostgREST sync (HTTPS Port 443)
        if (!string.IsNullOrWhiteSpace(_supabaseUrl) && !string.IsNullOrWhiteSpace(_supabaseKey))
        {
            try
            {
                var payload = new
                {
                    id = pack.PackId,
                    job_id = pack.JobId,
                    profile_id = pack.ProfileId,
                    version = 1,
                    content_json = pack,
                    created_at = DateTime.UtcNow.ToString("o")
                };

                using var req = new HttpRequestMessage(HttpMethod.Post, $"{_supabaseUrl.TrimEnd('/')}/rest/v1/interview_packs");
                req.Headers.Add("apikey", _supabaseKey);
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _supabaseKey);
                req.Headers.Add("Prefer", "resolution=merge-duplicates");
                req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                var res = await _httpClient.SendAsync(req, ct);
                if (res.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully synced interview pack {PackId} to Supabase via REST API.", pack.PackId);
                }
                else
                {
                    _logger.LogDebug("Supabase REST response: {Status}. Ensure table 'interview_packs' is created in Supabase.", res.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Supabase REST sync exception.");
            }
        }

        return pack;
    }

    public async Task<InterviewPackDto?> GetPackByIdAsync(Guid packId, CancellationToken ct = default)
    {
        if (_fallbackCache.TryGetValue(packId, out var cached))
        {
            return cached;
        }

        if (_dbContext != null)
        {
            try
            {
                var entity = await _dbContext.InterviewPacks.FirstOrDefaultAsync(p => p.Id == packId, ct);
                if (entity != null)
                {
                    var pack = JsonSerializer.Deserialize<InterviewPackDto>(entity.ContentJson);
                    if (pack != null)
                    {
                        _fallbackCache[packId] = pack;
                        return pack;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error reading from Supabase/PostgreSQL for pack {PackId}", packId);
            }
        }

        return null;
    }

    public async Task<IEnumerable<InterviewPackDto>> GetRecentPacksAsync(int count = 10, CancellationToken ct = default)
    {
        if (_dbContext != null)
        {
            try
            {
                var entities = await _dbContext.InterviewPacks
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(count)
                    .ToListAsync(ct);

                var list = new List<InterviewPackDto>();
                foreach (var entity in entities)
                {
                    var pack = JsonSerializer.Deserialize<InterviewPackDto>(entity.ContentJson);
                    if (pack != null) list.Add(pack);
                }
                if (list.Any()) return list;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error reading recent packs from Supabase/PostgreSQL");
            }
        }

        return _fallbackCache.Values.OrderByDescending(p => p.GeneratedAt).Take(count).ToList();
    }
}
