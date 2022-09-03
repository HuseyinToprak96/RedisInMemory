using MemoryWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            //AbsoluteExpiration kullanılır ise cache ömrü verilen zaman kadar olur.
            //SlidingExpiration kullanılır ise verilen süre içerisinde istekte bulunulursa zaman tekrar başa alınır aynı süre eklenmiş olur. Bunu kullanırken AbsoluteExpirationda kullanılmalıdır. Memorydeki cache in güncel kalması için. 
            //1.Yol
           // if (string.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))//var mı yok mu kontrol ederiz.
            //{
              //  _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
           //}
            //2.yol
          //  if (_memoryCache.TryGetValue("zaman",out string zamanCache))//varmı kontrol et varsa zamanCache zaman değerini ata. yoksa içindeki işlemi yap
            //{
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                // options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);//10saniye ömrü olsun
                options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                options.SlidingExpiration = TimeSpan.FromSeconds(10);//10 saniye içinde çağırılmazsa sil.
                options.Priority = CacheItemPriority.High; //High:Bu data önemli silme Low:Data önemsiz sil NeverRemove:Asla silme 
                options.RegisterPostEvictionCallback((key,value,reason,state)=> {
                    _memoryCache.Set("callback", $"{key}=>{value}=>sebep:{reason}");
                }); 
               // _memoryCache.Set<string>("zaman", DateTime.Now.ToString(),options);
                // _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
                //cache priority //cache dolduğunda silinecek olan keyleri belirlemek için kullanılır.
                //RegisterPostEvictionCallback //memoryden silinen metod neden silindi?
                product Product = new product { Id = 1, Name = "Kalem", Price = 200 };
                _memoryCache.Set<product>("product:1", Product);
            //}

            return View();
        }
        public IActionResult Show()
        {
            //_memoryCache.Remove("zaman"); der isek zaman a atanmış value değeri silinir.
            //_memoryCache.GetOrCreate<string>("zaman",entry=> {
              //  return DateTime.Now.ToString(); //bu kullanım eğer key'i zaman olan value değeri cache içinde varsa onu al yoksa oluştur anlamındadır. 
            //});
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.callback = callback;
            ViewBag.zaman = _memoryCache.Get<string>("zaman");
            ViewBag.product = _memoryCache.Get<product>("product:1");
            return View();
        }
    }
}
