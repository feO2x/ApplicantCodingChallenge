using System.Collections.Generic;
using Light.GuardClauses;

namespace Hahn.ApplicationProcess.December2020.Domain
{
    public static class ApplicantFactory
    {
        public static Range<int> NumberOfApplicantsRange { get; } =
            Range.FromInclusive(1).ToInclusive(50_000);

        public static List<Applicant> GenerateFakeData(int numberOfApplicants = 100)
        {
            numberOfApplicants.MustBeIn(NumberOfApplicantsRange, nameof(numberOfApplicants));

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