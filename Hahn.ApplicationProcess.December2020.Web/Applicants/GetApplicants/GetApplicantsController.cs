using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Web.Paging;
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
        [ProducesResponseType(typeof(List<Applicant>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<Applicant>>> GetApplicants([FromQuery] PageDto pageDto)
        {
            await using var session = _createSession();
            var applicants = await session.GetApplicantsAsync(pageDto.Skip, pageDto.Take, pageDto.SearchTerm);
            return applicants;
        }
    }
}