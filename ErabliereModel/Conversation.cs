using System;
using System.Collections.Generic;
using ErabliereApi.Donnees;

public class Conversation : IIdentifiable<Guid?, Conversation>
{
    public Guid? Id { get; set; }

    public string? UserId { get; set; }

    public string? Name { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? LastMessageDate { get; set; }

    public List<Message>? Messages { get; set; }

    public int CompareTo(Conversation? other)
    {
        if (other == null)
        {
            return 1;
        }

        if (other.LastMessageDate == null)
        {
            return LastMessageDate == null ? 0 : -1;
        }

        return LastMessageDate?.CompareTo(other.LastMessageDate.Value) ?? 0;
    }
}
