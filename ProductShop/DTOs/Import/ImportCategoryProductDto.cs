namespace ProductShop.DTOs.Import;

using System.Xml.Serialization;

[XmlType("CategoryProduct")]

public class ImportCategoryProductDto
{
    public int CategoryId { get; set; }

    public int ProductId { get; set; }
}
