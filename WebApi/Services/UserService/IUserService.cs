using Entities.Dtos.UserDtos;
using Entities.Models;

namespace WebApi.Services.UserService
{
    public interface IUserService
    {
        public Task<ServiceResponce<List<GetUserDto>>> GetAllUsersAsync();
        public Task<ServiceResponce<GetUserDto>> GetUserByIdAsync(int id);
        public Task<PageServiceResponce<List<GetUserDto>>> GetUserByPageAsync(int page, int pageSize);
        public Task<ServiceResponce<int>> AddUserAsync(AddUserDto newUser);
        public Task<ServiceResponce<string>> UpdateUserAsync (UpdateUserDto updatedUser);
        public Task<ServiceResponce<string>> DeleteUserAsync (int id);
    }
}
