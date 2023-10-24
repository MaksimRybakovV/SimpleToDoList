using System;
using System.Windows;
using System.Windows.Controls;
using WpfClient.Infrastructure.ObservableObject;

namespace WpfClient.Services.NavigationService
{
    internal class ViewNavigationService : ObservableObject, INavigationService<UserControl>
    {
        private readonly Func<Type, UserControl> _viewFactory;
        private UserControl? _currentView;

        public UserControl? CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ViewNavigationService(Func<Type, UserControl> viewFactory)
        {
            _viewFactory = viewFactory;
        }

        public void NavigateTo<TView>() where TView : UserControl
        {
            UserControl view = _viewFactory.Invoke(typeof(TView));
            CurrentView = view;
            Application.Current.MainWindow.MinHeight = view.MinHeight + 35;
            Application.Current.MainWindow.MinWidth = view.MinWidth;
        }
    }
}
