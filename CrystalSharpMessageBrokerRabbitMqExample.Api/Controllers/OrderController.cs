using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CrystalSharp.Messaging.Distributed;
using CrystalSharp.Messaging.Distributed.Models;
using CrystalSharpMessageBrokerRabbitMqExample.Api.Dto;

namespace CrystalSharpMessageBrokerRabbitMqExample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMessageBroker _messageBroker;

        public OrderController(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        [HttpPost]
        [Route("send-order-code")]
        public async Task<ActionResult> PostSendOrderCode([FromBody] OrderInfo request)
        {
            GeneralQueueMessage message = new()
            {
                Queue = new GeneralQueue { Name = "customer-order-queue" }
            };
            message.Body = request.OrderCode;

            await _messageBroker.SendString(message, CancellationToken.None).ConfigureAwait(false);

            return Ok("Message sent.");
        }
    }
}
