namespace AuthService.Helper
{
    public class CustomValidationException : Exception
    {
        public CustomValidationException(string message) : base(message) { }
    }
}
