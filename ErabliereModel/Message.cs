using System;
using ErabliereApi.Donnees;

/// <summary>
/// Classe représentant un message dans une conversation avec ErabliereAI
/// </summary>
public class Message : IIdentifiable<Guid?, Message>
{
    /// <summary>
    /// Clé primaire
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Clé étrangère de la conversation
    /// </summary>
    public Guid? ConversationId { get; set; }

    /// <summary>
    /// La conversation à laquelle le message appartient
    /// </summary>
    public Conversation? Conversation { get; set; }

    /// <summary>
    /// Le contenu du message
    /// </summary>
    public string Content { get; set; } = "";

    /// <summary>
    /// Date de création du message
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Indique si le message a été envoyé par un utilisateur
    /// </summary>
    public bool IsUser { get; set; }

    /// <summary>
    /// Compare deux messages par leur date de création
    /// </summary>
    public int CompareTo(Message? other)
    {
        if (other == null)
        {
            return 1;
        }

        return CreatedAt.CompareTo(other.CreatedAt);
    }
}
