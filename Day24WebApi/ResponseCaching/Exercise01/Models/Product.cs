namespace Exercise01.Models
{
    public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; // V2
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty; // V2
    public int Stock { get; set; } // V2
    public List<string> Tags { get; set; } = new(); // V2
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // V2
    public DateTime? UpdatedAt { get; set; } // V2
}
}