namespace WpfClient.Services.NavigationService
{
    internal interface INavigationService<T>
    {
        public T? CurrentView { get; set; }
        public void NavigateTo<V>() where V : T;
    }
}
