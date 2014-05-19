using System;
using System.Threading.Tasks;

namespace ResourcesFirstTranslations.Services
{
    public interface ICacheService
    {
        T Get<T>(string cacheKey, Func<T> loaderFunction);
        Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> loaderFunction);
        void Invalidate(string cacheKey);
        bool IsCacheEnabled { get; set; }
    }
}