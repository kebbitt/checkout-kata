using Bright.Supermarket.App.Domain;
using Moq;

namespace Bright.Supermarket.Tests;

public class CheckoutTests
{
    private readonly Mock<IOrderFactory> _orderFactoryMock = new();
    private readonly Mock<CheckoutOrder> _checkoutOrderMock = new();

    [Fact]
    public void Scan_WithNoExistingOrder_ShouldAddLineItemToNewOrder()
    {
        // Arrange
        var sku = "A";
        var expectedLineItem = new LineItem("A");
        var checkout = CreateCheckout();

        // Act
        checkout.Scan(sku);

        // Assert
        Assert.NotNull(checkout.CurrentOrder);
        Assert.Single(checkout.CurrentOrder.LineItems);
        Assert.Equal(expectedLineItem, checkout.CurrentOrder.LineItems.First());
    }

    [Fact]
    public void Scan_WithExistingOrder_AndItemInAnotherSku_ShouldAddLineItemToOrder()
    {
        // Arrange
        var sku = "B";
        var existingOrder = new CheckoutOrder { LineItems = new List<LineItem>() { new LineItem("A") } };
        _orderFactoryMock.Setup(of => of.CreateNewOrder()).Returns(existingOrder);
        var checkout = CreateCheckout();

        // Act
        checkout.Scan(sku);

        // Assert
        Assert.Equal(2, checkout.CurrentOrder.LineItems.Count);
        Assert.Contains(checkout.CurrentOrder.LineItems, li => li.Sku == "B");
    }

    [Fact]
    public void Scan_WithExistingOrder_AndItemInSameSku_ShouldUpdateLineItem()
    {
        // Arrange
        var sku = "A";
        var expectedLineItem = new LineItem("A") {Quantity = 2};
        var existingOrder = new CheckoutOrder { LineItems = new List<LineItem>() { new LineItem("A") } };
        _orderFactoryMock.Setup(of => of.CreateNewOrder()).Returns(existingOrder);
        var checkout = CreateCheckout();

        // Act
        checkout.Scan(sku);

        // Assert
        Assert.Single(checkout.CurrentOrder.LineItems);
        Assert.Equal(expectedLineItem, checkout.CurrentOrder.LineItems.First());
    }

    [Fact]
    public void GetTotalPrice_WithCheckoutOrder_ShouldDisplayCurrentOrderTotal()
    {
        // Arrange
        var expectedTotal = 125;
        _checkoutOrderMock.Setup(o => o.CalculateOrderTotal()).Returns(expectedTotal);
        _orderFactoryMock.Setup(of => of.CreateNewOrder()).Returns(_checkoutOrderMock.Object);
        var checkout = CreateCheckout();

        // Act
        var total = checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expectedTotal, total);
    }

    [Fact]
    public void GetTotalPrice_WithoutOrder_ShouldDisplayErrorMessage()
    {
        // Arrange
        var checkout = CreateCheckout();
        var expectedMessage = "No checkout order is in progress.";

        using var sw = new StringWriter();
        Console.SetOut(sw);

        // Act
        var total= checkout.GetTotalPrice();

        // Assert
        var result = sw.ToString();
        Assert.Equal(0, total);
        Assert.Equal(expectedMessage, result);
    }

    private Checkout CreateCheckout()
    {
        return new Checkout(_orderFactoryMock.Object);
    }
}
