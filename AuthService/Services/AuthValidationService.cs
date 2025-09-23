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

            if (request.FullName.Trim() == null || request.Email.Trim() == null || request.Password.Trim() == null || request.Username.Trim() == null)
            {
                throw new CustomValidationException("All fields are required");
            }
        }

        public void ValidationLogin(LoginDTO request)
        {
            if (request.Password.Trim() == null || request.Username.Trim() == null)
            {
                throw new CustomValidationException("All fields are required");
            }
        }
    }
}
