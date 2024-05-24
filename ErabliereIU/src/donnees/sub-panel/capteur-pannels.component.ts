import { CdkDrag, CdkDragDrop, CdkDragMove, CdkDropList, moveItemInArray } from '@angular/cdk/drag-drop';
import { NgFor, NgIf } from '@angular/common';
import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Capteur } from 'src/model/capteur';
import { Erabliere } from 'src/model/erabliere';
import { WeatherForecastComponent } from '../weatherforecast.component';
import { GraphPannelComponent } from './graph-pannel.component';
import { ImagePanelComponent } from './image-pannel.component';

@Component({
    selector: 'capteur-pannels',
    templateUrl: './capteur-pannels.component.html',
    styleUrls: ['./capteur-pannels.component.css'],
    standalone: true,
    imports: [NgIf, NgFor, GraphPannelComponent, WeatherForecastComponent, ImagePanelComponent, CdkDrag, CdkDropList],
})
export class CapteurPannelsComponent implements OnInit {
  @ViewChild('dropListContainer') dropListContainer?: ElementRef;

  @Input() capteurs?: Capteur[];
  @Input() erabliere?: Erabliere

  dropListReceiverElement?: HTMLElement;
  dragDropInfo?: { dragIndex: number; dropIndex: number; };

  constructor(private api: ErabliereApi, private route: ActivatedRoute) { }

  ngOnInit(): void {
    console.log('CapteurPannelsComponent.ngOnInit', this.erabliere?.id);

    this.route.paramMap.subscribe(params => {
      if (this.erabliere != null) {
        this.erabliere.id = params.get('idErabliereSelectionee');
      }
    });
  }
    const drag = event.item;
    const dropList = event.container;
    const dragIndex = drag.data;

    this.dragDropInfo = { dragIndex, dropIndex };
    console.log('dragEntered', { dragIndex, dropIndex });

    const phContainer = dropList.element.nativeElement;
    const phElement = phContainer.querySelector('.cdk-drag-placeholder');

    if (phElement) {
      phContainer.removeChild(phElement);
      phContainer.parentElement?.insertBefore(phElement, phContainer);

      moveItemInArray(this.capteurs!, dragIndex, dropIndex);
    }
  }

  dragMoved(event: CdkDragMove<number>) {
    if (!this.dropListContainer || !this.dragDropInfo) return;

    const placeholderElement =
      this.dropListContainer.nativeElement.querySelector(
        '.cdk-drag-placeholder'
      );

    const receiverElement =
      this.dragDropInfo.dragIndex > this.dragDropInfo.dropIndex
        ? placeholderElement?.nextElementSibling
        : placeholderElement?.previousElementSibling;

    if (!receiverElement) {
      return;
    }

    receiverElement.style.display = 'none';
    this.dropListReceiverElement = receiverElement;
  }

  dragDropped(event: CdkDragDrop<number>) {
    if (!this.dropListReceiverElement) {
      return;
    }

    this.dropListReceiverElement.style.removeProperty('display');
    this.dropListReceiverElement = undefined;
    this.dragDropInfo = undefined;
  }

  dragEnd(event: CdkDragEnd) {
    // let position = new PutPositionGraph();
    // position.id = this.capteurs[event.source.data].id;
    // position.d = "2024-04-15T14:58:10.604Z";
    // position.position = this.dragDropInfo?.dropIndex;
    // if (position.position === undefined) {
    //   return;
    // }
    // position.idErabliere = this.erabliere.id;

    // this._erabliereApi.putPositionGraph(this.erabliereId, position.id, position).then(resp => {
    //     console.log(resp);
    // });
}

getPositionGraph() {
  //   this._erabliereApi.getPositionGraph(this.erabliereId).then(resp => {
  //     console.log(resp);
  //     let positionGraph: any = resp[0].position;
  //     let removedItem = this.items.splice(this.items.indexOf("Temperature"), 1)[0];
  //     this.items.splice(positionGraph, 0, removedItem);

  //     positionGraph = resp[1].position;
  //     removedItem = this.items.splice(this.items.indexOf("Vaccium"), 1)[0];
  //     this.items.splice(positionGraph, 0, removedItem);

  //     positionGraph = resp[2].position;
  //     removedItem = this.items.splice(this.items.indexOf("Niveau Bassin"), 1)[0];
  //     this.items.splice(positionGraph, 0, removedItem);

  //     positionGraph = resp[3].position;
  //     removedItem = this.items.splice(this.items.indexOf("Dompeux"), 1)[0];
  //     this.items.splice(positionGraph, 0, removedItem);

  //     positionGraph = resp[4].position;
  //     removedItem = this.items.splice(this.items.indexOf("Weather"), 1)[0];
  //     this.items.splice(positionGraph, 0, removedItem);

  //     positionGraph = resp[5].position;
  //     removedItem = this.items.splice(this.items.indexOf("Images"), 1)[0];
  //     this.items.splice(positionGraph, 0, removedItem);
  //   });
  }
}
