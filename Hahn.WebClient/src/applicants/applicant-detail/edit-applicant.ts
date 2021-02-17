import { inject } from 'aurelia-framework';
import { Applicant } from '../applicant';
import { EditApplicantSession } from './edit-applicant-session';

@inject(EditApplicantSession)
export class EditApplicant {

    applicant: Applicant | null = null;

    constructor(
        private readonly session: EditApplicantSession
    ) { }

    activate(queryParameters: { id: number }): void {
        this.loadApplicant(queryParameters.id);
    }

    private async loadApplicant(id: number): Promise<void> {
        this.applicant = await this.session.getApplicant(id);
    }

}
