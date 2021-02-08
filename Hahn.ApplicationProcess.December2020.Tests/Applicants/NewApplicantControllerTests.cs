using Hahn.ApplicationProcess.December2020.Tests.TestHelpers;
using Hahn.ApplicationProcess.December2020.Web.Applicants.NewApplicant;

namespace Hahn.ApplicationProcess.December2020.Tests.Applicants
{
    public sealed class NewApplicantControllerTests : WebApiControllerTests<NewApplicantController>
    {
        public NewApplicantControllerTests() : base("api/applicants/new") { }
    }
}