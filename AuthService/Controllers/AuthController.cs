using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using UserService.DTO;
using UserService.Grpc;
using UserService.Services;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7184");
            var client = new UserGrpc.UserGrpcClient(channel);
            try
            {
                var reply = await client.CheckLoginAsync(new LoginRequest { Username = request.Username, Password = request.Password });
                var token = _jwtService.GenerateToken(reply.UserId.ToString(), reply.FullName, reply.Role);
                return Ok(new
                {
                    Message = "Login successfully!",
                    Token = token,
                });
            }
            catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                return NotFound("User not found!");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7184");
            var client = new UserGrpc.UserGrpcClient(channel);
            try
            {
                var reply = await client.CheckRegisterAsync(new RegisterRequest
                {
                    Username = request.Username,
                    Password = request.Password,
                    FullName = request.FullName,
                    Email = request.Email
                });
                return Ok(new
                {
                    Message = "User created successfully!",
                });
            }
            catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.AlreadyExists)
            {
                return Conflict("Account already exists!");
            }
        }
    }
}
