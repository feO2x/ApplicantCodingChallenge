import { ApplicantsSession } from "./applicants-session";
import { inject } from 'aurelia-framework';
import { AxiosInstance, AxiosRequestConfig } from 'axios';
import { axiosRegistrationKey } from 'http-client/http-client-module';
import { ApplicantsPageDto } from "./applicants-page-dto";

@inject(axiosRegistrationKey)
export class HttpApplicantsSession implements ApplicantsSession {

    constructor(private readonly axios: AxiosInstance) { }

    async getApplicants(skip: number, take: number, searchTerm?: string): Promise<ApplicantsPageDto> {
        const requestConfig: AxiosRequestConfig = {
            params: {
                skip,
                take,
                searchTerm
            }
        };
        const response = await this.axios.get<ApplicantsPageDto>('/api/applicants', requestConfig);
        return response.data;
    }

}
