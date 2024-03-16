using AutoMapper;
using HomeWork2.Models;
using HomeWork2.Models.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HomeWork2.Repo
{
    public class MappingProFile : Profile
    {
        public MappingProFile()
        {
            CreateMap<Product, ProductDto>(MemberList.Destination).ReverseMap();
            CreateMap<Category, GroupDto>(MemberList.Destination).ReverseMap();
            CreateMap<Storage, StoreDto>(MemberList.Destination).ReverseMap();
        }
    }
}