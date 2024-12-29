using Microsoft.AspNetCore.Mvc;
using RestaurantManagementAPI.Models;
using RestaurantManagementAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TablesController : ControllerBase
    {
        private readonly TableService _tableService;

        public TablesController(TableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet]
        public async Task<List<Table>> Get() =>
            await _tableService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Table>> Get(string id)
        {
            var table = await _tableService.GetAsync(id);

            if (table is null)
            {
                return NotFound();
            }

            return table;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Table newTable)
        {
            // التحقق من عدم وجود طاولة بنفس الرقم
            var existingTable = await _tableService.GetByTableNumberAsync(newTable.TableNumber);
            if (existingTable != null)
            {
                return Conflict($"يوجد بالفعل طاولة برقم {newTable.TableNumber}.");
            }

            await _tableService.CreateAsync(newTable);

            return CreatedAtAction(nameof(Get), new { id = newTable.Id }, newTable);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, Table updatedTable)
        {
            var table = await _tableService.GetAsync(id);

            if (table is null)
            {
                return NotFound();
            }

            updatedTable.Id = table.Id; // مهم جداً: تعيين الـ ID بشكل صحيح

            // التحقق من عدم وجود طاولة أخرى بنفس الرقم (باستثناء الطاولة الحالية)
            var existingTableWithSameNumber = await _tableService.GetByTableNumberAsync(updatedTable.TableNumber);
            if (existingTableWithSameNumber != null && existingTableWithSameNumber.Id != id)
            {
                return Conflict($"يوجد بالفعل طاولة أخرى برقم {updatedTable.TableNumber}.");
            }

            await _tableService.UpdateAsync(id, updatedTable);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var table = await _tableService.GetAsync(id);

            if (table is null)
            {
                return NotFound();
            }

            await _tableService.RemoveAsync(id);

            return NoContent();
        }
    }
}