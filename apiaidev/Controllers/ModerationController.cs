using Application.CQRS;
using Application.CQRS.moderation;
using Application.DTO.ModerationDTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace apiaidev.Controllers
{
    public class ModerationController : BaseApiController
    {
        private readonly ILogger<ModerationController> _logger;

        public ModerationController(IMediator mediator, ILogger<ModerationController> logger) : base(mediator)
        {
            _logger = logger;
        }

        [HttpGet("getanswer/{token}")]
        public async Task<IActionResult> GetTaskAsync(string token)
        {
            var result = await _mediator.Send(new QuestGet.Query { Token = token });
            return Ok(result);
        }

        [HttpPost("moderate")]
        public async Task<IActionResult> ModerateAsync(string moderationInput)
        {
            var command = new SendToModerationPost.Command { Sentences = moderationInput };
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("answermoderation")]
        public async Task<IActionResult> ModerateAsync(AnswerModerationDTO answerModerationDTO)
        {
            var command = new AnswerPost.Command { answerPost = answerModerationDTO };
            try
            {
                var result = await _mediator.Send(command);
                _logger.LogInformation($"Wysyłanie odpowiedzi: {JsonConvert.SerializeObject(result)}");
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

    }
