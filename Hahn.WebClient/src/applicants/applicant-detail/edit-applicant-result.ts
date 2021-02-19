export interface EditApplicantResult {
    hasErrors: boolean;
    errors?: { [key: string]: string[] }
}
