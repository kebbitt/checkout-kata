using Bright.Supermarket.App.Domain;
using Bright.Supermarket.App.Services;

// Normally DI would be used for handling all dependencies but just building quickly :)
var pricingRuleService = new SimplePricingRuleService();

CheckoutOrderFactory orderFactory = new CheckoutOrderFactory(pricingRuleService);

WriteLine("Starting Checkout...");
WriteLine();
var supermarketCheckout = new Checkout(orderFactory);
WriteLine("Welcome to Bright Supermarket!");
WriteLine();

ConsoleKey key = ConsoleKey.A;

WriteLine("Please enter a SKU letter to scan, press ENTER to complete your purchases, or ESC to exit.");
while (key is not ConsoleKey.Escape)
{
    key = ReadKey(intercept: true).Key;
    var order = supermarketCheckout.CurrentOrder;
    if (key == ConsoleKey.Enter)
    {
        var total = supermarketCheckout.GetTotalPrice();
        WriteLine("The total for transaction {0} is £{1}", order?.Id, total);
        WriteLine();
        if (order != null)
        {
            WriteLine("Here is your itemised receipt");
            WriteLine("------------------------------");
            foreach (var orderItem in order.LineItems)
            {
                WriteLine("SKU: {0} x{1}     £{2}", orderItem.Sku, orderItem.Quantity, orderItem.CalculateLineItemTotal());
            }
            WriteLine("------------------------------");
            WriteLine("TOTAL £{0}", total);
            WriteLine();
            WriteLine("Please enter a SKU letter to scan, press ENTER to complete your purchases, or ESC to exit.");
        }
    }
    else {
        supermarketCheckout.Scan(key.ToString());
        WriteLine("You scanned {0}", key.ToString());
        WriteLine();
    }
}


