using System;
using System.Threading.Tasks;
using Hahn.ApplicationProcess.December2020.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicant
{
    [ApiController]
    [Route("api/applicants")]
    public sealed class GetApplicantController : ControllerBase
    {
        public GetApplicantController(Func<IGetApplicantSession> createSession) => CreateSession = createSession;

        private Func<IGetApplicantSession> CreateSession { get; }

        [HttpGet("{id}")]
        public async Task<ActionResult<Applicant>> GetApplicant(int id)
        {
            if (id < 1)
            {
                ModelState.AddModelError("id", "The id must be positive.");
                return ValidationProblem();
            }

            await using var session = CreateSession();
            var applicant = await session.GetApplicantAsync(id);
            if (applicant == null)
                return NotFound();
            return applicant;
        }
    }
}