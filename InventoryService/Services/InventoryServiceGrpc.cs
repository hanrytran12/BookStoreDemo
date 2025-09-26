using Grpc.Core;
using InventoryService.Data;
using InventoryService.Grpc;

namespace InventoryService.Services
{
    public class InventoryServiceGrpc : InventoryGrpc.InventoryGrpcBase
    {
        private readonly InventoryDbContext _context;

        public InventoryServiceGrpc(InventoryDbContext context)
        {
            _context = context;
        }

        public override Task<InventoryReply> GetInventoryByBookId(InventoryRequest request, ServerCallContext context)
        {
            var inventoryItem = _context.InventoryItems.FirstOrDefault(i => i.BookId == request.BookId);
            if (inventoryItem == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Inventory item not found"));
            }
            return Task.FromResult(new InventoryReply
            {
                BookId = inventoryItem.BookId,
                Quantity = inventoryItem.Quantity
            });
        }

        public override async Task<CreateResponse> CreateInventory(CreateRequest request, ServerCallContext context)
        {
            var newItem = new Models.InventoryItem
            {
                BookId = request.BookId,
                Quantity = 0,
                LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            await _context.InventoryItems.AddAsync(newItem);
            await _context.SaveChangesAsync();

            return await Task.FromResult(new CreateResponse
            {
                Success = true,
            });
        }

        public override async Task<DecreaseReply> DecreaseStock(DecreaseRequest request, ServerCallContext context)
        {
            var inventoryItem = _context.InventoryItems.FirstOrDefault(i => i.BookId == request.BookId);
            inventoryItem.Quantity -= 1;
            await _context.SaveChangesAsync();
            return await Task.FromResult(new DecreaseReply
            {
                Success = true,
            });
        }
    }
}
