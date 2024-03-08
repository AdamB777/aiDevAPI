using Application.Config;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.CQRS
{
    public class QuestGet
    {
        public class Query : IRequest<string>
        {
            public string Token { get; set; }
        }
        public class Handler : IRequestHandler<Query, string>
        {
            private readonly HttpClient _httpClient;
            private readonly ILogger<Handler> _logger;
            private readonly Settings _settings;

            public Handler(HttpClient httpClient, ILogger<Handler> logger, IOptions<Settings> settings)
            {
                _httpClient = httpClient;
                _logger = logger;
                _settings = settings.Value;
            }

            public async Task<string> Handle(Query request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Wysyłanie żądania GET do {_settings.ApiUrl}/task/{request.Token}");

                var rsponse = await _httpClient.GetAsync($"{_settings.ApiUrl}/task/{request.Token}");

                if (rsponse.IsSuccessStatusCode)
                {
                    var responseContent = await rsponse.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Odpowiedź od serwera: {responseContent}");
                    return responseContent;
                }
                else
                {
                    var errorContent = await rsponse?.Content.ReadAsStringAsync();
                    _logger.LogError($"Błąd odpowiedzi: {errorContent}");
                    return $"Error: {errorContent}";
                }

            }
        }
    }
}
