using Finance.Application.DTOs.DtoCategory;
using Finance.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Finance.AvaloniaClient.Service
{
    internal class CategoryExpenseApiService : ICategoryExpenseApiInterface
    {
        private readonly HttpClient _httpClient;

        public CategoryExpenseApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<CategoryExpenseDto>> GetCategoriesExpenseAsync()
        {
            try
            {
                var categoryExpense = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryExpenseDto>>("api/CategoryExpense");
                return categoryExpense;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                throw ex;
            }
        }
    }
}
