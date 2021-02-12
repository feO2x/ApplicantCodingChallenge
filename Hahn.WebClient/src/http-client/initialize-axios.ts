import axios, { AxiosInstance } from 'axios';

export function initializeAxios(baseUrl?: string): AxiosInstance {

    const httpClient = axios.create({
        baseURL: baseUrl,
    });

    return httpClient;
}
