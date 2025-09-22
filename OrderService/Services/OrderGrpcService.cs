using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Grpc;

namespace OrderService.Services
{
    public class OrderGrpcService : OrderGrpc.OrderGrpcBase
    {
        private readonly OrderDbContext _context;
        public OrderGrpcService(OrderDbContext context)
        {
            _context = context;
        }

        public override async Task GetOrdersByUserId(UserRequest request, IServerStreamWriter<Order> responseStream, ServerCallContext context)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == request.UserId)
                .ToListAsync();

            foreach (var order in orders)
            {
                var orderMessage = new Order
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    BookId = order.BookId,
                    OrderDate = order.OrderDate.ToString("yyyy-MM-dd"),
                };

                await responseStream.WriteAsync(orderMessage);
                await Task.Delay(500);
            }
        }
    }
}
