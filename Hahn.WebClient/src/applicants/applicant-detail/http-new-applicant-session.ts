import { AxiosError, AxiosInstance } from "axios";
import { ApplicantProperties, Applicant } from "../applicant";
import { NewApplicantSession } from "./new-applicant-session";
import { axiosRegistrationKey } from 'http-client/http-client-module';
import { inject } from 'aurelia-framework';
import { NewApplicantResult } from "./new-applicant-result";

@inject(axiosRegistrationKey)
export class HttpNewApplicantSession implements NewApplicantSession {

    constructor(private readonly axios: AxiosInstance) { }

    createNewApplicant(applicant: ApplicantProperties): Promise<NewApplicantResult> {
        return new Promise<NewApplicantResult>((resolve, reject) => {
            this.axios
                .post<Applicant>('/api/applicants/new', applicant)
                .then(response => resolve({ hasErrors: false, createdApplicant: response.data }))
                .catch((error: AxiosError) => {
                    if (error.response && error.response.status === 400 && error.response.data.errors) {
                        resolve({ hasErrors: true, errors: error.response.data.errors });
                        return;
                    }

                    console.error(error.toJSON());
                    reject();
                });
        });
    }

}
