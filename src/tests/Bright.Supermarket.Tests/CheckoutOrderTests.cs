using System.Collections;
using Bright.Supermarket.App.Domain;
using Moq;

namespace Bright.Supermarket.Tests;
public class CheckoutOrderTests
{
    [Fact]
    public void AddItem_WithRecognisedSku_IsAddedToOrder()
    {
        // Arrange
        var order = CreateCheckoutOrder();

        // Act
        var result = order.AddItem("A");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AddItem_WithUnrecognisedSku_IsNotAddedToOrder()
    {
        // Arrange
        var order = CreateCheckoutOrder();
        var expectedMessage = "The Sku was not found.";

        using var sw = new StringWriter();
        Console.SetOut(sw);
        // Act
        var result = order.AddItem("Q");

        // Assert
        Assert.False(result);
        Assert.Equal(expectedMessage, sw.ToString());
    }

    [Fact]
    public void AddItem_WithExistingItemInSameSku_ShouldUpdateLineItemQuantity()
    {
        // Arrange
        var sku = "A";
        var expectedLineItem = new LineItem("A") {Quantity = 2};
        var order = CreateCheckoutOrder();
        order.AddItem(sku);

        // Act
        var result = order.AddItem(sku);

        // Assert
        Assert.Single(order.LineItems);
        Assert.Equal(expectedLineItem, order.LineItems.First());
    }

    [Fact]
    public void AddItem_WithItemInAnotherSku_ShouldAddLineItemToOrder()
    {
        // Arrange
        var sku = "B";
        var order = CreateCheckoutOrder();
        order.AddItem("A");

        // Act
        var result = order.AddItem(sku);

        // Assert
        Assert.True(result);
        Assert.Equal(2, order.LineItems.Count);
        Assert.Contains(order.LineItems, li => li.Sku == "B");
    }

    [Fact]
    public void CalculateOrderTotal_WithIndividuallyPricedItems_CalculatesCorrectPrice()
    {
        // Arrange
        var rules = BuildPricingRules();
        var expectedPrice = rules.Where(r => r.Sku is "C" or "D").Sum(r => r.UnitPrice);
        var order = CreateCheckoutOrder();
        order.AddItem("C");
        order.AddItem("D");

        // Act
        var total = order.CalculateOrderTotal();

        // Assert
        Assert.Equal(expectedPrice, total);
    }

    [Fact]
    public void CalculateOrderTotal_WithMultiBuyOffer_CalculatesCorrectPrice()
    {
        // Arrange
        var sku = "A";
        var rules = BuildPricingRules();
        var expectedPrice = rules.Where(r => r.Sku == sku).Select(r => r.MultiPrice).FirstOrDefault();
 
        var order = CreateCheckoutOrder(1, rules);
        order.AddItem(sku);
        order.AddItem(sku);
        order.AddItem(sku);

        // Act
        var total = order.CalculateOrderTotal();

        // Assert
        Assert.Equal(expectedPrice, total);
    }

    [Fact]
    public void CalculateOrderTotal_WithStackedMultiBuyOffers_CalculatesCorrectPrice()
    {
        // Arrange
        var sku = "A";
        var rules = BuildPricingRules();
        var expectedPrice = rules.Where(r => r.Sku == sku).Select(r => r.MultiPrice * 2).FirstOrDefault();
        var order = CreateCheckoutOrder(1, rules);
        order.AddItem(sku);
        order.AddItem(sku);
        order.AddItem(sku);
        order.AddItem(sku);
        order.AddItem(sku);
        order.AddItem(sku);

        // Act
        var total = order.CalculateOrderTotal();

        // Assert
        Assert.Equal(expectedPrice, total);
    }

    [Fact]
    public void CalculatesOrderTotal_WithMixedCheckoutOrder_CalculatesCorrectPrice()
    {
        // Arrange
        var expectedPrice = 210; // TODO: Use linq to set this in case test pricing rules are adjusted
        var order = CreateCheckoutOrder();
        order.AddItem("A");
        order.AddItem("A");
        order.AddItem("A");
        order.AddItem("B");
        order.AddItem("B");
        order.AddItem("C");
        order.AddItem("D");

        // Act
        var total = order.CalculateOrderTotal();

        // Assert
        Assert.Equal(expectedPrice, total);
    }

    private CheckoutOrder CreateCheckoutOrder(int? orderId = default, IReadOnlyList<PricingRule>? pricingRules = default)
    {
        return new CheckoutOrder(orderId ?? 1, pricingRules ?? BuildPricingRules());
    }

    private IReadOnlyList<PricingRule> BuildPricingRules()
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
