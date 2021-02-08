using System;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Tests.TestHelpers;
using Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicant;
using Light.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Hahn.ApplicationProcess.December2020.Tests.Applicants
{
    public sealed class GetApplicantControllerTests : WebApiControllerTests<GetApplicantController>
    {
        public GetApplicantControllerTests() : base("api/applicants")
        {
            Session = new GetApplicantSessionMock();
            SessionFactory = new FactorySpy<GetApplicantSessionMock>(Session);
            Controller = new GetApplicantController(SessionFactory.GetInstance);
        }

        private GetApplicantController Controller { get; }

        private GetApplicantSessionMock Session { get; }

        private FactorySpy<GetApplicantSessionMock> SessionFactory { get; }

        [Fact]
        public static void GetApplicantMustBeHttpGet()
        {
            var httpGetAttribute = ControllerType.GetMethod(nameof(GetApplicantController.GetApplicant))?.GetCustomAttribute<HttpGetAttribute>();
            httpGetAttribute.MustNotBeNull().Template.Should().Be("{id}");
        }

        [Fact]
        public async Task GetExistingApplicant()
        {
            var result = await Controller.GetApplicant(42);

            result.Value.Should().BeEquivalentTo(Session.Applicant);
            Session.MustHaveBeenDisposed();
        }

        [Fact]
        public async Task ApplicantNotFound()
        {
            var result = await Controller.GetApplicant(17);

            result.Result.Should().BeOfType<NotFoundResult>();
            Session.MustHaveBeenDisposed();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-18939)]
        public async Task InvalidId(int invalidId)
        {
            var result = await Controller.GetApplicant(invalidId);

            var expectedResult = Controller.ValidationProblem();
            result.Result.MustBeEquivalentToValidationProblem(expectedResult);
            SessionFactory.InstanceMustNotHaveBeenCreated();
        }

        private sealed class GetApplicantSessionMock : BaseReadOnlySessionMock<GetApplicantSessionMock>, IGetApplicantSession
        {
            public Applicant Applicant { get; } =
                new()
                {
                    Id = 42,
                    FirstName = "John",
                    LastName = "Doe",
                    Address = "135 Fantasy Road, Michigan",
                    CountryOfOrigin = "USA",
                    DateOfBirth = new DateTime(1979, 8, 18),
                    EmailAddress = "john.doe@gmail.com",
                    IsHired = false
                };

            public ValueTask<Applicant?> GetApplicantAsync(int id) =>
                id == 42 ? new(Applicant) : new ValueTask<Applicant?>((Applicant?) null);
        }
    }
}