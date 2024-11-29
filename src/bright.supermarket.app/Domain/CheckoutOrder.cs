namespace Bright.Supermarket.App.Domain;

public class CheckoutOrder
{
    public int Id { get; set; }

    public IList<LineItem> LineItems { get; set; } = new List<LineItem>();

    public virtual int CalculateOrderTotal()
    {
        throw new NotImplementedException();
    }
}