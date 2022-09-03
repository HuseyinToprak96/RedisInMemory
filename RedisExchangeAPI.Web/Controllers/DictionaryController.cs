using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string hashKey = "sozluk";
        public DictionaryController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDB(4);
        }
        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            if (db.KeyExists(hashKey))
            {
                //  db.HashGet(hashKey, "Pen");//tek veriyi almak için kullanılır.
               // bool control= db.HashExists(hashKey,"pen");  //var mı yok mu kontrol için
                db.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name,x.Value);
                });
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string key,string value)
        {
            db.HashSet(hashKey, key, value);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Remove(string key)
        {
            db.HashDelete(hashKey, key);
            return RedirectToAction("Index");
        }
    }
}
