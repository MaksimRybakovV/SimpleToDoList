using Entities.Dtos.TodoTaskDtos;
using Entities.Models;

namespace WebApi.Services.TodoTaskService
{
    public interface ITodoTaskService
    {
        public Task<ServiceResponse<List<GetTodoTaskDto>>> GetAllTodosAsync();
        public Task<ServiceResponse<GetTodoTaskDto>> GetTodoByIdAsync(int id);
        public Task<PageServiceResponse<List<GetTodoTaskDto>>> GetTodosByPageAsync(int page, int pageSize);
        public Task<ServiceResponse<List<GetTodoTaskDto>>> GetAllUsersTodosAsync(int userId);
        public Task<PageServiceResponse<List<GetTodoTaskDto>>> GetUsersTodosByPageAsync(int userId, int page, int pageSize);
        public Task<ServiceResponse<int>> AddTodoAsync(AddTodoTaskDto newTodoTask, int userId);
        public Task<ServiceResponse<string>> UpdateTodoAsync(UpdateTodoTaskDto updatedTodoTask);
        public Task<ServiceResponse<string>> DeleteTodoAsync(int id);
    }
}
