namespace Bright.Supermarket.App.Domain;
public class Checkout(IOrderFactory orderFactory) : ICheckout
{
    private readonly IOrderFactory _orderFactory = orderFactory ?? throw new ArgumentNullException(nameof(orderFactory));

    public CheckoutOrder? CurrentOrder { get; set; }

    public void Scan(string item)
    {
        throw new NotImplementedException();
    }

    public int GetTotalPrice()
    {
        throw new NotImplementedException();
    }
}