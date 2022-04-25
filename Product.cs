namespace ApiNet6;

public class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }
    public string? Category { get; set; }
    public string? Brief { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public string? MainImage { get; set; }

    // public List<string>? Images { get; set; }
    public string? Images { get; set; }
}
