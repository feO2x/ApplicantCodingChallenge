import { AxiosInstance } from 'axios';
import { axiosRegistrationKey } from 'http-client/http-client-module';
import { inject } from 'aurelia-framework';
import { Applicant } from '../applicant';
import { EditApplicantSession } from './edit-applicant-session';

@inject(axiosRegistrationKey)
export class HttpEditApplicantSession implements EditApplicantSession {

    constructor(private readonly axios: AxiosInstance) { }

    async getApplicant(id: number): Promise<Applicant> {
        const response = await this.axios.get<Applicant>('/api/applicants/' + id);
        return response.data;
    }

    async updateApplicant(applicant: Applicant): Promise<void> {
        await this.axios.put('/api/applicants/update', applicant);
    }

}
