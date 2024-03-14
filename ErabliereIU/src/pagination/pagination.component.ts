import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ErabliereApi} from "../core/erabliereapi.service";
import {Documentation} from "../model/documentation";
import {CommonModule} from "@angular/common";

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.component.html'
})
export class PaginationComponent {
  count: number = 0;
  pageActuelle: number = 1;
  @Input() nombreParPage: number = 2
  @Input() estDernierePage: boolean = false;
  @Output() changementDePageEvent = new EventEmitter<number>();


  pagePrecedente() : void {
    if(this.pageActuelle > 1) {
      --this.pageActuelle;
    }
    this.changerPage();
  }

  pageSuivante() : void {
    ++this.pageActuelle;
    this.changerPage();
  }

  private changerPage() : void {
    this.changementDePageEvent.emit(this.pageActuelle);
  }
}
