using AutoMapper;
using WebApi.Data;

namespace WebApi.Services
{
    public class BaseService<T>
    {
        protected readonly DataContext _context;
        protected readonly IMapper _mapper;
        protected readonly ILogger<T> _logger;

        public BaseService(DataContext context, IMapper mapper, ILogger<T> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
    }
}
