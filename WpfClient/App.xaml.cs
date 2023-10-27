using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfClient.Services.AuthorizationService;
using WpfClient.Services.NavigationService;
using WpfClient.Services.WebService;
using WpfClient.View.Windows;
using WpfClient.ViewModel;

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

                    services.AddSingleton<AuthorizationViewModel>();
                    services.AddSingleton<MainWindowViewModel>();
                    services.AddSingleton<RegistrationViewModel>();
                    services.AddSingleton<TableViewModel>();

                    services.AddAutoMapper(typeof(App).Assembly);

                    services.AddSingleton<INavigationService<UserControl>, ViewNavigationService>();
                    services.AddSingleton<IAuthorizationService, AuthorizationService>();
                    services.AddSingleton<IWebService, HttpWebService>();
                    services.AddSingleton<Func<Type, UserControl>>(serviceProvider => userControl => (UserControl)serviceProvider.GetRequiredService(userControl));

                    services.AddSingleton((services) => new MainWindow()
                    {
                        DataContext = services.GetRequiredService<MainWindowViewModel>()
                    });

                    services.AddSingleton((services) => new AuthorizationView()
                    {
                        DataContext = services.GetRequiredService<AuthorizationViewModel>()
                    });

                    services.AddSingleton((services) => new RegistrationView()
                    {
                        DataContext = services.GetRequiredService<RegistrationViewModel>()
                    });

                    services.AddSingleton((services) => new TableView()
                    {
                        DataContext = services.GetRequiredService<TableViewModel>()
                    });
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
