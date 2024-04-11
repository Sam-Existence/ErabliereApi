export class GetImageInfo {
    id: number;
    uniqueId: number;
    name?: string;
    object?: string;
    azureImageAPIInfo?: string;
    images?: string;
    dateAjout: Date;
    dateEmail?: Date;
    emailStatesId?: string;
    externalOwner?: string;

    constructor() {
        this.id = 0;
        this.uniqueId = 0;
        this.dateAjout = new Date();
    }
}