using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hahn.ApplicationProcess.December2020.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicants
{
    [ApiController]
    [Route("api/applicants")]
    public sealed class GetApplicantsController : ControllerBase
    {
        private readonly Func<IGetApplicantsSession> _createSession;

        public GetApplicantsController(Func<IGetApplicantsSession> createSession) => _createSession = createSession;

        [HttpGet]
        public async Task<ActionResult<List<Applicant>>> GetApplicants(int skip = 0, int take = 30, string? searchTerm = null)
        {
            await using var session = _createSession();
            var applicants = await session.GetApplicantsAsync(skip, take, searchTerm);
            return applicants;
        }
    }
}