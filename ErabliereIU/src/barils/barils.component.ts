import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Baril } from 'src/model/baril';
import { NgFor, NgIf } from '@angular/common';

@Component({
    selector: 'barils-panel',
    templateUrl: './barils.component.html',
    standalone: true,
    imports: [NgFor, NgIf]
})
export class BarilsComponent implements OnInit, OnChanges {
    barils?:Array<Baril>;
    @Input() erabliereId:any
    errorMessage?: string;

    constructor(private _erabliereApi : ErabliereApi) { }

    ngOnChanges(changes: SimpleChanges): void {
        this.fetchBaril();
    }

    ngOnInit() {
        this.fetchBaril();
    }

    fetchBaril() {
        this._erabliereApi.getBarils(this.erabliereId)
                          .then(d => {
                            this.barils = d
                            this.errorMessage = undefined
                          })
                          .catch(e => {
                            this.barils = undefined
                            this.errorMessage = e
                          })
    }
}