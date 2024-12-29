using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestaurantManagementAPI.Models
{
    public class MenuItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Image { get; set; } // رابط الصورة
        public List<string> Ingredients { get; set; } // قائمة المكونات
        public bool Available { get; set; } // حالة التوفر
    }
}