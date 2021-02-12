import { AureliaEnhancedI18Next, I18N, TCustomAttribute } from "aurelia-i18n";
import i18nXhrBackend from 'i18next-xhr-backend';

export function configureAureliaI18n(instance: I18N): Promise<AureliaEnhancedI18Next> {
    const aliases = ['t', 'i18n'];
    TCustomAttribute.configureAliases(aliases);
    instance.i18next.use(i18nXhrBackend);

    return instance.setup({
        lng: 'de',
        fallbackLng: 'en',
        backend: {
            loadPath: './locales/{{lng}}.json' // must be relative to main.ts / from base URL
        },
        attributes: aliases,
        debug: true
    });
}
