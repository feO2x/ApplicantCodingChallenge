using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Tests.TestHelpers;
using Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicants;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Hahn.ApplicationProcess.December2020.Tests.Applicants
{
    public sealed class GetApplicantsControllerTests : WebApiControllerTests<GetApplicantsController>
    {
        public GetApplicantsControllerTests() : base("api/applicants") { }

        [Fact]
        public static void GetApplicantsMustBeDecoratedWithHttpGetAttribute() =>
            ControllerType.GetMethod(nameof(GetApplicantsController.GetApplicants)).Should().BeDecoratedWith<HttpGetAttribute>();

        [Theory]
        [InlineData(0, 20)]
        [InlineData(20, 30)]
        [InlineData(90, 25)]
        public static async Task ReturnApplicantsOnValidRequest(int skip, int take)
        {
            var session = new GetApplicantsSessionMock(100);
            var controller = new GetApplicantsController(() => session);

            var actionResult = await controller.GetApplicants(skip, take);

            var expectedApplicants = session.Applicants.Skip(skip).Take(take).ToList();
            actionResult.Value.Should().Equal(expectedApplicants);
        }

        private sealed class GetApplicantsSessionMock : BaseReadOnlySessionMock<GetApplicantsSessionMock>, IGetApplicantsSession
        {
            public GetApplicantsSessionMock(int numberOfApplicants)
            {
                Applicants = Applicant.GenerateFakeData(numberOfApplicants);
            }

            public List<Applicant> Applicants { get; }

            public Task<List<Applicant>> GetApplicantsAsync(int skip, int take, string? searchTerm) =>
                Task.FromResult(Applicants.Skip(skip).Take(take).ToList());
        }
    }
}