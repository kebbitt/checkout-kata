namespace Bright.Supermarket.App.Domain;

public interface ICheckoutOrder
{
    int Id {get; }
    IList<LineItem> LineItems { get; }
    bool AddItem(string sku);
    int CalculateOrderTotal();
}