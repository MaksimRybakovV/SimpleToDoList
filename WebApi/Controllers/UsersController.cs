using Asp.Versioning;
using Entities.Dtos.UserDtos;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.UserService;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponce<List<GetUserDto>>>> GetAll()
        {
            return await _service.GetAllUsersAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ServiceResponce<GetUserDto>>> GetSingle(int id)
        {
            var responce = await _service.GetUserByIdAsync(id);

            if(responce.Data is null)
                return NotFound(responce);

            return Ok(responce);
        }

        [HttpGet]
        [Route("pagination")]
        public async Task<ActionResult<PageServiceResponce<List<GetUserDto>>>> GetPage([FromQuery]int page, [FromQuery]int pageSize)
        {
            var responce = await _service.GetUserByPageAsync(page, pageSize);

            if (responce.Data is null)
                return NotFound(responce);

            return Ok(responce);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponce<int>>> PostUser(AddUserDto newUser)
        {
            var responce = await _service.AddUserAsync(newUser);

            if (responce.Data == 0)
                return NotFound(responce);

            return Ok(responce);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ServiceResponce<string>>> UpdateUser(UpdateUserDto updatedUser)
        {
            var responce = await _service.UpdateUserAsync(updatedUser);

            if (responce.Data is null)
                return NotFound(responce);

            return Ok(responce);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ServiceResponce<string>>> DeleteUser(int id)
        {
            var responce = await _service.DeleteUserAsync(id);

            if (responce.Data is null)
                return NotFound(responce);

            return Ok(responce);
        }
    }
}
