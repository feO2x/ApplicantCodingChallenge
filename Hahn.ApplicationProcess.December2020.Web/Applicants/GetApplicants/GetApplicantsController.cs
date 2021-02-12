using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Web.Infrastructure;
using Hahn.ApplicationProcess.December2020.Web.Paging;
using Microsoft.AspNetCore.Mvc;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicants
{
    [ApiController]
    [Route("api/applicants")]
    public sealed class GetApplicantsController : ControllerBase
    {
        public GetApplicantsController(PageDtoValidator validator, Func<IGetApplicantsSession> createSession)
        {
            Validator = validator;
            CreateSession = createSession;
        }

        private PageDtoValidator Validator { get; }

        private Func<IGetApplicantsSession> CreateSession { get; }


        [HttpGet]
        [ProducesResponseType(typeof(List<Applicant>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApplicantsPageDto>> GetApplicants([FromQuery] PageDto pageDto)
        {
            if (this.CheckForErrors(pageDto, Validator, out var badRequestResult))
                return badRequestResult;

            await using var session = CreateSession();
            var totalNumberOfApplicants = await session.GetTotalNumberOfApplicantsAsync(pageDto.SearchTerm);
            List<Applicant> applicants;
            if (totalNumberOfApplicants > pageDto.Skip)
                applicants = await session.GetApplicantsAsync(pageDto.Skip, pageDto.Take, pageDto.SearchTerm);
            else
                applicants = new List<Applicant>(0);
            return new ApplicantsPageDto(totalNumberOfApplicants, applicants);
        }
    }
}