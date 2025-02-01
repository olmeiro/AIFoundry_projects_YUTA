namespace VoicebotAiFoundry.Domain.Entities
{
    public class DemandasRequest
    {
        public string CaseDescription { get; set; }

        public DemandasRequest() {}

        public DemandasRequest(string caseDescription)
        {
            CaseDescription = caseDescription;
        }
    }
}
