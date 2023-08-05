using E_Commerce.Areas.Identity.Data;
using E_Commerce.Models;
using AutoMapper;

namespace E_Commerce.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Book, BookModel>().ReverseMap();
            CreateMap<CartItem, CartItemModel>().ReverseMap();
            CreateMap<Bill, BillModel>().ReverseMap();
        }
    }
}
