﻿namespace Bright.Supermarket.App.Domain;
public interface IOrderFactory
{
    public CheckoutOrder CreateNewOrder();
}