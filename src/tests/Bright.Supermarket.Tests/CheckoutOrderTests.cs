using Bright.Supermarket.App.Domain;
using Moq;

namespace Bright.Supermarket.Tests;
public class CheckoutOrderTests
{
    [Fact]
    public void CalculateOrderTotal_WithIndividuallyPricedItems_CalculatesCorrectPrice()
    {
        // Arrange
        var expectedPrice = 35;
        var order = new CheckoutOrder()
        {
            LineItems = new List<LineItem>()
            {
                new LineItem("C")
                {
                    Quantity = 1,
                },
                new LineItem("D")
                {
                    Quantity = 1,
                }, 
            }
        };

        // Act
        var total = order.CalculateOrderTotal();

        // Assert
        Assert.Equal(expectedPrice, total);
    }

    public void CalculateOrderTotal_WithMultibuyOffer_CalculatesCorrectPrice()
    {
        // Arrange
        // Act
        // Assert
    }

    public void CalculateOrderTotal_WithStackedMultibuyOffers_CalculatesCorrectPrice()
    {
        // Arrange
        // Act
        // Assert
    }

    public void CalculatesOrderTotal_WithMixedCheckoutOrder_CalculatesCorrectPrice()
    {
        // Arrange
        // Act
        // Assert
    }

    private CheckoutOrder CreateCheckoutOrder()
    {
        return new CheckoutOrder();
    }
}
