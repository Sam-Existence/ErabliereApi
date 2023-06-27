// A component pour lister les capteurs

import { Component, Input, OnInit } from "@angular/core";
import { Capteur } from "src/model/capteur";

@Component({
    selector: 'capteur-list',
    templateUrl: 'capteur-list.component.html'
    })
export class CapteurListComponent implements OnInit {

    @Input() idErabliere?: string;
    @Input() capteurs: Capteur[] = [];

    constructor() {
    }

    ngOnInit(): void {
    }

    showModifierCapteur(capteur: Capteur) {

    }

    supprimerCapteur(capteur: Capteur) {

    }

    formatDateJour(date?: Date | string): string {
        if (!date) {
            return "";
        }

        if (typeof date === "string") {
            date = new Date(date);
        }
        return date.toLocaleDateString("fr-CA");
    }
}