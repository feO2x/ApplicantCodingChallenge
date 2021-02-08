using System.Threading.Tasks;
using Hahn.ApplicationProcess.December2020.Data;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.DeleteApplicant
{
    public interface IDeleteApplicantSession : ISession
    {
        ValueTask<Applicant?> GetApplicantAsync(int id);

        void DeleteApplicant(Applicant applicant);
    }
}