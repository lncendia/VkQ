﻿namespace VkQ.Domain.Transactions.Exceptions;

public class TransactionAlreadyAcceptedException : Exception
{
    public TransactionAlreadyAcceptedException() : base("Transaction already accepted")
    {
    }
}