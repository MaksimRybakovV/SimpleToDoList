using AutoMapper;
using Entities.Dtos.TodoTaskDtos;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Services.TodoTaskService
{
    public class TodoTaskService : BaseService<TodoTask>, ITodoTaskService
    {
        public TodoTaskService(DataContext context, IMapper mapper, ILogger<TodoTask> logger) : base(context, mapper, logger) { }

        public async Task<ServiceResponse<int>> AddTodoAsync(AddTodoTaskDto newTodoTask, int userId)
        {
            var response = new ServiceResponse<int>();

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Id == userId)
                    ?? throw new Exception($"User with Id '{userId}' not found!");

                user.Tasks.Add(_mapper.Map<TodoTask>(newTodoTask));

                await _context.SaveChangesAsync();
                
                var task = await _context.TodoTasks
                    .MaxAsync(t => t.Id);

                response.Data = task;
                _logger.LogInformation("The task was created with the values {@newTodoTask}. The task belongs to a user with ID {id}.", newTodoTask, userId);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
                _logger.LogError("The user with ID '{userId}' not found.", userId);
            }

            return response;
        }

        public async Task<ServiceResponse<string>> DeleteTodoAsync(int id)
        {
            var response = new ServiceResponse<string>();

            try
            {
                var todo = await _context.TodoTasks
                    .SingleOrDefaultAsync(t => t.Id == id)
                    ?? throw new Exception($"Task with Id '{id}' not found!");

                _context.TodoTasks.Remove(todo);
                await _context.SaveChangesAsync();
                response.Data = $"Task with Id '{id}' deleted!";
                _logger.LogInformation("The task with ID '{id}' has been deleted.", id);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
                _logger.LogError("The task with ID '{id}' not found.", id);
            }

            return response;
        }

        public async Task<ServiceResponse<List<GetTodoTaskDto>>> GetAllTodosAsync()
        {
            var response = new ServiceResponse<List<GetTodoTaskDto>>();

            response.Data = await _context.TodoTasks
                .Select(t => _mapper.Map<GetTodoTaskDto>(t))
                .ToListAsync();

            return response;
        }

        public async Task<ServiceResponse<List<GetTodoTaskDto>>> GetAllUsersTodosAsync(int userId)
        {
            var response = new ServiceResponse<List<GetTodoTaskDto>>();

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Id == userId)
                    ?? throw new Exception($"User with Id '{userId}' not found!");

                response.Data = await _context.TodoTasks
                    .Where(t => t.UserId == userId) 
                    .Select(t => _mapper.Map<GetTodoTaskDto>(t))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetTodoTaskDto>> GetTodoByIdAsync(int id)
        {
            var response = new ServiceResponse<GetTodoTaskDto>();

            try
            {
                var todo = await _context.TodoTasks
                    .SingleOrDefaultAsync(u => u.Id == id)
                    ?? throw new Exception($"Todo with Id '{id}' not found!");

                response.Data = _mapper.Map<GetTodoTaskDto>(todo);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<PageServiceResponse<List<GetTodoTaskDto>>> GetTodosByPageAsync(int page, int pageSize)
        {
            var response = new PageServiceResponse<List<GetTodoTaskDto>>();

            try
            {
                var pageCount = Math.Ceiling(_context.TodoTasks.Count() / (float)pageSize);
                pageCount = Math.Max(pageCount, 1);

                if (page > pageCount)
                    throw new Exception($"The page {page} does not exist. The maximum number of pages is {pageCount}.");

                var todos = await _context.TodoTasks
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(t => _mapper.Map<GetTodoTaskDto>(t))
                    .ToListAsync();

                response.Data = todos;
                response.CurrentPage = page;
                response.PageCount = (int)pageCount;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<PageServiceResponse<List<GetTodoTaskDto>>> GetUsersTodosByPageAsync(int userId, int page, int pageSize)
        {
            var response = new PageServiceResponse<List<GetTodoTaskDto>>();

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Id == userId)
                    ?? throw new Exception($"User with Id '{userId}' not found!");

                var pageCount = Math.Ceiling(_context.TodoTasks
                    .Where(t => t.UserId == userId)
                    .Count() 
                    / (float)pageSize);

                pageCount = Math.Max(pageCount, 1);

                if (page > pageCount)
                    throw new Exception($"The page {page} does not exist. The maximum number of pages is {pageCount}.");

                var todos = await _context.TodoTasks
                    .Where(t => t.UserId == userId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(t => _mapper.Map<GetTodoTaskDto>(t))
                    .ToListAsync();

                response.Data = todos;
                response.CurrentPage = page;
                response.PageCount = (int)pageCount;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<string>> UpdateTodoAsync(UpdateTodoTaskDto updatedTodoTask)
        {
            var response = new ServiceResponse<string>();

            try
            {
                var todo = await _context.TodoTasks
                    .SingleOrDefaultAsync(t => t.Id == updatedTodoTask.Id)
                    ?? throw new Exception($"Todo with Id '{updatedTodoTask.Id}' not found!");

                todo.Title = updatedTodoTask.Title;
                todo.Description = updatedTodoTask.Description;
                todo.Notes = updatedTodoTask.Notes;
                todo.Deadline = updatedTodoTask.Deadline;
                todo.Category = updatedTodoTask.Category;
                todo.Priority = updatedTodoTask.Priority;
                todo.Status = updatedTodoTask.Status;

                await _context.SaveChangesAsync();
                response.Data = $"Todo with Id '{updatedTodoTask.Id}' updated!";
                _logger.LogInformation("The task with ID '{updatedTodoTask.Id}' has been updated " +
                    "with values {@updatedTodoTask}.", updatedTodoTask.Id, updatedTodoTask);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
                _logger.LogError("The task with ID '{updatedTodoTask.Id}' not found.", updatedTodoTask.Id);
            }

            return response;
        }
    }
}
