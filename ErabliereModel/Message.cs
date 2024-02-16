using System;
using ErabliereApi.Donnees;

public class Message : IIdentifiable<Guid?, Message>
{
    public Guid? Id { get; set; }
    public Guid? ConversationId { get; set; }
    public Conversation? Conversation { get; set; }
    public string Content { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsUser { get; set; }

    public int CompareTo(Message? other)
    {
        if (other == null)
        {
            return 1;
        }

        return CreatedAt.CompareTo(other.CreatedAt);
    }
}
