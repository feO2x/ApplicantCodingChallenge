using System;
using System.Collections.Generic;
using Faker;
using Light.GuardClauses;
using Boolean = Faker.Boolean;
using Range = Light.GuardClauses.Range;

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
                var firstName = Name.First();
                var lastName = Name.Last();
                var fullName = $"{firstName} {lastName}";
                var country = Country.Name();
                var applicant = new Applicant
                {
                    Id = i + 1,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = DateOfBirth.CreateRandom(),
                    Address = $"{Address.StreetAddress()},{Environment.NewLine}{Address.ZipCode()} {Address.City()},{Environment.NewLine}{country}",
                    EmailAddress = Internet.Email(fullName),
                    CountryOfOrigin = Boolean.Random() ? Country.Name() : country,
                    IsHired = Boolean.Random()
                };
                applicants.Add(applicant);
            }

            return applicants;
        }
    }
}