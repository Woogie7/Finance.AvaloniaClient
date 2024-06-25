using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Finance.Application.Interface;
using Finance.AvaloniaClient.HostBulder;
using Finance.AvaloniaClient.Service.Store;
using Finance.AvaloniaClient.ViewModels;
using Finance.AvaloniaClient.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Finance.AvaloniaClient;

public partial class App : Avalonia.Application
{
    public IServiceProvider Services { get; private set; }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();
        collection.AddCommonServices();
        collection.AddViewModels();

        Services = collection.BuildServiceProvider();

        var navigationService = Services.GetRequiredService<NavigationService<LoginViewModel>>();
        navigationService.Navigate();

        var vm = Services.GetRequiredService<MainViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}

