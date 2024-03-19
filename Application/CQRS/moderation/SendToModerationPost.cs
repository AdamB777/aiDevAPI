using Application.Config;
using Application.DTO.HelloApiDTO;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Application.CQRS.moderation
{
    public class SendToModerationPost
    {
        public class Command : IRequest<string>
        {
            public string Sentences { get; set; }
        }


        public class Handler : IRequestHandler<Command, string>
        {
            private readonly IHttpClientFactory _httpClientFactory;
            private readonly ILogger<Handler> _logger;
            private readonly Settings _settings;
            private readonly OpenAIService _openAIService;

            public Handler(IHttpClientFactory httpClientFactory, ILogger<Handler> logger, IOptions<Settings> settings)
            {
                _httpClientFactory = httpClientFactory;
                _logger = logger;
                _settings = settings.Value;
                _openAIService = new OpenAIService(_httpClientFactory.CreateClient(), _settings.OpenAI_ApiKey);
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var prompt = "Dla podanych 4 zdań przeprowadź walidację i w wyniku cyfry 1 lub 0 dla kazdego zdania, gdzie 0 - zdania, które przeszły moderację. 1 - zdania, które nie przeszły moderacji. Zdania są oddzielone przecinkami i znajdują się w cudzysłowiach. Oto zdania do moderacji:  " + request.Sentences;

                _logger.LogInformation($"Wysyłanie promptu do OpenAI: {prompt}");

                try
                {
                    var response = await _openAIService.GetResponseFromOpenAI(prompt);
                    _logger.LogInformation($"Odpowiedź od OpenAI: {response}");
                    return response;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Błąd podczas wysyłania zapytania do OpenAI: {ex.Message}");
                    throw;
                }
            }
        }

    }
}
