using System;

namespace Hahn.ApplicationProcess.December2020.Domain
{
    public interface IApplicantProperties
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public string CountryOfOrigin { get; set; }

        public string EmailAddress { get; set; }

        public bool IsHired { get; set; }
    }
}