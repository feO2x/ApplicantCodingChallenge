import { Aurelia } from 'aurelia-framework';
import * as environment from '../config/environment.json';
import { PLATFORM } from 'aurelia-pal';
import { configureAureliaI18n, configureI18nValidationMessages } from './locales/configure-aurelia-i18n';
import { registerApplicantsModule } from 'applicants/applicants-module';
import { registerHttpClientModule } from 'http-client/http-client-module';

export function configure(aurelia: Aurelia): void {
    aurelia.use
        .standardConfiguration()
        .developmentLogging(environment.debug ? 'debug' : 'warn')
        .plugin(PLATFORM.moduleName('@aurelia-mdc-web/all'))
        .plugin(PLATFORM.moduleName('aurelia-i18n'), configureAureliaI18n)
        .plugin(PLATFORM.moduleName('aurelia-validation'));

    if (environment.testing) {
        aurelia.use.plugin(PLATFORM.moduleName('aurelia-testing'));
    }

    registerHttpClientModule(aurelia.container, environment.baseUrl);
    registerApplicantsModule(aurelia.container);
    configureI18nValidationMessages(aurelia.container);

    aurelia.start().then(() => aurelia.setRoot(PLATFORM.moduleName('app')));
}
