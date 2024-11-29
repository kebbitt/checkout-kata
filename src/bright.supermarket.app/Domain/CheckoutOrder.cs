namespace Bright.Supermarket.App.Domain;

public class CheckoutOrder
{
    private readonly IReadOnlyList<PricingRule> _pricingRules;
    public CheckoutOrder(int id, IReadOnlyList<PricingRule> pricingRules)
    {
        Id = id;
        _pricingRules = pricingRules;
    }

    public int Id { get; set; }

    public IList<LineItem> LineItems { get; } = new List<LineItem>();

    public virtual bool AddItem(string sku)
    {
        throw new NotImplementedException();
    }

    public virtual int CalculateOrderTotal()
    {
        throw new NotImplementedException();
    }
}