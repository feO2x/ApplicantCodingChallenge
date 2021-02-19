using System;
using System.Threading.Tasks;
using AutoMapper;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Web.ErrorResponses;
using Hahn.ApplicationProcess.December2020.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.NewApplicant
{
    [ApiController]
    [Route("api/applicants/new")]
    public sealed class NewApplicantController : ControllerBase
    {
        public NewApplicantController(NewApplicantDtoValidator validator,
                                      Func<INewApplicantSession> createSession,
                                      IMapper mapper,
                                      ILogger<NewApplicantController> logger)
        {
            Validator = validator;
            CreateSession = createSession;
            Mapper = mapper;
            Logger = logger;
        }

        private NewApplicantDtoValidator Validator { get; }
        private Func<INewApplicantSession> CreateSession { get; }
        private IMapper Mapper { get; }
        private ILogger<NewApplicantController> Logger { get; }

        [HttpPost]
        [ProducesResponseType(typeof(Applicant), 201)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateNewApplicant([FromBody] NewApplicantDto newApplicantDto)
        {
            var badRequestResult = await this.CheckForErrorsAsync(newApplicantDto, Validator);
            if (badRequestResult != null)
                return badRequestResult;

            var applicant = Mapper.Map<NewApplicantDto, Applicant>(newApplicantDto);
            await using var session = CreateSession();
            session.AddApplicant(applicant);
            await session.SaveChangesAsync();
            Logger.LogInformation("A new applicant {@Applicant} was created", applicant);
            return Created("/api/applicants/" + applicant.Id, applicant);
        }
    }
}