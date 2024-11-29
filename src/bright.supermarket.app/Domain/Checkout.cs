namespace Bright.Supermarket.App.Domain;
public class Checkout(IOrderFactory orderFactory) : ICheckout
{
    private readonly IOrderFactory _orderFactory = orderFactory ?? throw new ArgumentNullException(nameof(orderFactory));

    public ICheckoutOrder? CurrentOrder { get; private set; }

    public void Scan(string item)
    {
        try
        {
            // Simply create the order when 1st scan without an order occurs.
            CurrentOrder ??= _orderFactory.CreateNewOrder();

            if (!CurrentOrder.AddItem(item))
            {
                WriteLine("The Sku was not found.");
            }
        }
        catch (Exception)
        {
            WriteLine("Apologies, items cannot be scanned at this time.");
        }
    }

    public int GetTotalPrice()
    {
        int orderTotal = 0;
        if (CurrentOrder != null)
        {
            orderTotal = CurrentOrder.CalculateOrderTotal();
        }
        else {
            WriteLine("No checkout order is in progress.");
        }

        // This breaks PoLA (principle of least astonishment but as README mentions, we assume that order is discarded after price display.
        // In reality this should be in separate method etc
        // TODO - write an additional test to ensure this keeps working.
        CurrentOrder = null;

        return orderTotal;
    }
}