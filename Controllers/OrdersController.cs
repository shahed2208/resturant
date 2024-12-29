using Microsoft.AspNetCore.Mvc;
using RestaurantManagementAPI.Models;
using RestaurantManagementAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly MenuService _menuService; // نحتاج MenuService للتحقق من وجود الوجبات

        public OrdersController(OrderService orderService, MenuService menuService)
        {
            _orderService = orderService;
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<List<Order>> Get() =>
            await _orderService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Order>> Get(string id)
        {
            var order = await _orderService.GetAsync(id);

            if (order is null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Order newOrder)
        {
            // التحقق من وجود الوجبات المطلوبة في قائمة الطعام
            foreach (var item in newOrder.Items)
            {
                var menuItem = await _menuService.GetAsync(item.MenuItemId);
                if (menuItem == null)
                {
                    return BadRequest($"الوجبة بالمعرف {item.MenuItemId} غير موجودة.");
                }
                item.Name = menuItem.Name; // إضافة اسم الوجبة من قائمة الطعام
                item.Price = menuItem.Price; // إضافة سعر الوجبة من قائمة الطعام
            }

            newOrder.OrderTime = DateTime.UtcNow; // تسجيل وقت الطلب بتوقيت UTC
            newOrder.OrderId = GenerateOrderId(); // توليد رقم طلب فريد
            newOrder.Status = "جديد"; // تعيين حالة الطلب الأولية

            await _orderService.CreateAsync(newOrder);

            return CreatedAtAction(nameof(Get), new { id = newOrder.Id }, newOrder);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, Order updatedOrder)
        {
            var order = await _orderService.GetAsync(id);

            if (order is null)
            {
                return NotFound();
            }

            updatedOrder.Id = order.Id;

            await _orderService.UpdateAsync(id, updatedOrder);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var order = await _orderService.GetAsync(id);

            if (order is null)
            {
                return NotFound();
            }

            await _orderService.RemoveAsync(id);

            return NoContent();
        }

        // دالة لتوليد رقم طلب فريد
        private string GenerateOrderId()
        {
            // يمكنك استخدام طرق أخرى لتوليد أرقام فريدة، مثل GUID أو عداد متسلسل في قاعدة البيانات.
            return $"ORD-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}";
        }
    }
}