import { AureliaEnhancedI18Next, I18N, TCustomAttribute } from "aurelia-i18n";
import i18nXhrBackend from 'i18next-xhr-backend';
import LanguageDetector from 'i18next-browser-languagedetector';

const localStorageLanguageKey = 'language';

export function configureAureliaI18n(instance: I18N): Promise<AureliaEnhancedI18Next> {
    const aliases = ['t', 'i18n'];
    TCustomAttribute.configureAliases(aliases);
    instance.i18next.use(LanguageDetector);
    instance.i18next.use(i18nXhrBackend);

    trySetInitialLanguage();

    if (localStorage && !localStorage.getItem(localStorageLanguageKey))
        localStorage.setItem(localStorageLanguageKey, 'en');

    return instance.setup({
        whitelist: ['en', 'de'],
        fallbackLng: 'en',
        detection: {
            order: ['localStorage'],
            lookupLocalStorage: localStorageLanguageKey
        },
        backend: {
            loadPath: './locales/{{lng}}.json' // must be relative to main.ts / from base URL
        },
        attributes: aliases,
        debug: true
    });
}

function trySetInitialLanguage(): void {
    if (!localStorage || localStorage.getItem(localStorageLanguageKey))
        return;

    const initialLanguage = determineInitialLanguage();
    localStorage.setItem(localStorageLanguageKey, initialLanguage);
}

function determineInitialLanguage(): string {
    if (!navigator)
        return 'en';

    if (navigator.languages && navigator.languages.length > 0) {
        for (const language of navigator.languages) {
            if (isGermanLanguage(language)) {
                return 'de';
            } else if (isEnglishLanguage(language)) {
                return 'en';
            }
        }

        return 'en';
    }

    return isGermanLanguage(navigator.language) ? 'de' : 'en';
}

function isGermanLanguage(language: string): boolean {
    return isOrStartsWithLanguageCode(language, 'de');
}

function isEnglishLanguage(language: string): boolean {
    return isOrStartsWithLanguageCode(language, 'en');
}

function isOrStartsWithLanguageCode(language: string, twoCharacterLanguageCode: string): boolean {
    return (language.length === 2 && language === twoCharacterLanguageCode) || (language.startsWith(twoCharacterLanguageCode));
}
