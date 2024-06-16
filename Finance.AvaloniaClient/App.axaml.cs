using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Finance.Application.Interface;
using Finance.AvaloniaClient.Service;
using Finance.AvaloniaClient.ViewModels;
using Finance.AvaloniaClient.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Finance.AvaloniaClient;

public partial class App : Avalonia.Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();
        collection.AddCommonServices();

        var services = collection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            InitializeViewModelAsync(desktop.MainWindow, services);
        }
        

        base.OnFrameworkInitializationCompleted();
    }

    private async void InitializeViewModelAsync(object view, IServiceProvider services)
    {
        var vm = await MainViewModel.CreateAsync(services.GetRequiredService<IIncomeApiService>(), services.GetRequiredService<IExpenseApiService>());

        if (view is MainWindow mainWindow)
        {
            mainWindow.DataContext = vm;
        }
    }
}

