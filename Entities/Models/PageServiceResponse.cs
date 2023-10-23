namespace Entities.Models
{
    public class PageServiceResponce<T>
    {
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; }
        public T? Data { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
