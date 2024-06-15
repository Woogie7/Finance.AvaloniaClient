using Finance.Application.DTOs;
using Finance.Application.DTOs.Income;
using Finance.Application.Interface;
using Finance.Domain.Model;
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
    internal class IncomeApiService : IIncomeApiService
    {
        private readonly HttpClient _httpClient;

        public IncomeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<IncomeDTO>?> GetIncomesAsync()
        {
            try
            {
                var incomes = await _httpClient.GetFromJsonAsync<IEnumerable<IncomeDTO>>("api/Income");
                return incomes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                throw ex;
            }

        }

        public async Task<CreateIncomeDto> AddIncomeAsync(CreateIncomeDto newIncome)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(newIncome), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/Income/", itemJson);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStreamAsync();

                    var addIncome = await JsonSerializer.DeserializeAsync<CreateIncomeDto>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return addIncome;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                throw ex;
            }
        }

        public async Task<bool> DeleteIncomeAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Income/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                throw ex;
            }
        }
        public async Task<bool> DeleteAllIncomeAsync()
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Income/DeleteAllIncome");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStreamAsync();

                    var addIncome = await JsonSerializer.DeserializeAsync<DeteilsIncomeDTO>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<CategorySummary>?> GetCat(string selectedCurrency)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(selectedCurrency))
                    return null;
                var cat = await _httpClient.GetFromJsonAsync<IEnumerable<CategorySummary>>($"api/Income/Cat/{selectedCurrency}");
                return cat;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message); throw ex;
            }
        }
    }
}
