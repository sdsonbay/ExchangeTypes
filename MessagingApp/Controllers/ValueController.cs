using MassTransit;
using MessagingApp.Messages;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ValueController : ControllerBase
{
    private readonly IBus _bus;

    public ValueController(IBus bus)
    {
        _bus = bus;
    }
    
    [HttpPost("direct")]
    public async Task<IActionResult> DirectPost([FromBody] string message)
    {
        await _bus.Publish<IDirectMessage>(new
        {
            Message = message
        }, x => x.SetRoutingKey("A"));
        return Ok();
    }
    
    [HttpPost("topic")]
    public async Task<IActionResult> TopicPost([FromBody] string message)
    {
        await _bus.Publish<ITopicMessage>(new
        {
            Message = message
        }, x => x.SetRoutingKey("order.test"));
        return Ok();
    }
    
    [HttpPost("fanout")]
    public async Task<IActionResult> FanoutPost([FromBody] string message)
    {
        await _bus.Publish<IFanoutMessage>(new
        {
            Message = message
        });
        return Ok();
    }
}