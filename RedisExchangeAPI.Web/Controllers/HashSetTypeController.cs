using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashSetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "hashNames";
        public HashSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDB(2);
        }
        public IActionResult Index()
        {
            HashSet<string> namesList = new HashSet<string>();
            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }
            return View(namesList);
        }
        [HttpPost]
        public IActionResult Add(string hashName)
        {
            if (!db.KeyExists(listKey))//listkey var ise
            {
                db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));//5 dakika ömrü olsun.Her .alıştığında 5 dakika sıfırlanır. Yine 5 dakika ömrü olur.
            }
            db.SetAdd(listKey, hashName);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteItem(string name)
        {
            db.SetRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }
    }
}
