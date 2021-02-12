using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hahn.ApplicationProcess.December2020.Data;
using Hahn.ApplicationProcess.December2020.Domain;
using Light.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicants
{
    public sealed class EfGetApplicantsSession : EfReadOnlySession, IGetApplicantsSession
    {
        public EfGetApplicantsSession(DatabaseContext context) : base(context) { }

        public Task<int> GetTotalNumberOfApplicantsAsync(string? searchTerm) =>
            CreateBaseQuery(searchTerm).CountAsync();

        public Task<List<Applicant>> GetApplicantsAsync(int skip, int take, string? searchTerm) =>
            CreateBaseQuery(searchTerm).OrderBy(applicant => applicant.LastName)
                                       .Skip(skip)
                                       .Take(take)
                                       .ToListAsync();

        private IQueryable<Applicant> CreateBaseQuery(string? searchTerm)
        {
            IQueryable<Applicant> query = Context.Applicants;
            if (!searchTerm.IsNullOrWhiteSpace())
                query = ApplyFilter(query, searchTerm);
            return query;
        }

        private static IQueryable<Applicant> ApplyFilter(IQueryable<Applicant> query, string searchTerm) =>
            query.Where(applicant => applicant.FirstName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     applicant.LastName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     applicant.CountryOfOrigin.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase));
    }
}