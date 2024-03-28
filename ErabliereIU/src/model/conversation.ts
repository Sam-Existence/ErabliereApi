export class Conversation {
    id: any;
    userId?: string;
    name?: string;
    createdOn?: Date;
    lastMessageDate?: Date;
    messages?: Message[];
}

export class Message {
    id: any;
    conversationId?: any;
    content?: string;
    createdAt?: Date;
    isUser?: boolean;
}

export class PromptResponse {
    prompt: any;
    conversation?: Conversation;
    response?: any;
}