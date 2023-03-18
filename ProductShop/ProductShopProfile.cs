namespace ProductShop;

using Models;
using DTOs.Import;
using DTOs.Export;

using AutoMapper;

public class ProductShopProfile : Profile
{
    public ProductShopProfile()
    {
        CreateMap<ImportUserDto, User>();

        CreateMap<ImportProductDto, Product>();

        CreateMap<ImportCategoryDto, Category>();

        CreateMap<ImportCategoryProductDto, CategoryProduct>();

        CreateMap<Product, ExportProductUserDto>();

        CreateMap<User, ExportUserProductsDto>()
            .ForMember(udto => udto.SoldProducts,
                otp => otp.MapFrom(src => src.ProductsSold.Select(ps => new ExportProductUserDto()
                {
                    Name = ps.Name,
                    Price = ps.Price
                })));

        CreateMap<Category, ExportCategoryDto>()
            .ForMember(cdto => cdto.ProductCount,
                otp => otp.MapFrom(src => src.CategoryProducts.Count))
            .ForMember(cdto => cdto.ProductsAveragePrice,
                otp => otp.MapFrom(src => src.CategoryProducts.Average(cp => cp.Product.Price)))
            .ForMember(cdto => cdto.TotalRevenue,
                otp => otp.MapFrom(src => src.CategoryProducts.Sum(cp => cp.Product.Price)));
    }
}
