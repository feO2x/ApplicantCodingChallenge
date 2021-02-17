import { Applicant } from "applicants/applicant";
import { ApplicantsSession } from "./applicants-session";
import { inject } from 'aurelia-framework';
import { ApplicantsPageDto } from "./applicants-page-dto";
import { Router } from 'aurelia-router';

const numberOfApplicantsPerCall = 30;

@inject(ApplicantsSession, Router)
export class ApplicantList {
    
    private currentPromise: Promise<ApplicantsPageDto> | null = null;
    applicants: Applicant[] | null = null;
    totalNumberOfApplicants: number | null = null;

    constructor(
        private readonly session: ApplicantsSession,
        private readonly router: Router
    ) { }

    async created(): Promise<void> {
        const pageDto = await this.session.getApplicants(0, numberOfApplicantsPerCall);
        this.applicants = pageDto.applicants;
        this.totalNumberOfApplicants = pageDto.totalNumberOfApplicants;
    }

    loadNextItems(element: { scrollHeight: number, offsetHeight: number, scrollTop: number }): void {
        
        if (!this.applicants || this.currentPromise || element.offsetHeight + element.scrollTop < element.scrollHeight)
            return;

        this.loadNextItemsInternal();
    }

    private async loadNextItemsInternal(): Promise<void> {
        this.currentPromise = this.session.getApplicants(this.applicants.length, numberOfApplicantsPerCall);
        const pageDto = await this.currentPromise;
        if (pageDto.applicants && pageDto.applicants.length > 0)
            this.applicants = this.applicants.concat(pageDto.applicants);
        this.totalNumberOfApplicants = pageDto.totalNumberOfApplicants;
        this.currentPromise = null;
    }

    onItemSelected(applicantId: number): void {
        this.router.navigateToRoute('applicant-detail', { id: applicantId });
    }
}
