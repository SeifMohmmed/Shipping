namespace Shipping.Domain.Enums;
public enum OrderStatus
{
    Pending = 0,
    WaitingForConfirmation = 1,
    InProgress = 2,
    Delivered = 3,
    DeliveredToCourier = 4,
    Declined = 5,
    UnreachableCustomer = 6,
    PartialDelivery = 7,
    CanceledByRecipient = 8,
    DeclinedWithPartialPayment = 9,
    DeclinedWithFullPayment = 10
}

public enum OrderType
{
    Pickup,
    Delivery
}

public enum PaymentType
{
    Collectible,
    Prepaid,
    Expulsion
}