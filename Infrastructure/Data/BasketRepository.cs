using System.Text.Json;
using Core.Entites;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database=redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data=await _database.StringGetAsync(basketId);
            return data.IsNullOrEmpty?null:JsonSerializer.Deserialize<CustomerBasket>(data); //this line is like if we have data do deserialization and store it in customer basket cause above line will get json input from client 
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created=await _database.StringSetAsync(basket.Id,
            JsonSerializer.Serialize(basket),TimeSpan.FromDays(30));
            if(!created)return null;
            return await GetBasketAsync(basket.Id);

        }
    }
}