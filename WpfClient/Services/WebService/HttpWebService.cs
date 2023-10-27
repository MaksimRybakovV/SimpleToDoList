using Entities.Dtos.TodoTaskDtos;
using Entities.Dtos.UserDtos;
using Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Documents;

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

            HttpResponseMessage message = await client.GetAsync($"login?username={username}&password={passwordHash}");
            var responseJson = await message.Content.ReadAsStringAsync();
            var deserealizedResponse = JsonConvert.DeserializeObject<ServiceResponse<GetUserDto>>(responseJson);
            return deserealizedResponse!;
        }

        public async Task<ServiceResponse<GetUserDto>> Logout(int id, string token)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            ConfigureAuthClient(client, "https://localhost:7130/api/authorizations/", token);

            var response = await client.PutAsJsonAsync("logout/{id}", id);
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
            ConfigureClient(client, "https://localhost:7130/api/");

            var response = await client.PostAsJsonAsync("users", newUser);
            var responseJson = await response.Content.ReadAsStringAsync();
            var deserealizedResponse = JsonConvert.DeserializeObject<ServiceResponse<int>>(responseJson);
            return deserealizedResponse!;
        }

        public async Task<PageServiceResponse<List<GetTodoTaskDto>>> GetTasksPageByUser(int id, int page, int pageSize, string token)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            ConfigureAuthClient(client, $"https://localhost:7130/api/users/{id}/todotasks/", token);

            HttpResponseMessage message = await client.GetAsync($"pagination?page={page}&pageSize={pageSize}");
            var responseJson = await message.Content.ReadAsStringAsync();
            var deserealizedResponse = JsonConvert.DeserializeObject<PageServiceResponse<List<GetTodoTaskDto>>>(responseJson);
            return deserealizedResponse!;
        }

        public async Task<ServiceResponse<string>> DeleteTask(int id, string token)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            ConfigureAuthClient(client, $"https://localhost:7130/api/users/todotasks/", token);

            var response = await client.DeleteAsync($"{id}");
            var responseJson = await response.Content.ReadAsStringAsync();
            var deserealizedResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(responseJson);
            return deserealizedResponse!;
        }

        public async Task<ServiceResponse<string>> UpdateTask(UpdateTodoTaskDto updatedTask, string token)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            ConfigureAuthClient(client, $"https://localhost:7130/api/users/todotasks/", token);

            var response = await client.PutAsJsonAsync($"{updatedTask.Id}", updatedTask);
            var responseJson = await response.Content.ReadAsStringAsync();
            var deserealizedResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(responseJson);
            return deserealizedResponse!;
        }

        public async Task<ServiceResponse<int>> AddTask(AddTodoTaskDto newTask, int id, string token)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            ConfigureAuthClient(client, $"https://localhost:7130/api/users/todotasks/", token);

            var response = await client.PostAsJsonAsync($"?id={id}", newTask);
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
