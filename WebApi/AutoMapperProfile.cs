using AutoMapper;
using Entities.Dtos.TodoTaskDtos;
using Entities.Dtos.UserDtos;
using Entities.Models;

namespace WebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<TodoTask, GetTodoTaskDto>();
            CreateMap<AddTodoTaskDto, TodoTask>();

            CreateMap<User, GetUserDto>();
            CreateMap<AddUserDto, User>();
        }
    }
}
