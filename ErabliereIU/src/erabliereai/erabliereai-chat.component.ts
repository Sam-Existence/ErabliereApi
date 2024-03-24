import { NgFor, NgIf } from '@angular/common';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { formatDistanceToNow } from 'date-fns';
import { HostListener } from '@angular/core';
import { fr } from 'date-fns/locale';
import { PromptResponse } from 'src/model/conversation';

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
    top = 10;
    skip = 0;
    displaySearch = false;
    search = '';
    lastSearch = '';

    constructor(private api: ErabliereApi) {
        this.conversations = [];
        this.messages = [];
        this.typePrompt = 'Chat';
    }

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

    fetchConversations() {
        this.api.getConversations(this.search, this.top, this.skip).then(async (conversations) => {
            if (conversations) {
                if (this.currentConversation == null || this.search != this.lastSearch) {
                    this.conversations = conversations;
                    if (this.conversations.length > 0) {
                        this.currentConversation = this.conversations[0];
                        const currentMessages = await this.api.getMessages(this.currentConversation.id);
                        if (currentMessages) {
                            this.messages = currentMessages;
                        }
                        else {
                            this.messages = [];
                        }
                    }
                }
                else {
                    var newConversations = conversations.find((c) => {
                        return c.id === this.currentConversation.id;
                    });
                    if (newConversations) {
                        this.currentConversation = newConversations;
                        const currentMessages = await this.api.getMessages(this.currentConversation.id);
                        if (currentMessages) {
                            this.messages = currentMessages;
                        }
                        else {
                            this.messages = [];
                        }
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
            this.lastSearch = this.search;
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
        this.api.postPrompt(prompt).then((response: PromptResponse) => {
            this.newMessage = '';
            this.aiIsThinking = false;
            const newMessages = response.conversation?.messages;
            if (newMessages) {
                this.messages = newMessages;
            }
            if (this.currentConversation == null) {
                this.currentConversation = response.conversation;
                this.conversations.unshift(this.currentConversation);
            }
        }).catch((error) => {
            this.aiIsThinking = false;
            alert('Error sending message ' + error);
        });
    }

    async selectConversation(conversation: any) {
        this.currentConversation = conversation;
        if (this.currentConversation) {
            const currentMessages = await this.api.getMessages(this.currentConversation.id);
            if (currentMessages) {
                this.messages = currentMessages;
            }
            else {
                this.messages = [];
            }
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

    traduire(message: string, index: number) {
        this.api.traduire(message).then((response: any) => {
            this.messages[index].content = response[0].translations[0].text;
        }).catch((error: any) => {
            alert('Error sending message ' + JSON.stringify(error));
        });
    }

    searchConversation($event: Event) {
        this.skip = 0;
        this.top = 10;
        this.search = ($event.target as HTMLInputElement).value;
    }

    hideDisplaySearch() {
        this.displaySearch = !this.displaySearch;

        if (this.displaySearch == false) {
            this.search = '';
            this.fetchConversations();
        }
    }

    loadMore() {
        this.skip += this.top;
        this.api.getConversations(this.search, this.top, this.skip).then((conversations) => {
            this.conversations = this.conversations.concat(conversations);
        });
    }

    elispseText(text: string, nbChar: number) {
        if (text.length > nbChar) {
            return text.substr(0, nbChar) + '...';
        }
        else {
            return text;
        }
    }
}