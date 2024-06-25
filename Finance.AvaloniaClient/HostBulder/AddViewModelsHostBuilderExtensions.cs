using Finance.Application.Interface;
using Finance.AvaloniaClient.Service.Store;
using Finance.AvaloniaClient.ViewModels;
using Finance.AvaloniaClient.Views;
using LiveChartsCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.AvaloniaClient.HostBulder
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static void AddViewModels(this IServiceCollection collection)
        {
            collection.AddTransient((s) => CreateFinanceViewModel(s));
            collection.AddSingleton<Func<FinanceViewModel>>((s) => () => s.GetRequiredService<FinanceViewModel>());
            collection.AddSingleton<NavigationService<FinanceViewModel>>();

            collection.AddTransient<LoginView>();
            collection.AddTransient<LoginViewModel>();
            collection.AddSingleton<Func<LoginViewModel>>(s => () => s.GetRequiredService<LoginViewModel>());
            collection.AddSingleton<NavigationService<LoginViewModel>>();

            collection.AddTransient<AddIncomeView>();
            collection.AddTransient<AddIncomeViewModel>();
            collection.AddSingleton<Func<AddIncomeViewModel>>(s => () => s.GetRequiredService<AddIncomeViewModel>());
            collection.AddSingleton<NavigationService<AddIncomeViewModel>>();

            collection.AddTransient<AddExpenseeView>();
            collection.AddTransient<AddExpenseViewModel>();
            collection.AddSingleton<Func<AddExpenseViewModel>>(s => () => s.GetRequiredService<AddExpenseViewModel>());
            collection.AddSingleton<NavigationService<AddExpenseViewModel>>();

        }

        private static FinanceViewModel CreateFinanceViewModel(IServiceProvider services)
        {
            return FinanceViewModel.CreateAsync(
                services.GetRequiredService<IIncomeApiService>(),
                services.GetRequiredService<IExpenseApiService>(),
                services.GetRequiredService<NavigationService<AddIncomeViewModel>>(),
                services.GetRequiredService<NavigationService<AddExpenseViewModel>>());

        }
    }
}
