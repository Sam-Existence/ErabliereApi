import { NgFor, NgIf } from '@angular/common';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { formatDistanceToNow } from 'date-fns';
import { HostListener } from '@angular/core';
import { fr } from 'date-fns/locale';

@Component({
    selector: 'app-chat-widget',
    templateUrl: './erabliereai-chat.component.html',
    styleUrls: ['./erabliereai-chat.component.css'],
    imports: [FormsModule, NgIf, NgFor],
    standalone: true
})
export class ErabliereAIComponent {
    chatOpen = false;
    aiIsThinking = false;

    toggleChat() {
        this.chatOpen = !this.chatOpen;

        if (this.chatOpen) {
            this.fetchConversations();
        }
    }

    conversations: any[];
    currentConversation: any;
    messages: any[];
    typePrompt: string;

    constructor(private api: ErabliereApi) {
        this.conversations = [];
        this.messages = [];
        this.typePrompt = 'Chat';
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
            ConversationId: this.currentConversation?.id,
            PromptType: this.typePrompt
        };
        this.aiIsThinking = true;
        this.api.postPrompt(prompt).then((response: any) => {
            this.fetchConversations();
            this.newMessage = '';
            this.aiIsThinking = false;
        }).catch((error) => {
            this.aiIsThinking = false;
            alert('Error sending message ' + error);
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

    updateMessageType($event: Event) {
        this.typePrompt = ($event.target as HTMLInputElement).value;
        console.log(this.typePrompt);
    }

    formatMessageDate(date: Date | string) {
        return formatDistanceToNow(new Date(date), { addSuffix: true, locale: fr });
    }

    @HostListener('document:keydown', ['$event'])
    handleKeyboardEvent(event: KeyboardEvent) {
        if (this.chatOpen && event.key === 'Escape') {
            this.chatOpen = false;
        }
    }
}