import { RouterConfiguration } from 'aurelia-router';
import { PLATFORM } from "aurelia-framework";

export class App {

    configureRouter(config: RouterConfiguration): void {

        config.options.pushState = true;
        config.options.root = '/';
        config.map([
            {
                route: '',
                name: 'applicants',
                moduleId: PLATFORM.moduleName('./applicants/applicant-list/applicant-list')
            },
            {
                route: 'applicants/:id',
                name: 'applicant-detail',
                moduleId: PLATFORM.moduleName('./applicants/applicant-detail/applicant-detail')
            }
        ]);
    }
}
