namespace ProductShop.DTOs.Export;

using System.Xml.Serialization;

[XmlType("Product")]

public class ExportProductUserDto
{
    [XmlElement("name")]

    public string? Name { get; set; } 

    [XmlElement("price")]

    public decimal Price { get; set; }
}
