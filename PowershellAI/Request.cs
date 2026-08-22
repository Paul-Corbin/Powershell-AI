using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace PowershellAI
{
    internal class Request
    {
        private readonly HttpClient _httpClient= new HttpClient();
        private const string ApiUrl = "https://ai.hatz.ai/v1/anthropic/messages";
        private const string ApiModel = "anthropic.claude-haiku-4-5";
        private const string ApiKey = "6ae32ae1-a8a9-44bb-bfd0-80a5bed25a3c"; // REMOVE, STORE IN CREDENTIAL MANAGER
        private const string ApiMode = "lite"; //From fastest/least tokens -> slowest/most tokens- lite, performance, turbo
        private const string MaxTokens = "300000"; //Absurdly high but good for our purposes

        public async Task<Response> Submit(string submittedText)
        {
            //Build JSON object for the request
            JsonContent content = JsonContent.Create(new JsonObject{ 
            ["model"] = ApiModel,
            ["mode"] = ApiMode,
            ["max_tokens"] = MaxTokens,
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
        Include references for ALL commands you provide.
        If you do not know the answer, respond with 'ERROR: Request not understood' as the "command" and provide no references.

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
            var c = await content.ReadAsStringAsync();
            Debug.WriteLine(c);
            var uri = new Uri(ApiUrl);
            var requestMesage = new HttpRequestMessage(HttpMethod.Post, uri);
            requestMesage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
            requestMesage.Content = content;
            var httpResponse = await _httpClient.SendAsync(requestMesage);
            //Really long string manipulation...
            var responseText = JsonNode.Parse(
                    await httpResponse.Content.ReadAsStringAsync()
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
            try
            {
                Debug.WriteLine(responseText);
                JsonObject responseObject = JsonNode.Parse(responseText)!.AsObject();
                string command = responseObject["command"]!.ToString();
                Debug.WriteLine(command);
                JsonObject referencesObject = responseObject["references"]!.AsObject();
                Dictionary<String, String> references = new Dictionary<string, string>();
                foreach (var reference in referencesObject)
                {
                    if (reference.Value == null) continue;
                    Debug.WriteLine(reference.Key, reference.Value);
                    references.Add(reference.Key, reference.Value.ToString());
                }

                Response response = new Response(command, references);
                return response;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
