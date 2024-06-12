using Finance.Application.Interface;
using Finance.AvaloniaClient.ViewModels;
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
            collection.AddScoped<IIncomeApiService, IncomeApiService>();
        }
    }
}
