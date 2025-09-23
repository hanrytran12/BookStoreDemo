using BookService.DTO;
using BookService.Helper;

namespace BookService.Services
{
    public class BookValidationService
    {
        public void ValidationCreateBook(AddBookDTO request)
        {
            if(string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Author))
            {
                throw new CustomValidationException("All fields are required");
            }

            if(request.Year < 1000 || request.Year > DateTime.Now.Year)
            {
                throw new CustomValidationException("Invalid year!");
            }
        }
    }
}
