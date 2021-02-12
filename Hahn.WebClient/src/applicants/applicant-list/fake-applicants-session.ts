import { Applicant } from 'applicants/applicant';
import { ApplicantsSession } from './applicants-session';
import { generateFakeApplicants } from 'applicants/generate-fake-applicants';

export class FakeApplicantsSession extends ApplicantsSession {

    getApplicants(skip: number, take: number, searchTerm?: string): Promise<Applicant[]> {
        return new Promise<Applicant[]>(resolve => {
            setTimeout(
                () => {
                    const applicants = generateFakeApplicants(take, skip + 1);
                    resolve(applicants);
                },
                1500
            );
        });
        
    }
}
