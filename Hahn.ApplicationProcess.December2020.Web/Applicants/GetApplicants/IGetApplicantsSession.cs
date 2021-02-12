using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicants
{
    public interface IGetApplicantsSession : IAsyncDisposable
    {
        Task<int> GetTotalNumberOfApplicantsAsync(string? searchTerm);
        Task<List<Applicant>> GetApplicantsAsync(int skip, int take, string? searchTerm);
    }
}