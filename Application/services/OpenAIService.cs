using Newtonsoft.Json;
using System.Text;

public class OpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenAIService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<string> GetResponseFromOpenAI(string prompt)
    {
        var data = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
            new { role = "system", content = "W content zwracaj tylko tablice jedynek i zer, 0-dla prawdy, 1 dla fałszu. Przykładowa odpowiedź:[0,1,1,0]" },
            new { role = "user", content = prompt }
        }
        };
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
        else
        {
            return null;
        }
    }
}
