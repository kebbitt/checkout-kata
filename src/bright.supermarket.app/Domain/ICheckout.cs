namespace Bright.Supermarket.App.Domain;
public interface ICheckout
{
    void Scan(string item);
    int GetTotalPrice();
}
