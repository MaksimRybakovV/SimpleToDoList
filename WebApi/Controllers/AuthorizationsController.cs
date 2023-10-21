using Entities.Dtos.UserDtos;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.AuthotizationService;

namespace WebApi.Controllers
{
    [ApiController]
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
        public async Task<ActionResult<ServiceResponce<GetUserDto>>> Login(AuthUserDto request)
        {
            var responce = await _service.GetUserByAuthAsync(request);

            if (responce.Data is null)
                return BadRequest(responce);

            return Ok(responce);
        }

        [HttpPut]
        [Route("refreshtoken")]
        public async Task<ActionResult<ServiceResponce<GetUserDto>>> RefreshToken(TokenUserDto user)
        {
            var responce = await _service.RefreshTokenAsync(user);

            if (responce.Data is null)
                return Unauthorized(responce);

            return Ok(responce);
        }

        [HttpPut]
        [Route("logout")]
        public async Task<ActionResult<ServiceResponce<GetUserDto>>> Logout(int id)
        {
            var responce = await _service.ClearTokenAsync(id);

            if (responce.Data is null)
                return Unauthorized(responce);

            return Ok(responce);
        }
    }
}
