using System.Linq;

namespace Bright.Supermarket.App.Domain;

public class CheckoutOrder : ICheckoutOrder
{
    private readonly IReadOnlyList<PricingRule> _pricingRules;
    public CheckoutOrder(int id, IReadOnlyList<PricingRule> pricingRules)
    {
        Id = id;
        _pricingRules = pricingRules;
    }

    public int Id { get; set; }

    public IList<LineItem> LineItems { get; private set; } = new List<LineItem>();

    public virtual bool AddItem(string sku)
    {
        var skuPricingRules = _pricingRules.GroupBy(x => x.Sku).Where(g => g.Key == sku);

        if (!skuPricingRules.Any())
        {
            WriteLine("The Sku was not found.");
            return false;
        }

        var existingLineItem = LineItems.FirstOrDefault(li => li.Sku == sku);

        if(existingLineItem != null) {
            existingLineItem.Quantity++;
        }
        else
        {
            LineItems.Add(new LineItem(sku, skuPricingRules.SelectMany(g => g).ToList().AsReadOnly()));
        }

        return true;
    }

    public virtual int CalculateOrderTotal()
    {
        int orderTotal = 0;
        foreach (var item in LineItems) {
            orderTotal += item.CalculateLineItemTotal();
        }

        return orderTotal;
    }
}