using System;
using System.Threading.Tasks;
using FluentAssertions;
using Hahn.ApplicationProcess.December2020.Data;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Hahn.ApplicationProcess.December2020.Tests.Data
{
    public sealed class InMemoryDatabaseIntegrationTests
    {
        public InMemoryDatabaseIntegrationTests(ITestOutputHelper output) =>
            Output = output;

        private static DbContextOptions<DatabaseContext> Options { get; } =
            new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("Test Applicants")
                                                          .Options;

        private ITestOutputHelper Output { get; }

        // This test is an ice breaker that checks if the EF In-Memory database behaves like a regular database
        // and if the model default behavior is working as expected
        [Fact]
        public async Task CreateAndEditApplicant()
        {
            DatabaseContext context;
            Applicant newApplicant;
            int numberOfAffectedRecords;
            await using (context = new DatabaseContext(Options))
            {
                newApplicant = new Applicant
                {
                    FirstName = "Kenny",
                    LastName = "Pflug",
                    Address = "Würzburger Str. Regensburg",
                    CountryOfOrigin = "Germany",
                    EmailAddress = "kenny.pflug@live.de"
                };
                // ReSharper disable once MethodHasAsyncOverload
                context.Applicants.Add(newApplicant);
                numberOfAffectedRecords = await context.SaveChangesAsync();
                numberOfAffectedRecords.Should().Be(1);
            }

            await using (context = new DatabaseContext(Options))
            {
                newApplicant = await context.Applicants.SingleOrDefaultAsync(applicant => applicant.LastName == "Pflug");
                newApplicant.IsHired = true;
                numberOfAffectedRecords = await context.SaveChangesAsync();
                numberOfAffectedRecords.Should().Be(1);
            }

            await using (context = new DatabaseContext(Options))
            {
                newApplicant = await context.Applicants.SingleOrDefaultAsync(applicant => applicant.LastName == "Pflug");
                var expectedApplicant = new Applicant
                {
                    Id = 1,
                    FirstName = "Kenny",
                    LastName = "Pflug",
                    Address = "Würzburger Str. Regensburg",
                    CountryOfOrigin = "Germany",
                    EmailAddress = "kenny.pflug@live.de",
                    IsHired = true
                };
                newApplicant.Should().BeEquivalentTo(expectedApplicant);
            }
        }

        [Fact]
        public async Task UpdateNonExistingApplicant()
        {
            await using var context = new DatabaseContext(Options);
            var nonExistingApplicant = new Applicant
            {
                Id = 2542,
                FirstName = "John",
                LastName = "Doe"
            };
            context.Update(nonExistingApplicant);

            // ReSharper disable once AccessToDisposedClosure -- the delegate is called before context is disposed
            Func<Task> act = () => context.SaveChangesAsync();

            (await act.Should().ThrowAsync<DbUpdateConcurrencyException>()).Which.ShouldBeWrittenTo(Output);
        }

        [Fact]
        public async Task DeleteNonExistingApplicant()
        {
            await using var context = new DatabaseContext(Options);
            var nonExistingApplicant = new Applicant { Id = 942818 };
            context.Remove(nonExistingApplicant);

            // ReSharper disable once AccessToDisposedClosure -- the delegate is called before context is disposed
            Func<Task> act = () => context.SaveChangesAsync();

            (await act.Should().ThrowAsync<DbUpdateConcurrencyException>()).Which.ShouldBeWrittenTo(Output);
        }
    }
}