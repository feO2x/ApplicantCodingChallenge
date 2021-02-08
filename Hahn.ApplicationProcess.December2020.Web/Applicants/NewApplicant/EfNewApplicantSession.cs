using Hahn.ApplicationProcess.December2020.Data;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.NewApplicant
{
    public sealed class EfNewApplicantSession : Session, INewApplicantSession
    {
        public EfNewApplicantSession(DatabaseContext context) : base(context) { }

        public void AddApplicant(Applicant applicant) => Context.Applicants.Add(applicant);
    }
}