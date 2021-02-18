import { Applicant } from "applicants/applicant";
import { ApplicantsSession } from "./applicants-session";
import { bindable, inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { MdcSnackbarService } from '@aurelia-mdc-web/snackbar';
import { I18N } from 'aurelia-i18n';

const numberOfApplicantsPerCall = 30;
let requestCounter = 1;

@inject(ApplicantsSession, Router, MdcSnackbarService, I18N)
export class ApplicantList {

    private currentRequestId = 0;
    private scrollElement: HTMLElement | null = null;
    applicants: Applicant[] | null = null;
    totalNumberOfApplicants = 0;
    @bindable searchTerm = "";
    isLoading = false;

    constructor(
        private readonly session: ApplicantsSession,
        private readonly router: Router,
        private readonly snackbarService: MdcSnackbarService,
        private readonly i18n: I18N
    ) { }

    created(): void {
        this.loadApplicantsInternal(false);
    }

    loadNextItems(element: HTMLElement): void {

        this.scrollElement = element;
        if (!this.applicants || this.isLoading || element.offsetHeight + element.scrollTop < element.scrollHeight)
            return;

        this.loadApplicantsInternal(true);
    }

    private async loadApplicantsInternal(isAppending: boolean): Promise<void> {
        let skip = 0;
        if (isAppending && this.applicants)
            skip = this.applicants.length;

        const requestId = requestCounter++;
        this.currentRequestId = requestId;
        this.isLoading = true;
        try {
            const pageDto = await this.session.getApplicants(skip, numberOfApplicantsPerCall, this.searchTerm);

            // If another request has been made while we awaited the results, then we do not update.
            // The most prominent scenario is when the user changed the search term while a
            // request is ongoing.
            if (requestId !== this.currentRequestId)
                return;

            if (isAppending)
                this.applicants = this.applicants.concat(pageDto.applicants);
            else
                this.applicants = pageDto.applicants;

            this.totalNumberOfApplicants = pageDto.totalNumberOfApplicants;
            this.isLoading = false;
            if (!isAppending && this.scrollElement)
                this.scrollElement.scrollTop = 0;
        }
        catch {
            if (this.currentRequestId !== requestId)
                return;

            this.snackbarService.open(this.i18n.tr('service-call-error'));
            this.applicants = [];
            this.totalNumberOfApplicants = 0;
            this.isLoading = false;
            return;
        }
    }

    onItemSelected(applicantId: number): void {
        this.router.navigateToRoute('edit-applicant', { id: applicantId });
    }

    searchTermChanged(): void {
        if (this.searchTerm)
            this.searchTerm = this.searchTerm.trim();
        this.loadApplicantsInternal(false)
    }
}
