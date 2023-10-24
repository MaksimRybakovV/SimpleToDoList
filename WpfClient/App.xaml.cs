using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfClient.Services.NavigationService;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    services.AddHttpClient();

                    services.AddSingleton<MainWindow>();

                    services.AddSingleton<INavigationService<UserControl>, ViewNavigationService>();
                    services.AddSingleton<Func<Type, UserControl>>(serviceProvider => userControl => (UserControl)serviceProvider.GetRequiredService(userControl));
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
