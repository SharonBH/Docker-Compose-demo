using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Data;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CounterController : ControllerBase
    {
        private readonly ILogger<CounterController> _logger;
        private readonly IDistributedCache _cache;

        public CounterController(ILogger<CounterController> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        [HttpGet(Name = "GetCounter")]
        public string Get()
        {
            string key = "Counter";
            string? result = null;
            bool dbonline = false;
            try
            {
                var counterStr = _cache.GetString(key);
                if (int.TryParse(counterStr, out int counter))
                {
                    counter++;
                }
                else
                {
                    counter = 0;
                }
                result = counter.ToString();
                _cache.SetString(key, result);
                dbonline = isDBOnline();
            }
            catch(RedisConnectionException)
            {
                result = $"Redis cache is not found.";
            }
            return dbonline ? result: "DB offline";
        }

        bool isDBOnline()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=SqlServerDb;Initial Catalog=test_volume1;User ID=sa;Password=awsVamMdK4Q49t8d;TrustServerCertificate=True";
            connection.Open();
            var result = false;
            SqlCommand command = new SqlCommand("select 1", connection);
            result = command.ExecuteScalar().ToString() == "1";
            connection.Close();
            return result;
        }
    }
}