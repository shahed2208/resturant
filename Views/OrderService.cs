using MongoDB.Driver;
using RestaurantManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementAPI.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _ordersCollection;

        public OrderService(IMongoDatabase database)
        {
            _ordersCollection = database.GetCollection<Order>("orders"); // استخدام اسم المجموعة من الإعدادات أفضل
        }

        public async Task<List<Order>> GetAsync() =>
            await _ordersCollection.Find(_ => true).ToListAsync();

        public async Task<Order> GetAsync(string id) =>
            await _ordersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Order newOrder) =>
            await _ordersCollection.InsertOneAsync(newOrder);

        public async Task UpdateAsync(string id, Order updatedOrder) =>
            await _ordersCollection.ReplaceOneAsync(x => x.Id == id, updatedOrder);

        public async Task RemoveAsync(string id) =>
            await _ordersCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<Order>> GetOrdersByStatusAsync(string status) =>
          await _ordersCollection.Find(order => order.Status == status).ToListAsync();
    }
}