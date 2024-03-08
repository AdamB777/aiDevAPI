using apiaidev.DTO;
using Application.CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace apiaidev.Controllers
{
    public class AuthorizationController : BaseApiController
    {

        public AuthorizationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("gettoken")]
        public async Task<IActionResult> GetToken(AuthRequestDTO authRequestDTO)
        {
            var command = new TokenPost.Command { Task = authRequestDTO.Task };
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
