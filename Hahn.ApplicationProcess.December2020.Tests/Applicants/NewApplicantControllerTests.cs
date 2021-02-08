﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Tests.TestHelpers;
using Hahn.ApplicationProcess.December2020.Web.Applicants.NewApplicant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Hahn.ApplicationProcess.December2020.Tests.Applicants
{
    public sealed class NewApplicantControllerTests : WebApiControllerTests<NewApplicantController>
    {
        public NewApplicantControllerTests(ITestOutputHelper output) : base("api/applicants/new")
        {
            CountryNameValidator = new CountryNameValidatorStub();
            var serilogLogger = output.CreateTestLogger();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(serilogLogger));
            var validatorLogger = loggerFactory.CreateLogger<NewApplicantDtoValidator>();
            var validator = new NewApplicantDtoValidator(CountryNameValidator, validatorLogger);
            Session = new NewApplicantSessionMock();
            Mapper = new Mapper(new MapperConfiguration(expression => expression.AddProfile(new NewApplicantAutoMapperProfile())));
            var controllerLogger = loggerFactory.CreateLogger<NewApplicantController>();
            Controller = new NewApplicantController(validator, () => Session, Mapper, controllerLogger);
        }

        private NewApplicantController Controller { get; }

        private CountryNameValidatorStub CountryNameValidator { get; }

        private NewApplicantSessionMock Session { get; }

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

            var expectedResult = Controller.ValidationProblem();
            result.MustBeEquivalentToValidationProblem(expectedResult);
        }

        [Fact]
        public async Task ExceptionWhileValidatingCountry()
        {
            var dto = CreateDto();
            CountryNameValidator.ThrowException = true;

            var result = await Controller.CreateNewApplicant(dto);

            var expectedResult = Controller.ValidationProblem();
            result.MustBeEquivalentToValidationProblem(expectedResult);
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

        private sealed class CountryNameValidatorStub : ICountryNameValidator
        {
            public bool IsValidCountry { get; set; } = true;

            public bool ThrowException { get; set; }

            public Task<bool> CheckIfCountryNameIsValidAsync(string countryName, CancellationToken cancellationToken)
            {
                if (ThrowException)
                    throw new Exception("An exception occurred while validating the country");
                return Task.FromResult(IsValidCountry);
            }
        }

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