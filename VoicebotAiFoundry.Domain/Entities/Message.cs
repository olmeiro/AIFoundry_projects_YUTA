namespace VoicebotAiFoundry.Domain.Entities
{
    public class Message
    {
        public string Content { get; set; }

        public Message() {} // Constructor vacío requerido para deserialización

        public Message(string content)
        {
            Content = content;
        }
    }
}
