namespace Bright.Supermarket.App.Domain;

public class LineItem(string sku)
{
    public string Sku { get; } = sku;
    public int Quantity { get; set; } = 1;
}