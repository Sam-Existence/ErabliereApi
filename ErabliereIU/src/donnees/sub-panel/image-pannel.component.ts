import { CdkDragHandle } from '@angular/cdk/drag-drop';
import { NgFor, NgIf } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { GetImageInfo } from 'src/model/imageInfo';

@Component({
    selector: 'image-panel',
    templateUrl: "./image-pannel.component.html",
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
    imports: [NgFor, NgIf, CdkDragHandle]
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
    modalTitle: string = "Image";

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
        this.api.getImages(this.idErabliereSelectionnee, this.modalTake, this.modalSkip, this.search).then(images => {
            if (images.length > 0) {
                this.selectedImage = images[0].images;
                this.selectedImageMetadata = images[0];
                this.azureImageAPIInfo = JSON.stringify(JSON.parse(images[0].azureImageAPIInfo ?? ""), null, 2);
                this.modalHasNext = true;
                this.modalTitle = images[0].name ?? "Image";
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
        this.modalTitle = image.name ?? "Image";
    }

    searchFromInput(event: any) {
        console.log("searchFromInput", event);
        const search = event.target.value;
        this.search = search;
        console.log("search", search);
        this.fetchImages();
    }
}