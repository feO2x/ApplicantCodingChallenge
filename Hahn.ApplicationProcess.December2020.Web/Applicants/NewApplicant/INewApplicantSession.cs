using Hahn.ApplicationProcess.December2020.Data;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.NewApplicant
{
    public interface INewApplicantSession : ISession
    {
        void AddApplicant(Applicant applicant);
    }
}