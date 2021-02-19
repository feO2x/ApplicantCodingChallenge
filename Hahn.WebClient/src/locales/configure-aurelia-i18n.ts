import { Container } from 'aurelia-dependency-injection';
import { AureliaEnhancedI18Next, I18N, TCustomAttribute } from "aurelia-i18n";
import i18nXhrBackend from 'i18next-xhr-backend';
import LanguageDetector from 'i18next-browser-languagedetector';
import { ValidationMessageProvider } from 'aurelia-validation';

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

export function configureI18nValidationMessages(container: Container): void {
    ValidationMessageProvider.prototype.getMessage = function (key: string) {
        const i18n = container.get(I18N);
        const translation = i18n.tr(`errorMessages.${key}`);
        return this.parser.parse(translation);
    };

    ValidationMessageProvider.prototype.getDisplayName = function (propertyName: string, displayName: string) {
        if (displayName !== null && displayName !== undefined)
            return displayName;

        const i18n = container.get(I18N);
        return i18n.tr(propertyName);
    }
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
