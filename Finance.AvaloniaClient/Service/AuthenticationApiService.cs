using Finance.Application.DTOs;
using Finance.Application.DTOs.Income;
using Finance.Application.DTOs.UserDto;
using Finance.Application.Interface;
using Finance.Domain.Exceptions;
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
    internal class AuthenticationApiService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDto> Login(UserDto userDto)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/User/Login/", itemJson);

                if (response.IsSuccessStatusCode)
                {
                    if (response.Headers.TryGetValues("Set-Cookie", out var cookieHeaders))
                    {
                        foreach (var cookie in cookieHeaders)
                        {
                            var cookies = cookie;
                        }
                        return userDto;
                    }
                    return null;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new UserNotFoundException(userDto.Email);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new InvalidPasswordException(userDto.Email, userDto.PasswordHash);
                }
                else
                {
                    throw new Exception("Unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }

        public Task Register(CreateUserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}
