import {Component, EventEmitter, Input, OnChanges, Output, SimpleChanges} from '@angular/core';
import {CommonModule} from "@angular/common";
import {OnInit} from "@angular/core";

@Component({
    selector: 'app-pagination',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './pagination.component.html'
})
export class PaginationComponent implements OnChanges {
    @Input() nombreParPage: number = 1;
    @Input() nombreElements: number = 1;
    @Output() changementDePageEvent = new EventEmitter<number>();

    // Initialise un tableau avec le numéro des pages pour l'affichage dans le template
    pages: Array<number> = [];

    pageActuelle: number = 1;

    get nombrePages() {
        return this.pages.length;
    }

    get numeroPremierElementDeLaPage() {
        return (this.pageActuelle - 1) * this.nombreParPage + 1
    }

    get numeroDernierElementDeLaPage() {
        return this.pageActuelle * this.nombreParPage
    }

    get estDernierePage() {
        return this.nombrePages <= this.pageActuelle;
    }

    get estPremierePage() {
        return this.pageActuelle === 1
    }

    ngOnChanges(changes: SimpleChanges) {
        if(changes.nombreElements) {
            if(changes.nombreElements.currentValue) {
                this.pages = Array(Math.ceil(this.nombreElements / this.nombreParPage)).fill(null).map((_, i) => i + 1);
            } else {
                this.pages = [1];
            }
        }
    }

    pagePrecedente(): void {
        if (!this.estPremierePage) {
            --this.pageActuelle;
        }
        this.changementDePageEvent.emit(this.pageActuelle);
    }

    pageSuivante(): void {
        if(!this.estDernierePage) {
            ++this.pageActuelle;
        }
        this.changementDePageEvent.emit(this.pageActuelle);
    }

    changerPage(page: number): void {
        if(page >= 1 && page <= this.nombrePages && page !== this.pageActuelle) {
            this.pageActuelle = page;
            this.changementDePageEvent.emit(this.pageActuelle);
        }
    }
}
