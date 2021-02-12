import { Applicant } from "applicants/applicant";
import { ApplicantsSession } from "./applicants-session";
import { inject } from 'aurelia-framework';
import { AxiosInstance, AxiosRequestConfig } from 'axios';
import { axiosRegistrationKey } from 'http-client/http-client-module';

@inject(axiosRegistrationKey)
export class HttpApplicantsSession implements ApplicantsSession {

    constructor(private readonly axios: AxiosInstance) { }

    async getApplicants(skip: number, take: number, searchTerm?: string): Promise<Applicant[]> {
        const requestConfig: AxiosRequestConfig = {
            params: {
                skip,
                take,
                searchTerm
            }
        };
        const response = await this.axios.get<Applicant[]>('/api/applicants', requestConfig);
        return response.data;
    }

}
