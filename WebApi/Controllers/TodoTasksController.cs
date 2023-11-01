using Asp.Versioning;
using Entities.Dtos.TodoTaskDtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.TodoTaskService;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion(1.0)]
    public class TodoTasksController : ControllerBase
    {
        private readonly ITodoTaskService _service;

        public TodoTasksController(ITodoTaskService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/Users/[controller]")]
        public async Task<ActionResult<ServiceResponse<List<GetTodoTaskDto>>>> GetAll()
        {
            return Ok(await _service.GetAllTodosAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/Users/[controller]/{id}")]
        public async Task<ActionResult<ServiceResponse<GetTodoTaskDto>>> GetSingle(int id)
        {
            var response = await _service.GetTodoByIdAsync(id);
            
            if(response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/Users/[controller]/pagination")]
        public async Task<ActionResult<PageServiceResponse<List<GetTodoTaskDto>>>> GetPage([FromQuery]int page, [FromQuery]int pageSize)
        {
            var response = await _service.GetTodosByPageAsync(page, pageSize);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet]
        [Route("api/Users/{id}/[controller]")]
        public async Task<ActionResult<ServiceResponse<List<GetTodoTaskDto>>>> GetAllByUsers(int id)
        {
            var response = await _service.GetAllUsersTodosAsync(id);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet]
        [Route("api/Users/{id}/[controller]/pagination")]
        public async Task<ActionResult<ServiceResponse<List<GetTodoTaskDto>>>> GetPageByUser(int id, [FromQuery]int page, [FromQuery] int pageSize)
        {
            var response = await _service.GetUsersTodosByPageAsync(id, page, pageSize);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        [Route("api/Users/[controller]")]
        public async Task<ActionResult<ServiceResponse<int>>> PostTask(AddTodoTaskDto newTodoTask, [FromQuery]int id)
        {
            var response = await _service.AddTodoAsync(newTodoTask, id);

            if (response.Data == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPut]
        [Route("api/Users/[controller]/{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateTask(UpdateTodoTaskDto updatedTodoTask)
        {
            var response = await _service.UpdateTodoAsync(updatedTodoTask);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete]
        [Route("api/Users/[controller]/{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteTask(int id)
        {
            var response = await _service.DeleteTodoAsync(id);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }
    }
}
