using Bright.Supermarket.App.Domain.Services;

namespace Bright.Supermarket.App.Domain;
internal class CheckoutOrderFactory(IPricingRuleService pricingService) : ICheckoutOrderFactory
{
    private static int _orderNumberCounter = 0;

    public ICheckoutOrder CreateNewOrder()
    {
        return new CheckoutOrder(_orderNumberCounter++, pricingService.GetLatestPricingRules());
    }
}
