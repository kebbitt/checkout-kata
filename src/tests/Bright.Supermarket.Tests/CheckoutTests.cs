using System.IO;
using Bright.Supermarket.App.Domain;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using Xunit.Abstractions;

namespace Bright.Supermarket.Tests;

public class CheckoutTests(ITestOutputHelper console)
{
    private readonly ITestOutputHelper _console = console;
    private readonly Mock<ICheckoutOrderFactory> _orderFactoryMock = new();
    private readonly Mock<ICheckoutOrder> _checkoutOrderMock = new();

    [Fact]
    public void Scan_WithNoExistingOrder_ShouldCreateAnOrder()
    {
        // Arrange
        var sku = "A";
        var existingOrder = _checkoutOrderMock.Object;
        _orderFactoryMock.Setup(of => of.CreateNewOrder()).Returns(existingOrder);
        var checkout = CreateCheckout();

        // Act
        checkout.Scan(sku);

        // Assert
        Assert.NotNull(checkout.CurrentOrder);
    }

    [Fact]
    public void Scan_WithExistingOrder_AndAddItemFails_ShouldDisplayErrorMessage()
    {
        // Arrange
        var sku = "A";
        var checkout = CreateCheckout();
        var expectedMessage = "The Sku was not found.";
        _checkoutOrderMock.Setup(co => co.AddItem(It.IsAny<string>())).Returns(false);
        var existingOrder = _checkoutOrderMock.Object;
        _orderFactoryMock.Setup(of => of.CreateNewOrder()).Returns(existingOrder);
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Act
        checkout.Scan(sku);

        // Assert
        var result = consoleOutput.ToString().Trim();
        Assert.Equal(expectedMessage, result);
    }

    [Fact]
    public void Scan_WithNoExistingOrder_AndOrderCannotBeCreated_ShouldDisplayErrorMessage()
    {
        // Arrange
        var sku = "A";
        var checkout = CreateCheckout();
        var expectedMessage = "Apologies, items cannot be scanned at this time.";
        _orderFactoryMock.Setup(of => of.CreateNewOrder()).Throws<Exception>();
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Act
        checkout.Scan(sku);

        // Assert
        var result = consoleOutput.ToString().Trim();
        Assert.Equal(expectedMessage, result);
    }

    [Fact]
    public void GetTotalPrice_WithCheckoutOrder_ShouldDisplayCurrentOrderTotal()
    {
        // Arrange
        var expectedTotal = 125;
        _checkoutOrderMock.Setup(o => o.CalculateOrderTotal()).Returns(expectedTotal);
        _orderFactoryMock.Setup(of => of.CreateNewOrder()).Returns(_checkoutOrderMock.Object);
        var checkout = CreateCheckout();
        checkout.Scan("A"); // Scan at least one item to create the order

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
        var total = checkout.GetTotalPrice();

        // Assert
        var result = sw.ToString().Trim();
        Assert.Equal(0, total);
        Assert.Equal(expectedMessage, result);
    }

    private Checkout CreateCheckout()
    {
        return new Checkout(_orderFactoryMock.Object);
    }
}
