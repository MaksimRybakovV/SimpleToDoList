using AutoMapper;
using Entities.Dtos.TodoTaskDtos;

namespace WpfClient
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GetTodoTaskDto, UpdateTodoTaskDto>();
            CreateMap<GetTodoTaskDto, AddTodoTaskDto>();
        }
    }
}
