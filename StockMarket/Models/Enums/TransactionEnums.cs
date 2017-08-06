using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockMarket.Models.Enums
{
    public enum TransactionErrorCode
    {
        Ok = 0,
        StockNotFound = 1,
        PriceUnavailable = 2,
        UnsufficientAmount = 3,
        UnsufficientFounds = 4,
        UnknownError = 5
    }

    public enum TransactionTypes
    {
        Purchase = 0,
        Sale = 1
    }
}