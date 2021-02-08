using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Tests.Domain;
using Hahn.ApplicationProcess.December2020.Tests.TestHelpers;
using Hahn.ApplicationProcess.December2020.Web.Applicants.NewApplicant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Hahn.ApplicationProcess.December2020.Tests.Applicants
{
    public sealed class NewApplicantControllerTests : WebApiControllerTests<NewApplicantController>
    {
        public NewApplicantControllerTests(ITestOutputHelper output) : base("api/applicants/new")
        {
            CountryNameValidator = new CountryNameValidatorStub();
            var loggerFactory = output.CreateLoggerFactory();
            var validator = new NewApplicantDtoValidator(CountryNameValidator, loggerFactory.CreateLogger<NewApplicantDtoValidator>());
            Session = new NewApplicantSessionMock();
            SessionFactory = new FactorySpy<NewApplicantSessionMock>(Session);
            Mapper = new Mapper(new MapperConfiguration(expression => expression.AddProfile(new NewApplicantAutoMapperProfile())));
            Controller = new NewApplicantController(validator, SessionFactory.GetInstance, Mapper, loggerFactory.CreateLogger<NewApplicantController>());
        }

        private NewApplicantController Controller { get; }

        private CountryNameValidatorStub CountryNameValidator { get; }

        private NewApplicantSessionMock Session { get; }

        private FactorySpy<NewApplicantSessionMock> SessionFactory { get; }

        private IMapper Mapper { get; }

        [Fact]
        public static void CreateNewApplicantMustBeHttpPost() =>
            ControllerType.GetMethod(nameof(NewApplicantController.CreateNewApplicant)).Should().BeDecoratedWith<HttpPostAttribute>();

        [Fact]
        public static void NewApplicantDtoValidatorMustDeriveFromBaseApplicantValidator() =>
            typeof(NewApplicantDtoValidator).Should().BeDerivedFrom<BaseApplicantValidator<NewApplicantDto>>();

        [Fact]
        public async Task CreateNewApplicant()
        {
            var dto = CreateDto();

            var result = await Controller.CreateNewApplicant(dto);

            var expectedApplicant = Mapper.Map<NewApplicantDto, Applicant>(dto);
            Session.CapturedApplicant.Should().BeEquivalentTo(expectedApplicant);
            Session.SaveChangesMustHaveBeenCalled()
                   .MustHaveBeenDisposed();
            var expectedResult = Controller.Created("/api/applicants/0", Session.CapturedApplicant);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task InvalidCountry()
        {
            var dto = CreateDto();
            dto.CountryOfOrigin = "Invalid Country Name";
            CountryNameValidator.IsValidCountry = false;

            var result = await Controller.CreateNewApplicant(dto);

            CheckForValidationError(result);
        }

        [Fact]
        public async Task ExceptionWhileValidatingCountry()
        {
            var dto = CreateDto();
            CountryNameValidator.ThrowException = true;

            var result = await Controller.CreateNewApplicant(dto);

            CheckForValidationError(result);
        }

        private void CheckForValidationError(IActionResult result)
        {
            var expectedResult = Controller.ValidationProblem();
            result.MustBeEquivalentToValidationProblem(expectedResult);
            SessionFactory.InstanceMustNotHaveBeenCreated();
            Session.SaveChangesMustNotHaveBeenCalled();
        }

        private static NewApplicantDto CreateDto() =>
            new()
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "135 Fantasy Road, Michigan",
                CountryOfOrigin = "USA",
                DateOfBirth = new DateTime(1979, 8, 18),
                EmailAddress = "john.doe@gmail.com"
            };


        private sealed class NewApplicantSessionMock : BaseSessionMock<NewApplicantSessionMock>, INewApplicantSession
        {
            public Applicant? CapturedApplicant { get; private set; }

            public void AddApplicant(Applicant applicant)
            {
                CapturedApplicant = applicant;
            }
        }
    }
}