dotnet run --project VoicebotAiFoundry.Api


http://localhost:5000/swagger/index.html

dotnet clean
dotnet build
dotnet run --project VoicebotAiFoundry.Api


Body:

{
  "message": "hola, buenos días",
  "channel": "VOICE",  //("VOICE", "CHAT", "LEGAL").
  "skill": "statement", 
  "documentNumber": "123456",
  "documentType": "CC"
}

{
  "message": "hola, buenos días",
  "channel": "chat",
  "skill": "VOICE",
  "documentNumber": "123456",
  "documentType": "CC"
}


// Currency convert:
// What would you like to do?
// How much is 60 USD in new zealand dollars?

//Destination suggestions:
// What would you like to do?
// I'm planning an anniversary trip with my spouse, but they are currently using a wheelchair and accessibility is a must. What are some destinations that would be romantic for us?

//activity suggestions:
// What would you like to do?
// What are some things to do in Barcelona?

// Phrases:
// What would you like to do?
// please give me helpful phrases in spanish.


