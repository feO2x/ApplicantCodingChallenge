using System;

namespace Hahn.ApplicationProcess.December2020.Domain
{
    public sealed class Applicant : Entity<Applicant>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; } = string.Empty;

        public string CountryOfOrigin { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public bool IsHired { get; set; }
    }
}