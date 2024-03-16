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
                        <img 
                            src="data:image/png;base64,{{ image.images }}" 
                            class="img-thumbnail trigger-modal" 
                            alt="image" 
                            style="width: 100%; height: 100%;" 
                            data-bs-toggle="modal" 
                            data-bs-target="#imageModal" 
                            (click)="selectedImage = image.images">
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="imageModal" tabindex="-1" aria-labelledby="imageModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content">
                <div class="modal-body">
                    <img 
                        [src]="'data:image/png;base64,' + selectedImage" 
                        class="img-fluid modal-image-content" 
                        alt="Selected Image">
                </div>
                </div>
            </div>
        </div>
    `,
    styles: [`
        /* Style the Image Used to Trigger the Modal */
        .trigger-modal {
            border-radius: 5px;
            cursor: pointer;
            transition: 0.3s;
        }

            .trigger-modal:hover {
                opacity: 0.7;
                cursor: pointer;
            }

        /* The Modal (background) */
        .modal-image {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 100px; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.9); /* Black w/ opacity */
        }

        /* Modal Content (Image) */
        .modal-image-content {
            margin: auto;
            display: block;
            width: 80%;
            max-width: 700px;
        }

        /* Caption of Modal Image (Image Text) - Same Width as the Image */
        #caption {
            margin: auto;
            display: block;
            width: 80%;
            max-width: 700px;
            text-align: center;
            color: #ccc;
            padding: 10px 0;
            height: 150px;
        }

        /* Add Animation - Zoom in the Modal */
        .modal-image-content, #caption {
            animation-name: zoom;
            animation-duration: 0.6s;
        }

        @keyframes zoom {
            from {
                transform: scale(0)
            }

            to {
                transform: scale(1)
            }
        }

        /* The Close Button */
        .closeImageModel {
            position: absolute;
            top: 15px;
            right: 35px;
            color: #f1f1f1;
            font-size: 40px;
            font-weight: bold;
            transition: 0.3s;
        }

            .closeImageModel:hover,
            .closeImageModel:focus {
                color: #bbb;
                text-decoration: none;
                cursor: pointer;
            }

        /* 100% Image Width on Smaller Screens */
        @media only screen and (max-width: 700px) {
            .modal-content {
                width: 100%;
            }
        }
    `],
    standalone: true,
    imports: [NgFor, NgIf]
})
export class ImagePanelComponent implements OnInit {
    
    constructor(private api: ErabliereApi, private route: ActivatedRoute) {

    }

    @Input() idErabliereSelectionnee: any;
    images: any[] = [];
    selectedImage?: string;

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