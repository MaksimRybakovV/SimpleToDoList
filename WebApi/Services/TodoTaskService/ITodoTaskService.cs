using Entities.Dtos.TodoTaskDtos;
using Entities.Models;

namespace WebApi.Services.TodoTaskService
{
    public interface ITodoTaskService
    {
        public Task<ServiceResponce<List<GetTodoTaskDto>>> GetAllTodosAsync();
        public Task<ServiceResponce<GetTodoTaskDto>> GetTodoByIdAsync(int id);
        public Task<PageServiceResponse<List<GetTodoTaskDto>>> GetTodosByPageAsync(int page, int pageSize);
        public Task<ServiceResponce<List<GetTodoTaskDto>>> GetAllUsersTodosAsync(int userId);
        public Task<PageServiceResponse<List<GetTodoTaskDto>>> GetUsersTodosByPageAsync(int userId, int page, int pageSize);
        public Task<ServiceResponce<int>> AddTodoAsync(AddTodoTaskDto newTodoTask, int userId);
        public Task<ServiceResponce<string>> UpdateTodoAsync(UpdateTodoTaskDto updatedTodoTask);
        public Task<ServiceResponce<string>> DeleteTodoAsync(int id);
    }
}
