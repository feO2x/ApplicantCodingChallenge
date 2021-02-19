using System;
using System.Threading.Tasks;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Web.ErrorResponses;
using Hahn.ApplicationProcess.December2020.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.UpdateApplicant
{
    [ApiController]
    [Route("api/applicants/update")]
    public sealed class UpdateApplicantController : ControllerBase
    {
        public UpdateApplicantController(ApplicantValidator validator,
                                         Func<IUpdateApplicantSession> createSession,
                                         ILogger<UpdateApplicantController> logger)
        {
            Validator = validator;
            CreateSession = createSession;
            Logger = logger;
        }

        private ApplicantValidator Validator { get; }
        private Func<IUpdateApplicantSession> CreateSession { get; }
        private ILogger<UpdateApplicantController> Logger { get; }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        [ProducesResponseType(typeof(NotFoundResponse), 404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateApplicant([FromBody] Applicant applicant)
        {
            var badRequestResult = await this.CheckForErrorsAsync(applicant, Validator);
            if (badRequestResult != null)
                return badRequestResult;

            await using var session = CreateSession();
            session.UpdateApplicant(applicant);
            try
            {
                await session.SaveChangesAsync();
                Logger.LogInformation("Applicant {@Applicant} was updated successfully", applicant);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }
    }
}