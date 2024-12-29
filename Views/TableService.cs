using MongoDB.Driver;
using RestaurantManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementAPI.Services
{
    public class TableService
    {
        private readonly IMongoCollection<Table> _tablesCollection;

        public TableService(IMongoDatabase database)
        {
            _tablesCollection = database.GetCollection<Table>("tables"); // استخدام اسم المجموعة من الإعدادات أفضل
        }

        public async Task<List<Table>> GetAsync() =>
            await _tablesCollection.Find(_ => true).ToListAsync();

        public async Task<Table> GetAsync(string id) =>
            await _tablesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Table> GetByTableNumberAsync(int tableNumber) =>
            await _tablesCollection.Find(x => x.TableNumber == tableNumber).FirstOrDefaultAsync();

        public async Task CreateAsync(Table newTable) =>
            await _tablesCollection.InsertOneAsync(newTable);

        public async Task UpdateAsync(string id, Table updatedTable) =>
            await _tablesCollection.ReplaceOneAsync(x => x.Id == id, updatedTable);

        public async Task RemoveAsync(string id) =>
            await _tablesCollection.DeleteOneAsync(x => x.Id == id);
    }
}