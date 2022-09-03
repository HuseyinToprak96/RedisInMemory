using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDB(0);//db0 ı al
        }

        public IActionResult Index()
        {
            db.StringSet("name", "Huseyin Toprak");
            db.StringSet("ziyaretci", 100);
            return View();
        }
        public IActionResult Show()
        {
            // var value1= db.StringGet("name");
            //var value1 = db.StringGetRange("name", 0, 3); 0 ıncı değerden 3. değere kadarını ver
            var value1 = db.StringLength("name");
           // if(value1.HasValue)//var mı kontrol için kullanılır.
            ViewBag.Name = value1;
            db.StringIncrement("ziyaretci", 1);//1 1 arttırır.
            var count=  db.StringDecrementAsync("ziyaretci", 2).Result;//2 2 azaltır.
            db.StringDecrementAsync("ziyaretci", 3).Wait();//burda wait in anlamı geri dönen veri önemli değil sadece çalıştır anlamındadır...
            ViewBag.ziyaretci = count;
            return View();
        }
    }
}
