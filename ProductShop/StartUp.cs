namespace ProductShop;

using Data;
using Models;
using Utilities;
using DTOs.Import;
using DTOs.Export;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using System.Xml.Linq;

public class StartUp
{
    public static void Main()
    {
        using ProductShopContext context = new ProductShopContext();
    }

    //Problem 1

    public static string ImportUsers(ProductShopContext context, string inputXml)
    {
        XmlHelper serializer = new XmlHelper();

        ImportUserDto[] userDtos = serializer.Deserialize<ImportUserDto[]>(inputXml, "Users");

        IMapper mapper = CreateMapper();

        User[] users = mapper.Map<User[]>(userDtos);

        context.Users?.AddRange(users);
        context.SaveChanges();

        return $"Successfully imported {users.Length}";
    }

    //Problem 2

    public static string ImportProducts(ProductShopContext context, string inputXml)
    {
        XmlHelper serializer = new XmlHelper();

        ImportProductDto[] productDtos = serializer.Deserialize<ImportProductDto[]>(inputXml, "Products");

        IMapper mapper = CreateMapper();

        Product[] products = mapper.Map<Product[]>(productDtos);

        context.Products?.AddRange(products);
        context.SaveChanges();

        return $"Successfully imported {products.Length}";
    }

    //Problem 3

    public static string ImportCategories(ProductShopContext context, string inputXml)
    {
        XmlHelper serializer = new XmlHelper();

        ImportCategoryDto[] categoryDtos = serializer.Deserialize<ImportCategoryDto[]>(inputXml, "Categories");

        IMapper mapper = CreateMapper();

        Category[] categories = mapper.Map<Category[]>(categoryDtos);

        context.Categories?.AddRange(categories);
        context.SaveChanges();

        return $"Successfully imported {categories.Length}";
    }

    //Problem 4

    public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
    {
        XmlHelper serializer = new XmlHelper();

        ImportCategoryProductDto[] categoryProductDtos = serializer.Deserialize<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

        IMapper mapper = CreateMapper();

        CategoryProduct[] categoryProducts = mapper.Map<CategoryProduct[]>(categoryProductDtos);

        context.CategoryProducts?.AddRange(categoryProducts);
        context.SaveChanges();

        return $"Successfully imported {categoryProducts.Length}";
    }

    //Problem 5 (Solution was implemented using XDocument class for practice purposes)

    public static string GetProductsInRange(ProductShopContext context)
    {
        var productsInRange = context.Products?
            .AsNoTracking()
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .Take(10)
            .Select(p => new
            {
                p.Name,
                p.Price,
                Buyer = $"{p.Buyer!.FirstName} {p.Buyer.LastName}"
            })
            .ToArray();

        XDocument productsXml = new XDocument();

        XElement root = new XElement("Products");

        foreach (var product in productsInRange!)
        {
            XElement productElement = new XElement("Product");

            productElement.Add(
                new XElement("name", product.Name),
                new XElement("price", product.Price),
                new XElement("buyer", product.Buyer));

            root.Add(productElement);
        }

        productsXml.Add(root);

        using (TextWriter writer = new StringWriter())
        {
            productsXml.Save(writer);

            return writer.ToString()!;
        }
    }

    //Problem 6

    public static string GetSoldProducts(ProductShopContext context)
    {
        IMapper mapper = CreateMapper();

        var usersProducts = context.Users?
            .AsNoTracking()
            .Include(u => u.ProductsSold)
            .Where(u => u.ProductsSold.Count >= 1)
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Take(5)
            .ProjectTo<ExportUserProductsDto>(mapper.ConfigurationProvider)
            .ToArray();

        XmlHelper serializer = new XmlHelper();

        return serializer.Serialize(usersProducts!, "Users");
    }

    //Problem 7

    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        IMapper mapper = CreateMapper();

        var categories = context.Categories?
            .AsNoTracking()
            .ProjectTo<ExportCategoryDto>(mapper.ConfigurationProvider)
            .OrderByDescending(c => c.ProductCount)
            .ThenBy(c => c.TotalRevenue)
            .ToArray();

        XmlHelper serializer = new XmlHelper();

        string categoriesXml = serializer.Serialize(categories!, "Categories");

        return categoriesXml;
    }

    //Problem 8

    public static string GetUsersWithProducts(ProductShopContext context)
    {
        var usersWithProducts = context.Users?
            .AsNoTracking()
            .Include(u => u.ProductsSold)
            .Where(u => u.ProductsSold.Count >= 1)
            .OrderByDescending(u => u.ProductsSold.Count)
            .ToArray();

        List<ExportUserDto> userDtos = new List<ExportUserDto>();

        IMapper mapper = CreateMapper();

        foreach (var user in usersWithProducts!)
        {
            userDtos.Add(new ExportUserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                SoldProducts = new ExportProductsDto()
                {
                    ProductsCount = user.ProductsSold.Count,
                    Products = mapper
                        .Map<ExportProductUserDto[]>(user.ProductsSold)
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                }
            });
        }

        ExportUsersDto usersDto = new ExportUsersDto()
        {
            UsersCount = userDtos.Count,
            Users = userDtos
                .Take(10)
                .ToArray()
        };

        XmlHelper serializer = new XmlHelper();

        string usersProductsXml = serializer.Serialize(usersDto, "Users");

        return usersProductsXml;
    }

    private static IMapper CreateMapper()
    {
        IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));

        return mapper;
    }
}