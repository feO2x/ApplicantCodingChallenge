using System.Collections.Generic;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicants
{
    public sealed class ApplicantsPageDto
    {
        public ApplicantsPageDto(int totalNumberOfApplicants, List<Applicant> applicants)
        {
            TotalNumberOfApplicants = totalNumberOfApplicants;
            Applicants = applicants;
        }

        public int TotalNumberOfApplicants { get; }

        public List<Applicant> Applicants { get; }
    }
}