namespace ProductShop.Models;

using System.Collections.Generic;

public class Category
{
    public Category()
    {
        CategoryProducts = new HashSet<CategoryProduct>();
    }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<CategoryProduct> CategoryProducts { get; set; }
}
