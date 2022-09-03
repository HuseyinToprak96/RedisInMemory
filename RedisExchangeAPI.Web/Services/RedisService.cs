﻿using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        //private readonly string _redisHost;
        //private readonly string _redisPort;
        private ConnectionMultiplexer _redis;
        public IDatabase db { get; set; }
        public RedisService(IConfiguration configuration)
        {
         //   _redisHost = configuration["Redis:Host"];
           // _redisPort = configuration["Redis:Port"];
        }
        public void Connect()//redis server ile haberleşmek için kullanılır.
        {
         //   var configString = $"{_redisHost}:{_redisPort}";
            _redis = ConnectionMultiplexer.Connect("localhost:6379");
        }
        public IDatabase GetDB(int db)//redis server ımızda bulunan hangi db?
        {
            return _redis.GetDatabase(db);//uygulama ayağa kalktığında çağırılacak..
        }
    }
}
