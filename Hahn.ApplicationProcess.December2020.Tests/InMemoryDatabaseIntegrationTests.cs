using System.Threading.Tasks;
using FluentAssertions;
using Hahn.ApplicationProcess.December2020.Data;
using Hahn.ApplicationProcess.December2020.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Hahn.ApplicationProcess.December2020.Tests
{
    public static class InMemoryDatabaseIntegrationTests
    {
        private static DbContextOptions<DatabaseContext> Options { get; } =
            new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("Test Applicants")
                                                          .Options;

        // This test is an ice breaker that checks if the EF In-Memory database behaves like a regular database
        // and if the model default behavior is working as expected
        [Fact]
        public static async Task CreateAndEditApplicant()
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
    }
}