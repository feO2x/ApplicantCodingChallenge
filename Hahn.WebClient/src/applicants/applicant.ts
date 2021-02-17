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
