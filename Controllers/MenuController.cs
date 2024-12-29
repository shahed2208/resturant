using Microsoft.AspNetCore.Mvc;
using RestaurantManagementAPI.Models;
using RestaurantManagementAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly MenuService _menuService;

        public MenuController(MenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<List<MenuItem>> Get() =>
            await _menuService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<MenuItem>> Get(string id)
        {
            var menuItem = await _menuService.GetAsync(id);

            if (menuItem is null)
            {
                return NotFound();
            }

            return menuItem;
        }

        [HttpPost]
        public async Task<IActionResult> Post(MenuItem newMenuItem)
        {
            await _menuService.CreateAsync(newMenuItem);

            return CreatedAtAction(nameof(Get), new { id = newMenuItem.Id }, newMenuItem);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, MenuItem updatedMenuItem)
        {
            var menuItem = await _menuService.GetAsync(id);

            if (menuItem is null)
            {
                return NotFound();
            }

            updatedMenuItem.Id = menuItem.Id;

            await _menuService.UpdateAsync(id, updatedMenuItem);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var menuItem = await _menuService.GetAsync(id);

            if (menuItem is null)
            {
                return NotFound();
            }

            await _menuService.RemoveAsync(id);

            return NoContent();
        }
    }
}