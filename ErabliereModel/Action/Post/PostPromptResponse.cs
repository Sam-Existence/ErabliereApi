using System;

namespace ErabliereModel.Action.Post
{
    public class PostPromptResponse
    {
        public PostPrompt? Prompt { get; set; }

        public Conversation? Conversation { get; set; }

        public Message? Response { get; set; }
    }
}