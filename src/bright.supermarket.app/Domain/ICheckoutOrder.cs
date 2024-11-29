namespace Bright.Supermarket.App.Domain;

public interface ICheckoutOrder
{
    IList<LineItem> LineItems { get; }
    bool AddItem(string sku);
    int CalculateOrderTotal();
}