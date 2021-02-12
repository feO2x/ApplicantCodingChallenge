import { Container } from 'aurelia-dependency-injection';
import { ApplicantsSession } from './applicant-list/applicants-session';
// import { FakeApplicantsSession } from './applicant-list/fake-applicants-session';
import { HttpApplicantsSession } from './applicant-list/http-applicants-session';

export function registerApplicantsModule(container: Container): void {
    container.registerSingleton(ApplicantsSession, HttpApplicantsSession);
}
