import { ValidationController, ValidationControllerFactory } from 'aurelia-validation';
import { inject } from 'aurelia-framework';
import { ApplicantProperties, applicantValidationRules, createEmptyApplicant } from 'applicants/applicant';
import { NewApplicantSession } from './new-applicant-session';
import { Router } from 'aurelia-router';
import { MdcSnackbarService } from '@aurelia-mdc-web/snackbar';
import { I18N } from 'aurelia-i18n';

@inject(ValidationControllerFactory, NewApplicantSession, Router, MdcSnackbarService, I18N)
export class NewApplicant {

    private readonly validationController: ValidationController;
    applicant = createEmptyApplicant();

    constructor(
        validationControllerFactory: ValidationControllerFactory,
        private readonly session: NewApplicantSession,
        private readonly router: Router,
        private readonly snackbarService: MdcSnackbarService,
        private readonly i18n: I18N
    ) {
        this.validationController = validationControllerFactory.createForCurrentScope();
        this.validationController.addObject(this.applicant, applicantValidationRules);
    }

    async saveApplicant(): Promise<void> {
        
        const validationResult = await this.validationController.validate();
        if (!validationResult.valid)
            return;

        const applicant = this.applicant;
        try {
            const createResult = await this.session.createNewApplicant(applicant);
            // Check if any errors were reported that need to be added to the validation controller
            if (createResult.hasErrors) {
                
                if (createResult.errors.CountryOfOrigin) {
                    this.validationController.addError(this.i18n.tr('errorMessages.invalidCountry'), applicant, (a: ApplicantProperties) => a.countryOfOrigin);
                }
                else {
                    console.error(createResult.errors);
                    this.snackbarService.open(this.i18n.tr('client-side-error'));
                }

                return;
            }

            // Else everything worked fine
            this.snackbarService.open(this.i18n.tr('newApplicantCreated', createResult.createdApplicant));
            this.router.navigateToRoute('edit-applicant', { id: createResult.createdApplicant.id });         
        }
        catch {
            this.snackbarService.open(this.i18n.tr('service-call-error'));
        }
    }
}
