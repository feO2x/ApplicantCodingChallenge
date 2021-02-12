export interface LocaleOption {
    languageCode: string;
    name: string;
}

export const englishOption: LocaleOption = {
    languageCode: 'en',
    name: 'English'
};

export const germanOption: LocaleOption = {
    languageCode: 'de',
    name: 'Deutsch'
};

export const allLocales: LocaleOption[] = [
    englishOption,
    germanOption
];

export function getLocaleOptionByLanguageCode(languageCode: string): LocaleOption {
    switch (languageCode) {
        case 'en': return englishOption;
        case 'de': return germanOption;
        default:
            throw new Error(`The language code "${languageCode}" is not valid`);
    }
}
