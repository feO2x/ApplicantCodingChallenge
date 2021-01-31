using System.Collections.Generic;
using Light.GuardClauses;

namespace Hahn.ApplicationProcess.December2020.Domain
{
    public sealed class Applicant : Entity<Applicant>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string CountryOfOrigin { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public bool IsHired { get; set; }

        public static List<Applicant> GenerateFakeData(int numberOfApplicants = 100)
        {
            numberOfApplicants.MustBeIn(Range.FromInclusive(1).ToInclusive(50_000), nameof(numberOfApplicants));

            var applicants = new List<Applicant>(numberOfApplicants);
            for (var i = 0; i < numberOfApplicants; i++)
            {
                var firstName = Faker.Name.First();
                var lastName = Faker.Name.Last();
                var fullName = $"{firstName} {lastName}";
                var applicant = new Applicant
                {
                    Id = i + 1,
                    FirstName = firstName,
                    LastName = lastName,
                    Address = $"{Faker.Address.StreetAddress()}, {Faker.Address.ZipCode()} {Faker.Address.Country()}",
                    EmailAddress = Faker.Internet.Email(fullName),
                    CountryOfOrigin = Faker.Country.Name(),
                    IsHired = Faker.Boolean.Random()
                };
                applicants.Add(applicant);
            }

            return applicants;
        }
    }
}