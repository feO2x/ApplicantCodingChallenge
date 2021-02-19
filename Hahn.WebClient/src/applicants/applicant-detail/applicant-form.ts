import { ApplicantProperties } from '../applicant';
import { bindable, inject } from 'aurelia-framework';
import { ValidationController } from 'aurelia-validation';

@inject(ValidationController)
export class ApplicantForm {

    @bindable
    applicant: ApplicantProperties | null = null;
}
