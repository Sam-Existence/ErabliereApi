import { DonneeCapteur } from "./donneeCapteur";

export class PutCapteur {
    id?: number;
    indice?: number;
    nom?: string;
    symbole?: string;
    afficherCapteurDashboard?: boolean;
    ajouterDonneeDepuisInterface: boolean = false
    dc?: string;
    idErabliere?: string;
}
