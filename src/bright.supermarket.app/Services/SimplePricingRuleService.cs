using Bright.Supermarket.App.Domain;
using Bright.Supermarket.App.Domain.Services;

namespace Bright.Supermarket.App.Services;
public class SimplePricingRuleService : IPricingRuleService
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

        WriteLine("Internal Log: Pulling Latest Pricing Rules:");
        DisplayRules(rules);

        return rules;
    }

    private static void DisplayRules(IReadOnlyList<PricingRule> rules)
    {
        foreach (var rule in rules)
        {
            {
                WriteLine("SKU: {0}, UnitPrice: {1}, SpecialPrice: {2}", rule.Sku, rule.UnitPrice, rule.MultiPrice.HasValue ? $"{rule.MultiQuantity} for {rule.MultiPrice}" : "None");
            }
        }

        WriteLine();
    }
}