import { NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { StreamService } from 'src/core/stream.service';
import { Customer } from 'src/model/customer';
import { Erabliere } from 'src/model/erabliere';

@Component({
    selector: 'agora-call-service',
    template: `
        <div *ngIf="!showPhone">
            <button type="button" class="btn btn-success" (click)="showPhoneForm()">
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-telephone" viewBox="0 0 16 16">
                <path d="M3.654 1.328a.678.678 0 0 0-1.015-.063L1.605 2.3c-.483.484-.661 1.169-.45 1.77a17.6 17.6 0 0 0 4.168 6.608 17.6 17.6 0 0 0 6.608 4.168c.601.211 1.286.033 1.77-.45l1.034-1.034a.678.678 0 0 0-.063-1.015l-2.307-1.794a.68.68 0 0 0-.58-.122l-2.19.547a1.75 1.75 0 0 1-1.657-.459L5.482 8.062a1.75 1.75 0 0 1-.46-1.657l.548-2.19a.68.68 0 0 0-.122-.58zM1.884.511a1.745 1.745 0 0 1 2.612.163L6.29 2.98c.329.423.445.974.315 1.494l-.547 2.19a.68.68 0 0 0 .178.643l2.457 2.457a.68.68 0 0 0 .644.178l2.189-.547a1.75 1.75 0 0 1 1.494.315l2.306 1.794c.829.645.905 1.87.163 2.611l-1.034 1.034c-.74.74-1.846 1.065-2.877.702a18.6 18.6 0 0 1-7.01-4.42 18.6 18.6 0 0 1-4.42-7.009c-.362-1.03-.037-2.137.703-2.877z"></path>
            </svg>
            Appel
        </button>
        </div>
        <div *ngIf="showPhone">
            <div class="input-group">
                <label class="input-group-text" for="inputGroupSelect01">Érablière</label>
                <select class="form-select select-ellipsis" id="inputGroupSelect01" (change)="onChange($event.target)">
                    <option 
                        class="overflow"
                        *ngFor="let e of erablieres" 
                        [value]="e.id"
                        [title]="e.nom">
                            {{ e.nom }}
                    </option>
                </select>
                <button *ngIf="!callIsStarted" class="btn btn-success" (click)="startCall()">Appeler</button>
                <button *ngIf="callIsStarted" class="btn btn-danger" (click)="logout()">Raccrocher</button>
            </div>
            <div id="video-container" [hidden]="!callIsStarted">
                <span>En appel</span>
                <div class="col-4" *ngFor="let i of stream.remoteUsers">
                    <p class="player-name">{{ i.name ?? "Inconue " + i.uid  }}</p>
                    <div id="{{ 'remote-playerlist' + i.uid }}" class="player"></div>
                </div>
                <div class="video-group">
                    <div class="col-2">
                        <p id="local-player-name" class="player-name">Vous {{ userUid }}</p>
                        <div id="local-player" style="width: 200px; height: 200px;">
                    </div>
                </div>
            </div>
        </div>
    `,
    styles: [`
        #video-container {
            position: fixed;
            bottom: 20px;
            right: 20px;
            max-height: 95%;
            max-width: 95%;
            overflow: auto;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
            z-index: 2000;
            background-color: rgba(129, 129, 129, 0.5);
            border-radius: 10px;
            border-width: 2px;
            border-color: black;
        }
        .player {
            width: 480px;
            height: 320px;
        }
        .player-name {
            margin: 8px 0;
        }
        @media (max-width: 640px) {
            .player {
                width: 320px;
                height: 240px;
            }
        }
        .select-ellipsis {
            text-overflow: ellipsis;
            white-space: nowrap;
            overflow: hidden;
            max-width: 300px;
        }
    `],
    standalone: true,
    imports: [NgFor, NgIf]
})
export class AgoraCallServiceComponent {
    erabliereId?: string = "";
    erablieres: Erabliere[] = [];
    authService: IAuthorisationSerivce;
    callIsStarted: boolean = false;
    recordingIsStarted: boolean = false;
    userUid: number = 0;
    recordingResourceId: string = "";
    recordingSId: string = "";
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

    async startRecording() {
        if(this.callIsStarted && this.erabliereId) {
            let response = await this.stream.startRecordingCall(this.userUid, this.erabliereId);
            this.recordingResourceId = response.resourceId;
            this.recordingSId = response.sid;
            this.recordingIsStarted = true;
        }
    }

    async stopRecording() {
        if(this.recordingIsStarted && this.erabliereId) {
            await this.stream.stopRecordingCall(this.userUid, this.erabliereId, this.recordingResourceId, this.recordingSId);
            this.recordingIsStarted = false
        }
    }
}
