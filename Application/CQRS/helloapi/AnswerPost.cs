using Application.Config;
using Application.DTO.HelloApiDTO;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Application.CQRS.helloapi
{
    public class AnswerPost
    {
        public class Command : IRequest<string>
        {
            public AnswerHelloApiDTO answerPost { get; set; }
        }
        public class Handler : IRequestHandler<Command, string>
        {
            private readonly IHttpClientFactory _httpClientFactory;
            private readonly ILogger<Handler> _logger;
            private readonly Settings _settings;

            public Handler(IHttpClientFactory httpClientFactory, ILogger<Handler> logger, IOptions<Settings> settings)
            {
                _httpClientFactory = httpClientFactory;
                _logger = logger;
                _settings = settings.Value;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var httpClient=_httpClientFactory.CreateClient();
                var requestContent=JsonConvert.SerializeObject(new {answer=request.answerPost.Answer});
                var content=new StringContent(requestContent, Encoding.UTF8, "application/json");

                var fullUrl = $"{_settings.ApiUrl}/answer/{request.answerPost.Token}";
                _logger.LogInformation($"Wysyłanie żądania do {fullUrl} z treścią: {requestContent}");

                var response=await httpClient.PostAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent=await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Odpowiedź od serwera: {responseContent}");
                    return responseContent;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Błąd odpowiedzi: {errorContent}");
                    throw new HttpRequestException($"Błąd odpowiedzi: {errorContent}");
                }
            }
        }
    }
}
