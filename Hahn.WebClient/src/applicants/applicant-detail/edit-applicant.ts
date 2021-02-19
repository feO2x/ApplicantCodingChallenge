import { inject } from 'aurelia-framework';
import { ValidateEvent, ValidationController, ValidationControllerFactory } from 'aurelia-validation';
import { Applicant, applicantValidationRules, checkForStructuralEquality } from '../applicant';
import { EditApplicantSession } from './edit-applicant-session';
import { MdcSnackbarService } from '@aurelia-mdc-web/snackbar';
import { I18N } from 'aurelia-i18n';
import { MdcDialogService } from '@aurelia-mdc-web/dialog';
import { DeleteApplicantDialog } from './delete-applicant-dialog';
import { Router } from 'aurelia-router';

@inject(ValidationControllerFactory, EditApplicantSession, MdcSnackbarService, I18N, MdcDialogService, Router)
export class EditApplicant {

    private readonly validationController: ValidationController;
    applicant: Applicant | null = null;
    private originalApplicant: Applicant | null = null;
    isResetFormDisabled = true;

    constructor(
        validationControllerFactory: ValidationControllerFactory,
        private readonly session: EditApplicantSession,
        private readonly snackbarService: MdcSnackbarService,
        private readonly i18n: I18N,
        private readonly dialogService: MdcDialogService,
        private readonly router: Router,
    ) {
        this.validationController = validationControllerFactory.createForCurrentScope();
        this.validationController.subscribe(event => this.onFieldValidated(event));
    }

    private onFieldValidated(event: ValidateEvent): void {

        if (event.type !== 'validate')
            return;

        const equalityResult = checkForStructuralEquality(this.applicant, this.originalApplicant);
        if (equalityResult && !this.isResetFormDisabled)
            this.isResetFormDisabled = true;
        else if (!equalityResult && this.isResetFormDisabled)
            this.isResetFormDisabled = false;
    }

    activate(queryParameters: { id: number }): void {
        this.loadApplicant(queryParameters.id);
    }

    private async loadApplicant(id: number): Promise<void> {
        try {
            this.applicant = await this.session.getApplicant(id);
            this.originalApplicant = { ...this.applicant };
            this.validationController.addObject(this.applicant, applicantValidationRules);
        }
        catch {
            this.snackbarService.open(this.i18n.tr('service-call-error'));
        }
    }

    async save(): Promise<void> {

        if (checkForStructuralEquality(this.applicant, this.originalApplicant))
            return;
        const validationResult = await this.validationController.validate();
        if (!validationResult.valid)
            return;

        const applicant = this.applicant;
        try {
            const result = await this.session.updateApplicant(applicant);
            if (result.hasErrors) {
                if (result.errors.CountryOfOrigin) {
                    this.validationController.addError(this.i18n.tr('errorMessages.invalidCountry'), applicant, (a: Applicant) => a.countryOfOrigin);
                }
                else {
                    console.error(result.errors);
                    this.snackbarService.open(this.i18n.tr('client-side-error'));
                }

                return;
            }

            this.snackbarService.open(this.i18n.tr('applicantUpdated'));
            this.originalApplicant = { ...this.applicant };
            this.isResetFormDisabled = true;
        }
        catch {
            this.snackbarService.open(this.i18n.tr('service-call-error'));
        }
    }

    resetForm(): void {
        this.validationController.removeObject(this.applicant);
        this.validationController.reset();
        this.applicant = { ...this.originalApplicant };
        this.validationController.addObject(this.applicant, applicantValidationRules);
        this.isResetFormDisabled = true;
    }

    async tryDelete(): Promise<void> {
        const result = await this.dialogService.open({ viewModel: DeleteApplicantDialog, model: this.applicant });
        if (result !== 'delete')
            return;

        try {
            await this.session.deleteApplicant(this.applicant.id);
            this.snackbarService.open(this.i18n.tr('applicantDeleted', this.applicant));
            this.router.navigateToRoute('applicants');
        }
        catch {
            this.snackbarService.open(this.i18n.tr('service-call-error'));
        }
    }
}
