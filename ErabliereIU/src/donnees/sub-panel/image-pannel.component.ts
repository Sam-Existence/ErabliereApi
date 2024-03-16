import { NgFor, NgIf } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErabliereApi } from 'src/core/erabliereapi.service';

@Component({
    selector: 'image-panel',
    template: `
        <div class="card">
            <div class="card-header">
                <h5 class="card-title">Images</h5>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-6" *ngFor="let image of images">
                        <img src="data:image/png;base64,{{ image.images }}" class="img-thumbnail" alt="image" style="width: 100%; height: 100%;">
                    </div>
                </div>
            </div>
        </div>
    `,
    standalone: true,
    imports: [NgFor, NgIf]
})
export class ImagePanelComponent implements OnInit {
    
    constructor(private api: ErabliereApi, private route: ActivatedRoute) {

    }

    @Input() idErabliereSelectionnee: any;
    images: any[] = [];

    ngOnInit(): void {
        this.route.params.subscribe(params => {
            this.idErabliereSelectionnee = params['idErabliereSelectionee'];
            this.fetchImages();
        });
        this.fetchImages();
    }

    fetchImages() {
        console.log("fetch image for", this.idErabliereSelectionnee)
        this.api.getImages(this.idErabliereSelectionnee, 2).then(images => {
            this.images = images;
        });
    }
}