using System.Windows;
using System.Windows.Controls;
using WpfClient.Services.NavigationService;
using WpfClient.View.Windows;
using WpfClient.ViewModel.Base;

namespace WpfClient.ViewModel
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private INavigationService<UserControl>? _navigation;
        public WindowState State { get; set; }

        public INavigationService<UserControl>? Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged(nameof(Navigation));
            }
        }

        public MainWindowViewModel(INavigationService<UserControl> navigation)
        {
            Navigation = navigation;
            navigation.NavigateTo<AuthorizationView>();
        }
    }
}
