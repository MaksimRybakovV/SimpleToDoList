using AutoMapper;
using Entities.Dtos.TodoTaskDtos;
using Entities.Dtos.UserDtos;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfClient.Infrastructure.Commands;
using WpfClient.Services.AuthorizationService;
using WpfClient.Services.NavigationService;
using WpfClient.Services.WebService;
using WpfClient.View.Windows;
using WpfClient.ViewModel.Base;

namespace WpfClient.ViewModel
{
    class TableViewModel : BaseViewModel
    {
        private readonly IWebService _service;
        private readonly IMapper _mapper;
        private INavigationService<UserControl>? _navigation;
        private IAuthorizationService? _authorization;

        private const int _startPage = 1;
        private const int _pageSize = 10;

        private GetTodoTaskDto? _selectedTask;

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

        public GetTodoTaskDto TaskLayout { get; set; } = new();
        public int CurrentPage { get; set; } = _startPage;
        public int PageCount { get; set; }
        public string PageText => $"{CurrentPage} of {PageCount}";
        public string UserString => $"{Authorization?.CurrentUser.FirstName} {Authorization?.CurrentUser.LastName}";
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime Time { get; set; } = DateTime.Now;

        public BindingList<GetTodoTaskDto> TasksPage { get; set; } = new BindingList<GetTodoTaskDto>();

        public ICommand LoadTasksCommand { get; }
        public ICommand SwitchToNextPageCommand { get; }
        public ICommand SwitchToPrevPageCommand { get; }
        public ICommand SwitchToFirstPageCommand { get; }
        public ICommand SwitchToLastPageCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand SetSelectedItemCommand { get; }
        public ICommand ResetSelectedItemCommand { get; }
        public ICommand DeleteSelectedItemCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand UpdateItemCommand { get; }

        public TableViewModel(IWebService service, IMapper mapper, INavigationService<UserControl> navigation, IAuthorizationService authorization)
        {
            _service = service;
            _mapper = mapper;
            Navigation = navigation;
            Authorization = authorization;

            LoadTasksCommand = new RelayCommand(OnLoadTasksCommand, CanLoadTasksCommand);
            SwitchToNextPageCommand = new RelayCommand(OnSwitchToNextPageCommand, CanSwitchToNextPageCommand);
            SwitchToPrevPageCommand = new RelayCommand(OnSwitchToPrevPageCommand, CanSwitchToPrevPageCommand);
            SwitchToFirstPageCommand = new RelayCommand(OnSwitchToFirstPageCommand, CanSwitchToFirstPageCommand);
            SwitchToLastPageCommand = new RelayCommand(OnSwitchToLastPageCommand, CanSwitchToLastPageCommand);
            LogoutCommand = new RelayCommand(OnLogoutCommand, CanLogoutCommand);
            SetSelectedItemCommand = new RelayCommand(OnSetSelectedItemCommand, CanSetSelectedItemCommand);
            ResetSelectedItemCommand = new RelayCommand(OnResetSelectedItemCommand, CanResetSelectedItemCommand);
            DeleteSelectedItemCommand = new RelayCommand(OnDeleteSelectedItemCommand, CanDeleteSelectedItemCommand);
            AddItemCommand = new RelayCommand(OnAddItemCommand, CanAddItemCommand);
            UpdateItemCommand = new RelayCommand(OnUpdateItemCommand, CanUpdateItemCommand);
        }

        private bool CanLoadTasksCommand(object parameter) => true;

        private async void OnLoadTasksCommand(object parameter)
        {
            await GetPage(_startPage);
            await RefreshToken();
            ResetDates();
        }

        private bool CanSwitchToNextPageCommand(object parameter) => CurrentPage < PageCount;

        private async void OnSwitchToNextPageCommand(object parameter)
        {
            CurrentPage++;
            await GetPage(CurrentPage);
            await RefreshToken();
            ResetDates();
        }

        private bool CanSwitchToPrevPageCommand(object parameter) => CurrentPage > 1;

        private async void OnSwitchToPrevPageCommand(object parameter)
        {
            CurrentPage--;
            await GetPage(CurrentPage);
            await RefreshToken();
            ResetDates();
        }

        private bool CanSwitchToFirstPageCommand(object parameter) => CurrentPage > 1;

        private async void OnSwitchToFirstPageCommand(object parameter)
        {
            CurrentPage = 1;
            await GetPage(CurrentPage);
            await RefreshToken();
            ResetDates();
        }

        private bool CanSwitchToLastPageCommand(object parameter) => CurrentPage < PageCount;

        private async void OnSwitchToLastPageCommand(object parameter)
        {
            CurrentPage = PageCount;
            await GetPage(CurrentPage);
            await RefreshToken();
            ResetDates();
        }

        private bool CanLogoutCommand(object parameter) => true;

        private async void OnLogoutCommand(object parameter)
        {
            var response = await _service.Logout(Authorization!.CurrentUser.Id, Authorization!.CurrentUser.Token);

            if (response.IsSuccessful == false)
            {
                MessageBox.Show(response?.Message);
                return;
            }

            Authorization.SetCurrentUser(response.Data);
            Navigation?.NavigateTo<AuthorizationView>();
        }

