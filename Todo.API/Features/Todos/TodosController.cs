using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo.API.Features.Todos.Commands;

namespace Todo.API.Features.Todos
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TodosController> _logger;

        public TodosController(IMediator mediator, ILogger<TodosController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> AddTodo([FromBody] AddTodoCommand.Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }
        
    }
}
