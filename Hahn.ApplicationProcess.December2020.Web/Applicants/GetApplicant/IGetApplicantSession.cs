using System;
using System.Threading.Tasks;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicant
{
    public interface IGetApplicantSession : IAsyncDisposable
    {
        ValueTask<Applicant?> GetApplicantAsync(int id);
    }
}