using Entities.Dtos.UserDtos;
using Entities.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WpfClient.Services.WebService
{
    internal class HttpWebService : IWebService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpWebService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ServiceResponse<GetUserDto>> Authorization(string username, string passwordHash)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            ConfigureClient(client, "https://localhost:7130/api/authorizations/");

            var response = await client.GetStringAsync($"login?username={username}&passwordHash={passwordHash}");
            var deserealizedResponse = JsonConvert.DeserializeObject<ServiceResponse<GetUserDto>>(response);
            return deserealizedResponse!;
        }

        public async Task<ServiceResponse<GetUserDto>> Logout(int id, string token)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            ConfigureAuthClient(client, "https://localhost:7130/api/authorizations/", token);

            var response = await client.PutAsJsonAsync("logout", id);
            var responseJson = await response.Content.ReadAsStringAsync();
            var deserealizedResponse = JsonConvert.DeserializeObject<ServiceResponse<GetUserDto>>(responseJson);
            return deserealizedResponse!;
        }

        public async Task<ServiceResponse<GetUserDto>> RefreshToken(TokenUserDto user, string token)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            ConfigureAuthClient(client, "https://localhost:7130/api/", token);

            var response = await client.PutAsJsonAsync("users", user);
            var responseJson = await response.Content.ReadAsStringAsync();
            var deserealizedResponse = JsonConvert.DeserializeObject<ServiceResponse<GetUserDto>>(responseJson);
            return deserealizedResponse!;
        }

        public async Task<ServiceResponse<int>> Registration(AddUserDto newUser)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            ConfigureClient(client, "https://localhost:7130/api/authorizations/");

            var response = await client.PostAsJsonAsync("login", newUser);
            var responseJson = await response.Content.ReadAsStringAsync();
            var deserealizedResponse = JsonConvert.DeserializeObject<ServiceResponse<int>>(responseJson);
            return deserealizedResponse!;
        }

        private void ConfigureClient(HttpClient client, string url)
        {
            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        private void ConfigureAuthClient(HttpClient client, string url, string token)
        {
            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
