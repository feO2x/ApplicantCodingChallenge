import { I18N } from 'aurelia-i18n';
import { Applicant } from '../applicant';
import { inject } from 'aurelia-framework';

@inject(I18N)
export class DeleteApplicantDialog {

    question: string;

    constructor(private readonly i18n: I18N) { }

    activate(applicant: Applicant): void {
        this.question = this.i18n.tr("deleteApplicantQuestion", applicant);
    }
}
