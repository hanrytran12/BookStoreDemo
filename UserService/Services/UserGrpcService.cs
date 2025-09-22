using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Grpc;

namespace UserService.Services
{
    public class UserGrpcService : UserGrpc.UserGrpcBase
    {
        private readonly UserDbContext _context;
        public UserGrpcService(UserDbContext context)
        {
            _context = context;
        }

        public override async Task<LoginReply> CheckLogin(Grpc.LoginRequest request, ServerCallContext context)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            return new LoginReply
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public override async Task<RegisterReply> CheckRegister(Grpc.RegisterRequest request, ServerCallContext context)
        {
            var existingUser = await _context.Users
                .AnyAsync(u => u.Username == request.Username);
            if (existingUser)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, "Account already exists"));
            }
            var user = new Models.User
            {
                Username = request.Username,
                Password = request.Password,
                FullName = request.FullName,
                Email = request.Email,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                Role = "User"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new RegisterReply
            {
                Success = true
            };
        }
    }
}
