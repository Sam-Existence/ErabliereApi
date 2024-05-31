import { DonneeCapteur } from "./donneeCapteur";

export class Capteur {
    id?: string;
    idErabliere?: string;
    nom?: string;
    symbole?: string;
    afficherCapteurDashboard?: boolean;
    ajouterDonneeDepuisInterface: boolean = false
    dc?: string;
    donnees?: DonneeCapteur[];
    indiceOrdre?: number;
    taille?: number;
}
