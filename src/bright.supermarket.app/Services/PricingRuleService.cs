using Bright.Supermarket.App.Domain;
using Bright.Supermarket.App.Domain.Services;

namespace Bright.Supermarket.App.Services;
public class PricingRuleService : IPricingRuleService
{
    public IReadOnlyList<PricingRule> GetLatestPricingRules()
    {
        IReadOnlyList<PricingRule> rules = new List<PricingRule>()
        {
            new("A", 50, 3, 130),
            new PricingRule("B", 30, 2, 45),
            new PricingRule("C", 20),
            new PricingRule("D", 15)
        };

        return rules;
    }
}
