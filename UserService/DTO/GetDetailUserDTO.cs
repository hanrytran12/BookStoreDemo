namespace UserService.DTO
{
    public class GetDetailUserDTO
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateOnly CreatedAt { get; set; }
        public string Role { get; set; }
    }
}
