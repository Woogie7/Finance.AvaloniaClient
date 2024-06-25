using Finance.Application.Interface;
using Finance.AvaloniaClient.Service;
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

namespace Finance.AvaloniaClient.HostBulder
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
            collection.AddScoped<ICategoryIncomeApiInterface, CategoryIncomeApiService>();
            collection.AddScoped<ICategoryExpenseApiInterface, CategoryExpenseApiService>();
            collection.AddScoped<ICurrencyApiInterface, CurrencyApiService>();
            collection.AddSingleton<IAuthenticator, Authenticator>();
            collection.AddSingleton<IAuthenticationService, AuthenticationApiService>();

            collection.AddTransient<MainWindow>();

            collection.AddSingleton<NavigationStore>();
        }
    }
}
