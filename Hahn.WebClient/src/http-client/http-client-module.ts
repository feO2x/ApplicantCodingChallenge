import { Container } from 'aurelia-dependency-injection';
import { initializeAxios } from './initialize-axios';

export const axiosRegistrationKey = "axios-instance";

export function registerHttpClientModule(container: Container, baseUrl?: string): void {

    const instance = initializeAxios(baseUrl);
    container.registerInstance(axiosRegistrationKey, instance);
}
