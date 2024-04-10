import { NgFor, NgIf } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { GetImageInfo } from 'src/model/imageInfo';

@Component({
    selector: 'image-panel',
    template: `
        <div class="card">
            <div class="card-header">
                <h5 class="card-title">
                    Images 
                    <span class="float-end">
                        <input 
                            type="text" 
                            class="form-control form-control-sm" 
                            placeholder="Search" 
                            aria-label="Search" 
                            aria-describedby="basic-addon1"
                            (keyup)="searchFromInput($event)">
                    </span>
                </h5>
            </div>
            <div class="card-body">
                <div class="row">
                    <!-- Previous Button -->
                    <button *ngIf="skip" class="btn btn-light btn-sm position-absolute top-50 start-0 translate-middle-y" (click)="previousImage()" style="width: 5%;">&lt;</button>

                    <!-- Next Button -->
                    <button class="btn btn-light btn-sm position-absolute top-50 end-0 translate-middle-y" (click)="nextImage()" style="width: 5%;">&gt;</button>

                    <div class="col-md-6" *ngFor="let image of images; let ls = index">
                        <img 
                            src="data:image/png;base64,{{ image.images }}" 
                            class="img-thumbnail trigger-modal" 
                            alt="image" 
                            style="width: 100%; height: 100%;" 
                            data-bs-toggle="modal" 
                            data-bs-target="#imageModal" 
                            (click)="selectImage(image, ls)">
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="imageModal" tabindex="-1" aria-labelledby="imageModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content">
                    <div class="modal-body">
                        <!-- Previous Button -->
                        <button *ngIf="modalSkip" class="btn btn-light position-absolute top-50 start-0 translate-middle-y" (click)="modalPreviousImage()">&lt;</button>

                        <!-- Next Button -->
                        <button *ngIf="modalHasNext" class="btn btn-light position-absolute top-50 end-0 translate-middle-y" (click)="modalNextImage()">&gt;</button>

                        <img 
                            [src]="'data:image/png;base64,' + selectedImage" 
                            class="img-fluid modal-image-content" 
                            alt="Selected Image">
                    </div>
                    <div class="modal-footer">
                        <pre class="float-start" style="display: block; width: 100%;">{{azureImageAPIInfo}}</pre>
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
    images: GetImageInfo[] = [];
    selectedImageMetadata: GetImageInfo = new GetImageInfo();
    azureImageAPIInfo: string = "";
    selectedImage?: string;
    take: number = 2;
    skip: number = 0;
    search: string = "";
    modalTake: number = 1;
    modalSkip: number = 0;
    modalHasNext: boolean = true;

    ngOnInit(): void {
        this.route.params.subscribe(params => {
            this.idErabliereSelectionnee = params['idErabliereSelectionee'];
            this.fetchImages();
        });
        this.imageInterval = setInterval(() => {
            this.fetchImages();
        }, 1000 * 60 * 10);
    }

    ngOnDestroy() {
        clearInterval(this.imageInterval);
    }

    imageInterval: any;

    fetchImages() {
        console.log("fetch image for", this.idErabliereSelectionnee);
        this.api.getImages(
                this.idErabliereSelectionnee, this.take, this.skip, this.search)
            .then(images => {
                this.images = images;
            });
    }
    
    nextImage() {
        console.log("next image");
        this.take = 2;
        this.skip += this.take;
        this.fetchImages();
    }
        
    previousImage() {
        console.log("previous image");
        this.take = 2;
        this.skip -= this.take;
        if (this.skip < 0) {
            this.skip = 0;
        }
        this.fetchImages();
    }

    fetchModalImages() {
        console.log("fetch image for", this.idErabliereSelectionnee);
        this.api.getImages(this.idErabliereSelectionnee, this.modalTake, this.modalSkip).then(images => {
            if (images.length > 0) {
                this.selectedImage = images[0].images;
                this.selectedImageMetadata = images[0];
                this.azureImageAPIInfo = JSON.stringify(JSON.parse(images[0].azureImageAPIInfo ?? ""), null, 2);
                this.modalHasNext = true;
            }
            else {
                this.modalHasNext = false;
                this.modalSkip -= this.modalTake;
            }
        });
    }

    modalNextImage() {
        console.log("modal next image");
        this.modalTake = 1;
        this.modalSkip += this.modalTake;
        this.fetchModalImages();
    }
        
    modalPreviousImage() {
        console.log("modal previous image");
        this.modalTake = 1;
        this.modalSkip -= this.modalTake;
        if (this.modalSkip < 0) {
            this.modalSkip = 0;
        }
        this.fetchModalImages();
    }

    selectImage(image: GetImageInfo, localSkip: number) {
        this.selectedImage = image.images;
        this.selectedImageMetadata = image;
        this.azureImageAPIInfo = JSON.stringify(JSON.parse(image.azureImageAPIInfo ?? ""), null, 2);
        this.modalTake = 1;
        this.modalSkip = this.skip + localSkip;
        if (this.modalSkip < 0) {
            this.modalSkip = 0;
        }
    }

    searchFromInput(event: any) {
        console.log("searchFromInput", event);
        const search = event.target.value;
        this.search = search;
        console.log("search", search);
        this.fetchImages();
    }
}