import { Aurelia } from 'aurelia-framework';
import * as environment from '../config/environment.json';
import { PLATFORM } from 'aurelia-pal';
import { configureAureliaI18n } from './locales/configure-aurelia-i18n';
import { registerApplicantsModule } from 'applicants/applicants-module';
import { registerHttpClientModule } from 'http-client/http-client-module';

export function configure(aurelia: Aurelia): void {
    aurelia.use
        .standardConfiguration()
        .developmentLogging(environment.debug ? 'debug' : 'warn')
        .plugin(PLATFORM.moduleName('@aurelia-mdc-web/all'))
        .plugin(PLATFORM.moduleName('aurelia-i18n'), configureAureliaI18n);        

    if (environment.testing) {
        aurelia.use.plugin(PLATFORM.moduleName('aurelia-testing'));
    }

    registerHttpClientModule(aurelia.container, environment.baseUrl);
    registerApplicantsModule(aurelia.container);

    aurelia.start().then(() => aurelia.setRoot(PLATFORM.moduleName('app')));
}
