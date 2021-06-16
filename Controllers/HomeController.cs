using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RedisTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RedisTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache cache;

        public HomeController(ILogger<HomeController> logger, IDistributedCache _cache)
        {
            _logger = logger;
            this.cache = _cache;
        }

        public IActionResult Index()
        {
            string key = "test";
            string value = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //編輯快取
            cache.SetString(key, value);
            //獲取快取
            var values = cache.GetString(key);
            //更新快取過期時間
            cache.RefreshAsync(key);
            //刪除快取
            //cache.RemoveAsync(key);
            ViewBag.values = values;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
