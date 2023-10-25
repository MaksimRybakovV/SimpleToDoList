using Entities.Dtos.UserDtos;
using Entities.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfClient.Infrastructure.Commands;
using WpfClient.Services.AuthorizationService;
using WpfClient.Services.NavigationService;
using WpfClient.Services.WebService;
using WpfClient.ViewModel.Base;

namespace WpfClient.ViewModel
{
    internal class AuthorizationViewModel : BaseViewModel
    {
        private readonly IWebService _service;
        private INavigationService<UserControl>? _navigation;
        private IAuthorizationService? _authorization;

        public INavigationService<UserControl>? Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged(nameof(Navigation));
            }
        }

        public IAuthorizationService? Authorization
        {
            get => _authorization;
            set
            {
                _authorization = value;
                OnPropertyChanged(nameof(Authorization));
            }
        }

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public ICommand AutorizationCommand { get; }
        public ICommand GetPasswordCommand { get; }

        public AuthorizationViewModel(IWebService service, INavigationService<UserControl> navigation, IAuthorizationService authorization)
        {
            _service = service;
            Navigation = navigation;
            Authorization = authorization;

            AutorizationCommand = new RelayCommand(OnAutorizationCommand, CanAutorizationCommand);
            GetPasswordCommand = new RelayCommand(OnGetPasswordCommand, CanGetPasswordCommand);
        }

        private bool CanAutorizationCommand(object parameter) => (Username != "" && Password != "");

        private async void OnAutorizationCommand(object parameter)
        {
            var response = await _service.Authorization(Username, Password);

            if (response.IsSuccessful == false)
            {
                MessageBox.Show(response?.Message);
                return;
            }  

            Authorization?.SetCurrentUser(response?.Data!);
        }

        private bool CanGetPasswordCommand(object parameter) => true;

        private void OnGetPasswordCommand(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            Password = passwordBox!.Password;
        }
    }
}
