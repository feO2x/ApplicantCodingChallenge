using System.Threading.Tasks;
using Hahn.ApplicationProcess.December2020.Data;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.DeleteApplicant
{
    public sealed class EfDeleteApplicantSession : EfSession, IDeleteApplicantSession
    {
        public EfDeleteApplicantSession(DatabaseContext context) : base(context) { }
        public ValueTask<Applicant?> GetApplicantAsync(int id) => Context.FindAsync<Applicant?>(id);

        public void DeleteApplicant(Applicant applicant) => Context.Applicants.Remove(applicant);
    }
}