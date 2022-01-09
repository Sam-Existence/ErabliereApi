import { DonneeCapteur } from "./donneeCapteur";

export class Capteur {
    id?: number;
    nom?: string;
    symbole?: string;
    afficherCapteurDashboard?: boolean;
    ajouterDonneeDepuisInterface: boolean = false
    dc?: string;
    donnees?: DonneeCapteur[];
}