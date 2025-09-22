using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly IMapper _mapper;
        public UserController(UserDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var usersDto = _mapper.Map<List<DTO.GetUsersDTO>>(users);
            return Ok(usersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }
            var userDto = _mapper.Map<DTO.GetDetailUserDTO>(user);
            return Ok(userDto);
        }

        //[HttpGet("orders")]
        //public async Task<IActionResult> GetOrdersByUserId()
        //{
        //    using var channel = GrpcChannel.ForAddress("http://localhost:6000");
        //    //, new GrpcChannelOptions
        //    //{
        //    //    HttpHandler = new HttpClientHandler
        //    //    {
        //    //        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        //    //    }
        //    //});
        //    var client = new OrderGrpc.OrderGrpcClient(channel);
        //    var userId = int.Parse(User.FindFirst("userId")?.Value);
        //    var steam = client.GetOrdersByUserId(new UserRequest { UserId = userId });
        //    var orders = new List<Order>();
        //    while (await steam.ResponseStream.MoveNext(CancellationToken.None))
        //    {
        //        orders.Add(steam.ResponseStream.Current);
        //    }

        //    if (orders.Count == 0)
        //    {
        //        return NotFound(new { Message = "No orders found for this user." });
        //    }

        //    return Ok(orders);
        //}

        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "User deleted successfully." });
        }
    }
}
