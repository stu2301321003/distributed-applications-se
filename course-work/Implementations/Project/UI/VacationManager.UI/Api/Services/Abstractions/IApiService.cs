using VacationManager.UI.Api.Models;

namespace VacationManager.UI.Api.Services.Abstractions
{
    public interface IApiService
    {
        Task<ApiResult<string>> DeleteAsync(string relativePath);
        Task<ApiResult<T>> GetAsync<T>(string relativePath);
        Task<ApiResult<TOut>> PostAsync<TIn, TOut>(string relativePath, TIn data);
        Task<ApiResult<TOut>> PutAsync<TIn, TOut>(string relativePath, TIn data);
    }
}