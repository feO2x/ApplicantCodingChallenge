﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Tests.TestHelpers;
using Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicants;
using Hahn.ApplicationProcess.December2020.Web.Paging;
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
        [InlineData(110, 25)]
        public static async Task ReturnApplicantsOnValidRequest(int skip, int take)
        {
            var session = new GetApplicantsSessionMock(100);
            var controller = new GetApplicantsController(new PageDtoValidator(), () => session);

            var actionResult = await controller.GetApplicants(new PageDto { Skip = skip, Take = take });

            var expectedApplicants = session.Applicants.Skip(skip).Take(take).ToList();
            var expectedResult = new ApplicantsPageDto(100, expectedApplicants);
            actionResult.Value.Should().BeEquivalentTo(expectedResult);
            session.MustHaveBeenDisposed();
        }

        private sealed class GetApplicantsSessionMock : BaseReadOnlySessionMock<GetApplicantsSessionMock>, IGetApplicantsSession
        {
            public GetApplicantsSessionMock(int numberOfApplicants)
            {
                Applicants = ApplicantFactory.GenerateFakeData(numberOfApplicants);
            }

            public List<Applicant> Applicants { get; }

            public Task<int> GetTotalNumberOfApplicantsAsync(string? searchTerm) =>
                Task.FromResult(Applicants.Count);

            public Task<List<Applicant>> GetApplicantsAsync(int skip, int take, string? searchTerm) =>
                Task.FromResult(Applicants.Skip(skip).Take(take).ToList());
        }
    }
}