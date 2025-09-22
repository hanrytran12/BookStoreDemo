using BookService.Data;
using BookService.Grpc;
using Grpc.Core;

namespace BookService.Services
{
    public class BookGrpcService : BookGrpc.BookGrpcBase
    {
        private readonly BookDbContext _context;
        public BookGrpcService(BookDbContext context)
        {
            _context = context;
        }

        public override Task<BookReply> GetBookById(BookRequest request, ServerCallContext context)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == request.Id);
            if (book == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));
            }

            return Task.FromResult(new BookReply
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Year = book.Year
            });
        }
    }
}
