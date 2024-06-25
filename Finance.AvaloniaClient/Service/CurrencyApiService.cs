using Finance.Application.DTOs.DtoCurrency;
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
    internal class CurrencyApiService : ICurrencyApiInterface
    {
        private readonly HttpClient _httpClient;

        public CurrencyApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<CurrencyDto>?> GetCurenciesAsync()
        {
            try
            {
                var currency = await _httpClient.GetFromJsonAsync<IEnumerable<CurrencyDto>>("api/Currency");
                return currency;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                throw ex;
            }

        }
    }
}
