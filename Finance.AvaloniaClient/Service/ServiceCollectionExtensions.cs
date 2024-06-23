using Finance.Application.Interface;
using Finance.AvaloniaClient.Service.Store;
using Finance.AvaloniaClient.ViewModels;
using Finance.AvaloniaClient.Views;
using LiveChartsCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Finance.AvaloniaClient.Service
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });

            collection.AddTransient<MainViewModel>();
            collection.AddTransient<MonthNavigationViewModel>();

            collection.AddScoped<IIncomeApiService, IncomeApiService>();
            collection.AddScoped<IExpenseApiService, ExpenseApiService>();
            collection.AddSingleton<IAuthenticator, Authenticator>();
            collection.AddSingleton<IAuthenticationService, AuthenticationApiService>();

            collection.AddTransient<FinanceView>();
            collection.AddTransient<MainWindow>();

            collection.AddSingleton<NavigationStore>();


            collection.AddTransient<LoginView>();
            collection.AddTransient<LoginViewModel>();
            collection.AddSingleton<Func<LoginViewModel>>(s => () => s.GetRequiredService<LoginViewModel>());
            collection.AddSingleton<NavigationService<LoginViewModel>>();

            collection.AddTransient<FinanceView>();
            collection.AddTransient<FinanceViewModel>();
            collection.AddSingleton<Func<FinanceViewModel>>(s => () => s.GetRequiredService<FinanceViewModel>());
            collection.AddSingleton<NavigationService<FinanceViewModel>>();
        }
    }
}
