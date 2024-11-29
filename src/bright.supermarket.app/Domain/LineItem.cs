namespace Bright.Supermarket.App.Domain;

public class LineItem(string sku, PricingRule lineItemPricingRule)
{
    private readonly PricingRule _lineItemPricingRule = lineItemPricingRule ?? throw new ArgumentNullException(nameof(lineItemPricingRule));

    public string Sku { get; } = sku;
    public int Quantity { get; set; } = 1;

    public int CalculateLineItemTotal()
    {
        var lineItemTotal = 0;
        var quantityLeftToPrice = Quantity;
        while(quantityLeftToPrice > 0)
        {
            if (lineItemPricingRule is { MultiPrice: not null, MultiQuantity: not null }
                && quantityLeftToPrice >= lineItemPricingRule.MultiQuantity.Value)
            {
                lineItemTotal += lineItemPricingRule.MultiPrice.Value;
                quantityLeftToPrice-= lineItemPricingRule.MultiQuantity.Value;
            }
            else
            {
                lineItemTotal += lineItemPricingRule.UnitPrice;
                quantityLeftToPrice--;
            }
        }
        return lineItemTotal;
    }
}