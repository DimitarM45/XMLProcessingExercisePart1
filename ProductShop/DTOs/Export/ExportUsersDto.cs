namespace ProductShop.DTOs.Export;

using System.Xml.Serialization;

public class ExportUsersDto
{
    [XmlElement("count")]

    public int UsersCount { get; set; }

    [XmlArray("users")]

    public ExportUserDto[]? Users { get; set; }
}
