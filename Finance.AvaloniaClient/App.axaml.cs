using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Finance.AvaloniaClient.Service;
using Finance.AvaloniaClient.ViewModels;
using Finance.AvaloniaClient.Views;
using Microsoft.Extensions.DependencyInjection;

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

        var vm = services.GetRequiredService<MainViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
