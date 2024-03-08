import AgoraRTC, { IAgoraRTCClient, LiveStreamingTranscodingConfig, ICameraVideoTrack, IMicrophoneAudioTrack, ScreenVideoTrackInitConfig, VideoEncoderConfiguration, AREAS, IRemoteAudioTrack, ClientRole } from "agora-rtc-sdk-ng"
import { BehaviorSubject } from 'rxjs';
import { Injectable } from "@angular/core";
import { ErabliereApi } from "./erabliereapi.service";

@Injectable({ providedIn: 'root'})
export class StreamService {
  rtc: IRtc = {
    // For the local client.
    client: null,
    // For the local audio and video tracks.
    localAudioTrack: null,
    localVideoTrack: null,
  };
  options = {
    appId: "<app-id-get-from-backend>",  // set your appid here
    channel: "erabliereapi", // Set the channel name.
    // uid: null
  };
  remoteUsers: IUser[] = [];       // To add remote users in list
  updateUserInfo = new BehaviorSubject<any>(null); // to update remote users name

  constructor(public api: ErabliereApi) { }

  createRTCClient() {
    console.log("createRTCClient");
    this.rtc.client = AgoraRTC.createClient({ mode: "rtc", codec: "h264" });
    console.log(this.rtc.client, 'createRTCClient');
    console.log("*** done ceateRTCCLient ***");
  }

  // To join a call with tracks (video or audio)
  async localUser(token: string, uuid: number) {
    if (token == null) {
      throw new Error("Token is null");
    }
    console.log("localUser", token, uuid);
    const uid = await this.rtc.client?.join(this.options.appId, this.options.channel, token, uuid);
    console.log("after join", uid, 'uid');
    // Create an audio track from the audio sampled by a microphone.
    this.rtc.localAudioTrack = await AgoraRTC.createMicrophoneAudioTrack();
    // Create a video track from the video captured by a camera.
    this.rtc.localVideoTrack = await AgoraRTC.createCameraVideoTrack({
      encoderConfig: "120p",
    });

    // Publish the local audio and video tracks to the channel.
    this.rtc.localVideoTrack.play("local-player");
    // channel for other users to subscribe to it.
    await this.rtc.client?.publish([this.rtc.localAudioTrack, this.rtc.localVideoTrack]);

    console.log("*** done localUser ***");
  }

  agoraServerEvents(rtc: IRtc) {
    console.log('agoraserverevent', rtc);
    rtc.client?.on("user-published", async (user, mediaType) => {
      console.log(user, mediaType, 'user-published');

      await rtc.client?.subscribe(user, mediaType);
      if (mediaType === "video") {
        const remoteVideoTrack = user.videoTrack;
        remoteVideoTrack?.play('remote-playerlist' + user.uid);
      }
      if (mediaType === "audio") {
        const remoteAudioTrack = user.audioTrack;
        remoteAudioTrack?.play();
      }
    });
    rtc.client?.on("user-unpublished", user => {
      console.log(user, 'user-unpublished');
    });

    rtc.client?.on("user-joined", (user) => {
      let id = user.uid;
      this.remoteUsers.push({ 'uid': +id });
      this.updateUserInfo.next(id);
      console.log("user-joined", user, this.remoteUsers, 'event1');
    });
  }
  // To leave channel-
  async leaveCall() {
    // Destroy the local audio and video tracks.
    this.rtc.localAudioTrack?.close();
    this.rtc.localVideoTrack?.close();
    // Traverse all remote users.
    this.rtc.client?.remoteUsers.forEach(user => {
      // Destroy the dynamically created DIV container.
      const playerContainer = document.getElementById('remote-playerlist' + user.uid.toString());
      playerContainer && playerContainer.remove();
    });
    // Leave the channel.
    await this.rtc.client?.leave();
  }

  // rtc token
  async generateTokenAndUid(uid: number) {
    // use the nestjs backend here: https://github.com/freddycoder/LearnNestJS
    let url = 'http://localhost:3000/accesstoken/' + uid + '?channel=' + this.options.channel;
    const data = await this.api.getCallAccessToken(uid, this.options.channel);
    console.log(data);
    return { 'uid': uid, token: data.accessToken }
  }

  generateUid() {
    const length = 5;
    const randomNo = (Math.floor(Math.pow(10, length - 1) + Math.random() * 9 * Math.pow(10, length - 1)));
    return randomNo;
  }

  async initOption() {
    const data = await this.api.getCallAppId();
    console.log(data);
    this.options.appId = data.appId;
    console.log('appId is:', this.options.appId);
  }
}

export interface IUser {
  uid: number;
  name?: string;
}

export interface IRtc {
  client: IAgoraRTCClient | null,
  localAudioTrack: IMicrophoneAudioTrack | null,
  localVideoTrack: ICameraVideoTrack | null
}
