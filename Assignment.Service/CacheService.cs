using Microsoft.Extensions.Caching.Memory;
namespace Assignment.Service
{
    public interface ICacheService
    {
        void Clear();
        Task<T> GetOrCreateAsync<T>(string cacheKey, Func<T> retrieveDataFunc, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null, int? size = null);
        Task<T> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> retrieveDataFunc, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null, int? size = null);
        void Remove(string key);
    }

    public class CacheService : ICacheService
    {
        private static readonly SemaphoreSlim GetUsersSemaphore = new SemaphoreSlim(1, 1);
        private readonly IMemoryCache memoryCache;
        public CacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }
        public async Task<T> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> retrieveDataFunc, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null, int? size = null)
        {
            await GetUsersSemaphore.WaitAsync();
            bool isAvaiable = this.memoryCache.TryGetValue(cacheKey, out T cachedData);
            try
            {
                if (!isAvaiable)
                {
                    if (!this.memoryCache.TryGetValue(cacheKey, out cachedData))
                    {
                        // Data not in cache, retrieve it
                        cachedData = await retrieveDataFunc();

                        // Set cache options
                        var cacheEntryOptions = new MemoryCacheEntryOptions
                        {
                            SlidingExpiration = slidingExpiration ?? TimeSpan.FromSeconds(5),
                            AbsoluteExpiration = absoluteExpiration ?? DateTime.Now.AddSeconds(10),
                            Size = size ?? 1024
                        };

                        // Save data in cache
                        this.memoryCache.Set(cacheKey, cachedData, cacheEntryOptions);
                    }
                }

            }
            catch
            {

            }
            finally
            {
                GetUsersSemaphore.Release();
            }
            return cachedData;
        }
        public async Task<T> GetOrCreateAsync<T>(string cacheKey, Func<T> retrieveDataFunc, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null, int? size = null)
        {
            await GetUsersSemaphore.WaitAsync();
            bool isAvaiable = this.memoryCache.TryGetValue(cacheKey, out T cachedData);
            try
            {
                if (!isAvaiable)
                {
                    if (!this.memoryCache.TryGetValue(cacheKey, out cachedData))
                    {
                        // Data not in cache, retrieve it
                        cachedData = retrieveDataFunc();

                        // Set cache options
                        var cacheEntryOptions = new MemoryCacheEntryOptions
                        {
                            SlidingExpiration = slidingExpiration ?? TimeSpan.FromSeconds(5),
                            AbsoluteExpiration = absoluteExpiration ?? DateTime.Now.AddSeconds(10),
                            Size = size ?? 1024
                        };

                        // Save data in cache
                        this.memoryCache.Set(cacheKey, cachedData, cacheEntryOptions);
                    }
                }

            }
            catch
            {

            }
            finally
            {
                GetUsersSemaphore.Release();
            }
            return cachedData;
        }
        public void Clear()
        {
            this.memoryCache.Dispose();
        }
        public void Remove(string key)
        {
            this.memoryCache.Remove(key);
        }
    }
}
