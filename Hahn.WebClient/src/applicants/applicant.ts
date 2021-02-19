import { ValidationRules } from 'aurelia-validation';

export interface ApplicantProperties {
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    address: string;
    countryOfOrigin: string;
    emailAddress: string;
    isHired: boolean;
}

export interface Applicant extends ApplicantProperties {
    id: number;
}

export const applicantDateRangeRuleName = 'applicantDateRange';

ValidationRules.customRule(
    applicantDateRangeRuleName,
    (value: string, _obj): boolean => {
        const date = new Date(value);
        if (isNaN(date.getTime()))
            return false;
        
        const today = new Date();
        const minimumDate = new Date("1900-01-01");
        return minimumDate < date && date < today;
    },
    'The birth date is required and must be set to a value after 1900-01-01 and before today'
);

export const applicantValidationRules =
    ValidationRules
        .ensure((a: ApplicantProperties) => a.firstName).required().minLength(2).withMessageKey('invalidFirstName')
        .ensure((a: ApplicantProperties) => a.lastName).required().minLength(2).withMessageKey('invalidLastName')
        .ensure((a: ApplicantProperties) => a.dateOfBirth).required().satisfiesRule(applicantDateRangeRuleName).withMessageKey('invalidDateOfBirth')
        .ensure((a: ApplicantProperties) => a.address).required().minLength(10).withMessageKey('invalidAddress')
        .ensure((a: ApplicantProperties) => a.emailAddress).required().email().withMessageKey('invalidEmail')
        .ensure((a: ApplicantProperties) => a.countryOfOrigin).required().withMessageKey('invalidCountry')
        .rules;

export function createEmptyApplicant(): ApplicantProperties {
    return {
        firstName: null,
        lastName: null,
        dateOfBirth: null,
        address: null,
        countryOfOrigin: null,
        emailAddress: null,
        isHired: false
    };
}

export function checkForStructuralEquality(x: ApplicantProperties, y: ApplicantProperties): boolean {
    if (x.firstName !== y.firstName)
        return false;
    if (x.lastName !== y.lastName)
        return false;
    if (x.dateOfBirth !== y.dateOfBirth)
        return false;
    if (x.emailAddress !== y.emailAddress)
        return false;
    if (x.isHired !== y.isHired)
        return false;
    if (x.countryOfOrigin !== y.countryOfOrigin)
        return false;
    if (x.address !== y.address)
        return false;

    return true;
}