        private bool CanSetSelectedItemCommand(object parameter)
        {
            if(parameter is null)
            {
                _selectedTask = null;
                TaskLayout = new();
                OnPropertyChanged(nameof(TaskLayout));
                return false;
            }

            return true;
        }

        private void OnSetSelectedItemCommand(object parameter)
        {
            var originalTask = parameter as GetTodoTaskDto;
            var task = (GetTodoTaskDto)originalTask!.Clone();

            _selectedTask = task;
            TaskLayout = _selectedTask;
            Date = TaskLayout.Deadline;
            Time = TaskLayout.Deadline;
            OnPropertyChanged(nameof(TaskLayout));
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Time));
        }

        private bool CanResetSelectedItemCommand(object parameter) => _selectedTask is not null;

        private void OnResetSelectedItemCommand(object parameter)
        {
            var table = parameter as DataGrid;
            table!.SelectedItem = null;

            _selectedTask = null;
            TaskLayout = new();
            OnPropertyChanged(nameof(TaskLayout));
            ResetDates();
        }

        private bool CanDeleteSelectedItemCommand(object parameter) => _selectedTask is not null;

        private async void OnDeleteSelectedItemCommand(object parameter)
        {
            var response = await _service.DeleteTask(_selectedTask!.Id, Authorization!.CurrentUser.Token);

            if (response.IsSuccessful == false)
            {
                MessageBox.Show(response?.Message);
                return;
            }

            MessageBox.Show(response?.Data);

            await RefreshToken();
            await GetPageAfterDeleting();
        }

        private bool CanAddItemCommand(object parameter) => _selectedTask is null;

        private async void OnAddItemCommand(object parameter)
        {
            TaskLayout.Deadline = Date.AddHours(Time.Hour)
                .AddMinutes(Time.Minute)
                .AddSeconds(Time.Second);

            var newUser = _mapper.Map<AddTodoTaskDto>(TaskLayout);

            var response = await _service.AddTask(newUser, Authorization!.CurrentUser.Id, Authorization!.CurrentUser.Token);

            if (response.IsSuccessful == false)
            {
                MessageBox.Show(response?.Message);
                return;
            }

            await RefreshToken();
            await GetPage(CurrentPage);
        }

        private bool CanUpdateItemCommand(object parameter) => _selectedTask is not null;

        private async void OnUpdateItemCommand(object parameter)
        {
            TaskLayout.Deadline = Date.AddHours(Time.Hour)
                .AddMinutes(Time.Minute)
                .AddSeconds(Time.Second);

            var updatedUser = _mapper.Map<UpdateTodoTaskDto>(TaskLayout);

            var response = await _service.UpdateTask(updatedUser, Authorization!.CurrentUser.Token);

            if (response.IsSuccessful == false)
            {
                MessageBox.Show(response?.Message);
                return;
            }

            MessageBox.Show(response?.Data);

            await RefreshToken();
            await GetPage(CurrentPage);
        }

        private async Task GetPage(int page)
        {
            var response = await _service.GetTasksPageByUser(Authorization!.CurrentUser.Id, page, _pageSize, Authorization!.CurrentUser.Token);

            if (response.IsSuccessful == false)
            {
                MessageBox.Show(response?.Message);
                return;
            }

            CurrentPage = response.CurrentPage;
            PageCount = response.PageCount;
            TasksPage = new BindingList<GetTodoTaskDto>(response.Data);
            OnPropertyChanged(nameof(TasksPage));
            OnPropertyChanged(nameof(PageText));
        }

        private async Task GetPageAfterDeleting()
        {
            var response = await _service.GetTasksPageByUser(Authorization!.CurrentUser.Id, CurrentPage, _pageSize, Authorization!.CurrentUser.Token);

            if (response.IsSuccessful == false)
            {
                CurrentPage--;
                response = await _service.GetTasksPageByUser(Authorization!.CurrentUser.Id, CurrentPage, _pageSize, Authorization!.CurrentUser.Token);

                if(response.IsSuccessful == false)
                {
                    MessageBox.Show(response?.Message);
                    return;
                }
            }

            CurrentPage = response.CurrentPage;
            PageCount = response.PageCount;
            TasksPage = new BindingList<GetTodoTaskDto>(response.Data);
            OnPropertyChanged(nameof(TasksPage));
            OnPropertyChanged(nameof(PageText));
        }

        private async Task RefreshToken()
        {
            var currentUser = _mapper.Map<TokenUserDto>(Authorization!.CurrentUser);
            var response = await _service.RefreshToken(currentUser, Authorization.CurrentUser.Token);
            if (response.IsSuccessful == false)
            {
                MessageBox.Show("Due to long inactivity, reauthorization is required.");
                Navigation!.NavigateTo<AuthorizationView>();
            }

            Authorization.SetCurrentUser(response.Data);
        }

        private void ResetDates()
        {
            Date = DateTime.Now;
            Time = DateTime.Now;
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Time));
        }
    }
}
