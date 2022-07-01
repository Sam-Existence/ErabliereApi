import { Capteur } from "./capteur";

export class Erabliere {
    id?: any;
    nom?: string;
    ipRule?: string;
    indiceOrdre?: number;
    isPublic?: boolean;
    afficherSectionBaril?: boolean;
    afficherTrioDonnees?: boolean;
    afficherSectionDompeux?: boolean;
    capteurs?: Array<Capteur>;
}