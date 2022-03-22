using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProducerAPI.Models;
using System.Threading.Tasks;

namespace ProducerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public OrdersController(IConfiguration config)
        {
            this.configuration = config;
        }


        [HttpPost]
        public async Task<IActionResult> Post(Order order)
        {
            string message = JsonConvert.SerializeObject(order);
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:Server"]
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                var result = await producer.ProduceAsync("test", new Message<Null, string>
                {
                    Value = message
                });
                return await Task.FromResult(Ok("Message Sent"));
            };
        }
    }
}
