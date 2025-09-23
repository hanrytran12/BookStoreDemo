using AuthService.Helper;
using UserService.DTO;

namespace AuthService.Services
{
    public class AuthValidationService
    {
        public void ValidationRegister(RegisterDTO request)
        {
            if(request.Password.Length < 8)
            {
                throw new CustomValidationException("Password must be at least 8 characters long");
            }

            if (!request.Email.Contains("@"))
            {
                throw new CustomValidationException("Invalid email format");
            }

            if (string.IsNullOrEmpty(request.FullName) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Username))
            {
                throw new CustomValidationException("All fields are required");
            }
        }

        public void ValidationLogin(LoginDTO request)
        {
            if (string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Username))
            {
                throw new CustomValidationException("All fields are required");
            }
        }
    }
}
