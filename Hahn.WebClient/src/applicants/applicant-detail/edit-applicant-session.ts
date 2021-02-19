import { Applicant } from '../applicant';
import { EditApplicantResult } from './edit-applicant-result';

export abstract class EditApplicantSession {
    abstract getApplicant(id: number): Promise<Applicant>;
    abstract updateApplicant(applicant: Applicant): Promise<EditApplicantResult>;
    abstract deleteApplicant(id: number): Promise<void>;
}
