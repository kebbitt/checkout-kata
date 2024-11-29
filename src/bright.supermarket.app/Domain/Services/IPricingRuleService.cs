namespace Bright.Supermarket.App.Domain.Services;

public interface IPricingRuleService
{
    public IReadOnlyList<PricingRule> GetLatestPricingRules();
}