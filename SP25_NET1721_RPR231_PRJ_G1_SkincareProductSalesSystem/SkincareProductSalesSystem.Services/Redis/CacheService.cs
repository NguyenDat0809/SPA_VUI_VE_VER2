using StackExchange.Redis;
using System.Text.Json;


namespace SkincareProductSalesSystem.Services.ExtendServices
{

    public class CacheService : ICacheService
    {
        private IDatabase _cacheDb;
        public CacheService() 
        {
            var redis = ConnectionMultiplexer.Connect("redis-16060.c321.us-east-1-2.ec2.redns.redis-cloud.com:16060,password=JeHRL3CpEV1vfKOkl7n5MPhG8RQE6LDL,abortConnect=false");
            _cacheDb = redis.GetDatabase();
        }

        public async Task<T> GetDataAsync<T>(string key)
        {
            var value = await _cacheDb.StringGetAsync(key);
            if (!value.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<T>(value.ToString());
            }
            return default;

        }
        public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset? expirationTime = null)
        {
            TimeSpan? expiryTime = expirationTime.HasValue
                ? expirationTime.Value - DateTimeOffset.Now
                : (TimeSpan?)null;

            return await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
        }

        public async Task<object> RemoveDataAsync(string key)
        {
            var exist = _cacheDb.KeyExists(key);
            if (exist)
            {
                return await _cacheDb.KeyDeleteAsync(key);
            }
            return false;
        }


        public Task<List<T>> GetListDataAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetListDataAsync<T>(string key, List<T> value, DateTimeOffset? expirationTime)
        {
            throw new NotImplementedException();
        }
        //T ICacheService.GetData<T>(string key)
        //{
        //    var value = _cacheDb.StringGet(key);
        //    if (!string.IsNullOrEmpty(value))
        //    {
        //        return JsonSerializer.Deserialize<T>(value);
        //    }
        //    return default;
        //}

        //List<T> ICacheService.GetListData<T>(string key)
        //{
        //    throw new NotImplementedException();
        //}

        //object ICacheService.RemoveData(string key)
        //{
        //    var exist = _cacheDb.KeyExists(key);
        //    if (exist)
        //    {
        //        return _cacheDb.KeyDelete(key);
        //    }
        //    return false;
        //}

        //bool ICacheService.SetData<T>(string key, T value, DateTimeOffset expirationTime)
        //{
        //    var expirtyTime = expirationTime.DateTime.Subtract(DateTime.Now);
        //    return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expirtyTime);
        //}

        //bool ICacheService.SetListData<T>(string key, List<T> value, DateTimeOffset expirationTime)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
