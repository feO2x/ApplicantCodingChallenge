import { Applicant } from '../applicant';

export abstract class EditApplicantSession {
    abstract getApplicant(id: number): Promise<Applicant>;
    abstract updateApplicant(applicant: Applicant): Promise<void>;
}
