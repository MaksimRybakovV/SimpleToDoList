using Entities.Enums;

namespace Entities.Dtos.TodoTaskDtos
{
    public class GetTodoTaskDto : ICloneable
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime Creation { get; set; }
        public DateTime Deadline { get; set; } = DateTime.Now;
        public TaskCategory Category { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskExecutionStatus Status { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
