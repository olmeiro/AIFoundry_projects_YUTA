namespace VoicebotAiFoundry.Domain.Entities
{
    public class ChatRequest
    {
        public ChatRequest(string Message, string channel, string Skill, string? documentNumber, string? documentType)
        {
            this.Message = Message;
            Channel = channel;
            this.Skill = Skill;
            DocumentNumber = documentNumber;
            DocumentType = documentType;
        }

        public string Message { get; set; } = string.Empty;
        public string Channel { get; set; } // Valor por defecto si no se especifica
        public string Skill { get; set; } // Asegura que siempre llega un Skill
        public string? DocumentNumber { get; set; }
        public string? DocumentType { get; set; }
    }
}
