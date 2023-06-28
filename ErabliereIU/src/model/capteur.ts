import { DonneeCapteur } from "./donneeCapteur";

export class Capteur {
    id?: string;
    nom?: string;
    symbole?: string;
    afficherCapteurDashboard?: boolean;
    ajouterDonneeDepuisInterface: boolean = false
    dc?: string;
    donnees?: DonneeCapteur[];
}