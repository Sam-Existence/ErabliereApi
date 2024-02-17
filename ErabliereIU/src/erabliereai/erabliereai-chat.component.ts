import { NgFor, NgIf } from '@angular/common';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ErabliereApi } from 'src/core/erabliereapi.service';

@Component({
    selector: 'app-chat-widget',
    templateUrl: './erabliereai-chat.component.html',
    styleUrls: ['./erabliereai-chat.component.css'],
    imports: [FormsModule, NgIf, NgFor],
    standalone: true
})
export class ErabliereAIComponent {
    chatOpen = false;

    toggleChat() {
        this.chatOpen = !this.chatOpen;

        if (this.chatOpen) {
            this.fetchConversations();
        }
    }

    conversations: any[];
    currentConversation: any;
    messages: any[];

    constructor(private api: ErabliereApi) {
        this.conversations = [];
        this.messages = [];
    }

    fetchConversations() {
        this.api.getConversations().then((conversations) => {
            if (conversations) {
                if (this.currentConversation == null) {
                    this.conversations = conversations;
                    if (this.conversations.length > 0) {
                        this.currentConversation = this.conversations[this.conversations.length - 1];
                        this.messages = this.currentConversation.messages;
                    }
                }
                else {
                    var newConversations = conversations.find((c) => {
                        return c.id === this.currentConversation.id;
                    });
                    if (newConversations) {
                        this.currentConversation = newConversations;
                        this.messages = this.currentConversation.messages;
                    }
                    else {
                        this.conversations = conversations;
                    }
                }
            }
            else {
                this.conversations = [];
                this.currentConversation = null;
                this.messages = [];
            }
        });
    }

    newMessage = '';

    updateNewMessage($event: Event) {
        this.newMessage = ($event.target as HTMLInputElement).value;
    }

    sendMessage() {
        const prompt = {
            Prompt: this.newMessage,
            ConversationId: this.currentConversation?.id
        };
        this.api.postPrompt(prompt).then((response: any) => {
            this.fetchConversations();
            this.newMessage = '';
        });
    }

    selectConversation(_t16: any) {
        this.currentConversation = _t16;
        if (this.currentConversation) {
            this.messages = this.currentConversation.messages;
        }
        else {
            this.messages = [];
        }
    }

    newChat() {
        this.currentConversation = null;
        this.messages = [];
    }

    deleteConversation(c: any) {
        if (confirm('Are you sure you want to delete this conversation?')) {
            this.api.deleteConversation(c.id).then(() => {
                if (c.id === this.currentConversation?.id) {
                    this.currentConversation = null;
                    this.messages = [];
                }
                this.fetchConversations();
            });
        }
    }
}