namespace Bright.Supermarket.App.Domain;
public interface IOrderFactory
{
    public ICheckoutOrder CreateNewOrder();
}
