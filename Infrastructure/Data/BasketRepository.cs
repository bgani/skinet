using System;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data
{
    // we are not storing anything in an actual database
    // it's just a place where customers can leave their baskets behind in our memory
    // so if they will come back they can pick up where they left off
    // we gonna store basket id in a client side, and we will use that to get whatever basket left in a memory
    // basket's lifecycle set for a month, if user don't come back for a basket it will be destroyed
    // Since Redis stores data only in a memory, will we lose the baskets if it restarts?  
    // Redis takes a database snapshot every minute, so if it restarts it's gonna load the shapshot
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);
            return data.IsNullOrEmpty 
                ? null 
                : JsonSerializer
                    .Deserialize<CustomerBasket>(data);
        }

        
        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(
                basket.Id, 
                JsonSerializer.Serialize(basket),
                TimeSpan.FromDays(30));
            if(!created) return null;

            return await GetBasketAsync(basket.Id);
        }
    }
}