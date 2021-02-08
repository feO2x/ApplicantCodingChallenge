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

        public Task<List<Applicant>> GetApplicantsAsync(int skip, int take, string? searchTerm)
        {
            IQueryable<Applicant> query = Context.Applicants;
            if (!searchTerm.IsNullOrWhiteSpace())
            {
                query = query.Where(applicant => applicant.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                 applicant.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                 applicant.EmailAddress.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                 applicant.CountryOfOrigin.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            return query.OrderBy(applicant => applicant.LastName)
                        .Skip(skip)
                        .Take(take)
                        .ToListAsync();
        }
    }
}