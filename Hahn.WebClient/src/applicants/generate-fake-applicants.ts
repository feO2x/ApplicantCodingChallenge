import * as faker from 'faker';
import { Applicant } from './applicant';

export function generateFakeApplicants(numberOfApplicants = 100, startAtId = 1): Applicant[] {
    if (numberOfApplicants < 1)
        throw new Error('numberOfApplicants must be greater than zero.');
    
    const applicants: Applicant[] = [];
    for (let i = 0; i < numberOfApplicants; i++) {
        const firstName = faker.name.firstName();
        const lastName = faker.name.lastName();
        
        const applicant: Applicant = {
            id: i + startAtId,
            firstName,
            lastName,
            dateOfBirth: faker.date.between("1960-01-01", "2000-12-31").toDateString(),
            address: `${faker.address.streetAddress()}, ${faker.address.zipCode()} ${faker.address.country()}`,
            emailAddress: faker.internet.email(firstName, lastName),
            countryOfOrigin: faker.address.country(),
            isHired: faker.random.boolean()
        };
        applicants.push(applicant)        
    }

    return applicants;
}
