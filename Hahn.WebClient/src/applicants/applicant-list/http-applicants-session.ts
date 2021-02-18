import { ApplicantsSession } from "./applicants-session";
import { inject } from 'aurelia-framework';
import { AxiosInstance } from 'axios';
import { axiosRegistrationKey } from 'http-client/http-client-module';
import { ApplicantsPageDto } from "./applicants-page-dto";

@inject(axiosRegistrationKey)
export class HttpApplicantsSession implements ApplicantsSession {

    constructor(private readonly axios: AxiosInstance) { }

    async getApplicants(skip: number, take: number, searchTerm?: string): Promise<ApplicantsPageDto> {

        const params: { skip: number, take: number, searchTerm?: string } = {
            skip,
            take
        };

        if (searchTerm)
            params.searchTerm = searchTerm;

        const response = await this.axios.get<ApplicantsPageDto>('/api/applicants', { params });
        return response.data;
    }

}
