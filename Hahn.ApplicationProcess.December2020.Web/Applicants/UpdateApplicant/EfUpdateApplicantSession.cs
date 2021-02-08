using Hahn.ApplicationProcess.December2020.Data;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.UpdateApplicant
{
    public sealed class EfUpdateApplicantSession : EfSession, IUpdateApplicantSession
    {
        public EfUpdateApplicantSession(DatabaseContext context) : base(context) { }

        public void UpdateApplicant(Applicant applicant) => Context.Update(applicant);
    }
}