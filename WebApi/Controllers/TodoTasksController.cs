using Asp.Versioning;
using Entities.Dtos.TodoTaskDtos;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.TodoTaskService;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    public class TodoTasksController : ControllerBase
    {
        private readonly ITodoTaskService _service;

        public TodoTasksController(ITodoTaskService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/Users/[controller]")]
        public async Task<ActionResult<ServiceResponce<List<GetTodoTaskDto>>>> GetAll()
        {
            return Ok(await _service.GetAllTodosAsync());
        }

        [HttpGet]
        [Route("api/Users/[controller]/{id}")]
        public async Task<ActionResult<ServiceResponce<GetTodoTaskDto>>> GetSingle(int id)
        {
            var responce = await _service.GetTodoByIdAsync(id);
            
            if(responce.Data is null)
                return NotFound(responce);

            return Ok(responce);
        }

        [HttpGet]
        [Route("api/Users/[controller]/pagination")]
        public async Task<ActionResult<PageServiceResponce<List<GetTodoTaskDto>>>> GetPage([FromQuery]int page, [FromQuery]int pageSize)
        {
            var responce = await _service.GetTodosByPageAsync(page, pageSize);

            if (responce.Data is null)
                return NotFound(responce);

            return Ok(responce);
        }

        [HttpGet]
        [Route("api/Users/{id}/[controller]")]
        public async Task<ActionResult<ServiceResponce<List<GetTodoTaskDto>>>> GetAllByUsers(int id)
        {
            var responce = await _service.GetAllUsersTodosAsync(id);

            if (responce.Data is null)
                return NotFound(responce);

            return Ok(responce);
        }

        [HttpGet]
        [Route("api/Users/{id}/[controller]/pagination")]
        public async Task<ActionResult<ServiceResponce<List<GetTodoTaskDto>>>> GetPageByUser(int id, [FromQuery]int page, [FromQuery] int pageSize)
        {
            var responce = await _service.GetUsersTodosByPageAsync(id, page, pageSize);

            if (responce.Data is null)
                return NotFound(responce);

            return Ok(responce);
        }

        [HttpPost]
        [Route("api/Users/[controller]")]
        public async Task<ActionResult<ServiceResponce<int>>> PostTask(AddTodoTaskDto newTodoTask, [FromQuery]int id)
        {
            var responce = await _service.AddTodoAsync(newTodoTask, id);

            if (responce.Data == 0)
                return NotFound(responce);

            return Ok(responce);
        }

        [HttpPut]
        [Route("api/Users/[controller]/{id}")]
        public async Task<ActionResult<ServiceResponce<string>>> UpdateTask(UpdateTodoTaskDto updatedTodoTask)
        {
            var responce = await _service.UpdateTodoAsync(updatedTodoTask);

            if (responce.Data is null)
                return NotFound(responce);

            return Ok(responce);
        }

        [HttpDelete]
        [Route("api/Users/[controller]/{id}")]
        public async Task<ActionResult<ServiceResponce<string>>> DeleteTask(int id)
        {
            var responce = await _service.DeleteTodoAsync(id);

            if (responce.Data is null)
                return NotFound(responce);

            return Ok(responce);
        }
    }
}
