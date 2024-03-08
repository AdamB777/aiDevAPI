using Application.CQRS;
using Application.CQRS.helloapi;
using Application.DTO.HelloApiDTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace apiaidev.Controllers
{
    public class HelloApiController : BaseApiController
    {
        private readonly ExternalServiceSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthorizationController> _logger;

        public HelloApiController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("getanswer/{token}")]
        public async Task<IActionResult> GetTaskAsync(string token)
        {
            var result = await _mediator.Send(new QuestGet.Query { Token = token });
            return Ok(result);
        }
        [HttpPost("answer")]
        public async Task<IActionResult> AnswerAsync(AnswerHelloApiDTO answerHelloApiDTO)
        {
            var command=new AnswerPost.Command { answerPost = answerHelloApiDTO };
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

    }
}
