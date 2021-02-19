import { AxiosError, AxiosInstance } from 'axios';
import { axiosRegistrationKey } from 'http-client/http-client-module';
import { inject } from 'aurelia-framework';
import { Applicant } from '../applicant';
import { EditApplicantSession } from './edit-applicant-session';
import { EditApplicantResult } from './edit-applicant-result';

@inject(axiosRegistrationKey)
export class HttpEditApplicantSession implements EditApplicantSession {

    constructor(private readonly axios: AxiosInstance) { }
    
    async getApplicant(id: number): Promise<Applicant> {
        const response = await this.axios.get<Applicant>('/api/applicants/' + id);
        return response.data;
    }

    updateApplicant(applicant: Applicant): Promise<EditApplicantResult> {

        return new Promise<EditApplicantResult>((resolve, reject) => {
            this.axios
                .put('/api/applicants/update', applicant)
                .then(_response => resolve({ hasErrors: false }))
                .catch((error: AxiosError) => {
                    if (error.response && error.response.status === 400 && error.response.data.errors) {
                        resolve({ hasErrors: true, errors: error.response.data.errors });
                        return;
                    }

                    console.error(error.toJSON());
                    reject();
                });
        })
    }

    async deleteApplicant(id: number): Promise<void> {
        await this.axios.delete('/api/applicants/delete/' + id);
    }
}
