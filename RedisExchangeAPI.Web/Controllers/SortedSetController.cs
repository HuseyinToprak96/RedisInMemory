using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "sortedSetNames";
        public SortedSetController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDB(3);
        }
        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();
            if (db.KeyExists(listKey))
            {
                //db.SortedSetScan(listKey).ToList().ForEach(x =>
                //{
                //    list.Add(x.ToString());
                //});
                //BÜYÜKTEN KÜÇÜĞE SIRALAMA
                db.SortedSetRangeByRank(listKey, order: Order.Descending).ToList().ForEach(x=> {
                    list.Add(x.ToString());
                });
                //db.SortedSetRangeByRank(listKey,0,5, order: Order.Descending).ToList().ForEach(x => {
                //    list.Add(x.ToString()); 0. değerden 5 . değere kadar büyükten küçüğe sıralar.
                //});
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name,int score)
        {
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
            db.SortedSetAdd(listKey, name, score);//aynı isimde veri eklenmeye çalışılır ise eklemez score kısmını günceller...
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Remove(string item)
        {
            db.SortedSetRemove(listKey, item);
            return RedirectToAction("Index");
        }
    }
}
