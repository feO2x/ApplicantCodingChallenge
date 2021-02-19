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

export const applicantValidationRules =
    ValidationRules
        .ensure((a: ApplicantProperties) => a.firstName).required().minLength(2).withMessageKey('invalidFirstName')
        .ensure((a: ApplicantProperties) => a.lastName).required().minLength(2).withMessageKey('invalidLastName')
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
