using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Resiliency.Cache.Controllers
{
    [Route("")]
    public class ValuesController : Controller
    {
        private readonly IMemoryCache _cache;

        public ValuesController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpGet("{key}")]
        public string Get(string key)
        {
            return _cache.Get<string>(key);
        }

        [HttpPut("{key}")]
        public void Put(string key, [FromBody]string value)
        {
            _cache.Set(key, value);
        }

        [HttpDelete("{key}")]
        public void Delete(string key)
        {
            _cache.Remove(key);
        }
    }
}
