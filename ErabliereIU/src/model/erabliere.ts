import { Capteur } from "./capteur";

export class Erabliere {
    id?: any;
    nom?: string;
    ipRule?: string;
    indiceOrdre?: number;
    afficherSectionBaril?: boolean;
    afficherTrioDonnees?: boolean;
    afficherSectionDompeux?: boolean;
    capteurs?: Array<Capteur>;
}