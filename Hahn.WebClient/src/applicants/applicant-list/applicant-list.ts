import { Applicant } from "applicants/applicant";
import { ApplicantsSession } from "./applicants-session";
import { inject } from 'aurelia-framework';

const numberOfApplicantsPerCall = 30;

@inject(ApplicantsSession)
export class ApplicantList {
    
    private currentPromise: Promise<Applicant[]> | null = null;
    applicants: Applicant[] | null = null;

    constructor(private readonly session: ApplicantsSession) { }

    async created(): Promise<void> {
        this.applicants = await this.session.getApplicants(0, numberOfApplicantsPerCall);
    }

    loadNextItems(element: { scrollHeight: number, offsetHeight: number, scrollTop: number }): void {
        
        if (this.currentPromise || element.offsetHeight + element.scrollTop < element.scrollHeight)
            return;

        this.loadNextItemsInternal();
    }

    private async loadNextItemsInternal(): Promise<void> {
        this.currentPromise = this.session.getApplicants(this.applicants.length, numberOfApplicantsPerCall);
        const loadedApplicants = await this.currentPromise;
        this.applicants = this.applicants.concat(loadedApplicants);
        this.currentPromise = null;
    }
    
}
