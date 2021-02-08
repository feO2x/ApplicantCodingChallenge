using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.DeleteApplicant
{
    [ApiController]
    [Route("api/applicants/delete")]
    public sealed class DeleteApplicantController : ControllerBase
    {
        public DeleteApplicantController(Func<IDeleteApplicantSession> createSession,
                                         ILogger<DeleteApplicantController> logger)
        {
            CreateSession = createSession;
            Logger = logger;
        }

        private Func<IDeleteApplicantSession> CreateSession { get; }
        private ILogger<DeleteApplicantController> Logger { get; }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicant(int id)
        {
            if (id < 1)
            {
                ModelState.AddModelError(nameof(id), "id must be a positive number");
                return ValidationProblem();
            }

            await using var session = CreateSession();
            var applicant = await session.GetApplicantAsync(id);
            if (applicant == null)
                return NotFound();

            session.DeleteApplicant(applicant);
            try
            {
                await session.SaveChangesAsync();
                Logger.LogInformation("Applicant {@Applicant} was delete successfully", applicant);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Logger.LogWarning(exception, "Could not delete applicant {@Applicant}, likely because a parallel operation deleted the entity at the same time.");
            }
            return NoContent();
        }
    }
}