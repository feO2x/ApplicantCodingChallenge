import { Applicant } from 'applicants/applicant';
import { ApplicantsSession } from './applicants-session';
import { generateFakeApplicants } from 'applicants/generate-fake-applicants';

export class FakeApplicantsSession extends ApplicantsSession {

    getApplicants(skip: number, take: number, searchTerm?: string): Promise<Applicant[]> {
        const appplicants = generateFakeApplicants(take, skip + 1);
        return Promise.resolve(appplicants);
    }
}
