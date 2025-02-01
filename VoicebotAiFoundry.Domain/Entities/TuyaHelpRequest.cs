namespace VoicebotAiFoundry.Domain.Entities
{
    public class TuyaHelpRequest
    {
        public string Query { get; set; }

        public TuyaHelpRequest() {}

        public TuyaHelpRequest(string query)
        {
            Query = query;
        }
    }
}
