using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Model.MessageModel;

namespace OrderPublisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IBus _bus;
        public OrderController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            if (ticket is null)
                return BadRequest();

            ticket.Booked = DateTime.Now;
            Uri uri = new("rabbitmq://localhost/orderTicketQueue");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(ticket);
            return Ok();
        }
    }
}
