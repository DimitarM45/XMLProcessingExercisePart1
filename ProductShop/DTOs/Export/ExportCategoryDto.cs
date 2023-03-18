namespace ProductShop.DTOs.Export;

using System.Xml.Serialization;

[XmlType("Category")]

public class ExportCategoryDto
{
    [XmlElement("name")]

    public string? Name { get; set; }

    [XmlElement("count")]

    public int ProductCount { get; set; }

    [XmlElement("averagePrice")]

    public decimal ProductsAveragePrice { get; set; }

    [XmlElement("totalRevenue")]

    public decimal TotalRevenue { get; set; }
}
