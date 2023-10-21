using AutoMapper;
using Entities.Dtos.TodoTaskDtos;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Services.TodoTaskService
{
    public class TodoTaskService : BaseService, ITodoTaskService
    {
        public TodoTaskService(DataContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<ServiceResponce<int>> AddTodoAsync(AddTodoTaskDto newTodoTask, int userId)
        {
            var responce = new ServiceResponce<int>();

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Id == userId)
                    ?? throw new Exception($"User with Id '{userId}' not found!");

                user.Tasks.Add(_mapper.Map<TodoTask>(newTodoTask));

                await _context.SaveChangesAsync();
                
                var task = await _context.TodoTasks
                    .MaxAsync(t => t.Id);

                responce.Data = task;
            }
            catch (Exception ex)
            {
                responce.IsSuccessful = false;
                responce.Message = ex.Message;
            }

            return responce;
        }

        public async Task<ServiceResponce<string>> DeleteTodoAsync(int id)
        {
            var responce = new ServiceResponce<string>();

            try
            {
                var todo = await _context.TodoTasks
                    .SingleOrDefaultAsync(t => t.Id == id)
                    ?? throw new Exception($"Task with Id '{id}' not found!");

                _context.TodoTasks.Remove(todo);
                await _context.SaveChangesAsync();
                responce.Data = $"Task with Id '{id}' deleted!";
            }
            catch (Exception ex)
            {
                responce.IsSuccessful = false;
                responce.Message = ex.Message;
            }

            return responce;
        }

        public async Task<ServiceResponce<List<GetTodoTaskDto>>> GetAllTodosAsync()
        {
            var responce = new ServiceResponce<List<GetTodoTaskDto>>();

            responce.Data = await _context.TodoTasks
                .Select(t => _mapper.Map<GetTodoTaskDto>(t))
                .ToListAsync();

            return responce;
        }

        public async Task<ServiceResponce<List<GetTodoTaskDto>>> GetAllUsersTodosAsync(int userId)
        {
            var responce = new ServiceResponce<List<GetTodoTaskDto>>();

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Id == userId)
                    ?? throw new Exception($"User with Id '{userId}' not found!");

                responce.Data = await _context.TodoTasks
                    .Where(t => t.UserId == userId) 
                    .Select(t => _mapper.Map<GetTodoTaskDto>(t))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                responce.IsSuccessful = false;
                responce.Message = ex.Message;
            }

            return responce;
        }

        public async Task<ServiceResponce<GetTodoTaskDto>> GetTodoByIdAsync(int id)
        {
            var responce = new ServiceResponce<GetTodoTaskDto>();

            try
            {
                var todo = await _context.TodoTasks
                    .SingleOrDefaultAsync(u => u.Id == id)
                    ?? throw new Exception($"Todo with Id '{id}' not found!");

                responce.Data = _mapper.Map<GetTodoTaskDto>(todo);
            }
            catch (Exception ex)
            {
                responce.IsSuccessful = false;
                responce.Message = ex.Message;
            }

            return responce;
        }

        public async Task<PageServiceResponce<List<GetTodoTaskDto>>> GetTodosByPageAsync(int page, int pageSize)
        {
            var responce = new PageServiceResponce<List<GetTodoTaskDto>>();

            try
            {
                var pageCount = Math.Ceiling(_context.TodoTasks.Count() / (float)pageSize);

                if (page > pageCount)
                    throw new Exception($"The page {page} does not exist. The maximum number of pages is {pageCount}.");

                var todos = await _context.TodoTasks
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(t => _mapper.Map<GetTodoTaskDto>(t))
                    .ToListAsync();

                responce.Data = todos;
                responce.CurrentPage = page;
                responce.PageCount = (int)pageCount;
            }
            catch (Exception ex)
            {
                responce.IsSuccessful = false;
                responce.Message = ex.Message;
            }

            return responce;
        }

        public async Task<PageServiceResponce<List<GetTodoTaskDto>>> GetUsersTodosByPageAsync(int userId, int page, int pageSize)
        {
            var responce = new PageServiceResponce<List<GetTodoTaskDto>>();

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Id == userId)
                    ?? throw new Exception($"User with Id '{userId}' not found!");

                var pageCount = Math.Ceiling(_context.TodoTasks
                    .Where(t => t.UserId == userId)
                    .Count() 
                    / (float)pageSize);

                if (page > pageCount)
                    throw new Exception($"The page {page} does not exist. The maximum number of pages is {pageCount}.");

                var todos = await _context.TodoTasks
                    .Where(t => t.UserId == userId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(t => _mapper.Map<GetTodoTaskDto>(t))
                    .ToListAsync();

                responce.Data = todos;
                responce.CurrentPage = page;
                responce.PageCount = (int)pageCount;
            }
            catch (Exception ex)
            {
                responce.IsSuccessful = false;
                responce.Message = ex.Message;
            }

            return responce;
        }

        public async Task<ServiceResponce<string>> UpdateTodoAsync(UpdateTodoTaskDto updatedTodoTask)
        {
            var responce = new ServiceResponce<string>();

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
                responce.Data = $"Todo with Id '{updatedTodoTask.Id}' updated!";
            }
            catch (Exception ex)
            {
                responce.IsSuccessful = false;
                responce.Message = ex.Message;
            }

            return responce;
        }
    }
}
