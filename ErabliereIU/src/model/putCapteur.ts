import { DonneeCapteur } from "./donneeCapteur";

export class PutCapteur {
    id?: number;
    idErabliere?: string;
    nom?: string;
    symbole?: string;
    afficherCapteurDashboard?: boolean;
    ajouterDonneeDepuisInterface: boolean = false
    dc?: string;
    taille?: number;
}
