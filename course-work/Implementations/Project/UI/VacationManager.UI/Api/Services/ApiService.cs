using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using VacationManager.UI.Api.Models;
using VacationManager.UI.Api.Services.Abstractions;

namespace VacationManager.UI.Api.Services
{
    public class ApiService(HttpClient httpClient, ILocalStorageService localStorage, ApiConfig config) : IApiService
    {
        private readonly string _baseUrl = config.BaseUrl.TrimEnd('/');

        private async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string relativePath, object? content = null)
        {
            string? tokenSerialized = await localStorage.GetItemAsStringAsync("authToken");

            var url = $"{_baseUrl}/{relativePath.TrimStart('/')}";

            var request = new HttpRequestMessage(method, url);

            if (!string.IsNullOrWhiteSpace(tokenSerialized))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JsonSerializer.Deserialize<string>(tokenSerialized));

            if (content is not null)
            {
                request.Content = new StringContent(
                    JsonSerializer.Serialize(content),
                    Encoding.UTF8,
                    "application/json"
                );
            }

            return request;
        }

        private async Task<ApiResult<T>> SendRequestAsync<T>(HttpRequestMessage request)
        {
            try
            {
                var response = await httpClient.SendAsync(request);
                var apiResult = new ApiResult<T>
                {
                    StatusCode = (int)response.StatusCode,
                    IsSuccess = response.IsSuccessStatusCode
                };

                if (response.IsSuccessStatusCode)
                {
                    if (response.Content.Headers.ContentLength == 0)
                        return apiResult;

                    apiResult.Data = await response.Content.ReadFromJsonAsync<T>();
                }
                else
                {
                    apiResult.ErrorMessage = await response.Content.ReadAsStringAsync();
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                return new ApiResult<T>
                {
                    IsSuccess = false,
                    StatusCode = 0,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<ApiResult<T>> GetAsync<T>(string relativePath)
        {
            var request = await CreateRequestAsync(HttpMethod.Get, relativePath);
            return await SendRequestAsync<T>(request);
        }

        public async Task<ApiResult<TOut>> PostAsync<TIn, TOut>(string relativePath, TIn data)
        {
            var request = await CreateRequestAsync(HttpMethod.Post, relativePath, data);
            return await SendRequestAsync<TOut>(request);
        }

        public async Task<ApiResult<TOut>> PutAsync<TIn, TOut>(string relativePath, TIn data)
        {
            var request = await CreateRequestAsync(HttpMethod.Put, relativePath, data);
            return await SendRequestAsync<TOut>(request);
        }

        public async Task<ApiResult<string>> DeleteAsync(string relativePath)
        {
            var request = await CreateRequestAsync(HttpMethod.Delete, relativePath);
            return await SendRequestAsync<string>(request);
        }
    }
}
