using Finance.Application.DTOs;
using Finance.Application.DTOs.DtoExpense;
using Finance.Application.DTOs.Income;
using Finance.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Finance.AvaloniaClient.Service
{
    internal class ExpenseApiService : IExpenseApiService
    {
        private readonly HttpClient _httpClient;

        public ExpenseApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreateExpenseDto> AddExpenseAsync(CreateExpenseDto newExpense)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(newExpense), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/Expense/", itemJson);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStreamAsync();

                    var addExpense = await JsonSerializer.DeserializeAsync<CreateExpenseDto>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return addExpense;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpenseAsync()
        {
            try
            {
                var expenses = await _httpClient.GetFromJsonAsync<IEnumerable<ExpenseDto>>("api/Expense");
                return expenses;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                throw ex;
            }
        }
    }
}
