namespace Bright.Supermarket.App.Domain;

public class LineItem(string sku, IReadOnlyList<PricingRule>? lineItemPricingRules = default)
{
    private readonly IReadOnlyList<PricingRule> _lineItemPricingRules = lineItemPricingRules ?? throw new ArgumentNullException(nameof(lineItemPricingRules));

    public string Sku { get; } = sku;
    public int Quantity { get; set; } = 1;

    public int CalculateLineItemTotal()
    {
        throw new NotImplementedException();
    }
}