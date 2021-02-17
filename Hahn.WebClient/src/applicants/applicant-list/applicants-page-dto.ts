import { Applicant } from '../applicant';

export interface ApplicantsPageDto {
    readonly totalNumberOfApplicants: number;
    readonly applicants: Applicant[];
}
