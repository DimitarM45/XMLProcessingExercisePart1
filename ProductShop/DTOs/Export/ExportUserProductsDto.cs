namespace ProductShop.DTOs.Export;

using System.Xml.Serialization;

[XmlType("User")]

public class ExportUserProductsDto
{
    [XmlElement("firstName")]

    public string? FirstName { get; set; }

    [XmlElement("lastName")]

    public string? LastName { get; set; }

    [XmlArray("soldProducts")]

    public ExportProductUserDto[]? SoldProducts { get; set; }
}
