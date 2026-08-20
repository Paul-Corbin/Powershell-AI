using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;

namespace PowershellAI
{
    internal class Request
    {
        private HttpClient _httpClient = new HttpClient();
        private Response response;

        private string API_URL = "https://ai.hatz.ai/v1/anthropic/messages";
        private string API_MODEL = "anthropic.claude-haiku-4-5";
        private string API_KEY = "6ae32ae1-a8a9-44bb-bfd0-80a5bed25a3c"; // REMOVE, STORE IN CREDENTIAL MANAGER
        private string API_MODE = "lite"; //From fastest/least tokens -> slowest/most tokens- lite, performance, turbo
        private string MAX_TOKENS = "300000"; //Absurdly high but good for our purposes

        public async Task<Response> Submit(string submittedText)
        {
            //Build JSON object for the request
            JsonContent content = JsonContent.Create(new JsonObject{ 
            ["model"] = API_MODEL,
            ["mode"] = API_MODE,
            ["max_tokens"] = MAX_TOKENS,
            ["messages"] = new JsonArray { 
                new JsonObject{
                    ["role"]="user",
                    ["content"]=submittedText
                },
                new JsonObject{
                    ["role"]="system",
                    ["content"] = """
        You are a system assistant that translates requests into PowerShell commands.
        Respond ONLY with a valid JSON object format in pure text.
    
        NEVER include markdown code fences, backticks, explanations, or any other text.
    
        Response format:
        {
          "command": "command1; command2",
          "references": {
            "command1": "https://link1",
            "command2": "https://link2"
          }
        }
    
        If a command requires an identity and none are provided, use "user1", "user2" and so on.
        If a command requires a domain and none are provided, use "@domain.com".
        """
                    }
            }
            });
            //Submit HTTP Request
            Debug.WriteLine("Submitting");
            string c = await content.ReadAsStringAsync();
            Debug.WriteLine(c);
            Uri uri = new Uri(API_URL);
            HttpRequestMessage requestMesage = new HttpRequestMessage(HttpMethod.Post, uri);
            requestMesage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", API_KEY);
            requestMesage.Content = content;
            HttpResponseMessage response = await _httpClient.SendAsync(requestMesage);
            //Really long string manipulation...
            string responseText = JsonNode.Parse(
                    await response.Content.ReadAsStringAsync()
                )!
                ["content"]!
                [0]!
                ["text"]!
                .ToString()
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();
            /*
             Should look like this-
             {
                "command": "Write-Host 'Test'",
                "references": {
                    "Write-Host": "https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.utility/write-host"
                    
                }
             }
            */

            //Parse Json to get the command and the references in the correct format
            JsonObject responseObject = JsonNode.Parse(responseText)!.AsObject();
            string command = responseObject["command"]!.ToString();
            JsonObject referencesObject = responseObject["references"]!.AsObject();
            Dictionary<String,String> references = new Dictionary<string, string>();
            foreach (var reference in referencesObject)
            {
                if (reference.Value == null) continue;
                references.Add(reference.Key, reference.Value.ToString());
            }
            this.response = new Response(command, references);
            return this.response;
        }
    }
}
