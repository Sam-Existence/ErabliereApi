export class Alerte {
  id?: any;
  idErabliere: any;
  nom?: string;
  envoyerA?: string;
  temperatureThresholdLow?: string
  temperatureThresholdHight?: string
  vacciumThresholdLow?: string
  vacciumThresholdHight?: string
  niveauBassinThresholdLow?: string
  niveauBassinThresholdHight?: string
  isEnable?: boolean;
  emails?: string[];
}
