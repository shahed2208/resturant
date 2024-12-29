using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestaurantManagementAPI.Models
{
    public class Table
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; } // فارغة، مشغولة، محجوزة
    }
}