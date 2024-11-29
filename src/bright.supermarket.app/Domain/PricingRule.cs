namespace Bright.Supermarket.App.Domain;

public class PricingRule(string sku, int unitPrice, int? multiQuantity = default, int? multiPrice = default)
{
    public string Sku { get; } = sku ?? throw new ArgumentNullException(nameof(sku));

    public int UnitPrice { get; } = unitPrice;

    public int? MultiQuantity { get; } = multiQuantity;

    public int? MultiPrice { get; } = multiPrice;
}