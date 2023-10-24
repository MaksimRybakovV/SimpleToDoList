using Entities.Dtos.UserDtos;
using WpfClient.Infrastructure.ObservableObject;

namespace WpfClient.Services.AuthorizationService
{
    internal class AuthorizationService : ObservableObject, IAuthorizationService
    {
        private GetUserDto _currentUser = new();

        public GetUserDto CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public void SetCurrentUser(GetUserDto currentUser)
        {
            CurrentUser = currentUser;
        }
    }
}
