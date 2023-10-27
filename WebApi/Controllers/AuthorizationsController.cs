using Asp.Versioning;
using Entities.Dtos.UserDtos;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.AuthotizationService;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/[controller]")]
    public class AuthorizationsController : ControllerBase
    {
        private readonly IAuthorizationService _service;

        public AuthorizationsController(IAuthorizationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("login")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> Login([FromQuery]string username, [FromQuery]string password)
        {
            var response = await _service.GetUserByAuthAsync(username, password);

            if (response.Data is null)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut]
        [Route("refreshtoken")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> RefreshToken(TokenUserDto user)
        {
            var response = await _service.RefreshTokenAsync(user);

            if (response.Data is null)
                return Unauthorized(response);

            return Ok(response);
        }

        [HttpPut]
        [Route("logout/{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> Logout(int id)
        {
            var response = await _service.ClearTokenAsync(id);

            if (response.Data is null)
                return Unauthorized(response);

            return Ok(response);
        }
    }
}
