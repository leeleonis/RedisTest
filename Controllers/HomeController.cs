using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RedisTest.Models;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RedisTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache _cache;
        private readonly IRedisCacheClient _redisCacheClient;

        public HomeController(ILogger<HomeController> logger, IDistributedCache cache, IRedisCacheClient redisCacheClient)
        {
            _logger = logger;
            _cache = cache;
            _redisCacheClient = redisCacheClient;
        }

        public IActionResult Index()
        {
            string key = "test";
            string value = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //編輯快取
            _cache.SetString(key, value);
            //獲取快取
            var values = _cache.GetString(key);
            //更新快取過期時間
            _cache.RefreshAsync(key);
            //刪除快取
            //cache.RemoveAsync(key);
            ViewBag.values = values;
            var valuest = new List<Tuple<string, Product>>
    {
        new Tuple<string, Product>("Product1", new Product()
        {
            Id = 1,
            Name = "hand sanitizer 1",
            Price = 100
        }),
        new Tuple<string, Product>("Product2",new Product()
        {
            Id = 2,
            Name = "hand sanitizer 2",
            Price = 200
        }),
        new Tuple<string, Product>("Product3", new Product()
        {
            Id = 3,
            Name = "hand sanitizer 3",
            Price = 300
        })
    };

            _redisCacheClient.Db3.AddAsync(key + 3, valuest, DateTimeOffset.Now.AddMinutes(10));
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

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}