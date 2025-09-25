using BookService.Grpc;
using Grpc.Core;
using Grpc.Net.Client;
using InventoryService.Grpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using GrpcStatusCode = Grpc.Core.StatusCode;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;
        public OrdersController(OrderDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return Ok(orders);
        }

        [HttpPost("{bookId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateOrder(int bookId)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new BookGrpc.BookGrpcClient(channel);
            try
            {
                var userId = int.Parse((User.FindFirst("userId")?.Value));
                var reply = await client.GetBookByIdAsync(new BookRequest { Id = bookId });
                await _context.Orders.AddAsync(new Order
                {
                    BookId = reply.Id,
                    OrderDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    UserId = userId
                });
                await _context.SaveChangesAsync();

                using var inventoryChannel = GrpcChannel.ForAddress("https://localhost:7220");
                var inventoryClient = new InventoryGrpc.InventoryGrpcClient(inventoryChannel);
                var inventoryReply = await inventoryClient.DecreaseStockAsync(new DecreaseRequest { BookId = bookId });

                return Ok(new
                {
                    Message = "Order created successfully!",
                    Book = reply
                });
            }
            catch (RpcException ex) when (ex.StatusCode == GrpcStatusCode.NotFound)
            {
                return NotFound(new { Message = $"Book with id {bookId} not found in BookService." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the order.", Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound(new { Message = "Order not found." });
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Order deleted successfully!" });
        }
    }
}
