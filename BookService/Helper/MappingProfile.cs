using AutoMapper;

namespace BookService.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DTO.AddBookDTO, Models.Book>();
            CreateMap<Models.Book, DTO.GetBooksDTO>();
            CreateMap<Models.Book, DTO.GetDetailBookDTO>();
        }
    }
}
