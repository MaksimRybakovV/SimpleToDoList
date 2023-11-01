using Asp.Versioning;
using Entities.Dtos.UserDtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize (Roles ="Admin")]
        public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> GetAll()
        {
            return await _service.GetAllUsersAsync();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetSingle(int id)
        {
            var response = await _service.GetUserByIdAsync(id);

            if(response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("pagination")]
        public async Task<ActionResult<PageServiceResponse<List<GetUserDto>>>> GetPage([FromQuery]int page, [FromQuery]int pageSize)
        {
            var response = await _service.GetUserByPageAsync(page, pageSize);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<int>>> PostUser(AddUserDto newUser)
        {
            var response = await _service.AddUserAsync(newUser);

            if (response.Data == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateUser(UpdateUserDto updatedUser)
        {
            var response = await _service.UpdateUserAsync(updatedUser);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteUser(int id)
        {
            var response = await _service.DeleteUserAsync(id);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }
    }
}
