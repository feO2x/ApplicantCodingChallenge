import { bindable } from 'aurelia-framework';

export class ApplicantListCommandBar {
    @bindable
    isCommandBarVisible: boolean;

    @bindable
    searchTerm = "";
}
