export class ApplicantList {

    id: number;

    activate(queryParameters: { id: number }): void {
        this.id = queryParameters.id;
    }

}
