using Finance.Application.DTOs.DtoCategory;
using Finance.Application.DTOs.DtoCurrency;
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
    internal class CategoryIncomeApiService : ICategoryIncomeApiInterface
    {
        private readonly HttpClient _httpClient;

        public CategoryIncomeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<CategoryIncomeDto>> GetCategoriesAsync()
        {
            try
            {
                var categoryIncome = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryIncomeDto>>("api/CategoryIncome");
                return categoryIncome;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                throw ex;
            }
        }
    }
}
