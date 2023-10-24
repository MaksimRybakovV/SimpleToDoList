using Entities.Dtos.UserDtos;

namespace WpfClient.Services.AuthorizationService
{
    internal interface IAuthorizationService
    {
        public GetUserDto CurrentUser { get; }

        public void SetCurrentUser(GetUserDto currentUser);
    }
}
