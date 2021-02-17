import { AureliaEnhancedI18Next, I18N, TCustomAttribute } from "aurelia-i18n";
import i18nXhrBackend from 'i18next-xhr-backend';
import LanguageDetector from 'i18next-browser-languagedetector';

export function configureAureliaI18n(instance: I18N): Promise<AureliaEnhancedI18Next> {
    const aliases = ['t', 'i18n'];
    TCustomAttribute.configureAliases(aliases);
    instance.i18next.use(i18nXhrBackend);
    instance.i18next.use(LanguageDetector);

    return instance.setup({
        fallbackLng: 'en',
        backend: {
            loadPath: './locales/{{lng}}.json' // must be relative to main.ts / from base URL
        },
        detection: {
            order: [ 'localStorage', 'navigator' ],
            lookupLocalStorage: 'language'
        },
        attributes: aliases,
        debug: true
    });
}
