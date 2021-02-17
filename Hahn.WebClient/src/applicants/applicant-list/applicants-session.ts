import { ApplicantsPageDto } from './applicants-page-dto';

export abstract class ApplicantsSession {
    abstract getApplicants(skip: number, take: number, searchTerm?: string): Promise<ApplicantsPageDto>;
}
