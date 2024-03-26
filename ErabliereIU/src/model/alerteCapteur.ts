import { Capteur } from "./capteur";

export class AlerteCapteur {
  id?: any;
  idCapteur?: any;
  nom?: string;
  envoyerA?: string;
  texterA?: string;
  minVaue?: number;
  maxValue?: number;
  dc?: string
  isEnable?: boolean;
  emails?: string[];
  numeros?: string[];
  capteur?: Capteur;
}
