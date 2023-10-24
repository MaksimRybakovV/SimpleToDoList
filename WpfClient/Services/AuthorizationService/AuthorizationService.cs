using Entities.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
