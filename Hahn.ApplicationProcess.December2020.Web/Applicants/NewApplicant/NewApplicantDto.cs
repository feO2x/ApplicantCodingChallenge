﻿using System;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.NewApplicant
{
    public sealed class NewApplicantDto : IApplicantProperties
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