using Entities.Dtos.UserDtos;
using Entities.Models;

namespace WebApi.Services.AuthotizationService
{
    public interface IAuthorizationService
    {
        public Task<ServiceResponce<GetUserDto>> GetUserByAuthAsync(AuthUserDto authUser);
        public Task<ServiceResponce<GetUserDto>> ClearTokenAsync(int id);
        public Task<ServiceResponce<GetUserDto>> RefreshTokenAsync(TokenUserDto user);
    }
}
