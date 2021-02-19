import { Applicant } from '../applicant';

export interface NewApplicantResult {
    hasErrors: boolean;
    createdApplicant?: Applicant;
    errors?: { [key: string]: string[] }
}
