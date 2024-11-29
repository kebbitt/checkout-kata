using Bright.Supermarket.App.Domain;
using Moq;

namespace Bright.Supermarket.Tests;

public class CheckoutTests
{
    private readonly Mock<IOrderFactory> _orderFactoryMock = new();

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
        var existingOrder = new CheckoutOrder { LineItems = new List<LineItem>() { new LineItem("A") } };
        _orderFactoryMock.Setup(of => of.CreateNewOrder()).Returns(existingOrder);
        var checkout = CreateCheckout();

        // Act
        checkout.Scan(sku);

        // Assert
        Assert.NotNull(checkout.CurrentOrder);
    }

    private Checkout CreateCheckout()
    {
        return new Checkout(_orderFactoryMock.Object);
    }
}
