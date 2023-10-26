using Entities.Dtos.TodoTaskDtos;
using System.ComponentModel;
using System.Net;
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
    class TableViewModel : BaseViewModel
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

        public BindingList<GetTodoTaskDto> TasksPage { get; set; } = new BindingList<GetTodoTaskDto>();

        public ICommand LoadTasksCommand { get; }

        public TableViewModel(IWebService service, INavigationService<UserControl> navigation, IAuthorizationService authorization)
        {
            _service = service;
            Navigation = navigation;
            Authorization = authorization;

            LoadTasksCommand = new RelayCommand(OnLoadTasksCommand, CanLoadTasksCommand);
        }

        private bool CanLoadTasksCommand(object parameter) => true;

        private async void OnLoadTasksCommand(object parameter)
        {
            var response = await _service.GetTasksPageByUser(Authorization!.CurrentUser.Id, 1, 10);

            if (response.IsSuccessful == false)
            {
                MessageBox.Show(response?.Message);
                return;
            }

            TasksPage = new BindingList<GetTodoTaskDto>(response.Data);
            OnPropertyChanged(nameof(TasksPage));
        }
    }
}
