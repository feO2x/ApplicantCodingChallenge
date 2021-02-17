import { ApplicantProperties } from '../applicant';
import { bindable } from 'aurelia-framework';

export class ApplicantForm {

    @bindable
    applicant: ApplicantProperties | null = null;
}
