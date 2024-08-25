using App.Domain.Enum;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.Application.Cache
{
    public class RedisCacheManager
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCacheManager> _logger;
        //private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(50); // Compartido entre operaciones de lectura y escritura

        public RedisCacheManager(IDistributedCache cache, ILogger<RedisCacheManager> logger/*, IConnectionMultiplexer connectionMultiplexer*/)
        {
            _cache = cache;
            _logger = logger;
            //_connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            //try
            //{
            //    var data = await _cache.GetStringAsync(key);
            //    if (data != null)
            //    {
            //        return JsonConvert.DeserializeObject<T>(data);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error getting data from cache for key: {Key}", key);
            //}
            
            return null;
        }

        /// <summary>
        /// THIS WORK...USE This ONLY if Nescessary review that when conection fail on program process stop.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        //public async Task<IDictionary<string, byte[]>> GetValuesBaseByTypeAsync(CacheBaseKeyType baseType, CacheValueType valueType)
        //{
        //    var results = new Dictionary<string, byte[]>();
        //    var pattern = $"app_tattoos{baseType.GetDescription()}:{valueType}:*";
        //    var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
        //    var db = _connectionMultiplexer.GetDatabase();

        //    try
        //    {
        //        // Agregar un log para verificar el patrón
        //        _logger.LogInformation("Using pattern: {Pattern}", pattern);

        //        var keys = server.Keys(pattern: pattern).ToArray();

        //        // Agregar un log para verificar si se obtienen claves
        //        _logger.LogInformation("Found {KeyCount} keys matching pattern: {Pattern}", keys.Length, pattern);

        //        foreach (var key in keys)
        //        {
        //            var value = await db.HashGetAsync(key, "data"); // Obtener el campo "data" del hash
        //            if (!value.IsNull)
        //            {
        //                results[key] = (byte[])value;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error getting multiple data from cache for pattern: {Pattern}", pattern);
        //    }

        //    return results;
        //}

        public async Task<IDictionary<string, byte[]>> GetValuesAsync(string[] keys)
        {
            var results = new Dictionary<string, byte[]>();
            var tasks = new List<Task>();

            
            //try
            //{
            //    foreach (var key in keys)
            //    {
            //        tasks.Add(Task.Run(async () =>
            //        {
            //            await _semaphore.WaitAsync();
            //            try
            //            {
            //                var result = await _cache.GetAsync(key);
            //                lock (results)
            //                {
            //                    results[key] = result;
            //                }
            //            }
            //            finally
            //            {
            //                _semaphore.Release();
            //            }
            //        }));
            //    }

            //    await Task.WhenAll(tasks);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error getting multiple data from cache for keys: {Key}", string.Join(',', keys));
            //}
            return results;
        }

        public async Task<IList<string>> GetExistingKeysAsync(string[] keys)
        {
            var existingKeys = new List<string>();
            var tasks = new List<Task>();
            //try
            //{
            //    foreach (var key in keys)
            //    {
            //        tasks.Add(Task.Run(async () =>
            //        {
            //            await _semaphore.WaitAsync();
            //            try
            //            {
            //                var result = await _cache.GetAsync(key);
            //                if (result != null)
            //                {
            //                    lock (existingKeys)
            //                    {
            //                        existingKeys.Add(key);
            //                    }
            //                }
            //            }
            //            finally
            //            {
            //                _semaphore.Release();
            //            }
            //        }));
            //    }

            //    await Task.WhenAll(tasks);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error getting data from cache for key: {Key}", string.Join(',', keys));
            //}
            
            return existingKeys;
        }

        public async Task SetAsync<T>(string key, T data, TimeSpan? expiry = null)
        {
            //try
            //{
            //    var options = new DistributedCacheEntryOptions
            //    {
            //        AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromHours(72) // Default cache time
            //    };
            //    var jsonData = JsonConvert.SerializeObject(data);
            //    await _cache.SetStringAsync(key, jsonData, options);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error setting data in cache for key: {Key}", key);
            //}
        }

        public async Task SetMultipleAsync<T>(string baseKey, IEnumerable<T> items, Func<T, string> keySelector, TimeSpan? expiry = null)
        {
            //try
            //{
            //    var options = new DistributedCacheEntryOptions
            //    {
            //        AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromHours(72)
            //    };

            //    var tasks = items.Select(async item =>
            //    {
            //        await _semaphore.WaitAsync();
            //        try
            //        {
            //            var key = $"{baseKey}:{keySelector(item)}";
            //            var jsonData = JsonConvert.SerializeObject(item);
            //            await _cache.SetStringAsync(key, jsonData, options);
            //        }
            //        finally
            //        {
            //            _semaphore.Release();
            //        }
            //    });

            //    await Task.WhenAll(tasks);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error setting data in cache for basekey: {Key}", baseKey);
            //}
            
        }

        public async Task SetMultipleByKeyAsync<T>( (long,T)[] items, CacheBaseKeyType baseType, CacheValueType valueType, TimeSpan? expiry = null)
        {
            //try
            //{
            //    var options = new DistributedCacheEntryOptions
            //    {
            //        AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromHours(72)
            //    };

            //    var tasks = items.Select(async item =>
            //    {
            //        await _semaphore.WaitAsync();
            //        try
            //        {
            //            var key = KeyBuilder.BuildCacheKey(existKey: false, baseType, id:item.Item1, valueType);
                        
            //            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(item.Item2), options);
            //        }
            //        finally
            //        {
            //            _semaphore.Release();
            //        }
            //    });

            //    await Task.WhenAll(tasks);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error setting data in cache for key: {Key}", string.Join(",",items.ToArray()));
            //}
            
        }

        public async Task RemoveAsync(string key)
        {
            //try
            //{
            //    await _cache.RemoveAsync(key);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error removing key from cache: {Key}", key);
            //}
        }
    }

}
