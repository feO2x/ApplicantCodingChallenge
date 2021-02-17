import { Applicant } from 'applicants/applicant';
import { ApplicantsSession } from './applicants-session';
import { generateFakeApplicants } from 'applicants/generate-fake-applicants';
import { ApplicantsPageDto } from './applicants-page-dto';

export class FakeApplicantsSession extends ApplicantsSession {

    getApplicants(skip: number, take: number, searchTerm?: string): Promise<ApplicantsPageDto> {
        return new Promise<ApplicantsPageDto>(resolve => {
            setTimeout(
                () => {
                    let applicants: Applicant[];
                    if (skip < 1000)
                        applicants = generateFakeApplicants(take, skip + 1);
                    else
                        applicants = [];
                    const dto: ApplicantsPageDto = {
                        totalNumberOfApplicants: 1000,
                        applicants
                    };
                    resolve(dto);
                },
                1500
            );
        });
        
    }
}
