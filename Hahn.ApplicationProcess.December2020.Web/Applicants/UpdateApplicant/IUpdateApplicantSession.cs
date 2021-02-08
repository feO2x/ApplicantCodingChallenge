using Hahn.ApplicationProcess.December2020.Data;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.UpdateApplicant
{
    public interface IUpdateApplicantSession : ISession
    {
        void UpdateApplicant(Applicant applicant);
    }
}