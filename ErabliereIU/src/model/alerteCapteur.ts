import { Capteur } from "./capteur";

export class AlerteCapteur {
    id?:any;
    idCapteur?:any;
    envoyerA?:string;
    minVaue?:number;
    maxValue?:number;
    dc?:string
    isEnable?: boolean;

    capteur?:Capteur;
}