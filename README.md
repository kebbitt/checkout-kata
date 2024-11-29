# checkout-kata
In a normal supermarket, products are identified using Stock Keeping Units, or SKUs. In our supermarket, we’ll use individual letters of the alphabet (A, B, C, and so on). Our goods are priced individually. In addition, some items are multipriced: buy _n_ of them, and they’ll cost you _y_. For example, item ‘A’ might cost 50 pounds individually, but this week we have a special offer; buy three ‘A’s and they’ll cost you 130. The current pricing and offers are as follows:

| SKU  | Unit Price | Special Price |
| ---- | ---------- | ------------- |
| A    | 50         | 3 for 130     |
| B    | 30         | 2 for 45      |
| C    | 20         |               |
| D    | 15         |               |

Our checkout scans items individually and accepts items in any order, so that if we scan a B, an A, and another B, we’ll recognize the two Bs qualify for a special offer for a a total price of 95. You can qualify for a special offer multiple times e.g. if you scan 6 As then you will have a total price of 260. Because the pricing changes frequently, we need to be able to pass in a set of pricing rules each time we start handling a checkout transaction.

Here's a suggested interface for the checkout...
```cs
interface ICheckout
{
    void Scan(string item);
    int GetTotalPrice();
}
```

# Instructions
Implement a class or classes that satisfies the problem described above. The solution should include unit tests, and we welcome test first approaches to it.

We're as interested in the process that you go through to develop the code as the end result, so commit early and often so we can see the steps that you go through to arrive at your solution. We want to see a git repository containing your solution, ideally uploaded to your own github account. Use of AI assistance will disqualify the candidate.

# Acknowledgements
Adapted from http://codekata.com/kata/kata09-back-to-the-checkout/

---

# Solution Design

Some candidate notes before attempting the solution. This will be implemented with TDD based on Acceptance & Exception Criteria detailed below

## Assumptions
Some assumptions that have been made from the interpretations of the description & instructions, or decided upon by the developer:
* A checkout order/transaction consists of all successful item scans (from one shopper) and ends with a total price being calculated.
* Unit Price or Special Price could potentially change with each new checkout transaction.
* Multiprice is the only supported type of special price for now i.e. x for y.
* We're not going to have more that 26 products at present, char is fine for SKU id.
* If special price is missing or cannot be supported, we fall back to the unit price.
* If unit price is unavailable, the item cannot be scanned but transaction remains valid.
* A checkout order and checkout transaction is used interchangeably here but checkout order is probably a clearer term. In reality an order may have multiple financial transactions e.g. part card, part cash.
* Lineitem may be a useful term entity to bring in here as can represent a quanitity of the same product SKU
* We'll assume the checkout lives beyond one order as represents a supermarket checkout, i.e. it handles multiple orders during it's lifetime.

## Initial Domain Model
* Checkout - Represents a Supermarket Checkout. It will handle 0-M orders during it's lifetime.
* CheckoutOrder - Represents a single order that is completed by shopper personna. It will contain 1-M lineitems.
* LineItem - Represents a line in the shopper's order which is a quantity of a particular SKU. (Know useful due to ecommerce background). For simplicity, will add all units to a lineitem regardless of offers.That would possibly be easier/clearer to manage in code.
* PricingRules - A list of rules representing unit prices & offers on each available SKU. These are refreshed/pulled from somewhere with each new CheckoutOrder


## Test Cases

These are built around Checkout suggested interface. Not looking for 80% coverage etc, just to cover the usecases sufficiently and solve the problem. Also may not have time to implement all of these, this is simply to demonstrate the process taken to derive.

#### Acceptance Criteria

*Scan of 1st item added to new order*\
Given a checkout order does not exist\
When Scan is called on the item at checkout\
Then a new order is created\
And the item is added to a new lineitem

*Scan of additional item in different SKU*\
Given a checkout order exists\
And a lineitem for a different SKU exists\
When Scan is called on the item at checkout\
Then a new lineitem is created for the additional SKU

*Scan of additional item in same SKU*\
Given a checkout order exists\
And a lineitem for a SKU exists\
When Scan is called on the item at checkout\
Then quantity on existing lineitem is incremented

*Display Basic Total*\
Given a checkout order with 1 item\
When GetTotalPrice is called\
Then the expected price is displayed

*Display correct price on order with multibuy offer*\
Given a checkout order with items qualifying for an offer\
When GetTotalPrice is called\
Then the expected price is displayed

*Display correct price on order with stacked multibuy offers*\
Given a checkout order with items qualifying for an offer\
And mulitbuy is stacked over 2 or more groups
When GetTotalPrice is called\
Then the expected price is displayed

*Display correct price on order with mix of multibuy and individual items of same SKU*\
Given a checkout order with items qualifying for an offer\
And items of same SKU that do not qualify for an offer\
When GetTotalPrice is called\
Then the expected price is displayed

*Display correct price on subsequent order*\
Given a completed order\
And a 2nd customer scans 1 or more items\
When GetTotalPrice is called\
Then 2nd customer only sees price of their items

#### Exception Criteria

*Unscannable item is not added to the checkout order*\
Given a checkout order exists\
When Scan is called on an item at checkout\
And the SKU is not recognised\
Then the scan is unsuccessful
And the issue is logged

*Scanned SKU with no unit price is not added to the checkout order*\
Given a checkout order exists\
When Scan is called on an item at checkout\
And the SKU unit price is missing from pricing rules\
Then the scan is unsuccessful\
And the issue is logged

*Item with unrecognised offer is treated individually*\
Given a checkout order exists\
When Scan is called on the item at checkout\
And the item has an unrecognised special offer
Then the SKU unit price is used instead
And the issue is logged

## Technical Considerations

* .net 9 Console App to address problem
* xUnit project for tests
* Moq for mocking, if required
* It could be useful to identify individual orders, perhaps numbered or guid for each one
* Orders need to be created at runtime as a dependency for checkout. Factory method or abstract factory might be useful here, to facilitate testing.
* Orders have a dependency on pricing rules. A service to get latest rules each time an order is created would be useful. 
* Special Price format seem to be the most likely source of future change, keep in mind during development
* Add Console logs where appropriate, particularly around exception cases
