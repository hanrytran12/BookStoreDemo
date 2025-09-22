using InventoryService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        public InventoryController(InventoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventoryItems()
        {
            var items = await _context.InventoryItems.ToListAsync();
            return Ok(items);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInventoryItem(int bookId, [FromBody] int quantity)
        {
            var item = await _context.InventoryItems.FirstOrDefaultAsync(i => i.BookId == bookId);
            if (item == null)
            {
                return NotFound();
            }

            item.Quantity = quantity;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Updated Completely!" });
        }
    }
}
