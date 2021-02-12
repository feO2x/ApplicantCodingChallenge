import { Applicant } from "applicants/applicant";

export abstract class ApplicantsSession {
    abstract getApplicants(skip: number, take: number, searchTerm?: string): Promise<Applicant[]>;
}
