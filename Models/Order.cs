using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace RestaurantManagementAPI.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string OrderId { get; set; } // رقم الطلب
        public string OrderType { get; set; } // نوع الطلب (داخلي/خارجي)
        public string Status { get; set; } // حالة الطلب
        public List<OrderItem> Items { get; set; } // قائمة الطلبات
        public decimal TotalPrice { get; set; } // السعر الإجمالي
        public int? TableNumber { get; set; } // رقم الطاولة (اختياري)
        public string CustomerName { get; set; } // اسم الزبون
        public string CustomerPhone { get; set; } // رقم هاتف الزبون
        public DateTime OrderTime { get; set; } // وقت الطلب
        public DeliveryAddress DeliveryAddress { get; set; } // عنوان التوصيل
    }

    public class OrderItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string MenuItemId { get; set; } // مرجع إلى الوجبة
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class DeliveryAddress
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }
}