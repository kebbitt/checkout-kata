﻿namespace Bright.Supermarket.App.Domain;

public class CheckoutOrder
{
    public int Id { get; set; }

    public required IList<LineItem> LineItems { get; set; }
    public virtual int CalculateOrderTotal()
    {
        throw new NotImplementedException();
    }
}