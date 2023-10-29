using AutoMapper;
using Entities.Dtos.TodoTaskDtos;
using Entities.Dtos.UserDtos;

namespace WpfClient
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GetTodoTaskDto, UpdateTodoTaskDto>();
            CreateMap<GetTodoTaskDto, AddTodoTaskDto>();
            CreateMap<GetUserDto, TokenUserDto>();
        }
    }
}
