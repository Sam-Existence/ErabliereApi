import {Component, Input, OnInit} from '@angular/core';
import {ErabliereSideBarComponent} from "./erablieres-side-bar/erablieres-side-bar.component";
import {NavigationEnd, Router, RouterOutlet} from "@angular/router";
import {ClientNavBarComponent} from "./client-nav-bar/client-nav-bar.component";
import {YouAreNotConnectedComponent} from "./you-are-not-connected/you-are-not-connected.component";

@Component({
  selector: 'client-view',
  standalone: true,
    imports: [
        ErabliereSideBarComponent,
        RouterOutlet,
        ClientNavBarComponent,
        YouAreNotConnectedComponent
    ],
  templateUrl: './client-view.component.html'
})
export class ClientViewComponent implements OnInit {
    private _pagesSansMenu = ["apropos"];
    @Input() idErabliereSelectionee?: string;
    showMenu: boolean = true;
    thereIsAtLeastOneErabliere: boolean = false;

    constructor(private _router: Router) {
    }

    ngOnInit(): void {
        let splitUrl = this._router.url.split("/");

        if (splitUrl.length > 1) {
            let page = splitUrl[1];
            this.showMenu = !this._pagesSansMenu.includes(page);
        }

        // update showMenu when the route changes
        this._router.events.subscribe((val) => {
            if (val instanceof NavigationEnd) {
                const urlParts = val.url.split('/');
                if (urlParts.length > 1) {
                    this.showMenu = !this._pagesSansMenu.includes(urlParts[1]);
                }
            }
        });
    }
}
