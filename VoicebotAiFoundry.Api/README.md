dotnet run --project VoicebotAiFoundry.Api


http://localhost:5000/swagger/index.html

dotnet clean
dotnet build
dotnet run --project VoicebotAiFoundry.Api


Body:

{
  "message": "hola, buenos días",
  "channel": "chat",
  "skill": "statement",
  "documentNumber": "123456",
  "documentType": "CC"
}


