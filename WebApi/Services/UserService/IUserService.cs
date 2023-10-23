using Entities.Dtos.UserDtos;
using Entities.Models;

namespace WebApi.Services.UserService
{
    public interface IUserService
    {
        public Task<ServiceResponse<List<GetUserDto>>> GetAllUsersAsync();
        public Task<ServiceResponse<GetUserDto>> GetUserByIdAsync(int id);
        public Task<PageServiceResponse<List<GetUserDto>>> GetUserByPageAsync(int page, int pageSize);
        public Task<ServiceResponse<int>> AddUserAsync(AddUserDto newUser);
        public Task<ServiceResponse<string>> UpdateUserAsync (UpdateUserDto updatedUser);
        public Task<ServiceResponse<string>> DeleteUserAsync (int id);
    }
}
