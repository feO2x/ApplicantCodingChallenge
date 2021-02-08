using System;
using System.Threading.Tasks;
using FluentAssertions;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Tests.Domain;
using Hahn.ApplicationProcess.December2020.Tests.TestHelpers;
using Hahn.ApplicationProcess.December2020.Web.Applicants.UpdateApplicant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Hahn.ApplicationProcess.December2020.Tests.Applicants
{
    public sealed class UpdateApplicantControllerTests : WebApiControllerTests<UpdateApplicantController>
    {
        public UpdateApplicantControllerTests(ITestOutputHelper output) : base("api/applicants/update")
        {
            var loggerFactory = output.CreateLoggerFactory();
            var validator = new ApplicantValidator(new CountryNameValidatorStub(), loggerFactory.CreateLogger<ApplicantValidator>());
            Session = new UpdateApplicantSessionMock();
            SessionFactory = new FactorySpy<UpdateApplicantSessionMock>(Session);
            Controller = new UpdateApplicantController(validator, SessionFactory.GetInstance, loggerFactory.CreateLogger<UpdateApplicantController>());
        }

        private UpdateApplicantController Controller { get; }

        private UpdateApplicantSessionMock Session { get; }

        private FactorySpy<UpdateApplicantSessionMock> SessionFactory { get; }

        [Fact]
        public static void UpdateApplicantMustBeHttpPut() =>
            ControllerType.GetMethod(nameof(UpdateApplicantController.UpdateApplicant)).Should().BeDecoratedWith<HttpPutAttribute>();

        [Fact]
        public async Task UpdateValidApplicant()
        {
            var applicant = CreateApplicant();

            var result = await Controller.UpdateApplicant(applicant);

            result.Should().BeOfType<NoContentResult>();
            SessionMustBeSavedAndDisposed(applicant);
        }

        [Fact]
        public async Task ApplicantNotPresent()
        {
            var applicant = CreateApplicant();
            Session.ThrowDbUpdateConcurrencyException = true;

            var result = await Controller.UpdateApplicant(applicant);

            result.Should().BeOfType<NotFoundResult>();
            SessionMustBeSavedAndDisposed(applicant);
        }

        private void SessionMustBeSavedAndDisposed(Applicant applicant)
        {
            Session.CapturedApplicant.Should().BeSameAs(applicant);
            Session.SaveChangesMustHaveBeenCalled()
                   .MustHaveBeenDisposed();
        }

        [Fact]
        public async Task InvalidApplicant()
        {
            var invalidApplicant = CreateApplicant(0);

            var result = await Controller.UpdateApplicant(invalidApplicant);

            var expectedResult = Controller.ValidationProblem();
            result.MustBeEquivalentToValidationProblem(expectedResult);
            SessionFactory.InstanceMustNotHaveBeenCreated();
        }

        private static Applicant CreateApplicant(int id = 42) =>
            new()
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Address = "135 Fantasy Road, Michigan",
                CountryOfOrigin = "USA",
                DateOfBirth = new DateTime(1979, 8, 18),
                EmailAddress = "john.doe@gmail.com"
            };
        

        private sealed class UpdateApplicantSessionMock : BaseSessionMock<UpdateApplicantSessionMock>, IUpdateApplicantSession
        {
            public Applicant? CapturedApplicant { get; private set; }

            public bool ThrowDbUpdateConcurrencyException { get; set; }

            public void UpdateApplicant(Applicant applicant) => CapturedApplicant = applicant;

            public override Task SaveChangesAsync()
            {
                base.SaveChangesAsync();
                if (ThrowDbUpdateConcurrencyException)
                    throw new DbUpdateConcurrencyException();
                return Task.CompletedTask;
            }
        }
    }
}