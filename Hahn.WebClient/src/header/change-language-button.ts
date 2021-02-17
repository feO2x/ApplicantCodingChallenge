import { I18N } from 'aurelia-i18n';
import { inject } from 'aurelia-framework';
import { allLocales, getLocaleOptionByLanguageCode, LocaleOption } from 'locales/all-locales';

@inject(I18N)
export class ChangeLanguageButton {

    currentLocale: LocaleOption;

    constructor(private readonly i18n: I18N) {
        this.updateCurrentLocale(i18n.getLocale());
    }

    get allLocales(): LocaleOption[] {
        return allLocales;
    }

    onMenuItemSelected(newLocale: string): void {
        if (this.currentLocale.languageCode === newLocale)
            return;

        this.updateCurrentLocale(newLocale);
        this.i18n.setLocale(newLocale);
    }

    private updateCurrentLocale(newLocale: string): void {
        this.currentLocale = getLocaleOptionByLanguageCode(newLocale);
    }
 }
