using System.Threading.Tasks;
using Hahn.ApplicationProcess.December2020.Data;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicant
{
    public sealed class EfGetApplicantSession : EfReadOnlySession, IGetApplicantSession
    {
        public EfGetApplicantSession(DatabaseContext context) : base(context) { }

        public ValueTask<Applicant?> GetApplicantAsync(int id) =>
#nullable disable
            Context.Applicants.FindAsync(id);
#nullable restore
    }
}