using System;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Tests.TestHelpers;
using Hahn.ApplicationProcess.December2020.Web.Applicants.DeleteApplicant;
using Light.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Hahn.ApplicationProcess.December2020.Tests.Applicants
{
    public sealed class DeleteApplicantControllerTests : WebApiControllerTests<DeleteApplicantController>
    {
        public DeleteApplicantControllerTests(ITestOutputHelper output) : base("api/applicants/delete")
        {
            var loggerFactory = output.CreateLoggerFactory();
            Session = new DeleteApplicantSessionMock();
            SessionFactory = new FactorySpy<DeleteApplicantSessionMock>(Session);
            Controller = new DeleteApplicantController(SessionFactory.GetInstance, loggerFactory.CreateLogger<DeleteApplicantController>());
        }

        private DeleteApplicantController Controller { get; }
        private DeleteApplicantSessionMock Session { get; }
        private FactorySpy<DeleteApplicantSessionMock> SessionFactory { get; }

        [Fact]
        public static void DeleteApplicantMustBeHttpDelete()
        {
            var httpDeleteAttribute = ControllerType.GetMethod(nameof(DeleteApplicantController.DeleteApplicant))?.GetCustomAttribute<HttpDeleteAttribute>();
            httpDeleteAttribute.MustNotBeNull().Template.Should().Be("{id}");
        }

        [Fact]
        public async Task DeleteExistingApplicant()
        {
            var result = await Controller.DeleteApplicant(42);

            result.Should().BeOfType<NoContentResult>();
            VerifyApplicantWasDeleted();
        }

        [Fact]
        public async Task DeleteNonExistingApplicant()
        {
            var result = await Controller.DeleteApplicant(109);

            result.Should().BeOfType<NotFoundResult>();
            Session.CapturedApplicant.Should().BeNull();
            Session.SaveChangesMustNotHaveBeenCalled()
                   .MustHaveBeenDisposed();
        }

        [Fact]
        public async Task RaceConditionOnSaveChanges()
        {
            Session.ThrowDbUpdateConcurrencyException = true;

            var result = await Controller.DeleteApplicant(42);

            result.Should().BeOfType<NoContentResult>();
            VerifyApplicantWasDeleted();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-62183912)]
        public async Task InvalidId(int invalidId)
        {
            var result = await Controller.DeleteApplicant(invalidId);

            var expectedResult = Controller.ValidationProblem();
            result.MustBeEquivalentToValidationProblem(expectedResult);
            SessionFactory.InstanceMustNotHaveBeenCreated();
        }

        private void VerifyApplicantWasDeleted()
        {
            Session.CapturedApplicant.Should().BeSameAs(Session.Applicant);
            Session.SaveChangesMustHaveBeenCalled()
                   .MustHaveBeenDisposed();
        }

        private sealed class DeleteApplicantSessionMock : BaseSessionMock<DeleteApplicantSessionMock>, IDeleteApplicantSession
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
                    EmailAddress = "john.doe@gmail.com"
                };

            public Applicant? CapturedApplicant { get; private set; }

            public bool ThrowDbUpdateConcurrencyException { get; set; }

            public ValueTask<Applicant?> GetApplicantAsync(int id) =>
                id == 42 ? new ValueTask<Applicant?>(Applicant) : new ValueTask<Applicant?>((Applicant?) null);

            public void DeleteApplicant(Applicant applicant) => CapturedApplicant = applicant;

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