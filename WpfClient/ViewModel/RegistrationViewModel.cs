using Entities.Dtos.UserDtos;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfClient.Infrastructure.Commands;
using WpfClient.Services.NavigationService;
using WpfClient.Services.WebService;
using WpfClient.View.Windows;
using WpfClient.ViewModel.Base;

namespace WpfClient.ViewModel
{
    internal class RegistrationViewModel : BaseViewModel
    {
        private readonly IWebService _service;
        private INavigationService<UserControl>? _navigation;

        public INavigationService<UserControl>? Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged(nameof(Navigation));
            }
        }
        public AddUserDto NewUser { get; set; } = new AddUserDto();

        public ICommand RegistrationCommand { get; }
        public ICommand GetPasswordCommand { get; }
        public ICommand SwitchToAuthorizationCommand { get; }

        public RegistrationViewModel(IWebService service, INavigationService<UserControl> navigation)
        {
            _service = service;
            Navigation = navigation;

            RegistrationCommand = new RelayCommand(OnRegistrationCommand, CanRegistrationCommand);
            GetPasswordCommand = new RelayCommand(OnGetPasswordCommand, CanGetPasswordCommand);
            SwitchToAuthorizationCommand = new RelayCommand(OnSwitchToAuthorizationCommand, CanSwitchToAuthorizationCommand);
        }

        private bool CanRegistrationCommand(object parameter) => (NewUser.Username != ""
            && NewUser.PasswordHash != ""
            && NewUser.FirstName != ""
            && NewUser.LastName != "");

        private async void OnRegistrationCommand(object parameter)
        {
            var response = await _service.Registration(NewUser);

            if (response.IsSuccessful == false)
            {
                MessageBox.Show(response?.Message);
                return;
            }

            var passwordBox = parameter as PasswordBox;
            ClearTextBoxes(passwordBox!);
            MessageBox.Show("Registration was successful.");
            Navigation?.NavigateTo<AuthorizationView>();
        }

        private bool CanGetPasswordCommand(object parameter) => true;

        private void OnGetPasswordCommand(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            NewUser.PasswordHash = passwordBox!.Password;
        }

        private bool CanSwitchToAuthorizationCommand(object parameter) => true;

        private void OnSwitchToAuthorizationCommand(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            ClearTextBoxes(passwordBox!);
            Navigation?.NavigateTo<AuthorizationView>();
        }

        private void ClearTextBoxes(PasswordBox passwordBox)
        {
            passwordBox?.Clear();
            NewUser = new();
            OnPropertyChanged(nameof(NewUser));
        }
    }
}
