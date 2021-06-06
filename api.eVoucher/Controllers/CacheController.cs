using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.eVoucher.Controllers
{
    [Route("[controller]")]
    public class CacheController : Controller
    {
		private readonly IDistributedCache _distributedCache;

		public CacheController(IDistributedCache distributedCache)
		{
			_distributedCache = distributedCache;
		}

		[HttpGet]
		public async Task<string> Get()
		{
			var cacheKey = "TheTime";
			var existingTime = await _distributedCache.GetStringAsync(cacheKey);
			if (!string.IsNullOrEmpty(existingTime))
			{
				return "Fetched from cache : " + existingTime;
			}
			else
			{
				existingTime = DateTime.UtcNow.ToString();
				await _distributedCache.SetStringAsync(cacheKey, existingTime);
				return "Added to cache : " + existingTime;
			}
		}
	}
}
