import { Component } from '@angular/core';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { StreamService } from 'src/core/stream.service';
import { Erabliere } from 'src/model/erabliere';

@Component({
    selector: 'agora-call-service',
    templateUrl: 'agora-call-service.component.html',
    styleUrls: ['agora-call-service.component.css'],
    standalone: true
})
export class AgoraCallServiceComponent {
    erabliereId?: string = "";
    erablieres: Erabliere[] = [];
    authService: IAuthorisationSerivce;
    callIsStarted: boolean = false;
    userUid: number = 0;
    showPhone: boolean = false;

    constructor(authFactoryService: AuthorisationFactoryService,
        private api: ErabliereApi,
        public stream: StreamService) {
        this.authService = authFactoryService.getAuthorisationService();
    }

    showPhoneForm() {
        this.showPhone = true;
        this.api.getErablieres(true).then((data) => {
            this.erablieres = data;
        });
    }

    onChange(erabliereId?: any) {
        this.erabliereId = erabliereId.value;
        this.stream.options.channel = this.erabliereId ?? "";
    }

    async startCall() {
        this.stream.initOption();
        const uid = this.stream.generateUid();
        this.userUid = uid;
        const rtcDetails = await this.stream.generateTokenAndUid(uid);
        this.stream.createRTCClient();
        this.stream.agoraServerEvents(this.stream.rtc);
        await this.stream.localUser(rtcDetails.token, uid);
        this.callIsStarted = true;
    }

    async logout() {
        await this.stream.leaveCall();
        this.callIsStarted = false;
    }
}
