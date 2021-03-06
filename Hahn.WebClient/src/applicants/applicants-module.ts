import { Container } from 'aurelia-dependency-injection';
import { ApplicantsSession } from './applicant-list/applicants-session';
import { HttpApplicantsSession } from './applicant-list/http-applicants-session';
import { EditApplicantSession } from './applicant-detail/edit-applicant-session';
import { HttpEditApplicantSession } from './applicant-detail/http-edit-applicant-session';
import { NewApplicantSession } from './applicant-detail/new-applicant-session';
import { HttpNewApplicantSession } from './applicant-detail/http-new-applicant-session';

export function registerApplicantsModule(container: Container): void {

    container.registerSingleton(ApplicantsSession, HttpApplicantsSession);
    container.registerSingleton(EditApplicantSession, HttpEditApplicantSession);
    container.registerSingleton(NewApplicantSession, HttpNewApplicantSession);
}
