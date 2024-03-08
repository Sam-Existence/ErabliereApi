import { NgFor, NgIf } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import AgoraRTC, { IAgoraRTCClient } from 'agora-rtc-sdk-ng';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { IRtc, StreamService } from 'src/core/stream.service';
import { EnvironmentService } from 'src/environments/environment.service';
import { Customer } from 'src/model/customer';

@Component({
    selector: 'agora-call-service',
    template: `
        <div class="input-group mb-3">
            <label class="input-group-text" for="inputGroupSelect01">Appeler</label>
            <select class="form-select" id="inputGroupSelect01" (change)="onChange($event.target)">
                <option *ngFor="let user of userList" [value]="user.id">{{ user.name }} ({{ user.email }})</option>
            </select>
        </div>
        <button *ngIf="!callIsStarted" class="btn btn-primary" (click)="startCall()">Start Call</button>
        <button *ngIf="callIsStarted" class="btn btn-danger" (click)="logout()">End Call</button>
        <div [hidden]="!callIsStarted">
            <div id="video-container" class="row video-group">
                <div class="col">
                    <p id="local-player-name" class="player-name"></p>
                <div id="local-player" class="player"></div>
            </div>
            <div class="w-100">Remote Users</div>
                <div class="col" *ngFor="let i of stream.remoteUsers">
                    <div id="{{ 'remote-playerlist' + i.uid }}" class="ui centered medium image" style="width: 200px; height: 200px;">{{i}}</div>
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
            overflow: auto;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
            z-index: 1000;
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
    `],
    standalone: true,
    imports: [NgFor, NgIf]
})
export class AgoraCallServiceComponent {
    selectedUser?: string = "";
    userList: Customer[] = [];
    authService: IAuthorisationSerivce;
    callIsStarted: boolean = false;

    constructor(authFactoryService: AuthorisationFactoryService,
        private environmentService: EnvironmentService,
        private cdr: ChangeDetectorRef,
        private router: Router,
        private api: ErabliereApi,
        public stream: StreamService) {
        this.authService = authFactoryService.getAuthorisationService();
    }

    onChange(userId?: any) {
        console.log("Event", userId);
        this.selectedUser = userId.value;
        console.log("SelectedUser", this.selectedUser);
    }

    async startCall() {
        this.stream.initOption();
        const uid = this.stream.generateUid();
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