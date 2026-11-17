using Microsoft.AspNetCore.Mvc;
using Equitech.Domain.Interfaces;
using Equitech.Domain.Models;

namespace Equitech.Api.Controllers;

[ApiController]
[Route("api/v1/sample-data")]
public class SampleDataController : ControllerBase
{
    private readonly ISampleDataProvider _dataProvider;

    public SampleDataController(ISampleDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    [HttpGet("jobs")]
    public ActionResult<IReadOnlyList<JobDescriptionDto>> GetJobs()
    {
        return Ok(_dataProvider.GetSampleJobs());
    }

    [HttpGet("candidates")]
    public ActionResult<IReadOnlyList<CandidateProfileDto>> GetCandidates()
    {
        return Ok(_dataProvider.GetSampleCandidates());
    }

    [HttpGet("pair/{roleKey}")]
    public IActionResult GetPair(string roleKey)
    {
        var pair = _dataProvider.GetPair(roleKey);
        if (pair == null)
        {
            return NotFound(new { message = $"No sample pair found for key: {roleKey}" });
        }

        return Ok(new
        {
            job = pair.Value.Job,
            candidate = pair.Value.Candidate
        });
    }
}
