import {
    Component,
    EventEmitter,
    OnInit,
    Output
} from '@angular/core';
import {IAuthorisationSerivce} from "../iauthorisation-service";
import {AuthorisationFactoryService} from "../authorisation-factory-service";
import {MsalService} from "@azure/msal-angular";

@Component({
  selector: 'connection-button',
  standalone: true,
  imports: [],
  templateUrl: './connection-button.component.html'
})
export class ConnectionButtonComponent implements OnInit {
    private _authService: IAuthorisationSerivce;
    isLoggedIn: boolean;
    @Output() loginChange = new EventEmitter<boolean>();

    constructor(private _authFactoryService: AuthorisationFactoryService) {
        this._authService = this._authFactoryService.getAuthorisationService();
        this.isLoggedIn = false;
    }

    ngOnInit(): void {
        this._authService.isLoggedIn().then(isLoggedIn => {
            this.isLoggedIn = isLoggedIn;
            this.loginChange.emit(isLoggedIn);
        });
        this._authService.loginChanged.subscribe(isLoggedIn => {
            this.isLoggedIn = isLoggedIn;
            this.loginChange.emit(isLoggedIn);
        });
    }

    login() {
        this._authService.login();
    }

    logout() {
        this._authService.logout();
    }
}
