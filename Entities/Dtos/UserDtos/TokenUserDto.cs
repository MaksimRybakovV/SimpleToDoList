namespace Entities.Dtos.UserDtos
{
    public class TokenUserDto
    {
        public int Id { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenExpires { get; set; }
    }
}
