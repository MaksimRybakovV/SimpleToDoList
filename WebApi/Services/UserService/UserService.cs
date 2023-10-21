using AutoMapper;
using Entities.Dtos.UserDtos;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Services.UserService
{
    public class UserService : BaseService, IUserService
    {
        public UserService(DataContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<ServiceResponce<int>> AddUserAsync(AddUserDto newUser)
        {
            var responce = new ServiceResponce<int>();
            var oldUser = await _context.Users
                .SingleOrDefaultAsync(u => u.Username == newUser.Username);

            if (oldUser is null)
            {
                await _context.Users
                    .AddAsync(_mapper.Map<User>(newUser));

                await _context.SaveChangesAsync();

                var user = _context.Users
                    .Max(u => u.Id);

                responce.Data = user;
            }
            else
            {
                responce.IsSuccessful = false;
                responce.Message = "A user with the same name already exists.";
            }

            return responce;
        }

        public async Task<ServiceResponce<string>> DeleteUserAsync(int id)
        {
            var responce = new ServiceResponce<string>();

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Id == id)
                    ?? throw new Exception($"User with Id '{id}' not found!");

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                responce.Data = $"User with Id '{id}' deleted!";
            }
            catch (Exception ex)
            {
                responce.IsSuccessful = false;
                responce.Message = ex.Message;
            }

            return responce;
        }

        public async Task<ServiceResponce<List<GetUserDto>>> GetAllUsersAsync()
        {
            var responce = new ServiceResponce<List<GetUserDto>>();

            responce.Data = await _context.Users
                .Select(u => _mapper.Map<GetUserDto>(u))
                .ToListAsync();

            return responce;
        }

        public async Task<ServiceResponce<GetUserDto>> GetUserByIdAsync(int id)
        {
            var responce = new ServiceResponce<GetUserDto>();

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Id == id)
                    ?? throw new Exception($"User with Id '{id}' not found!");

                responce.Data = _mapper.Map<GetUserDto>(user);
            }
            catch (Exception ex)
            {
                responce.IsSuccessful = false;
                responce.Message = ex.Message;
            }

            return responce;
        }

        public async Task<PageServiceResponce<List<GetUserDto>>> GetUserByPageAsync(int page, int pageSize)
        {
            var responce = new PageServiceResponce<List<GetUserDto>>();

            try
            {
                var pageCount = Math.Ceiling(_context.Users.Count() / (float)pageSize);

                if (page > pageCount)
                    throw new Exception($"The page {page} does not exist. The maximum number of pages is {pageCount}.");

                var users = await _context.Users
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => _mapper.Map<GetUserDto>(u))
                    .ToListAsync();

                responce.Data = users;
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

        public async Task<ServiceResponce<string>> UpdateUserAsync(UpdateUserDto updatedUser)
        {
            var responce = new ServiceResponce<string>();

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Id == updatedUser.Id)
                    ?? throw new Exception($"User with Id '{updatedUser.Id}' not found!");

                var oldUser = await _context.Users
                    .SingleOrDefaultAsync(u => u.Username == updatedUser.Username);

                if (oldUser is null)
                {
                    user.Username = updatedUser.Username;
                    user.FirstName = updatedUser.FirstName;
                    user.LastName = updatedUser.LastName;
                }
                else
                {
                    throw new Exception("A user with the same name already exists.");
                }

                responce.Data = $"User with Id '{updatedUser.Id}' updated!";
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

