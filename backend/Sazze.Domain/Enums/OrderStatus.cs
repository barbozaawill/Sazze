namespace Sazze.Domain.Enums;

public enum OrderStatus
{
    AwaitingPayment,
    PaymentConfirmed,
    InPreparation,
    OutForDelivery,
    Delivered,
    Cancelled,
    Refunded
}
