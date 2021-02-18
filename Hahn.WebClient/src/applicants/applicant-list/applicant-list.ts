import { Applicant } from "applicants/applicant";
import { ApplicantsSession } from "./applicants-session";
import { bindable, inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { MdcSnackbarService } from '@aurelia-mdc-web/snackbar';
import { I18N } from 'aurelia-i18n';
import { ApplicantsPageDto } from "./applicants-page-dto";

const numberOfApplicantsPerCall = 30;
let requestCounter = 1;

@inject(ApplicantsSession, Router, MdcSnackbarService, I18N)
export class ApplicantList {

    private currentRequestId: number | null = null;
    private scrollElement: HTMLElement | null = null;
    applicants: Applicant[] | null = null;
    totalNumberOfApplicants: number | null = null;
    @bindable
    searchTerm = "";

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
        if (!this.applicants || this.currentRequestId || element.offsetHeight + element.scrollTop < element.scrollHeight)
            return;

        this.loadApplicantsInternal(true);
    }

    private async loadApplicantsInternal(isAppending: boolean): Promise<void> {
        let skip = 0;
        if (isAppending && this.applicants)
            skip = this.applicants.length;

        const requestId = requestCounter++;
        this.currentRequestId = requestId;
        let pageDto: ApplicantsPageDto;
        try {
            pageDto = await this.session.getApplicants(skip, numberOfApplicantsPerCall, this.searchTerm);
        }
        catch {
            this.snackbarService.open(this.i18n.tr('service-call-error'));
            return;
        }

        // If another request has been made while we awaited the results, then we do not update.
        if (this.currentRequestId === null || requestId !== this.currentRequestId)
            return;

        if (isAppending)
            this.applicants = this.applicants.concat(pageDto.applicants);
        else
            this.applicants = pageDto.applicants;

        this.totalNumberOfApplicants = pageDto.totalNumberOfApplicants;
        this.currentRequestId = null;
        if (!isAppending && this.scrollElement)
            this.scrollElement.scrollTop = 0;
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
