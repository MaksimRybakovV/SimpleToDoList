namespace Entities.Models
{
    public class ServiceResponce<T>
    {
        public T? Data { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
