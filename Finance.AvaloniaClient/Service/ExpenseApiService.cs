using Finance.Application.DTOs.DtoExpense;
using Finance.Application.DTOs.Income;
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
    internal class ExpenseApiService : IExpenseApiService
    {
        private readonly HttpClient _httpClient;

        public ExpenseApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
