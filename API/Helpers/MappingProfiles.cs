using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // in ForMember method the "destination" parameter is ProductToReturnDto
            // "memberOptions" is an expression, and in MapFrom method we pass the source, 
            // where do we want to get the property from that wa want to insert into our ProductBrand field
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(
                    d => d.ProductBrand, 
                    o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(
                    d => d.ProductType, 
                    o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            // ReverseMap maps the other way additonal to default mapping
            CreateMap<Address, AddressDto>().ReverseMap();
        } 
    }
}