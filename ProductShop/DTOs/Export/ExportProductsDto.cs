namespace ProductShop.DTOs.Export;

using System.Xml.Serialization;

[XmlType("SoldProducts")]

public class ExportProductsDto
{
    [XmlElement("count")]

    public int ProductsCount { get; set; }

    [XmlArray("products")]

    public ExportProductUserDto[]? Products { get; set; }
}
