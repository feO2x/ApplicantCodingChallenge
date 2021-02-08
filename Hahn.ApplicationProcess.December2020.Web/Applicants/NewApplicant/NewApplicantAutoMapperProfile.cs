using AutoMapper;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.NewApplicant
{
    public sealed class NewApplicantAutoMapperProfile : Profile
    {
        public NewApplicantAutoMapperProfile()
        {
            CreateMap<NewApplicantDto, Applicant>();
        }
    }
}