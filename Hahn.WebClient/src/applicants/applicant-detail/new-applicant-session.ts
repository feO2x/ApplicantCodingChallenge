import { Applicant, ApplicantProperties } from '../applicant';
import { NewApplicantResult } from './new-applicant-result';

export abstract class NewApplicantSession {
    abstract createNewApplicant(applicant: ApplicantProperties): Promise<NewApplicantResult>;
}
