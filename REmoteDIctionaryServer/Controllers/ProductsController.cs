using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using REmoteDIctionaryServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace REmoteDIctionaryServer.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);//ne kadar süre memoryde duracak
                                                                              // _distributedCache.SetString("soyad", "Hasan",cacheEntryOptions);
                                                                              //await _distributedCache.SetStringAsync("soyad2", "TOPRAK", cacheEntryOptions);
            Product product = new Product { Id = 1, Name = "Kalem", Price = 20 };
            string jsonProduct = JsonConvert.SerializeObject(product);
            await _distributedCache.SetStringAsync("product:1", jsonProduct);
            await _distributedCache.SetStringAsync("product:2", jsonProduct,cacheEntryOptions);
            
            return View();
        }
        public async Task<IActionResult> Show()
        {
            string name = _distributedCache.GetString("ad");
            ViewBag.Ad = name;
            ViewBag.Product = JsonConvert.DeserializeObject<Product>(await _distributedCache.GetStringAsync("product:1"));
            return View();
        }
        public IActionResult Remove()
        {
            _distributedCache.Remove("ad");
            return View();
        }
        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/1_1598452306_resim.png");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);//resmi byte dizisine cevirir.
            _distributedCache.Set("image", imageByte);
            return View();
        }
        public IActionResult ImageUrl()
        {
            byte[] resimByte = _distributedCache.Get("image");
            return File(resimByte, "image/png");
        }
    }
}
