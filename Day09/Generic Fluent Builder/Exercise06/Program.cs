// See https://aka.ms/new-console-template for more information
/*
Build:
1. Generic builder base class
2. Builders for different entities
3. Validation in builders
4. Clone/Copy functionality

Example:
ProductBuilder
    .Create()
    .WithName("Laptop")
    .WithPrice(999.99m)
    .InCategory("Electronics")
    .Build();
*/

var product = ProductBuilder
            .Create()
            .WithName("Laptop")
            .WithPrice(34567m)
            .InCategory("Electronics")
            .Build();
System.Console.WriteLine(product.Name);
System.Console.WriteLine(product.Price);
System.Console.WriteLine(product.Category);

var baseBuilder = ProductBuilder
                    .Create()
                    .WithName("Laptop")
                    .WithPrice(1000);

var gamingLaptop = baseBuilder.Clone<ProductBuilder>()
                    .InCategory("Gaming")
                    .Build();

var officeLaptop = baseBuilder.Clone<ProductBuilder>()
                    .InCategory("Office")
                    .Build();
            

public class Product
{
    public string Name{get; set;} = null!;
    public decimal Price{get; set;}

    public string Category{get; set;} = null!;


}
public abstract class Builder<T> where T : new()
{
    protected T instace = new T();
    public T Build()
    {
        Validate();
        return instace;
    }

    protected virtual void Validate()
    {
        
    }

    public TBuilder Clone<TBuilder>() where TBuilder : Builder<T>
{
    return (TBuilder)this.MemberwiseClone();
}
}

public class ProductBuilder : Builder<Product>
{
    public static ProductBuilder Create()
    {
        return new ProductBuilder();
    }

    public ProductBuilder WithName(string name)
    {
        instace.Name = name;
        return this;
    }

    public ProductBuilder WithPrice(decimal price)
    {
        instace.Price = price;
        return this;
    }
    public ProductBuilder InCategory(string category)
    {
        instace.Category = category;
        return this;
    }

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(instace.Name))
        {
            throw new Exception("Product name is required");
        }
        if(instace.Price < 0)
        {
            throw new Exception("Price must be greater than zero");
        }
    }
}

