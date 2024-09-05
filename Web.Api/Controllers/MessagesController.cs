using Business.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Models;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController (IRabbitMQConnection connection ): ControllerBase
    {
        [HttpPost("{message}")]
        public async Task<IActionResult>Post([FromForm]User model)
        {
            connection.Connect(model);

            return Ok();
        }
    }
}
